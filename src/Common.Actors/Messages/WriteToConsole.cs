namespace Common.Actors.Messages
{
    public sealed class WriteToConsole
    {
        public IMessageToWrite Message { get; }

        public WriteToConsole(IMessageToWrite message)
        {
            Message = message;
        }

        public static WriteToConsole Info(string text) =>
            new WriteToConsole(new InfoMessage(text));

        public static WriteToConsole Success(string text) =>
            new WriteToConsole(new SuccessMessage(text));

        public static WriteToConsole Error(string text) =>
            new WriteToConsole(new ErrorMessage(text));
    }
}