using System;
using Microsoft.Extensions.Configuration;

namespace MiniRpgGame.Settings
{
    internal sealed class SettingsProvider :
        IBattleSettings,
        IHealerSettings,
        IGameplaySettings,
        IStockroomSetting
    {
        private readonly IConfiguration _configuration;

        public SettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #region IBattleSettings

        public int LossOfHealthRate => _configuration.GetValue<int>($"battle:{nameof(LossOfHealthRate)}");

        public int LossOfHealth => _configuration.GetValue<int>($"battle:{nameof(LossOfHealth)}");

        public int NumerOfCoins => _configuration.GetValue<int>($"battle:{nameof(NumerOfCoins)}");

        #endregion IBattleSettings

        #region IHealerSettings

        public int MaxHealth => _configuration.GetValue<int>($"healer:{nameof(MaxHealth)}");
        public int MinHealth => _configuration.GetValue<int>($"healer:{nameof(MinHealth)}");
        public int Cost => _configuration.GetValue<int>($"healer:{nameof(Cost)}");

        #endregion IHealerSettings

        #region IGameplaySettings

        public int InitialPower => _configuration.GetValue<int>($"gameplay:{nameof(InitialPower)}");
        public int InitialCoins => _configuration.GetValue<int>($"gameplay:{nameof(InitialCoins)}");
        public int InitialHealth => _configuration.GetValue<int>($"gameplay:{nameof(InitialHealth)}");
        public int InitialMaxHealth => _configuration.GetValue<int>($"gameplay:{nameof(InitialMaxHealth)}");

        #endregion IGameplaySettings

        #region IStockroomSetting

        public int CostOfWeapons => _configuration.GetValue<int>($"stockroom:{nameof(CostOfWeapons)}");
        public int MinCostOfArmor => _configuration.GetValue<int>($"stockroom:{nameof(MinCostOfArmor)}");
        public int MaxCostOfArmor => _configuration.GetValue<int>($"stockroom:{nameof(MaxCostOfArmor)}");
        public int MinValueOfArmor => _configuration.GetValue<int>($"stockroom:{nameof(MinValueOfArmor)}");
        public int MaxValueOfArmor => _configuration.GetValue<int>($"stockroom:{nameof(MaxValueOfArmor)}");
        public int MinValueOfWeapons => _configuration.GetValue<int>($"stockroom:{nameof(MinValueOfWeapons)}");
        public int MaxValueOfWeapons => _configuration.GetValue<int>($"stockroom:{nameof(MaxValueOfWeapons)}");

        #endregion IStockroomSetting
    }
}