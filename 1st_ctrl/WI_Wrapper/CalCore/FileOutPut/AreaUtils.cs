using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using TxRxFileProceed;
using RayCalInfo;

namespace FileOutPut
{
    class AreaUtils
    {
        public static List<int> getCount4Rx(ReceiveArea rArea)
        {
            List<int> count = new List<int>();
            int countX = rArea.rxLength % rArea.spacing == 0 ? (int)(rArea.rxLength / rArea.spacing) : (int)(rArea.rxLength / rArea.spacing) + 1;
            int countY = rArea.rxWidth % rArea.spacing == 0 ? (int)(rArea.rxWidth / rArea.spacing) : (int)(rArea.rxWidth / rArea.spacing) + 1;
            count.Add(countX);
            count.Add(countY);
            return count;
        }


        /// <summary>
        ///把态势中的路径分成每条路径只有一个Rx点的路径list
        /// </summary>
        public static List<CalculateModelClasses.Path> getPaths(List<CalculateModelClasses.Path> Paths)
        {
            List<CalculateModelClasses.Path> temp = new List<CalculateModelClasses.Path>();
            for (int i = 0; i < Paths.Count; i++)
            {
                List<CalculateModelClasses.Path> temp1 = makeNewPath(Paths[i]);
                temp.AddRange(temp1);
            }
            return temp;
        }

        /// <summary>
        ///把态势中的一条路径分成每条路径只有一个Rx点的路径list
        /// </summary>
        public static List<CalculateModelClasses.Path> makeNewPath(CalculateModelClasses.Path path)
        {
            List<CalculateModelClasses.Path> test = new List<CalculateModelClasses.Path>();
            List<Node> temp = new List<Node>();
            foreach (Node item in path.node)   //对路径的每个节点进行遍历
            {
                List<Node> nodes = new List<Node>();
                if (item.NodeStyle==NodeStyle.Tx)
                {
                    temp.Add(item);
                }
                else if (item.NodeStyle == NodeStyle.ReflectionNode)
                {
                    temp.Add(item);
                }
                else if (item.NodeStyle == NodeStyle.DiffractionNode)
                {
                    temp.Add(item);
                }
                else
                {
                    Node pathnode = (Node)item.Clone();
                    pathnode.NodeStyle = NodeStyle.Rx;
                    for (int i = 0; i < temp.Count; i++)
                    {
                        nodes.Add(temp[i]);
                    }
                    nodes.Add(pathnode);
                    CalculateModelClasses.Path path1 = new CalculateModelClasses.Path(nodes);
                    test.Add(path1);
                
                }
            }
            return test;
        }

