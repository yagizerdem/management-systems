using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


if (builder.Environment.IsDevelopment())
{
    var conString = builder.Configuration.GetConnectionString("HotelManagementDatabase") ??
     throw new InvalidOperationException("Connection string 'HotelManagementDatabase'" +
    " not found.");

    builder.Services.AddDbContextPool<HotelManagementContext>(options =>
        options.UseSqlServer(conString));
}
else
{
    var conString = builder.Configuration.GetConnectionString("HotelManagementDatabase") ??
 throw new InvalidOperationException("Connection string 'HotelManagementDatabase'" +
" not found.");

    builder.Services.AddDbContextPool<HotelManagementContext>(options =>
        options.UseSqlServer(conString));
}


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
