using IrcSomeBot.Responder;
using NUnit.Framework;

namespace IrcSomeBot.Tests.Responder
{
    public class TickerTrackerRecordTests
    {
        [TestFixture]
        public class WhenComparingTickerTrackerRecord
        {
            [Test]
            public void ThenShouldBeEqualGivenSameTickerAndSameContext()
            {
                //arrange
                var record1 = new TickerTrackerRecord("TSLA", "r-wsb");
                var record2 = new TickerTrackerRecord("TSLA", "r-wsb");

                //act
                var @equals = record1.Equals(record2);

                //assert
                Assert.That(@equals, Is.True);
            }
            [Test]
            public void ThenShouldBeNotEqualGivenDifferentTickerAndSameContext()
            {
                //arrange
                var record1 = new TickerTrackerRecord("TSLA", "r-wsb");
                var record2 = new TickerTrackerRecord("F", "r-wsb");

                //act
                var @equals = record1.Equals(record2);

                //assert
                Assert.That(@equals, Is.False);
            }
            [Test]
            public void ThenShouldBeNotEqualGivenSameTickerAndDifferentContext()
            {
                //arrange
                var record1 = new TickerTrackerRecord("F", "hss");
                var record2 = new TickerTrackerRecord("F", "r-wsb");

                //act
                var @equals = record1.Equals(record2);

                //assert
                Assert.That(@equals, Is.False);
            }
        } 
    }
}