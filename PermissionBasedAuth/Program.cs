using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PermissionBasedAuth.Context;
using PermissionBasedAuth.Filters;
using PermissionBasedAuth.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectioString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(connectioString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
{
    // Password settings.
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 0;

})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddSingleton<IAuthorizationPolicyProvider,PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

var app = builder.Build();

using var dataSeedingScope = app.Services.CreateScope();
var services = dataSeedingScope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var scopFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using (var scop = scopFactory.CreateScope())
    {
        var roleManager = scop.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scop.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        await DefaultRoles.CrateDefualtRoles(roleManager);
        await DefaultUsers.CrateDefualtUser(userManager, roleManager);
    };

}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, ex.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
