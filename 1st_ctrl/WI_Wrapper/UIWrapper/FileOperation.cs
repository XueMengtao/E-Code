using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace WF1
{
    class FileOperation
    {
       public  static void WriteFile(string msg, string filePath,bool b)
        {
            Encoding gbk = Encoding.GetEncoding("GBK");
            using (StreamWriter sw = new StreamWriter(filePath,b,gbk))
            {
                try
                {
                    sw.Write(msg);
                }
                catch (Exception exc)
                {
                    LogFileManager.ObjLog.debug(exc.Message);
                }
            }
        }
       public static string ReadFile(string filePath)
       {
           string resultStr = null;
           using (StreamReader sr = new StreamReader(filePath))
           {
               try
               {
                   resultStr = sr.ReadToEnd();                 
               }
               catch (Exception exc)
               {
                   LogFileManager.ObjLog.debug(exc.Message);
               }
               return resultStr;
           }
       }
       public static void WriteLineFile(string msg, string filePath,bool b)
       {
           Encoding gbk = Encoding.GetEncoding("GBK");
           using (StreamWriter sw = new StreamWriter(filePath,b,gbk))
           {
               try
               {
                   sw.WriteLine(msg);
               }
               catch (Exception exc)
               {
                   LogFileManager.ObjLog.debug(exc.Message);
               }
           }
       }
       public static string[] ReadLineFile(string filePath, int n)
       {
           string[] resultStr = new string[n];
           int i = 0;
           using (StreamReader sr = new StreamReader(filePath))
           {
               try
               {
                   while (!sr.EndOfStream)
                   {
                       resultStr[i] = sr.ReadLine();
                       i++;
                   }
               }
               catch (Exception exc)
               {
                   LogFileManager.ObjLog.debug(exc.Message);
               }
               return resultStr;
           }
       }
       public  static void RecoverOldRevision(string trFileStrTemp, string trFilePath, string transmitterParStrTemp, string transmitterNodeInfoTemp)
       {
           WriteFile(trFileStrTemp, trFilePath, false);
           WriteFile(transmitterParStrTemp, MainWindow.waveinfoFilePath, false);
           WriteFile(transmitterNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
       }
    }
}
