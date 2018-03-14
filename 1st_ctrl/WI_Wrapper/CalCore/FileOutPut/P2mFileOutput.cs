using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CalculateModelClasses;
using FileOutPut;

namespace FileOutput
{
    public class P2mFileOutput
    {
        protected static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 总的函数调用接口
        /// </summary>
        /// <param name="Paths">所有接收机的所有路径</param>
        /// <param name="Rxname">接收机名字</param>
        /// <param name="groupnum">接收机的类型号</param>
        /// <param name="filePath">存放路径</param>
        /// <param name="ProjName">工程名</param>
        public static void p2mfileoutput(List<CalculateModelClasses.Path> Paths, string filePath, string ProjName, int TxIndex, int txTotol, int index, ReceiveBall rxball, Point txPosition ,int Frequence,int minFrequence,int maxFrequence)
        {
            lock (typeof(P2mFileOutput))
            {
                if (index == 0)
                {
                    if (!Directory.Exists(filePath))
                    {
                        //DeleteDir(filePath);
                        //Directory.Delete(filePath);
                        Directory.CreateDirectory(filePath);
                    }
                    //Directory.CreateDirectory(filePath);
                }

                    OutputDelegate output = PowerOutput.OutPath;
                    output += PathOutput.OutPath;
                    output += PlOutput.OutPath;                    
                    output += PgOutput.OutPath;
                    output += SpreadOutput.OutPath;
                    //output += ErmOutput.OutPath;
                    output += ExmagOutput.OutPath;
                    output += ExphsOutput.OutPath;
                    output += EymagOutput.OutPath;
                    output += EyphsOutput.OutPath;
                    output += EzmagOutput.OutPath;
                    output += EzphsOutput.OutPath;
                    output(Paths, filePath, ProjName, TxIndex, txTotol, rxball, txPosition, Frequence, minFrequence, maxFrequence);
                
            }
        }
        public static void p2mfileoutputArea(List<CalculateModelClasses.Path> Paths, string filePath, string ProjName, int TxIndex, int txTotol, ReceiveArea rArea, Terrain ter, int index, int Frequence, int minFrequence, int maxFrequence)
        {
            if (maxFrequence > 9999)
            { throw new Exception("频率过大"); }
            lock (typeof(P2mFileOutput))
            {
                if (index == 0)
                {
                    if (!Directory.Exists(filePath))
                    {
                        //DeleteDir(filePath);
                        //Directory.Delete(filePath);
                        Directory.CreateDirectory(filePath);
                    }
                    //Directory.CreateDirectory(filePath);
                }
                if (Paths.Count != 0)
                {
                    Dictionary<int, List<Plural>> projectionResult = null;
                    OutputDelegateArea output = PowerOutput.OutPathArea;
                    output += ErmOutput.OutPathArea;
                    output(Paths, filePath, ProjName, TxIndex, txTotol, rArea, ter,ref projectionResult,Frequence, minFrequence, maxFrequence);
                    projectionResult.Clear();
                    GC.Collect();
                }
                else
                {
                    OutputDelegate1 output1 = PowerOutput.OutPath1;
                    output1 += ErmOutput.OutPath1;
                    output1(Paths, filePath);
                }
            }
        }

        //protected static double GetTolPower(List<CalculateModelClasses.Path> RxPaths)
        //{
        //    double tolPower = 0;
        //    foreach (CalculateModelClasses.Path path in RxPaths)
        //    {
        //        tolPower += path.node[path.node.Count - 1].Power;
        //    }
        //    return tolPower;
        

        //}
        protected static Plural GetTolEx(List<CalculateModelClasses.Path> RxPaths)
        {
            Plural TolEx = new Plural();
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                TolEx += path.node[path.node.Count - 1].TotalE.X;
            }
            return TolEx;
        }
        protected static Plural GetTolEy(List<CalculateModelClasses.Path> RxPaths)
        {
            Plural TolEy = new Plural();
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                TolEy += path.node[path.node.Count - 1].TotalE.Y;
            }
            return TolEy;
        }
        protected static Plural GetTolEz(List<CalculateModelClasses.Path> RxPaths)
        {
            Plural TolEz = new Plural();
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                TolEz += path.node[path.node.Count - 1].TotalE.Z;
            }
            return TolEz;
        }
        protected static double GetTolTime(List<CalculateModelClasses.Path> RxPaths)
        {
            double tolTime = 0;
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                tolTime += path.Delay;
            }
            return tolTime;
        }
        protected static double GetTolDelay(List<CalculateModelClasses.Path> RxPaths)
        {
            double tolDelay = 0;
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                tolDelay += path.Delay;
            }
            return tolDelay;
        }
        protected static double GetPassLoss(List<CalculateModelClasses.Path> RxPaths)
        {
            double passLoss = 0;
            for (int j = 0; j < RxPaths.Count; j++)
            {
                passLoss += RxPaths[j].pathloss;
            }
            passLoss /= RxPaths.Count;
            return passLoss;
        }


        private delegate void OutputDelegate(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveBall rxball, Point txPosition, int Frequence, int minFrequence, int maxFrequence);
        private delegate void OutputDelegate1(List<CalculateModelClasses.Path> Paths, string sPath);
        private delegate void OutputDelegateArea(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveArea rArea, Terrain ter,ref Dictionary<int, List<Plural>> projectionResult ,int Frequence, int minFrequence, int maxFrequence);
        private static void DeleteDir(string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
                string[] fileList = System.IO.Directory.GetFiles(aimPath);
                //string[] fileList = Directory.GetFileSystemEntries(aimPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        DeleteDir(aimPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Delete文件
                    else
                    {
                        System.IO.File.Delete(aimPath + System.IO.Path.GetFileName(file));
                    }
                }
                //删除文件夹
                System.IO.Directory.Delete(aimPath, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
