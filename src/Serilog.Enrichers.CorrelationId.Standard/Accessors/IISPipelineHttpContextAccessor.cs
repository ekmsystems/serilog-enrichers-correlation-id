using System.Web;

namespace Serilog.Enrichers.CorrelationId.Accessors
{
    public class IISPipelineHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext => HttpContext.Current;
    }
}
