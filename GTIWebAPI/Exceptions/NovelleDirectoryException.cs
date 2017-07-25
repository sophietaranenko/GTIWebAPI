using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Exceptions
{
    public class NovelleDirectoryException : Exception
    {
        public NovelleDirectoryException()
        {
        }

        public NovelleDirectoryException(string message)
        : base(message)
        {
        }

        public NovelleDirectoryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
