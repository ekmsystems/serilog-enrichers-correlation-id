using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Serilog.Enrichers;
using Serilog.Enrichers.CorrelationId.Middlewares;

namespace Serilog.Tests.Middlewares
{
    [TestFixture]
    [Parallelizable]
    public class CorrelationIdHeaderSupplierMiddlewareTests
    {
        private readonly RequestDelegate _onNext = (innerHttpContext) => { return Task.CompletedTask; };

        [Test]
        public void WhenNoCorrelationIdInRequest_ThenShould_CreateNewOne()
        {
            const string headerKey = "some-correlation-id-key";
            var context = new DefaultHttpContext();

            new CorrelationIdHeaderSupplierMiddleware(_onNext, headerKey).Invoke(context);

            var correlationIdFromItems = (string)context.Items[CorrelationIdConstants.CorrelationIdItemName];
            context.Response.Headers.TryGetValue(headerKey, out var correlationIdFromResponse);

            Assert.IsNotEmpty(correlationIdFromItems);
            Assert.IsNotEmpty(correlationIdFromResponse);
            Assert.AreEqual(correlationIdFromItems, correlationIdFromResponse);
        }

        [Test]
        public void WhenCorrelationIdSuppliedInRequest_ThenShould_ReuseIt()
        {
            const string correlationId = "some correlation id value";
            var context = new DefaultHttpContext();
            context.Request.Headers.Add(CorrelationIdConstants.CorrelationIdHeaderKay, correlationId);

            new CorrelationIdHeaderSupplierMiddleware(_onNext).Invoke(context);

            var correlationIdFromItems = (string)context.Items[CorrelationIdConstants.CorrelationIdItemName];
            context.Response.Headers.TryGetValue(CorrelationIdConstants.CorrelationIdHeaderKay, out var correlationIdFromResponse);

            Assert.IsNotEmpty(correlationIdFromItems);
            Assert.IsNotEmpty(correlationIdFromResponse);
            Assert.AreEqual(correlationId, correlationIdFromItems);
            Assert.AreEqual(correlationId, correlationIdFromResponse);
        }
    }
}
