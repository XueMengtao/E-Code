using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    public class RxObject
    {
        public string name;
        public int num;
        public string active;
        public bool vertical_line;
        public double cubesize, CVxLength, CVyLength, CVzLength;
        public string AutoPatternScale;
        public bool ShowDescription, CVsVisible;
        public int CVsThickness;
        public Location Lc;
        public double? rotation;
        public Antenna at;
        public Sbr sb;
        public double NoiseFigure;
        public bool pattern_show_arrow, pattern_show_as_sphere, generate_p2p;
        public String flag;
        public RxObject(string str)
        {
            int start;
            int linelength;
            linelength = Tool.readline(str, 1);
            name = str.Substring(1, linelength);
            start = str.IndexOf("RxSet");
            linelength = Tool.readline(str, start);
            num =Convert.ToInt32( str.Substring(start, linelength).Substring(6));
            start = str.IndexOf("active");
            linelength = Tool.readline(str, start);
            active = str.Substring(start, linelength);
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            vertical_line = (str.Substring(start, linelength).Substring(14) == "yes");
            start = str.IndexOf("cube_size");
            if(start==-1)
            {
                cubesize = 0;
            }
            else
            {
                linelength = Tool.readline(str, start);
                cubesize =Convert.ToDouble(str.Substring(start, linelength).Substring(10));
            }
            start = str.IndexOf("CVxLength");
            linelength = Tool.readline(str, start);
            CVxLength = Convert.ToDouble(str.Substring(start, linelength).Substring(10));
            start = str.IndexOf("CVyLength");
            linelength = Tool.readline(str, start);
            CVyLength = Convert.ToDouble(str.Substring(start, linelength).Substring(10));
            start = str.IndexOf("CVzLength");
            linelength = Tool.readline(str, start);
            CVzLength = Convert.ToDouble(str.Substring(start, linelength).Substring(10));
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            AutoPatternScale = str.Substring(start, linelength);
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            ShowDescription = (str.Substring(start, linelength).Substring(16) == "yes");
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            CVsVisible = (str.Substring(start, linelength).Substring(11) == "yes");
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            CVsThickness =Convert.ToInt32(str.Substring(start, linelength).Substring(13));
                //str.Substring(start, str.IndexOf("begin_<location>") - start - 2);
            start = str.IndexOf("begin_<location>");
            Lc=new Location(str.Substring(start+19,str.IndexOf("end_<location>")-start-20));
            start=str.IndexOf("rotation ");
            if(start==-1)rotation=null;
            else 
            {
                linelength=Tool.readline(str, start);
                rotation=Convert.ToDouble( str.Substring(start,linelength).Substring(9));
            }
            start = str.IndexOf("begin_<antenna>");
            at=new Antenna(str.Substring(start+18,str.IndexOf("end_<antenna>")-start-19));
            start = str.IndexOf("begin_<sbr>");
            if (start == -1) sb = null;
            else
            {
                sb = new Sbr(str.Substring(start+14, str.IndexOf("end_<sbr>")-start-16));
            }
            start = str.IndexOf("NoiseFigure");
            linelength = Tool.readline(str, start);
            NoiseFigure = Convert.ToDouble(str.Substring(start, linelength).Substring(12));
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            pattern_show_arrow = (str.Substring(start, linelength).Substring(19) == "yes");
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            pattern_show_as_sphere = (str.Substring(start, linelength).Substring(23) == "yes");
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            generate_p2p = (str.Substring(start, linelength).Substring(13) == "yes");
            start += linelength + 2;
            linelength = Tool.readline(str, start);
            string[] test = str.Substring(start, linelength).Split('_');
            flag = test[1];
        }
    }
}
