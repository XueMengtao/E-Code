using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
   public class ReceiverStp
    {
        public string filename;
        public int FirstAvailableRxNumber;
        public void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            filename = str.Substring(pos, linelength).Substring(9);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            FirstAvailableRxNumber = Convert.ToInt32(str.Substring(pos, linelength).Substring(23));
        }
         public ReceiverStp(string str)
        {
            init(str);
        }
    }
}
