#!/bin/bash
# =============================================================================
# SSL Setup Script using Let's Encrypt
# Run this on the Lightsail instance after deployment
# =============================================================================

set -e

# Configuration
DOMAIN="${1:-}"
EMAIL="${2:-}"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}========================================${NC}"
echo -e "${YELLOW}  SSL Certificate Setup (Let's Encrypt) ${NC}"
echo -e "${YELLOW}========================================${NC}"

# Check arguments
if [ -z "$DOMAIN" ] || [ -z "$EMAIL" ]; then
    echo -e "${RED}Usage: $0 <domain> <email>${NC}"
    echo "Example: $0 senado.gob.ar admin@senado.gob.ar"
    exit 1
fi

# Install certbot if not present
if ! command -v certbot &> /dev/null; then
    echo -e "${GREEN}Installing Certbot...${NC}"
    sudo apt-get update
    sudo apt-get install -y certbot python3-certbot-nginx
fi

# Obtain certificate
echo -e "${GREEN}Obtaining SSL certificate for $DOMAIN...${NC}"
sudo certbot --nginx -d "$DOMAIN" -d "www.$DOMAIN" --non-interactive --agree-tos -m "$EMAIL"

# Test automatic renewal
echo -e "${GREEN}Testing certificate renewal...${NC}"
sudo certbot renew --dry-run

# Setup automatic renewal cron job
echo -e "${GREEN}Setting up automatic renewal...${NC}"
(crontab -l 2>/dev/null; echo "0 12 * * * /usr/bin/certbot renew --quiet") | crontab -

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  SSL Setup Complete!                   ${NC}"
echo -e "${GREEN}  Your site is now available at:        ${NC}"
echo -e "${GREEN}  https://$DOMAIN                       ${NC}"
echo -e "${GREEN}========================================${NC}"
