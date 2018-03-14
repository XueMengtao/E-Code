using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
   public class WaveformStp
    {
        //public const string begin = "begin_<Waveform>";
        //public const string end = "end_<Waveform>\r\n";
       //所有波形都有的字段
        public string name;
        public string type;
        public int waveform;
       //部分波形有的字段
        public double phase;
        public double bandwidth;
        public double CarrierFrequency;
        public double PulseWidth;
        public string Dispersive;
        public string TDFilename;
        public double Phase;
        public double Rolloff;
        public string FrequencyVariation;
        public double StartFrequency;
        public double StopFrequency;
        public void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            name = str.Substring(pos, linelength).Substring(1);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            type = str.Substring(pos, linelength);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            waveform = Convert.ToInt32(str.Substring(pos, linelength).Substring(9));
            pos = str .IndexOf ("phase ");
            if (pos == -1) phase = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                phase = Convert.ToDouble(str.Substring(pos, linelength).Substring(6));
            }
            pos = str.IndexOf("bandwidth ");
            if (pos == -1) bandwidth = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                bandwidth = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            }
            pos = str.IndexOf("CarrierFrequency ");
            if (pos == -1) CarrierFrequency = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                CarrierFrequency = Convert.ToDouble(str.Substring(pos, linelength).Substring(17));
            }
            pos = str.IndexOf("PulseWidth ");
            if (pos == -1) PulseWidth = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                PulseWidth = Convert.ToDouble(str.Substring(pos, linelength).Substring(11));
            }
            pos = str.IndexOf("Dispersive ");
            if (pos == -1) Dispersive = null;
            else
            {
                linelength = Tool.readline(str, pos);
                Dispersive = str.Substring(pos, linelength);
            }
            pos = str.IndexOf("TDFilename ");
            if (pos == -1) TDFilename = null;
            else
            {
                linelength = Tool.readline(str, pos);
                TDFilename = str.Substring(pos, linelength).Substring(11);
            }
            pos = str.IndexOf("Phase ");
            if (pos == -1) Phase = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                Phase = Convert.ToDouble(str.Substring(pos, linelength).Substring(6));
            }
            pos = str.IndexOf("Rolloff ");
            if (pos == -1) Rolloff = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                Rolloff = Convert.ToDouble(str.Substring(pos, linelength).Substring(8));
            }
            pos = str.IndexOf("FrequencyVariation ");
            if (pos == -1) FrequencyVariation = null;
            else
            {
                linelength = Tool.readline(str, pos);
                FrequencyVariation = str.Substring(pos, linelength).Substring(19);
            }
            pos = str.IndexOf("StartFrequency ");
            if (pos == -1) StartFrequency = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                StartFrequency = Convert.ToDouble(str.Substring(pos, linelength).Substring(15));
            }
            pos = str.IndexOf("StopFrequency ");
            if (pos == -1) StopFrequency = 0;
            else
            {
                linelength = Tool.readline(str, pos);
                StopFrequency = Convert.ToDouble(str.Substring(pos, linelength).Substring(14));
            }
        }
        public WaveformStp(string str)
        {
            init(str);
        }
        public WaveformStp()
        { }
    }
    
    public class TotalWaveform
    {
        string[] waveformSeperator = new string[] { "begin_<Waveform>" };
        string[] result = null;
        public List<WaveformStp> allWaveforms;
        List <WaveformStp > Getwaveform(string str)
        {
            List<WaveformStp> waveStp = new List<WaveformStp>();
            result = str.Split(waveformSeperator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < result.Length; i++)
            {
                WaveformStp wave = new WaveformStp(result [i]);
                waveStp.Add(wave );
              
            }
            return waveStp;
        }
        public TotalWaveform(string str)
        {
            allWaveforms = Getwaveform(str);
        }
    }
}
