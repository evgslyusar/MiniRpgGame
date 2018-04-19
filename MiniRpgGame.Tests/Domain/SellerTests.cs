using FluentAssertions;
using MiniRpgGame.Domain;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;
using Moq;
using Xunit;

namespace Game.Tests.Domain
{
    public class SellerTests
    {
        [Fact]
        public void BuyWeaponsWhenHaveEnoughCoins()
        {
            const int expectedCoins = 7;
            
            var eventBusMock = new Mock<IEventBus>();
            
            var weapons = new Weapons(3);
            var warehouseMock = new Mock<IStockroom>();
            warehouseMock.Setup(w => w.TakeWeapons()).Returns(() => (3, weapons));

            var seller = new Seller(warehouseMock.Object, eventBusMock.Object);
            var player = new Player(100, 100, 1, 10, Mock.Of<EventBus>());

            seller.SellWeapons(player);
            
            eventBusMock.Verify(m => m.Publish(It.IsAny<MoveCompleteEvent>()));
            player.Weapons.Should().Contain(x => x == weapons);
            player.Coins.Should().Be(expectedCoins);
        }
        
        [Fact]
        public void DoNotSellWeaponsWhenCoinsAreNotEnough()
        {
            const int expectedCoins = 1;
            
            var eventBusMock = new Mock<IEventBus>();
            
            var warehouseMock = new Mock<IStockroom>();
            warehouseMock.Setup(w => w.TakeWeapons()).Returns(() => (3,  new Weapons(3)));

            var seller = new Seller(warehouseMock.Object, eventBusMock.Object);
            var player = new Player(100, 100, 1, 1, Mock.Of<EventBus>());

            seller.SellWeapons(player);
            
            eventBusMock.Verify(m => m.Publish(It.IsAny<MoveCompleteEvent>()));
            player.Weapons.Should().BeEmpty();
            player.Coins.Should().Be(expectedCoins);
        }
        
        [Fact]
        public void BuyArmorsWhenHaveEnoughCoins()
        {
            const int expectedCoins = 3;
            const int expectedMaxHealth = 102;
            
            var eventBusMock = new Mock<IEventBus>();
            
            var armor = new Armor(2);
            var warehouseMock = new Mock<IStockroom>();
            warehouseMock.Setup(w => w.TakeArmor()).Returns(() => (7, armor));

            var seller = new Seller(warehouseMock.Object, eventBusMock.Object);
            var player = new Player(91, 100, 1, 10, Mock.Of<EventBus>());

            seller.SellArmor(player);
            
            eventBusMock.Verify(m => m.Publish(It.IsAny<MoveCompleteEvent>()));
            player.Armor.Should().Contain(x => x == armor);
            player.MaxHealth.Should().Be(expectedMaxHealth);
            player.Coins.Should().Be(expectedCoins);
        }
    }

   
}