using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectLib
{
    public class WrongFileException:ApplicationException
    {
        public WrongFileException(string message):base(message)
        { }
        public WrongFileException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class LackOfFileContentException : ApplicationException
    {
        public LackOfFileContentException(string message)
            : base(message)
        { }
        public LackOfFileContentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class LackOfSomeTypeFileException : ApplicationException
    {
        public LackOfSomeTypeFileException(string message)
            : base(message)
        { }
        public LackOfSomeTypeFileException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
