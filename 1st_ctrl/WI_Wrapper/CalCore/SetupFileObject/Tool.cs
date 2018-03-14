using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
    class Tool
    {
      public static int readline(string s, int start)
        {
            int length = 0;
            while (s[start + length] != '\r')
                length++;
            return length;
        }
    }
    class SpaceFinding
    {
        public static int read(string s, int start)
        {
            int length = 0;
            while ((s[start + length] != ' ') && (s[start + length] != '\r'))
                length++;
            return length;
        }
    }
    class BoolJudeg
    {
        public static string judge(bool b)
        {
            if (b)
                return "yes\r\n";
            else
                return "no\r\n";
        }
    }
   class GetTypeOfAntenna
    {
        public static string type(string s)
        {
            string type;
            int pos = s.IndexOf("type");
            int linelength = Tool.readline(s, pos);
            type = s.Substring(pos, linelength).Substring(5);
            return type;
        }
    }
    class GetTypeOfWaveform
    {
        public static string type(string s)
        {
            string type;
            int pos = 0;
            int linelength = Tool.readline(s, pos);
            pos += linelength + 2;
            linelength = Tool.readline(s, pos);
            type = s.Substring(pos, linelength);
            return type;
        }
    }
}
