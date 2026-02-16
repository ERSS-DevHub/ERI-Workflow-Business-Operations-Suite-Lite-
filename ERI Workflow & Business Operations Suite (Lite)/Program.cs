using ERI_Workflow___Business_Operations_Suite__Lite_.Data;
using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using ERI_Workflow___Business_Operations_Suite__Lite_.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews();

// Database Configuration (SQLite - Local Storage)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Register Application Services
builder.Services.AddScoped<IWorkflowService, WorkflowService>();

// Optional Session Support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


// ----------------------
// DATABASE MIGRATION + SEEDING
// ----------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Automatically create database + apply migrations
    dbContext.Database.Migrate();

    await SeedDataAsync(services);
}


// ----------------------
// HTTP PIPELINE
// ----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


// ----------------------
// SEEDING METHOD
// ----------------------
async Task SeedDataAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "Manager", "Staff" };

    // Create roles if they don't exist
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // ----------------------
    // ADMIN
    // ----------------------
    var adminEmail = "admin@eri.co.za";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Administrator",
            Department = "IT",
            RoleType = RoleType.Admin,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // ----------------------
    // MANAGER
    // ----------------------
    var managerEmail = "manager@eri.co.za";
    var managerUser = await userManager.FindByEmailAsync(managerEmail);

    if (managerUser == null)
    {
        managerUser = new ApplicationUser
        {
            UserName = managerEmail,
            Email = managerEmail,
            FullName = "Department Manager",
            Department = "Operations",
            RoleType = RoleType.Manager,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(managerUser, "Manager@123");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(managerUser, "Manager");
    }

    // ----------------------
    // STAFF
    // ----------------------
    var staffEmail = "staff@eri.co.za";
    var staffUser = await userManager.FindByEmailAsync(staffEmail);

    if (staffUser == null)
    {
        staffUser = new ApplicationUser
        {
            UserName = staffEmail,
            Email = staffEmail,
            FullName = "Staff Member",
            Department = "General",
            RoleType = RoleType.Staff,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(staffUser, "Staff@123");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(staffUser, "Staff");
    }
}
