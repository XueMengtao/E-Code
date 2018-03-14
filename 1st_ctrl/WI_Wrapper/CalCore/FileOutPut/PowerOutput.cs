using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CalculateModelClasses;
using RayCalInfo;
using FileOutPut;
//using RayCalInfo;
namespace FileOutput
{
    public class PowerOutput : P2mFileOutput
    {

        public static void OutPath(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveBall rxBall, Point txPosition,int Frequence,int minFrequence,int maxFrequence)//需要提供一个接受区域所有路径的信息
        {
            //sPath += projectName;
            ReceiveBall Rxball = rxBall;
            string RxName = Rxball.RxName;
            string groupNum = Convert.ToString(Rxball.RxNum);
            if (Rxball.RxNum < 10)
            {
                groupNum = "0" + groupNum;
            }
            FileStream fs = new FileStream(sPath + projectName + "_power" + "_t00" + txIndex + "_0" + txTotol + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + "_r0" + groupNum + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);
            if (Paths.Count != 0)
            {
                OutputRxPower(Paths[0].node[Paths[0].node.Count - 1], RxName,Power.GetTotalPowerInDifferentPhase(Paths), txPosition.GetDistance(rxBall.Receiver));
                
            }
            else
            {

                double distance = Rxball.Receiver.GetDistance( txPosition);
                sb.AppendLine("# Receiver Set:" + RxName);
                sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)     Distance (m) Power (dBm)  Phase (Deg.)");
                sb.AppendLine("     " + 1 + "  " + Rxball.Receiver.X.ToString("0.#######E+00") + "  " + Rxball.Receiver.Y.ToString("0.#######E+00") + "     " + Rxball.Receiver.Z.ToString("f3") + "    " + distance.ToString("f2") + "       " + "-250.00" + "       " + "0.00");


            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }

        public static void GetRxTotalPower(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveBall rxballtemp, Point txpotemp)
        {

            ReceiveBall Rxball = rxballtemp;
            string Rxname = Rxball.RxName;
            string groupnum = Convert.ToString(Rxball.RxNum);
            if (Rxball.RxNum < 10)
            {
                groupnum = "0" + groupnum;
            }
            FileStream fs = new FileStream(sPath + projectName + "_power" + "_t00" + txIndex + "_0" + txTotol + "_" + "total" + "_r0" + groupnum + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);
            if (Paths.Count != 0)
            {
                OutputRxPower(Paths[0].node[Paths[0].node.Count - 1], Rxname,Power.GetTotalPowerInDifferentPhase(Paths), txpotemp.GetDistance(rxballtemp.Receiver));
                
            }
            else
            {

                double distance = Rxball.Receiver.GetDistance( txpotemp);
                sb.AppendLine("# Receiver Set:" + Rxname);
                sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)     Distance (m) Power (dBm)  Phase (Deg.)");
                sb.AppendLine("     " + 1 + "  " + Rxball.Receiver.X.ToString("0.#######E+00") + "  " + Rxball.Receiver.Y.ToString("0.#######E+00") + "     " + Rxball.Receiver.Z.ToString("f3") + "    " + distance.ToString("f2") + "       " + "-250.00" + "       " + "0.00");


            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }

