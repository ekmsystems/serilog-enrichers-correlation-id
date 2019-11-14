using System;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

#if NETFULL
using Serilog.Enrichers.CorrelationId.Accessors;
using Serilog.Enrichers.CorrelationId.Extensions;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class CorrelationIdHeaderEnricher : ILogEventEnricher
    {
        private const string CorrelationIdPropertyName = "CorrelationId";
        private static readonly string CorrelationIdHttpContextItemName = $"{nameof(CorrelationIdHeaderEnricher)}.CorrelationId";

        private readonly string _headerKey;
        private readonly IHttpContextAccessor _contextAccessor;

        public CorrelationIdHeaderEnricher(string headerKey) : this(headerKey, new HttpContextAccessor())
        {
        }

        internal CorrelationIdHeaderEnricher(string headerKey, IHttpContextAccessor contextAccessor)
        {
            _headerKey = headerKey;
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_contextAccessor.HttpContext == null)
            {
                return;
            }

            var correlationId = GetCorrelationId();

            var correlationIdProperty = new LogEventProperty(CorrelationIdPropertyName, new ScalarValue(correlationId));

            logEvent.AddOrUpdateProperty(correlationIdProperty);
        }

        private string GetCorrelationId()
        {
            var correlationId = Guid.NewGuid().ToString();

            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue(_headerKey, out var values))
            {
                correlationId = values.FirstOrDefault();
            }
            else if (_contextAccessor.HttpContext.Response.Headers.TryGetValue(_headerKey, out values))
            {
                correlationId = values.FirstOrDefault();
            }
#if NETFULL
            else if (_contextAccessor.HttpContext.Items.Contains(CorrelationIdHttpContextItemName))
#else
            else if (_contextAccessor.HttpContext.Items.ContainsKey(CorrelationIdHttpContextItemName))
#endif
            {
                correlationId = (string)_contextAccessor.HttpContext.Items[CorrelationIdHttpContextItemName];
            }
            else
            {
                _contextAccessor.HttpContext.Items[CorrelationIdHttpContextItemName] = correlationId;
            }

            return correlationId;
        }
    }
}
