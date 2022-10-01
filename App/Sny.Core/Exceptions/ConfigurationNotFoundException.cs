using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.Exceptions
{
    public class ConfigurationNotFoundException : ApplicationException
    {
        public ConfigurationNotFoundException(string key) : base($"Configuration '{key}' not found.")
        {
        }
    }
}
