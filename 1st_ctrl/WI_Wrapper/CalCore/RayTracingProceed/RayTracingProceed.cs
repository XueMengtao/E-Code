using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CalculateModelClasses;
using TxRxFileProceed;
using RayCalInfo;
using FileOutput;
using System.Reflection;
using System.Configuration;
using SetupFileObject;
using log4net;
using UanFileProceed;
using CityAndFloor;




namespace RayTracingProceed
{
    public class RayTracingProceed
    {
        private static ILog myLog = LogManager.GetLogger(typeof(RayTracingProceed));
        public static void Calculate(string setuppath, string terpath, string txpath, string rxpath)
        {
            //此判断用来将ter文件中的三角面读入Intersection.Tris中
            Terrain newTer = new Terrain(terpath);
            string DirectoryPath = GetDirectory(setuppath) + "result\\";
            SetupObject setupOne = SetupFileProceed.GetSetupFile.GetSetup(setuppath);
            int txTotal = setupOne.tr.FirstAvailableTxNumber;
            string ProName = GetProName(setuppath);
            List<Node> txs = TxFileProceed.GetTx(txpath, setuppath, terpath);
            myLog.Info(txs.Count);
            List<List<ReceiveBall>> rxs = RxFileProceed.GetRx(rxpath, setuppath, terpath);
            myLog.Info(rxs.Count);
            //rxs.AddRange(TxFileProceed.GetTxAsRx(txpath, setuppath, terpath));
            City buildings = new City(GetDirectory(setuppath)+ProName+".city");
            buildings.RestructTerrainByBuildings(newTer.TerRect);
            if (buildings.Build !=null&& buildings.Build.Count != 0)
            { newTer.OutPutNewTerrain(GetDirectory(setuppath)); }//输出新地形
            int?[] index={null,null};
            int txindex = 0;
            for (int i = 0; i < txs.Count; i++)
            {
                List<FrequencyBand> txFrequencyBand = TxFileProceed.GetTxFrequenceBand(txpath, setuppath, i);//获取频段信息
                myLog.Debug("这是第" + (i + 1) + "个发射机的射线追踪==========================================================================");
                for (int j = 0; j < rxs.Count; j++)
                {
                    //该接收区只有一个接收机
                        if (rxs[j].Count == 1)
                        {
                            if (rxs[j][0].GetType().ToString().Equals("CalculateModelClasses.ReceiveArea"))
                            {
                                //电磁态势的追踪模块
                            ReceiveArea reArea = (ReceiveArea)rxs[j][0];
                            //第一步：构建态势区域
                            myLog.Debug("构建态势区域");
                            reArea.CreateAreaSituation(newTer.TerRect);
                            //第二步：正向追踪，获取路径
                            myLog.Debug("正向追踪，获取粗略路径");
                            RayTubeMethod areaTracing = new RayTubeMethod(txs[i], reArea, newTer, buildings, 64);
                            //读取发射天线的极化信息
                            string txPol = GetRxTxPol("tx", txs[i].TxNum);
                            //读取接收天线的极化信息（删除？？）
                            string rxPol = GetRxTxPol("areaSituation", rxs[j][0].RxNum);
                            //第三步：反向追踪，获取准确路径
                            //for (int m = 0; m < reArea.areaSituationNodes.Count; m++)
                            //{
                            //    areaTracing.ReverseAreaTracingPathsAndDeleteRepeatedPaths(reArea.areaSituationNodes[m].paths);
                            //}
                            //获取发射天线的4个参数
                            ReadUan.GetGainPara(txs[i].UAN);
                            for (int m = 0; m < reArea.areaSituationNodes.Count; m++)
                            {
                                reArea.areaSituationNodes[m].classifiedFrequencyPaths = new List<List<CalculateModelClasses.Path>>();
                                if (reArea.areaSituationNodes[m].paths.Count != 0)
                                {
                                    PathsafterPolization(reArea.areaSituationNodes[m].paths, txPol, rxPol);//极化代码
                                    //第四步：分频段求每条路径上的场强
                                    reArea.areaSituationNodes[m].classifiedFrequencyPaths = new List<List<CalculateModelClasses.Path>>();
                                    //areaTracing.ScreenAreaSituationPathsByFrequencyAndCalculateEField(txFrequencyBand, reArea.areaSituationNodes[m].paths, reArea.areaSituationNodes[m].classifiedFrequencyPaths);
                                    //计算每个频段上的各个路径叠加的总场强
                                    for (int n = 0; n < txFrequencyBand.Count; n++)
                                    {
                                        EField tempEfield = new EField();
                                        tempEfield.X = tempEfield.GetTolEx(reArea.areaSituationNodes[m].classifiedFrequencyPaths[n]);
                                        tempEfield.Y = tempEfield.GetTolEy(reArea.areaSituationNodes[m].classifiedFrequencyPaths[n]);
                                        tempEfield.Z = tempEfield.GetTolEz(reArea.areaSituationNodes[m].classifiedFrequencyPaths[n]);
                                        //得到每个频段上各个路径叠加的场强 
                                        reArea.areaSituationNodes[m].totleEfields.Add(tempEfield);
                                        //得到各个频段上各个路径后得到的总场强
                                        reArea.areaSituationNodes[m].totleEfield.X += tempEfield.X;
                                        reArea.areaSituationNodes[m].totleEfield.Y += tempEfield.Y;
                                        reArea.areaSituationNodes[m].totleEfield.Z += tempEfield.Z;
                                    }
                                }
                            }
                            //文件输出
                            string path = ".\\.\\project\\station\\areaSituationResult.txt";
                            if(!System.IO.File.Exists(path))
                            {
                                System.IO.File.Create(path).Close();
                            }
                            StringBuilder sb = new StringBuilder();
                            for (int m = 0; m < reArea.areaSituationNodes.Count; m++)
                            {
                                if (reArea.areaSituationNodes[m].paths.Count != 0)
                                {
                                    //string appendText1 =  m+" "  + reArea.areaSituationNodes[m].paths.Count+" " ;
                                    //sb.Append(appendText1);
                                    //string appendText2 = "Ex " + reArea.areaSituationNodes[m].totleEfield.X.Re + "+j" + reArea.areaSituationNodes[m].totleEfield.X.Im + ",Ey " + reArea.areaSituationNodes[m].totleEfield.Y.Re + "+j" + reArea.areaSituationNodes[m].totleEfield.Y.Im + ",Ez " + reArea.areaSituationNodes[m].totleEfield.Z.Re + "+j" + reArea.areaSituationNodes[m].totleEfield.Z.Im + "\r\n";
                                    //sb.Append(appendText2);
                                    string appendText1 = "第" + m + "个态势点上有" + reArea.areaSituationNodes[m].paths.Count + "条路径，";
                                    sb.Append(appendText1);
                                    string appendText2 = "总场强为:Ex " + reArea.areaSituationNodes[m].totleEfield.X.Re + "+j" + reArea.areaSituationNodes[m].totleEfield.X.Im + ",Ey " + reArea.areaSituationNodes[m].totleEfield.Y.Re + "+j" + reArea.areaSituationNodes[m].totleEfield.Y.Im + ",Ez " + reArea.areaSituationNodes[m].totleEfield.Z.Re + "+j" + reArea.areaSituationNodes[m].totleEfield.Z.Im + "\r\n";
                                    sb.Append(appendText2);
                                }
                                //else
                                //{
                                //    string appendText1 = "没有到达第" + m + "个态势点的路径\r\n";
                                //    sb.Append(appendText1);
                                //}
                            }
                            StreamWriter stw = new StreamWriter(path);
                            stw.Write(sb);
                            stw.Flush();//清空缓冲区
                            stw.Close();//关闭流
                            //---------------------------------
                            myLog.Debug("结果计算和文件输出过程结束，进入下一个接收机的追踪--------------------------------------");
                        }
                        else
                            {
                                if (("tx_"+txs[i].NodeName).Equals(rxs[j][0].RxName))
                                {
                                    myLog.Debug("这是第" + (i + 1) + "个发射机第" + (j + 1) + "个接收机的射线追踪:是同一个发射机");
                                    continue;
                                }
                                myLog.Debug("这是第" + (i + 1) + "个发射机第" + (j + 1) + "个接收机的射线追踪*****************************************************");
                                index[0] = j;
                                RayTubeMethod rayTubeMethod = new RayTubeMethod(txs[i], rxs[j][0], newTer, buildings, 32);
                                rayTubeMethod.ReverseTracingPathsAndDeleteRepeatedPaths();
                                rayTubeMethod.UpdateRayInForNodes();
                         //       PunctiformLaunchMethod punctiformMethod = new PunctiformLaunchMethod(txs[i], rxs[j][0], newTer, buildings, 64, txFrequencyBand);
                        //        List<CalculateModelClasses.Path> temp = punctiformMethod.GetPunctiformRxPath(tx, rxs[j][0], newTer,cityBuilding, 32, 128,TxFrequencyBand);

                        //        if (rayTubeMethod.ReceivedPaths.Count != 0)//若存在直射射线
                       //         {
                       //             string txPol = GetRxTxPol("tx", txs[i].TxNum);//极化代码
                       //             string rxPol = null;
                      //              if (rxs[j][0].isTx == true)
                      //              {
                      //                  rxPol = GetRxTxPol("tx", rxs[j][0].RxNum);
                      //              }
                      //              else
                      //              {
                      //                  rxPol = GetRxTxPol("rx", rxs[j][0].RxNum);//
                      //              }
                     //               PathsafterPolization(rayTubeMethod.ReceivedPaths, txPol, rxPol);//
                      //          }
                                myLog.Debug("射线追踪过程结束，进入射线筛选和计算过程-----------------------------------------");
                            //输出结果
                            ReadUan.GetGainPara(rxs[j][0].UAN);
                            List<List<CalculateModelClasses.Path>> ClassifiedPaths = rayTubeMethod.ScreenPunctiformPathsByFrequencyAndCalculateEField(txFrequencyBand);
                            for (int m = 0; m < txFrequencyBand.Count; m++)
                            {
                                P2mFileOutput.p2mfileoutput(ClassifiedPaths[m], DirectoryPath, ProName, txs[i].TxNum, txTotal, txindex, rxs[j][0], txs[i].Position, txFrequencyBand[m].MidPointFrequence, txFrequencyBand[m].FrequenceWidthMin, txFrequencyBand[m].FrequenceWidthMax);
                            }
                            List<CalculateModelClasses.Path> totalPowerPath = GetTotalPowerPath(ClassifiedPaths);
                            PowerOutput.GetRxTotalPower(totalPowerPath, DirectoryPath, ProName, txs[i].TxNum, txTotal, rxs[j][0], txs[i].Position);
                            myLog.Debug("结果计算和文件输出过程结束，进入下一个接收机的追踪---------------------------------------");
                            }
                        }
                        else
                        {
                            throw new Exception("没有接收机");
                        }        
                }
            }
            GC.Collect();
            
        }
        //参数是接收机和发射机之间的路径
        private static string GetRxTxPol(string rxOrTx, int rxTxNum)
        {
            string pathSet = ConfigurationManager.AppSettings["setupath"];
            SetupObject so = SetupFileProceed.GetSetupFile.GetSetup(pathSet);
            SetupFileObject.TotalAntenna ta = so.antenna;
            List<SetupFileObject.AntennaStp> asAll = ta.allAntennas;
            //从tx，rx,setup文件中得到信息
            string pathTx = ConfigurationManager.AppSettings["txpath"];
            //存放rxtx的文件内容
            string s;
            int tantNum = -1;
            int rantNum = -1;
            try
            {
                if (rxOrTx == "tx")
                {
                    StreamReader st = new StreamReader(pathTx);
                    s = st.ReadToEnd();
                    int tstart = s.IndexOf("TxSet " + Convert.ToString(rxTxNum));
                    int tbstart = s.IndexOf("<antenna>", tstart);
                    int tastart = s.IndexOf("antenna", tbstart + 7);
                    string str = s.Substring(tastart + 8);
                    tantNum = Convert.ToInt32(s.Substring(tastart + 8, 1));
                    st.Close();
                    s = "";
                    if (tantNum == -1)
                    {
                        throw new Exception("读取天线的编号出现问题");
                    }

                }
                else if(rxOrTx=="rx")
                {
                    //读取文件rxset
                    string pathRx = ConfigurationManager.AppSettings["rxpath"];
                    StreamReader sr = new StreamReader(pathRx);
                    s = sr.ReadToEnd();
                    int tstart = s.IndexOf("RxSet " + Convert.ToString(rxTxNum));
                    int tbstart = s.IndexOf("<antenna>", tstart);
                    int tastart = s.IndexOf("antenna", tbstart + 7);
                    string str = s.Substring(tastart + 8);
                    rantNum = Convert.ToInt32(s.Substring(tastart + 8, 1));
                    s = "";
                    if (rantNum == -1)
                    {
                        throw new Exception("读取天线的编号出现问题");
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            for (int an = 0; an < asAll.Count; an++)
            {
                if (tantNum == asAll[an].antenna)
                {
                    //返回天线极化类型
                    return asAll[an].polarization;
                }
                if (rantNum == asAll[an].antenna)
                {
                    //返回天线的极化类型
                    return asAll[an].polarization;
                }
            }
            return "";
        }
        private static void PathsafterPolization(List<CalculateModelClasses.Path> paths, string txPol, string rxPol)
        {
            for (int m = paths.Count-1; m >=0; m--)
            {
                //如果发射是线极化的话不做处理
                if (txPol == "vertical" || txPol == "Horizontal" || txPol == "")
                    break;
                //如果发射是左旋圆极化
                else if (txPol == "Left-hand circular")
                {
                    //如果发射是左旋圆极化并且接收是线极化，则最后的能量只接收一半
                    if (rxPol == "vertical" || rxPol == "Horizontal")
                    {
                        int len = paths[m].node.Count;
                        paths[m].node[len - 1].Power/= 2;
                        //paths[m].node[len - 1].Power.Im /= 2;
                    }
                    //如果发射是左旋圆极化并且接收是右旋极化，则把此路径删除
                    else if (rxPol == "Right-hand circular")
                    {
                        paths.Remove(paths[m]);
                    }
                    //如果发射是左旋圆极化并且接收是左旋极化，则判断反射次数是偶数次还是基数次，偶数次则接收，基数次舍弃
                    else
                    {
                        int tempNodeNum = 0;
                        for (int temNodeNum = 0; temNodeNum < paths[m].node.Count; temNodeNum++)
                        {
                            if (paths[m].node[temNodeNum].NodeStyle==NodeStyle.ReflectionNode || paths[m].node[temNodeNum].NodeStyle==NodeStyle.DiffractionNode)
                                tempNodeNum++;
                        }
                        if (tempNodeNum % 2 != 0)
                        {
                            paths.Remove(paths[m]);
                        }
                    }
                }
                //如果发射是左旋圆极化 情况同上
                else if (txPol == "Right-hand circular")
                {
                    if (rxPol == "vertical" || rxPol == "Horizontal")
                    {
                        int len = paths[m].node.Count;
                        paths[m].node[len - 1].Power /= 2;
                       
                    }
                    else if (rxPol == "Light-hand circular")
                    {
                        paths.Remove(paths[m]);
                    }
                    else
                    {
                        int tempNodeNum = 0;
                        for (int temNodeNum = 0; temNodeNum < paths[m].node.Count; temNodeNum++)
                        {
                            if (paths[m].node[temNodeNum].NodeStyle != null)
                                tempNodeNum++;
                        }
                        if (tempNodeNum % 2 != 0)
                        {
                            paths.Remove(paths[m]);
                        }
                    }

                }
            }
        }

        private static string GetDirectory(string path)
        {
            FileInfo file = new FileInfo(path);
            DirectoryInfo dir = file.Directory;
            if (dir.FullName[dir.FullName.Length - 1] != '\\')
                return dir.FullName + "\\";
            else return dir.FullName;
        }

        private static string GetProName(string path)
        {
            FileInfo file = new FileInfo(path);
            string name = file.Name;
            int pos = name.LastIndexOf('.');
            return name.Substring(0, pos);
        }

        /// <summary>
        /// 将不同频段的路径放到一起来计算总功率
        /// </summary>
        private static List<CalculateModelClasses.Path> GetTotalPowerPath(List<List<CalculateModelClasses.Path>> ClassifiedPaths)
        {
            List<CalculateModelClasses.Path> totalPowerPath = new List<CalculateModelClasses.Path>();
            for (int x = 0; x < ClassifiedPaths.Count; x++)
            {
                for (int y = 0; y < ClassifiedPaths[x].Count; y++)
                {
                    totalPowerPath.Add(ClassifiedPaths[x][y]);
                }
            }
            return totalPowerPath;
        }
             

     
        /// <summary>
        /// 整个计算模块
        /// </summary>
        /// <param name="args">文件的路劲</param>
        public static void test(String[] args)
        {
            //String[] args = (String[])para;
            Console.WriteLine(args[0] + "计算开始");
            string path1 = @".\" + args[0] + args[2];
            string path3 = @".\" + args[0] + args[1];
            string path4 = @".\" + args[0] + args[3];
            string path5 = @".\" + args[0] + args[4];
            LogFileManager.ObjLog.info(path3 + "|" +""+ path1);
            ConfigurationManager.AppSettings.Set("terpath", path1);
            ConfigurationManager.AppSettings.Set("setupath", path3);
            ConfigurationManager.AppSettings.Set("txpath", path4);
            ConfigurationManager.AppSettings.Set("rxpath", path5);
            RayTracingProceed.Calculate(path3, path1, path4, path5);
            Console.WriteLine(args[0]+"计算节点跑完了");
        }
    
    }
}
