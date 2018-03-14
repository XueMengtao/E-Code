using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
    class ScalesStp
    {
        NpathsStp np;
        BERStp be;
        ThroughputStp trp;
        void init(string str)
        {
            int pos = str.IndexOf("begin_<NPaths>");
            int linelength = Tool.readline(str, pos);
            np = new NpathsStp(str.Substring(pos + linelength + 2, str.IndexOf("end_<NPaths>") - pos - linelength - 2));
            pos = str.IndexOf("begin_<BER>");
            linelength = Tool.readline(str, pos);
            be = new BERStp(str.Substring(pos + linelength + 2, str.IndexOf("end_<BER>") - pos - linelength - 2));
            pos = str.IndexOf("begin_<Throughput>");
            linelength = Tool.readline(str, pos);
            trp = new ThroughputStp(str.Substring(pos + linelength + 2, str.IndexOf("end_<Throughput>") - pos - linelength - 2));
        }
         public ScalesStp(string str)
        {
            init(str);
        }
    }
}
