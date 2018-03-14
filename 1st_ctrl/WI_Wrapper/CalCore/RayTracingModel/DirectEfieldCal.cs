using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using UanFileProceed;
using System.IO;

namespace RayCalInfo
{
    public class DirectEfieldCal
    {
        public const int CSpeed = 300;
        //static List<string> thetaGain = new List<string>();  //这四个数组的值以后从uan格式的天线文件中获取
        //static List<string> phiGain = new List<string>();
        //static List<string> thetaPhase = new List<string>();
        //static List<string> phiPhase = new List<string>();

        //私有函数
        #region
        //求theta方向的电场强度
        static Plural DirectEThetaCal(string uan, double power, double frequency, Point originPoint, Point targetPoint, Point rotateAngle = null)
        {
            double r = Math.Sqrt(Math.Pow((originPoint.X - targetPoint.X), 2) + Math.Pow((originPoint.Y - targetPoint.Y), 2) + Math.Pow((originPoint.Z - targetPoint.Z), 2));
            Plural Etheta = new Plural();
            Plural PhaseAccumulate = new Plural(Math.Cos(2 * Math.PI * frequency * r / CSpeed) / r, -Math.Sin(2 * Math.PI * frequency * r / CSpeed) / r);
            Etheta = PhaseAccumulate * Atheta(uan, power, originPoint, targetPoint);
            return Etheta;
        }

        //求phi方向的电场强度
        static Plural DirectEPhiCal(string uan, double power, double frequency, Point originPoint, Point targetPoint, Point rotateAngle = null)
        {
            Plural Ephi = new Plural();
            double r = Math.Sqrt(Math.Pow((originPoint.X - targetPoint.X), 2) + Math.Pow((originPoint.Y - targetPoint.Y), 2) + Math.Pow((originPoint.Z - targetPoint.Z), 2));
            Plural PhaseAccumulate = new Plural(Math.Cos(2 * Math.PI * frequency * r / CSpeed) / r, -Math.Sin(2 * Math.PI * frequency * r / CSpeed) / r);
            Ephi = PhaseAccumulate * Aphi(uan, power, originPoint, targetPoint);
            return Ephi;
        }

