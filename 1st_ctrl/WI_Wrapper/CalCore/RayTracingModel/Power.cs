using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using UanFileProceed;
using System.Configuration;
using TxRxFileProceed;
using System.Collections;

namespace RayCalInfo
{
    public class Power
    {

        #region
        /// <summary>
        /// 获得beta的值（王楠）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="node1"></param>
        /// <returns></returns>
        static private double GetBeta(Node node, Node node1)
        {
            double[] ftx = GetFr(node);//发射端的频率数组
            double[] frx = GetFr(node1);//接收端的频率数组
            double fr1 = Math.Min(ftx[0], frx[0]);//。。。。。。。。。。。Math.Min(frx[0], ftx[0]);
            double fr2 = Math.Max(ftx[1], frx[1]);//。。。。。。。。。。。Math.Max(ftx[1], frx[1]);
            double fr = fr1 - fr2;//接受和发射的重叠带宽
            double ft = ftx[0] - ftx[1];//发射带宽
            if (ft != 0)
            {
                double result = fr / ft;//两式相除得出beta的值
                return result;
            }
            else
            {
                throw new Exception("发射频率范围不能为0");
            }

        }

        /// <summary>
        /// 获得频率的上下限（王楠）
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>数组fr[0]上限fr[1]下限</returns>
        static private double[] GetFr(Node node)
        {
            double[] fr = new double[2];
            //频率上限
            fr[0] = node.Frequence + node.FrequenceWidth / 2;
            //频率下限
            fr[1] = node.Frequence - node.FrequenceWidth / 2;
            return fr;
        }



        #endregion
        /// <summary>
        /// 计算每条路径的功率（王楠）
        /// </summary>
        /// <param name="Rx_uan">接收机UAN文件</param>
        /// <param name="finalNode">最后一个节点</param>
        /// <param name="parentNode">倒数第二个节点</param>
        /// <returns></returns>
        static public double[] GetPower(Node finalNode, Node parentNode)
        {
            //先计算前面的系数temp1
            double lamada = 300 / finalNode.Frequence;
            double temp1 = (Math.Pow(lamada, 2) * GetBeta(finalNode, parentNode)) / (8 * Math.PI * 377);//377是真空中的本征阻抗  
            //在计算后面的模的平方
            EField totalE = finalNode.TotalE;//获得接收点场强
            Point finalPosition = finalNode.Position;//获得接收点的位置
            Point parentPosition = parentNode.Position;//上一节点的位置
            //用两点的位置信息确定天线的theta和phi方向的增益和相位。
            double ThetaAngle = ReadUan.GetThetaAngle(parentPosition, finalPosition);//获得射线的theta角thetaAngle
            double phiAngle = ReadUan.GetPhiAngle(parentPosition, finalPosition);//获得射线的phi角phiAngle
            int index = Convert.ToInt32(Math.Floor(phiAngle * 181 + ThetaAngle));//将相应的index对应到读取UAN所形成的数组中
            double thetagain1 = Convert.ToDouble(ReadUan.thetaGain[index]);//天线theta方向上的增益
            double phigain1 = Convert.ToDouble(ReadUan.phiGain[index]);//天线phi方向上的增益
            double thetaPhase1 = Convert.ToDouble(ReadUan.thetaPhase[index]);//天线theta方向上的相位
            double phiPhase1 = Convert.ToDouble(ReadUan.phiPhase[index]);//天线phi方向上的相位
                                                                         //求theta方向的场强
            Plural Etheta = new Plural(totalE.X.Re * Math.Cos(ThetaAngle) * Math.Cos(phiAngle) + totalE.Y.Re * Math.Cos(ThetaAngle) * Math.Sin(phiAngle) - totalE.Z.Re * Math.Sin(ThetaAngle), totalE.X.Im * Math.Cos(ThetaAngle) * Math.Cos(phiAngle) + totalE.Y.Im * Math.Cos(ThetaAngle) * Math.Sin(phiAngle) - totalE.Z.Im * Math.Sin(ThetaAngle));
            //求phi方向的场强
            Plural Ephi = new Plural(totalE.X.Re * Math.Sin(phiAngle) * (-1) + totalE.Y.Re * Math.Cos(phiAngle), totalE.X.Im * Math.Sin(phiAngle) * (-1) + totalE.Y.Im * Math.Cos(phiAngle));

            //求g在theta和phi方向的向量
            Plural thetaGainPlural = new Plural(Math.Sqrt(Math.Pow(10, thetagain1 / 10.0)) * Math.Cos(thetaPhase1), Math.Sqrt(Math.Pow(10, thetagain1 / 10.0)) * Math.Sin(thetaPhase1));
            Plural phiGainPlural = new Plural(Math.Sqrt(Math.Pow(10, phigain1 / 10.0)) * Math.Cos(phiPhase1), Math.Sqrt(Math.Pow(10, phigain1 / 10.0)) * Math.Sin(phiPhase1));
            //求得模的平方temp2
            Plural temp2 = (Etheta * thetaGainPlural + Ephi * phiGainPlural);
            double temp3 = Math.Pow((temp2.GetMag()), 2);
            double phase = 0;
            if (temp2.Re == 0)
            {
                if (temp2.Im >= 0)
                {
                    phase = 90;
                }
                else
                { phase = -90; }
            }
            if (temp2.Im >= 0 && temp2.Re > 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI;
            }
            if (temp2.Im <= 0 && temp2.Re > 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI;
            }
            if (temp2.Im >= 0 && temp2.Re < 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI + 180;
            }
            if (temp2.Im <= 0 && temp2.Re < 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI - 180;
            }

            double[] powerAndPhase = new double[2];
            powerAndPhase[0] = temp1 * temp3;
            powerAndPhase[1] = phase;
            return powerAndPhase;
        }

