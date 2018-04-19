using Microsoft.Extensions.Configuration;

namespace MiniRpgGame.Settings
{
    public interface IStockroomSetting
    {
        int CostOfWeapons { get; }

        int MinCostOfArmor { get; }

        int MaxCostOfArmor { get; }
        
        int MinValueOfArmor { get; }

        int MaxValueOfArmor { get; }
        
        int MinValueOfWeapons { get; }

        int MaxValueOfWeapons { get; }

    }
}