using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CalculateModelClasses;
using RayCalInfo;
using UanFileProceed;

namespace FileOutput
{
    public class SpreadOutput : P2mFileOutput
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
            FileStream fs = new FileStream(sPath + projectName + ".spread" + ".t00" + TxIndex + "_0" + txTotol + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + ".r0" + groupnum + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);

            if (Paths.Count != 0)
            {
                double[] distance = new double[Paths.Count];
                double[] SpreadTime = new double[Paths.Count];
                double[] pathPower = new double[Paths.Count];
                double averageTime = 0;
                double tolPower = Power.GetTotalPowerInDifferentPhase(Paths)[0];
                double spreadSigma = 0;
                double outputSigma = 0;
                for (int j = 0; j < Paths.Count; j++)
                {

                    for (int k = 0; k < Paths[j].node.Count - 1; k++)
                    {
                        distance[j] += Paths[j].node[k].Position.GetDistance( Paths[j].node[k + 1].Position);
                    }
                    SpreadTime[j] = distance[j] / 300000000;
                    pathPower[j] = Paths[j].node[Paths[j].node.Count - 1].Power;
                }

                for (int j = 0; j < Paths.Count; j++)
                {
                    averageTime += SpreadTime[j] * pathPower[j];
                }
                averageTime /= tolPower;

                for (int j = 0; j < Paths.Count; j++)
                {
                    spreadSigma += pathPower[j] * Math.Pow(SpreadTime[j], 2);
                }
                spreadSigma /= tolPower;
                spreadSigma -= Math.Pow(averageTime, 2);
                outputSigma = Math.Sqrt(Math.Abs(spreadSigma));
                double totaldistance = txpotemp.GetDistance(rxballtemp.Receiver);
                OutEveryRxPath(Paths[0].node[Paths[0].node.Count - 1], Rxname, outputSigma, totaldistance);
                
            }
            else
            {
                //double totaldistance = Intersection.Distance(Paths[0][0].node[Paths[0][0].node.Count - 1].position, Paths[0][0].node[0].position);
                double totaldistance = Rxball.Receiver.GetDistance( txpotemp);
                sb.AppendLine("# Receiver Set:" + Rxname);
                sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)     Distance (m) Delay Spread (s)");
                sb.AppendLine("     " + 1 + "  " + Rxball.Receiver.X.ToString("0.#######E+00") + "  " + Rxball.Receiver.Y.ToString("0.#######E+00") + "     " + Rxball.Receiver.Z.ToString("f3") + "       " + totaldistance.ToString("f2") + "       " + "NULL");
            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }

        private static void OutEveryRxPath(Node RxNode, string Rxname, double outputSigma, double totaldistance)//接收机的名字,加到path里面
        {
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)     Distance (m) Delay Spread (s)");
            sb.AppendLine("     " +  1 + "  " + RxNode.Position.X.ToString("0.#######E+00") + "  " + RxNode.Position.Y.ToString("0.#######E+00") + "     " + RxNode.Position.Z.ToString("f3") + "       " + totaldistance.ToString("f2") + "       " + outputSigma.ToString("0.#######E+00"));
        }
    }
}
