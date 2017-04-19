using System.IO;
using System.Web;
using NUnit.Framework;
using Serilog.Events;
using Serilog.Tests.Support;

namespace Serilog.Tests.Enrichers
{
    [TestFixture]
    [Parallelizable]
    public class CorrelationIdHeaderEnricherTests
    {
        [Test]
        public void When_CorrelationIdNotInHeader_Should_CreateCorrelationIdProperty()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("test", "https://serilog.net/", ""),
                new HttpResponse(new StringWriter()));
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationIdHeader()
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
            var request = new HttpRequest("test", "https://serilog.net/", "");
            HttpContext.Current = new HttpContext(request, new HttpResponse(new StringWriter()));
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationIdHeader()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            request.AddHeader("x-correlation-id", "my-correlation-id");

            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsTrue(evt.Properties.ContainsKey("CorrelationId"));
            Assert.AreEqual(evt.Properties["CorrelationId"].LiteralValue(), "my-correlation-id");
        }

        [Test]
        public void When_CurrentHttpContextIsNull_ShouldNot_CreateCorrelationIdProperty()
        {
            HttpContext.Current = null;
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationIdHeader()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Does not have a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsFalse(evt.Properties.ContainsKey("CorrelationId"));
        }
    }
}