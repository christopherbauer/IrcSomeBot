using System;
using System.Collections.Generic;
using System.Linq;
using IrcSomeBot.Responder;
using Moq;
using NUnit.Framework;

namespace IrcSomeBot.Tests.Responder
{
    public class QuoteResponderTests
    {

        [TestFixture]
        public class WhenGetResponse
        {
            [Test]
            public void ThenShouldRequestTransformedTickerGivenTickerTransformExists()
            {
                // Arrange
                var stockTickerDataSource = new Mock<IStockTickerDataSource>();
                stockTickerDataSource.Setup(source => source.GetPricingData("TURD")).Returns(new List<string> { "TURD" });

                var settingsSource = new Mock<ISettingsSource>();
                settingsSource.Setup(source => source.GetValue<string>("quote-ticker-transforms")).Returns("LOL:TURD");

                var responder = new QuoteResponder(stockTickerDataSource.Object, settingsSource.Object, TimeSpan.MinValue, "test", "test");

                //Act
                var responses = responder.GetResponse(new IrcMessage("HSS", "gateway", "r-wsb", ",LOL"));

                //Assert
                Assert.That(responses.First(), Is.EqualTo("PRIVMSG r-wsb :TURD"));
            }
        }

    }
}