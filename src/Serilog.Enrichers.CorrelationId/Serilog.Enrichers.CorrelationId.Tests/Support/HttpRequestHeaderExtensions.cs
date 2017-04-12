using System.Collections;
using System.Reflection;
using System.Web;

namespace Serilog.Tests.Support
{
    internal static class HttpRequestHeaderExtensions
    {
        public static void AddHeader(this HttpRequest request, string name, string value)
        {
            var headers = request.Headers;
            var hdr = headers.GetType();
            var ro = hdr.GetProperty("IsReadOnly",
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.IgnoreCase |
                BindingFlags.FlattenHierarchy);
            ro.SetValue(headers, false, null);
            hdr.InvokeMember("InvalidateCachedArrays",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                headers,
                null);
            hdr.InvokeMember("BaseAdd",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                headers,
                new object[] {name, new ArrayList {value}});
            ro.SetValue(headers, true, null);
        }
    }
}