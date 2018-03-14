using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF1
{
    class WriteConst
    {
        public static string Write(string msg)
        {
            string conString = "begin_<AntennaGain>\r\nAutoScaling 1\r\nDrawValues 0\r\nAutoUpdating 1\r\nDiscrete 0\r\nUseGlobalOpacity 1\r\nManualValuesSet 0\r\nClampedHigh 1\r\nClampedLow 1\r\nAlpha 1.000e+000\r\nManualMin 0.000e+000\r\nManualMax 1.000e+000\r\nTextColor 1.000 1.000 1.000\r\nColors 6\r\n0.300 0.000 0.500\r\n0.000 0.000 1.000\r\n0.000 1.000 0.000\r\n1.000 1.000 0.000\r\n1.000 0.500 0.000\r\n1.000 0.000 0.000\r\nPartitionValues\r\n0\r\nend_<AntennaGain>\r\n";
            if (msg.LastIndexOf("end_<AntennaGain>\r\n") == -1)
            {
                int AntanaGain = msg.LastIndexOf("end_<NPaths>");
                msg = msg.Insert(AntanaGain + 14, conString);
            }
            return msg;
        }

    }
}
