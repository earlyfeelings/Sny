namespace Sny.Core.Goals
{
    public class Goal
    {
        public Goal(Guid id, string name, bool active, string description, Guid accountId)
        {
            Id = id;
            Name = name;
            Active = active;
            Description = description;
            AccountId = accountId;
        }        

        public Guid Id { get; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public string Description { get; set; }

        public Guid AccountId { get; }
    }
}
