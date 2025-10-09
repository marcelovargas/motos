using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Repositories;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services;
using MotoApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository and service
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<IMotoService, MotoService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
