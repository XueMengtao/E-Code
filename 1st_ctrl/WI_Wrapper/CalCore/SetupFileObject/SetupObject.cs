using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
    public class SetupObject 
    {
        public string name;
        int IndexOfGl, IndexOfStu, IndexOfFe,IndexOfTr, IndexOfRx, IndexOfAntenna, IndexOfWaveform;
        int FirstAvailableStudyAreaNumber;
        int FirstAvailableCommSystemNumber;
        int FirstAvailableFilterNumber;
        GlobalsStp gl;
        StudyareaStp stu;
        FeatureStp fe;
        RequestsStp re;
        ScalesStp sca;
        public TransmitterStp tr;
        public ReceiverStp rx;
        public TotalAntenna antenna;
        public TotalWaveform waveform;
        public void init(string str)
        {
            int start;
            int linelength;
            start = str.IndexOf("begin_<project>") + 16;
            linelength = Tool.readline(str, start);
            name = str.Substring(start, linelength);
            start = str.IndexOf("begin_<globals>");
            IndexOfGl = start;
            if (start != -1)
            {
                linelength = Tool.readline(str, start);
                gl = new GlobalsStp(str.Substring(start + linelength + 2, str.IndexOf("end_<globals>") - start - linelength - 2));
            }
            else
                gl = null;
            start = str.IndexOf("FirstAvailableStudyAreaNumber ");
            linelength = Tool.readline(str, start);
            FirstAvailableStudyAreaNumber = Convert.ToInt32(str.Substring(start, linelength).Substring(30));
            start = str.IndexOf("FirstAvailableCommSystemNumber ");
            linelength = Tool.readline(str, start);
            FirstAvailableCommSystemNumber = Convert.ToInt32(str.Substring(start, linelength).Substring(31));
            start = str.IndexOf("FirstAvailableFilterNumber ");
            linelength = Tool.readline(str, start);
            FirstAvailableFilterNumber = Convert.ToInt32(str.Substring(start, linelength).Substring(27));
            start = str.IndexOf("begin_<studyarea>");
            IndexOfStu = start;
            if (start != -1)
                stu = new StudyareaStp(str.Substring(start, str.IndexOf("end_<studyarea>") - start - 2));
            else
                stu = null;
            start = str.IndexOf("begin_<feature>");
            IndexOfFe = start;
            if (start != -1)
            {
                linelength = Tool.readline(str, start);
                fe = new FeatureStp(str.Substring(start + linelength + 2, str.IndexOf("end_<feature>") - start - linelength - 2));
            }
            else
                fe = null;
            start = str.IndexOf("begin_<transmitter>");
            IndexOfTr = start;
            if (start != -1)
            {
                linelength = Tool.readline(str, start);
                tr = new TransmitterStp(str.Substring(start + linelength + 2, str.IndexOf("end_<transmitter>") - start - linelength - 2));
            }
            else
                tr = null;
            start = str.IndexOf("begin_<receiver>");
            IndexOfRx = start;
            if (start != -1)
            {
                linelength = Tool.readline(str, start);
                rx = new ReceiverStp(str.Substring(start + linelength + 2, str.IndexOf("end_<receiver>") - start - linelength - 2));
            }
            else
                rx = null;
            start = str.IndexOf("begin_<antenna>");
            IndexOfAntenna = start;
            if (start != -1)
                antenna = new TotalAntenna(str.Substring(start, str.LastIndexOf("end_<antenna>") - start));
            else
                antenna = null;
            start = str.IndexOf("begin_<Waveform>");
            IndexOfWaveform = start;
            if (start != -1)
                waveform = new TotalWaveform(str.Substring(start, str.LastIndexOf("end_<Waveform>") - start));
            else
                waveform = null;
            start = str.IndexOf("begin_<requests>");
            linelength = Tool.readline(str, start);
            re = new RequestsStp(str.Substring(start + linelength + 2, str.IndexOf("end_<requests>") - start - linelength - 2));
            start = str.IndexOf("begin_<Scales>");
            linelength = Tool.readline(str, start);
            sca = new ScalesStp(str.Substring(start + linelength + 2, str.IndexOf("end_<Scales>") - start - linelength - 2));
        }
        public SetupObject(string str)
        {
            init(str);
        }
    }
    
      
      
  
}
