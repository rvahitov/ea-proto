namespace ManageApp.Actors.Message
{
    public sealed class ParseCommand
    {
        public ParseCommand(string commandText)
        {
            CommandText = commandText;
        }

        public string CommandText { get; }
    }
}