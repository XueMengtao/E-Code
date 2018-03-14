using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
   class FeatureStp
    {
        int feature;
        string sealevel;
        string active;
        string filename;
        void init(string str)
        {
            int pos = 0;
            int linelenth = Tool.readline(str, pos);
            feature = Convert.ToInt32(str.Substring(pos, linelenth).Substring(8));
            pos += linelenth + 2;
            linelenth = Tool.readline(str, pos);
            sealevel = str.Substring(pos, linelenth);
            pos += linelenth + 2;
            linelenth = Tool.readline(str, pos);
            active = str.Substring(pos, linelenth);
            pos += linelenth + 2;
            linelenth = Tool.readline(str, pos);
            filename = str.Substring(pos, linelenth).Substring(9);
        }
         public FeatureStp(string str)
        {
            init(str);
        }
    }
}
