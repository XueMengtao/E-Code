using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace WcfService
{
    public class Split
    {
        public static string FileSplit(string filepath, string setuppath)
        {
            //获取打开的文件的名称及文件的目录
            System.IO.FileInfo tx = new System.IO.FileInfo(filepath);
            System.IO.FileInfo setup = new FileInfo(setuppath);
            string[] a = new string[] { "." };
            string[] txname = tx.Name.Split(a, StringSplitOptions.RemoveEmptyEntries);
            string splitPath = ConfigurationManager.AppSettings.Get("splitPath");
            string dirname = splitPath + txname[0];
            if (Directory.Exists(dirname))
            {
                Directory.Delete(dirname, true);
                Directory.CreateDirectory(dirname);
            }
            else
            {
                Directory.CreateDirectory(dirname);
            }
            //Console.WriteLine(dirname);
            //Console.WriteLine(txname[1]);
            //打开文件流，进行读取工作
            try
            {
                //StreamReader sr = tx.OpenText();
                //StreamReader sr1 = setup.OpenText();
                StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("GBK"));
                StreamReader sr1 = new StreamReader(setuppath, Encoding.GetEncoding("GBK"));
                string s = sr.ReadToEnd();
                string x = sr1.ReadToEnd();
                if (s == "" || x == "")
                    throw new nullException("the file is empty");
                //if (s == null)
                //Console.WriteLine("file is empty");

                else
                {

                    //设置分割字符串对文件内容进行分割
                    string[] strseperator = new string[] { "begin_<points>" };
                    string[] result = null;
                    //string[] frequence = null;
                    //string[] finalresult = null;
                    result = s.Split(strseperator, StringSplitOptions.RemoveEmptyEntries);
                    //frequence = new string[result.Length];
                    //finalresult = new string[result.Length];
                    //for(int l=0;l<result.Length;l++)
                    //{
                    //    int m=result[l].IndexOf("end_<points>");
                    //    int n = result[l].LastIndexOf("\r");
                    //    frequence[l] = result[l].Substring(m + 13, n - m - 13);
                    //    finalresult[l] = result[l].Substring(0, m + 12);
                    //}
                    int i = result.Length;
                    //创建分割的tx文件
                    for (int j = 1; j <= i; j++)
                    {
                        //int p1 = result[j - 1].IndexOf("TxSet");
                        //result[j - 1] = result[j - 1].Remove(p1 + 6, 1);
                        //result[j - 1] = result[j - 1].Insert(p1 + 6, "1");
                        int m = result[j - 1].IndexOf("waveform");
                        int n = result[j - 1].IndexOf("rotation_x");
                        string waveform = result[j - 1].Substring(m, n - m - 2);
                        if (waveform == "waveform -1")
                        {
                            m = result[j - 1].IndexOf("antenna ");
                            n = result[j - 1].IndexOf("waveform -1");
                            string antenna = result[j - 1].Substring(m, n - m - 2);
                            int iPolarInSetup = x.IndexOf("polarization", x.IndexOf(antenna));
                            if ((iPolarInSetup - x.IndexOf(antenna)) > 50)
                            {
                                iPolarInSetup = x.IndexOf("power_threshold ", x.IndexOf(antenna));
                            }
                            int iWaveFormInSetup = x.IndexOf("waveform ", x.IndexOf(antenna));
                            if (iPolarInSetup == -1)
                            {
                                iPolarInSetup = x.IndexOf("type", x.IndexOf(antenna));
                            }
                            waveform = x.Substring(iWaveFormInSetup, iPolarInSetup - iWaveFormInSetup - 2);
                        }
                        int l = x.IndexOf(waveform + "\r\nCarrierFrequency");
                        char c = x[l + waveform.Length + 19];
                        int count = 0;
                        while (c != '\r')
                        {
                            c = x[l + waveform.Length + 19 + (++count)];
                        }
                        double f = Convert.ToDouble(x.Substring(l + waveform.Length + 19, count)) / 1000000.0;
                        string frequence = string.Format("{0:#0.0}", f);
                        string newpath = dirname + "\\\\" + txname[0] + j.ToString() + "_" + frequence + "MHz." + txname[1];
                        System.IO.FileInfo newtx = new System.IO.FileInfo(newpath);
                        
                        //using (StreamWriter sw = newtx.CreateText())
                        using (StreamWriter sw = new StreamWriter(newpath,true,Encoding.GetEncoding("GBK")))
                        {
                            sw.Write(strseperator[0]);
                            sw.Write(result[j - 1]);
                            sw.Close();
                        }
                    }
                }
                sr.Close();
            }
            catch (nullException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.error(e.Message, e);
                throw e;
            }
            return dirname;
        }

    }
}
