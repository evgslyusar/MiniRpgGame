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

        public (int Cost, Weapons Weapons) TakeWeapons() => (
            _setting.CostOfWeapons,
            new Weapons(GetValue(_setting.MinValueOfWeapons, _setting.MaxValueOfWeapons)));

        public (int Cost, Armor Armor) TakeArmor() => (
            GetValue(_setting.MinCostOfArmor, _setting.MaxCostOfArmor),
            new Armor(GetValue(_setting.MinValueOfArmor, _setting.MaxValueOfArmor)));

        private int GetValue(int start, int end) => _random.Next(start, end + 1);
    }
}