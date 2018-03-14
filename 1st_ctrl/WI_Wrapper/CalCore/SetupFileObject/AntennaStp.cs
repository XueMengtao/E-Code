using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
    public class AntennaStp
    {
        //const string begin = "begin_<antenna>";
        // public const string end = "end_<antenna>\r\n";
        //所有天线都有的字段
        public string name;
        public int antenna;
        public string type;
        public double? power_threshold;
        public double? cable_loss;
        public double? VSWR;
        public double? temperature;
        public string component;
        public double? gain_range;
        public bool? show_arrow;
        public bool? is_sphere;
        //部分天线有的字段
        public int waveform;
        public string polarization;
        public double radius;
        public double length;
        public double blockageradius;
        public string EFieldDistribution;
        public double EdgeTaper;
        public double pitch;
        public string uan_path;
        public void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            name = str.Substring(pos, linelength).Substring(1);
            pos = str.IndexOf("antenna ");
            linelength = Tool.readline(str, pos);
            antenna = Convert.ToInt32(str.Substring(pos, linelength).Substring(8));
            pos = str.IndexOf("type ");
            linelength = Tool.readline(str, pos);
            type = str.Substring(pos, linelength).Substring(5);
            pos = str.IndexOf("power_threshold ");
            if (pos == -1) power_threshold = null;
            else
            {
                linelength = Tool.readline(str, pos);
                power_threshold = Convert.ToDouble(str.Substring(pos, linelength).Substring(16));
            }
            pos = str.IndexOf("cable_loss ");
            if (pos == -1) cable_loss = null;
            else
            {
                linelength = Tool.readline(str, pos);
                cable_loss = Convert.ToDouble(str.Substring(pos, linelength).Substring(11));
            }
            pos = str.IndexOf("VSWR ");
            if (pos == -1) VSWR = null;
            else
            {
                linelength = Tool.readline(str, pos);
                VSWR = Convert.ToDouble(str.Substring(pos, linelength).Substring(5));
            }
            pos = str.IndexOf("temperature ");
            if (pos == -1) temperature = null;
            else
            {
                linelength = Tool.readline(str, pos);
                temperature = Convert.ToDouble(str.Substring(pos, linelength).Substring(12));
            }
            pos = str.IndexOf("component ");
            if (pos == -1) component = null;
            else
            {
                linelength = Tool.readline(str, pos);
                component = str.Substring(pos, linelength).Substring(10);
            }
            pos = str.IndexOf("gain_range");
            if (pos == -1) gain_range = null;
            else
            {
                linelength = Tool.readline(str, pos);
                gain_range = Convert.ToDouble(str.Substring(pos, linelength).Substring(11));
            }
            pos = str.IndexOf("show_arrow ");
            if (pos == -1) show_arrow = null;
            else
            {
                linelength = Tool.readline(str, pos);
                show_arrow = Convert.ToBoolean(str.Substring(pos, linelength).Substring(11) == "yes");
            }
            pos = str.IndexOf("is_sphere ");
            if (pos == -1) is_sphere = null;
            else
            {
                linelength = Tool.readline(str, pos);
                is_sphere = Convert.ToBoolean(str.Substring(pos, linelength).Substring(10) == "yes");
            }
            pos = str.IndexOf("waveform ");
            if (pos == -1) waveform = -1;
            else
            {
                linelength = Tool.readline(str, pos);
                waveform = Convert.ToInt32(str.Substring(pos, linelength).Substring(9));
            }
            pos = str.IndexOf("polarization ");
            if (pos == -1) polarization = null;
            else
            {
                linelength = Tool.readline(str, pos);
                polarization = str.Substring(pos, linelength).Substring(13);
            }
            pos = str.IndexOf("radius ");
            if (pos == -1) radius = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                radius = Convert.ToDouble(str.Substring(pos, linelength).Substring(7));
            }
            pos = str.IndexOf("length ");
            if (pos == -1) length = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                length = Convert.ToDouble(str.Substring(pos, linelength).Substring(7));
            }
            pos = str.IndexOf("blockageradius ");
            if (pos == -1) blockageradius = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                blockageradius = Convert.ToDouble(str.Substring(pos, linelength).Substring(15));
            }
            pos = str.IndexOf("EFieldDistribution ");
            if (pos == -1) EFieldDistribution = null;
            else
            {
                linelength = Tool.readline(str, pos);
                EFieldDistribution = str.Substring(pos, linelength).Substring(19);
            }
            pos = str.IndexOf("EdgeTaper ");
            if (pos == -1) EdgeTaper = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                EdgeTaper = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            }
            pos = str.IndexOf("pitch ");
            if (pos == -1) pitch = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                pitch = Convert.ToDouble(str.Substring(pos, linelength).Substring(6));
            }
            pos = str.IndexOf("uan_path");
            if (pos == -1) uan_path = null;
            else
            {
                linelength = Tool.readline(str, pos);
                uan_path = str.Substring(pos, linelength).Substring(9);
            }
        }
        public AntennaStp(string str)
        {
            init(str);
        }
    }
        public class TotalAntenna
        {
            string[] antennaseperator = new string[] { "begin_<antenna>" };
            string[] result = null;
            public List <AntennaStp> allAntennas;
            List<AntennaStp> GetAntenna(string str)
            {
                List<AntennaStp> anStp = new List<AntennaStp>();
                result = str.Split(antennaseperator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < result.Length; i++)
                {
                    AntennaStp an = new AntennaStp(result[i]);
                    anStp.Add(an);
                }
                return anStp;
            }
            public TotalAntenna(string str)
            {
                allAntennas = GetAntenna(str);
            }




        }
    
}
