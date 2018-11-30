using System.Collections.Generic;
using System.Collections.Specialized;

namespace Serilog.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static bool TryGetValue(this NameValueCollection headers, string key, out IEnumerable<string> values)
        {
            values = headers.GetValues(key);

            return values != null;
        }
    }
}
