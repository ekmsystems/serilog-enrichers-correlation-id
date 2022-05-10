using System;
using NUnit.Framework;
using Serilog.Enrichers;

namespace Serilog.Tests.Enrichers
{
    [TestFixture]
    [Parallelizable]
    public class DefaultCorrelationIdGeneratorTests
    {
        [Test]
        public void When_CallingDefaultCorrelationIdGenerator_Should_ReturnValidGuid()
        {
            var generator = new DefaultCorrelationIdGenerator( );

            var sut = generator.Generate( );

            Assert.NotNull(sut);
            Assert.IsNotEmpty(sut);
            Assert.IsTrue(Guid.TryParse(sut, out var _));
        }
    }
}