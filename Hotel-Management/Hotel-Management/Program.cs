using DAL.Context;
using Entity.Identity;
using Hotel_Management;
using IOC.Container;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var conString = builder.Configuration.GetConnectionString("HotelManagementDatabase") ??
 throw new InvalidOperationException("Connection string 'HotelManagementDatabase'" +
" not found.");

builder.Services.AddDbContextPool<HotelManagementContext>(options =>
    options.UseSqlServer(conString));

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<HotelManagementContext>()
    .AddSignInManager()
    .AddUserManager<UserManager<AppUser>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>();



builder.Services.AddAuthorization();

IConfigurationSection jwtSection = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(jwtOptions =>
{
    // override defulat auth scheme to jwt bearer scheme do not delete this jwtOptions settings
    jwtOptions.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    jwtOptions.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;

    jwtOptions.DefaultForbidScheme =
        JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
{
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
    };

    jwtOptions.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogDebug(
                "JWT message received. Path: {Path}, Authorization header exists: {HasAuthHeader}",
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Headers.ContainsKey("Authorization")
            );

            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            var userId = context.Principal?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            var email = context.Principal?
                .FindFirst(ClaimTypes.Email)?
                .Value;

            var roles = context.Principal?
                .FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToArray();

            logger.LogDebug(
                "JWT validated successfully. UserId: {UserId}, Email: {Email}, Roles: {Roles}",
                userId,
                email,
                roles
            );

            foreach (var claim in context.Principal?.Claims ?? [])
            {
                logger.LogDebug(
                    "JWT claim. Type: {ClaimType}, Value: {ClaimValue}",
                    claim.Type,
                    claim.Value
                );
            }

            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogWarning(
                context.Exception,
                "JWT authentication failed. Path: {Path}, ExceptionType: {ExceptionType}, Message: {Message}",
                context.HttpContext.Request.Path,
                context.Exception.GetType().Name,
                context.Exception.Message
            );

            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogDebug(
                "JWT challenge triggered. Path: {Path}, Error: {Error}, Description: {Description}",
                context.HttpContext.Request.Path,
                context.Error,
                context.ErrorDescription
            );

            return Task.CompletedTask;
        },

        OnForbidden = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogWarning(
                "JWT authorization forbidden. Path: {Path}, User: {User}",
                context.HttpContext.Request.Path,
                context.Principal?.Identity?.Name
            );

            return Task.CompletedTask;
        }
    };

});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
    "çÇğĞıİöÖşŞüÜ" +
    "0123456789" +
    "-._@+!";
    options.User.RequireUniqueEmail = true;
});

ServiceIOC.ServiceConfigure(builder.Services);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// seed roles

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
}

using (var scope = app.Services.CreateScope())
{
    await RoomSeeder.SeedRoomAsync(scope.ServiceProvider);
}

using (var scope = app.Services.CreateScope())
{
    await AdminSeeder.SeedAdminAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Use the global exception handler
app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