        static public double[] GetTotalPowerInDifferentPhase(List<Path> areaPaths)
        {
            //在计算后面的模的平方
            Plural temp2 = new Plural(0,0);
            double[] frequence = new double[areaPaths.Count];
            int i=0;
            foreach (Path item in areaPaths)
            {
                frequence[i]=item.node[item.node.Count-1].Frequence;
                i++;

                EField totalE = item.node[item.node.Count - 1].TotalE;//获得最后一个节点rx的场强
                Point finalPosition = item.node[item.node.Count - 1].Position;
                Point parentPosition = item.node[item.node.Count - 2].Position;
                double ThetaAngle = ReadUan.GetThetaAngle(parentPosition, finalPosition);//获得射线的theta角thetaAngle
                double phiAngle = ReadUan.GetPhiAngle(parentPosition, finalPosition);//获得射线的phi角phiAngle
                int index = Convert.ToInt32(Math.Floor(phiAngle * 181 + ThetaAngle));//将相应的index对应到读取UAN所形成的数组中
                double thetagain1 = Convert.ToDouble(ReadUan.thetaGain[index]);//天线theta方向上的增益
                double phigain1 = Convert.ToDouble(ReadUan.phiGain[index]);//天线phi方向上的增益
                double thetaPhase1 = Convert.ToDouble(ReadUan.thetaPhase[index]);//天线theta方向上的相位
                double phiPhase1 = Convert.ToDouble(ReadUan.phiPhase[index]);//天线phi方向上的相位
                //求theta方向的场强
                Plural Etheta = new Plural(totalE.X.Re * Math.Cos(Math.PI*ThetaAngle/180.0) * Math.Cos(Math.PI*phiAngle/180.0) + totalE.Y.Re * Math.Cos(Math.PI*ThetaAngle/180.0) * Math.Sin(Math.PI*phiAngle/180.0) - totalE.Z.Re * Math.Sin(Math.PI*ThetaAngle/180.0), totalE.X.Im * Math.Cos(Math.PI*ThetaAngle/180.0) * Math.Cos(Math.PI*phiAngle/180.0) + totalE.Y.Im * Math.Cos(Math.PI*ThetaAngle/180.0) * Math.Sin(Math.PI*phiAngle/180.0) - totalE.Z.Im * Math.Sin(Math.PI*ThetaAngle/180.0));
                //求phi方向的场强
                Plural Ephi = new Plural(totalE.X.Re * Math.Sin(phiAngle * Math.PI / 180.0) * (-1) + totalE.Y.Re * Math.Cos(phiAngle * Math.PI / 180.0), totalE.X.Im * Math.Sin(phiAngle * Math.PI / 180.0) * (-1) + totalE.Y.Im * Math.Cos(Math.PI * phiAngle / 180.0));

                //求g在theta和phi方向的向量
                Plural thetaGainPlural = new Plural((Math.Pow(10, thetagain1 /20.0)) * Math.Cos(thetaPhase1 * Math.PI / 180.0), (Math.Pow(10, thetagain1 / 20.0)) * Math.Sin(thetaPhase1 * Math.PI / 180.0));
                Plural phiGainPlural = new Plural((Math.Pow(10, phigain1 / 20.0)) * Math.Cos(phiPhase1 * Math.PI / 180.0), (Math.Pow(10, phigain1 / 20.0)) * Math.Sin(phiPhase1 * Math.PI / 180.0));
                //求得模的平方temp2
                //Plural mul1 = Etheta * thetaGainPlural;
                //Plural mul2 = Ephi * phiGainPlural;
                 temp2 += (Etheta * thetaGainPlural + Ephi * phiGainPlural);

            }
            Array.Sort(frequence);
            
            //先计算前面的系数temp1
            double lamada = 300 / frequence[(int)(frequence.Length/2)];
            double temp1 = (Math.Pow(lamada, 2) )/ (8 * Math.PI * 377);//377是真空中的本征阻抗  
            double temp3 = Math.Pow((temp2.GetMag()), 2);
            double phase = 0;
            if (temp2.Re == 0)
            {
                if (temp2.Im >= 0)
                {
                    phase = 90;
                }
                else
                { phase = -90; }
            }
            if (temp2.Im >= 0 && temp2.Re > 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI;
            }
            if (temp2.Im <= 0 && temp2.Re > 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI;
            }
            if (temp2.Im >= 0 && temp2.Re < 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI + 180;
            }
            if (temp2.Im <= 0 && temp2.Re < 0)
            {
                phase = Math.Atan(temp2.Im / temp2.Re) * 180.0 / Math.PI - 180;
            }
             
            double[] powerAndPhase = new double[2];
            powerAndPhase[0] = temp1 * temp3;
            powerAndPhase[1] = phase;
            return powerAndPhase;
        }
        
    }
}
