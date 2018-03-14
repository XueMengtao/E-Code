using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileOutput;
using System.IO;
using CalculateModelClasses;
using RayCalInfo;

namespace FileOutPut
{
    class ErmOutput : P2mFileOutput
    {
        public static void OutPath(List<List<CalculateModelClasses.Path>> Paths, string sPath, string projectName,int TxIndex,int txTotol)//需要提供一个接受区域所有路径的信息
        {
            string Rxname = Paths[0][0].node[Paths[0][0].node.Count - 1].NodeName;
            string groupnum = Paths[0][0].node[Paths[0][0].node.Count - 1].RxNum.ToString();
            if (Paths[0][0].node[Paths[0][0].node.Count - 1].RxNum < 10)
            {
                groupnum = "0" + groupnum;
            }
            //sPath += projectName;
            FileStream fs = new FileStream(sPath + projectName + "_erm" + "_t00" + TxIndex + "_0" + txTotol + "_r0" + groupnum + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);
            List<Node> RxNode = new List<Node>();
            for (int i = 0; i < Paths.Count; i++)
            {
                RxNode.Add(Paths[i][0].node[Paths[i][0].node.Count - 1]);
            }
            foreach (List<CalculateModelClasses.Path> path in Paths)
            {

            }
            for (int i = 0; i < Paths.Count; i++)
            {
                double passloss = 0;
                for (int j = 0; j < Paths[i].Count; j++)
                {
                    passloss += Paths[i][j].pathloss;
                }
                passloss /= Paths[i].Count;
                double distance = Paths[i][0].node[Paths[i][0].node.Count - 1].Position.GetDistance( Paths[i][0].node[0].Position);
                OutEveryRxPath(RxNode, Rxname, passloss, distance);
                
            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }
        public static void OutPathArea(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveArea rArea, Terrain ter,ref Dictionary<int, List<Plural>> projectionResult , int Frequence, int minFrequence, int maxFrequence)
        {
            string Rxname = Paths[0].node[Paths[0].node.Count - 1].NodeName;
            Console.WriteLine(sPath + projectName + "_erm" + "_t00" + txIndex + "_0" + txTotol + ".p2m");
            FileStream fs = new FileStream(sPath + projectName + "_" + "situation" + "_erm" + "_t00" + txIndex + "_0" + txTotol + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);
            List<int> rxCounter = AreaUtils.getCount4Rx(rArea);
            int Rxnum = rxCounter[0] * rxCounter[1];
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("#   Rx#      X(m)           Y(m)            Z(m)     Distance          Erms");
            if (projectionResult == null)
            {
                List<CalculateModelClasses.Path> omg = AreaUtils.getPaths(Paths);
                projectionResult = AreaUtils.yingshe(rxCounter, omg, rArea);
            }
            double temp = rArea.temp;
            foreach (var item in projectionResult.Keys)
            {
                Point receiverPoint = AreaUtils.GetCenterPointInAreaDivision(rArea, rxCounter, ter, item, temp);
                double distance = Paths[0].node[0].Position.GetDistance( receiverPoint);
                Plural exTotal = projectionResult[item][1];
                Plural eyTotal = projectionResult[item][2];
                Plural ezTotal = projectionResult[item][3];
                Plural eTotal = new Plural(exTotal.Re + eyTotal.Re + ezTotal.Re, exTotal.Im + eyTotal.Im + ezTotal.Im);
                sb.AppendLine(AreaUtils.getBlank(item, 6) + item + "  " + receiverPoint.X.ToString("E7") + "  " + receiverPoint.Y.ToString("E7") + "     " + receiverPoint.Z.ToString("f3") + "       " + distance.ToString("f2") + "  " + Math.Sqrt(Math.Pow(eTotal.Re, 2) + Math.Pow(eTotal.Im, 2)).ToString("E5"));
            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }
        public static void OutPath1(List<CalculateModelClasses.Path> Paths, string sPath)
        {
            FileStream fs = new FileStream(sPath + @"ermOutputNull.p2m", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
            fs.Close();
        }
        private static void OutEveryRxPath(List<Node> RxNode, string Rxname, double passloss, double distance)//接收机的名字,加到path里面
        {
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("#   Rx#      X(m)           Y(m)            Z(m)   Distance        Erms");
            for (int i = 0; i < RxNode.Count; i++)
            {
                sb.AppendLine(AreaUtils.getBlank(i,6) + (i + 1) + "  " + RxNode[i].Position.X.ToString("0.#######E+00") + "  " + RxNode[i].Position.Y.ToString("0.#######E+00") + "     " + RxNode[i].Position.Z.ToString("f3") + "       " + distance.ToString("f2") +"  ");
            }
        }
       

    }
}