        public static void OutPath1(List<CalculateModelClasses.Path> Paths, string sPath)
        {
            FileStream fs = new FileStream(sPath + @"PowerOutputNull.p2m", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
            fs.Close();
        }
        public static void OutPathArea(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveArea rArea, Terrain ter,ref Dictionary<int, List<Plural>> projectionResult , int Frequence, int minFrequence, int maxFrequence)
        {
            string Rxname = Paths[0].node[Paths[0].node.Count - 1].NodeName;
            Console.WriteLine(sPath + projectName + "_power" + "_t00" + txIndex + "_0" + txTotol + ".p2m");
            FileStream fs = new FileStream(sPath + projectName + "_" + "situation" + "_power" + "_t00" + txIndex + "_0" + txTotol + "_" + Frequence.ToString("D4") + "_" + minFrequence.ToString("D4") + "_" + maxFrequence.ToString("D4") + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
            StreamWriter sw = new StreamWriter(fs);
            List<int> rxCounter = AreaUtils.getCount4Rx(rArea);
            int Rxnum = rxCounter[0] * rxCounter[1];
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)       Distance (m)   Power (dBm)    Phase (Deg.)");
            List<CalculateModelClasses.Path> omg = AreaUtils.getPaths(Paths);
            projectionResult = AreaUtils.yingshe(rxCounter, omg, rArea);
            double temp = rArea.temp;
            foreach (var item in projectionResult.Keys)
            {
                Point receiverPoint = AreaUtils.GetCenterPointInAreaDivision(rArea, rxCounter, ter, item, temp);
                double distance = Paths[0].node[0].Position.GetDistance(receiverPoint);
                Plural tempPower = projectionResult[item][0];
                double angle = Math.Atan(tempPower.Im / tempPower.Re) * 180 / Math.PI;
                if (angle < 0)
                {
                    angle = angle + 180;
                }
                if (tempPower.Re == 0 && tempPower.Im == 0)
                {
                    sb.AppendLine(AreaUtils.getBlank(item, 6) + item + "  " + receiverPoint.X.ToString("E7") + "  " + receiverPoint.Y.ToString("E7") + "     " + receiverPoint.Z.ToString("f3") + "       " + distance.ToString("f2") + "        " + "-100.00" + "          " + "0.00");
                }
                else
                {
                    double powerMag = (10 * Math.Log10(Math.Sqrt(Math.Pow(tempPower.Re, 2) + Math.Pow(tempPower.Im, 2)))) + 30;
                    //string test = AreaUtils.getBlank((int)Math.Floor(powerMag), 11);
                    //AreaUtils.getBlank((int)Math.Floor(angle), 11);
                    //string test2 = AreaUtils.getBlank(int.Parse(angle.ToString()), 11);
                    sb.AppendLine(AreaUtils.getBlank(item, 6) + item + "  " + receiverPoint.X.ToString("E7") + "  " + receiverPoint.Y.ToString("E7") + "     " + receiverPoint.Z.ToString("f3") + "       " + distance.ToString("f2") + AreaUtils.getBlank((int)Math.Floor(powerMag), 11) + powerMag.ToString("f2") + AreaUtils.getBlank((int)Math.Floor(angle), 11) + angle.ToString("f2"));
                }
            }
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }

        public static void GetAreaTotalPower(List<CalculateModelClasses.Path> Paths, string sPath, string projectName, int txIndex, int txTotol, ReceiveArea rArea, Terrain ter)
        {
            if (Paths.Count != 0)
            {
                string Rxname = Paths[0].node[Paths[0].node.Count - 1].NodeName;
                Console.WriteLine(sPath + projectName + "_power" + "_t00" + txIndex + "_0" + txTotol + ".p2m");
                FileStream fs = new FileStream(sPath + projectName + "_" + "situation" + "_power" + "_total" + "_" + "t00" + txIndex + "_0" + txTotol  + ".p2m", FileMode.Create);//如果是接收区域的话，多个接收点组成一组接收机组成一个编号
                StreamWriter sw = new StreamWriter(fs);
                List<int> rxCounter = AreaUtils.getCount4Rx(rArea);
                int Rxnum = rxCounter[0] * rxCounter[1];
                sb.AppendLine("# Receiver Set:" + Rxname);
                sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)       Distance (m)   Power (dBm)    Phase (Deg.)");
                List<CalculateModelClasses.Path> omg = AreaUtils.getPaths(Paths);
                Console.WriteLine("Power begin" + System.DateTime.Now);
                Dictionary<int, List<Plural>> projectionResult = AreaUtils.yingshe(rxCounter, omg, rArea);
                Console.WriteLine("Power finish" + System.DateTime.Now);
                double temp = rArea.temp;
                foreach (var item in projectionResult.Keys)
                {
                    Point receiverPoint = AreaUtils.GetCenterPointInAreaDivision(rArea, rxCounter, ter, item, temp);
                    double distance =Paths[0].node[0].Position.GetDistance(receiverPoint);
                    Plural tempPower = projectionResult[item][0];
                    //角度计算，从局域信息中读取
                    double angle = projectionResult[item][4].Re;
                    
                    if (tempPower.Re == 0 && tempPower.Im == 0)
                    {
                        sb.AppendLine(AreaUtils.getBlank(item, 6) + item + "  " + receiverPoint.X.ToString("E7") + "  " + receiverPoint.Y.ToString("E7") + "     " + receiverPoint.Z.ToString("f3") + "       " + distance.ToString("f2") + "        " + "-100.00" + "          " + "0.00");
                    }
                    else
                    {
                        double powerMag = (10 * Math.Log10(tempPower.Re)) + 30;
                        //string test = AreaUtils.getBlank((int)Math.Floor(powerMag), 11);
                        //AreaUtils.getBlank((int)Math.Floor(angle), 11);
                        //string test2 = AreaUtils.getBlank(int.Parse(angle.ToString()), 11);
                        sb.AppendLine(AreaUtils.getBlank(item, 6) + item + "  " + receiverPoint.X.ToString("E7") + "  " + receiverPoint.Y.ToString("E7") + "     " + receiverPoint.Z.ToString("f3") + "       " + distance.ToString("f2") + AreaUtils.getBlank((int)Math.Floor(powerMag), 11) + powerMag.ToString("f2") + AreaUtils.getBlank((int)Math.Floor(angle), 11) + angle.ToString("f2"));
                    }
                }
                projectionResult.Clear();
                sw.Write(sb);
                sb.Clear();
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(sPath + "t00" + txIndex + "_0" + txTotol + "TotalPowerOutputNull.p2m", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Close();
                fs.Close();
            }
        }

        private static void OutputRxPower(Node RxNode, string Rxname, double []totalPowerAndPhase, double distance)//接收机的名字,加到path里面
        {
            sb.AppendLine("# Receiver Set:" + Rxname);
            sb.AppendLine("#   Rx#      X(m)           Y(m)          Z(m)     Distance (m) Power (dBm)  Phase (Deg.)");
            
            double angle = totalPowerAndPhase[1];
            
            sb.AppendLine("     " + 1 + "  " + RxNode.Position.X.ToString("0.#######E+00") + "  " + RxNode.Position.Y.ToString("0.#######E+00") + "     " + RxNode.Position.Z.ToString("f3") + "    " + distance.ToString("f2") + "       " + ((10 * Math.Log10(Math.Sqrt(Math.Pow(totalPowerAndPhase[0], 2) ))) + 30).ToString("f2") + "       " + angle.ToString("f2"));//distance需要调用两点距离公式计算出来。
           
        }
    }
}






