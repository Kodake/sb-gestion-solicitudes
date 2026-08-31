using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SB.GestionSolicitudes.Api.Middlewares;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Application.Validators;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Infrastructure.Authentication;
using SB.GestionSolicitudes.Infrastructure.Notifications;
using SB.GestionSolicitudes.Infrastructure.Persistence;
using SB.GestionSolicitudes.Infrastructure.Persistence.Repositories;
using SB.GestionSolicitudes.Infrastructure.Services;
using SB.GestionSolicitudes.Services;
using SB.GestionSolicitudes.Services.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog Setup
var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt");
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 2. DbContext EF Core (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=SB_GestionSolicitudes;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// 3. MediatR for Domain Events
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(NotificacionDomainEventHandlers).Assembly);
});

// 4. Dependency Injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IEntidadGubernamentalService, EntidadGubernamentalService>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<INotificacionService, NotificacionSender>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// 5. FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// 6. JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") 
    ?? builder.Configuration["JwtSettings:Secret"];

if (string.IsNullOrWhiteSpace(secretKey) || secretKey.StartsWith("REPLACE_WITH_"))
{
    if (!builder.Environment.IsDevelopment())
    {
        throw new InvalidOperationException(
            "CRITICAL SECURITY ERROR: 'JWT_SECRET' environment variable or 'JwtSettings:Secret' configuration is missing or configured with an insecure placeholder value in non-Development environment.");
    }

    Log.Warning("SECURITY NOTICE: Using local development fallback signing key for JWT Bearer. Set JWT_SECRET in production.");
    secretKey = "DevLocalFallbackKeyForTestingAndEvaluationOnly_32BytesMinimum!";
}

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "SB.GestionSolicitudes.Api",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "SB.GestionSolicitudes.Clients",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 7. CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 8. Swagger / OpenAPI Configuration with Bearer Auth
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SB.GestionSolicitudes.Api",
        Version = "v1",
        Description = "API RESTful de Gestión de Solicitudes Internas para la Superintendencia de Bancos (SB)",
        Contact = new OpenApiContact
        {
            Name = "Departamento de TI - Arquitectura SB",
            Email = "arquitectura@sb.gob.do"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT Bearer. Ingrese 'Bearer' seguido de un espacio y su token. Ejemplo: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 9. Auto Seed Database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.SeedAsync(dbContext);
}

// 10. Pipeline Middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SB.GestionSolicitudes.Api v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
