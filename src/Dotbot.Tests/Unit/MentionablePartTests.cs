using Dotbot.Tests.Fixtures;
using Dotbot.Tests.Stubs;
using Xunit;

namespace Dotbot.Tests.Unit
{
    public sealed class MentionablePartTests
    {
        public sealed class TheHandleMentionMethod
        {
            [Fact]
            public void Should_Handle_Mention_If_Meant_For_Bot()
            {
                // Given
                var sut = new MentionablePartStub();

                var fixture = new ReplyContextFixture();
                var context = fixture.Create(
                    from: fixture.Human, 
                    to: fixture.Bot);

                // When
                var result = sut.Handle(context);

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Not_Handle_Mention_If_Not_Meant_For_Bot()
            {
                // Given
                var sut = new MentionablePartStub();

                var fixture = new ReplyContextFixture();
                var context = fixture.Create(
                    from: fixture.Bot,
                    to: fixture.Human);

                // When
                var result = sut.Handle(context);

                // Then
                Assert.False(result);
            }
        }
    }
}
