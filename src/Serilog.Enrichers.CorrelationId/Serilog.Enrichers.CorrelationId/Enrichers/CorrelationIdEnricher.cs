using System;
using System.Web;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private const string CorrelationIdPropertyName = "CorrelationId";
        private static readonly string CorrelationIdItemName = $"{typeof(CorrelationIdEnricher).Name}+CorrelationId";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

            if (HttpContext.Current == null)
                return;

            var correlationId = GetCorrelationId();
            var correlationIdProperty = new LogEventProperty(CorrelationIdPropertyName, new ScalarValue(correlationId));

            logEvent.AddPropertyIfAbsent(correlationIdProperty);
        }

        private static string GetCorrelationId()
        {
            return (string) (HttpContext.Current.Items[CorrelationIdItemName] ??
                             (HttpContext.Current.Items[CorrelationIdItemName] = Guid.NewGuid().ToString()));
        }
    }
}