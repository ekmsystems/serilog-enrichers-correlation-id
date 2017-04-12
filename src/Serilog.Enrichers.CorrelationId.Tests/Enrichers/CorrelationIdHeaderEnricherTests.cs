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
        [SetUp]
        public void SetUp()
        {
            _request = new HttpRequest("test", "https://serilog.net/", "");
            _response = new HttpResponse(new StringWriter());

            HttpContext.Current = new HttpContext(_request, _response);
        }

        private HttpRequest _request;
        private HttpResponse _response;

        [Test]
        public void ShouldCreateCorrelationIdIfNotPresentInHeader()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationIdHeader()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.NotNull((string) evt.Properties["CorrelationId"].LiteralValue());
        }

        [Test]
        public void ShouldExtractCorrelationIdFromHeader()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCorrelationIdHeader()
                .WriteTo.Sink(new DelegateSink.DelegatingSink(e => evt = e))
                .CreateLogger();

            _request.AddHeader("x-correlation-id", "my-correlation-id");

            log.Information(@"Has a CorrelationId property");

            Assert.NotNull(evt);
            Assert.AreEqual((string) evt.Properties["CorrelationId"].LiteralValue(), "my-correlation-id");
        }
    }
}