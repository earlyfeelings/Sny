namespace Sny.DB.Entities
{
    public class Task : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public Guid GoalId { get; set; }

        public virtual Goal Goal { get; set; }
    }
}
