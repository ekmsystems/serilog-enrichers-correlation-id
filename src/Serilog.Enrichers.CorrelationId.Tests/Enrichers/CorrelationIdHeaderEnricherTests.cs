using FakeItEasy;
using NUnit.Framework;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Tests.Support;
using Microsoft.AspNetCore.Http;

namespace Serilog.Tests.Enrichers
{
    [TestFixture]
    [Parallelizable]
    public class CorrelationIdHeaderEnricherTests
    {
        private const string HeaderKey = "x-correlation-id";

        [SetUp]
        public void SetUp()
        {
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();
            _enricher = new CorrelationIdHeaderEnricher(HeaderKey, _httpContextAccessor);
        }

        private IHttpContextAccessor _httpContextAccessor;
        private CorrelationIdHeaderEnricher _enricher;

        [Test]
        public void When_CorrelationIdNotInHeader_Should_CreateCorrelationIdProperty()
        {
            A.CallTo(() => _httpContextAccessor.HttpContext)
                .Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(_enricher)
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsTrue(evt.Properties.ContainsKey("CorrelationId"));
            Assert.NotNull(evt.Properties["CorrelationId"].LiteralValue());
        }

        [Test]
        public void When_CorrelationIdIsInHeader_Should_ExtractCorrelationIdFromHeader()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderKey] = "my-correlation-id";

            A.CallTo(() => _httpContextAccessor.HttpContext)
                .Returns(httpContext);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(_enricher)
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();


            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsTrue(evt.Properties.ContainsKey("CorrelationId"));
            Assert.AreEqual(evt.Properties["CorrelationId"].LiteralValue(), "my-correlation-id");
        }

        [Test]
        public void When_CurrentHttpContextIsNull_ShouldNot_CreateCorrelationIdProperty()
        {
            A.CallTo(() => _httpContextAccessor.HttpContext)
                .Returns(null);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(_enricher)
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Does not have a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsFalse(evt.Properties.ContainsKey("CorrelationId"));
        }

        [Test]
        public void When_MultipleLoggingCallsMade_Should_KeepUsingCreatedCorrelationIdProperty()
        {
            var httpContext = new DefaultHttpContext();

            A.CallTo(() => _httpContextAccessor.HttpContext)
                .Returns(httpContext);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(_enricher)
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Has a CorrelationId property");

            var correlationId = evt.Properties["CorrelationId"].LiteralValue();

            log.Information(@"Here is another event");

            Assert.AreEqual(correlationId, evt.Properties["CorrelationId"].LiteralValue());
        }
    }
}
