using FluentAssertions;
using MiniRpgGame.Domain;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;
using MiniRpgGame.Settings;
using Moq;
using Xunit;

namespace Game.Tests.Domain
{
    public class DoctorTests
    {
        [Fact]
        public void IncreasesHealthWhenHealWhereHaveEnoughCoins()
        {
            const int expectedCoins = 7;
            const int health = 52;
            
            var eventBusMock = new Mock<IEventBus>();

            var settings = new Mock<IHealerSettings>();
            settings.Setup(l => l.MinHealth).Returns(1);
            settings.Setup(l => l.MaxHealth).Returns(10);
            settings.Setup(l => l.Cost).Returns(3);

            var healer = new Doctor(eventBusMock.Object, settings.Object);
            var player = new Player(health, 100, 1, 10, Mock.Of<EventBus>());

            healer.Heal(player);

            eventBusMock.Verify(m => m.Publish(It.IsAny<MoveCompleteEvent>()));
            player.Coins.Should().Be(expectedCoins);
            player.Health.Should().BeGreaterThan(health);
        }
    }
}