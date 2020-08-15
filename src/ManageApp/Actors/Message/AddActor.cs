namespace ManageApp.Actors.Message
{
    public sealed class AddActor
    {
        public AddActor(int serverNumber, string organization, string color)
        {
            ServerNumber = serverNumber;
            Organization = organization;
            Color = color;
        }

        public int ServerNumber { get; }
        public string Organization { get; }
        public string Color { get; }
    }
}