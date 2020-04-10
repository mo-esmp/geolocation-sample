using GeographySample.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeographySample.WebApi.Middlewares
{
    public class ApiExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ApiExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ApiExceptionHandlingMiddleware> logger,
            IWebHostEnvironment hostEnvironment
        )
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string result;

            if (ex is DomainException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var problemDetail = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = context.Request.Path,
                    Detail = ex.Message
                };
                result = JsonSerializer.Serialize(problemDetail);
            }
            else
            {
                _logger.LogError(ex, $"Unhandled exception has occurred. {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(new { error = _hostEnvironment.IsDevelopment() ? ex.Message : "Internal Server Error!" });
            }

            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);
        }
    }
}