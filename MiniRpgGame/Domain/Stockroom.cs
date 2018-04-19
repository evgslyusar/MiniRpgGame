using System;
using MiniRpgGame.Settings;

namespace MiniRpgGame.Domain
{
    public sealed class Stockroom : IStockroom
    {
        private readonly Random _random = new Random(); 
        
        private readonly IStockroomSetting _setting;

        public Stockroom(IStockroomSetting setting)
        {
            _setting = setting ?? throw new ArgumentNullException(nameof(setting));
        }

        public (int Cost, Weapon Weapons) TakeWeapons() => (
            _setting.CostOfWeapons,
            new Weapon(GetRandomValue(_setting.MinValueOfWeapons, _setting.MaxValueOfWeapons)));

        public (int Cost, Armor Armor) TakeArmor() => (
            GetRandomValue(_setting.MinCostOfArmor, _setting.MaxCostOfArmor),
            new Armor(GetRandomValue(_setting.MinValueOfArmor, _setting.MaxValueOfArmor)));

        private int GetRandomValue(int start, int end) => _random.Next(start, end + 1);
    }
}