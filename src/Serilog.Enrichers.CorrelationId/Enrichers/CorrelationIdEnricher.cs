using System;

#if NETFULL
using Serilog.Enrichers.CorrelationId.Accessors;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Serilog.Enrichers
{
    public class CorrelationIdEnricher : CorrelationIdEnricherBase
    {
        public CorrelationIdEnricher() : this(new HttpContextAccessor())
        {
        }

        internal CorrelationIdEnricher(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override string GetCorrelationId()
        {
            return (string)(ContextAccessor.HttpContext.Items[CorrelationIdConstants.CorrelationIdItemName] ??
                             (ContextAccessor.HttpContext.Items[CorrelationIdConstants.CorrelationIdItemName] = Guid.NewGuid().ToString()));
        }
    }
}
