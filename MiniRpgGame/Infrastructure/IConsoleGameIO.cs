using System;
using MiniRpgGame.Domain;

namespace MiniRpgGame.Infrastructure
{
    public interface IConsoleGameIO
    {
        void DisplayMessage(string message);
        void Display(Player gameplayPlayer);
        void DisplayError(string message);
        void DisplayLine();
        ConsoleKey ReadKey(string message);
    }
}