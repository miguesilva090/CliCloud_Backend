# Initial Setup Guide for New Collaborators

This guide will help you set up the Luma API project on a new computer, whether you're using **Windows** or **macOS**.

## Prerequisites

Before starting, ensure you have the following installed:

- **.NET SDK 9.0** or later
  - Download from: https://dotnet.microsoft.com/download
  - Verify installation: `dotnet --version`
  
- **Docker Desktop**
  - Windows: https://www.docker.com/products/docker-desktop/
  - macOS: https://www.docker.com/products/docker-desktop/
  - Verify installation: `docker --version` and `docker compose version`

- **Git** (if cloning from repository)
  - Verify installation: `git --version`

## Step-by-Step Setup

### 1. Clone or Download the Repository

If using Git:
```bash
git clone <repository-url>
cd luma-client-api
```

Or extract the downloaded ZIP file and navigate to the project directory.

### 2. Start the Database Container

Start the SQL Server database using Docker:

```bash
docker compose -f docker-compose.db.yml up -d
```

Wait for the database to be healthy. You can check the status with:

```bash
docker ps
```

Wait until the health status shows "healthy" (this may take 30-60 seconds).

### 3. Restore NuGet Packages

Restore all NuGet packages for the solution:

```bash
dotnet restore
```

This will download all required dependencies for all projects in the solution.

### 3.5. Install Entity Framework Tools (if needed)

If you need to create migrations, install the Entity Framework Core tools:

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
```

**Note:** If you get an error installing the tool, you may need to update it instead:
```bash
dotnet tool update --global dotnet-ef
```

Verify the installation:
```bash
dotnet ef --version
```

**PATH Configuration (if needed):**

If you get an error like "Run 'dotnet tool restore' to make the 'dotnet-ef' command available", you may need to add the tools directory to your PATH. The installation will tell you the exact path, but typically:

**For macOS with oh-my-zsh (recommended):**
```bash
# Add to your ~/.zshrc file (oh-my-zsh uses this instead of ~/.zprofile)
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
source ~/.zshrc
```

**For macOS with standard zsh (without oh-my-zsh):**
```bash
# Add to your ~/.zprofile to make it permanent
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zprofile
source ~/.zprofile
```

**For Windows:**
The tools directory is usually automatically added to PATH. If not, add `%USERPROFILE%\.dotnet\tools` to your system PATH.

**Note:** On some systems, the PATH may already be configured correctly, and you won't need to do this step.

### 4. Set Up HTTPS Certificate

The application requires an HTTPS certificate for local development. The setup differs by platform:

#### For macOS:

```bash
dotnet dev-certs https --trust
```

This will:
- Generate a development certificate
- Trust it in your macOS keychain
- You may be prompted for your password to install the certificate

#### For Windows:

```bash
dotnet dev-certs https --trust
```

This will:
- Generate a development certificate
- Trust it automatically (Windows handles this differently than macOS)
- You may see a security prompt - click "Yes" to trust the certificate

### 5. Apply Database Migrations

**Note:** The application automatically applies pending migrations on startup, so you typically don't need to run this step manually. However, if you encounter errors about pending model changes, see the troubleshooting section below.

To manually apply migrations (optional):

```bash
dotnet ef database update -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/
```

This will create the `Luma` database and apply all existing migrations.

**Important:** If you see an error about "pending model changes" when running the application, you need to create a new migration first (see troubleshooting section).

### 6. Verify Setup

At this point, you should have:
- ✅ Docker running with SQL Server container healthy
- ✅ NuGet packages restored
- ✅ HTTPS certificate generated and trusted
- ✅ Database created and migrations applied

### 7. Run the Application

Navigate to the WebApi project and run with hot reload:

```bash
cd Luma.API.WebApi
dotnet watch run
```

The application will be available at:
- HTTP: `http://localhost:8084`
- HTTPS: `https://localhost:8094`

## Platform-Specific Notes

### Windows

- Use PowerShell or Command Prompt for commands
- Certificate trust is usually automatic after running `dotnet dev-certs https --trust`
- If you encounter permission issues, run PowerShell/Command Prompt as Administrator

### macOS

- Use Terminal (bash/zsh)
- Certificate trust requires your macOS password
- If certificate issues persist, you may need to clean and regenerate:
  ```bash
  dotnet dev-certs https --clean
  dotnet dev-certs https --trust
  ```

## Troubleshooting

### Issue: "Unable to configure HTTPS endpoint"

**Solution:**
- Run `dotnet dev-certs https --trust` again
- On macOS, you may need to enter your password
- If still failing, clean and regenerate:
  ```bash
  dotnet dev-certs https --clean
  dotnet dev-certs https --trust
  ```

