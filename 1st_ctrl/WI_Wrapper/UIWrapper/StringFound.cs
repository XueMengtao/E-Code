using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
namespace WF1
{
    class StringFound
    {
        //从字符串resultSet中找出索引字符串matchStr后面的字符串
        public static string FoundBackStr(string matchStr, string resultSet, bool b)
        {
            StringBuilder sb = new StringBuilder();
            int position = 0;
            //b为true则查找第一项即从前面查找
            if (b)
            {
                position = resultSet.IndexOf(matchStr);
            }
                //从后面开始查找
            else
            {
                position = resultSet.LastIndexOf(matchStr);
            }
            //如果找到了matchStr，则找出它后面并且在字符'\r'之前的字符串
            if (position != -1)
            {
                for (int i = 0; resultSet[position + matchStr.Length + i + 1] != '\r'; i++)
                {
                    sb = sb.Append(resultSet[position + matchStr.Length + i + 1]);
                }
                return sb.ToString();
            }
            //如果没找到matchStr则返回null
            else
            {
                return null;
            }
        }
        public static string FoundBackStr(string matchStr, string resultSet)
        {
            StringBuilder sb = new StringBuilder();
            int position =resultSet.IndexOf(matchStr); ;
            for (int i = 0; resultSet[position + matchStr.Length + i ] != '\r'; i++)
            {
                sb = sb.Append(resultSet[position + matchStr.Length +i]);
            }
            return sb.ToString();
        }
        //判断波形是否可删除,参数只能是字符串"wave"和"antenna"之一
        //public static bool WaveOrAntennaInUse(string waveOrAntennaName, string filePath,string str)
        //{
        //    bool b = false;
        //    if (!File.Exists(filePath))
        //    {
        //        return false;
        //    }
        //    using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default))
        //    {
        //        try
        //        {
        //            string strTem = null;
        //            while (!sr.EndOfStream)
        //            {
        //                strTem = sr.ReadLine();
        //                switch (str)
        //                {
        //                    case "wave":
        //                    char[] sep1 = { '#',  '\r' };
        //                    string[] strArr1 = strTem.Split(sep1);
        //                    //每一行的第三个是波形的名字
        //                    if (strArr1[1].Equals(waveOrAntennaName))
        //                    {
        //                        b = true;
        //                        //break;
        //                    }
        //                        break;
        //                    case "antenna":
        //                    char[] sep2 = { '*',  '\r' };
        //                    string[] strArr2 = strTem.Split(sep2);
        //                    //每一行的第二个是波形的名字
        //                    if (strArr2[1].Equals(waveOrAntennaName))
        //                    {
        //                        b = true;
        //                        //break;
        //                    }
        //                    break;
        //                    default:
        //                    LogFileManager.ObjLog.debug("参数" + str + "不是函数WaveOrAntennaInUse()的合法参数");
        //                    break;
        //                }
        //                if (b)
        //                    break;
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            LogFileManager.ObjLog.debug(exc.Message);
        //            return false;
        //        }
        //        return b;
        //    }

        //}
        public static bool WaveOrAntennaInUse(string waveOrAntennaName,string indStr)
        {
            bool b = false;
            try
            {
                string matchStr = FileOperation.ReadFile(MainWindow.relationOfAntAndWavePath );
                string temp = null;
                switch (indStr)
                {
                    case "wave":
                        temp = "#" + waveOrAntennaName + "\r\n";
                        break;
                    case "antenna":
                        temp = "*" + waveOrAntennaName + "\r\n";
                        break;
                    default:
                        LogFileManager.ObjLog.debug("参数" + indStr + "不是函数WaveOrAntennaInUse()的合法参数");
                        break;
                }
                if (matchStr.IndexOf(temp) != -1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                b = false;
            }
            return b;
        }
        public static bool DelBackIndStr(string indStr)
        {
            bool b = false;
            try
            {
                string initStr = FileOperation.ReadFile(MainWindow.relationOfAntAndWavePath);
                int site = initStr.IndexOf(indStr);
                while (initStr[site] != '\r')
                {
                    initStr = initStr.Remove(site, 1);
                }
                initStr = initStr.Remove(site, 2);
                FileOperation.WriteFile(initStr, MainWindow.relationOfAntAndWavePath, false);
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message,e);
            }
            return b;
        }
    }
}
