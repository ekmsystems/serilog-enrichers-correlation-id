using System;

namespace Serilog.Enrichers
{
    public class DefaultCorrelationIdGenerator : ICorrelationIdGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid( ).ToString( );
        }
    }
}