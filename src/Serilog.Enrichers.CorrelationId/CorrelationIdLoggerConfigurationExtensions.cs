using System;
using Serilog.Configuration;
using Serilog.Enrichers;

namespace Serilog
{
    public static class CorrelationIdLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithCorrelationId(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            ICorrelationIdGenerator correlationIdGenerator = null)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            correlationIdGenerator = correlationIdGenerator ?? new DefaultCorrelationIdGenerator( );
            return enrichmentConfiguration.With(new CorrelationIdEnricher(correlationIdGenerator));
        }

        public static LoggerConfiguration WithCorrelationIdHeader(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string headerKey = "x-correlation-id",
            ICorrelationIdGenerator correlationIdGenerator = null)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            correlationIdGenerator = correlationIdGenerator ?? new DefaultCorrelationIdGenerator( );
            return enrichmentConfiguration.With(new CorrelationIdHeaderEnricher(headerKey, correlationIdGenerator));
        }
    }
}
