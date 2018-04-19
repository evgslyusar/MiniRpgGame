namespace MiniRpgGame.Settings
{
    public interface IGameplaySettings
    {
        int InitialPower { get; }

        int InitialCoins { get; }

        int InitialHealth { get; }

        int InitialMaxHealth { get; }
    }
}