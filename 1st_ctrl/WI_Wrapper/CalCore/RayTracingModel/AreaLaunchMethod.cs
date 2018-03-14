using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using TxRxFileProceed;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using CityAndFloor;

namespace RayCalInfo
{
    public class AreaLaunchMethod:PunctiformLaunchMethod
    {

        public AreaLaunchMethod()
        {

        }
        public AreaLaunchMethod(Node tx, ReceiveArea reArea, Terrain ter, City cityBuilding, int TessellationFrequency, List<FrequencyBand> txFrequencyBand)
        {
            this.rayPaths = this.LuanchRaysOmnidirectionalInArea(tx, reArea, ter, cityBuilding, TessellationFrequency, txFrequencyBand);
            tx.JudgeIfNodeIsInArea(ter, reArea); //判断节点是否在态势区域内
            this.SetSpacingNodeInAreaPath(tx, reArea);
        }


        public AreaLaunchMethod(Node tx, ReceiveArea reArea, Terrain ter, City cityBuilding, int firstN, int finalN, List<FrequencyBand> txFrequencyBand)
        {
            this.rayPaths = this.GetAreaPath(tx, reArea, ter, cityBuilding, firstN, finalN, txFrequencyBand);
            tx.JudgeIfNodeIsInArea(ter, reArea); //判断节点是否在态势区域内
            this.SetSpacingNodeInAreaPath(tx, reArea);
        }

        /// <summary>
        ///获取态势区域所有射线的路径
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="firstN">初始细分点个数</param>
        ///  <param name="finalN">最终细分点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private List<Path> GetAreaPath(Node tx, ReceiveArea reArea, Terrain ter, City cityBuilding, int firstN, int finalN, List<FrequencyBand> txFrequencyBand)
        {
            List<Path> pathInfo = new List<Path>();
            List<RayTracingModel> originUnits = this.GetOriginUnitsOfIcosahedron(tx);//正二十面体的二十个三角面
            double finalDivideAngle = this.GetInitialAngleOfRayBeam(finalN);//最终细分角度，弧度制
            double firstDivideAngle = this.GetInitialAngleOfRayBeam(firstN);//最初细分角度，弧度制
            for (int i = 0; i < originUnits.Count; i++)
            {
                List<RayTracingModel> firstUnits = this.GetInitialTrianglePath(tx, ter, reArea, cityBuilding, originUnits[i], firstN, firstDivideAngle, txFrequencyBand);
                this.GetPathOfUnit(firstUnits, pathInfo, tx, ter, reArea, cityBuilding, finalDivideAngle, txFrequencyBand);
            }

            return pathInfo;
        }

     
        /// <summary>
        ///初始阶段在每个三角形上发出一定的射线
        /// </summary>
        /// <param name="tx">发射机</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="N">三角形处理单元每条边取点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="divideAngle">射线束夹角</param>
        /// <returns>返回对正二十面体每个三角面初始细分后得到的多个处理单元</returns>
        private List<RayTracingModel> GetInitialTrianglePath(Node tx, Terrain ter, ReceiveArea reArea, City cityBuilding, RayTracingModel unit, int N, double divideAngle, List<FrequencyBand> txFrequencyBand)
        {
            Point vertex1 = new Point(unit.FirstRay.SVector);//三角形的顶点
            Point vertex2 = new Point(unit.SecondRay.SVector);//三角形的顶点
            Point vertex3 = new Point(unit.ThirdRay.SVector);//三角形的顶点
            SpectVector vector12 = new SpectVector(vertex1, vertex2);
            SpectVector vector23 = new SpectVector(vertex2, vertex3);
            double length = vertex1.GetDistance(vertex2) / N;//每段的距离
            Point[] pointOfEdge12 = new Point[N]; //存放棱上的点,不包括顶点1
            //    p1[0] = vertex1;
            for (int i = 0; i < N; i++)
            {
                pointOfEdge12[i] = new Point(vertex1.X + (i + 1) * (vector12.a / Math.Sqrt(Math.Pow(vector12.a, 2) + Math.Pow(vector12.b, 2) + Math.Pow(vector12.c, 2)) * length), vertex1.Y + (i + 1) * (vector12.b / Math.Sqrt(Math.Pow(vector12.a, 2) + Math.Pow(vector12.b, 2) + Math.Pow(vector12.c, 2)) * length), vertex1.Z + (i + 1) * (vector12.c / Math.Sqrt(Math.Pow(vector12.a, 2) + Math.Pow(vector12.b, 2) + Math.Pow(vector12.c, 2)) * length));

            }
            List<Point>[] vertexPoints = new List<Point>[N + 1];//存放三角形内每条平行边上的点
            for (int k = 0; k <= N; k++)
            {
                vertexPoints[k] = new List<Point>();
            }
            vertexPoints[0].Add(vertex1);
            for (int j = 1; j <= N; j++)//得到三角形切分后每条边上的点
            {
                vertexPoints[j].Add(pointOfEdge12[j - 1]);
                this.GetTriangleInternalEdgePoint(vector23, pointOfEdge12[j - 1], length, j, vertexPoints[j]);
            }
            List<ClassNewRay>[] raysPath = this.GetEachTriangleRay(tx, ter, reArea, cityBuilding, N, txFrequencyBand, vertexPoints);//根据得到的点构造射线
            List<RayTracingModel> triangleUnits = this.HandleInitialRayToUnit(raysPath);//得到初始追踪完的三角形处理单元
            return triangleUnits;
        }



