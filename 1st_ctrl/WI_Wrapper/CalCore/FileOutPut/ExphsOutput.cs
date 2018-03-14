using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CalculateModelClasses;
using FileOutput;
using RayCalInfo;

namespace FileOutPut
{
    class ExphsOutput : P2mFileOutput
    {
        public static void OutPath(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int TxIndex, int txTotol, ReceiveBall rxballtemp, Point txpotemp, int Frequence, int minFrequence, int maxFrequence)//需要提供一个接受区域所有路径的信息
        {

            ReceiveBall Rxball = rxballtemp;
            string Rxname = Rxball.RxName;
            string groupnum = Convert.ToString(Rxball.RxNum);
            if (Rxball.RxNum < 10)
            {
                groupnum = "0" + groupnum;
            }
            //sPath += projectName;
            FileStream fs = new FileStream(sPath + projectName + "_exphs" + "_t00" + TxIndex + "_0" + txTotol + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + "_r0" + groupnum + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);
            if (Paths.Count != 0)
            {
                OutputRxEXPhase(Paths[0].node[Paths[0].node.Count - 1], Rxname, txpotemp.GetDistance(rxballtemp.Receiver), GetTolEx(Paths));  
            }
            else
            {
                double distance =Rxball.Receiver.GetDistance( txpotemp);
                sb.AppendLine("# Receiver Set:" + Rxname);
                sb.AppendLine("#   Rx#      X(m)           Y(m)            Z(m)   Distance     Phase     Real        Imag");
                sb.AppendLine("     " + 1 + "  " + Rxball.Receiver.X.ToString("0.#######E+00") + "  " + Rxball.Receiver.Y.ToString("0.#######E+00") + "     " + Rxball.Receiver.Z.ToString("f3") + "       " + distance.ToString("f2") + "  " + "0.00000E-80" + "  " + "0.00001E-80" + "  " + "0.00002E-80");
            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }
        public static void OutPath1(List<List<CalculateModelClasses.Path>> Paths, string sPath)
        {
            FileStream fs = new FileStream(sPath + @"exphsOutputNull.p2m", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
            fs.Close();
        }

        private static void OutputRxEXPhase(Node RxNode, string Rxname, double distance, Plural tolEx)//接收机的名字,加到path里面
        {
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("#   Rx#      X(m)           Y(m)            Z(m)   Distance     Phase     Real        Imag");
            sb.AppendLine("     " + 1 + "  " + RxNode.Position.X.ToString("0.#######E+00") + "  " + RxNode.Position.Y.ToString("0.#######E+00") + "     " + RxNode.Position.Z.ToString("f3") + "       " + distance.ToString("f2") + "  " + Math.Atan2(tolEx.Im, tolEx.Re).ToString("0.#####E+00") + "  " + tolEx.Re.ToString("0.#####E+00") + "  " + tolEx.Im.ToString("0.#####E+00"));
        }
    }
}
