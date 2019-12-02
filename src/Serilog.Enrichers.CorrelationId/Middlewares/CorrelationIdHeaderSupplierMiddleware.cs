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

        public CorrelationIdHeaderSupplierMiddleware(RequestDelegate next, string headerKey = CorrelationIdConstants.CorrelationIdHeaderKay)
        {
            _next = next;
            _headerKey = headerKey;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string correlationId = httpContext.Request.Headers.TryGetValue(_headerKey, out var values)
                ? values.First()
                : Guid.NewGuid().ToString();

            if (!httpContext.Items.ContainsKey(CorrelationIdConstants.CorrelationIdItemName))
            {
                httpContext.Items.Add(CorrelationIdConstants.CorrelationIdItemName, correlationId);
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
        public static IApplicationBuilder UseCorrelationIdHeaderSupplier(this IApplicationBuilder builder, string headerKey = CorrelationIdConstants.CorrelationIdHeaderKay)
        {
            return builder.UseMiddleware<CorrelationIdHeaderSupplierMiddleware>(headerKey);
        }
    }
}
#endif
