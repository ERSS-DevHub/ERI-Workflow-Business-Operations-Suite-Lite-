# ERI Workflow Suite - Quick Start Script
# This script automates the initial setup process

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  ERI Workflow Suite - Quick Start" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Check prerequisites
Write-Host "Checking prerequisites..." -ForegroundColor Yellow

# Check .NET SDK
try {
    $dotnetVersion = dotnet --version
    Write-Host "✓ .NET SDK installed: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ .NET SDK not found. Please install .NET 8.0 SDK" -ForegroundColor Red
    Write-Host "  Download from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    exit 1
}

# Check EF Core tools
Write-Host "Checking Entity Framework Core tools..." -ForegroundColor Yellow
$efVersion = dotnet ef --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ EF Core tools installed" -ForegroundColor Green
} else {
    Write-Host "✗ EF Core tools not found. Installing..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ EF Core tools installed successfully" -ForegroundColor Green
    } else {
        Write-Host "✗ Failed to install EF Core tools" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 1: Restore NuGet Packages" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Failed to restore packages" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Packages restored successfully" -ForegroundColor Green

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 2: Build the Project" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Build failed" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Build successful" -ForegroundColor Green

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 3: Database Setup" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

# Check if database already exists
if (Test-Path "eri_workflow.db") {
    Write-Host "⚠ Database already exists: eri_workflow.db" -ForegroundColor Yellow
    $response = Read-Host "Do you want to delete and recreate it? (y/N)"
    if ($response -eq 'y' -or $response -eq 'Y') {
        Remove-Item "eri_workflow.db"
        Write-Host "✓ Old database deleted" -ForegroundColor Green
    } else {
        Write-Host "⊘ Keeping existing database" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "=====================================" -ForegroundColor Cyan
        Write-Host "Step 4: Run the Application" -ForegroundColor Cyan
        Write-Host "=====================================" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "Starting application..." -ForegroundColor Yellow
        Write-Host "The application will open in your default browser" -ForegroundColor Yellow
        Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
        Write-Host ""
        dotnet run
        exit 0
    }
}

# Create initial migration
Write-Host "Creating initial migration..." -ForegroundColor Yellow
dotnet ef migrations add InitialCreate
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Failed to create migration" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Migration created successfully" -ForegroundColor Green

# Apply migration
Write-Host "Applying migration to database..." -ForegroundColor Yellow
dotnet ef database update
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Failed to apply migration" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Database created and seeded successfully" -ForegroundColor Green

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 4: Run the Application" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Display default credentials
Write-Host "Default Login Credentials:" -ForegroundColor Cyan
Write-Host "───────────────────────────" -ForegroundColor Cyan
Write-Host "Admin:   admin@eri.co.za / Admin@123" -ForegroundColor White
Write-Host "Manager: manager@eri.co.za / Manager@123" -ForegroundColor White
Write-Host "Staff:   staff@eri.co.za / Staff@123" -ForegroundColor White
Write-Host ""

Write-Host "Starting application..." -ForegroundColor Yellow
Write-Host "The application will open in your default browser" -ForegroundColor Yellow
Write-Host "URLs:" -ForegroundColor Yellow
Write-Host "  HTTPS: https://localhost:5001" -ForegroundColor White
Write-Host "  HTTP:  http://localhost:5000" -ForegroundColor White
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host ""

# Run the application
dotnet run