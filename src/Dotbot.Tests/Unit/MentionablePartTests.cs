using Dotbot.Models;
using Dotbot.Tests.Fakes;
using Dotbot.Tests.Fixtures;
using Xunit;

namespace Dotbot.Tests.Unit
{
    public sealed class MentionablePartTests
    {
        public sealed class TheHandleMentionMethod
        {
            [Fact]
            public void Should_Handle_Mention_For_Correctly_Formatted_Message()
            {
                // Given
                var sut = new FakeMentionablePart();
                var fixture = new ReplyContextFixture
                {
                    Text = "@bot hello world!",
                    Bot = new User("1", "bot", "Botty McBotface")
                };

                // When
                var result = sut.Handle(fixture.Create());

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Not_Handle_Mention_For_Other_Users()
            {
                // Given
                var sut = new FakeMentionablePart();
                var fixture = new ReplyContextFixture
                {
                    Text = "@troll hello world!",
                    Bot = new User("1", "bot", "Botty McBotface")
                };

                // When
                var result = sut.Handle(fixture.Create());

                // Then
                Assert.False(result);
            }

            [Theory]
            [InlineData("bot_user")]
            [InlineData("bot-user")]
            [InlineData("botuser#01")]
            public void Should_Handle_Mention_For_Usernames_With_Special_Characters(string botName)
            {
                // Given
                var sut = new FakeMentionablePart();
                var fixture = new ReplyContextFixture
                {
                    Text = $"@{botName} hello world!",
                    Bot = new User("1", botName, "Botty McBotface")
                };

                // When
                var result = sut.Handle(fixture.Create());

                // Then
                Assert.True(result);
            }
        }
    }
}
