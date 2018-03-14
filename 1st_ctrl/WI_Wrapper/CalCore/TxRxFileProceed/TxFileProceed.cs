using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileObject;
using CalculateModelClasses;
using SetupFileObject;
using System.IO;
using UanFileProceed;

namespace TxRxFileProceed
{
    
    public class TxFileProceed
    {
        //path为Tx文件路径,setup为setup文件路径，terpath为ter文件路径
        #region
        private static List<TxObject> TxReader(string path)
        {
            List<TxObject> tx = new List<TxObject>();
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
            {
                return tx;
                throw new Exception("输入路径不存在");
            }

            string[] paths = path.Split(new char[] { '.' });
            if (paths.Length < 1 || !paths[paths.Length - 1].Equals("tx"))
            {
                return tx;
                throw new Exception("这不是一个rx文件");
            }

            StreamReader sr = new StreamReader(path,System.Text.Encoding.UTF8);
            string s = sr.ReadToEnd();
            sr.Close();
            string[] strseperator = new string[] { "begin_<points>", "begin_<grid>" };
            string[] result = null;
            result = s.Split(strseperator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].TrimStart(' ');
                TxObject temp = TxObject.txObjFac(result[i]);
                tx.Add(temp);
            }
            return tx;

        }

        /// <summary>
        ///从setup文件中获取发射机的载波频率
        /// </summary>
        private static double GetTxCarrierFrequence(TxObject tx, SetupObject setup)
        {
            List<WaveformStp> waveform = setup.waveform.allWaveforms;
            List<AntennaStp> antenna = setup.antenna.allAntennas;
            if (tx.at.waveformNum == -1)
            {
                for (int j = 0; j < antenna.Count; j++)
                {
                    if (tx.at.antennaNum == antenna[j].antenna)
                        tx.at.waveformNum = antenna[j].waveform;
                }
            }
            int i;
            for (i = 0; i < waveform.Count; i++)
            {
                if (tx.at.waveformNum == waveform[i].waveform)
                    break;
            }
            return waveform[i].CarrierFrequency;
        }

        /// <summary>
        ///从setup中获取发射频率带宽
        /// </summary>
        private static double GetWidth(TxObject tx, SetupObject setup)
        {
            double temp;
            List<WaveformStp> waveform = setup.waveform.allWaveforms;
            int i;
            for (i = 0; i < waveform.Count; i++)
            {
                if (tx.at.waveformNum == waveform[i].waveform)
                    break;
            }
            switch (waveform[i].type)
            {
                case "Sinusoid":
                    temp = waveform[i].bandwidth;
                    break;
                case "RaisedCosine":
                    temp = 10000000*(1+waveform[i].Rolloff)/(waveform[i].PulseWidth/0.0000001);
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
                    temp =16480000 / (waveform[i].PulseWidth / 0.0000001);
                    break;
                case "Hamming":
                    temp = 40000000 / (waveform[i].PulseWidth / 0.0000001);
                    break;
                case "RootRaisedCosine":
                    temp = 10000000 * (1 + waveform[i].Rolloff) / (waveform[i].PulseWidth / 0.0000001);
                    break;
                default:
                    temp = 0;
                    break;
            }
            return temp/1000000;
        }

        /// <summary>
        ///从setup中获取发射机波形信息
        /// </summary>
        private static WaveformStp GetTxWaveForm(TxObject tx, SetupObject setup)
        {
            WaveformStp txwaveform = new WaveformStp();
            List<WaveformStp> waveform = setup.waveform.allWaveforms;
            List<AntennaStp> antenna = setup.antenna.allAntennas;
            if (tx.at.waveformNum == -1)
            {
                for (int j = 0; j < antenna.Count; j++)
                {
                    if (tx.at.antennaNum == antenna[j].antenna)
                        tx.at.waveformNum = antenna[j].waveform;
                }
            }
            int i;
            for (i = 0; i < waveform.Count; i++)
            {
                if (tx.at.waveformNum == waveform[i].waveform)
                    txwaveform= waveform[i];
            }
            return txwaveform;
        }


        /// <summary>
        ///从setup中获取发射机波形信息
        /// </summary>
        private static List<FrequencyBand> GetBandInformation(double power, DivFrequency Frequencyinfo)
        {
            List<FrequencyBand> TxFrequencyBand = new List<FrequencyBand>();
            for (int i = 0; i < 7; i++)
            {
               double divPower = (power * Frequencyinfo.proportion[i]);
                FrequencyBand param = new FrequencyBand((int)(Frequencyinfo.startFre[i]/1000000),(int)(Frequencyinfo.endFre[i]/1000000),divPower);
                TxFrequencyBand.Add(param);
            }
            return TxFrequencyBand;
        }
         
        #endregion

