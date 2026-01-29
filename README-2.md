# Senado - Sistema de Gestión de Solicitudes

Sistema de gestión de solicitudes de ayuda para el Senado de Santa Fe, Argentina.

## 🏗️ Arquitectura

- **Backend:** .NET 8 Web API
- **Base de Datos:** MongoDB Atlas (M0 Free Tier)
- **Infraestructura:** AWS Lightsail (São Paulo)
- **CI/CD:** GitHub Actions

## 💰 Costo Mensual

| Recurso | Costo |
|---------|-------|
| AWS Lightsail 1GB | $5.00 |
| MongoDB Atlas M0 | $0.00 |
| **Total** | **~$5/mes** |

## 🚀 Quick Start

### Desarrollo Local

```bash
# Clonar repositorio
git clone https://github.com/your-org/senado.git
cd senado

# Iniciar MongoDB local con Docker
docker-compose -f docker/docker-compose.yml up -d

# Ejecutar aplicación
dotnet run --project src/Senado.Api
```

### Despliegue a Producción

```bash
# 1. Configurar Terraform
cd terraform/aws-lightsail
cp terraform.tfvars.example terraform.tfvars
# Editar terraform.tfvars con tus valores

# 2. Crear infraestructura
terraform init
terraform plan
terraform apply

# 3. Desplegar aplicación
export LIGHTSAIL_IP=<tu-ip>
./scripts/deploy.sh
```

## 📁 Estructura del Proyecto

```
senado/
├── src/
│   ├── Senado.Api/          # Web API
│   ├── Senado.Core/         # Dominio y lógica de negocio
│   └── Senado.Infrastructure/  # Acceso a datos
├── terraform/
│   ├── aws-lightsail/       # Terraform para Lightsail ($5/mes)
│   └── aws-ec2/             # Terraform para EC2 Free Tier
├── docker/
│   ├── Dockerfile           # Imagen de producción
│   └── docker-compose.yml   # Desarrollo local
├── scripts/
│   ├── deploy.sh            # Script de despliegue manual
│   └── setup-ssl.sh         # Configuración de SSL
└── docs/
    └── CLOUD_DEPLOYMENT_GUIDE.md  # Guía completa
```

## 🔧 Configuración

### Variables de Entorno

```bash
# MongoDB
MongoDB__ConnectionString=mongodb+srv://user:pass@cluster.mongodb.net/senado
MongoDB__DatabaseName=senado

# ASP.NET
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
```

### appsettings.json

```json
{
  "MongoDB": {
    "ConnectionString": "",
    "DatabaseName": "senado"
  },
  "Jwt": {
    "Secret": "your-secret-key",
    "Issuer": "senado-api",
    "Audience": "senado-app"
  }
}
```

## 📚 Documentación

- [Guía de Despliegue en la Nube](docs/CLOUD_DEPLOYMENT_GUIDE.md)
- [Configuración de MongoDB Atlas](scripts/mongodb-atlas-setup.md)

## 🔒 Seguridad

- [ ] Configurar HTTPS con Let's Encrypt
- [ ] Restringir IPs en MongoDB Atlas
- [ ] Usar variables de entorno para secretos
- [ ] Habilitar MFA en AWS
- [ ] Configurar backups automáticos

## 📞 Soporte

Para preguntas o problemas, contactar al equipo de desarrollo.
