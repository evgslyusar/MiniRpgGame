using System;

namespace MiniRpgGame.Domain
{
    public sealed class DefaultBattleLogic : IBattleLogic
    {
        private readonly Random _random = new Random();
        
        public bool IsWon(int power) => _random.NextDouble() <= CalculateProbabilityOfVictory(power);
        
        private double CalculateProbabilityOfVictory(int power) => Math.Min(0.4 + power * 0.05, 0.7);
    }
}