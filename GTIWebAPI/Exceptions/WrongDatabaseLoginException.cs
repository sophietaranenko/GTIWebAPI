using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Exceptions
{
    public class WrongDatabaseLoginException : Exception
    {
        public WrongDatabaseLoginException()
        {
        }

        public WrongDatabaseLoginException(string message)
        : base(message)
        {
        }

        public WrongDatabaseLoginException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
