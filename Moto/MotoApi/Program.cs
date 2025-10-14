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
builder.Services.AddScoped<IEntregadorRepository, EntregadorRepository>();
builder.Services.AddScoped<IEntregadorService, EntregadorService>();
builder.Services.AddScoped<ILocacaoRepository, LocacaoRepository>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

builder.Services.AddControllers();

// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Moto API", 
        Version = "v1",
        Description = "Web API for managing motorcycles" 
    });
    
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
    // Enable annotations for better documentation
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto API v1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
