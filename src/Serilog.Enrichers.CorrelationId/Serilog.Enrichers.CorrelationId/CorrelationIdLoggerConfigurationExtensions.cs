using System;
using Serilog.Configuration;
using Serilog.Enrichers;

namespace Serilog
{
    public static class CorrelationIdLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithCorrelationId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<CorrelationIdEnricher>();
        }
    }
}