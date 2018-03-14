using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileObject;
using CalculateModelClasses;
using System.IO;
using SetupFileObject;
using UanFileProceed;
using System.Text.RegularExpressions;

namespace TxRxFileProceed
{
    public class RxFileProceed
    {
        //path为文件路径 
        //如果路径不对或者为非rx文件抛出异常
        public static double Spacing;
    //    private static List<RxObject> rx;
        public static List<RxObject> RxReader(string path)
        {
            List<RxObject> rx = new List<RxObject>();
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
            {
                return rx;
                throw new Exception("输入路径不存在");
            }

            string[] paths = path.Split(new char[] { '.' });
            if (paths.Length < 1 || !paths[paths.Length - 1].Equals("rx"))
            {
                return rx;
                throw new Exception("这不是一个rx文件");
            }

            StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8);
            string s = sr.ReadToEnd();
            sr.Close();
            string[] strseperator = new string[] { "begin_<points>", "begin_<grid>", "begin_<situation>" };
            string[] result = null;
            result = s.Split(strseperator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < result.Length; i++)
            {
                //result[i]=result[i].Trim();
                RxObject temp = new RxObject(result[i]);
                rx.Add(temp);
            }
            return rx;

        }

        /// <summary>
        ///从setup文件中获取发射机的频率
        /// </summary>
        private static double GetFrequence(RxObject rx, SetupObject setup)
        {
            List<WaveformStp> waveform = setup.waveform.allWaveforms;
            List<AntennaStp> antenna = setup.antenna.allAntennas;
            if (rx.at.waveformNum == -1)
            {
                for (int j = 0; j < antenna.Count; j++)
                {
                    if (rx.at.antennaNum == antenna[j].antenna)
                        rx.at.waveformNum = antenna[j].waveform;
                }
            }

            //接收机频率有用么？

            int i;
            for (i = 0; i < waveform.Count; i++)
            {
                if (rx.at.waveformNum == waveform[i].waveform)
                    break;
            }
            if (i > waveform.Count - 1)
            {
                LogFileManager.ObjLog.info("如果这是点状接收机，则没问题。如果是态势，则有问题");
                return 0;
            }
            return waveform[i].CarrierFrequency;
        }


        /// <summary>
        ///从setup中获取发射频率带宽????
        /// </summary>
        private static double GetWidth(RxObject rx, SetupObject setup)
        {

            if (rx.at.waveformNum == -1)
            {
                LogFileManager.ObjLog.info("如果这是点状接收机，则没问题。如果是态势，则有问题");
                return 0;
            }
            double temp;
            List<WaveformStp> waveform = setup.waveform.allWaveforms;
            int i;
            for (i = 0; i < waveform.Count; i++)
            {
                if (rx.at.waveformNum == waveform[i].waveform)
                    break;
            }
            switch (waveform[i].type)
            {
                case "Sinusoid":
                    temp = waveform[i].bandwidth;
                    break;
                case "RaisedCosine":
                    temp = 10000000 * (1 + waveform[i].Rolloff) / (waveform[i].PulseWidth / 0.0000001);
                    break;
                case "Chirp":
                    temp = waveform[i].StopFrequency - waveform[i].StartFrequency;
                    break;
                case "Blackman":
                    temp = 60000000 / (waveform[i].PulseWidth / 0.0000001);
                    break;
                case "Gaussian":
                    temp = 23190000 / (waveform[i].PulseWidth / 0.0000001);
                    break;
                case "GaussianDerivative":
                    temp = 16480000 / (waveform[i].PulseWidth / 0.0000001); ;
                    break;
                case "Hamming":
                    temp = 40000000 / (waveform[i].PulseWidth / 0.0000001); ;
                    break;
                case "RootRaisedCosine":
                    temp = 10000000 * (1 + waveform[i].Rolloff) / (waveform[i].PulseWidth / 0.0000001);
                    break;
                default:
                    temp = 0;
                    break;
            }
            return temp / 1000000;
        }

