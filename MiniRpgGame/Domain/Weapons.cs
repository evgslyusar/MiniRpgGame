using System;

namespace MiniRpgGame.Domain
{
    public sealed class Weapons
    {
        public Weapons(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}