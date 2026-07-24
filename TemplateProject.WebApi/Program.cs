using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSerilogUi(options =>
{ 
 
    options.UseSqlServer(sqlOpts =>
    {
        sqlOpts.WithConnectionString(connectionString);
        sqlOpts.WithTable("Logs");
    });
});

builder.Services.AddCors(options =>
{
    //options.AddPolicy("AllowAngularApp",
    //    policy =>
    //    {
    //        policy.WithOrigins("http://localhost:4200") // Angular'ın çalıştığı adres
    //              .AllowAnyHeader()
    //              .AllowAnyMethod();
    //    });
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()   // Tüm adreslere izin ver
                   .AllowAnyMethod()   // Tüm metodlara (GET, POST vb.) izin ver
                   .AllowAnyHeader();  // Tüm başlıklara izin ver
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
builder.Services.AddPresentation();
builder.Services.AddControllers()
    .AddApplicationPart(QrAssignment.Presentation.AssemblyReference.Assembly);
    //.AddJsonOptions(opts =>
    //{
    //    opts.JsonSerializerOptions.Converters.Add(new ResultJsonConverterFactory());
    //}); 
 
    
// 1. Varsayılan doğrulama şemasını JWT olarak belirliyoruz
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}) 
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Kontrol edilecek parametreleri aktifleştiriyoruz
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Değerleri appsettings.json'dan okuyoruz
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
        ),
         
        ClockSkew = TimeSpan.Zero
    };
}); 
builder.Services.AddLocalization();

// 2. Kendi yazdığımız JSON Merkez Motorunu Singleton olarak kaydet (Tüm uygulama tek bir RAM üzerinden okusun)
builder.Services.AddSingleton<JsonLocalizationManager>();

// 3. .NET'e "Kendi Localizer'ını değil, bizim yazdığımız JSON Localizer Factory'sini kullan" diyoruz
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

 
var supportedCultures = new[] { "tr-TR", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("tr-TR"); // Hiç dil yollamazsa Türkçe kabul et
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});
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
    // Kurala 429 Too Many Requests durum kodu atıyoruz
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Limit aşıldığında Client'a dönülecek özel JSON (Kendi Result yapına uydurabilirsin)
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        var errorResponse = "{\"success\": false, \"message\": \"Çok fazla istek attınız. Lütfen bir süre bekleyip tekrar deneyin.\"}";
        await context.HttpContext.Response.WriteAsync(errorResponse, cancellationToken: token);
    };

    // "IpBasedRateLimit" adında bir kural seti (Policy) oluşturuyoruz
    options.AddPolicy("IpBasedRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            // Partition Key: İstek atan kişinin IP adresi. Eğer IP bulunamazsa "unknown" olarak grupla.
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,             // Süre dolduğunda limiti otomatik yenile
                PermitLimit = 60,                     // Maksimum İstek Sayısı
                QueueLimit = 0,                       // Sıraya alma (0 ise limiti aşan anında reddedilir)
                Window = TimeSpan.FromMinutes(1)      // Zaman Dilimi (1 Dakika)
            }
        ));
});
 
var app = builder.Build();
app.UseExceptionHandler();
app.UseRouting();
//app.UseCors("AllowAngularApp");
app.UseCors("AllowAll");
// ... (diğer app.Use... middleware tanımları) ...

// --- LOKALİZASYON (ÇOKLU DİL) AYARLARI ---
var supportedCultures1 = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };

var localizationOptions = new RequestLocalizationOptions
{ 
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCultures1,
    SupportedUICultures = supportedCultures1
};
 
localizationOptions.RequestCultureProviders.Clear();

app.UseRequestLocalization(localizationOptions); 
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}
app.MapScalarApiReference();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();  
app.UseSerilogUi(options =>
{
    options.WithRoutePrefix("logs");
});
app.MapControllers()
   .RequireRateLimiting("IpBasedRateLimit");
    //.RequireAuthorization();
app.Run();
