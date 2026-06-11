using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using QrAssignment.Application;
using QrAssignment.Application.Behaviors;
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
using System.Text;

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
 
 

var app = builder.Build();
app.UseExceptionHandler();
app.UseRouting();
//app.UseCors("AllowAngularApp");
app.UseCors("AllowAll");
app.UseRequestLocalization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}
app.MapScalarApiReference();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogUi(options =>
{
    options.WithRoutePrefix("logs");
});
app.MapControllers();
    //.RequireAuthorization();
app.Run();
