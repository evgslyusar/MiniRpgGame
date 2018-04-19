namespace MiniRpgGame.Settings
{
    public interface IBattleSettings
    {
        int LossOfHealthRate { get; }

        int LossOfHealth { get; }

        int NumerOfCoins { get; }
    }
}