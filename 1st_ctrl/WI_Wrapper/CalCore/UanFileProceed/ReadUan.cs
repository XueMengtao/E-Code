using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FileObject;
using SetupFileObject;
using CalculateModelClasses;
using LogFileManager;
using System.Collections;
//using System.Net.Cache;

namespace UanFileProceed
{
    /// <summary>
    ///获得uan文件中的theta方向和phi方向上的增益和与增益对应的相位
    /// </summary>
    public class ReadUan
    {
        //提取UAN文件的内容，供直射计算使用
        private static int indexUanRead = 0;
        private static int indexUanSplit = 0;
        private static string s1 = String.Empty;
        private static string s2 = String.Empty;
        public static List<string> thetaGain=new List<string>();
        public static List<string> phiGain=new List<string>();
        public static List<string> thetaPhase=new List<string>();
        public static List<string> phiPhase=new List<string>();

        /// <summary>
        /// 读取UAN（王楠）
        /// </summary>
        /// <param name="tx">发射</param>
        /// <param name="setup">设置</param>
        /// <returns>UAN字符串</returns>
        public static string GetUan(TxObject tx, SetupObject setup)
        {
            DateTime startime = DateTime.Now;//记录开始时间
            StreamReader sr = new StreamReader(GetUanPath(tx,setup));
            //string s = sr.ReadToEnd();
            if (string.IsNullOrEmpty(s1))//判断是否读到结尾
            {
                s1 = sr.ReadToEnd();//将读取的流转化为string类型
                DateTime finishtime = DateTime.Now;//记录结束时间
                TimeSpan span = finishtime - startime;//读取所花时间
                LogFileManager.ObjLog.debug("第" + (++indexUanRead) + "次读取uan文件到内存中耗时：" + span);
            }
            sr.Close();//读取关闭
            return s1;
        }

        /// <summary>
        /// 读取UAN（王楠）
        /// </summary>
        /// <param name="rx"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static string GetUan(RxObject rx, SetupObject setup)
        {
            DateTime startime = DateTime.Now;
            StreamReader sr = new StreamReader(GetUanPath(rx, setup));
            //string s = sr.ReadToEnd();
            if (string.IsNullOrEmpty(s2))
            {
                s2 = sr.ReadToEnd();
                DateTime finishtime = DateTime.Now;
                TimeSpan span = finishtime - startime;
                LogFileManager.ObjLog.debug("第" + (++indexUanRead) + "次读取uan文件到内存中耗时：" + span);
            }
            sr.Close(); 
            return s2;
        }

