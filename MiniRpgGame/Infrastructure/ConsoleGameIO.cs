using System;
using System.Linq;
using MiniRpgGame.Domain;

namespace MiniRpgGame.Infrastructure
{
    public sealed class ConsoleGameIO : IConsoleGameIO
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Display(Player player)
        {
            var playerState = $"MaxHealth: {player.MaxHealth} | " +
                              $"Health: {player.Health} | " +
                              $"Power: {player.Power} | " +
                              $"Coins: {player.Coins} | " +
                              $"Number Of Weapons: {player.Weapons.Count()} | " +
                              $"Number Of Armor: {player.Armor.Count()}";
            DisplayMessage(playerState);
        }

        public void DisplayError(string message)
        {
            var foregroundColor  = Console.ForegroundColor;
            var backgroundColor  = Console.BackgroundColor;
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            
            Console.WriteLine(message);
            
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        public void DisplayLine()
        {
            DisplayMessage("--------------------------------------------------------------------------------------------------------");
        }

        public ConsoleKey ReadKey(string message)
        {
            Console.Write(message);
            var pressed = Console.ReadKey();
            Console.WriteLine();
            return pressed.Key;
        }
    }
}