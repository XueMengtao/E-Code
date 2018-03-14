using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CalNode_server
{
    class RXSplit_server
    {
        public static void FileSplit(string filepath)
        {
            //获取打开的文件的名称及文件的目录
            FileInfo rx = new FileInfo(filepath);
            string[] a = new string[] { "." };
            string[] rxname = rx.Name.Split(a, StringSplitOptions.RemoveEmptyEntries);
            string dirname = rx.DirectoryName;
            try
            {
                StreamReader sr = rx.OpenText();
                string s = sr.ReadToEnd();
                if (s == "")
                    throw new nullException_server("the file is empty");
                //if (s == null)
                //Console.WriteLine("file is empty");

                else
                {

                    //设置分割字符串对文件内容进行分割
                    string[] strseperator = new string[] { "begin_<points>", "begin_<grid>", "begin_<situation>" };
                    string[] result = null;
                    result = s.Split(strseperator, StringSplitOptions.RemoveEmptyEntries);
                    int i = result.Length;
                    //创建分割的tx文件
                    for (int j = 1; j <= i; j++)
                    {
                        string newpath = dirname + "\\" + rxname[0] + j.ToString() + "." + rxname[1];
                        System.IO.FileInfo newrx = new System.IO.FileInfo(newpath);
                        using (StreamWriter sw = newrx.CreateText())
                        {
                            int m = result[j - 1].IndexOf("end_<points>");
                            int n = result[j - 1].IndexOf("end_<grid>");
                            if (m != -1)
                            {
                                sw.Write(strseperator[0]);
                                sw.Write(result[j - 1]);
                                sw.Close();
                            }
                            else
                            {
                                if (n != -1)
                                {
                                    sw.Write(strseperator[1]);
                                    sw.Write(result[j - 1]);
                                    sw.Close();
                                }
                                else
                                {
                                    sw.Write(strseperator[2]);
                                    sw.Write(result[j - 1]);
                                    sw.Close();
                                }
                            }
                        }
                    }
                }
                sr.Close();
            }
            catch (nullException_server e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            File.Delete(filepath);
        }
    }
}
