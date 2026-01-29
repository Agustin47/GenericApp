# MongoDB Atlas M0 (Free Tier) Setup Guide

## Step 1: Create MongoDB Atlas Account

1. Go to [MongoDB Atlas](https://www.mongodb.com/cloud/atlas/register)
2. Sign up with email or Google/GitHub
3. Verify your email

## Step 2: Create Organization & Project

1. Create an organization (e.g., "Senado")
2. Create a project (e.g., "Senado-Production")

## Step 3: Create Free Cluster (M0)

1. Click **"Build a Database"**
2. Select **"M0 FREE"** tier
3. Choose provider: **AWS**
4. Choose region: **sa-east-1 (São Paulo)** ← Important for Argentina!
5. Cluster name: `senado-cluster`
6. Click **"Create"**

## Step 4: Create Database User

1. Go to **Database Access** in left menu
2. Click **"Add New Database User"**
3. Authentication Method: **Password**
4. Username: `senado-app`
5. Password: Generate a strong password (save it!)
6. Database User Privileges: **Read and write to any database**
7. Click **"Add User"**

## Step 5: Configure Network Access

1. Go to **Network Access** in left menu
2. Click **"Add IP Address"**
3. For development: **"Allow Access from Anywhere"** (0.0.0.0/0)
4. For production: Add your **Lightsail Static IP** only
5. Click **"Confirm"**

## Step 6: Get Connection String

1. Go to **Database** in left menu
2. Click **"Connect"** on your cluster
3. Choose **"Connect your application"**
4. Driver: **.NET/C#** - Version **2.19 or later**
5. Copy the connection string:

```
mongodb+srv://senado-app:<password>@senado-cluster.xxxxx.mongodb.net/?retryWrites=true&w=majority
```

6. Replace `<password>` with your actual password
7. Add database name before the `?`:

```
mongodb+srv://senado-app:YOUR_PASSWORD@senado-cluster.xxxxx.mongodb.net/senado?retryWrites=true&w=majority
```

## Step 7: Configure Your Application

### appsettings.Production.json

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://senado-app:PASSWORD@senado-cluster.xxxxx.mongodb.net/senado?retryWrites=true&w=majority",
    "DatabaseName": "senado"
  }
}
```

### Using Environment Variables (Recommended)

```bash
export MongoDB__ConnectionString="mongodb+srv://senado-app:PASSWORD@cluster.mongodb.net/senado"
export MongoDB__DatabaseName="senado"
```

## M0 Free Tier Limits

| Feature | Limit |
|---------|-------|
| Storage | 512 MB |
| RAM | Shared |
| Connections | 500 max |
| Operations | Unlimited |
| Backup | None (manual only) |

## Is 512MB Enough?

For your use case:
- ~10,000 people records
- ~15 users
- Solicitations history

**Estimated storage:**
- People: ~10,000 × 1KB = ~10MB
- Users: ~15 × 0.5KB = ~0.01MB
- Solicitations: ~50,000 × 2KB = ~100MB
- **Total: ~150-200MB** ✅ You have plenty of room!

## Upgrading Later

If you need more storage:

| Tier | Storage | RAM | Cost/Month |
|------|---------|-----|------------|
| M0 | 512 MB | Shared | **FREE** |
| M2 | 2 GB | Shared | $9 |
| M5 | 5 GB | Shared | $25 |
| M10 | 10 GB | 2 GB | $57 |

## Monitoring

1. Go to your cluster
2. Click **"Metrics"** tab
3. Monitor:
   - Connections
   - Operations/sec
   - Storage used
   - CPU/Memory (M10+)
