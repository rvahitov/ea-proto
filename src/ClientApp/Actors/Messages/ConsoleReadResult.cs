namespace ClientApp.Actors.Messages
{
    public sealed class ConsoleReadResult
    {
        public ConsoleReadResult( string line )
        {
            Line = line;
        }

        public string Line { get; }
    }
}