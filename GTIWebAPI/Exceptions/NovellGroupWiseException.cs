﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Exceptions
{
    public class NovellGroupWiseException : Exception
    {
        public NovellGroupWiseException()
        {
        }

        public NovellGroupWiseException(string message)
        : base(message)
        {
        }

        public NovellGroupWiseException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    public class NotOnPostOfficeException : Exception
    {
        public NotOnPostOfficeException()
        {
        }

        public NotOnPostOfficeException(string message)
        : base(message)
        {
        }

        public NotOnPostOfficeException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
