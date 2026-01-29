# =============================================================================
# AWS Lightsail Deployment for Senado Application
# Cost: ~$5/month
# Region: sa-east-1 (São Paulo, Brazil) - Closest to Argentina
# =============================================================================

terraform {
  required_version = ">= 1.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  # Optional: Store state in S3 (uncomment for production)
  # backend "s3" {
  #   bucket = "senado-terraform-state"
  #   key    = "lightsail/terraform.tfstate"
  #   region = "sa-east-1"
  # }
}

# =============================================================================
# Provider Configuration
# =============================================================================

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project     = "Senado"
      Environment = var.environment
      ManagedBy   = "Terraform"
    }
  }
}

# =============================================================================
# Variables
# =============================================================================

variable "aws_region" {
  description = "AWS region for deployment"
  type        = string
  default     = "sa-east-1" # São Paulo - closest to Argentina
}

variable "environment" {
  description = "Environment name"
  type        = string
  default     = "production"
}

variable "instance_name" {
  description = "Name for the Lightsail instance"
  type        = string
  default     = "senado-app"
}

variable "blueprint_id" {
  description = "Lightsail blueprint (OS image)"
  type        = string
  default     = "ubuntu_22_04" # Ubuntu 22.04 LTS
}

variable "bundle_id" {
  description = "Lightsail bundle (instance size)"
  type        = string
  default     = "micro_2_0" # 1 vCPU, 1GB RAM, 40GB SSD - $5/month
}

variable "mongodb_connection_string" {
  description = "MongoDB Atlas connection string"
  type        = string
  sensitive   = true
  default     = "" # Set via terraform.tfvars or environment variable
}

variable "app_port" {
  description = "Port where the .NET app runs"
  type        = number
  default     = 5000
}

variable "domain_name" {
  description = "Custom domain name (optional)"
  type        = string
  default     = ""
}

# =============================================================================
# Lightsail Instance
# =============================================================================

resource "aws_lightsail_instance" "senado" {
  name              = var.instance_name
  availability_zone = "${var.aws_region}a"
  blueprint_id      = var.blueprint_id
  bundle_id         = var.bundle_id

  user_data = <<-EOF
    #!/bin/bash
    set -e

    # Update system
    apt-get update && apt-get upgrade -y

    # Install Docker
    curl -fsSL https://get.docker.com -o get-docker.sh
    sh get-docker.sh
    usermod -aG docker ubuntu

    # Install Docker Compose
    curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    chmod +x /usr/local/bin/docker-compose

    # Install .NET 8 Runtime (alternative to Docker)
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    apt-get update && apt-get install -y aspnetcore-runtime-8.0

    # Install Nginx as reverse proxy
    apt-get install -y nginx certbot python3-certbot-nginx

    # Create app directory
    mkdir -p /opt/senado
    chown ubuntu:ubuntu /opt/senado

    # Configure Nginx
    cat > /etc/nginx/sites-available/senado << 'NGINX'
    server {
        listen 80;
        server_name _;

        location / {
            proxy_pass http://localhost:5000;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
            proxy_cache_bypass $http_upgrade;
        }
    }
    NGINX

    ln -sf /etc/nginx/sites-available/senado /etc/nginx/sites-enabled/
    rm -f /etc/nginx/sites-enabled/default
    systemctl restart nginx
    systemctl enable nginx

    # Create systemd service for the app
    cat > /etc/systemd/system/senado.service << 'SERVICE'
    [Unit]
    Description=Senado .NET Application
    After=network.target

    [Service]
    WorkingDirectory=/opt/senado
    ExecStart=/usr/bin/dotnet /opt/senado/Senado.dll
    Restart=always
    RestartSec=10
    SyslogIdentifier=senado
    User=ubuntu
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=ASPNETCORE_URLS=http://localhost:5000

    [Install]
    WantedBy=multi-user.target
    SERVICE

    systemctl daemon-reload

    echo "Setup completed successfully!"
  EOF

  tags = {
    Name = var.instance_name
  }
}

