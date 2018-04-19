namespace MiniRpgGame.Settings
{
    public interface IHealerSettings
    {
        int MaxHealth { get; }

        int MinHealth { get; }

        int Cost { get; }
    }
}