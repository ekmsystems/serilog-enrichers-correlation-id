using System;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

#if FULLFRAMEWORK
using Serilog.Enrichers.CorrelationId.Accessors;
#endif

#if NETSTANDARD || NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
#endif

using Serilog.Enrichers.CorrelationId.Extensions;

namespace Serilog.Enrichers
{
    public class CorrelationIdHeaderEnricher : ILogEventEnricher
    {
        private const string CorrelationIdPropertyName = "CorrelationId";
        private readonly string _headerKey;
        private readonly IHttpContextAccessor _contextAccessor;

#if FULLFRAMEWORK
        public CorrelationIdHeaderEnricher(string headerKey) : this(headerKey, new IISPipelineHttpContextAccessor())
        {
        }
#endif

#if NETSTANDARD || NETSTANDARD2_0
        public CorrelationIdHeaderEnricher(string headerKey) : this(headerKey, new HttpContextAccessor())
        {
        }
#endif

        private CorrelationIdHeaderEnricher(string headerKey, IHttpContextAccessor contextAccessor)
        {
            _headerKey = headerKey;
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_contextAccessor.HttpContext == null)
                return;

            var correlationId = GetCorrelationId();

            var correlationIdProperty = new LogEventProperty(CorrelationIdPropertyName, new ScalarValue(correlationId));

            logEvent.AddPropertyIfAbsent(correlationIdProperty);
        }

        private string GetCorrelationId()
        {
            var header = string.Empty;

            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue(_headerKey, out var values))
            {
                header = values.FirstOrDefault();
            }

            var correlationId = string.IsNullOrEmpty(header)
                                    ? Guid.NewGuid().ToString()
                                    : header;

            _contextAccessor.HttpContext.Response.AddHeader(_headerKey, correlationId);

            return correlationId;
        }
    }
}