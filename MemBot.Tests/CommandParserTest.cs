using MemBot.Parsers;
using MemBotModels.Models;
using Xunit;

namespace MemBot.Tests
{
    public class CommandParserTest
    {
        [Fact]
        public void TryParseServiceCommandReturnsValidResult()
        {
            // arrange
            string rawCommand = "/add arg0 arg1";
            bool isParseSucceed;

            // act
            isParseSucceed = CommandParser.TryParseServiceCommand(rawCommand, out var serviceCommand, out var args);

            // assert
            Assert.True(isParseSucceed);
            Assert.Equal(ServiceCommand.Add, serviceCommand);
            Assert.Equal(new string[] { "arg0", "arg1" }, args);
        }

        [Theory]
        [InlineData("/command arg0 arg1")]
        [InlineData("/add")]
        public void TryParseServiceCommandReturnsFalse(string rawCommand)
        {
            // arrange
            bool isParseSucceed;

            // act
            isParseSucceed = CommandParser.TryParseServiceCommand(rawCommand, out var serviceCommand, out var args);

            // assert
            Assert.False(isParseSucceed);
            Assert.Equal(ServiceCommand.Unknown, serviceCommand);
            Assert.Null(args);
        }

        // TODO: Add test for case when ServiceCommand == Help
    }
}
