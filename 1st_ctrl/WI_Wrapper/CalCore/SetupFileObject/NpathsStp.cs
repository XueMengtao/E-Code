using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
    class NpathsStp
    {
        int AutoScaling;
        int DrawValues;
        int AutoUpdating;
        int Discrete;
        int UseGlobalOpacity;
        int ManualValuesSet;
        int ClampedHigh;
        int ClampedLow;
        double Alpha;
        double ManualMin;
        double ManualMax;
        double[] TextColor;
        int Colors;
        double[,] col;
        int PartitionValues;
        void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            AutoScaling = Convert.ToInt32(str.Substring(pos, linelength).Substring(12));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            DrawValues = Convert.ToInt32(str.Substring(pos, linelength).Substring(11));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            AutoUpdating = Convert.ToInt32(str.Substring(pos, linelength).Substring(13));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            Discrete = Convert.ToInt32(str.Substring(pos, linelength).Substring(9));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            UseGlobalOpacity = Convert.ToInt32(str.Substring(pos, linelength).Substring(17));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            ManualValuesSet = Convert.ToInt32(str.Substring(pos, linelength).Substring(16));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            ClampedHigh = Convert.ToInt32(str.Substring(pos, linelength).Substring(12));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            ClampedLow = Convert.ToInt32(str.Substring(pos, linelength).Substring(11));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            Alpha = Convert.ToDouble(str.Substring(pos, linelength).Substring(6));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            ManualMin = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            ManualMax = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            pos += linelength + 2;
            pos += 10;
            TextColor = new double[3];
            int length;
            for (int i = 0; i < 3; i++)
            {
                length = SpaceFinding.read(str, pos);
                TextColor[i] = Convert.ToDouble(str.Substring(pos, length));
                pos += length + 1;
            }
            pos += 1;
            linelength = Tool.readline(str, pos);
            Colors = Convert.ToInt32(str.Substring(pos, linelength).Substring(7));

            col = new double[6, 3];
            int len = 0;
            int start = pos;//pos指向Colors 6行的起始位置
            pos += linelength + 2;
            for (int k = 0; k < 6; k++)//找出整个数组的长度
            {
                start += linelength + 2;
                linelength = Tool.readline(str, start);
                len += linelength + 2;
            }
            string temp = str.Substring(pos, len);
            start = 0;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 3; j++)
                {
                    length = SpaceFinding.read(temp, start);
                    col[i, j] = Convert.ToDouble(temp.Substring(start, length));
                    if (j == 2)
                        start += length + 2;
                    else
                        start += length + 1;
                }
            // pos += 1;
            pos += len;
            linelength = Tool.readline(str, pos);
            string temp1 = str.Substring(pos, linelength);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            PartitionValues = Convert.ToInt32(str.Substring(pos, linelength));
            int a = pos;
        }
         public NpathsStp(string str)
        {
            init(str);
        }
    }
}
