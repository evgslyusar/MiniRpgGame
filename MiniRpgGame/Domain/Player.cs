using System;
using System.Collections.Generic;
using System.Linq;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;

namespace MiniRpgGame.Domain
{
    public sealed class Player
    {
        private readonly List<Weapon> _weapons = new List<Weapon>();

        private readonly List<Armor> _armor = new List<Armor>();

        private readonly int _maxHealth;
        private readonly IEventBus _eventBus;

        private const int MinHealth = 0;

        private const int MinCoins = 0;

        public IEnumerable<Weapon> Weapons => _weapons;

        public IEnumerable<Armor> Armor => _armor;

        public int Power { get; }

        public int Coins { get; private set; }

        public int Health { get; private set; }

        public int MaxHealth => _maxHealth + _armor.Sum(a => a.Value);

        public bool IsDead => Health == MinHealth;

        public Player(int health, int maxHealth, int power, int coins, IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            Health = health;
            Power = power;
            Coins = coins;
            _maxHealth = maxHealth;
        }

        public bool CanWithdrawalCoins(int cost)
        {
            ThrowExceptionIfPlayerIsDead();

            return Coins >= cost;
        }

        public void TakeCoins(int count)
        {
            ThrowExceptionIfPlayerIsDead();

            if (!CanWithdrawalCoins(count))
            {
                throw new InvalidOperationException("not enough coins.");
            }

            Coins -= count;
        }

        public void WearWeapons(Weapon weapoin)
        {
            if (weapoin == null) throw new ArgumentNullException(nameof(weapoin));
            ThrowExceptionIfPlayerIsDead();

            _weapons.Add(weapoin);
        }

        public void WearArmor(Armor armor)
        {
            if (armor == null) throw new ArgumentNullException(nameof(armor));
            ThrowExceptionIfPlayerIsDead();

            _armor.Add(armor);
        }

        public void DecreaseHealth(int value)
        {
            ThrowExceptionIfPlayerIsDead();
            Health = Math.Max(MinHealth, Health - value);

            if (IsDead)
            {
                _eventBus.Publish(new PlayerDiedEvent());
            }
        }

        public void IncreaseCoins(int numerOfCoins)
        {
            ThrowExceptionIfPlayerIsDead();
            Coins += numerOfCoins;
        }

        public void DecreaseCoins(int value)
        {
            ThrowExceptionIfPlayerIsDead();
            Coins = Math.Max(MinCoins, Coins - value);
        }

        public void IncreaseHealth(int value)
        {
            ThrowExceptionIfPlayerIsDead();
            Health = Math.Min(MaxHealth, Health + value);
        }

        private void ThrowExceptionIfPlayerIsDead()
        {
            if (IsDead)
            {
                throw new InvalidOperationException("the player is dead");
            }
        }
    }
}