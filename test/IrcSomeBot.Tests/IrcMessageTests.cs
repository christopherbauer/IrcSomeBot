using NUnit.Framework;

namespace IrcSomeBot.Tests
{
    public class IrcMessageTests
    {

        [TestFixture]
        public class WhenParseUserMessage
        {
            [Test]
            public void ThenShouldSetMessageToWhatComesAfterSpaceColonGivenMessageExists()
            {
                // Arrange
                var inputLine = ":hss!~hss@pool-96-250-215-84.nycmny.fios.verizon.net PRIVMSG ##wsb :test";

                //Act
                var ircMessage = IrcMessage.ParseInput(inputLine);

                //Assert
                Assert.That(ircMessage.Message, Is.EqualTo("test"));
            }
            [Test]
            public void ThenShouldSetSenderToWhatComesAfterColonButBeforeExclamationPoint()
            {
                // Arrange
                var inputLine = ":hss!~hss@pool-96-250-215-84.nycmny.fios.verizon.net PRIVMSG ##wsb :test";

                //Act
                var ircMessage = IrcMessage.ParseInput(inputLine);

                //Assert
                Assert.That(ircMessage.Sender, Is.EqualTo("hss"));
            }
            [Test]
            public void ThenShouldSetTargetToWhatComesAfterSecondSpaceButBeforeSpaceColon()
            {
                // Arrange
                var inputLine = ":hss!~hss@pool-96-250-215-84.nycmny.fios.verizon.net PRIVMSG ##wsb :test";

                //Act
                var ircMessage = IrcMessage.ParseInput(inputLine);

                //Assert
                Assert.That(ircMessage.Target, Is.EqualTo("##wsb"));
            }
            [Test]
            public void ThenShouldSetGatwayToWhatComesAfterExclamationPointButBeforeFirstSpace() //TODO: this is wrong, but I'm not dealing with it just yet
            {
                // Arrange
                var inputLine = ":hss!~hss@pool-96-250-215-84.nycmny.fios.verizon.net PRIVMSG ##wsb :test";

                //Act
                var ircMessage = IrcMessage.ParseInput(inputLine);

                //Assert
                Assert.That(ircMessage.Gateway, Is.EqualTo("~hss@pool-96-250-215-84.nycmny.fios.verizon.net"));
            }
        }
 
    }
}