        /// <summary>
        ///将处理后的路径投影到划分的小区域上
        /// </summary>
        public static Dictionary<int, List<Plural>> yingshe(List<int> rxCounter, List<CalculateModelClasses.Path> paths, ReceiveArea rArea)
        {
            
            int rxCount = rxCounter[0] * rxCounter[1];
            Dictionary<int, List<Path>> tempPaths = new Dictionary<int, List<Path>>();
            Dictionary<int, List<Plural>> map = new Dictionary<int, List<Plural>>();
            for (int i = 0; i < rxCount; i++)
            {
                List<Path> path = new List<Path>();
                tempPaths.Add(i + 1, path);
            }
            for (int j = 0; j < paths.Count; j++)
            {
                DistributePathToUnits(paths[j], rxCounter, tempPaths,rArea);
            }
//         for (int i = 0; i < rxCount; i++)
//        {
//             List<Path> path = new List<Path>();
//              for (int j = 0; j < paths.Count; j++)//若该条路径的Rx点在该区域内，则将这条路径放入这块区域中
//             {
//                 if (util(paths[j].node[paths[j].node.Count - 1], rxCounter, i, rArea))  //如果这个点在区域i内则将这条路径放到区域中
//                 {
//                     path.Add(paths[j]);
//                  }
//             }
//              tempPaths.Add(i + 1, path);
//        }
            foreach (var item in tempPaths.Keys)//对每块小区域进行遍历
            {
                if (tempPaths[item].Count != 0)
                {

                    List<Path> areaPaths = tempPaths[item];
                    //如果节点数量为2的话就是直射，可以直接处理
                    int path2Count = 0;
                    
                    Plural totalEX = new Plural(0, 0);
                    Plural totalEY = new Plural(0, 0);
                    Plural totalEZ = new Plural(0, 0);
                    
                    Plural tempEX2 = new Plural(0, 0);
                    Plural tempEY2 = new Plural(0, 0);
                    Plural tempEZ2 = new Plural(0, 0);
                    List<Path> paths3 = new List<Path>();//声明用以存储路径node数目为3的路径集合
                    List<Path> paths4 = new List<Path>();//声明用以存储路径node数目为4的路径集合
                    List<Path> paths5 = new List<Path>();//声明用以存储路径node数目为5的路径集合
                    for (int i = 0; i < areaPaths.Count; i++)
                    {
                        if (areaPaths[i].node.Count == 2)//对直射进行处理，直接将最后的结点的值处理
                        {
                            //tempPower2 += areaPaths[i].node[areaPaths[i].node.Count - 1].Power;
                            tempEX2 += areaPaths[i].node[areaPaths[i].node.Count - 1].TotalE.X;
                            tempEY2 += areaPaths[i].node[areaPaths[i].node.Count - 1].TotalE.Y;
                            tempEZ2 += areaPaths[i].node[areaPaths[i].node.Count - 1].TotalE.Z;
                            path2Count++;
                            if (path2Count>=2)
                            {
                                areaPaths.RemoveAt(i);
                                i--;
                            }
                           
                        }
                        else if (areaPaths[i].node.Count == 3)
                        { paths3.Add(areaPaths[i]); }
                        else if (areaPaths[i].node.Count == 4)
                        { paths4.Add(areaPaths[i]); }
                        else if (areaPaths[i].node.Count == 5)
                        { paths5.Add(areaPaths[i]); }
                    }
                    List<Path> newPaths3 = new List<Path>(paths3);
                    List<Path> newPaths4 = new List<Path>(paths4);
                    List<Path> newPaths5 = new List<Path>(paths5);
                    if (path2Count != 0)
                    {
                        //totalPower += tempPower2 / path2Count;
                        totalEX += tempEX2 / path2Count;
                        totalEY += tempEY2 / path2Count;
                        totalEZ += tempEZ2 / path2Count;
                    }
                    if (newPaths3.Count != 0)
                    {
                        for (int i = 0; i < newPaths3.Count; i++)
                        {
                            //totalPower += newPaths3[i].node[newPaths3[i].node.Count - 1].Power;
                            totalEX += newPaths3[i].node[newPaths3[i].node.Count - 1].TotalE.X;
                            totalEY += newPaths3[i].node[newPaths3[i].node.Count - 1].TotalE.Y;
                            totalEZ += newPaths3[i].node[newPaths3[i].node.Count - 1].TotalE.Z;
                        }
                    }
                    if (newPaths4.Count != 0)
                    {
                        for (int i = 0; i < newPaths4.Count; i++)
                        {
                            //totalPower += newPaths4[i].node[newPaths4[i].node.Count - 1].Power;
                            totalEX += newPaths4[i].node[newPaths4[i].node.Count - 1].TotalE.X;
                            totalEY += newPaths4[i].node[newPaths4[i].node.Count - 1].TotalE.Y;
                            totalEZ += newPaths4[i].node[newPaths4[i].node.Count - 1].TotalE.Z;
                        }
                    }
                    if (newPaths5.Count != 0)
                    {
                        for (int i = 0; i < newPaths5.Count; i++)
                        {
                            //totalPower += newPaths5[i].node[newPaths5[i].node.Count - 1].Power;
                            totalEX += newPaths5[i].node[newPaths5[i].node.Count - 1].TotalE.X;
                            totalEY += newPaths5[i].node[newPaths5[i].node.Count - 1].TotalE.Y;
                            totalEZ += newPaths5[i].node[newPaths5[i].node.Count - 1].TotalE.Z;
                        }
                    }

                    List<Plural> ans = new List<Plural>();
                    double [] totalPowerAndPhase = Power.GetTotalPowerInDifferentPhase(areaPaths);
                    Plural totalPowerOfXushu = new Plural(totalPowerAndPhase[0]);
                    Plural phase = new Plural(totalPowerAndPhase[1]);
                    ans.Add(totalPowerOfXushu);
                    ans.Add(totalEX);
                    ans.Add(totalEY);
                    ans.Add(totalEZ);
                    ans.Add(phase);
                    map.Add(item, ans);
                    //test
                    //List<List<CalculateModelClasses.Path>> pathtemp = new List<List<CalculateModelClasses.Path>>();
                    //foreach (var item in map.Keys)
                    //{
                    //    if (map[item].Count != 0)
                    //    {
                    //        pathtemp.Add(map[item]);
                    //    }
                    //}



                }
            }
            return map;
        }


