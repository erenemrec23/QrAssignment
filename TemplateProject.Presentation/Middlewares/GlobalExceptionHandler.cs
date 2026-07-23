using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Exceptions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Presentation.Middlewares
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IAppLocalizer _localizer;
        private readonly IDbExceptionTranslator _dbExceptionTranslator;
        public GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IAppLocalizer localizer,
    IDbExceptionTranslator dbExceptionTranslator)
        {
            _logger = logger;
            _localizer = localizer;
            _dbExceptionTranslator = dbExceptionTranslator;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (_dbExceptionTranslator.TryTranslate(exception, out var translated))
            {
                _logger.LogWarning(exception, "Veritabanı kısıt ihlali: {RequestPath}", httpContext.Request.Path);
                exception = translated;
            }
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
                        string.Join(". ", validationException.Errors.Select(e => string.Join(". ", e.Value)).ToList())
                    },
                    validationErrors = validationException.Errors
                };

                await httpContext.Response.WriteAsJsonAsync(resultValidation, cancellationToken);
                return true;
            }

            // DuplicateEntityException, BusinessException'dan türediği için
            // onu BusinessException'dan ÖNCE kontrol etmemiz gerekiyor (daha spesifik tip önce gelmeli)
            if (exception is DuplicateEntityException duplicateEntityException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                httpContext.Response.ContentType = "application/json";

                var errorResult = new
                {
                    isSuccess = false,
                    isFailure = true,
                    error = new
                    {
                        code = "Database.DuplicateKey",
                        message = duplicateEntityException.Message
                    }
                };

                await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken);
                return true;
            }

            if (exception is BusinessException businessException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                var errorResult = new
                {
                    isSuccess = false,
                    isFailure = true,
                    error = new
                    {
                        code = "BusinessRule.Violation",
                        message = businessException.Message
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

            string errorMessage = _localizer["Errors.UnKnownException"];
            var error = new Error("Server.InternalError", errorMessage);
            var result = Result.Failure(error);

            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
            return true;
        }
    }
}