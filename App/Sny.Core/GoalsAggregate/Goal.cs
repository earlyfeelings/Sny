namespace Sny.Core.Goals
{
    public class Goal
    {
        public Goal(Guid id, string name, bool active, string description)
        {
            Id = id;
            Name = name;
            Active = active;
            Description = description;
        }        
        

        public Guid Id { get; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public string Description { get; set; }
    }
}
