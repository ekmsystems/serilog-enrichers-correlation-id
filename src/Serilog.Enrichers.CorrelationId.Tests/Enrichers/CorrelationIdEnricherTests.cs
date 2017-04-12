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
        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("test", "https://serilog.net/", ""),
                new HttpResponse(new StringWriter()));
        }

        [Test]
        public void ShouldCreateCorrelationId()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationId()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.NotNull((string) evt.Properties["CorrelationId"].LiteralValue());
        }
    }
}