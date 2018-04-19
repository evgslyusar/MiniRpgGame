using System;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;
using MiniRpgGame.Settings;

namespace MiniRpgGame.Domain
{
    public sealed class Doctor
    {
        private readonly Random _random = new Random();
        private readonly IEventBus _eventBus;
        private readonly IHealerSettings _settings;

        public Doctor(IEventBus eventBus, IHealerSettings settings)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Heal(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            if (!player.CanWithdrawalCoins(_settings.Cost))
            {
                _eventBus.Publish(new MoveCompleteEvent(false, "Not enough coins for healing.."));
            }
            else
            {
                var value = _random.Next(_settings.MinHealth, _settings.MaxHealth);
                player.DecreaseCoins(_settings.Cost);
                player.IncreaseHealth(value);
                
                _eventBus.Publish(new MoveCompleteEvent(true, $"The level of health is increased by {value} points."));
            }
        }
    }
}