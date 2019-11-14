#if NETCORE
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Serilog.Enrichers.CorrelationId.Middlewares
{
    public class CorrelationIdHeaderSupplierMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _headerKey;

        public CorrelationIdHeaderSupplierMiddleware(RequestDelegate next, string headerKey = "x-correlation-id")
        {
            _next = next;
            _headerKey = headerKey;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string correlationId = Guid.NewGuid().ToString();
            if (httpContext.Request.Headers.TryGetValue(_headerKey, out var values))
            {
                correlationId = values.First();
            }
            if (!httpContext.Response.Headers.ContainsKey(_headerKey))
            {
                httpContext.Response.Headers.Add(_headerKey, correlationId);
            }
            return _next(httpContext);
        }
    }

    public static class CorrelationIdHeaderSupplierMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationIdHeaderSupplier(this IApplicationBuilder builder, string headerKey = "x-correlation-id")
        {
            return builder.UseMiddleware<CorrelationIdHeaderSupplierMiddleware>(headerKey);
        }
    }
}
#endif
