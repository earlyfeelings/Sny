using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.GoalsAggregate.Exceptions
{
    public class GoalNotFoundException : ApplicationException
    {
        public GoalNotFoundException() : base("Goal not found.")
        {
        }
    }
}
