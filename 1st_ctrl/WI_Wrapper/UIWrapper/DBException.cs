using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF1
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
