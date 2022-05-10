using Serilog.Enrichers;

namespace Serilog.Tests.Support
{
    public class CustomCorrelationIdGenerator : ICorrelationIdGenerator
    {
        public static string CustomCorrelationIdValue = "CustomCorrelationIdValue";
        
        public string Generate()
        {
            return CustomCorrelationIdValue;
        }
    }
}