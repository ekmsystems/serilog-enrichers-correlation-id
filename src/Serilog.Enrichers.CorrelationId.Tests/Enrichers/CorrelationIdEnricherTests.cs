using System.IO;
using System.Web;
using NUnit.Framework;
using Serilog.Events;
using Serilog.Tests.Support;

namespace Serilog.Tests.Enrichers
{
    [TestFixture]
    [Parallelizable]
    public class CorrelationIdEnricherTests
    {
        [Test]
        public void When_CurrentHttpContextIsNotNull_Should_CreateCorrelationIdProperty()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("test", "https://serilog.net/", ""),
                new HttpResponse(new StringWriter()));
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationId()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsTrue(evt.Properties.ContainsKey("CorrelationId"));
            Assert.NotNull(evt.Properties["CorrelationId"].LiteralValue());
        }

        [Test]
        public void When_CurrentHttpContextIsNotNull_ShouldNot_CreateCorrelationIdProperty()
        {
            HttpContext.Current = null;
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationId()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Does not have a CorrelationId property");

            Assert.NotNull(evt);
            Assert.IsFalse(evt.Properties.ContainsKey("CorrelationId"));
        }
    }
}