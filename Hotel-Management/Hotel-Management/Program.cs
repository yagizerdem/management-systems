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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if(!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
