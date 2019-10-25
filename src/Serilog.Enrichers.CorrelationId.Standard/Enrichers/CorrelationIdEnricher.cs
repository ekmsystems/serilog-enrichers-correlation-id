using System;
using Serilog.Core;
using Serilog.Events;
#if FULLFRAMEWORK
using Serilog.Enrichers.CorrelationId.Accessors;

#endif
#if NETSTANDARD
using Microsoft.AspNetCore.Http;

#endif

namespace Serilog.Enrichers
{
	public class CorrelationIdEnricher : ILogEventEnricher
	{
		private const string CorrelationIdPropertyName = "CorrelationId";
		private static readonly string CorrelationIdItemName = $"{typeof(CorrelationIdEnricher).Name}+CorrelationId";
		private readonly IHttpContextAccessor _contextAccessor;

#if FULLFRAMEWORK
		public CorrelationIdEnricher() : this(new IISPipelineHttpContextAccessor())
		{
		}
#endif

#if NETSTANDARD
		public CorrelationIdEnricher() : this(new HttpContextAccessor())
		{
		}
#endif

		public CorrelationIdEnricher(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
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
			return
				(_contextAccessor.HttpContext.Items[CorrelationIdItemName] ??
				 (_contextAccessor.HttpContext.Items[CorrelationIdItemName] = Guid.NewGuid()))
				.ToString();
		}
	}
}
