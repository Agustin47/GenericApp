#!/bin/bash
# =============================================================================
# Manual Deployment Script for Senado Application
# Run this script from your local machine to deploy to AWS Lightsail
# =============================================================================

set -e

# Configuration
LIGHTSAIL_IP="${LIGHTSAIL_IP:-YOUR_LIGHTSAIL_IP}"
SSH_KEY="${SSH_KEY:-~/.ssh/LightsailDefaultKey-sa-east-1.pem}"
APP_PATH="/opt/senado"
REMOTE_USER="ubuntu"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}========================================${NC}"
echo -e "${YELLOW}  Senado Application Deployment Script  ${NC}"
echo -e "${YELLOW}========================================${NC}"

# Check if IP is configured
if [ "$LIGHTSAIL_IP" == "YOUR_LIGHTSAIL_IP" ]; then
    echo -e "${RED}Error: Please set LIGHTSAIL_IP environment variable${NC}"
    echo "Example: export LIGHTSAIL_IP=54.123.45.67"
    exit 1
fi

# Check if SSH key exists
if [ ! -f "$SSH_KEY" ]; then
    echo -e "${RED}Error: SSH key not found at $SSH_KEY${NC}"
    echo "Download it from AWS Lightsail console"
    exit 1
fi

echo -e "${GREEN}Step 1: Building application...${NC}"
dotnet publish -c Release -o ./publish

echo -e "${GREEN}Step 2: Stopping remote application...${NC}"
ssh -i "$SSH_KEY" "$REMOTE_USER@$LIGHTSAIL_IP" "sudo systemctl stop senado || true"

echo -e "${GREEN}Step 3: Creating backup...${NC}"
ssh -i "$SSH_KEY" "$REMOTE_USER@$LIGHTSAIL_IP" "
    if [ -d $APP_PATH ]; then
        sudo cp -r $APP_PATH ${APP_PATH}.backup.\$(date +%Y%m%d%H%M%S)
    fi
    sudo mkdir -p $APP_PATH
    sudo chown $REMOTE_USER:$REMOTE_USER $APP_PATH
"

echo -e "${GREEN}Step 4: Uploading new version...${NC}"
scp -i "$SSH_KEY" -r ./publish/* "$REMOTE_USER@$LIGHTSAIL_IP:$APP_PATH/"

echo -e "${GREEN}Step 5: Setting permissions...${NC}"
ssh -i "$SSH_KEY" "$REMOTE_USER@$LIGHTSAIL_IP" "
    chmod +x $APP_PATH/*.dll 2>/dev/null || true
"

echo -e "${GREEN}Step 6: Starting application...${NC}"
ssh -i "$SSH_KEY" "$REMOTE_USER@$LIGHTSAIL_IP" "sudo systemctl start senado"

echo -e "${GREEN}Step 7: Health check...${NC}"
sleep 5
if curl -sf "http://$LIGHTSAIL_IP/health" > /dev/null; then
    echo -e "${GREEN}✓ Application is healthy!${NC}"
else
    echo -e "${YELLOW}⚠ Health check failed, checking logs...${NC}"
    ssh -i "$SSH_KEY" "$REMOTE_USER@$LIGHTSAIL_IP" "sudo journalctl -u senado -n 20"
fi

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  Deployment Complete!                  ${NC}"
echo -e "${GREEN}  URL: http://$LIGHTSAIL_IP            ${NC}"
echo -e "${GREEN}========================================${NC}"
