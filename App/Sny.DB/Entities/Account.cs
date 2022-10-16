namespace Sny.DB.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Goal> Goals { get; private set; }
    }
}
