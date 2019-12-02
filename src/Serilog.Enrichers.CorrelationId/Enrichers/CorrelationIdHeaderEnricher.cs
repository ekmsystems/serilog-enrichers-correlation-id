using System;
using System.Linq;

#if NETFULL
using Serilog.Enrichers.CorrelationId.Accessors;
using Serilog.Enrichers.CorrelationId.Extensions;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class CorrelationIdHeaderEnricher : CorrelationIdEnricherBase
    {
        private readonly string _headerKey;

        public CorrelationIdHeaderEnricher(string headerKey) : this(headerKey, new HttpContextAccessor())
        {
        }

        internal CorrelationIdHeaderEnricher(string headerKey, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _headerKey = headerKey;
        }

        protected override string GetCorrelationId()
        {
            var correlationId = Guid.NewGuid().ToString();

            if (ContextAccessor.HttpContext.Request.Headers.TryGetValue(_headerKey, out var values))
            {
                correlationId = values.FirstOrDefault();
            }
            else if (ContextAccessor.HttpContext.Response.Headers.TryGetValue(_headerKey, out values))
            {
                correlationId = values.FirstOrDefault();
            }
#if NETFULL
            else if (ContextAccessor.HttpContext.Items.Contains(CorrelationIdConstants.CorrelationIdItemName))
#else
            else if (ContextAccessor.HttpContext.Items.ContainsKey(CorrelationIdConstants.CorrelationIdItemName))
#endif
            {
                correlationId = (string)ContextAccessor.HttpContext.Items[CorrelationIdConstants.CorrelationIdItemName];
            }
            else
            {
                ContextAccessor.HttpContext.Items[CorrelationIdConstants.CorrelationIdItemName] = correlationId;
            }

            return correlationId;
        }
    }
}
