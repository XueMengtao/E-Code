using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using System.IO;
using RayCalInfo;
using UanFileProceed;

namespace FileOutput
{
    public class PathOutput : P2mFileOutput
    {
        private static StringBuilder sbtemp = new StringBuilder();
  
       // private static int index2 = 0;
       // private static List<string> strListTemp = new List<string>();
        /// <summary>
        /// 输出P2M格式的path文件
        /// </summary>
        /// <param name="gridPath">一个发射机和一组接收机所有的路径信息</param>
        /// <param name="Rxname">接收机的名字(点接收机还是区域接收机)</param>
        /// <param name="groupnum">这组接收机是第几组</param>
        public static void OutPath(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveBall rxballtemp, Point txpotemp, int Frequence, int minFrequence, int maxFrequence)//需要提供一个接受区域所有路径的信息<加上输入路径参数和工程名>
        {
            if (Paths.Count != 0)
            {
                ReceiveBall Rxball = rxballtemp;
                string Rxname = Rxball.RxName;
                string groupnum = Convert.ToString(Rxball.RxNum);
                if (Rxball.RxNum < 10)
                {
                    groupnum = "0" + groupnum;
                }

                FileStream fs = new FileStream(sPath + projectName + "_paths" + "_t00" + txIndex + "_0" + txTotol + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + "_r0" + groupnum + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
                StreamWriter sw = new StreamWriter(fs);
                OutEveryRxPath(Paths, Rxname, 1);
                sw.Write(sb);
                sb.Clear();
                sw.Close();
                fs.Close();
            }
            else
            {
                ReceiveBall Rxball = rxballtemp;
                string groupnum = Convert.ToString(Rxball.RxNum);
                if (Rxball.RxNum < 10)
                {
                    groupnum = "0" + groupnum;
                }
                FileStream fs = new FileStream(sPath + "PathOutputNull" + "_t00" + txIndex + "_r0" + groupnum + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + ".p2m", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Close();
                fs.Close();
            }
        }

        public static void OutPath1(List<List<CalculateModelClasses.Path>> Paths, string sPath)
        {
            FileStream fs = new FileStream(sPath + @"PathOutputNull.p2m", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 将直接过程辐射源和接收机的位置信息放到要输出的StringBuider对象中
        /// </summary>
        /// <param name="path">发射机和一组接收机所有的路径信息</param>
        private static void Direct(CalculateModelClasses.Path path)
        {

            sbtemp.AppendLine("Tx-Rx");

            string tempx1 = path.node[0].Position.X.ToString("0.#####E+00");
            string tempy1 = path.node[0].Position.Y.ToString("0.#####E+00");
            string tempz1 = path.node[0].Position.Z.ToString("f3");

            string tempx2 = path.node[path.node.Count - 1].Position.X.ToString("0.#####E+00");
            string tempy2 = path.node[path.node.Count - 1].Position.Y.ToString("0.#####E+00");
            string tempz2 = path.node[path.node.Count - 1].Position.Z.ToString("f3");

            sbtemp.AppendFormat("{0,20}{1,20}{2,20}", tempx1, tempy1, tempz1);
            sbtemp.Append("\r\n");
            sbtemp.AppendFormat("{0,20}{1,20}{2,20}", tempx2, tempy2, tempz2);
            sbtemp.Append("\r\n");

        }

        /// <summary>
        /// 将反射绕射过程中发射机接收机和其他各点的位置信息放到要输出的StringBuider对象中
        /// </summary>
        /// <param name="path">发射机和一组接收机所有的路径信息</param>
        private static void NotDirect(CalculateModelClasses.Path path)
        {
            string stylestring="Tx-";
            for (int i = 1; i < path.node.Count-1; i++)
            {

                if (path.node[i].NodeStyle == NodeStyle.ReflectionNode)
                    stylestring = stylestring + "R-";
                else
                    stylestring = stylestring + "D-";
            }
            stylestring = stylestring + "Rx";
            sbtemp.AppendLine(stylestring);
            string tempx, tempy, tempz;
            for (int i = 0; i < path.node.Count ; i++)
            {
                tempx = path.node[i].Position.X.ToString("0.#######E+00");
                tempy = path.node[i].Position.Y.ToString("0.#######E+00");
                tempz = path.node[i].Position.Z.ToString("f3");

                sbtemp.AppendFormat("{0,20}{1,20}{2,20}", tempx, tempy, tempz);
                sbtemp.Append("\r\n");
            }
           

        }
        private static void OutEveryRxPath(List<CalculateModelClasses.Path> paths, string Rxname, int Rxnum)//接收机的名字,加到path里面,Rxnum接收机的个数
        {
            int N=paths.Count;
            int index = 0;
            List<string> strListTemp = new List<string>();
            //StringBuilder sbtemp = new StringBuilder();
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("     " + Rxnum);//Rxnum是这一组有多少个接收机
            for (int i = 1; i <= Rxnum; i++)
            {
                sb.AppendLine("     " + i + "     " + "N");
                sb.Replace("N", N.ToString());
                if (paths.Count != 0)
                {
                    //string powertemp = ((10 * Math.Log10(Math.Sqrt(Math.Pow(P2mFileOutput.GetTolPower(paths), 2) ))) + 30).ToString("f2");
                    string powertemp = ((10 * Math.Log10(Math.Sqrt(Math.Pow(Power.GetTotalPowerInDifferentPhase(paths)[0], 2)))) + 30).ToString("f2");
                    //string powertemp = (10*Math.Log10(Math.Sqrt(Math.Pow(P2mFileOutput.GetTolPower(paths).Re, 2) + Math.Pow(P2mFileOutput.GetTolPower(paths).Im, 2)))).ToString("f2");
                    sb.AppendLine(powertemp + "   " + P2mFileOutput.GetTolTime(paths).ToString("0.#####E+00") + "  " + P2mFileOutput.GetTolDelay(paths).ToString("0.#####E+00"));
                    for (int j = 0; j < paths.Count; j++)
                    {
                        double distance = 0;
                        string theta_a = ReadUan.GetThetaAngle(paths[j].node[0].Position, paths[j].node[1].Position).ToString("f4");
                        string phi_a = ReadUan.GetPhiAngle(paths[j].node[0].Position, paths[j].node[1].Position).ToString("f4");
                        string theta_d = ReadUan.GetThetaAngle(paths[j].node[paths[j].node.Count - 2].Position, paths[j].node[paths[j].node.Count - 1].Position).ToString("f4");
                        string phi_d = ReadUan.GetPhiAngle(paths[j].node[paths[j].node.Count - 2].Position, paths[j].node[paths[j].node.Count - 1].Position).ToString("f4");

                        for (int k = 0; k < paths[j].node.Count - 1; k++)
                        {
                            distance += paths[j].node[k].Position.GetDistance( paths[j].node[k + 1].Position);
                        }

                        //string temp2 = Math.Sqrt(Math.Pow(paths[j].node[paths[j].node.Count - 1].power.Re, 2) + Math.Pow(paths[j].node[paths[j].node.Count - 1].power.Im, 2)).ToString("f4");
                        string temp2 = ((10 * Math.Log10(Math.Sqrt(Math.Pow(paths[j].node[paths[j].node.Count - 1].Power, 2) ))) + 30).ToString("f4");
                        string weishuju = Math.Sqrt(Math.Pow(paths[j].node[paths[j].node.Count - 1].TotalE.Z.Re, 2) + Math.Pow(paths[j].node[paths[j].node.Count - 1].TotalE.Z.Im, 2)).ToString("f4");
                        sbtemp.AppendLine("    " + ("M") + "   " + (paths[j].node.Count - 2).ToString() + "     " + temp2 + "   " + (30 / distance).ToString("0.#####E+00") + "    " + theta_a + "   " + phi_a + "   " + theta_d + "   " + phi_d + "   " + weishuju);//达到时间需要计算，另外四个角度需要计算"(theta,phia,thetd,phid")
                        if (paths[j].node.Count == 2)
                        {
                            //if (paths[j].node[1].PointStyle.Equals("FinalPoint"))
                            //{
                            //    continue;
                            //}
                            //else
                            //{
                            //    Direct(paths[j]);
                            //    string[] temp = sbtemp.ToString().Split('|');
                            //    //检验是否重复
                            //    check(temp, strListTemp);
                            //    //清空临时字符串
                            //    sbtemp.Clear();
                            //}
                            Direct(paths[j]);
                            index++;
                            sb.Append(sbtemp.ToString().Replace("M",index.ToString()));
                            sbtemp.Clear();
                        }
                        else
                        {
                            NotDirect(paths[j]);
                            index++;
                            //string[] temp = sbtemp.ToString().Split('|');
                            //check(temp, strListTemp);
                            ////清空临时字符串
                            sb.Append(sbtemp.ToString().Replace("M",index.ToString()));
                            sbtemp.Clear();
                        }
                    }
                }
            }
        }

    }
}
