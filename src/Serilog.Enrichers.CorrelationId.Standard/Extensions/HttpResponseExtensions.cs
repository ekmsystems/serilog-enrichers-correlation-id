using Microsoft.AspNetCore.Http;

namespace Serilog.Enrichers.CorrelationId.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddHeader(this HttpResponse response, string headerKey, string value)
        {
            response.Headers.Add(headerKey, value);
        }
    }
}