        /// <summary>
        ///获取态势区域分块中心点
        /// </summary>
        /// <param name="reArea">态势区域</param>
        /// <param name="rxCounter">态势区域X,Y上分段个数</param>
        /// <param name="terRect">地形矩形</param>
        /// <param name="index">当前分块的信息</param>
        /// <param temp="terRect">前台设置的高度</param>
        /// <returns></returns>
        public static Point GetCenterPointInAreaDivision(ReceiveArea reArea, List<int> rxCounter, Terrain ter, int index, double temp)
        { 
            Point centerTemp=new Point();
            int test1 = (index - 1) % rxCounter[0];
            int test2 = (index - 1 - test1) / rxCounter[0];
            //centerTemp.x=area.origen.x+area.spacing/2+test1*area.spacing;
            //centerTemp.y = area.origen.y + area.spacing / 2 + test2 * area.spacing;
            if (test1 != rxCounter[0] - 1)
            {
                centerTemp.X = reArea.OriginPoint.X + test1 * reArea.spacing;
            }
            else
            {
                centerTemp.X = reArea.OriginPoint.X+reArea.rxLength;
            }
            if (test2 != rxCounter[1] - 1)
            {
                centerTemp.Y = reArea.OriginPoint.Y + test2 * reArea.spacing;
            }
            else
            {
                centerTemp.Y = reArea.OriginPoint.Y + reArea.rxWidth;
            }
            SetCenterPointZValue(centerTemp, ter);
            centerTemp.Z += temp;
            return centerTemp;
        }

        /// <summary>
        ///获取态势区域分块中心的的Z坐标
        /// </summary>
        /// <param name="centerPoint">点</param>
        /// <param name="terRect">地形矩形</param>
        /// <param name="reArea">态势区域</param>
        /// <returns></returns>
        private static void SetCenterPointZValue(Point centerPoint,Terrain ter)
        {
            //先求出该中心点在地形矩形的行列序号
            int rowID = (int)((centerPoint.X - ter.TerRect[0, 0].TriangleOne.MinUnitX) / Terrain.UnitX);
            int lineID = (int)((centerPoint.Y - ter.TerRect[0, 0].TriangleOne.MinUnitY) / Terrain.UnitY);
            //对该矩形的三角面进行遍历
            for (int i = 0; i < ter.TerRect[lineID, rowID].RectTriangles.Count; i++)
            {
                if (ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle.Count == 0)//若该三角面没有被细分
                {
                    if (ter.TerRect[lineID, rowID].RectTriangles[i].JudgeIfPointInTriangleInXY(centerPoint))//判断中心点是否在该三角面的XY投影上
                    {
                        centerPoint.Z = centerPoint.GetZValueByFace(ter.TerRect[lineID, rowID].RectTriangles[i]);//求中心点的Z坐标
                        return;
                    }
                }
                else
                {
                    for (int j = 0; j < ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle.Count; j++)//对细分三角面进行遍历
                    {
                        if (ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle[j].JudgeIfPointInTriangleInXY(centerPoint))//判断中心点是否在该三角面的XY投影上
                        {
                            centerPoint.Z = centerPoint.GetZValueByFace(ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle[j]);//求中心点的Z坐标
                            return;
                        }
                    }
                }
            }
        }



        public static string getBlank(int m,int blankCount)
        {
            string temp = "";
            string mstr = m.ToString();
            int count = mstr.Length;
            int index = blankCount - count;
            for (int i = 0; i < index; i++)
            {
                //if (m < 0 && i == index - 1)
                //{
                //    continue;
                //}
                //else
                //{
                    temp += " ";
                //}
            }
            return temp;
        }


        /// <summary>
        /// 根据每条路径最后的rx的坐标分到相应的细分区域上
        /// </summary>
        /// <param name="paramPath">路径</param>
        ///  <param name="rxCounter">接收区域长和宽上细分区域的个数</param>
        /// <param name="tempPaths">每块小区域的路径</param>
        /// <param name="reArea">接收区域</param>
        /// <returns></returns>
        private static void DistributePathToUnits(Path paramPath,List<int> rxCounter, Dictionary<int, List<Path>> tempPaths, ReceiveArea reArea)
        {
            //int row = (int)((paramPath.node[paramPath.node.Count - 1].Position.X - reArea.BottomSideRect[0, 0].TriangleOne.MinUnitX) / reArea.spacing);
            //int line = (int)((paramPath.node[paramPath.node.Count - 1].Position.Y - reArea.BottomSideRect[0, 0].TriangleOne.MinUnitY) / reArea.spacing);
            //int key = line * rxCounter[0] + row + 1;
            //tempPaths[key].Add(paramPath);

        }
    }
}
