namespace MiniRpgGame.Domain
{
    public sealed class Weapon
    {
        public Weapon(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}