#if FULLFRAMEWORK
using System.Web;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddHeader(this HttpResponse response, string headerKey, string value)
        {
            response.Headers.Add(headerKey, value);
        }
    }
}
