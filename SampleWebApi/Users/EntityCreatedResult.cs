namespace SampleWebApi.Users
{
    public class EntityCreatedResult
    {
        public EntityCreatedResult(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}