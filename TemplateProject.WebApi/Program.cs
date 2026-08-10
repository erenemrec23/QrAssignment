using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using QrAssignment.Application;
using QrAssignment.Application.Interfaces;
using QrAssignment.Infrastructure;
using QrAssignment.Infrastructure.Localization;
using QrAssignment.Persistance;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Seeding;
using QrAssignment.Presentation;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using QrAssignment.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? new[] { "http://localhost:4200" };

// -------------------- Serilog UI --------------------
builder.Services.AddSerilogUi(options =>
{
    options.UseSqlServer(sqlOpts =>
    {
        sqlOpts.WithConnectionString(connectionString);
        sqlOpts.WithTable("Logs");
    });
});

// -------------------- CORS (yalnızca Angular) --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Proje API Başlığı";
        document.Info.Version = "v1";
        document.Info.Description = "Sistemdeki tüm backend servisleri için dokümantasyon.";
        return Task.CompletedTask;
    });
});

// -------------------- Presentation / Controllers --------------------
builder.Services.AddPresentation();
builder.Services.AddControllers()
    .AddApplicationPart(QrAssignment.Presentation.AssemblyReference.Assembly);

// -------------------- Authentication (JWT) --------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
        ),

        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role,

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

// -------------------- Localization --------------------
builder.Services.AddLocalization();
builder.Services.AddSingleton<JsonLocalizationManager>();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

var supportedCultures = new[] { "tr-TR", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("tr-TR");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

// -------------------- Serilog host --------------------
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.MSSqlServer(
            connectionString: connectionString,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            }));

builder.Services.AddSingleton<IAppLocalizer, AppLocalizer>();
builder.Services.AddScoped<ILocalizationService, JsonLocalizationManager>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        var errorResponse = "{\"success\": false, \"message\": \"Çok fazla istek attınız. Lütfen bir süre bekleyip tekrar deneyin.\"}";
        await context.HttpContext.Response.WriteAsync(errorResponse, cancellationToken: token);
    };

    options.AddPolicy("IpBasedRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 60,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }
        ));
});

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var sp = scope.ServiceProvider;
//    var db = sp.GetRequiredService<AppDbContext>();
//    await db.Database.MigrateAsync();
//    await new MenuCatalogSeeder(db).SeedAsync();
//}

app.UseExceptionHandler();
app.UseRouting();

app.UseCors("AllowAngularApp");

// Localization ayarları
var supportedCulturesInfo = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCulturesInfo,
    SupportedUICultures = supportedCulturesInfo
};
localizationOptions.RequestCultureProviders.Clear();
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    // 1. API Dokümantasyonu (Scalar / OpenAPI)
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();

    // 2. Serilog Log Arayüzü (/logs) - Canlıda asla açılmaz!
    app.UseSerilogUi(options =>
    {
        options.WithRoutePrefix("logs");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.UseSerilogUi(options =>
{
    options.WithRoutePrefix("logs");
});
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", time = DateTime.UtcNow }))
   .AllowAnonymous();
app.MapControllers()
   .RequireRateLimiting("IpBasedRateLimit")
   .RequireAuthorization();
await DatabaseSeeder.SeedAsync(app.Services);
await app.RunAsync();
