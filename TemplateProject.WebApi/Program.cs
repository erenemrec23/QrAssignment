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
using QrAssignment.Presentation;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Angular uygulamasının çalıştığı origin'ler. Prod origin'ini appsettings'ten okumak daha temiz.
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
        // JWT header-based olduğu için AllowCredentials gerekmiyor.
        // Cookie tabanlı bir şeye geçersen: .AllowCredentials() ekle (ve AllowAnyOrigin KULLANMA).
    });
});

// -------------------- Katman servisleri --------------------
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

// -------------------- OpenAPI / Scalar --------------------
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
    // Custom claim isimleri ("TenantId", "sub" vb.) otomatik map'lenmesin;
    // FindFirst("TenantId") ve FindFirst(ClaimTypes.NameIdentifier) tutarlı çalışsın.
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

        // MapInboundClaims=false olduğu için token'daki claim tiplerini net belirt.
        // Token'ı üretirken user id'yi hangi claim ile yazıyorsan onunla eşleşmeli
        // (JwtRegisteredClaimNames.Sub ise "sub", ClaimTypes.NameIdentifier ise o).
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role,

        ClockSkew = TimeSpan.Zero
    };
});

// -------------------- Authorization (global fallback) --------------------
// Aksi ([AllowAnonymous]) belirtilmedikçe TÜM endpoint'ler authenticated olmalı.
// AuthController gibi anonim kalması gerekenlere [AllowAnonymous] koy.
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

// -------------------- Rate limiting --------------------
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

// ==================== MIDDLEWARE PIPELINE ====================
// Sıra kritik: Exception -> (dev docs) -> Routing -> CORS -> Localization
//             -> Authentication -> Authorization -> RateLimiter -> Endpoints

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();   // Prod'da aç

app.UseRouting();

app.UseCors("AllowAngularApp");

var supportedCulturesInfo = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCulturesInfo,
    SupportedUICultures = supportedCulturesInfo
};
localizationOptions.RequestCultureProviders.Clear();
app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

// Log arayüzü hassastır — mümkünse admin yetkisiyle koru (SerilogUi authorization filtresi).
app.UseSerilogUi(options =>
{
    options.WithRoutePrefix("logs");
});

app.MapControllers()
   .RequireRateLimiting("IpBasedRateLimit");

app.Run();