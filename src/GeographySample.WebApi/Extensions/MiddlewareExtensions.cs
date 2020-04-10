using GeographySample.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace GeographySample.WebApi
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ApiExceptionHandlingMiddleware>();
    }
}