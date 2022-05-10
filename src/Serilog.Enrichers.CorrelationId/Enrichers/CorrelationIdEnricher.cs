using System;
using Serilog.Core;
using Serilog.Events;

#if NETFULL
using Serilog.Enrichers.CorrelationId.Accessors;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private const string CorrelationIdPropertyName = "CorrelationId";
        private static readonly string CorrelationIdItemName = $"{typeof(CorrelationIdEnricher).Name}+CorrelationId";
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public CorrelationIdEnricher(ICorrelationIdGenerator correlationIdGenerator) : this(new HttpContextAccessor(), correlationIdGenerator)
        {
        }

        internal CorrelationIdEnricher(IHttpContextAccessor contextAccessor, ICorrelationIdGenerator correlationIdGenerator)
        {
            _contextAccessor = contextAccessor;
            _correlationIdGenerator = correlationIdGenerator;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_contextAccessor.HttpContext == null)
                return;

            var correlationId = GetCorrelationId();

            var correlationIdProperty = new LogEventProperty(CorrelationIdPropertyName, new ScalarValue(correlationId));

            logEvent.AddOrUpdateProperty(correlationIdProperty);
        }

        private string GetCorrelationId()
        {
            return (string) (_contextAccessor.HttpContext.Items[CorrelationIdItemName] ??
                             (_contextAccessor.HttpContext.Items[CorrelationIdItemName] = this._correlationIdGenerator.Generate()));
        }
    }
}