        static Plural AreaAtheta(string uan, double power, Point originPoint, Point targetPoint)
        {
            //List<string> thetaGain = new List<string>();  //这四个数组的值以后从uan格式的天线文件中获取
            //List<string> phiGain = new List<string>();
            //List<string> thetaPhase = new List<string>();
            //List<string> phiPhase = new List<string>();
  //          ReadUan.GetGainPara(uan);
            Plural Atheta = new Plural();
            
                try
                {
                    int row = (ReadUan.GetPhiAngle(originPoint, targetPoint)) * 181 + ReadUan.GetThetaAngle(originPoint, targetPoint);
                    double A = Math.Sqrt(60 * power * Math.Abs(Math.Pow(10, Convert.ToDouble(ReadUan.thetaGain[row]) / 10)));
                    Plural theta = new Plural(Math.Cos(Math.PI / 180 * Convert.ToDouble(ReadUan.thetaPhase[row])),
                        Math.Sin(Math.PI / 180 * Convert.ToDouble(ReadUan.thetaPhase[row])));
                    Atheta = Plural.PluralMultiplyDouble(theta, A);
                    //Console.WriteLine("theta="+ReadUan .GetThetaAngle (originPoint ,targetPoint )+" phi="+ReadUan .GetPhiAngle (originPoint ,targetPoint ));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return Atheta;
            
        }

        static Plural Atheta(string uan, double power, Point originPoint, Point targetPoint)
        {
            //List<string> thetaGain = new List<string>();  //这四个数组的值以后从uan格式的天线文件中获取
            //List<string> phiGain = new List<string>();
            //List<string> thetaPhase = new List<string>();
            //List<string> phiPhase = new List<string>();
            //          ReadUan.GetGainPara(uan);
            Plural Atheta = new Plural();
            if (ReadUan.thetaGain.Count < 10)
            {
                return Atheta;
                throw new Exception("Could not get the value of Antenna thetaGain!");
            }
            else
            {
                try
                {
                    int row = (ReadUan.GetPhiAngle(originPoint, targetPoint)) * 181 + ReadUan.GetThetaAngle(originPoint, targetPoint);
                    double A = Math.Sqrt(60 * power * Math.Abs(Math.Pow(10, Convert.ToDouble(ReadUan.thetaGain[row]) / 10)));
                    Plural theta = new Plural(Math.Cos(Math.PI / 180 * Convert.ToDouble(ReadUan.thetaPhase[row])),
                        Math.Sin(Math.PI / 180 * Convert.ToDouble(ReadUan.thetaPhase[row])));
                    Atheta = Plural.PluralMultiplyDouble(theta, A);
                    //Console.WriteLine("theta="+ReadUan .GetThetaAngle (originPoint ,targetPoint )+" phi="+ReadUan .GetPhiAngle (originPoint ,targetPoint ));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return Atheta;
            }
        }

        //求得场强计算需要的Aphi
        static Plural Aphi(string uan, double power, Point originPoint, Point targetPoint)
        {
            //List<string> thetaGain = new List<string>();  //这四个数组的值以后从uan格式的天线文件中获取
            //List<string> phiGain = new List<string>();
            //List<string> thetaPhase = new List<string>();
            //List<string> phiPhase = new List<string>();
            //, out thetaGain, out phiGain, out thetaPhase, out phiPhase
 //           ReadUan.GetGainPara(uan);
            Plural Aphi = new Plural();
            if (ReadUan.phiGain.Count < 10)
            {
                return Aphi;
                throw new Exception("Could not get the value of Antenna thetaGain!");
            }
            else
            {
                try
                {
                    int row = (ReadUan.GetPhiAngle(originPoint, targetPoint)) * 181 + ReadUan.GetThetaAngle(originPoint, targetPoint);
                   // double A = 0;
                    double A = Math.Sqrt(60 * power * Math.Abs(Math.Pow(10, Convert.ToDouble(ReadUan.phiGain[row]) / 10)));
                    Plural phi = new Plural(Math.Cos(Math.PI / 180 * Convert.ToDouble(ReadUan.phiPhase[row])),
                      Math.Sin(Math.PI / 180 * Convert.ToDouble(ReadUan.phiPhase[row])));
                    Aphi = Plural.PluralMultiplyDouble(phi, A);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return Aphi;
            }
        }

        #endregion

        //求得电场的xyz方向的量 
        public static EField EfieldCal(string uan, double power1, double frequency, Point originPoint, Point targetPoint, Point rotateAngle = null)
        {
            double power = power1;     //传入的power1单位是dBm,转换为单位为瓦的power
    //      ReadUan.GetGainPara(uan);
            Plural Etheta = DirectEThetaCal(uan, power, frequency, originPoint, targetPoint, rotateAngle);
            Plural Ephi = DirectEPhiCal(uan, power, frequency, originPoint, targetPoint, rotateAngle);
    //        double The = ReadUan.GetThetaAngle(originPoint, targetPoint) * Math.PI / 180.0;
    //        double Ph = ReadUan.GetPhiAngle(originPoint, targetPoint) * Math.PI / 180.0;
            EField e = new EField();
            double thetaAngle = Convert.ToInt32(ReadUan.GetThetaAngle(originPoint, targetPoint));
            double phiAngle = Convert.ToInt32(ReadUan.GetPhiAngle(originPoint, targetPoint));
            double Xtheta = Math.Cos(thetaAngle * Math.PI / 180.0) * Math.Cos(phiAngle / 180 * Math.PI);
            double Xphi = Math.Sin(ReadUan.GetPhiAngle(originPoint, targetPoint) * Math.PI / 180.0);
            double Ytheta = Math.Cos(thetaAngle * Math.PI / 180.0) * Math.Sin(phiAngle / 180 * Math.PI);
            double Yphi = Math.Cos(phiAngle * Math.PI / 180.0);
            double Ztheta = Math.Sin(thetaAngle * Math.PI / 180.0);
            double Zphi = 0;
            e.X = Plural.PluralMultiplyDouble(Etheta, Xtheta) - Plural.PluralMultiplyDouble(Ephi, Xphi);
            e.Y = Plural.PluralMultiplyDouble(Etheta, Ytheta) + Plural.PluralMultiplyDouble(Ephi, Yphi);
            e.Z = Plural.PluralMultiplyDouble(Ephi, Zphi) - Plural.PluralMultiplyDouble(Etheta, Ztheta);
            //string pathtest = "D:\\renwu\\"+DateTime.Today.ToString("yy/MM/dd")+".txt";
            //File.WriteAllText(pathtest, e.X.Re + "  " + e.X.Im + "|  " + e.Y.Re + "  " + e.Y.Im + "|  " + e.Z.Re + "  " + e.Z.Im);
            //需添加计算方法
            return e;
        }

        public static EField EfieldCalSingle(string uan, double power1, double frequency, Point originPoint, Point targetPoint, Point rotateAngle = null)
        {
            double power = Math.Pow(10, (power1 - 30) / 10);     //传入的power1单位是dBm,转换为单位为瓦的power
//            ReadUan.GetGainPara(uan);

            Plural Etheta = DirectEThetaCal(uan, power, frequency, originPoint, targetPoint, rotateAngle);
            Plural Ephi = DirectEPhiCal(uan, power, frequency, originPoint, targetPoint, rotateAngle);
            //double The = ReadUan.GetThetaAngle(originPoint, targetPoint) * Math.PI / 180.0;
           // double Ph = ReadUan.GetPhiAngle(originPoint, targetPoint) * Math.PI / 180.0;
            EField e = new EField();
            int Xtheta = Convert.ToInt32(Math.Cos(ReadUan.GetThetaAngle(originPoint, targetPoint) * Math.PI / 180.0) * Math.Cos(ReadUan.GetPhiAngle(originPoint, targetPoint) / 180 * Math.PI));
            double Xphi = Convert.ToInt32(Math.Sin(ReadUan.GetPhiAngle(originPoint, targetPoint) * Math.PI / 180.0));
            double Ytheta =Convert.ToInt32( Math.Cos(ReadUan.GetThetaAngle(originPoint, targetPoint) * Math.PI / 180.0) * Math.Sin(ReadUan.GetPhiAngle(originPoint, targetPoint) / 180 * Math.PI));
            double Yphi =Convert.ToInt32( Math.Cos(ReadUan.GetPhiAngle(originPoint, targetPoint) * Math.PI / 180.0));
            double Ztheta = Convert.ToInt32(Math.Sin(ReadUan.GetThetaAngle(originPoint, targetPoint) * Math.PI / 180.0));
            double Zphi = 0;
            e.X = Plural.PluralMultiplyDouble(Etheta, Xtheta) - Plural.PluralMultiplyDouble(Ephi, Xphi);
            e.Y = Plural.PluralMultiplyDouble(Etheta, Ytheta) + Plural.PluralMultiplyDouble(Ephi, Yphi);
            e.Z = Plural.PluralMultiplyDouble(Ephi, Zphi) - Plural.PluralMultiplyDouble(Etheta, Ztheta);
            //需添加计算方法
            return e;
        }

    }
}
