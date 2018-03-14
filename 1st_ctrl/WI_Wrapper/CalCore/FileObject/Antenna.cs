using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    public class Antenna
    {
        public int antennaNum;
        public int waveformNum;
        public double rotation_x, rotation_y, rotation_z;
        public double power;
        public Antenna(string str)
        {
            int pos = 0;
            int linelength;
            linelength = Tool.readline(str, pos);
            antennaNum = Convert.ToInt32(str.Substring(pos, linelength).Substring(8));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            waveformNum = Convert.ToInt32(str.Substring(pos, linelength).Substring(9));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            rotation_x = Convert.ToDouble(str.Substring(pos, linelength).Substring(11));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            rotation_y = Convert.ToDouble(str.Substring(pos, linelength).Substring(11));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            rotation_z = Convert.ToDouble(str.Substring(pos, linelength).Substring(11));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            power = Convert.ToDouble(str.Substring(pos, linelength).Substring(6));
        }
    }
}
