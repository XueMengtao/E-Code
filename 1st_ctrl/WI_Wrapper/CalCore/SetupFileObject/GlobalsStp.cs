using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
   class GlobalsStp
    {
        string offset_mode;
        public double longitude;
        public double latitude;
        void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            offset_mode = str.Substring(pos, linelength).Substring(12);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            longitude = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            latitude = Convert.ToDouble(str.Substring(pos, linelength).Substring(9));
        }
         public GlobalsStp(string str)
        {
            init(str);
        }
    }
}
