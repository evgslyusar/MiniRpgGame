namespace MiniRpgGame.Domain.Events
{
    public sealed class MoveCompleteEvent
    {
        public bool Successful { get; }

        public string Message { get; }
    
        public MoveCompleteEvent(bool successful = true, string message = "")
        {
            Successful = successful;
            Message = message;
        }
    }
}