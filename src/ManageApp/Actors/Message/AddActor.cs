namespace ManageApp.Actors.Message
{
    public sealed class AddActor
    {
        public AddActor(int serverNumber, string organization, string color)
        {
            ServerNumber = serverNumber;
        }

        public int ServerNumber { get; }
    }
}