        //从传入的文件中提取发射机点
        //public static double TxFrequence;
       // public static double TxFrequenceWideth;
        /// <summary>
        ///从传入的文件中提取发射机点
        /// </summary>
        public static List<Node> GetTx(string txpath,string setup,string terpath)
        {
            List<Node> txnode = new List<Node>();
            List<TxObject> txs = TxReader(txpath);    
            Node temp;
            for (int i = 0; i < txs.Count; i++)
            {
                temp = new Node();
                temp.IsEnd = false; 
                temp.IsReceiver = false; 
                temp.LayNum = 1;
                temp.NodeStyle = NodeStyle.Tx;
                temp.Position = new Point(txs[i].Lc.vertical[0], txs[i].Lc.vertical[1], txs[i].Lc.vertical[2]);
                temp.Position.Z = (txs[i].Lc.vertical[2]+txs[i].cubesize);
                temp.TxNum = txs[i].num; 
                temp.Power = (txs[i].at.power); 
                temp.NodeName = txs[i].name;
                //加从setup文件中获取的频率函数、UAN
                //temp.frequence = GetTxCarrierFrequence(txs[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                //TxFrequence = temp.frequence;
                //temp.FrequenceWidth = GetWidth(txs[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                //TxFrequenceWideth = temp.FrequenceWidth;
                temp.UAN = ReadUan.GetUan(txs[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                temp.Height = txs[i].cubesize;
                txnode.Add(temp);

            }
            return txnode;
        }
        /// <summary>
        ///计算威胁度时，需要把其他的发射机也当接收机来算，从发射机文件中提取接收球
        /// </summary>
        public static List<List<ReceiveBall>> GetTxAsRx(string txpath, string setup, string terpath)
        {
            List<List<ReceiveBall>> TxAsRxBalls = new List<List<ReceiveBall>>();
            List<TxObject> txForRx = TxReader(txpath);
            for (int i = 0; i < txForRx.Count; i++)
            {
                Point lc = new Point(txForRx[i].Lc.vertical[0], txForRx[i].Lc.vertical[1], txForRx[i].Lc.vertical[2]);
                lc.Z = txForRx[i].Lc.vertical[2] + txForRx[i].cubesize;
                List<ReceiveBall> temp = new List<ReceiveBall>();
                ReceiveBall rxtemp = new ReceiveBall(lc, txForRx[i].num, txForRx[i].name);
               // rxtemp.frequence = GetTxCarrierFrequence(txForRx[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
               // rxtemp.FrequenceWidth = GetWidth(txForRx[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                rxtemp.UAN = ReadUan.GetUan(txForRx[i], SetupFileProceed.GetSetupFile.GetSetup(setup));
                rxtemp.isTx = true;
                temp.Add(rxtemp);
                TxAsRxBalls.Add(temp);
            }
            return TxAsRxBalls;
        }

        /// <summary>
        ///获取每个发射机的频段信息
        /// </summary>
        public static List<FrequencyBand> GetTxFrequenceBand(string txpath, string setuppath, int txindex)
        {
            List<FrequencyBand> TxFrequenceBand = new List<FrequencyBand>();
            List<TxObject> txs = TxReader(txpath);
            SetupObject setupfile = SetupFileProceed.GetSetupFile.GetSetup(setuppath);
            WaveformStp txwaveform = GetTxWaveForm(txs[txindex], setupfile);
            if (txwaveform.type == null)
            { throw new Exception("波形信息错误"); }
            if (txwaveform.type == "Sinusoid")
            {
                if (txwaveform.bandwidth < 2)
                {
                    FrequencyBand temp = new FrequencyBand((int)(txwaveform.CarrierFrequency -1), (int)(txwaveform.CarrierFrequency + 1), (txs[txindex].at.power));
                    TxFrequenceBand.Add(temp);
                }
                else
                { 
                    FrequencyBand temp = new FrequencyBand((int)(txwaveform.CarrierFrequency - txwaveform.bandwidth/2), (int)(txwaveform.CarrierFrequency + txwaveform.bandwidth/2), (txs[txindex].at.power));
                    TxFrequenceBand.Add(temp);
                }
            }
            else
            {
                psd TxDivFrequence = new psd(txwaveform.type,txwaveform.PulseWidth, (long)txwaveform.CarrierFrequency, txwaveform.Rolloff,txwaveform.FrequencyVariation,txwaveform.StartFrequency,txwaveform.StopFrequency);
                DivFrequency Frequencyinfo = TxDivFrequence.GetFandPSD();
                TxFrequenceBand = GetBandInformation((txs[txindex].at.power), Frequencyinfo);
            }
            return TxFrequenceBand;
        }

        /// <summary>
        ///获得最小频率
        /// </summary>
        /// <param name="txFrequencyBand">发射机频段信息</param>
        /// <returns>返回发射机最小频率</returns>
        public static int GetMinFrequenceFromList(List<FrequencyBand> txFrequencyBand)
        {
            int minFrequence = txFrequencyBand[0].MidPointFrequence;
            for (int i = 1; i < txFrequencyBand.Count - 1; i++)
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
        public static int GetMaxFrequenceFromList(List<FrequencyBand> txFrequencyBand)
        {
            int maxFrequence = txFrequencyBand[0].MidPointFrequence;
            for (int i = 1; i < txFrequencyBand.Count - 1; i++)
            {
                if (maxFrequence <= txFrequencyBand[i].MidPointFrequence)
                { maxFrequence = txFrequencyBand[i].MidPointFrequence; }
            }
            return maxFrequence;
        }

    }
}
