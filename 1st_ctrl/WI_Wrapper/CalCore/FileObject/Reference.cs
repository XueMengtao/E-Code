using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    public class Reference
    {
        public string coordinate;
        public double longitude;
        public double latitude;
        public bool visible;
        public string sealevel;
        public Reference(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            coordinate = str.Substring(pos, linelength);
            pos += coordinate.Length + 2;
            linelength = Tool.readline(str, pos);
            longitude = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            latitude = Convert.ToDouble(str.Substring(pos, linelength).Substring(9));
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            visible = Convert.ToBoolean(str.Substring(pos, linelength).Substring(8) == "yes");
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            sealevel = str.Substring(pos, linelength);
        }
    }
}
