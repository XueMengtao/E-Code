using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalLog
{
    public class LogForCal
    {
        // <summary>
        /// 日志文件记录
        /// </summary>
        /// <param name="msg">写入信息</param>
        public static void WriteMsg(string msg)
        {
            string path = Path.Combine("./log");
            if (!Directory.Exists(path))//判断是否有该文件
                Directory.CreateDirectory(path);
            string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//生成日志文件
            if (!File.Exists(logFileName))//判断日志文件是否为当天
                File.Create(logFileName);//创建文件
            StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流
            writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + msg);
            writer.Flush();
            writer.Close();
        }
    }
}