### Issue: Database connection failed

**Solution:**
1. Verify Docker is running: `docker ps`
2. Check if SQL Server container is healthy: `docker ps` (look for "healthy" status)
3. Wait a bit longer - SQL Server can take 30-60 seconds to fully start
4. Check connection string in `appsettings.Development.json`
5. Verify the database name is `Luma` in the connection string

### Issue: Port already in use

**Solution:**
- Check what's using the port:
  - Windows: `netstat -ano | findstr :8084` or `netstat -ano | findstr :8094`
  - macOS: `lsof -i :8084` or `lsof -i :8094`
- Stop the conflicting process or change the port in `launchSettings.json`

### Issue: NuGet packages not restoring

**Solution:**
1. Check your internet connection
2. Clear NuGet cache:
   ```bash
   dotnet nuget locals all --clear
   dotnet restore
   ```
3. Verify .NET SDK version: `dotnet --version` (should be 9.0 or later)

### Issue: Docker container won't start

**Solution:**
1. Check Docker Desktop is running
2. Verify Docker has enough resources allocated (Settings → Resources)
3. Check for port conflicts (port 1433 for SQL Server)
4. Try removing and recreating:
   ```bash
   docker compose -f docker-compose.db.yml down
   docker compose -f docker-compose.db.yml up -d
   ```

### Issue: "The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database"

**Solution:**
This error occurs when your Entity Framework model has changed but no migration has been created yet. The application automatically applies migrations on startup, but it requires all model changes to be captured in migrations first.

1. Create a new migration to capture the pending model changes:
   ```bash
   dotnet ef migrations add <MigrationName> -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/ -o Persistence/Migrations
   ```
   Replace `<MigrationName>` with a descriptive name (e.g., `InitialCreate` or `AddNewEntity`).

2. The migration will be created in `Luma.API.Infrastructure/Persistence/Migrations/`

3. Now you can run the application - migrations will be applied automatically on startup:
   ```bash
   dotnet watch run
   ```

**Note:** The application automatically applies pending migrations on startup, so you don't need to run `dotnet ef database update` manually unless you're working with migrations outside of the application.

### Issue: Migrations fail to apply

**Solution:**
1. Ensure the database container is running and healthy
2. Verify the connection string in `appsettings.Development.json` matches the Docker container settings
3. Check that you're running the migration command with the correct parameters:
   ```bash
   dotnet ef database update -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/
   ```
4. If the database already exists but migrations are out of sync, you may need to drop and recreate:
   ```bash
   # Be careful - this will delete all data!
   dotnet ef database drop -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/
   dotnet ef database update -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/
   ```

## Next Steps

After successful setup:

1. **Verify the API is working**:
   - Open `https://localhost:8094` (or your configured port) in a browser
   - Check the API endpoints
   - Test with the Postman collection in `assets/Luma.postman_collection.json`

2. **Review Project Structure**:
   - `Luma.API.Domain`: Domain entities and business logic
   - `Luma.API.Application`: Application services and business rules
   - `Luma.API.Infrastructure`: Data access, external services, and infrastructure concerns
   - `Luma.API.WebApi`: Controllers, middleware, and API configuration

3. **Read Additional Documentation**:
   - See `readme.md` for project-specific commands and workflows
   - See `DEPLOYMENT.md` for deployment instructions

## Quick Reference Commands

```bash
# Start database only
docker compose -f docker-compose.db.yml up -d

# Restore packages
dotnet restore

# Setup HTTPS certificate
dotnet dev-certs https --trust

# Apply database migrations
dotnet ef database update -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/

# Run locally (from project root)
cd Luma.API.WebApi
dotnet watch run

# View database logs
docker compose -f docker-compose.db.yml logs -f sqlserver

# Stop database
docker compose -f docker-compose.db.yml down

# Create a new migration
dotnet ef migrations add <MigrationName> -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/ -o Persistence/Migrations

# Remove last migration
dotnet ef migrations remove -c ApplicationDbContext -s Luma.API.WebApi/ -p Luma.API.Infrastructure/
```

## Need Help?

If you encounter issues not covered here:
1. Check the `readme.md` for more detailed information about project commands
2. Review the `DEPLOYMENT.md` for deployment-specific documentation
3. Check Docker and .NET logs for error messages
4. Contact the team lead or check project documentation

---

**Last Updated:** Based on project setup requirements
**Compatible with:** .NET 9.0, Docker Desktop, Windows 10/11, macOS 10.15+
