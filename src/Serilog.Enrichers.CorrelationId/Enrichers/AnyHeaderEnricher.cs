using System;
using System.Linq;
using Serilog.Core;
using Serilog.Events;
using Serilog.Configuration;
#if NETFULL
using Serilog.Enrichers.CorrelationId.Accessors;
using Serilog.Enrichers.CorrelationId.Extensions;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class IdHeaderEnricher : ILogEventEnricher
    {
        private readonly string _logPropertyName;
        private readonly string _headerKey;
        private readonly bool _createIfNoneFound;
        private readonly IHttpContextAccessor _contextAccessor;

        public IdHeaderEnricher(string headerKey, string logPropertyName, bool createIfNoneFound) : this(headerKey, logPropertyName, createIfNoneFound, new HttpContextAccessor())
        {
        }

        internal IdHeaderEnricher(string headerKey, string logPropertyName, bool createIfNoneFound, IHttpContextAccessor contextAccessor)
        {
            _headerKey = headerKey;
            _contextAccessor = contextAccessor;
            _createIfNoneFound = createIfNoneFound;
            _logPropertyName = logPropertyName;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_contextAccessor.HttpContext == null)
            {
                return;//throw new ArgumentNullException("Please call AddHttpContextAccessor in startup for this to work");
            }

            var Id = GetId();
            if (Id != null)
            {
                var IdProperty = new LogEventProperty(_logPropertyName, new ScalarValue(Id));

                logEvent.AddOrUpdateProperty(IdProperty);
            }
        }

        private string GetId()
        {
            var header = string.Empty;

            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue(_headerKey, out var values))
            {
                header = values.FirstOrDefault();
            }
            else if (_contextAccessor.HttpContext.Response.Headers.TryGetValue(_headerKey, out values))
            {
                header = values.FirstOrDefault();
            }
            if (string.IsNullOrEmpty(header) && !_createIfNoneFound)
            {
                return null;
            }
            var Id = string.IsNullOrEmpty(header)
                                    ? Guid.NewGuid().ToString()
                                    : header;

#if NETFULL
            if(!_contextAccessor.HttpContext.Response.HeadersWritten &&
                !_contextAccessor.HttpContext.Response.Headers.AllKeys.Contains(_headerKey))
#else
            if (!_contextAccessor.HttpContext.Response.Headers.ContainsKey(_headerKey))
#endif
            {
                _contextAccessor.HttpContext.Response.Headers.Add(_headerKey, Id);
            }

            return Id;
        }
    }
    public static class HeaderValueLoggerConfigurationExtensions
    {
        public static LoggerConfiguration CreateOrIncludeWithHeader(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            string headerKey, string logPropertyName, bool createIfNoneFound)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new IdHeaderEnricher(headerKey, logPropertyName, createIfNoneFound));
        }
    }
}