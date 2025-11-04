using FastEndpoints;
using FastEndpoints.Swagger;
using HelseFlow_Backend.Application.Interfaces;
using HelseFlow_Backend.Application.Services;
using HelseFlow_Backend.Infrastructure.Data;
using HelseFlow_Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add FastEndpoints
builder.Services.AddFastEndpoints();

// Add Swagger for FastEndpoints with custom configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<AuthService>();

// Register Repositories (we will replace the rest soon)
builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IVitalLogRepository, VitalLogRepository>(); // To be added
// builder.Services.AddScoped<IDoctorRepository, DoctorRepository>(); // To be added
// builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>(); // To be added
// builder.Services.AddScoped<IGuidelineRepository, GuidelineRepository>(); // To be added

builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<GuidelineService>();
builder.Services.AddScoped<AiService>();

// Register Data Seeding services
builder.Services.AddScoped<DataFactory>();
builder.Services.AddScoped<DataSeeder>();

// Register HttpClient for OpenWeatherMapService
builder.Services.AddHttpClient<IWeatherService, OpenWeatherMapService>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

// Configure authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure FastEndpoints
app.UseFastEndpoints();

// Configure Swagger UI
app.UseOpenApi(); // Generates swagger.json
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelseFlow API v1");
    c.DocumentTitle = "HelseFlow API";
});

app.Run();
