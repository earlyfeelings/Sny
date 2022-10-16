namespace Sny.DB.Entities
{
    public class Goal : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public bool IsCompleted { get; set; }

        public string Description { get; set; }

        public Guid AccountId { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<Task> Tasks { get; private set; }
    }
}
