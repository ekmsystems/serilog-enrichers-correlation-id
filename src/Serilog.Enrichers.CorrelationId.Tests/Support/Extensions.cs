using Serilog.Events;

namespace Serilog.Tests.Support
{
    internal static class Extensions
    {
        public static object LiteralValue(this LogEventPropertyValue @this)
        {
            return ((ScalarValue) @this).Value;
        }
    }
}
