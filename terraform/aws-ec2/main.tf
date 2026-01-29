# =============================================================================
# AWS EC2 Free Tier Deployment for Senado Application
# Cost: $0 for first 12 months (Free Tier), ~$10/month after
# Region: sa-east-1 (São Paulo, Brazil)
# =============================================================================

terraform {
  required_version = ">= 1.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
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
  default     = "sa-east-1"
}

variable "environment" {
  description = "Environment name"
  type        = string
  default     = "production"
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
  default     = "t3.micro" # Free Tier eligible
}

variable "key_pair_name" {
  description = "Name of the SSH key pair"
  type        = string
  default     = "senado-key"
}

variable "mongodb_connection_string" {
  description = "MongoDB Atlas connection string"
  type        = string
  sensitive   = true
  default     = ""
}

variable "allowed_ssh_cidr" {
  description = "CIDR block allowed for SSH access"
  type        = string
  default     = "0.0.0.0/0" # Restrict this in production
}

# =============================================================================
# Data Sources
# =============================================================================

# Get latest Ubuntu 22.04 AMI
data "aws_ami" "ubuntu" {
  most_recent = true
  owners      = ["099720109477"] # Canonical

  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd/ubuntu-jammy-22.04-amd64-server-*"]
  }

  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }
}

# Get default VPC
data "aws_vpc" "default" {
  default = true
}

# Get default subnets
data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# =============================================================================
# Security Group
# =============================================================================

resource "aws_security_group" "senado" {
  name        = "senado-sg"
  description = "Security group for Senado application"
  vpc_id      = data.aws_vpc.default.id

  # SSH
  ingress {
    description = "SSH"
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = [var.allowed_ssh_cidr]
  }

  # HTTP
  ingress {
    description = "HTTP"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # HTTPS
  ingress {
    description = "HTTPS"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Allow all outbound
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "senado-sg"
  }
}

# =============================================================================
# Key Pair
# =============================================================================

resource "tls_private_key" "senado" {
  algorithm = "RSA"
  rsa_bits  = 4096
}

resource "aws_key_pair" "senado" {
  key_name   = var.key_pair_name
  public_key = tls_private_key.senado.public_key_openssh
}

resource "local_file" "private_key" {
  content         = tls_private_key.senado.private_key_pem
  filename        = "${path.module}/senado-key.pem"
  file_permission = "0400"
}

# =============================================================================
# EC2 Instance
# =============================================================================

resource "aws_instance" "senado" {
  ami                    = data.aws_ami.ubuntu.id
  instance_type          = var.instance_type
  key_name               = aws_key_pair.senado.key_name
  vpc_security_group_ids = [aws_security_group.senado.id]
  subnet_id              = data.aws_subnets.default.ids[0]

  root_block_device {
    volume_size           = 20 # GB - Free Tier allows up to 30GB
    volume_type           = "gp3"
    encrypted             = true
    delete_on_termination = true
  }

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

    # Install .NET 8 Runtime
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    apt-get update && apt-get install -y aspnetcore-runtime-8.0

    # Install Nginx
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

    # Create systemd service
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

    echo "Setup completed!"
  EOF

  tags = {
    Name = "senado-app"
  }

  lifecycle {
    create_before_destroy = true
  }
}

# =============================================================================
# Elastic IP
# =============================================================================

resource "aws_eip" "senado" {
  instance = aws_instance.senado.id
  domain   = "vpc"

  tags = {
    Name = "senado-eip"
  }
}

# =============================================================================
# CloudWatch Alarms (Optional - Free Tier includes basic monitoring)
# =============================================================================

resource "aws_cloudwatch_metric_alarm" "cpu_high" {
  alarm_name          = "senado-cpu-high"
  comparison_operator = "GreaterThanThreshold"
  evaluation_periods  = 2
  metric_name         = "CPUUtilization"
  namespace           = "AWS/EC2"
  period              = 300
  statistic           = "Average"
  threshold           = 80
  alarm_description   = "This metric monitors EC2 CPU utilization"

  dimensions = {
    InstanceId = aws_instance.senado.id
  }
}

# =============================================================================
# Outputs
# =============================================================================

output "instance_id" {
  description = "EC2 instance ID"
  value       = aws_instance.senado.id
}

output "public_ip" {
  description = "Elastic IP address"
  value       = aws_eip.senado.public_ip
}

output "instance_url" {
  description = "URL to access the application"
  value       = "http://${aws_eip.senado.public_ip}"
}

output "ssh_command" {
  description = "SSH command to connect"
  value       = "ssh -i ${path.module}/senado-key.pem ubuntu@${aws_eip.senado.public_ip}"
}

output "private_key_path" {
  description = "Path to the private SSH key"
  value       = local_file.private_key.filename
}

output "monthly_cost_estimate" {
  description = "Estimated monthly cost"
  value       = "Free Tier: $0/month (first 12 months), After: ~$8-10/month"
}

output "free_tier_details" {
  description = "AWS Free Tier included"
  value       = <<-EOT
    
    ============================================
    AWS FREE TIER INCLUDES (12 months):
    ============================================
    - 750 hours/month of t3.micro (enough for 1 instance 24/7)
    - 30 GB of EBS storage
    - 15 GB of bandwidth out
    - 1 million requests to Lambda
    - 5 GB of S3 storage
    ============================================
  EOT
}
