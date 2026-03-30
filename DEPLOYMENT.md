# Multi-Client Deployment Guide

This guide explains how to deploy the same build to multiple clients, each with their own database and IIS configuration.

## Overview

The application is designed to be **client-agnostic** - one build can be deployed to multiple clients. Client-specific configurations are handled through:

1. **Environment Variables** (recommended for secrets)
2. **appsettings.Production.json** (client-specific file)
3. **IIS web.config** (for ports and environment settings)

## Build Process

### 1. Create the Build

```bash
dotnet publish -c Release -o publish
```

**Important**: The build should NOT include:

- `appsettings.Production.json` (client-specific)
- Client-specific connection strings or secrets
- Client-specific IIS configurations

### 2. Package the Build

Create a deployment package (ZIP file) containing:

- All DLLs and dependencies
- `web.config` (with default ports that can be customized)
- `appsettings.Production.Template.json` (as reference for creating appsettings.Production.json)

## Deployment Process (Per Client)

### Step 1: Deploy the Build

1. Extract the build package to the client's IIS directory
2. Ensure the application pool is configured correctly

### Step 2: Configure Client-Specific Settings

You have **three options** for client-specific configuration (in order of precedence):

#### Option A: Environment Variables (Recommended for Secrets)

Set environment variables in IIS `web.config` or at the system level:

```xml
<environmentVariables>
  <environmentVariable name="ConnectionStrings__DefaultConnection" value="Server=CLIENT_SERVER;Database=CLIENT_DB;..." />
  <environmentVariable name="JWTSettings__Key" value="CLIENT_SPECIFIC_JWT_KEY" />
  <environmentVariable name="MailSettings__Host" value="smtp.clientdomain.com" />
  <!-- Use double underscore (__) for nested properties -->
</environmentVariables>
```

#### Option B: appsettings.Production.json (Recommended for Non-Secrets)

1. Copy `appsettings.Production.Template.json` to `appsettings.Production.json`
2. Fill in client-specific values
3. Place it in the application root directory

**Note**: This file should NOT be in source control or the build package.

#### Option C: Modify web.config

Edit the `web.config` file directly for client-specific settings:

```xml
<environmentVariables>
  <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
  <environmentVariable name="ASPNETCORE_URLS" value="https://+:CLIENT_HTTPS_PORT;http://+:CLIENT_HTTP_PORT" />
  <!-- Add other environment variables as needed -->
</environmentVariables>
```

### Step 3: Configure IIS

#### IIS Site Bindings (External Ports)

1. Open **IIS Manager**
2. Navigate to your site
3. Click **Bindings...**
4. Configure:
   - **HTTP**: Port 8080 (or client-specific port)
   - **HTTPS**: Port 8081 (or client-specific port)

**Important**: IIS bindings (external ports) can be different from `ASPNETCORE_URLS` (internal ports). IIS will forward requests automatically.

#### Example Configuration

**Client A:**

- IIS HTTP Binding: `8084`
- IIS HTTPS Binding: `4443`
- ASPNETCORE_URLS: `https://+:8081;http://+:8080`
- IIS forwards: `8084 → 8080` and `4443 → 8081`

**Client B:**

- IIS HTTP Binding: `8080`
- IIS HTTPS Binding: `8081`
- ASPNETCORE_URLS: `https://+:8081;http://+:8080`
- Direct match, no forwarding needed

### Step 4: Database Configuration

Each client needs:

- Their own SQL Server instance
- Their own database
- Database connection string configured via:
  - Environment variable: `ConnectionStrings__DefaultConnection`
  - OR `appsettings.Production.json`: `ConnectionStrings.DefaultConnection`

### Step 5: Verify Deployment

1. Check application logs: `.\logs\stdout`
2. Test HTTP endpoint: `http://client-domain:HTTP_PORT/api/health`
3. Test HTTPS endpoint: `https://client-domain:HTTPS_PORT/api/health`
4. Verify database connectivity
5. Test authentication (JWT)

## Configuration Priority

ASP.NET Core loads configuration in this order (later overrides earlier):

1. `appsettings.Production.json` (client-specific configuration - excluded from build)
2. Environment Variables (highest priority)
3. Command-line arguments

## Security Best Practices

### ✅ DO:

- Use environment variables for secrets (JWT keys, passwords, API keys)
- Keep `appsettings.Production.json` out of source control (client-specific)
- Use strong, unique keys for each client
- Restrict file permissions on configuration files
- Use different database credentials per client

### ❌ DON'T:

- Commit client-specific `appsettings.Production.json` to source control
- Use the same JWT key across multiple clients
- Hardcode secrets in configuration files
- Share database credentials between clients

## Environment Variable Naming

For nested configuration properties, use double underscore (`__`):

| JSON Path                             | Environment Variable                   |
| ------------------------------------- | -------------------------------------- |
| `ConnectionStrings.DefaultConnection` | `ConnectionStrings__DefaultConnection` |
| `JWTSettings.Key`                     | `JWTSettings__Key`                     |
| `MailSettings.Host`                   | `MailSettings__Host`                   |
| `Cloudinary.ApiKey`                   | `Cloudinary__ApiKey`                   |

## Troubleshooting

### Application won't start

- Check `.\logs\stdout` for errors
- Verify `ASPNETCORE_ENVIRONMENT` is set correctly
- Ensure connection string is valid
- Check IIS application pool is running

### Port conflicts

- Verify IIS bindings don't conflict with other sites
- Check `ASPNETCORE_URLS` ports are available
- Ensure firewall allows the ports

### Database connection fails

- Verify connection string format
- Check SQL Server is accessible
- Verify credentials are correct
- Check network connectivity

### Configuration not loading

- Verify environment variable naming (use `__` for nested)
- Check `appsettings.Production.json` exists and is valid JSON
- Ensure `ASPNETCORE_ENVIRONMENT=Production` is set
- Check file permissions

## Example: Complete Client Setup

### Client: "ClientABC"

1. **Deploy build** to: `C:\inetpub\wwwroot\Luma-api-clientabc\`

2. **Create `appsettings.Production.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=clientabc-sql;Database=Luma_ClientABC;User Id=sa;Password=ClientABC_Password;TrustServerCertificate=true;MultipleActiveResultSets=true;"
  },
  "JWTSettings": {
    "Key": "ClientABC_Specific_JWT_Key_256_Bits_Minimum"
  }
}
```

3. **Update `web.config`**:

```xml
<environmentVariables>
  <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
  <environmentVariable name="ASPNETCORE_URLS" value="https://+:8081;http://+:8080" />
</environmentVariables>
```

4. **Configure IIS**:

   - Site name: `Luma-ClientABC`
   - HTTP binding: `8084`
   - HTTPS binding: `4443`
   - Application pool: `.NET CLR Version: No Managed Code`

5. **Restart IIS** and verify the application is running.

## Summary

- ✅ One build for all clients
- ✅ Client-specific config via environment variables or `appsettings.Production.json`
- ✅ IIS bindings configured per client
- ✅ Database connection per client
- ✅ Secure secrets management
