using MiniRpgGame.Domain;

namespace MiniRpgGame.Infrastructure
{
    public interface IBot
    {
        void MakeMove(Gameplay gameplay);
    }
}