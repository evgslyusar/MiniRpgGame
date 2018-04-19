using FluentAssertions;
using MiniRpgGame.Domain;
using MiniRpgGame.Domain.Platform;
using MiniRpgGame.Settings;
using Moq;
using Xunit;

namespace Game.Tests.Domain
{
    public class BattleTests
    {
        [Fact]
        public void DecreaseHealthAndIncreaseNumberOfCoinsWhenWin()
        {
            const int expectedCoins = 10; 
            const int expectedHealth = 48;

            var eventBus = Mock.Of<IEventBus>();
            
            var battleLogicMock = new Mock<IBattleLogic>();
            battleLogicMock.Setup(l => l.IsWon(It.IsAny<int>())).Returns(true);
            
            var settings = new Mock<IBattleSettings>();
            settings.Setup(l => l.LossOfHealthRate).Returns(10);
            settings.Setup(l => l.NumerOfCoins).Returns(5);
            
            var battle = new Battle(battleLogicMock.Object, eventBus, settings.Object);
            var player = new Player(53, 100, 1, 5, eventBus);
            
            battle.Do(player);

            player.Coins.Should().Be(expectedCoins);
            player.Health.Should().Be(expectedHealth);
        }
        
        [Fact]
        public void ReduceHealthAndReduceNumberOfCoinsWhenLooseFight()
        {
            const int expectedHealth = 20;
            
            var eventBus = Mock.Of<IEventBus>();
            
            var battleLogicMock = new Mock<IBattleLogic>();
            battleLogicMock.Setup(l => l.IsWon(It.IsAny<int>())).Returns(false);
            
            var settings = new Mock<IBattleSettings>();
            settings.Setup(l => l.LossOfHealth).Returns(40);
            
            var battle = new Battle(battleLogicMock.Object, eventBus, settings.Object);
            var player = new Player(60, 100, 1, 5, eventBus);
            
            battle.Do(player);

            player.Health.Should().Be(expectedHealth);
        }
    }
}