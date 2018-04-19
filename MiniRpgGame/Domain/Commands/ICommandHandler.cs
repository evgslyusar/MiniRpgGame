namespace MiniRpgGame.Domain.Commands
{
    public interface ICommandHandler<in T> where T : Command
    {
        void Handle(T command);
    }
}