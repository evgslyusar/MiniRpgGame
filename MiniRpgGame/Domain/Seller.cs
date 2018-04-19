using System;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;

namespace MiniRpgGame.Domain
{
    public sealed class Seller
    {
        private readonly IStockroom _stockroom;
        private readonly IEventBus _eventBus;

        public Seller(IStockroom stockroom, IEventBus eventBus)
        {
            _stockroom = stockroom ?? throw new ArgumentNullException(nameof(stockroom));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void SellWeapons(Player player)
        {
            var item = _stockroom.TakeWeapons();
            if (!player.CanWithdrawalCoins(item.Cost))
            {
                _eventBus.Publish(new MoveCompleteEvent(false, "Not enough coins to buy an armor."));
            }
            else
            {
                player.TakeCoins(item.Cost);
                player.WearWeapons(item.Weapons);

                _eventBus.Publish(new MoveCompleteEvent(true, $"You got a new weapon!"));
            }
        }

        public void SellArmor(Player player)
        {
            var item = _stockroom.TakeArmor();
            if (!player.CanWithdrawalCoins(item.Item1))
            {
                _eventBus.Publish(new MoveCompleteEvent(false, "Not enough coins to buy an armor."));
            }
            else
            {
                player.TakeCoins(item.Cost);
                player.WearArmor(item.Armor);

                _eventBus.Publish(new MoveCompleteEvent(true, $"You got a new armor!"));
            }
        }
    }
}