using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    public class Tool
    {
        public static int readline(string s, int start)
        {
            int length = 0;
            while (s[start + length] != '\r')
                length++;
            return length;
        }
    }
}
