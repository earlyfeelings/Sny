using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface ICurrentAccountContext
    {
        public Guid? CurrentAccountId { get; set; }

    }
}
