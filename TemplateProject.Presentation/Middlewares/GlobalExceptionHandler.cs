using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TemplateProject.Application;
using TemplateProject.Application.Interfaces;
using TemplateProject.Domain.Exceptions;
using TemplateProject.Domain.Shared; // Kendi Result ve Error sınıflarımızın olduğu yol
namespace TemplateProject.Presentation.Middlewares
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IAppLocalizer _localizer;
        //private readonly IEmailService _emailService; 

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger,
            IAppLocalizer localizer
            //, IEmailService emailService
            )
        {
            _logger = logger;
            _localizer = localizer;
            //_emailService = emailService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationAppException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var resultValidation = new
                {
                    isSuccess = false,
                    error = new
                    {
                        code = "Validation.Error",
                        message =
                        validationException.Errors == null || !validationException.Errors.Any() ?
                        _localizer["Validations.ValidationErrors"] :
                        string.Join(". ",  validationException.Errors.Select(e => string.Join(". ", e.Value)).ToList())
                    },
                    validationErrors = validationException.Errors
                };

                await httpContext.Response.WriteAsJsonAsync(resultValidation, cancellationToken);
                return true;
            }
            if (exception is BusinessException appException)
            {
                // Kullanıcı hatası olduğu için 400 döneriz (Loglamaya gerek yok, sistem çökmedi)
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                // Senin Handler'dan fırlattığın mesajı doğrudan alırız (veya Localizer'dan çeviririz)
                // Eğer Handler'dan "Errors.BrandNotFound" gibi bir key yollarsan: _localizer[appException.Message] yapabilirsin.
                var businessError = new Error("BusinessRule.Violation", appException.Message);

                await httpContext.Response.WriteAsJsonAsync(Result.Failure(businessError), cancellationToken);
                return true;
            }


            _logger.LogError(exception, "Kritik Hata: {RequestPath}", httpContext.Request.Path);


            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            string errorMessage = _localizer["Errors.Unexpected"];

            var error = new Error("Server.InternalError", errorMessage);

            var result = Result.Failure(error);

            // 6. Response'a Result nesnesini yaz
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}