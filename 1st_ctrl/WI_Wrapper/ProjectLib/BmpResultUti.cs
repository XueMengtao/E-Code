using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using TranmitterLib;

namespace ProjectLib
{
    public interface IStructSerialize
    {
        void Serialize(BinaryWriter bw);
    }
    public class BmpResultUti
    {
        public static string CreateBMP(List<PointValue> pointValueList, string imagePath, double tempMax, double tempMin)
        {
            int r = 0, c = 0;
            double x, y, preX = 0, preY = 0;
            int insertPoint = 1;
            bool needInsertPoint = true;
            if (pointValueList.Count < 1000)
                insertPoint = 100;
            else if (pointValueList.Count < 10000)
                insertPoint = 10;
            else if (pointValueList.Count < 100000)
                insertPoint = 10;
            else
            {
                insertPoint = 0;
                needInsertPoint = false;
            }

            //int insertPoint = 1;
            int afterInterpR = 1;//还未初始化// r + (r - 1) * insertPoint;
            int afterInterpC = 1;//还未初始化 c + (c - 1) * insertPoint;
            HSL[,] hslArray = null;
            RgbColor[,] rgbArray = null;
            bool flagY = false;
            try
            {
                for (int it = 0; it < pointValueList.Count; ++it)//对结果点进行遍历
                {
                    if (it == 0)
                    {
                        preX = pointValueList[it].x;
                        preY = pointValueList[it].y;
                        c = 1; r = 1;
                        continue;
                    }
                    x = pointValueList[it].x;
                    y = pointValueList[it].y;
                    if (y == preY && !flagY)
                    { ++c; }
                    else
                    { flagY = true; }
                    if (x == preX)
                    { ++r; }
                }
                afterInterpR = r + (r - 1) * insertPoint;
                afterInterpC = c + (c - 1) * insertPoint;
                double[,] afterInterp = new double[afterInterpR, afterInterpC];
                //如果需要插值

                for (int i = 0; i < r; i++)
                {
                    int j = 0;
                    int tmpR = 0, tmpC = 0, addC = 1;

                    for (; j < c - 1; j++)
                    {
                        tmpR = i * (insertPoint + 1);
                        tmpC = j * (insertPoint + 1);
                        afterInterp[tmpR, tmpC] = GetValueFromList(pointValueList, r, c, i, j);
                        if (needInsertPoint)
                        {
                            //横插
                            double diffC = (GetValueFromList(pointValueList, r, c, i, j + 1) -
                                GetValueFromList(pointValueList, r, c, i, j)) / (insertPoint + 1);
                            addC = 1;
                            for (; addC <= insertPoint; ++addC)
                            {
                                afterInterp[tmpR, tmpC + addC] = afterInterp[tmpR, tmpC] + diffC * addC;
                            }
                        }
                    }
                    tmpR = i * (insertPoint + 1);
                    tmpC = j * (insertPoint + 1);
                    afterInterp[tmpR, tmpC] = GetValueFromList(pointValueList, r, c, i, j);
                }
                if (needInsertPoint)
                {
                    //纵插
                    for (int i = 0; i < r - 1; i++)
                    {
                        for (int j = 0; j < afterInterpC; j++)
                        {
                            int tmpR = i * (insertPoint + 1);
                            double diffR = (afterInterp[(i + 1) * (insertPoint + 1), j] - afterInterp[tmpR, j]) / (insertPoint + 1);
                            for (int addR = 1; addR <= insertPoint; ++addR)
                            {
                                afterInterp[tmpR + addR, j] = afterInterp[tmpR, j] + diffR * addR;
                            }

                        }
                    }
                }

                //转化为 HSL
                hslArray = new HSL[afterInterpR, afterInterpC];
                for (int i = 0; i < afterInterpR; i++)
                {
                    for (int j = 0; j < afterInterpC; j++)
                    {
                        hslArray[i, j] = new HSL();
                        hslArray[i, j].H = (afterInterp[i, j] * 240) / (360);

                    }
                }

                rgbArray = new RgbColor[afterInterpR, afterInterpC];
                for (int i = 0; i < afterInterpR; i++)
                {
                    for (int j = 0; j < afterInterpC; j++)
                    {
                        rgbArray[i, j] = Hsl2Rgb(hslArray[i, j].H);

                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                //LogFileManager.ObjLog.info(e.Message);

            }
            catch (Exception e)
            {
                //LogFileManager.ObjLog.info(e.Message);
            }
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                fs = new FileStream(imagePath, FileMode.Create | FileMode.Append, FileAccess.Write, FileShare.None);
                bw = new BinaryWriter(fs);

                BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
                ConstructBih(afterInterpC, afterInterpR, ref bih);

                BITMAPFILEHEADER bhh = new BITMAPFILEHEADER();
                ContructBhh(afterInterpC, afterInterpR, ref bhh);
                bhh.bfSize = bih.biSizeImage + 54;
                bhh.bfOffBits = 54;

                //fwrite(&bhh,1,sizeof(BITMAPFILEHEADER),fpw);
                //fwrite(&bih,1,sizeof(BITMAPINFOHEADER),fpw);
                bhh.Serialize(bw);
                bih.Serialize(bw);

                //保存像素数据  
                //左->右 下->上 
                int index = 0;
                for (int i = 0; i <= afterInterpR - 1; ++i)
                {

                    for (int j = 0; j < afterInterpC; ++j)
                    {
                        index += 3;
                        byte red, green, blue;
                        red = (byte)rgbArray[i, j].red;
                        green = (byte)rgbArray[i, j].green;
                        blue = (byte)rgbArray[i, j].blue;
                        //注意三条语句的顺序：否则颜色会发生变化 
                        bw.Write(red);
                        bw.Write(green);
                        bw.Write(blue);

                        //fwrite(&r, 1, sizeof(BYTE), fpw);
                        //fwrite(&g, 1, sizeof(BYTE), fpw);
                        //fwrite(&b, 1, sizeof(BYTE), fpw);
                        int len = afterInterpC % 4;
                        if ((len != 0) && (index % (afterInterpC * 3) == 0))
                        {

                            byte[] blank = new byte[len];
                            for (int b = 0; b < len; ++b)
                            {
                                blank[b] = 0;
                                bw.Write(blank[b]);
                                //sw.Write(blank[b].ToString());
                            }
                            //fwrite(blank, sizeof(BYTE), len, fpw);
                        }
                    }
                }
                bw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                //if(sw != null)
                //    sw.Close();
                if (fs != null)
                    fs.Close();
                //LogFileManager.ObjLog.info(e.Message);
            }
            return imagePath;
        }



        //从链表中获取指定位置数据
        static PointValue GetEleFromList(List<PointValue> l, int r, int c, int i, int j)
        {
            if (l.Count <= c * i + j)
                return null;
            return l[c * i + j];
        }

        static double GetValueFromList(List<PointValue> l, int r, int c, int i, int j)
        {
            PointValue p = GetEleFromList(l, r, c, i, j);
            if (p == null)
                return 0;
            double value = p.value;
            if (value < 0)
            {
                value = Math.Pow(10, value / 10) * 0.001;
            }
            return value;
        }

        static public RgbColor Hsl2Rgb(double h)//h范围为0~1
        {
            double[] T = new double[3];
            double R, G, B;
            T[0] = h + 1.0 / 3.0;
            T[1] = h;
            T[2] = h - 1.0 / 3.0;
            for (int i = 0; i < 3; i++)
            {
                if (T[i] < 0) T[i] += 1.0;
                if (T[i] > 1) T[i] -= 1.0;
                if ((T[i] * 6) < 1)
                {
                    T[i] = 6 * T[i];
                }
                else if ((T[i] * 2.0) < 1)
                {
                    T[i] = 1;
                }
                else if ((T[i] * 3.0) < 2)
                {
                    T[i] = 4 - 6 * T[i];
                }
                else T[i] = 0.0;
            }
            R = T[0] * 255.0f;
            G = T[1] * 255.0f;
            B = T[2] * 255.0f;

            byte R1 = (byte)((R > 255) ? 255 : ((R < 0) ? 0 : R));
            byte G1 = (byte)((G > 255) ? 255 : ((G < 0) ? 0 : G));
            byte B1 = (byte)((B > 255) ? 255 : ((B < 0) ? 0 : B));
            return new RgbColor(R1, G1, B1);
        }


        //The structure that will hold the RGB Values
        public class RgbColor
        {
            public RgbColor(byte r, byte g, byte b)
            {
                red = r;
                green = g;
                blue = b;
            }
            public byte red;
            public byte green;
            public byte blue;
        }
        class HSL
        {
            public double H;
            //public double S;
            //public double L;
        }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public class BITMAPFILEHEADER : IStructSerialize //文件头
        {
            public int bfType;              //文件类型字段("BM"即=19778)
            public long bfSize;               // '文件的大小
            public int bfReserved1, bfReserved2;//保留(=0)
            public long bfOffBits;                   //第一个像素的偏移量
            public void Serialize(BinaryWriter bw)
            {
                try
                {
                    //System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //formatter.Serialize(stream, obj);

                    bw.Write(BitConverter.GetBytes(this.bfType), 0, 2);
                    bw.Write(BitConverter.GetBytes(this.bfSize), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.bfReserved1), 0, 2);
                    bw.Write(BitConverter.GetBytes(this.bfReserved2), 0, 2);
                    bw.Write(BitConverter.GetBytes(this.bfOffBits), 0, 4);
                }
                catch (Exception e)
                {
                    //if(stream != null)
                    //    stream.Close();
                    //LogFileManager.ObjLog.info(e.Message);
                }
            }
        }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public class BITMAPINFOHEADER : IStructSerialize
        {
            public int biSize;
            public long biWidth;
            public long biHeight;
            public int biPlanes;
            public int biBitCount;
            public int biCompression;
            public int biSizeImage;
            public long biXPelsPerMeter;
            public long biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
            public void Serialize(BinaryWriter bw)
            {
                try
                {
                    //System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //formatter.Serialize(stream, obj);
                    bw.Write(BitConverter.GetBytes(this.biSize), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biWidth), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biHeight), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biPlanes), 0, 2);
                    bw.Write(BitConverter.GetBytes(this.biBitCount), 0, 2);
                    bw.Write(BitConverter.GetBytes(this.biCompression), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biSizeImage), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biXPelsPerMeter), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biYPelsPerMeter), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biClrUsed), 0, 4);
                    bw.Write(BitConverter.GetBytes(this.biClrImportant), 0, 4);
                }
                catch (Exception e)
                {
                    //if(stream != null)
                    //    stream.Close();
                    //LogFileManager.ObjLog.info(e.Message);
                }
            }

        }

        //构建BMP位图文件头  
        static void ContructBhh(int nWidth, int nHeight, ref BITMAPFILEHEADER bhh)
        {
            //int widthStep = (((nWidth * 24) + 31) & (~31)) / 8 ; //每行实际占用的大小（每行都被填充到一个4字节边界）   
            //bhh.bfSize = (DWORD)sizeof(BITMAPFILEHEADER) + (DWORD)sizeof(BITMAPINFOHEADER) + widthStep * nHeight;  
            bhh.bfType = 0x4d42;
            bhh.bfReserved1 = 0;
            bhh.bfReserved2 = 0;
            //bhh.bfOffBits = (int)sizeof(BITMAPFILEHEADER) + (int)sizeof(BITMAPINFOHEADER);
            bhh.bfOffBits = System.Runtime.InteropServices.Marshal.SizeOf(new BITMAPFILEHEADER()) +
                System.Runtime.InteropServices.Marshal.SizeOf(new BITMAPINFOHEADER());

        }
        //构建BMP文件信息头  
        static void ConstructBih(int nWidth, int nHeight, ref BITMAPINFOHEADER bih)
        {
            /* int widthStep = (((nWidth * 24) + 31) & (~31)) / 8 ;*/

            bih.biSize = 40;       // header size  
            bih.biWidth = nWidth;
            bih.biHeight = nHeight;
            bih.biPlanes = 1;
            bih.biBitCount = 24;     // RGB encoded, 24 bit  
            bih.biCompression = 0;   // no compression 非压缩  
            /* bih.biSizeImage=widthStep*nHeight*3; */
            //int lineByteCount = (((nWidth*3)+3)>>2)<<2;
            int lineByteCount = (nWidth * 24 / 8 + 3) / 4 * 4;
            bih.biSizeImage = lineByteCount * nHeight;

            bih.biXPelsPerMeter = 0;
            bih.biYPelsPerMeter = 0;
            bih.biClrUsed = 0;
            bih.biClrImportant = 0;
        }
    }
}
