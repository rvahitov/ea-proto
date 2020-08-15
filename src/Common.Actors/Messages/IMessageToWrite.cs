namespace Common.Actors.Messages
{
    public interface IMessageToWrite
    {
        string Text { get; }
    }

    public sealed class SuccessMessage : IMessageToWrite
    {
        public SuccessMessage(string message)
        {
            Text = message;
        }

        public string Text { get; }
    }

    public sealed class InfoMessage : IMessageToWrite
    {
        public InfoMessage(string message)
        {
            Text = message;
        }

        public string Text { get; }
    }

    public sealed class ErrorMessage : IMessageToWrite
    {
        public ErrorMessage(string message)
        {
            Text = message;
        }

        public string Text { get; }
    }
}