        /// <summary>
        ///返回接收球，如果是区域状则返回多个接收球
        /// </summary>
        public static List<List<ReceiveBall> > GetRx(string rxpath,string setup,string terpath)
        {
            List<List<ReceiveBall> > RxBalls = new List<List<ReceiveBall> >();
            List<RxObject> Rx = RxReader(rxpath); 
            for (int i = 0; i < Rx.Count; i++)
            {
                //这是点状和区域状
                if (Rx[i].flag != "<situation>")
                {
                    if (Rx[i].flag.Equals("<points>"))
                    {
                        Point lc = new Point(Rx[i].Lc.vertical[0], Rx[i].Lc.vertical[1], Rx[i].Lc.vertical[2]);
                        //GetZValue(ter, lc);
                        lc.Z = Rx[i].Lc.vertical[2] + Rx[i].cubesize;
                        List<ReceiveBall> temp = new List<ReceiveBall>();
                        ReceiveBall rxtemp = new ReceiveBall(lc, Rx[i].num, Rx[i].name);
                        rxtemp.UAN = ReadUan.GetUan(Rx[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                        temp.Add(rxtemp);
                        RxBalls.Add(temp);
                    }
                    else  //现在不会进入这个分支
                    {  
                        List<ReceiveBall> temp = new List<ReceiveBall>();
                        double length = (double)Rx[i].Lc.side1;
                        double width = (double)Rx[i].Lc.side2;
                        double space = (double)Rx[i].Lc.spacing;
                        for (int j = 0; j < width / space + 1; j++)
                            for (int k = 0; k < length / space + 1; k++)
                            {
                                Point lc = new Point(Rx[i].Lc.vertical[0] + k * space, Rx[i].Lc.vertical[1] - j * space, Rx[i].Lc.vertical[2]);
                                //GetZValue(ter, lc);
                                lc.Z += Rx[i].Lc.vertical[2] + Rx[i].cubesize;
                                ReceiveBall rxtemp = new ReceiveBall(lc, Rx[i].num, Rx[i].name);
                                rxtemp.UAN = ReadUan.GetUan(Rx[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                                temp.Add(new ReceiveBall(lc, Rx[i].num, Rx[i].name));
                            }
                        RxBalls.Add(temp);
                    }
                 
                }
                  //这是态势显示
                else
                {
                    if ((double)Rx[i].Lc.side1 == 0 || (double)Rx[i].Lc.side2 == 0)//态势区域边长不能为0
                    { LogFileManager.ObjLog.debug("态势区域边长为0，出错"); }
                    
           //         Rx[i].Lc.spacing = 50;
           //         Spacing = (double)Rx[i].Lc.spacing;
                    List<ReceiveBall> temp = new List<ReceiveBall>();
                    ReceiveBall receiveArea = new ReceiveArea();
                    receiveArea.UAN = ReadUan.GetUan(Rx[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                    Point lc = new Point(Rx[i].Lc.vertical[0], Rx[i].Lc.vertical[1], Rx[i].Lc.vertical[2]);
                    receiveArea.temp = Rx[i].Lc.vertical[2];
                    //GetZValue(ter, lc);
                    lc.Z = Rx[i].Lc.vertical[2] + Rx[i].cubesize;
                    //receiveArea.rxLength = (double)Rx[i].Lc.side1;
                    //receiveArea.rxWidth = (double)Rx[i].Lc.side2;
                    //receiveArea.spacing =(double)Rx[i].Lc.spacing;
                    //receiveArea.origen = lc;
                    receiveArea.Instance((double)Rx[i].Lc.side1, (double)Rx[i].Lc.side2, (double)Rx[i].Lc.spacing, lc);
                    receiveArea.RxName = Rx[i].name;
                    receiveArea.RxNum = Rx[i].num;
                
                    temp.Add(receiveArea);
                    RxBalls.Add(temp);
                }
            }
            return RxBalls;        
        }
    }
}
