using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FileObject;
using System.IO;
using log4net;
namespace TerFileProceed
{
  
    public class ReadTerMaterial
    {
        private static ILog myLog = LogManager.GetLogger(typeof(ReadTerMaterial));
        private static int ReadTerIndex = 0;
        private static List<KeyValuePair<string, List<double>>> materials = new List<KeyValuePair<string, List<double>>>();
        //提取地形中的材料信息，返回材料编号及相对应的导电率（0）和介电常数（1），
        public static List<KeyValuePair<string, List<double>>> GetMaterial(string ter)
        {
            if (materials.Count == 0)
            {
                DateTime statime = DateTime.Now;
                // List<KeyValuePair<string, List<double>>> materials = new List<KeyValuePair<string, List<double>>>();
                string material = MaterialInfo(ter);
                string[] strseperator = new string[] { "begin_<Material>" };
                string[] result = null;
                result = material.Split(strseperator, StringSplitOptions.RemoveEmptyEntries);
                double conduct, permit;
                for (int i = 0; i < result.Length; i++)
                {
                    int length = FileObject.Tool.readline(result[i], result[i].IndexOf("Material"));
                    string MaterialNum = result[i].Substring(result[i].IndexOf("Material"), length);
                    if (result[i].IndexOf("conductivity ") != -1)
                    {
                        length = FileObject.Tool.readline(result[i], result[i].IndexOf("conductivity ") + 13);
                        conduct = Convert.ToDouble(result[i].Substring(result[i].IndexOf("conductivity ") + 13, length));
                    }
                    else conduct = 0;
                    if (result[i].IndexOf("permittivity ") != -1)
                    {
                        length = FileObject.Tool.readline(result[i], result[i].IndexOf("permittivity ") + 13);
                        permit = Convert.ToDouble(result[i].Substring(result[i].IndexOf("permittivity ") + 13, length));
                    }
                    else permit = 0;
                    materials.Add(new KeyValuePair<string, List<double>>(MaterialNum, new List<double>() { conduct, permit }));
                }
                DateTime finishtime = DateTime.Now;
                TimeSpan span = finishtime - statime;
                myLog.Debug("第" + (++ReadTerIndex) + "次从ter文件中收集三角面信息耗时" + span);
            }
            return materials;
        }

        static string MaterialInfo(string ter)
        {
            //StreamReader sr = new StreamReader(ter);
            //string s = sr.ReadToEnd();
            int m = ter.IndexOf("begin_<Material>");
            int n = ter.LastIndexOf("end_<Material>");
            return ter.Substring(m, n - m + 14);
        }
    }
}
