using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Exceptions
{
    public class FailedDatabaseConnectionException : Exception
    {
        public FailedDatabaseConnectionException()
        {
        }

        public FailedDatabaseConnectionException(string message)
        : base(message)
        {
        }

        public FailedDatabaseConnectionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
