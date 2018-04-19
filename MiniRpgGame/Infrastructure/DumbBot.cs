using System;
using MiniRpgGame.Domain;
using MiniRpgGame.Domain.Commands;

namespace MiniRpgGame.Infrastructure
{
    /// <summary>
    /// This dumb bot implements the simplest logic.
    /// used as an bot example!
    /// </summary>
    internal sealed class DumbBot : IBot
    {
        private const int MinHealth = 40;
        private const int MinCoins = 10;
        private const double HealthRate = 0.9;
        
        public void MakeMove(Gameplay gameplay)
        {
            if (gameplay == null) 
                throw new ArgumentNullException(nameof(gameplay));
            
            if (gameplay.Player.Coins >= MinCoins)
            {
                if (gameplay.Player.Health < MinHealth)
                {
                    gameplay.Handle(new HealCommand());
                    return;
                }

                if (gameplay.Player.Health < gameplay.Player.MaxHealth * HealthRate)
                {
                    gameplay.Handle(new BuyArmorCommand());
                    return;
                }

                gameplay.Handle(new BuyWeaponsCommand());
            }
            else
            {
                gameplay.Handle(new FightMonsterCommand());
            }
        }
    }
}