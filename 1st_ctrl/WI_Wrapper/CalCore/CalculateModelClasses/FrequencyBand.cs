using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///每段频段的信息
    /// </summary>
    public class FrequencyBand
    {
        private int midPointFrequence;
        private double power;
        private int frequenceWidth;
        private int frequenceWidthMin;
        private int frequenceWidthMax;
        private double proportion;
        /// <summary>
        ///中点频率
        /// </summary>
        public int MidPointFrequence
        {
            get { return midPointFrequence; }
            set { midPointFrequence = value; }
        }

        /// <summary>
        ///该频段的功率
        /// </summary>
        public double Power
        {
            get { return power; }
            set { power = value; }
        }

        /// <summary>
        ///频段宽度
        /// </summary>
        public int FrequenceWidth
        {
            get { return frequenceWidth; }
            set { frequenceWidth = value; }
        }

        /// <summary>
        ///频段的左端点
        /// </summary>
        public int FrequenceWidthMin
        {
            get { return frequenceWidthMin; }
            set { frequenceWidthMin = value; }
        }

        /// <summary>
        ///频段的右端点
        /// </summary>
        public int FrequenceWidthMax
        {
            get { return frequenceWidthMax; }
            set { frequenceWidthMax = value; }
        }
        public double Proportion
        {
            get { return this.proportion; }
            set { this.proportion = value; }
        }


        public FrequencyBand()
        { 

        }


        public FrequencyBand(int frequencewidthmin, int frequencewidthmax, double txpower)
        {
            frequenceWidthMax = frequencewidthmax;
            frequenceWidthMin = frequencewidthmin;
            midPointFrequence = (frequencewidthmin + frequencewidthmax) / 2;
            frequenceWidth = frequencewidthmax - frequencewidthmin;
            power = txpower;
        }
    }

    /// <summary>
    ///分频类
    /// </summary>
    public class DivFrequency
    {
        private FrequencyBand[] frequencyBand;

        public long[] startFre;
        public long[] endFre;
        public double[] proportion;
        public FrequencyBand[] FrequencyBand
        {
            get { return this.frequencyBand; }
            set { this.frequencyBand = value; }
        }
        //构造函数

        public DivFrequency()
        {
            startFre = new long[7];
            endFre = new long[7];
            proportion = new double[7];
            this.frequencyBand = new FrequencyBand[7];
        }

        /// <summary>
        ///获得最小频率
        /// </summary>
        /// <param name="txFrequencyBand">发射机频段信息</param>
        /// <returns>返回发射机最小频率</returns>
        public  int GetMinFrequenceFromList(FrequencyBand[] txFrequencyBand)
        {
            int minFrequence = txFrequencyBand[0].MidPointFrequence;
            for (int i = 1; i < txFrequencyBand.Length - 1; i++)
            {
                if (minFrequence >= txFrequencyBand[i].MidPointFrequence)
                { minFrequence = txFrequencyBand[i].MidPointFrequence; }
            }
            return minFrequence;
        }

        /// <summary>
        ///获得最大频率
        /// </summary>
        /// <param name="txFrequencyBand">发射机频段信息</param>
        /// <returns>返回发射机最大频率</returns>
        public  int GetMaxFrequenceFromList(FrequencyBand[] txFrequencyBand)
        {
            int maxFrequence = txFrequencyBand[0].MidPointFrequence;
            for (int i = 1; i < txFrequencyBand.Length - 1; i++)
            {
                if (maxFrequence <= txFrequencyBand[i].MidPointFrequence)
                { maxFrequence = txFrequencyBand[i].MidPointFrequence; }
            }
            return maxFrequence;
        }
    }

    /// <summary>
    ///分频计算类
    /// </summary>
    public class psd
    {
        private int N = 1024;     //采集的数据点数，注意为2的n次方
        private int n = 10;       //2^n=N

        public Plural[] input;//原始输入信号
        public Plural[] IO;//经过重新排序的信号
        public double[] psddata;//存放功率谱

        public int[] code;//0～（2^n－1）的倒置码自然数组

        public Plural[] outdata;//输出变换的结果（复数）
        public Plural[] temp;//保存中间变换结果（复数）

        public long carrierFrequency;//载波频率
        public long maxFrequency;//最大频率
        public long samplingFrequency;//内奎斯特抽样频率
        public Plural Wtemp;
        public double totalpsd;//功率谱总积分
        public double frequencyspace;//最小频率间隔

        public psd(string WaveformType, double PulseWidth, long CarrierFrequency, double Rolloff, string FrequencyVariation, double StartFrequency, double StopFrequency)//(波形函数，载波频率，最大频率)
        {
            input = new Plural[1024];
            IO = new Plural[1024];
            psddata = new double[1024];
            code = new int[1024];
            outdata = new Plural[1024];
            temp = new Plural[1024];
            for (int k = 0; k < N; k++)
            {
                input[k] = new Plural(0, 0);
                IO[k] = new Plural(0, 0);
                outdata[k] = new Plural(0, 0);
                temp[k] = new Plural(0, 0);
            }
            Wtemp = new Plural(0, 0);

            if (WaveformType == "Chirp")
            {
                if ((FrequencyVariation != "Linear" || FrequencyVariation != "exponential") || (StartFrequency < 0) || (StopFrequency < 0) || (StartFrequency == StopFrequency) || (Rolloff > 1) || (Rolloff < 0))
                { throw new Exception("Chrip波形信息错误"); }
                StartFrequency = StartFrequency * 1000000;
                StopFrequency = StopFrequency * 1000000;
                samplingFrequency = (int)Math.Abs(StopFrequency - StartFrequency);
            }
            else
            {
                carrierFrequency = CarrierFrequency * 1000000;
                maxFrequency = (int)(1 / PulseWidth);
                if (maxFrequency > carrierFrequency || PulseWidth > 0.000000143)
                { throw new Exception("脉冲宽度过大或者过小"); }
                samplingFrequency = 2 * maxFrequency;
            }
            totalpsd = 0;
            frequencyspace = (float)samplingFrequency / N;
            GetInPutInformation(WaveformType, PulseWidth, samplingFrequency, Rolloff, FrequencyVariation, StartFrequency, StopFrequency);
        }

        private void GetInPutInformation(string WaveformName, double PulseWidth, long samplingFrequency, double Rolloff, string FrequencyVariation, double StartFrequency, double StopFrequency)
        {
            //将波形函数中的t变成k/samplingFrequency进行取值
            switch (WaveformName)
            {
                case "RaisedCosine":
                    {
                        if (Rolloff > 1 || Rolloff < 0)
                        { throw new Exception("roll-off factor不在0-1范围内"); }
                        for (int k = 1; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            if (t <= 6 * PulseWidth && t >= 3 * PulseWidth)
                            { input[k].Re = (Math.Sin(Math.PI * t / PulseWidth) / Math.PI * t / PulseWidth) * Math.Cos(Math.PI * Rolloff * t / PulseWidth) / ((1 - Math.Pow(2 * t * Rolloff, 2)) / (Math.Pow(PulseWidth, 2))); }
                        }
                    }
                    break;
                case "Chirp":
                    {
                        for (int k = 1; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            double Prc = 0;
                            if (t >= 0 && t <= (Rolloff * PulseWidth / (1 + Rolloff)))
                            { Prc = 0.5 * (1 + Math.Cos((1 + PulseWidth) * Math.PI * (t - Rolloff * PulseWidth / (1 + PulseWidth)) / PulseWidth * Rolloff)); }
                            if (t > (Rolloff * PulseWidth / (1 + Rolloff)) && t <= (PulseWidth / (1 + Rolloff)))
                            { Prc = 1; }
                            if (t > (PulseWidth / (1 + Rolloff)) && t <= PulseWidth)
                            { Prc = 0.5 * (1 + Math.Cos((1 + PulseWidth) * Math.PI * (t - PulseWidth / (1 + PulseWidth)) / PulseWidth * Rolloff)); }
                            if (FrequencyVariation == "Linear")
                            {
                                if (t < PulseWidth)
                                { input[k].Re = Prc * Math.Sin(2 * Math.PI * StartFrequency + 2 * Math.PI * (StopFrequency - StartFrequency) * t / 2 * PulseWidth) * t; }
                            }
                            else
                            {
                                if (t < PulseWidth)
                                { input[k].Re = Prc * Math.Sin(2 * Math.PI * (PulseWidth / Math.Log(StopFrequency - StartFrequency) * (StartFrequency * Math.Pow(StopFrequency / StartFrequency, t / PulseWidth) - StartFrequency))); }
                            }
                        }

                    }
                    break;
                case "Blackman":
                    {
                        for (int k = 0; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            if (t < PulseWidth)
                            { input[k].Re = 0.42 - 0.5 * Math.Cos(2 * t * Math.PI / PulseWidth) + 0.08 * Math.Cos(4 * t * Math.PI / PulseWidth); }
                        }
                    }
                    break;
                case "Gaussian":
                    {
                        double a = 16 / Math.Pow(PulseWidth, 2);
                        for (int k = 0; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            if (t < 2 * PulseWidth)
                            { input[k].Re = Math.Exp(-a * Math.Pow(t - PulseWidth, 2)); }
                        }
                    }
                    break;
                case "GaussianDerivative":
                    {
                        double a = 16 / Math.Pow(PulseWidth, 2);
                        for (int k = 0; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            if (t < 2 * PulseWidth)
                            { input[k].Re = -2 * Math.Sqrt(Math.E / 2 * a) * a * (t - PulseWidth) * Math.Exp(-a * Math.Pow(t - PulseWidth, 2)); }
                        }
                    }
                    break;
                case "Hamming":
                    {
                        for (int k = 0; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            if (t < PulseWidth)
                            { input[k].Re = 0.54 - 0.46 * Math.Cos(2 * t * Math.PI / PulseWidth); }
                        }
                    }
                    break;
                case "RootRaisedCosine":
                    {
                        if (Rolloff > 1 || Rolloff < 0)
                        { throw new Exception("roll-off factor不在0-1范围内"); }
                        for (int k = 0; k < N; k++)
                        {
                            double t = (float)k / samplingFrequency;
                            if (t <= 6 * PulseWidth && t >= 3 * PulseWidth)
                            {
                                input[k].Re = (4 * Rolloff * Math.Cos((1 + Rolloff) * Math.PI * t / PulseWidth) + Math.Sin((1 - Rolloff) * Math.PI * t / PulseWidth) * Math.Pow(4 * Rolloff * t / PulseWidth, -1))
                                  / (Math.PI * Math.Sqrt(PulseWidth) * ((1 - 16 * Math.Pow(Rolloff * t, 2)) / (Math.Pow(PulseWidth, 2))));
                            }
                        }
                    }
                    break;
                default:
                    throw new Exception("输入的波形与数据库存储的波形不匹配");
            }

        }

        //快速傅里叶变换
        public void fft(int count)
        {
            int group, i, p, q, k;
            if (count == -1)
            {
                for (k = 0; k < N; k++)
                {
                    outdata[k].Im = IO[k].Im;
                    outdata[k].Re = IO[k].Re;
                }
            }
            else
            {
                fft(count - 1);          //递归调用

                group = (int)(N / Math.Pow(2, count + 1)); //每一级的分组数

                for (i = 0; i < group; i++)
                {
                    for (p = (int)(i * Math.Pow(2, count + 1)); p <= (int)(i * Math.Pow(2, count + 1) + Math.Pow(2, count) - 1); p++)
                    {
                        double pR, pI, qR, qI;
                        q = p + (int)Math.Pow(2, count);
                        pR = outdata[p].Re;
                        pI = outdata[p].Im;
                        qR = outdata[q].Re;
                        qI = outdata[q].Im;
                        Wtemp.Re = Math.Cos(2 * Math.PI / (int)Math.Pow(2, count + 1) * (int)(p - i * Math.Pow(2, count + 1)));
                        Wtemp.Im = -Math.Sin(2 * Math.PI / (int)Math.Pow(2, count + 1) * (int)(p - i * Math.Pow(2, count + 1)));
                        temp[p].Re = pR + (Wtemp.Re * qR - Wtemp.Im * qI);
                        temp[p].Im = pI + (Wtemp.Re * qI + Wtemp.Im * qR);
                        temp[q].Re = pR - (Wtemp.Re * qR - Wtemp.Im * qI);
                        temp[q].Im = pI - (Wtemp.Re * qI + Wtemp.Im * qR);
                    }
                }
                for (k = 0; k < N; k++)
                {
                    outdata[k].Re = temp[k].Re;
                    outdata[k].Im = temp[k].Im;
                }
            }
        }


        public void trans_code()
        {
            int[] bit = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int i = 0;
            int j = 0;
            int k = 0;
            int mytemp = 0;
            int num;
            num = (int)Math.Pow(2, n);

            for (j = 0; j < num; j++)
            {
                mytemp = j;
                ////////////////////////////////////////////////////////////
                for (i = 0; i < n; i++)              //十进制数二进制化
                {
                    bit[i] = mytemp % 2;
                    mytemp = mytemp / 2;
                }
                ////////////////////////////////////////////////////////////
                for (k = 0; k < (n / 2); k++)           //码位倒置
                {
                    mytemp = bit[k];
                    bit[k] = bit[n - 1 - k];
                    bit[n - 1 - k] = mytemp;
                }
                ///////////////////////////////////////////////////////////

                for (i = 0; i < n; i++)              //恢复为十进制数
                {
                    code[j] = code[j] + (int)Math.Pow(2, i) * bit[i];
                }
            }
        }

        //功率谱计算
        public void cal()
        {
            int k;
            //////////////////////////////////   把输入数据进行码位倒置
            for (k = 0; k < N; k++)
            {
                code[k] = 0;
            }
            trans_code();
            for (k = 0; k < N; k++)
            {
                IO[k].Re = input[code[k]].Re;
                IO[k].Im = input[code[k]].Im;
            }
            /////////////////////////////////   进行FFT变换，结果保存在outdata[N]中

            fft(n - 1);

            for (k = 0; k < N; k++)
            {
                psddata[k] = (Math.Pow(outdata[k].Re, 2) + Math.Pow(outdata[k].Im, 2)) / N;
            }
            for (k = 0; k < N; k++)
            {
                totalpsd = totalpsd + psddata[k] * frequencyspace;
            }
        }

        //得到频点和功率占比
        public DivFrequency GetFandPSD()
        {
            this.cal();
            int k;
            DivFrequency result = new DivFrequency();
            int Fwidth = 147;//1024/7
            result.startFre[3] = (long)((-(Fwidth - 1) / 2 - 0.5) * frequencyspace + carrierFrequency);
            result.endFre[3] = (long)(result.startFre[3] + Fwidth * frequencyspace);
            result.startFre[0] = (long)(result.startFre[3] - 3 * Fwidth * frequencyspace);
            result.endFre[0] = result.startFre[1] = (long)(result.startFre[3] - 2 * Fwidth * frequencyspace);
            result.endFre[1] = result.startFre[2] = (long)(result.startFre[3] - Fwidth * frequencyspace);
            result.endFre[2] = result.startFre[3];
            result.startFre[4] = result.endFre[3];
            result.endFre[4] = result.startFre[5] = (long)(result.endFre[3] + Fwidth * frequencyspace);
            result.endFre[5] = result.startFre[6] = (long)(result.endFre[3] + 2 * Fwidth * frequencyspace);
            result.endFre[6] = (long)(result.endFre[3] + 3 * Fwidth * frequencyspace);

            double tmp = 0;
            for (k = 1; k <= (Fwidth - 1) / 2; k++)
            {
                tmp = tmp + psddata[k] * frequencyspace;
            }
            result.proportion[3] = 2 * (tmp + psddata[0] * frequencyspace / 2) / totalpsd;

            tmp = 0;
            for (k = (Fwidth - 1) / 2 + 1; k <= (Fwidth - 1) / 2 + Fwidth; k++)
            {
                tmp = tmp + psddata[k] * frequencyspace;
            }
            result.proportion[2] = result.proportion[4] = tmp / totalpsd;

            tmp = 0;
            for (k = (Fwidth - 1) / 2 + Fwidth + 1; k <= (Fwidth - 1) / 2 + 2 * Fwidth; k++)
            {
                tmp = tmp + psddata[k] * frequencyspace;
            }
            result.proportion[1] = result.proportion[5] = tmp / totalpsd;

            tmp = 0;
            for (k = (Fwidth - 1) / 2 + 2 * Fwidth + 1; k < N / 2; k++)
            {
                tmp = tmp + psddata[k] * frequencyspace;
            }
            result.proportion[0] = result.proportion[6] = tmp / totalpsd;

            return result;
        }

    }
}
