using FastEndpoints;
using FastEndpoints.Swagger;
using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Application.Services;
using HelseFlow_Backend.Infrastructure.Data;
using HelseFlow_Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection; // Added this using directive

var builder = WebApplication.CreateBuilder(args);

// Add FastEndpoints
builder.Services.AddFastEndpoints();

// Add Swagger for FastEndpoints
builder.Services.AddFastEndpoints()
    .SwaggerDocument();

// Configure JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured."));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Register dependencies
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddSingleton<IVitalLogRepository, InMemoryVitalLogRepository>();
builder.Services.AddSingleton<IDoctorRepository, InMemoryDoctorRepository>();
builder.Services.AddSingleton<IAppointmentRepository, InMemoryAppointmentRepository>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<AdminService>();

builder.Services.AddSingleton<IGuidelineRepository, InMemoryGuidelineRepository>();
builder.Services.AddScoped<GuidelineService>();

builder.Services.AddScoped<AiService>();

// Register HttpClient for OpenWeatherMapService
builder.Services.AddHttpClient<IWeatherService, OpenWeatherMapService>();

var app = builder.Build();

// Configure FastEndpoints
app.UseFastEndpoints();

// Configure Swagger UI
app.UseSwaggerGen();

// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();