        //, out  List<string> thetaGain, out List<string> phiGain, out List<string> thetaPhase, out List<string> phiPhase
        /// <summary>
        ///从UAN中获得四个list存放直射中电场计算需要的参数（王楠）
        /// </summary>
        public static void GetGainPara(string uan)
        {
            DateTime startime = DateTime.Now;//记录开始时间
            //thetaGain = new List<string>();
            //phiGain = new List<string>();
            //thetaPhase = new List<string>();
            //phiPhase = new List<string>();
            if (uan.IndexOf("end_<parameters>") == null)//如果读取不到"end_<parameters>"行，输出“    ”
            {
                Console.WriteLine( "   ");
            }
            int start = uan.IndexOf("end_<parameters>");//记录开始"end_<parameters>"行，为开始行
            int linelength = readline(uan, start);//读取UAN的"end_<parameters>"这一行的长度
            string[] st;
            string[] st1 = new string[50];
            string str;
            start += linelength + 2;//这是/r和/n的长度
            int length = readline(uan, start);
            //if (thetaGain.Count == 0 && thetaGain.Count == 0 && thetaGain.Count == 0 && thetaGain.Count == 0)
             if(thetaGain.Count==0 && phiGain.Count==0 && thetaPhase.Count==0 && phiPhase.Count==0)
            {
                if (length > 44)//判断UAN文件有几列，因为UAN文件参数不一样多
                {
                    //两次for循环，遍历所有theta角phi角
                    for (int i = 0; i <= 360; i++)
                    {
                        for (int j = 0; j <= 180; j++)
                        {
                            linelength = readline(uan, start);//读取选定行（数据的某一行）的长度；
                            str = uan.Substring(start, linelength);//将这一行读取出来
                            st = str.Split(new Char[] { ' ' });//把这一行字符串，用’‘分割成数组
                            try
                            {
                                thetaGain.Add(st[4]);//将各个数据分别添加到4个list里面
                                phiGain.Add(st[6]);
                                thetaPhase.Add(st[8]);
                                phiPhase.Add(st[10]);
                                start += linelength + 2;//将开始索引加上这一行的长度再加2（/r /n）
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                    DateTime finishtime = DateTime.Now;//记录结束时间
                    TimeSpan span = finishtime - startime;//计算花费总时间
                    LogFileManager.ObjLog.debug("第" + (++indexUanSplit) + "次从内存中取uan数据并且提取数据到四个List中耗时：" + span);
                }
                    //这是uan文件只有4列的情况
                else
                {
                    //两次for循环，遍历所有theta角phi角
                    for (int i = 0; i <= 360; i++)
                    {
                        for (int j = 0; j <= 180; j++)
                        {
                            linelength = readline(uan, start);//读取选定行（数据的某一行）的长度；
                            str = uan.Substring(start, linelength);// 将这一行读取出来
                            st = str.Split(new Char[] { ' ' });//把这一行字符串，用’‘分割成数组
                            try
                            {
                                thetaGain.Add(st[4]);//将各个数据分别添加到4个list里面
                                phiGain.Add(st[6]);
                                thetaPhase.Add("0");
                                phiPhase.Add("0");
                                start += linelength + 2;//将开始索引加上这一行的长度再加2（/r /n）
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    DateTime finishtime = DateTime.Now;//记录结束时间
                    TimeSpan span = finishtime - startime;//计算花费总时间
                    LogFileManager.ObjLog.debug("第" + (++indexUanSplit) + "次从内存中取uan数据并且提取数据到四个List中耗时：" + span);
                }

                if (TxObject.test != null)
                {
                    if (!TxObject.test.at.rotation_x.Equals(0) && !TxObject.test.at.rotation_y.Equals(0) && !TxObject.test.at.rotation_z.Equals(0))
                    {
                        double[,] dataGain = new double[65341, 6];//声明一个二维的数组用来存储数据
                        double[,] temp = new double[65341, 6];
                        double[,] result;
                        for (int i = 0; i < thetaGain.Count; i++)
                        {
                            dataGain[i, 0] = i % 181;
                            dataGain[i, 1] = i / 181;
                            dataGain[i, 2] = Convert.ToDouble(thetaGain[i]);
                            dataGain[i, 3] = Convert.ToDouble(phiGain[i]);
                            dataGain[i, 4] = Convert.ToDouble(thetaPhase[i]);
                            dataGain[i, 5] = Convert.ToDouble(phiPhase[i]);
                            temp[i, 0] = dataGain[i, 0];
                            temp[i, 1] = dataGain[i, 1];
                            temp[i, 2] = dataGain[i, 2];
                            temp[i, 3] = dataGain[i, 3];
                            temp[i, 4] = dataGain[i, 4];
                            temp[i, 5] = dataGain[i, 5];
                        }
                        double[] step1 = { 1, 0, 0, TxObject.test.at.rotation_x };
                        double[] step2 = { 0, 1, 0, TxObject.test.at.rotation_y };
                        double[] step3 = { 0, 0, 1, TxObject.test.at.rotation_z };
                        Inputs(step1, dataGain);
                        Inputs(step2, dataGain);
                        Inputs(step3, dataGain);
                        result = interpolation(dataGain, temp);
                        for (int i = 0; i < thetaGain.Count; i++)
                        {
                            thetaGain[i] = result[i, 2].ToString();
                            phiGain[i] = result[i, 3].ToString();
                            thetaPhase[i] = result[i, 4].ToString();
                            phiPhase[i] = result[i, 5].ToString();
                        }
                    }
                }
                
            }

        }

        /// <summary>
        ///判断求得场点所在的射线的theta角（射线与z轴正方向的夹角，极坐标的方式（王楠））
        /// </summary>
        /// <param name="originNode">源点</param>
        /// <param name=="targetPoint">目标点</param>
        /// <return></return>
        public static int GetThetaAngle(Point originPoint, Point targetPoint)
        {

            int theta;
            double A = Math.Sqrt(Math.Pow(targetPoint.X - originPoint.X, 2) + Math.Pow(targetPoint.Y - originPoint.Y, 2));//求得A和B的在XOY面上的投影的长度
            double B = targetPoint.Z - originPoint.Z;//目标点的z坐标减去源点的z坐标
            double t = Math.Atan(A / B);//求反正切arctan（A/B）
            if (originPoint.X == targetPoint.X && originPoint.Y == targetPoint.Y && originPoint.Z == targetPoint.Z)//（判断如果A和B点重合那就输出无效）
                throw new Exception("场点位置与源点重合，无效！");
            else
            {
                if (t >= 0 && B >= 0) //求出的角度为往xoy平面上方射的
                    theta = Convert.ToInt32(t * 180 / Math.PI);//将弧度制转化为角度值
                else//求得的角度往xoy下面射时
                    theta = Convert.ToInt32(t * 180 / Math.PI) + 180;//将角度再加上180.
            }
            return theta;//返回theta角
        }

        /// <summary>
        ///判断求得场点所在的射线的Aphi角（王楠）
        /// </summary>
        /// <param name="originNode">源点</param>
        /// <param name=="targetPoint">目标点</param>
        /// <return></return>
        public static int GetPhiAngle(Point originPoint, Point targetPoint)
        {
            int phi;
            double m = Math.Sqrt(Math.Pow(targetPoint.X - originPoint.X, 2) + Math.Pow(targetPoint.Y - originPoint.Y, 2));//求A和B在底面的投影的长度
            double p = Math.Asin((targetPoint.Y - originPoint.Y) / m);//利用arcsin函数求得射线在xoy面上的投影与x轴的夹角
            if (originPoint.X == targetPoint.X && originPoint.Y == targetPoint.Y && originPoint.Z == targetPoint.Z)//当A和B点重合，则输出无效
                throw new Exception("场点位置与源点重合，无效！");
            else
            {
                if (m == 0)
                    phi = 0;//当射线沿着x轴方向时，phi角为0
                else if (p >= 0 && (targetPoint.X - originPoint.X) >= 0)//当射线往第一象限方向发射
                    phi = Convert.ToInt32(p * 180 / Math.PI);//将弧度制装化为角度值
                else if (p >= 0)//这里的else if表示了target.x-origin.x<0。这是往第二象限方向发射
                    phi = 180 - Convert.ToInt32(p * 180 / Math.PI);//转化为角度
                else if (p < 0 && (targetPoint.X - originPoint.X) >= 0)//往第四象限发射
                    phi = Convert.ToInt32(p * 180 / Math.PI) + 360;//将得到的角度加360，例如-45+360=315。
                else
                    phi = Convert.ToInt32(-p * 180 / Math.PI) + 180;//往第三象限方向射的，化为角度值
            }
            return phi;
        }

        /// <summary>
        ///根据发射机文件和setup文件得到UAN文件的路径
        /// </summary>
        static string GetUanPath(TxObject tx, SetupObject setup)
        {
            //用配置文件读取程序安装位置
            List<AntennaStp> antennas = setup.antenna.allAntennas;
            int i;
            for (i = 0; i < antennas.Count; i++)
            {
                if (tx.at.antennaNum == antennas[i].antenna)
                    break;
            }
            //DLL路径加上相对路径
            string path;
            if (antennas[i].uan_path == null)
            {
                path = GetDllPath() + "\\" + antennas[i].type + ".uan";
            }
            else
            {
                path = ".\\project\\" + setup.name +"\\"+ antennas[i].uan_path;
            }
            //
            return path;
        }
        static string GetUanPath(RxObject rx, SetupObject setup)
        {
            //用配置文件读取程序安装位置
            List<AntennaStp> antennas = setup.antenna.allAntennas;
            int i;
            for (i = 0; i < antennas.Count; i++)
            {
                if (rx.at.antennaNum == antennas[i].antenna)
                    break;
            }
            //DLL路径加上相对路径
            string path;
            if (antennas[i].uan_path == null)
            {
                path = GetDllPath() + "\\" + antennas[i].type + ".uan";
            }
            else
            {
                path = ".\\project\\" + setup.name +"\\"+ antennas[i].uan_path;
            }
            return path;
        }

        //获取UAN文件DLL的路径

        static string GetDllPath()
        {
            string dllpath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            dllpath = dllpath.Substring(8, dllpath.Length - 8);    // 8是 file:// 的长度
            return System.IO.Path.GetDirectoryName(dllpath);
        }

        //读取文件中的一行内容
        /// <summary>
        /// 计算一行的长度（王楠）
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        static int readline(string s, int start)
        {
            int length = 0;
            while (s[start + length] != '\n')
                length++;
            return length;
        }
        public static void Inputs(double[] directions, double[,] gaindata)
        {
            //第一部分：输入旋转信息处理
            double sums = 0;
            //double[] directions = new double[4];//存储输入数据
            double[] normalizations = new double[4];//存储归一化或弧度化后的数据
            double[] rotateparameters = new double[9];
            for (int i = 0; i < (directions.Length - 1); i++)//旋转轴方向向量归一化处理
            {
                sums += directions[i] * directions[i];
                sums = Math.Sqrt(sums);
            }
            for (int j = 0; j < (directions.Length - 1); j++)
            {
                normalizations[j] = directions[j] / sums;
            }
            normalizations[3] = directions[3] / 180 * Math.PI;//绕旋转轴右旋角度值弧度化处理
            Console.WriteLine("旋转轴向量归一化后x，y，z值分别为：");
            for (int j = 0; j < (directions.Length - 1); j++)
            {
                Console.WriteLine(normalizations[j]);
            }
            Console.WriteLine("绕旋转轴右旋角度值弧度化后值为：");
            Console.WriteLine(normalizations[3]);
            //天线旋转转移矩阵处理
            rotateparameters[0] = Math.Cos(normalizations[3]) + normalizations[0] * normalizations[0] * (1 - Math.Cos(normalizations[3]));
            rotateparameters[1] = normalizations[0] * normalizations[1] * (1 - Math.Cos(normalizations[3])) + normalizations[2] * Math.Sin(normalizations[3]);
            rotateparameters[2] = normalizations[0] * normalizations[2] * (1 - Math.Cos(normalizations[3])) - normalizations[1] * Math.Sin(normalizations[3]);
            rotateparameters[3] = normalizations[0] * normalizations[1] * (1 - Math.Cos(normalizations[3])) - normalizations[2] * Math.Sin(normalizations[3]);
            rotateparameters[4] = Math.Cos(normalizations[3]) + normalizations[1] * normalizations[1] * (1 - Math.Cos(normalizations[3]));
            rotateparameters[5] = normalizations[1] * normalizations[2] * (1 - Math.Cos(normalizations[3])) + normalizations[0] * Math.Sin(normalizations[3]);
            rotateparameters[6] = normalizations[0] * normalizations[2] * (1 - Math.Cos(normalizations[3])) + normalizations[1] * Math.Sin(normalizations[3]);
            rotateparameters[7] = normalizations[1] * normalizations[2] * (1 - Math.Cos(normalizations[3])) - normalizations[0] * Math.Sin(normalizations[3]);
            rotateparameters[8] = Math.Cos(normalizations[3]) + normalizations[2] * normalizations[2] * (1 - Math.Cos(normalizations[3]));
            //第三部分：
            double[,] gaintheta = new double[65341, 3];//定义存储theta分量的数据的数组
            double[,] gainthetaRec = new double[65341, 3];//定义存储theta分量转换为直角坐标系后的数据的数组
            double[,] gainthetarot = new double[65341, 3];//定义存储theta分量转换为直角坐标系后的绕旋转轴旋转后的数据的数组
            //double[,] gainDataRot = new double[65341, 6];//定义存储转换后的球坐标系下数据的数组
            for (int i = 0; i < 65341; i++)//求theta分量的球坐标系下模值表示
            {
                gaintheta[i, 0] = gaindata[i, 0] * Math.PI / 180;//theta
                gaintheta[i, 1] = gaindata[i, 1] * Math.PI / 180;//phi
                gaintheta[i, 2] = Math.Abs(gaindata[i, 2]);// * Math.Cos(gaindata[i, 4]*Math.PI/180)  gaintheta值（dB）
            }
            for (int j = 0; j < 65341; j++)//求theta分量的直角坐标系下模值表示
            {
                gainthetaRec[j, 0] = gaintheta[j, 2] * Math.Sin(gaintheta[j, 0]) * Math.Cos(gaintheta[j, 1]);//x
                gainthetaRec[j, 1] = gaintheta[j, 2] * Math.Sin(gaintheta[j, 0]) * Math.Sin(gaintheta[j, 1]);//y
                gainthetaRec[j, 2] = gaintheta[j, 2] * Math.Cos(gaintheta[j, 0]);//z

            }
            for (int k = 0; k < 65341; k++)//theta分量转换为直角坐标系下绕旋转轴旋转后的表示
            {
                gainthetarot[k, 0] = gainthetaRec[k, 0] * rotateparameters[0] + gainthetaRec[k, 1] * rotateparameters[3] + gainthetaRec[k, 2] * rotateparameters[6];//x'
                gainthetarot[k, 1] = gainthetaRec[k, 0] * rotateparameters[1] + gainthetaRec[k, 1] * rotateparameters[4] + gainthetaRec[k, 2] * rotateparameters[7];//y'
                gainthetarot[k, 2] = gainthetaRec[k, 0] * rotateparameters[2] + gainthetaRec[k, 1] * rotateparameters[5] + gainthetaRec[k, 2] * rotateparameters[8];//z'
            }
            for (int m = 0; m < 65341; m++)
            {
                //求旋转后对应的theta值

                if (gainthetarot[m, 2] < 0)
                {
                    gaindata[m, 0] = Math.Atan(Math.Sqrt(gainthetarot[m, 0] * gainthetarot[m, 0] + gainthetarot[m, 1] * gainthetarot[m, 1]) / gainthetarot[m, 2]) / Math.PI * 180 + 180;
                }
                else
                {
                    gaindata[m, 0] = Math.Atan(Math.Sqrt(gainthetarot[m, 0] * gainthetarot[m, 0] + gainthetarot[m, 1] * gainthetarot[m, 1]) / gainthetarot[m, 2]) / Math.PI * 180;
                }

                //求旋转后对应的phi值
                if ((gainthetarot[m, 0] >= 0) && (gainthetarot[m, 1] >= 0))
                {
                    gaindata[m, 1] = Math.Atan(gainthetarot[m, 1] / gainthetarot[m, 0]) / Math.PI * 180;
                }
                else if ((gainthetarot[m, 0] < 0) && (gainthetarot[m, 1] >= 0))
                {
                    gaindata[m, 1] = Math.Atan(gainthetarot[m, 1] / gainthetarot[m, 0]) / Math.PI * 180 + 180;
                }
                else if ((gainthetarot[m, 0] < 0) && (gainthetarot[m, 1] < 0))
                {
                    gaindata[m, 1] = Math.Atan(gainthetarot[m, 1] / gainthetarot[m, 0]) / Math.PI * 180 + 180;
                }
                else
                {
                    gaindata[m, 1] = Math.Atan(gainthetarot[m, 1] / gainthetarot[m, 0]) / Math.PI * 180 + 360;
                }
            }

        }
        public static double[,] interpolation(double[,] gainDataRot, double[,] gaindata)//插值
        {
            double[,] gainDatastep = new double[65341, 6];//插值预处理：将theta，phi相位分量分别统一为0度和180度
            for (int istep = 0; istep < 65341; istep++)
            {
                gainDatastep[istep, 0] = gainDataRot[istep, 0];
                gainDatastep[istep, 1] = gainDataRot[istep, 1];
                gainDatastep[istep, 2] = gainDataRot[istep, 2] * Math.Cos(gainDataRot[istep, 4]);
                gainDatastep[istep, 3] = -gainDataRot[istep, 3] * Math.Cos(gainDatastep[istep, 5]);
                gainDatastep[istep, 4] = 0;
                gainDatastep[istep, 5] = 180;
            }
            //插值
            double[,] gaininter = new double[65341, 6];
            for (int i = 0; i < 65341; i++)
            {
                gaininter[i, 0] = gaindata[i, 0];
                gaininter[i, 1] = gaindata[i, 1];
                gaininter[i, 4] = 0;
                gaininter[i, 5] = 180;
            }
            for (int r = 0; r < 361; r++)
            {
                for (int k = 0; k < 181; k++)
                {

                    ArrayList steplistA = new ArrayList();
                    ArrayList steplistB = new ArrayList();
                    for (int n = 0; n < 65341; n++)
                    {
                        if ((Math.Abs(gainDatastep[n, 0] - k) < 10) && (Math.Abs(gainDatastep[n, 1] - r) < 10))//缩小检索范围
                        {
                            steplistA.Add(n);
                            double distance = Math.Sqrt((gainDatastep[n, 0] - k) * (gainDatastep[n, 0] - k) + (gainDatastep[n, 1] - r) * (gainDatastep[n, 1] - r));
                            steplistB.Add(distance);
                        }
                    }
                    double[] length = new double[4];
                    int[] number = new int[4];
                    for (int ilen = 0; ilen < 4; ilen++)
                    {
                        int Nnear = (int)steplistA[0];
                        double Dnear = (double)steplistB[0];
                        int num = 0;
                        for (int inear = 0; inear < steplistA.Count; inear++)
                        {
                            if ((double)steplistB[inear] < Dnear)
                            {
                                num = inear;
                                Nnear = (int)steplistA[inear];
                                Dnear = (double)steplistB[inear];
                            }
                        }
                        number[ilen] = Nnear;
                        length[ilen] = Dnear;
                        steplistA.RemoveAt(num);
                        steplistB.RemoveAt(num);
                    }

                    if (length[0] == 0)
                    {
                        gaininter[181 * r + k, 2] = gainDatastep[(number[0]), 2];
                        gaininter[181 * r + k, 3] = gainDatastep[(number[0]), 3];
                    }
                    else if (length[1] == 0)
                    {
                        gaininter[181 * r + k, 2] = gainDatastep[(number[1]), 2];
                        gaininter[181 * r + k, 3] = gainDatastep[(number[1]), 3];
                    }
                    else if (length[2] == 0)
                    {
                        gaininter[181 * r + k, 2] = gainDatastep[(number[2]), 2];
                        gaininter[181 * r + k, 3] = gainDatastep[(number[2]), 3];
                    }
                    else if (length[3] == 0)
                    {
                        gaininter[181 * r + k, 2] = gainDatastep[(number[3]), 2];
                        gaininter[181 * r + k, 3] = gainDatastep[(number[3]), 3];
                    }
                    else
                    {
                        double weighting = 1 / length[0] + 1 / length[1] + 1 / length[2] + 1 / length[3];
                        for (int ith = 0; ith < 4; ith++)
                        {
                            gaininter[181 * r + k, 2] += 1 / (length[ith] * weighting) * gainDatastep[(number[ith]), 2];
                            gaininter[181 * r + k, 3] += 1 / (length[ith] * weighting) * gainDatastep[(number[ith]), 3];
                        }
                    }

                }

            }
            return gaininter;
        }
    }
}
