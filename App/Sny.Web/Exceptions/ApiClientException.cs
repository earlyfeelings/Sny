using System.Runtime.Serialization;

namespace Sny.Web.Exceptions
{
    public class ApiClientException : ApplicationException
    {
        public ApiClientException()
        {
        }

        public ApiClientException(string msg) : base(msg)
        {
        }
    }
}
