using System;

namespace MiniRpgGame.Domain
{
    public sealed class CommandResult
    {
        private CommandResult(Type type, bool successful, string message = "")
        {
            Type = type;
            Successful = successful;
            Message = message;
        }

        public Type Type { get; }
            
        public bool Successful { get; }

        public string Message { get; }

        public static CommandResult Ok<TCommand>(TCommand command) => new CommandResult(command.GetType(), true);

        public static CommandResult Fail<TCommand>(TCommand command, string message) => new CommandResult(command.GetType(), false, message);
    }
}