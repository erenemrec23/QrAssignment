using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Exceptions;
using QrAssignment.Domain.Shared;
using System.Data.Entity;
namespace QrAssignment.Presentation.Middlewares
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
            if (exception is BusinessException businessException)
            {
                // Kullanıcı hatası olduğu için 400 döneriz (Loglamaya gerek yok, sistem çökmedi)
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                //// Senin Handler'dan fırlattığın mesajı doğrudan alırız (veya Localizer'dan çeviririz)
                //// Eğer Handler'dan "Errors.BrandNotFound" gibi bir key yollarsan: _localizer[appException.Message] yapabilirsin.
                //var businessError = new Error("BusinessRule.Violation", appException.Message);

                //await httpContext.Response.WriteAsJsonAsync(Result.Failure(businessError), cancellationToken);

                var errorResult = new
                {
                    isSuccess = false,
                    isFailure = true,
                    error = new
                    {
                        code = "BusinessRule.Violation",
                        message = businessException.Message // "Bu mail adresi zaten kayıtlı" mesajı buraya gelir
                    }
                };

                await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken);
                return true;
            }
            if (exception is UnauthorizedAccessException unauthorizedAccessException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                httpContext.Response.ContentType = "application/json";

                var errorResult = new
                {
                    isSuccess = false,
                    isFailure = true,
                    error = new
                    {
                        code = "Authorization.Forbidden",
                        message = unauthorizedAccessException.Message
                    }
                };

                await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken);
                return true;
            }

            _logger.LogError(exception, "Kritik Hata: {RequestPath}", httpContext.Request.Path);


            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            string errorMessage = _localizer["Errors.Unauthorized"];

            var error = new Error("Server.InternalError", errorMessage);

            var result = Result.Failure(error);

            // 6. Response'a Result nesnesini yaz
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}