using Microsoft.EntityFrameworkCore;
using Redde.Infraestructure.Persistence;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configurar el puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new Exception("JWT_SECRET is missing!");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "DefaultIssuer";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "DefaultAudience";
var jwtExpires = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES"), out var expires) ? expires : 60;

var app = builder.Build();

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
