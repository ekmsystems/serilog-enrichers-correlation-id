using System.Web;

namespace Serilog.Accessors
{
    public class IISPipelineHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext => HttpContext.Current;
    }
}