# =============================================================================
# Static IP
# =============================================================================

resource "aws_lightsail_static_ip" "senado" {
  name = "${var.instance_name}-static-ip"
}

resource "aws_lightsail_static_ip_attachment" "senado" {
  static_ip_name = aws_lightsail_static_ip.senado.name
  instance_name  = aws_lightsail_instance.senado.name
}

# =============================================================================
# Firewall Rules
# =============================================================================

resource "aws_lightsail_instance_public_ports" "senado" {
  instance_name = aws_lightsail_instance.senado.name

  port_info {
    protocol  = "tcp"
    from_port = 22
    to_port   = 22
    cidrs     = ["0.0.0.0/0"] # Restrict this to your IP in production
  }

  port_info {
    protocol  = "tcp"
    from_port = 80
    to_port   = 80
    cidrs     = ["0.0.0.0/0"]
  }

  port_info {
    protocol  = "tcp"
    from_port = 443
    to_port   = 443
    cidrs     = ["0.0.0.0/0"]
  }
}

# =============================================================================
# Automatic Snapshots (Backups)
# =============================================================================

resource "aws_lightsail_instance_automatic_snapshots" "senado" {
  instance_name = aws_lightsail_instance.senado.name
  enabled       = true
  
  # Snapshot at 3 AM UTC (midnight Argentina time)
  snapshot_time = "03:00"
}

# =============================================================================
# DNS Zone (Optional - if using custom domain)
# =============================================================================

resource "aws_lightsail_domain" "senado" {
  count       = var.domain_name != "" ? 1 : 0
  domain_name = var.domain_name
}

resource "aws_lightsail_domain_entry" "root" {
  count       = var.domain_name != "" ? 1 : 0
  domain_name = aws_lightsail_domain.senado[0].domain_name
  name        = ""
  type        = "A"
  target      = aws_lightsail_static_ip.senado.ip_address
}

resource "aws_lightsail_domain_entry" "www" {
  count       = var.domain_name != "" ? 1 : 0
  domain_name = aws_lightsail_domain.senado[0].domain_name
  name        = "www"
  type        = "A"
  target      = aws_lightsail_static_ip.senado.ip_address
}

# =============================================================================
# Outputs
# =============================================================================

output "instance_name" {
  description = "Name of the Lightsail instance"
  value       = aws_lightsail_instance.senado.name
}

output "instance_ip" {
  description = "Static IP address of the instance"
  value       = aws_lightsail_static_ip.senado.ip_address
}

output "instance_url" {
  description = "URL to access the application"
  value       = "http://${aws_lightsail_static_ip.senado.ip_address}"
}

output "ssh_command" {
  description = "SSH command to connect to the instance"
  value       = "ssh -i ~/.ssh/LightsailDefaultKey-${var.aws_region}.pem ubuntu@${aws_lightsail_static_ip.senado.ip_address}"
}

output "monthly_cost" {
  description = "Estimated monthly cost"
  value       = "$5.00 USD"
}

output "deployment_instructions" {
  description = "Next steps for deployment"
  value       = <<-EOT
    
    ============================================
    DEPLOYMENT INSTRUCTIONS
    ============================================
    
    1. Download SSH key from Lightsail console:
       AWS Console > Lightsail > Account > SSH Keys
    
    2. Connect to the instance:
       ${aws_lightsail_instance.senado.name}
    
    3. Upload your application:
       scp -i ~/.ssh/LightsailDefaultKey-${var.aws_region}.pem -r ./publish/* ubuntu@${aws_lightsail_static_ip.senado.ip_address}:/opt/senado/
    
    4. Configure MongoDB connection:
       Edit /opt/senado/appsettings.Production.json
    
    5. Start the application:
       sudo systemctl start senado
       sudo systemctl enable senado
    
    6. (Optional) Setup SSL with Let's Encrypt:
       sudo certbot --nginx -d yourdomain.com
    
    ============================================
  EOT
}
