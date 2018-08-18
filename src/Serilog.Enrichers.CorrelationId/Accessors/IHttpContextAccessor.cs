using System.Web;

namespace Serilog.Enrichers.CorrelationId.Accessors
{
    public interface IHttpContextAccessor
    {
        HttpContext HttpContext { get; }
    }
}
