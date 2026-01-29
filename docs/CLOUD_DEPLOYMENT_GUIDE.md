# Cloud Deployment Guide - Senado Application

## Application Overview

| Component | Details |
|-----------|---------|
| Backend | .NET 8 Web API |
| Database | MongoDB |
| Users | 10-15 (authenticated) |
| People Records | ~10,000 |
| Location | Argentina, Santa Fe |
| Type | Light application |

---

## Cloud Provider Comparison

### 🏆 Recommended: AWS (Best for Free Tier + Low Cost)

**Region:** `sa-east-1` (São Paulo, Brazil) - ~1,200km from Santa Fe, Argentina

#### Resources Needed

| Resource | Service | Specification | Monthly Cost |
|----------|---------|---------------|--------------|
| Compute | EC2 t3.micro | 2 vCPU, 1GB RAM | **$0** (Free Tier 12 months) / $8.47 after |
| Database | MongoDB Atlas M0 | 512MB, Shared | **$0** (Always Free) |
| Storage | EBS gp3 | 20GB | **$0** (Free Tier) / $1.60 after |
| Load Balancer | ALB | Optional | $16.43 + $0.008/LCU-hour |
| DNS | Route 53 | 1 hosted zone | $0.50 + $0.40/million queries |
| SSL | ACM | Certificate | **$0** (Free) |
| Container Registry | ECR | Docker images | **$0** (500MB free) |

**Total First Year:** ~$6-12/month (mainly DNS)
**Total After Free Tier:** ~$25-35/month

#### Alternative: AWS Lightsail (Simpler)

| Resource | Specification | Monthly Cost |
|----------|---------------|--------------|
| Lightsail Instance | 1GB RAM, 1 vCPU, 40GB SSD | **$5/month** |
| MongoDB Atlas M0 | 512MB Shared | **$0** |
| Static IP | Included | **$0** |
| DNS | Included (limited) | **$0** |

**Total:** ~$5-7/month ✅ **BEST VALUE**

---

### Microsoft Azure

**Region:** `brazilsouth` (São Paulo, Brazil)

| Resource | Service | Specification | Monthly Cost |
|----------|---------|---------------|--------------|
| Compute | App Service B1 | 1.75GB RAM, 1 vCPU | $13.14/month |
| Database | Cosmos DB (MongoDB API) | Serverless | ~$25+/month |
| Alternative DB | MongoDB Atlas M0 | 512MB | **$0** |
| DNS | Azure DNS | 1 zone | $0.50 + queries |

**Total:** ~$15-20/month (with Atlas) / $40+/month (with Cosmos)

**Azure Free Tier:**
- 12 months of B1S VM
- $200 credit first 30 days

---

### Google Cloud Platform (GCP)

**Region:** `southamerica-east1` (São Paulo, Brazil)

| Resource | Service | Specification | Monthly Cost |
|----------|---------|---------------|--------------|
| Compute | Cloud Run | Serverless containers | ~$0-5/month |
| Compute Alt | e2-micro VM | 1GB RAM, 2 vCPU | **$0** (Always Free) |
| Database | MongoDB Atlas M0 | 512MB | **$0** |
| Load Balancer | Cloud Load Balancing | Regional | $18/month |
| DNS | Cloud DNS | 1 zone | $0.20 + queries |

**Total:** ~$5-25/month

**GCP Always Free:**
- 1 e2-micro instance (US regions only 😞)
- Cloud Run: 2M requests/month free

---

### DigitalOcean

**Region:** `nyc1` (New York) - No South America region 😞

| Resource | Service | Specification | Monthly Cost |
|----------|---------|---------------|--------------|
| Compute | Basic Droplet | 1GB RAM, 1 vCPU, 25GB | **$6/month** |
| Database | MongoDB Atlas M0 | 512MB | **$0** |
| Load Balancer | DO Load Balancer | Optional | $12/month |
| DNS | Included | Unlimited | **$0** |

**Total:** ~$6-18/month

**Cons:** Higher latency from Argentina (~150-200ms)

---

### Render.com (PaaS - Simplest)

| Resource | Service | Specification | Monthly Cost |
|----------|---------|---------------|--------------|
| Web Service | Starter | 512MB RAM | **$7/month** |
| Database | MongoDB Atlas M0 | 512MB | **$0** |

**Total:** ~$7/month

**Pros:** Zero DevOps, automatic deployments
**Cons:** US/EU regions only, higher latency

---

### Railway.app (PaaS)

| Resource | Service | Specification | Monthly Cost |
|----------|---------|---------------|--------------|
| Compute | Usage-based | Per execution | ~$5-10/month |
| Database | MongoDB Atlas M0 | 512MB | **$0** |

**Total:** ~$5-10/month

---

## 📊 Final Recommendation

