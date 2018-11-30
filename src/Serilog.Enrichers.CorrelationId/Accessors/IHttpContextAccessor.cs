
using System.Web;

namespace Serilog.Accessors
{
    public interface IHttpContextAccessor
    {
        HttpContext HttpContext { get; }
    }
}
