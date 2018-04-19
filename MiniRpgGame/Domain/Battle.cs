using System;
using System.Collections.Generic;
using System.Linq;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;
using MiniRpgGame.Settings;

namespace MiniRpgGame.Domain
{
    public class Battle
    {
        private readonly IBattleLogic _battleLogic;
        private readonly IEventBus _eventBus;
        private readonly IBattleSettings _settings;

        public Battle(IBattleLogic battleLogic, IEventBus eventBus, IBattleSettings settings)
        {
            _battleLogic = battleLogic ?? throw new ArgumentNullException(nameof(battleLogic));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Do(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            var playerPower = CalculatePower(player.Power, player.Weapons);

            if (_battleLogic.IsWon(playerPower))
            {
                PlayerWin(player);
                _eventBus.Publish(new MoveCompleteEvent(true, "You won the battle!"));
            }
            else
            {
                PlayerLose(player);
                _eventBus.Publish(new MoveCompleteEvent(true, "You lost the battle!"));
            }
        }

        private void PlayerWin(Player player)
        {
            var healthValue = (int) Math.Round((double) player.Health / 100 * _settings.LossOfHealthRate);
            player.DecreaseHealth(healthValue);
            player.IncreaseCoins(_settings.NumerOfCoins);
        }

        private void PlayerLose(Player player)
        {
            player.DecreaseHealth(_settings.LossOfHealth);
        }

        private static int CalculatePower(int power, IEnumerable<Weapons> weapons) => power + weapons.Sum(w => w.Value);
    }
}