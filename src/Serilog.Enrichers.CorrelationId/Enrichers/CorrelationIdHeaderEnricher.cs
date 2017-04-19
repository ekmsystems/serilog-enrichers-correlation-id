using System;
using System.Web;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers
{
    public class CorrelationIdHeaderEnricher : ILogEventEnricher
    {
        private const string CorrelationIdPropertyName = "CorrelationId";
        private readonly string _headerKey;

        public CorrelationIdHeaderEnricher(string headerKey)
        {
            _headerKey = headerKey;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (HttpContext.Current == null)
                return;

            var correlationId = GetCorrelationId();
            var correlationIdProperty = new LogEventProperty(CorrelationIdPropertyName, new ScalarValue(correlationId));

            logEvent.AddPropertyIfAbsent(correlationIdProperty);
        }

        private string GetCorrelationId()
        {
            var header = HttpContext.Current.Request.Headers[_headerKey];
            var correlationId = string.IsNullOrEmpty(header)
                ? Guid.NewGuid().ToString()
                : header;

            HttpContext.Current.Response.AddHeader(_headerKey, correlationId);

            return correlationId;
        }
    }
}