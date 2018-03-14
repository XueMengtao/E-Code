using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranmitterLib
{
    class DBException:Exception
    {
        public  DBException()
        {
        }
        public DBException(string message)
            : base(message)
        {
        }
    }
}
