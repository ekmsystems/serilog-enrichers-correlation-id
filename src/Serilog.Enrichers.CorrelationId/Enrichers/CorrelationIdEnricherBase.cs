using Serilog.Core;
using Serilog.Events;

#if NETFULL
using Serilog.Enrichers.CorrelationId.Accessors;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public abstract class CorrelationIdEnricherBase: ILogEventEnricher
    {
        private const string _correlationIdPropertyName = "CorrelationId";

        protected readonly IHttpContextAccessor ContextAccessor;

        public CorrelationIdEnricherBase(IHttpContextAccessor contextAccessor)
        {
            ContextAccessor = contextAccessor;
        }

        protected abstract string GetCorrelationId();

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (ContextAccessor.HttpContext == null)
            {
                return;
            }

            var correlationId = GetCorrelationId();

            var correlationIdProperty = new LogEventProperty(_correlationIdPropertyName, new ScalarValue(correlationId));

            logEvent.AddOrUpdateProperty(correlationIdProperty);
        }
    }
}
