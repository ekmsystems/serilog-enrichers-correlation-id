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
            return (string)(ContextAccessor.HttpContext.Items[CorrelationIdItemName] ??
                             (ContextAccessor.HttpContext.Items[CorrelationIdItemName] = Guid.NewGuid().ToString()));
        }
    }
}