        /// <summary>
        ///获得处理单元及其细分单元中的射线
        /// </summary>
        /// <param name="units">处理单元</param>
        /// <param name="pathInfo">所有路径</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="divideAngle">射线束夹角</param>
        /// <returns></returns>
        private void GetPathOfUnit(List<RayTracingModel> units, List<Path> pathInfo, Node tx, Terrain ter, ReceiveArea reArea, City cityBuilding, double divideAngle, List<FrequencyBand> txFrequencyBand)
        {
            if (units == null || units.Count == 0)
            { throw new Exception("处理追踪单元时，输入出错"); }
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].HaveTraced)//该处理单元已完成射线追踪
                {
                    if (units[i].AngleOfRayBeam < divideAngle * 180 / Math.PI)//若该处理单元的精度已满足要求
                    {
                        this.ExtractPathOfUnit(units[i], pathInfo);//提取射线
                    }
                    else
                    {
                        List<RayTracingModel> divideUnits = this.DivideRayUnit(units[i]);//细分三角形
                        if (divideUnits.Count == 0)
                            continue;
                        else
                        {
                            this.GetPathOfUnit(divideUnits, pathInfo, tx, ter, reArea, cityBuilding, divideAngle, txFrequencyBand); //追踪细分三角形内的射线
                        }
                    }
                }
                else//该处理单元还未进行射线追踪
                {
                    this.TracingUnitRay(units[i], tx, ter, reArea, cityBuilding, txFrequencyBand);//射线追踪
                    if (units[i].AngleOfRayBeam < divideAngle * 180 / Math.PI)//若该处理单元的精度已满足要求
                    {
                        this.ExtractPathOfUnit(units[i], pathInfo);//提取射线
                    }
                    else
                    {
                        List<RayTracingModel> divideUnits = this.DivideRayUnit(units[i]);
                        if (divideUnits.Count == 0)
                            continue;
                        else
                        {
                            this.GetPathOfUnit(divideUnits, pathInfo, tx, ter, reArea, cityBuilding, divideAngle, txFrequencyBand); //追踪细分三角形内的射线
                        }
                    }
                }
            }
        }

        /// <summary>
        ///得到三角形切分后每条边上的点，但不包括初始端点
        /// </summary>
        /// <returns></returns>
        private void GetTriangleInternalEdgePoint(SpectVector vector, Point leftPoint, double length, int TessellationFrequency, List<Point> vertex)
        {
            for (int i = 1; i <= TessellationFrequency; i++)//根据端点和方向向量，得到平行边上每一个点
            {
                Point temp = new Point(leftPoint.X + (vector.a / Math.Sqrt(Math.Pow(vector.a, 2) + Math.Pow(vector.b, 2) + Math.Pow(vector.c, 2)) * length * i), leftPoint.Y + (vector.b / Math.Sqrt(Math.Pow(vector.a, 2) + Math.Pow(vector.b, 2) + Math.Pow(vector.c, 2)) * length * i),
                             leftPoint.Z + (vector.c / Math.Sqrt(Math.Pow(vector.a, 2) + Math.Pow(vector.b, 2) + Math.Pow(vector.c, 2)) * length * i));
                vertex.Add(temp);
            }
        }

        /// <summary>
        ///得到正二十面体每个三角形初始细分时的所有射线
        /// </summary>
        /// <param name="tx">发射机</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="N">三角形处理单元每条边取点个数</param>
        ///  <param name="divideAngle">射线束夹角</param>
        ///  <param vertexPoints="N">三角形上用作发射射线的点</param>
        /// <returns></returns>
        private List<ClassNewRay>[] GetEachTriangleRay(Node tx, Terrain ter, ReceiveArea reArea, City cityBuilding, int N, List<FrequencyBand> txFrequencyBand, List<Point>[] vertexPoints)
        {
            List<ClassNewRay>[] raysPath = new List<ClassNewRay>[N + 1];
            for (int m = 0; m < vertexPoints.Length; m++)
            {
                raysPath[m] = new List<ClassNewRay>();
                for (int n = 0; n < vertexPoints[m].Count; n++)
                {
                    ClassNewRay temp = new ClassNewRay();//每个点发射一条射线，并进行追踪
                    temp.Origin = tx.Position;
                    SpectVector paramVector = new SpectVector(vertexPoints[m][n].X, vertexPoints[m][n].Y, vertexPoints[m][n].Z);
                    RayInfo paramRay =  new RayInfo(tx.Position, paramVector);
                    List<Path> path = this.GetSingleRayPaths(tx, ter, reArea, cityBuilding, paramRay, txFrequencyBand);
                    temp.Flag = JudgeIfArriveRx(path);
                    temp.Path = path;
                    temp.SVector = paramVector;
                    temp.WhetherHaveTraced = true;
                    raysPath[m].Add(temp);
                }
            }
            return raysPath;
        }

        /// <summary>
        ///追踪处理单元中的射线
        /// </summary>
        /// <param name="tx">发射机</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="unit">射线处理单元</param>
        /// <returns></returns>
        private void TracingUnitRay(RayTracingModel unit, Node tx, Terrain ter, ReceiveArea reArea, City cityBuilding, List<FrequencyBand> txFrequencyBand)
        {
            //追踪射线1
            if (unit.FirstRay.WhetherHaveTraced == false)//若该射线之前还没有追踪
            {
                RayInfo newRay =  new RayInfo(unit.FirstRay.Origin, unit.FirstRay.SVector);
                List<Path> pathtemp = this.GetSingleRayPaths(tx, ter, reArea, cityBuilding, newRay, txFrequencyBand);
                unit.FirstRay.Flag = JudgeIfArriveRx(pathtemp);
                unit.FirstRay.Path = pathtemp;
                unit.FirstRay.WhetherHaveTraced = true;
            }
            //追踪射线2
            if (unit.SecondRay.WhetherHaveTraced == false)//若该射线之前还没有追踪
            {
                RayInfo newRay =new RayInfo(unit.SecondRay.Origin, unit.SecondRay.SVector) ;
                List<Path> pathtemp = this.GetSingleRayPaths(tx, ter, reArea, cityBuilding, newRay, txFrequencyBand);
                unit.SecondRay.Flag = JudgeIfArriveRx(pathtemp);
                unit.SecondRay.Path = pathtemp;
                unit.FirstRay.WhetherHaveTraced = true;
            }
            //追踪射线3
            if (unit.ThirdRay.WhetherHaveTraced == false)//若该射线之前还没有追踪
            {
                RayInfo newRay =new RayInfo(unit.ThirdRay.Origin, unit.ThirdRay.SVector);
                List<Path> pathtemp = this.GetSingleRayPaths(tx,ter,reArea,cityBuilding,newRay,txFrequencyBand);
                unit.ThirdRay.Flag = JudgeIfArriveRx(pathtemp);
                unit.ThirdRay.Path = pathtemp;
                unit.FirstRay.WhetherHaveTraced = true;
            }
            unit.HaveTraced = true;
            return;
        }


        /// <summary>
        ///求从辐射源发出的一条射线的所有路径
        /// </summary>
        /// <param name="tx">发射机</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="inRay">发射射线</param>
        /// <returns>该射线产生的路径</returns>
        private List<Path> GetSingleRayPaths(Node tx, Terrain ter, ReceiveArea reArea,City cityBuilding,RayInfo inRay, List<FrequencyBand> txFrequencyBand)
        {
      //      inRay = new RayInfo(tx.Position, new SpectVector(-0.27911979649,-0.52392969013, -0.55086104634));
            //AreaPath newPaths = new AreaPath(inRay, ter,reArea);
            //newPaths.SetAreaRayPathNodes(tx, ter, reArea, cityBuilding, txFrequencyBand);
            List<Path> temp = new List<Path>();
            if (tx.ChildNodes.Count != 0)
            {
                temp = this.GetLegalPathsArea(tx);
                tx.tempNode = null;
            }
            return temp;
        }

        /// <summary>
        ///提取态势追踪时的有效地路径
        /// </summary>
        ///  <param name="root">根节点</param>
        /// <returns>返回一条射线产生的有效地路径</returns>
        private  List<Path> GetLegalPathsArea(Node root)
        {
            List<Path> allPaths = GetAllPaths(root);//获取所有射线
            List<Path> areaPaths = DeleteNoneAreaCrossNodePaths(allPaths);//把没有穿过态势区域的射线删去
            for (int i = 0; i < areaPaths.Count; i++)//把射线在态势区域外的点删去
            {
                for (int j = areaPaths[i].node.Count - 1; j > 0; j--)
                {
                    if (areaPaths[i].node[j].NodeStyle != NodeStyle.AreaCrossNode)//若最后一个点不是Rx点
                    {
                        areaPaths[i].node.RemoveAt(j);
                    }
                    else
                    { break; }
                }
            }
            return areaPaths;
        }

        /// <summary>
        ///把没有经过态势区域的射线删去
        /// </summary>
        ///  <param name="multiple">所有路径</param>
        /// <returns>返回经过态势区域的路径</returns>
        private  List<Path> DeleteNoneAreaCrossNodePaths(List<Path> allPaths)
        {
            List<Path> remainPaths = new List<Path>();
            foreach (Path temp in allPaths)
            {
                for (int i = 0; i < temp.node.Count; i++)
                {
                    if (temp.node[i].NodeStyle == NodeStyle.AreaCrossNode)
                    {
                        temp.Rxnum = temp.node[i].RxNum;
                        remainPaths.Add(temp);
                        break;
                    }
                }
            }
            return remainPaths;
        }


        /// <summary>
        ///为在态势区域内的路径设置计算节点
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <returns></returns>
        private void SetSpacingNodeInAreaPath(Node tx, ReceiveArea reArea)
        {
            for (int i = 0; i < this.rayPaths.Count; i++)
            {
                if (!tx.IsReceiver)//若发射机不在态势区域内
                {
                    int areaCrossNodeNum = 0;//与态势区域交点的个数
                    for (int j = 1; j < this.rayPaths[i].node.Count - 1; j++)
                    {
                        if (this.rayPaths[i].node[j].NodeStyle == NodeStyle.AreaCrossNode)
                        {
                            areaCrossNodeNum++;
                            this.rayPaths[i].node[j].NodeStyle = NodeStyle.Rx;
                        }
                        if (areaCrossNodeNum % 2 == 1)
                        {
                            List<Node> rxNodes = makeNodesInTheLine(this.rayPaths[i].node[j], this.rayPaths[i].node[j + 1], reArea, reArea.spacing);
                            this.rayPaths[i].node.InsertRange(j, rxNodes);
                            j += rxNodes.Count;
                        }
                    }
                    if (this.rayPaths[i].node[this.rayPaths[i].node.Count - 1].NodeStyle == NodeStyle.AreaCrossNode)
                    { this.rayPaths[i].node[this.rayPaths[i].node.Count - 1].NodeStyle = NodeStyle.Rx; }

                }
                else//若发射机在态势区域内
                {
                    int areaCrossNodeNum = 0;//与态势区域交点的个数
                    for (int j = 1; j < this.rayPaths[i].node.Count; j++)
                    {
                        if (this.rayPaths[i].node[j].NodeStyle == NodeStyle.AreaCrossNode)
                        {
                            areaCrossNodeNum++;
                            this.rayPaths[i].node[j].NodeStyle = NodeStyle.Rx;
                        }
                        if (areaCrossNodeNum % 2 == 1 || (areaCrossNodeNum % 2 == 0 && this.rayPaths[i].node[j].NodeStyle != NodeStyle.AreaCrossNode))
                        {
                            List<Node> rxNodes = makeNodesInTheLine(this.rayPaths[i].node[j - 1], this.rayPaths[i].node[j], reArea, reArea.spacing);
                            this.rayPaths[i].node.InsertRange(j, rxNodes);
                            j += rxNodes.Count;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 将两个点连起来，然后用用spacing长度取点，存放入List中
        /// </summary>
        /// <param name="rxNode1">当前节点</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="rxNode2">下一个节点</param>
        /// <param name="spacing">间隔</param>
        /// <returns></returns>
        private List<Node> makeNodesInTheLine(Node rxNode1, Node rxNode2, ReceiveArea reArea, double spacing)
        {

            List<Node> rxNodes = new List<Node>();
            double distance = rxNode1.Position.GetDistance(rxNode2.Position);
            if (distance < spacing)
            {
                Node newNode = this.MakeAreaRxNode(rxNode1, rxNode2, reArea, 0.5 * distance);
                rxNodes.Add(newNode);
            }
            else
            {
                int index = 0;
                if (distance % spacing == 0)
                {
                    index = (int)(distance / spacing) - 1;
                }
                else
                {
                    index = (int)(distance / spacing);
                }
                for (int i = 1; i <= index; i++)
                {
                    //做新点的生成
                    Node newNode = this.MakeAreaRxNode(rxNode1, rxNode2, reArea, spacing * i);
                    rxNodes.Add(newNode);
                }
            }
            return rxNodes;

        }

        /// <summary>
        /// 构建态势区域内的Rx节点
        /// </summary>
        /// <param name="rxNode1">当前节点</param>
        /// <param name="rxNode2">下一个节点</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="distance">与当前节点的距离</param>
        /// <returns></returns>
        private Node MakeAreaRxNode(Node rxNode1, Node rxNode2, ReceiveArea reArea, double distance)
        {
            SpectVector vector12 = new SpectVector(rxNode1.Position, rxNode2.Position).GetNormalizationVector();
            Point position = new Point(rxNode1.Position.X + distance * vector12.a, rxNode1.Position.Y + distance * vector12.b, rxNode1.Position.Z + distance * vector12.c);
            Node rxNode = new Node();
            rxNode.Position = position;
            rxNode.NodeStyle = NodeStyle.Rx;
            rxNode.LayNum = rxNode1.LayNum;
            rxNode.DiffractionNum = rxNode1.DiffractionNum;
            rxNode.RayIn = rxNode2.RayIn;
            rxNode.IsReceiver = true;
            rxNode.IsEnd = false;
            rxNode.DistanceToFrontNode = distance;
            rxNode.RxNum = reArea.RxNum;
            rxNode.RayTracingDistance = rxNode1.RayTracingDistance + rxNode.DistanceToFrontNode;
            rxNode.NodeName = reArea.RxName;
            rxNode.UAN = reArea.UAN;
            return rxNode;
        }


        #region 传统静态发射发射，主要拿来比较
        /// <summary>
        ///全向静态发射射线
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="N">细分点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private List<Path> LuanchRaysOmnidirectionalInArea(Node tx,ReceiveArea reArea, Terrain ter, City cityBuilding, int N, List<FrequencyBand> txFrequencyBand)
        {

            List<Path> pathInfo = new List<Path>();
            List<RayTracingModel> originUnits = this.GetOriginUnitsOfIcosahedron(tx);
            HandleEachSurface(originUnits, pathInfo, tx, reArea, ter, cityBuilding, N, txFrequencyBand);
            return pathInfo;
        }

        /// <summary>
        ///全向静态发射射线，对每个三角形进行追踪
        /// </summary>
        /// <param name="units">处理单元</param>
        /// <param name="pathInfo">路径</param>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="N">初始细分点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private void HandleEachSurface(List<RayTracingModel> units, List<Path> pathInfo, Node tx, ReceiveArea reArea, Terrain ter, City cityBuilding, int N, List<FrequencyBand> txFrequencyBand)
        {
            double PA = 4 / (Math.Sqrt(10 + 2 * Math.Sqrt(5)));
            double AW = (PA / 2) / Math.Sin(36 * Math.PI / 180);
            for (int num = 0; num < units.Count; num++)
            {
                List<Point> vertex = this.GetSubDivisionPoints(units[num], N);
                for (int i = 0; i < vertex.Count; i++)
                {
                    RayInfo param = new RayInfo(tx.Position, new SpectVector(vertex[i].X, vertex[i].Y, vertex[i].Z));
                    List<Path> path = this.GetSingleRayPaths(tx, ter, reArea, cityBuilding, param, txFrequencyBand);
                    if (path.Count != 0)
                    {
                         pathInfo.AddRange(path); 
                    }
                }
            }
        }

        #endregion



        /// <summary>
        /// 根据不同的频率进行态势区域路径的分类，并进行场强，功率的计算
        /// </summary>
        ///  <param name="TxFrequencyBand">频段信息</param>
        ///  <param name="multiple">绕射距离倍数</param>
        /// <returns>筛选后的路径</returns>
        public  List<List<Path>> ScreenAreaPathsByFrequencyAndCalculation(List<FrequencyBand> TxFrequencyBand, int multiple)
        {
            List<List<Path>> classifiedPath = new List<List<CalculateModelClasses.Path>>();
            if (this.rayPaths.Count == 0)//若态势区域内没有射线，加入空的链表到输出结果中
            {
                for (int c = 0; c < TxFrequencyBand.Count; c++)
                { classifiedPath.Add(new List<Path>()); }
            }
            else
            {
                for (int i = 0; i < TxFrequencyBand.Count; i++)//对频段进行遍历
                {
                    double freDistance = multiple * 300 / TxFrequencyBand[i].MidPointFrequence;//波长的倍数，是判断发生绕射的条件
                    List<Path> paramPaths = new List<CalculateModelClasses.Path>();
                    for (int j = 0; j < this.rayPaths.Count; j++)//对在态势区域的射线进行遍历
                    {
                        if (JudgeIfDiffractionHappen(this.rayPaths[j], freDistance))//若该射线在该频段会发生
                        {

                            List<Node> pathNode = new List<Node>();//存放所有节点
                            List<Node> originNode = new List<Node>();//存放Tx,ReflectionNode,DiffractionNode节点，用于计算其后面节点
                            for (int n = 0; n < this.rayPaths[j].node.Count; n++)
                            {
                                Node temp = new Node(this.rayPaths[j].node[n]);
                                temp.Frequence = TxFrequencyBand[i].MidPointFrequence;
                                temp.FrequenceWidth = TxFrequencyBand[i].FrequenceWidth;
                                if (n == 0)
                                {
                                    temp.Power = TxFrequencyBand[i].Power;//第一个点为Tx点,输入功率
                                }
                                if (n != 0)
                                {
                                    this.GetEfield(temp.UAN, originNode[originNode.Count - 1], ref temp);//计算场强
                                    //接收点求功率,有问题，电导率和节点常数是面的还是空气的
                                    temp.Power = Power.GetPower( temp, originNode[originNode.Count - 1])[0];//计算功率
                                }
                                if (temp.NodeStyle != NodeStyle.Rx)
                                { originNode.Add(temp); }
                                pathNode.Add(temp);
                            }
                            Path midpath = new CalculateModelClasses.Path(pathNode);
                            midpath.Rxnum = this.rayPaths[j].Rxnum;
                            if (midpath.Rxnum != 0)
                            {
                                GetLossAndComponentOFPath(midpath);//计算路径的损耗，延时，相位
                                paramPaths.Add(midpath);
                            }
                        }
                    }
                    classifiedPath.Add(paramPaths);
                }
            }
            return classifiedPath;
        }


    }
}
