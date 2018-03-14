using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    public class Location
    {
        public Reference Rf;
        public double? side1, side2, spacing;
        public double[] vertical = new double[3];//坐标位置
        public Location(string str)
        {
            int doublelength;
            string verticals;
            int pos = 20;
            int length = str.IndexOf("end_<reference>");
            Rf = new Reference(str.Substring(pos, length - pos - 1));
            pos = str.IndexOf("side1");
            if (pos == -1)
            {
                side1 = null;
                side2 = null;
                spacing = null;
                pos = str.IndexOf("nVertices");
            }
            else
            {
                length = Tool.readline(str, pos);
                side1 = Convert.ToDouble(str.Substring(pos, length).Substring(6));
                pos += str.Substring(pos, length).Length + 2;
                length = Tool.readline(str, pos);
                side2 = Convert.ToDouble(str.Substring(pos, length).Substring(6));
                pos += str.Substring(pos, length).Length + 2;
                length = Tool.readline(str, pos);
                spacing = Convert.ToDouble(str.Substring(pos, length).Substring(7));
                pos += str.Substring(pos, length).Length + 2;
            }
            length = Tool.readline(str, pos);
            pos += length + 2;
            length = Tool.readline(str, pos);
            verticals = str.Substring(pos, length);
            pos = 0;
            doublelength = Readdouble(verticals, pos);
            vertical[0] = Convert.ToDouble(verticals.Substring(pos, doublelength));
            pos += doublelength + 1;
            doublelength = Readdouble(verticals, pos);
            vertical[1] = Convert.ToDouble(verticals.Substring(pos, doublelength));
            pos += doublelength + 1;
            vertical[2] = Convert.ToDouble(verticals.Substring(pos));
        }
        public int Readdouble(string s, int pos)
        {
            int length = 0;
            while (s[pos + length] != ' ')
            {
                length++;
            }
            return length;
        }//方法函数
    }
}
