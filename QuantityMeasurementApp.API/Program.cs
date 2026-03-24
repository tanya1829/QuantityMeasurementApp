using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApp.API.Middleware;
using QuantityMeasurementApp.API.Settings;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.RepoLayer.Cache;
using QuantityMeasurementApp.RepoLayer.Auth;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Persistence;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// ── JWT Settings ──────────────────────────────────────────────────────
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? new JwtSettings
    {
        SecretKey                = "ThisIsAVeryStrongSecretKeyForQuantityMeasurementApp2024!@#",
        Issuer                   = "QuantityMeasurementApp",
        Audience                 = "QuantityMeasurementAppUsers",
        AccessTokenExpiryMinutes = 60,
        RefreshTokenExpiryDays   = 7
    };
builder.Services.AddSingleton(jwtSettings);

var jwtService = new JwtService(
    jwtSettings.SecretKey,
    jwtSettings.Issuer,
    jwtSettings.Audience,
    jwtSettings.AccessTokenExpiryMinutes,
    jwtSettings.RefreshTokenExpiryDays);
builder.Services.AddSingleton(jwtService);

// ── Redis ─────────────────────────────────────────────────────────────────
var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? "localhost:6379,abortConnect=false";
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConnection));
builder.Services.AddSingleton<RedisService>();

// ── Controllers ───────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger with JWT support ──────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Quantity Measurement API",
        Version     = "v1",
        Description = "REST API for quantity measurement operations with JWT"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token only (without Bearer prefix)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ── SQL Server Database ───────────────────────────────────────────────────
builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── JWT Authentication ────────────────────────────────────────────────────
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtSettings.Issuer,
        ValidAudience            = jwtSettings.Audience,
        IssuerSigningKey         = new SymmetricSecurityKey(
                                       Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew                = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ── Repositories + Services ───────────────────────────────────────────────
builder.Services.AddScoped<IAuthRepository,                AuthRepository>();
builder.Services.AddScoped<IAuthService,                   AuthServiceImpl>();
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementEfRepository>();
builder.Services.AddScoped<IQuantityMeasurementService,    QuantityMeasurementServiceImpl>();

var app = builder.Build();

// ── Run EF Core Migrations automatically ─────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<QuantityMeasurementDbContext>();
    db.Database.Migrate();
}

// ── Middleware pipeline ───────────────────────────────────────────────────
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }