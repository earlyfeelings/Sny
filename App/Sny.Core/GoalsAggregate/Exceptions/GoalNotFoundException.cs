namespace Sny.Core.GoalsAggregate.Exceptions
{
    public class GoalNotFoundException : ApplicationException
    {
        public GoalNotFoundException() : base("Goal not found.")
        {
        }
    }
}
