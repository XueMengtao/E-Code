using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileObject;
using SetupFileObject;
using System.IO;


namespace SetupFileProceed
{
    public class GetSetupFile
    {
        //获取setup文件对象，path为setup路径
        public static SetupObject GetSetup(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
            {
                return null;
                throw new Exception("输入路径不存在");
            }
            string[] paths = path.Split(new char[] { '.' });
            if (paths.Length < 1 || !paths[paths.Length - 1].Equals("setup"))
            {
                return null;
                throw new Exception("这不是一个setup文件");
            }
            StreamReader sr = new StreamReader(path);
            string s = sr.ReadToEnd();
            sr.Close();
            SetupObject setupfile = new SetupObject(s);
             return setupfile;
        }
        //path是传入的路径参数，获取setup文件中的波形信息
        public static List<WaveformStp> GetWaveformStp(SetupObject setup)
        {
            return setup.waveform.allWaveforms;
        }
        //path是传入的路径参数，获取setup文件中的天线信息
        public static List<AntennaStp> GetAntennaStp(SetupObject setup)
        {
            return setup.antenna.allAntennas;
        }
    }
}