### For Your Use Case (Light App, Argentina Users)

| Priority | Option | Cost/Month | Complexity | Latency |
|----------|--------|------------|------------|---------|
| 1️⃣ | **AWS Lightsail** | $5 | Low | ~30ms |
| 2️⃣ | AWS EC2 Free Tier | $0-6 | Medium | ~30ms |
| 3️⃣ | Azure App Service + Atlas | $13-15 | Low | ~30ms |
| 4️⃣ | DigitalOcean | $6 | Low | ~150ms |

### 🎯 Winner: AWS Lightsail + MongoDB Atlas M0

**Why:**
1. **Closest Region** - São Paulo is ~1,200km from Santa Fe
2. **Lowest Cost** - $5/month flat rate
3. **Simple** - No complex AWS setup
4. **Includes** - Static IP, firewall, snapshots
5. **MongoDB Atlas M0** - Free forever, 512MB is enough for your ~10K records

---

## Architecture Diagram

```
                                    ┌─────────────────────────────────┐
                                    │         MongoDB Atlas           │
                                    │      (M0 Free - São Paulo)      │
                                    │         512MB Storage           │
                                    └──────────────┬──────────────────┘
                                                   │
                                                   │ MongoDB Driver
                                                   │
┌──────────────┐    HTTPS     ┌────────────────────┴───────────────────┐
│   Users in   │◄────────────►│          AWS Lightsail                 │
│  Argentina   │              │    (sa-east-1 - São Paulo)             │
│  Santa Fe    │              │                                        │
└──────────────┘              │  ┌──────────────────────────────────┐  │
                              │  │         Docker Container          │  │
                              │  │                                   │  │
                              │  │  ┌─────────────────────────────┐ │  │
                              │  │  │     .NET 8 Web API          │ │  │
                              │  │  │                             │ │  │
                              │  │  │  • User Authentication      │ │  │
                              │  │  │  • People Management        │ │  │
                              │  │  │  • Solicitations            │ │  │
                              │  │  │  • Budget Management        │ │  │
                              │  │  └─────────────────────────────┘ │  │
                              │  └──────────────────────────────────┘  │
                              │                                        │
                              │  1 vCPU | 1GB RAM | 40GB SSD          │
                              │  Static IP included                    │
                              └────────────────────────────────────────┘
```

---

## MongoDB Atlas M0 Setup

1. Go to [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. Create free account
3. Create **M0 Sandbox** cluster (FREE)
4. Select **AWS** as provider
5. Select **sa-east-1 (São Paulo)** region
6. Create database user
7. Whitelist your Lightsail IP
8. Get connection string

**Connection String Format:**
```
mongodb+srv://<username>:<password>@cluster0.xxxxx.mongodb.net/senado?retryWrites=true&w=majority
```

---

## Cost Summary

### Monthly Costs

| Item | Cost |
|------|------|
| AWS Lightsail 1GB | $5.00 |
| MongoDB Atlas M0 | $0.00 |
| Domain (optional) | ~$1.00/month (annual) |
| **Total** | **~$5-6/month** |

### Annual Costs

| Item | Cost |
|------|------|
| AWS Lightsail | $60.00 |
| Domain (.com.ar or .com) | $10-15 |
| **Total** | **~$70-75/year** |

---

## Scaling Considerations

If your app grows:

| Scenario | Solution | New Cost |
|----------|----------|----------|
| More traffic | Lightsail 2GB | $10/month |
| More data | Atlas M2 (2GB) | $9/month |
| High availability | Add Load Balancer | +$18/month |
| Multiple instances | Lightsail containers | $7/month per node |

---

## Files Included

| File | Description |
|------|-------------|
| `terraform/aws-lightsail/` | Terraform for AWS Lightsail deployment |
| `terraform/aws-ec2/` | Terraform for AWS EC2 (Free Tier) deployment |
| `docker/Dockerfile` | Dockerfile for .NET 8 app |
| `docker/docker-compose.yml` | Local development setup |
| `.github/workflows/deploy.yml` | CI/CD pipeline |

---

## Quick Start

### Option 1: AWS Lightsail (Recommended)

```bash
cd terraform/aws-lightsail
terraform init
terraform plan
terraform apply
```

### Option 2: AWS EC2 Free Tier

```bash
cd terraform/aws-ec2
terraform init
terraform plan
terraform apply
```

---

## Security Checklist

- [ ] Enable HTTPS with Let's Encrypt
- [ ] Configure MongoDB Atlas IP whitelist
- [ ] Use environment variables for secrets
- [ ] Enable AWS CloudWatch logging
- [ ] Set up automatic snapshots
- [ ] Configure firewall rules (ports 80, 443 only)
- [ ] Use strong MongoDB passwords
- [ ] Enable MFA on AWS account
