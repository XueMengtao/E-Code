using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using CityAndFloor;

namespace RayCalInfo
{
    /// <summary>
    ///射线管模型类
    /// </summary>
    public class RayTubeModel
    {
        private RayTubeCrossType crossType = RayTubeCrossType.NoneCrossNodes;
        private LaunchType launchType;//发射类型，分为点和棱
        private TubeType tubeType;
        private int tessellationTimes;//射线管细分的次数
        private int reflectionTimes;//反射追踪的次数
        private int diffractionTimes;//绕射追踪的次数
        private int passTimes;//穿过反射区域未发生反射的次数
        public List<int> crossLayerNum = new List<int>();//射线管与态势区域的第几层有交点
        private bool haveTraced;//射线的三条射线是否完成该次追踪
        private bool isReachingRx = false;//是否到达接收机的标志位
        private bool isReachingArea = false;//是否到达态势区域的标志位
        private List<OneRayModel> oneRayModels;//射线管的三条射线
        //      private Triangle rayTubeTriangle;//若射线管从反射面发射，该三角面代表射线管在反射面的形状,若是从辐射源或者绕射点发射，为波前面
        private Point launchPoint;//反射射线管和绕射射线管为虚拟发射点，直射射线管为发射点
        private AdjacentEdge launchEdge;//绕射射线管的发射棱

        public RayTubeModel FatherRayTube;
        public RayTubeCrossType CrossType
        {
            get { return this.crossType; }
        }
        public LaunchType LaunchType
        {
            get { return this.launchType; }
        }
        public TubeType WavefrontType
        {
            get { return this.tubeType; }
        }
        public List<OneRayModel> OneRayModels
        {
            get { return this.oneRayModels; }
            set { this.oneRayModels = value; }
        }
        public int ReflectionTracingTimes
        {
            get { return this.reflectionTimes; }
        }
        public int DiffractionTracingTimes
        {
            get { return this.diffractionTimes; }
        }
        public int PassTracingTimes
        {
            get { return this.passTimes; }
        }
        public bool HaveTraced
        {
            get { return this.haveTraced; }
        }
        public bool IsReachingRx
        {
            get { return this.isReachingRx; }
        }
        public bool IsReachingArea
        {
            get { return this.isReachingArea; }
        }
        public RayTubeModel(List<OneRayModel> rayModels)
        {
            if (rayModels == null || rayModels.Count != 3)
            {
                throw new ArgumentException("构造射线管模型时，输入的参数有错");
            }
            this.oneRayModels = rayModels;
        }
        //输入为从点发射的射线管时，三条射线随机摆放；输入为从棱发射的射线管时，按照0、1共面不共点0、2共面且共点1、2不共面的顺序存储
        public RayTubeModel(List<OneRayModel> rayModels, Point launchPoint, int reflectionTimes = 0, int diffractionTimes = 0,
            int tessellationTimes = 0, int passTimes = 0)
        {
            if (rayModels == null || rayModels.Count != 3)
            {
                throw new ArgumentException("构造射线管模型时，输入的参数有错");
            }
            this.launchType = LaunchType.Point;
            this.tubeType = TubeType.FlatTube;
            this.oneRayModels = rayModels;
            this.reflectionTimes = reflectionTimes;
            this.diffractionTimes = diffractionTimes;
            this.tessellationTimes = tessellationTimes;
            this.passTimes = passTimes;
            this.launchPoint = launchPoint;
            this.SetTheTracingFlag();
        }
        public RayTubeModel(List<OneRayModel> rayModels, Point launchPoint, AdjacentEdge launchEdge, int reflectionTimes = 0,
            int diffractionTimes = 0, int tessellationTimes = 0, int passTimes = 0)
        {
            if (rayModels == null || rayModels.Count != 3)
            {
                throw new ArgumentException("构造射线管模型时，输入的参数有错");
            }
            this.oneRayModels = rayModels;
            this.launchType = LaunchType.Line;
            this.tubeType = TubeType.CurveTube;
            this.reflectionTimes = reflectionTimes;
            this.diffractionTimes = diffractionTimes;
            this.tessellationTimes = tessellationTimes;
            this.passTimes = passTimes;
            this.launchPoint = launchPoint;
            this.launchEdge = launchEdge;
            this.SetTheTracingFlag();
        }


        /// <summary>
        ///更新射线管是否到达接收机的标志位
        /// </summary>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <returns></returns>
        public void UpdateTheReveivedFlag(Terrain ter, ReceiveBall rxBall, City buildings)
        {
            SpaceFace verticalFace = new SpaceFace(rxBall.Receiver, new SpectVector(this.oneRayModels[0].LaunchNode.Position, rxBall.Receiver));
            List<Point> crossPoints = new List<Point>();
            for (int i = 0; i < this.oneRayModels.Count; i++)//求射线管与该平面的交点
            {               
                Point crossPoint = this.oneRayModels[i].LaunchRay.GetCrossPointWithFace(verticalFace);
                if (crossPoint != null)
                { crossPoints.Add(crossPoint); }
            }
            if (crossPoints.Count != 3)
            {
                return;
            }
            else
            {
                Face wavefrontFace = new Triangle(crossPoints[0], crossPoints[1], crossPoints[2]);
                //接收机在射线管内
                if (wavefrontFace.JudgeIfPointInFace(rxBall.Receiver))
                {
                    OneRayModel rayModelToRx = this.StructureRayModelToTargetPoint(rxBall.Receiver);
                    if (rayModelToRx == null)
                    {
                        LogFileManager.ObjLog.error("判断点是否在从绕射棱发出的射线管内时,构造到接收机的射线时出错");
                    }
                    else
                    {
                        rayModelToRx.TracingThisRay(ter, buildings);//进行追踪
                        //若该射线与地形，建筑物无交点或者交点在接收机后面，则说明该射线管到达接收机
                        if ((rayModelToRx.CrossNode == null) || (rayModelToRx.CrossNode != null &&
                            rayModelToRx.CrossNode.DistanceToFrontNode >= rayModelToRx.LaunchNode.Position.GetDistance(rxBall.Receiver)))
                        {
                            //构造接收节点
                            Node rx = new Node
                            {
                                Position = rxBall.Receiver,
                                RxNum = rxBall.RxNum,
                                UAN = rxBall.UAN,
                                NodeStyle = NodeStyle.Rx,
                                DistanceToFrontNode = rayModelToRx.LaunchNode.Position.GetDistance(rxBall.Receiver),
                                RayIn = rayModelToRx.LaunchRay,
                                FatherNode = rayModelToRx.LaunchNode
                            };
                            //rayModelToRx.LaunchNode.ChildNodes.Add(rx);
                            this.isReachingRx = true;
                            for (int i = 0; i < this.oneRayModels.Count; i++)
                            {
                                this.oneRayModels[i].LaunchNode.ChildNodes.Add(rx);
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 更新射线管是否到达态势区域的标志位
        /// </summary>
        /// <param name="reArea">态势区域</param>
        /// <param name="buildings">建筑物</param>
        public void UpdateAreaSituationFlag(ReceiveArea reArea)
        {
            Node crossNode = new Node();
            for (int i = 0; i < reArea.areaSituationRect.Count; i++)//依次对态势层进行循环
            {
                Rectangle currentRect = reArea.areaSituationRect[i];//当前态势层
                for (int j = 0; j < this.oneRayModels.Count; j++)//对组成射线管的三条射线进行循环
                {
                    crossNode = this.oneRayModels[j].LaunchRay.GetCrossNodeWithRect(currentRect);//计算当前射线与态势层有无交点
                    if (crossNode != null)//表明当前射线与此态势层存在相交点
                    {
                        break;//跳出射线管循环
                    }
                }
                if (crossNode != null)//当前射线管与当前态势层存在相交节点
                {
                    this.isReachingArea = true;//更新射线管是否到达态势区域的标志位
                    this.crossLayerNum.Add(i);
                }
            }
        }

        /// <summary>
        ///对射线管的射线进行追踪
        /// </summary>
        /// <param name="ter">地形</param>
        ///  <param name="buildings">建筑物</param>
        /// <returns></returns>
        public void TracingThisRayTubeModel(Terrain ter, City buildings)
        {
            for (int i = 0; i < this.oneRayModels.Count; i++)
            {
                if (!this.oneRayModels[i].HaveTraced && (!double.IsNaN(this.oneRayModels[i].LaunchRay.Origin.X)))//若该射线还没有追踪，进行追踪
                {
                    this.oneRayModels[i].TracingThisRay(ter, buildings);
                }
            }
            this.haveTraced = true;
            this.UpdataRayTubeCrossFlag();

        }

        /// <summary>
        ///根据射线管每条射线的交点情况判断该射线管是否需要细分
        /// </summary>
        /// <returns></returns>
        public bool JudgeIfThisModelNeedToBeDivided()
        {
            switch (this.crossType)
            {
                case RayTubeCrossType.NoneCrossNodes:
                case RayTubeCrossType.TFF:
                case RayTubeCrossType.FTF:
                case RayTubeCrossType.FFT:
                case RayTubeCrossType.TaTaF:
                case RayTubeCrossType.TaFTa:
                case RayTubeCrossType.FTaTa:
                case RayTubeCrossType.TaTaTb_A:
                case RayTubeCrossType.TaTbTa_A:
                case RayTubeCrossType.TaTbTb_A:
                    return false;
                default:
                    if (this.tessellationTimes > 2)
                    { return false; }
                    else
                    { return true; }
            }
        }

        /// <summary>
        /// 根据射线管三条射线与地形面相交情况来判断细分的边的段数
        /// </summary>
        /// <returns></returns>
        public int GetDivisionNumOfLine(double lengthX, double lengthY)
        {
            double maxLength = 0;
            for(int i =0; i <= 2; i++)
            {
                if (this.oneRayModels[i].CrossNode != null)
                {
                    Face crossFace = this.oneRayModels[i].CrossNode.ReflectionFace.SwitchToSpaceFace();
                    Point vertex1 = this.oneRayModels[0].LaunchRay.GetCrossPointWithFace(crossFace);
                    Point vertex2 = this.oneRayModels[1].LaunchRay.GetCrossPointWithFace(crossFace);
                    Point vertex3 = this.oneRayModels[2].LaunchRay.GetCrossPointWithFace(crossFace);
                    if (vertex1 != null && vertex2 != null) maxLength = Math.Max(maxLength, vertex1.GetDistance(vertex2));
                    if (vertex2 != null && vertex3 != null) maxLength = Math.Max(maxLength, vertex3.GetDistance(vertex2));
                    if (vertex1 != null && vertex3 != null) maxLength = Math.Max(maxLength, vertex1.GetDistance(vertex3));
                }
            }
            maxLength = Math.Min(maxLength, lengthX < lengthY ? lengthX : lengthY);
            return (int)maxLength / 70 + 2;//认为70为地形三角面边的最小值
        }

        /// <summary>
        ///细分射线管模型
        /// </summary>
        /// <returns>细分后的射线管模型</returns>
        public List<RayTubeModel> GetDivisionRayTubeModels(int divisionNum)
        {
            if (this.tessellationTimes > 2)//细分达到上限
            { return new List<RayTubeModel>(); }
            else
            {
                if (this.tubeType == TubeType.FlatTube)
                {
                    return this.GetFlatTubeDivisionModels(divisionNum);
                }
                else
                {
                    return this.GetCurveTubeDivisionModels(divisionNum);
                }
            }
        }
        

        /// <summary>
        ///细分平面射线管模型
        /// </summary>
        /// <returns>细分后的射线管模型</returns>
        private List<RayTubeModel> GetFlatTubeDivisionModels(int divisionNum)
        {
            //三角形截面射线管不会从绕射点上发出
            if (this.launchPoint == null)
            { 
                LogFileManager.ObjLog.error("三角形截面射线管的镜像发射点个数不为1"); 
            }
            List<Point>[] divisionPoints = this.GetDivisionPointsOnTriangle(divisionNum);
            if (divisionPoints == null) return new List<RayTubeModel>();
            List<OneRayModel>[] divisionRays = new List<OneRayModel>[divisionNum + 1];
            //射线管是从辐射源或特殊绕射点发出
            if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.Tx || this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode)
            {
                for (int i = 0; i < divisionNum + 1; i++)
                {
                    divisionRays[i] = new List<OneRayModel>();
                    for (int j = 0; j <= i; j++)
                        divisionRays[i].Add(new OneRayModel(this.oneRayModels[0].LaunchNode, 
                            new RayInfo(this.oneRayModels[0].LaunchNode.Position, divisionPoints[i][j])));
                } 
            }
            else //射线管是反射射线管
            {
                SpaceFace reflectionLaunchFace = this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace();
                for (int i = 0; i < divisionNum + 1; i++)
                {
                    divisionRays[i] = new List<OneRayModel>();
                    for (int j = 0; j <= i; j++)
                        divisionRays[i].Add(new OneRayModel(this.StructureNewLaunchNode(
                            new RayInfo(this.launchPoint, divisionPoints[i][j]).GetCrossPointWithFace(reflectionLaunchFace)),
                            new RayInfo(this.launchPoint, divisionPoints[i][j])));
                }
            }
            return this.GetDivisionRayTubes(divisionRays);
        }


        /// <summary>
        ///细分曲面射线管模型
        /// </summary>
        /// <returns>细分后的射线管模型</returns>
        private List<RayTubeModel> GetCurveTubeDivisionModels(int divisionNum)
        { 
            List<OneRayModel>[] divisionRays = new List<OneRayModel>[divisionNum + 1];
            if (this.launchType == LaunchType.Line)//射线管从线段发射
            {
                if (this.launchEdge == null || this.launchPoint == null)
                {
                    LogFileManager.ObjLog.error("曲面射线管细分时无法找到虚拟发射点和棱");
                    return new List<RayTubeModel>();
                }
                for (int i = 0; i < this.oneRayModels.Count; i++)
                {
                    if (this.oneRayModels[i].LaunchRay.RayVector.a == 0 && this.oneRayModels[i].LaunchRay.RayVector.b == 0
                        && this.oneRayModels[i].LaunchRay.RayVector.c == 0)
                        return new List<RayTubeModel>();
                }
                List<Point>[] divisionPoints = this.GetDivisionPointsOnTriangle(divisionNum);
                if (divisionPoints == null) return new List<RayTubeModel>();
                //取一个面作为参考，求射线管各个射线打到这个面上的交点，得到波前，对其进行细分
                if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode)
                {
                    for (int i = 0; i < divisionNum + 1; i++)
                    {
                        divisionRays[i] = new List<OneRayModel>();
                        for (int j = 0; j <= i; j++)
                        {
                            Point newLaunchPoint = this.launchEdge.GetInfiniteDiffractionPoint(this.launchPoint, divisionPoints[i][j]);
                            divisionRays[i].Add(new OneRayModel(this.StructureNewLaunchNode(newLaunchPoint),
                                new RayInfo(newLaunchPoint, divisionPoints[i][j])));
                        }
                    }
                }
                else if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.ReflectionNode)
                {
                    SpaceFace reflectionLaunchFace = this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace();
                    for (int i = 0; i < divisionNum + 1; i++)
                    {
                        divisionRays[i] = new List<OneRayModel>();
                        for (int j = 0; j <= i; j++)
                        {
                            Point newLaunchPoint = new RayInfo(this.launchEdge.GetInfiniteDiffractionPoint(
                                this.launchPoint, divisionPoints[i][j]), divisionPoints[i][j]).GetCrossPointOfStraightLineAndInfiniteFace(reflectionLaunchFace);
                            divisionRays[i].Add(new OneRayModel(this.StructureNewLaunchNode(newLaunchPoint),
                                new RayInfo(newLaunchPoint, divisionPoints[i][j])));
                        }
                    }
                }
                else
                {
                    LogFileManager.ObjLog.error("曲面射线管不可能是从除了棱和反射的方式得到的");
                    return new List<RayTubeModel>();
                }
            }
            else
            {
                LogFileManager.ObjLog.error("曲面射线管不可能不是从棱发出的");
                return new List<RayTubeModel>();
            }
            return this.GetDivisionRayTubes(divisionRays);
        }


        /// <summary>
        /// 得到需要细分的射线管波前面上的细分点
        /// </summary>
        /// <param name="divisionNum">三角形波前面的边被细分的段数</param>
        /// <returns>细分点</returns>
        private List<Point>[] GetDivisionPointsOnTriangle(int divisionNum)
        {
            Point vertex1 = this.OneRayModels[0].LaunchRay.GetPointOnRayVector(10);//取三角形的顶点
            Point vertex2 = this.OneRayModels[1].LaunchRay.GetPointOnRayVector(10);//三角形的顶点
            Point vertex3 = this.OneRayModels[2].LaunchRay.GetPointOnRayVector(10);//三角形的顶点
            RayInfo rayFromVertex1To2 = new RayInfo(vertex1, new SpectVector(vertex1, vertex2));//顶点1到2的射线
            SpectVector vectorFromVertex2To3 = new SpectVector(vertex2, vertex3);//顶点2到3的射线
            double unitLength1 = vertex1.GetDistance(vertex2) / divisionNum;//每段的距离
            double unitLength2 = vertex2.GetDistance(vertex3) / divisionNum;
            if (unitLength1 < 0.000001 || unitLength2 < 0.000001) return null;
            Point[] pointOfEdge12 = new Point[divisionNum]; //存放棱12上的点,不包括顶点1
            for (int i = 0; i < divisionNum; i++)//按间隔在棱12上去点
            {
                pointOfEdge12[i] = rayFromVertex1To2.GetPointOnRayVector((i + 1) * unitLength1);

            }
            List<Point>[] vertexPoints = new List<Point>[divisionNum + 1];//存放三角形内每条平行边上的点
            vertexPoints[0] = new List<Point> { vertex1 };//把顶点1放到数组第一位
            for (int j = 1; j <= divisionNum; j++)//得到三角形切分后每条边上的点
            {
                vertexPoints[j] = new List<Point>();
                RayInfo lineRay = new RayInfo(pointOfEdge12[j - 1], vectorFromVertex2To3);
                for (int i = 1; i <= j; i++)//根据端点和方向向量，得到在射线方向上每一个点
                {
                    Point temp = lineRay.GetPointOnRayVector(i * unitLength2);
                    vertexPoints[j].Add(temp);
                }
                vertexPoints[j].Insert(0, pointOfEdge12[j - 1]);
            }
            return vertexPoints;
        }

        /// <summary>
        /// 根据细分射线模型构造细分后的射线管
        /// </summary>
        private List<RayTubeModel> GetDivisionRayTubes(List<OneRayModel>[] divisionRays)
        {
            List<RayTubeModel> res = new List<RayTubeModel>();
            for (int i = 0; i < divisionRays.Length - 1; i++)//得到正立的三角面射线管
            {
                for (int j = 0; j < divisionRays[i].Count; j++)
                {
                    if (this.tubeType == TubeType.CurveTube)
                    {
                        RayTubeModel tempTube = new RayTubeModel(new List<OneRayModel> { divisionRays[i][j], divisionRays[i + 1][j], divisionRays[i + 1][j + 1] },
                            this.launchPoint, this.launchEdge, this.reflectionTimes, this.diffractionTimes, this.tessellationTimes + 1, this.passTimes);
                        tempTube.FatherRayTube = this.FatherRayTube;
                        res.Add(tempTube);
                    }
                    else
                    {
                        RayTubeModel tempTube = new RayTubeModel(new List<OneRayModel> { divisionRays[i][j], divisionRays[i + 1][j], divisionRays[i + 1][j + 1] },
                            this.launchPoint, this.reflectionTimes, this.diffractionTimes, this.tessellationTimes + 1, this.passTimes);
                        tempTube.FatherRayTube = this.FatherRayTube;
                        res.Add(tempTube);
                    }
                }
            }
            if (divisionRays.Length > 2)//若三角形平分大于2层，得到倒立的三角面射线管
            {
                for (int m = 2; m < divisionRays.Length; m++)
                {
                    for (int n = 1; n < divisionRays[m].Count - 1; n++)
                    {
                        if (this.tubeType == TubeType.CurveTube)
                        {
                            RayTubeModel tempTube = new RayTubeModel(new List<OneRayModel> { divisionRays[m][n], divisionRays[m - 1][n - 1], divisionRays[m - 1][n] },
                                this.launchPoint, this.launchEdge, this.reflectionTimes, this.diffractionTimes, this.tessellationTimes + 1, this.passTimes);
                            tempTube.FatherRayTube = this.FatherRayTube;
                            res.Add(tempTube);
                        }
                        else
                        {
                            RayTubeModel tempTube = new RayTubeModel(new List<OneRayModel> { divisionRays[m][n], divisionRays[m - 1][n - 1], divisionRays[m - 1][n] },
                                this.launchPoint, this.reflectionTimes, this.diffractionTimes, this.tessellationTimes + 1, this.passTimes);
                            tempTube.FatherRayTube = this.FatherRayTube;
                            res.Add(tempTube);
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        ///获取射线管打到地形面后下一阶段的射线管模型
        /// </summary>
        /// <returns>细分后的射线管模型</returns>
        public List<RayTubeModel> GetNextRayTubeModels()
        {
            //平面射线管只可能从点发出
            List<RayTubeModel> nextModels = new List<RayTubeModel>();
            if (this.crossType == RayTubeCrossType.NoneCrossNodes)//射线都没有交点
            {
                return nextModels;
            }
            else if (this.crossType == RayTubeCrossType.FTaTa)//射线0无交点,射线1,2在同一个三角面
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTaTaF(this.oneRayModels[1], this.oneRayModels[2], this.oneRayModels[0]));
            }
            else if (this.crossType == RayTubeCrossType.TaFTa)//射线1无交点,射线0,2在一个三角面
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTaTaF(this.oneRayModels[0], this.oneRayModels[2], this.oneRayModels[1]));
            }
            else if (this.crossType == RayTubeCrossType.TaTaF)//射线2无交点,射线0,1在一个三角面
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTaTaF(this.oneRayModels[0], this.oneRayModels[1], this.oneRayModels[2]));
            }
            else if (this.crossType == RayTubeCrossType.FFT)//射线2有交点，射线0,1无交点
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTFF(this.oneRayModels[2], this.oneRayModels[0], this.oneRayModels[1]));
            }
            else if (this.crossType == RayTubeCrossType.TFF)//射线0有交点，射线1,2无交点
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTFF(this.oneRayModels[0], this.oneRayModels[1], this.oneRayModels[2]));
            }
            else if (this.crossType == RayTubeCrossType.FTF)//射线1有交点，射线0,2无交点
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTFF(this.oneRayModels[1], this.oneRayModels[0], this.oneRayModels[2]));
            }
            else if (this.crossType == RayTubeCrossType.TaTaTa_S)//三条射线都有交点,都在一个平面上
            {
                RayTubeModel tempRayTubeModel = this.GetReflectionModels(
                    new List<Node> { this.oneRayModels[0].CrossNode, this.oneRayModels[1].CrossNode, this.oneRayModels[2].CrossNode },
                    this.oneRayModels[0].CrossNode.ReflectionFace);
                if (tempRayTubeModel != null)
                {
                    nextModels.Add(tempRayTubeModel);
                    this.oneRayModels[0].LaunchNode.ChildNodes.Add(nextModels[0].oneRayModels[0].LaunchNode);
                }
            }
            else if (this.crossType == RayTubeCrossType.TaTaTb_A)//射线0,1的交点在一个三角面,射线2的在相邻三角面
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTaTaTb(this.oneRayModels[0], this.oneRayModels[1], this.oneRayModels[2]));
            }
            else if (this.crossType == RayTubeCrossType.TaTbTa_A)//射线0,2的交点在一个三角面,射线1的在相邻三角面
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTaTaTb(this.oneRayModels[0], this.oneRayModels[2], this.oneRayModels[1]));
            }
            else if (this.crossType == RayTubeCrossType.TaTbTb_A)//射线1,2的交点在一个三角面,射线0的在相邻三角面
            {
                nextModels.AddRange(this.GetNextRayTubeModelsWhenTaTaTb(this.oneRayModels[1], this.oneRayModels[2], this.oneRayModels[0]));
            }
            for (int i = 0; i < nextModels.Count; i++)
            {
                if (nextModels[i].FatherRayTube == null) { nextModels[i].FatherRayTube = this; }
            }
            return nextModels;
        }



        /// <summary>
        ///判断射线管模型的射线是否已经追踪
        /// </summary>
        /// <returns></returns>
        private void SetTheTracingFlag()
        {
            int haveTracedRaysCount = 0;
            for (int i = 0; i < this.oneRayModels.Count; i++)
            {
                if (this.oneRayModels[i].HaveTraced)
                { haveTracedRaysCount++; }
            }
            if (haveTracedRaysCount == this.oneRayModels.Count)
            {
                this.haveTraced = true;
                this.UpdataRayTubeCrossFlag();
            }
            else
            { this.haveTraced = false; }
        }

        /// <summary>
        /// 获取反射射线管和未发生反射的射线管
        /// </summary>
        /// <param name="crossRay11">与面1相交的射线</param>
        /// <param name="crossRay12">与面1相交的射线2</param>
        /// <param name="crossRay2">与面2相交的射线</param>
        /// <returns></returns>
        private List<RayTubeModel> GetNextRayTubeModelsWhenTaTaTb(OneRayModel crossRay11, OneRayModel crossRay12, OneRayModel crossRay2)
        {
            List<RayTubeModel> nextModels = new List<RayTubeModel>();

            int edgeNum11 = -1, edgeNum12 = -1, edgeNum21 = -1, edgeNum22 = -1;
            List<List<Point>> newTubePoints = GetReflectAndPassPoints(crossRay11, crossRay12, crossRay2, 
                ref edgeNum11, ref edgeNum12, ref edgeNum21, ref edgeNum22);
            if (newTubePoints.Count == 0) return nextModels;
            AdjacentEdge sameEdge = new AdjacentEdge(crossRay11.CrossNode.ReflectionFace, crossRay2.CrossNode.ReflectionFace);
            //获得面1上的反射射线管
            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                newTubePoints[0], crossRay11.CrossNode.ReflectionFace, new List<OneRayModel> { crossRay12, crossRay11 }));
            //获得面2上的反射射线管
            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                newTubePoints[1], crossRay2.CrossNode.ReflectionFace, new List<OneRayModel> { crossRay2 }));
            if (!(crossRay11.CrossNode.ReflectionFace.Lines[edgeNum11].isSameAdjacent(sameEdge) &&
                crossRay11.CrossNode.ReflectionFace.Lines[edgeNum12].isSameAdjacent(sameEdge)&&
                crossRay2.CrossNode.ReflectionFace.Lines[edgeNum21].isSameAdjacent(sameEdge)&&
                crossRay2.CrossNode.ReflectionFace.Lines[edgeNum22].isSameAdjacent(sameEdge)))
            {
                //获得未发生反射的射线管
                for (int i = 2; i < newTubePoints.Count; i++)
                { nextModels.AddRange(GetPassRayTubeModels(newTubePoints[i], crossRay11, crossRay2)); }
            }
            //获得绕射射线
            nextModels.AddRange(this.GetDiffrectTubeOnAdjReflectFaces(newTubePoints[0], newTubePoints[1], crossRay11, crossRay2,
                edgeNum11, edgeNum12, edgeNum21, edgeNum22));
            return nextModels;
        }

        /// <summary>
        /// 生成一个list，里面的第一第二个元素分别为反射面1和2中反射多边形的顶点，若有第三个或第四个元素，为未发生反射其构成射线管的波前面各个顶点
        /// </summary>
        /// <param name="crossRay11">与面1相交</param>
        /// <param name="crossRay12">与面1相交</param>
        /// <param name="crossRay2">与面2相交</param>
        private List<List<Point>> GetReflectAndPassPoints(OneRayModel crossRay11, OneRayModel crossRay12, OneRayModel crossRay2,
            ref int edgeNum11, ref int edgeNum12, ref int edgeNum21, ref int edgeNum22)
        {
            List<List<Point>> res = new List<List<Point>>();
            List<Point> crossPointsOfFace1 = new List<Point>(), crossPointsOfFace2 = new List<Point>();
            AdjacentEdge sameEdge = new AdjacentEdge(crossRay11.CrossNode.ReflectionFace, crossRay2.CrossNode.ReflectionFace);
            //获得各侧面与地形面的交点，及交点所在地形面棱的编号
            Point point11to2OnFace1 = this.GetCrossPointOfLineAndFaceLines(crossRay11, crossRay2, ref edgeNum11);
            Point point11to2OnFace2 = this.GetCrossPointOfLineAndFaceLines(crossRay2, crossRay11, ref edgeNum21);
            Point point12to2OnFace1 = this.GetCrossPointOfLineAndFaceLines(crossRay12, crossRay2, ref edgeNum12);
            Point point12to2OnFace2 = this.GetCrossPointOfLineAndFaceLines(crossRay2, crossRay12, ref edgeNum22);
            if (point11to2OnFace1 == null || point11to2OnFace2 == null || point12to2OnFace1 == null || point12to2OnFace2 == null)
                return res;
            //分别获得两三角面上发生反射多边形的各个顶点，并按顺序存储
            GetCrossPoinsOnOneFace(crossPointsOfFace1, crossRay11.CrossNode.ReflectionFace, edgeNum11, edgeNum12, sameEdge, point11to2OnFace1, point12to2OnFace1);
            GetCrossPoinsOnOneFace(crossPointsOfFace2, crossRay2.CrossNode.ReflectionFace, edgeNum21, edgeNum22, sameEdge, point11to2OnFace2, point12to2OnFace2);
            res.Add(crossPointsOfFace1);
            res.Add(crossPointsOfFace2);
            //获得未发生反射部分的List
            res.AddRange(GetNewTubePoints(edgeNum11, edgeNum12, crossRay11.CrossNode.ReflectionFace, sameEdge, 
                point11to2OnFace1, point12to2OnFace1, point11to2OnFace2, point12to2OnFace2));

            return res;
        } 

        /// <summary>
        /// 求ray1，ray2 与ray1平面交点的连线与ray1平面三棱的交点，设置交点所在棱的编号
        /// </summary>
        /// <param name="edgeNumber">交点所在棱的编号</param>
        /// <returns>交点</returns>
        private Point GetCrossPointOfLineAndFaceLines(OneRayModel ray1, OneRayModel ray2, ref int edgeNumber)
        {
            Point crossPoint = null;
            RayInfo rayFromCrossPointTovirtualPoint = new RayInfo(ray1.CrossNode.Position,
                ray2.LaunchRay.GetCrossPointOfStraightLineAndInfiniteFace(ray1.CrossNode.ReflectionFace.SwitchToSpaceFace()));
            List<AdjacentEdge> faceLines = ray1.CrossNode.ReflectionFace.Lines;
            for (int i = 0; i < faceLines.Count; i++)
            {
                crossPoint = rayFromCrossPointTovirtualPoint.GetCrossPointWithOtherRay(faceLines[i].SwitchToRay());
                if (crossPoint != null && faceLines[i].JudgeIfPointInLineRange(crossPoint) &&
                     new SpectVector(rayFromCrossPointTovirtualPoint.Origin, crossPoint).IsParallelAndSamedirection(rayFromCrossPointTovirtualPoint.RayVector))
                {
                    edgeNumber = i;
                    break;
                }
                else
                { crossPoint = null; }
            }

            return crossPoint;
        }

        /// <summary>
        /// 求一个面上反射多边形的所有顶点的按序list
        /// </summary>
        /// <param name="crossPoints">需返回的list</param>
        /// <param name="face">面</param>
        /// <param name="isSameLine">两棱上交点是否在同一棱上</param>
        /// <param name="sameEdge">面与相邻面的公共棱</param>
        /// <param name="p1">棱上交点1</param>
        /// <param name="p2">棱上交点2</param>
        /// <param name="cp1">三角形顶点1</param>
        /// <param name="cp2">三角形顶点2</param>
        private void GetCrossPoinsOnOneFace(List<Point> crossPoints, Face face, int edgeNum1, int edgeNum2, AdjacentEdge sameEdge, Point p1, Point p2)
        {
            crossPoints.Add(p1);
            if (edgeNum1 != edgeNum2)
            {
                if (!p1.isInline(sameEdge)) crossPoints.AddRange(face.Lines[edgeNum1].GetCommonPointsWithOtherLine(sameEdge));
                if (!p2.isInline(sameEdge)) crossPoints.AddRange(face.Lines[edgeNum2].GetCommonPointsWithOtherLine(sameEdge));
            }
            crossPoints.Add(p2);
        }


        /// <summary>
        /// 获取未发生反射部分波前面多边形的各个顶点
        /// </summary>
        /// <param name="edgeNum1">p11与面1相交棱编号</param>
        /// <param name="edgeNum2">p12与面1相交棱编号</param>
        /// <param name="face1"></param>
        /// <param name="sameEdge">俩面的公共棱</param>
        /// <param name="p11">面1上第一交点</param>
        /// <param name="p12">面1上第二交点</param>
        /// <param name="p21">面2上第一交点</param>
        /// <param name="p22">面2上第二交点</param>
        /// <returns></returns>
        private List<List<Point>> GetNewTubePoints(int edgeNum1, int edgeNum2, Face face1, AdjacentEdge sameEdge, Point p11, Point p12, Point p21, Point p22)
        {
            List<List<Point>> res = new List<List<Point>>();
            if (!p11.isInline(sameEdge))
            {
                if (edgeNum1 == edgeNum2)
                {
                    res.Add(new List<Point>{p11, p12, p22, p21});
                }
                else
                {
                    List<Point> tubePoints1 = new List<Point>{p11, p21};
                    tubePoints1.AddRange(face1.Lines[edgeNum1].GetCommonPointsWithOtherLine(sameEdge));
                    res.Add(tubePoints1);
                    if(!p12.isInline(sameEdge))
                    {
                        List<Point> tubePoints2 = new List<Point>{p12, p22};
                        tubePoints2.AddRange(face1.Lines[edgeNum2].GetCommonPointsWithOtherLine(sameEdge));
                        res.Add(tubePoints2);
                    }
                }
            }
            else if (!p12.isInline(sameEdge))
            {
                List<Point> tubePoints1 = new List<Point>{p12, p22};
                tubePoints1.AddRange(face1.Lines[edgeNum2].GetCommonPointsWithOtherLine(sameEdge));
                res.Add(tubePoints1);
            }
            return res;
        }

        /// <summary>
        /// 求未发生反射的部分重构而成的射线管
        /// </summary>
        private List<RayTubeModel> GetPassRayTubeModels(List<Point> crossPoints, OneRayModel crossRay1, OneRayModel crossRay2)
        {
            List<RayTubeModel> temp = new List<RayTubeModel>(), res = new List<RayTubeModel>();
            if (crossPoints.Count < 3)
            { LogFileManager.ObjLog.error("构造反射射线管是，射线小于三条"); return res; }
            if (this.tubeType == TubeType.FlatTube)//平面射线管
            {
                if (this.launchPoint == null) LogFileManager.ObjLog.error("三角形截面射线管的镜像发射点个数不为1"); 
                if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.Tx || this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode)
                {//若射线管为由发射机发出，则以发射点直接构管
                    List<OneRayModel> rayModels = new List<OneRayModel>();
                    for(int i = 0; i  < crossPoints.Count; i++)
                        rayModels.Add(new OneRayModel(crossRay1.LaunchNode, new RayInfo(crossRay1.LaunchNode.Position, crossPoints[i])));
                    for(int i = 1; i < crossPoints.Count - 1; i++)
                    {
                        temp.Add(new RayTubeModel(new List<OneRayModel> {rayModels[0], rayModels[i], rayModels[i+1]},
                            this.launchPoint, this.reflectionTimes, this.diffractionTimes,this.tessellationTimes, this.passTimes + 1));
                    }
                }
                else//若是反射射线管，则需找到反射面上对应的点再构管
                {
                    Face launchFace = crossRay1.LaunchNode.ReflectionFace;
                    List<OneRayModel> rayModels = new List<OneRayModel>();
                    for(int i = 0; i < crossPoints.Count; i++)
                    {
                        rayModels.Add(new OneRayModel(
                            this.StructureNewLaunchNode(new RayInfo(this.launchPoint, crossPoints[i]).GetCrossPointWithFace(launchFace)), 
                            new RayInfo(this.launchPoint, crossPoints[i])));
                    }
                    for(int i = 1; i < crossPoints.Count - 1; i++)
                    {
                        temp.Add(new RayTubeModel(new List<OneRayModel> {rayModels[0], rayModels[i], rayModels[i+1]},
                            this.launchPoint, this.reflectionTimes, this.diffractionTimes,this.tessellationTimes, this.passTimes + 1));
                    }
                }
            }
            else if (this.tubeType == TubeType.CurveTube)//曲面射线管
            {
                if (this.launchEdge == null || this.launchPoint == null) LogFileManager.ObjLog.error("曲面射线管细分时无法找到虚拟发射点和棱");
                if (crossRay1.LaunchNode.NodeStyle == NodeStyle.DiffractionNode)//射线管从线段发射
                {
                    List<OneRayModel> rayModels = new List<OneRayModel>();
                    for (int i = 0; i < crossPoints.Count; i++)
                    {
                        Point diffractPoint = this.launchEdge.GetInfiniteDiffractionPoint(launchPoint, crossPoints[i]);
                        rayModels.Add(new OneRayModel(this.StructureNewLaunchNode(diffractPoint), new RayInfo(diffractPoint, crossPoints[i])));
                    }
                    for (int i = 1; i < crossPoints.Count - 1; i++)
                    {
                        temp.Add(new RayTubeModel(new List<OneRayModel> { rayModels[0], rayModels[i], rayModels[i + 1] },
                            this.launchPoint, this.launchEdge, this.reflectionTimes, this.diffractionTimes, this.tessellationTimes, this.passTimes + 1));
                    }
                }
                else if(crossRay1.LaunchNode.NodeStyle == NodeStyle.ReflectionNode)
                {
                    List<OneRayModel> rayModels = new List<OneRayModel>();
                    for (int i = 0; i < crossPoints.Count; i++)
                    {
                        Point beginPoint = this.launchEdge.GetInfiniteDiffractionPoint(launchPoint, crossPoints[i]);
                        if (beginPoint == null) return res;
                        Point reflectPoint = new RayInfo(beginPoint, crossPoints[i]).
                            GetCrossPointOfStraightLineAndInfiniteFace(crossRay1.LaunchNode.ReflectionFace.SwitchToSpaceFace());
                        rayModels.Add(new OneRayModel(this.StructureNewLaunchNode(reflectPoint), new RayInfo(reflectPoint, crossPoints[i])));
                    }
                    for (int i = 1; i < crossPoints.Count - 1; i++)
                    {
                        temp.Add(new RayTubeModel(new List<OneRayModel> { rayModels[0], rayModels[i], rayModels[i + 1] },
                            this.launchPoint, this.launchEdge, this.reflectionTimes, this.diffractionTimes, this.tessellationTimes, this.passTimes + 1));
                    }
                }
            }
            for (int i = 0; i < temp.Count; i++)
            {
                if (this.IsPass(temp[i], crossRay1.CrossNode.ReflectionFace, crossRay2.CrossNode.ReflectionFace))
                {
                    temp[i].FatherRayTube = this.FatherRayTube;
                    res.Add(temp[i]);
                }
            }
            return res;
        }

        /// <summary>
        /// 判断射线管是否真的没有打在两个相邻的三角面上
        /// </summary>
        /// <param name="rayTube">射线管</param>
        /// <param name="face1">三角面1</param>
        /// <param name="face2">三角面2</param>
        private bool IsPass(RayTubeModel rayTube, Face face1, Face face2)
        {
            bool pass = true;
            for (int i = 0; i < rayTube.oneRayModels.Count; i++)
            {
                Point temp = rayTube.oneRayModels[i].LaunchRay.GetCrossPointWithFace(face1);
                if (temp != null && face1.Lines[0].getDistanceP2Line(temp) > 0.000001 && face1.Lines[1].getDistanceP2Line(temp) > 0.000001
                    && face1.Lines[2].getDistanceP2Line(temp) > 0.000001)
                {
                    pass = false;
                    break;
                }
            }
            if (pass)
            {
                for (int i = 0; i < rayTube.oneRayModels.Count; i++)
                {
                    Point temp = rayTube.oneRayModels[i].LaunchRay.GetCrossPointWithFace(face2);
                    if (temp != null && face2.Lines[0].getDistanceP2Line(temp) > 0.000001 && face2.Lines[1].getDistanceP2Line(temp) > 0.000001
                        && face2.Lines[2].getDistanceP2Line(temp) > 0.000001)
                    {
                        pass = true;
                        break;
                    }
                }
            }
            if (!pass)
            {
                int i = 1;
            }
            return pass;
        }

        /// <summary>
        /// 根据两面的点list生成绕射射线管
        /// </summary>
        /// <param name="pointsForFace1">面1棱上的点</param>
        /// <param name="pointsForFace2">面2棱上的点</param>
        /// <param name="ray1">与面1相交的射线</param>
        /// <param name="ray2">与面2相交的射线</param> 
        private List<RayTubeModel> GetDiffrectTubeOnAdjReflectFaces(List<Point> pointsForFace1, List<Point> pointsForFace2, 
            OneRayModel ray1, OneRayModel ray2, int edgeNum11, int edgeNum12, int edgeNum21, int edgeNum22)
        {
            Face face1 = ray1.CrossNode.ReflectionFace, face2 = ray2.CrossNode.ReflectionFace;
            List<RayTubeModel> res = new List<RayTubeModel>();
            int edgeNum = 3 - edgeNum11 - edgeNum12;
            if (pointsForFace1.Count == 2)
            {
                if (ray1.CrossNode.ReflectionFace.Lines[edgeNum11].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[1]))
                    res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[0], pointsForFace1[1], ray1.CrossNode.ReflectionFace.Lines[edgeNum11]));
                if (!face1.Lines[edgeNum11].isSameAdjacent(face2.Lines[edgeNum21]))
                {
                    if (ray2.CrossNode.ReflectionFace.Lines[edgeNum21].IsDiffractionEdge && ray2.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray2.CrossNode.ReflectionFace.Lines[edgeNum21].AdjacentTriangles[0], ray2.CrossNode.ReflectionFace.Lines[edgeNum21].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace2[0], pointsForFace2[1], ray2.CrossNode.ReflectionFace.Lines[edgeNum21]));
                }
            }
            else if (pointsForFace1.Count == 3)
            {
                if (face1.Lines[edgeNum11].isSameAdjacent(face2.Lines[edgeNum21]))
                {
                    if (ray1.CrossNode.ReflectionFace.Lines[edgeNum11].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[0], pointsForFace1[1], ray1.CrossNode.ReflectionFace.Lines[edgeNum11]));
                    if (ray1.CrossNode.ReflectionFace.Lines[edgeNum12].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray1.CrossNode.ReflectionFace.Lines[edgeNum12].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum12].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[1], pointsForFace1[2], ray1.CrossNode.ReflectionFace.Lines[edgeNum12]));
                    if (ray2.CrossNode.ReflectionFace.Lines[edgeNum22].IsDiffractionEdge && ray2.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray2.CrossNode.ReflectionFace.Lines[edgeNum22].AdjacentTriangles[0], ray2.CrossNode.ReflectionFace.Lines[edgeNum22].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[1], pointsForFace1[2], ray2.CrossNode.ReflectionFace.Lines[edgeNum22]));
                }
                else
                {
                    if (ray1.CrossNode.ReflectionFace.Lines[edgeNum11].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[0], pointsForFace1[1], ray1.CrossNode.ReflectionFace.Lines[edgeNum11]));
                    if (ray2.CrossNode.ReflectionFace.Lines[edgeNum21].IsDiffractionEdge && ray2.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray2.CrossNode.ReflectionFace.Lines[edgeNum21].AdjacentTriangles[0], ray2.CrossNode.ReflectionFace.Lines[edgeNum21].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[0], pointsForFace1[1], ray1.CrossNode.ReflectionFace.Lines[edgeNum21]));
                    if (ray1.CrossNode.ReflectionFace.Lines[edgeNum12].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        ray1.CrossNode.ReflectionFace.Lines[edgeNum12].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum12].AdjacentTriangles[1]))
                        res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[1], pointsForFace1[2], ray1.CrossNode.ReflectionFace.Lines[edgeNum12]));
                }
            }
            else if (pointsForFace1.Count == 4)
            {
                if (ray1.CrossNode.ReflectionFace.Lines[edgeNum11].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum11].AdjacentTriangles[1]))
                    res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[0], pointsForFace1[1], ray1.CrossNode.ReflectionFace.Lines[edgeNum11]));
                if (ray2.CrossNode.ReflectionFace.Lines[edgeNum21].IsDiffractionEdge && ray2.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    ray2.CrossNode.ReflectionFace.Lines[edgeNum21].AdjacentTriangles[0], ray2.CrossNode.ReflectionFace.Lines[edgeNum21].AdjacentTriangles[1]))
                    res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[0], pointsForFace1[1], ray1.CrossNode.ReflectionFace.Lines[edgeNum21]));
                if (ray1.CrossNode.ReflectionFace.Lines[edgeNum].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    ray1.CrossNode.ReflectionFace.Lines[edgeNum].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum].AdjacentTriangles[1]))
                    res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[1], pointsForFace1[2], ray1.CrossNode.ReflectionFace.Lines[edgeNum]));
                if (ray1.CrossNode.ReflectionFace.Lines[edgeNum12].IsDiffractionEdge && ray1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    ray1.CrossNode.ReflectionFace.Lines[edgeNum12].AdjacentTriangles[0], ray1.CrossNode.ReflectionFace.Lines[edgeNum12].AdjacentTriangles[1]))
                    res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[2], pointsForFace1[3], ray1.CrossNode.ReflectionFace.Lines[edgeNum12]));
                if (ray2.CrossNode.ReflectionFace.Lines[edgeNum22].IsDiffractionEdge && ray2.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    ray2.CrossNode.ReflectionFace.Lines[edgeNum22].AdjacentTriangles[0], ray2.CrossNode.ReflectionFace.Lines[edgeNum22].AdjacentTriangles[1]))
                    res.AddRange(this.GetDiffractionRayTubeModels(pointsForFace1[2], pointsForFace1[3], ray2.CrossNode.ReflectionFace.Lines[edgeNum22]));
            }
            return res;
        }

        /// <summary>
        ///获取当一条射线打到三角面，其他射线无交点时的下一阶段的射线管模型
        ///对应图2-X
        /// </summary>
        /// <param name="crossRay">有交点的射线</param>
        /// <param name="missRay1">没有交点的射线</param>
        ///  <param name="missRay2">没有交点的射线</param>
        /// <returns>下一阶段的射线管模型</returns>
        private List<RayTubeModel> GetNextRayTubeModelsWhenTFF(OneRayModel crossRay, OneRayModel missRay1, OneRayModel missRay2)
        {
            List<RayTubeModel> nextModels = new List<RayTubeModel>();
            //两条没有交点的射线与有交点射线的交点三角面所在平面的交点
            Point missRay1crossPoint1 = missRay1.LaunchRay.GetCrossPointWithFace(crossRay.CrossNode.ReflectionFace.SwitchToSpaceFace());
            Point missRay2CrossPoint2 = missRay2.LaunchRay.GetCrossPointWithFace(crossRay.CrossNode.ReflectionFace.SwitchToSpaceFace());
            if (missRay1crossPoint1 == null || missRay2CrossPoint2 == null)//射线管跟该平面只有一个交点
            {
                return nextModels;
            }
            //由在三角面内的交点与在三角面外的交点构成两条射线，用来求与棱的交点
            RayInfo ray1FromCrossPointToMissRay1CrossPoint = new RayInfo(crossRay.CrossNode.Position, missRay1crossPoint1);
            RayInfo ray2FromCrossPointToMissRay2CrossPoint = new RayInfo(crossRay.CrossNode.Position, missRay2CrossPoint2);
            //射线管与棱的交点
            int edgeNumber1 = -1, edgeNumber2 = -1;
            Point crossPoint3OfRay1AndEdge = this.GetCrossPointAndAssignEdgeNumber(crossRay.CrossNode.ReflectionFace.Lines, ray1FromCrossPointToMissRay1CrossPoint, ref edgeNumber1);
            Point crossPoint4OfRay2AndEdge = this.GetCrossPointAndAssignEdgeNumber(crossRay.CrossNode.ReflectionFace.Lines, ray2FromCrossPointToMissRay2CrossPoint, ref edgeNumber2);
            if (crossPoint3OfRay1AndEdge != null && crossPoint4OfRay2AndEdge != null)//交点不为0
            {
                //Point nextMirrorLuanchPoint = this.launchPoints[0].GetMirrorPoint(crossRay.CrossNode.ReflectionFace);
                if (edgeNumber1 == edgeNumber2)//两个交点在一条棱上
                {
                    //求反射模型
                    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                        new List<Point> { crossPoint3OfRay1AndEdge, crossPoint4OfRay2AndEdge }, crossRay.CrossNode.ReflectionFace,
                        new List<OneRayModel> { crossRay }));
                    if (this.DiffractionTracingTimes == 0)
                    {
                        //发生绕射
                        if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].IsDiffractionEdge &&
                            crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                            crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[1]))
                        {
                            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint3OfRay1AndEdge, crossPoint4OfRay2AndEdge, crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1]));
                        }
                    }
                }
                else//两个交点不在一条棱上
                {
                    List<Point> edgeCommonPoints = crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].GetCommonPointsWithOtherLine(crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2]);
                    if (edgeCommonPoints.Count == 1)
                    {
                        //对应图2-2
                        if (new Triangle(crossRay.CrossNode.Position, missRay1crossPoint1, missRay2CrossPoint2).JudgeIfPointInFace(edgeCommonPoints[0]))
                        {
                            //求反射模型
                            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                            new List<Point> { crossPoint3OfRay1AndEdge, edgeCommonPoints[0], crossPoint4OfRay2AndEdge }, crossRay.CrossNode.ReflectionFace,
                            new List<OneRayModel> { crossRay }));
                            if (this.DiffractionTracingTimes == 0)
                            {
                                //发射绕射
                                if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].IsDiffractionEdge &&
                                    crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                                    crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[1]))
                                {
                                    nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint3OfRay1AndEdge, edgeCommonPoints[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1]));
                                }
                                if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].IsDiffractionEdge &&
                                  crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                                  crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[1]))
                                {
                                    nextModels.AddRange(this.GetDiffractionRayTubeModels(edgeCommonPoints[0], crossPoint4OfRay2AndEdge, crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2]));
                                }
                            }
                        }
                        else//对应图2-3,2-4
                        {
                            nextModels.AddRange(this.GetNextRayTubeModelsWhenTFF_Child(crossRay, missRay1, missRay2,
                                missRay1crossPoint1, missRay2CrossPoint2, crossPoint3OfRay1AndEdge, crossPoint4OfRay2AndEdge, edgeNumber1, edgeNumber2, edgeCommonPoints[0]));
                        }

                    }
                    else
                    {
                        LogFileManager.ObjLog.error("当一条射线打到三角面，其他射线无交点时,求两条棱的公共点时出错");
                    }
                }
            }
            else
            {
                LogFileManager.ObjLog.error("当一条射线打到三角面，其他射线无交点时,求射线管与棱交点时出错");
            }

            return nextModels;
        }

        /// <summary>
        ///获取当一条射线打到三角面，其他射线无交点时的下一阶段的射线管模型
        ///对应图2-3和2-4
        /// </summary>
        /// <param name="crossRay">有交点的射线</param>
        /// <param name="missRay1">没有交点的射线</param>
        ///  <param name="missRay2">没有交点的射线</param>
        /// <returns>下一阶段的射线管模型</returns>
        private List<RayTubeModel> GetNextRayTubeModelsWhenTFF_Child(OneRayModel crossRay, OneRayModel missRay1, OneRayModel missRay2,
            Point missRay1crossPoint1, Point missRay2CrossPoint2, Point crossPoint3OfRay1AndEdge, Point crossPoint4OfRay2AndEdge, int edgeNumber1, int edgeNumber2, Point edgeCommonPoint)
        {
            List<RayTubeModel> nextModels = new List<RayTubeModel>();
            Point edge1OtherPoint = (edgeCommonPoint == crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].StartPoint) ?
                               crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].EndPoint : crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].StartPoint;
            Point edge2OtherPoint = (edgeCommonPoint == crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].StartPoint) ?
              crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].EndPoint : crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].StartPoint;
            if (new Triangle(crossRay.CrossNode.Position, missRay1crossPoint1, missRay2CrossPoint2).JudgeIfPointInFace(edge1OtherPoint))
            {
                //对应图2-4
                //求反射模型
                nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                new List<Point> { crossPoint3OfRay1AndEdge, edge1OtherPoint, edge2OtherPoint, crossPoint4OfRay2AndEdge }, crossRay.CrossNode.ReflectionFace,
                new List<OneRayModel> { crossRay }));
                if (this.DiffractionTracingTimes == 0)
                {
                    //发射绕射
                    if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].IsDiffractionEdge &&
                        crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                        crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[1]))
                    {
                        nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint3OfRay1AndEdge, edge1OtherPoint, crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1]));
                    }
                    if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].IsDiffractionEdge &&
                      crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                      crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[1]))
                    {
                        nextModels.AddRange(this.GetDiffractionRayTubeModels(edge2OtherPoint, crossPoint4OfRay2AndEdge, crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2]));
                    }
                    if (crossRay.CrossNode.ReflectionFace.Lines[3 - edgeNumber2 - edgeNumber1].IsDiffractionEdge &&
                    crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                    crossRay.CrossNode.ReflectionFace.Lines[3 - edgeNumber2 - edgeNumber1].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[3 - edgeNumber2 - edgeNumber1].AdjacentTriangles[1]))
                    {
                        nextModels.AddRange(this.GetDiffractionRayTubeModels(edge1OtherPoint, edge2OtherPoint, crossRay.CrossNode.ReflectionFace.Lines[3 - edgeNumber2 - edgeNumber1]));
                    }
                }
            }
            else//对应图2-3
            {

                RayInfo rayFromMissCrossPoint1To2 = new RayInfo(missRay1crossPoint1, missRay2CrossPoint2);
                Point crossPoint3 = rayFromMissCrossPoint1To2.GetCrossPointWithOtherRay(crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].SwitchToRay());
                Point crossPoint4 = rayFromMissCrossPoint1To2.GetCrossPointWithOtherRay(crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].SwitchToRay());
                if (crossPoint3 != null && crossPoint4 != null)
                {
                    //求反射模型
                    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                    new List<Point> { crossPoint3OfRay1AndEdge, crossPoint3, crossPoint4, crossPoint4OfRay2AndEdge }, crossRay.CrossNode.ReflectionFace,
                    new List<OneRayModel> { crossRay }));
                    if (this.DiffractionTracingTimes == 0)
                    {
                        //发射绕射
                        if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].IsDiffractionEdge &&
                            crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                            crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[1]))
                        {
                            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint3OfRay1AndEdge, crossPoint3, crossRay.CrossNode.ReflectionFace.Lines[edgeNumber1]));
                        }
                        if (crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].IsDiffractionEdge &&
                          crossRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                          crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[0], crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[1]))
                        {
                            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint4, crossPoint4OfRay2AndEdge, crossRay.CrossNode.ReflectionFace.Lines[edgeNumber2]));
                        }
                    }
                }
                else
                {
                    LogFileManager.ObjLog.error("求图2-3时出错");
                }
            }
            return nextModels;
        }


        /// <summary>
        ///获取当两条射线打到一个三角面，一条射线无交点时的下一阶段的射线管模型
        ///对应图3-1和3-2
        /// </summary>
        /// <param name="crossRay1">有交点的射线</param>
        /// <param name="crossRay2">有交点的射线</param>
        ///  <param name="missRay">没有交点的射线</param>
        /// <returns>下一阶段的射线管模型</returns>
        private List<RayTubeModel> GetNextRayTubeModelsWhenTaTaF(OneRayModel crossRay1, OneRayModel crossRay2, OneRayModel missRay)
        {
            List<RayTubeModel> nextModels = new List<RayTubeModel>();
            //求两有交点的射线与有交点射线的交点三角面所在平面的交点
            Point missCrossPoint1 = missRay.LaunchRay.GetCrossPointWithFace(crossRay1.CrossNode.ReflectionFace.SwitchToSpaceFace());
            if (missCrossPoint1 == null)//说明没有交点的射线与该三角面不相交
            {
                LogFileManager.ObjLog.error("求当两条射线打到三角面，一条射线无交点时求没有交点的射线与平面交点时出错");
                return nextModels;
            }
            //由有交点的射线与没有交点的射线构成的一个平面，用来求与绕射棱的交点
            RayInfo ray1FromCrossPoint1ToMissCrossPoint = new RayInfo(crossRay1.CrossNode.Position, missCrossPoint1);
            RayInfo ray2FromCrossPoint2ToMissCrossPoint = new RayInfo(crossRay2.CrossNode.Position, missCrossPoint1);
            //射线管与棱的交点
            int edgeNumber1 = -1, edgeNumber2 = -1;
            Point crossPoint2OfRay1AndEdge = this.GetCrossPointAndAssignEdgeNumber(crossRay1.CrossNode.ReflectionFace.Lines, ray1FromCrossPoint1ToMissCrossPoint, ref edgeNumber1);
            Point crossPoint3OfRay2AndEdge = this.GetCrossPointAndAssignEdgeNumber(crossRay1.CrossNode.ReflectionFace.Lines, ray2FromCrossPoint2ToMissCrossPoint, ref edgeNumber2);
            if (crossPoint2OfRay1AndEdge != null && crossPoint3OfRay2AndEdge != null)//交点不为0
            {
                if (edgeNumber1 == edgeNumber2)//两个交点在一条棱上
                {
                    //求反射模型
                    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                        new List<Point> { crossPoint2OfRay1AndEdge, crossPoint3OfRay2AndEdge }, crossRay1.CrossNode.ReflectionFace,
                        new List<OneRayModel> { crossRay1, crossRay2 }));
                    if (this.DiffractionTracingTimes == 0)
                    {
                        //发生绕射
                        if (crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].IsDiffractionEdge &&
                            crossRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                            crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[0], crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[1]))
                        {
                            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint2OfRay1AndEdge, crossPoint3OfRay2AndEdge, crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1]));
                        }
                    }
                }
                else//两个交点不在一条棱上
                {
                    List<Point> edgeCommonPoints = crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].GetCommonPointsWithOtherLine(crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber2]);
                    if (edgeCommonPoints.Count == 1)
                    {
                        //求反射模型
                        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
                            new List<Point> { crossPoint2OfRay1AndEdge, edgeCommonPoints[0], crossPoint3OfRay2AndEdge }, crossRay1.CrossNode.ReflectionFace,
                            new List<OneRayModel> { crossRay1, crossRay2 }));
                        if (this.DiffractionTracingTimes == 0)
                        {
                            //发射绕射
                            if (crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].IsDiffractionEdge &&
                                crossRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                                crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[0], crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1].AdjacentTriangles[1]))
                            {
                                nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint2OfRay1AndEdge, edgeCommonPoints[0], crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber1]));
                            }
                            if (crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber2].IsDiffractionEdge &&
                            crossRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(
                            crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[0], crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber2].AdjacentTriangles[1]))
                            {
                                nextModels.AddRange(this.GetDiffractionRayTubeModels(edgeCommonPoints[0], crossPoint3OfRay2AndEdge, crossRay1.CrossNode.ReflectionFace.Lines[edgeNumber2]));
                            }
                        }
                    }
                    else
                    {
                        LogFileManager.ObjLog.error("当一条射线打到三角面，其他射线无交点时,求两条棱的公共点时出错");
                    }
                }
            }
            else
            {
                LogFileManager.ObjLog.error("当一条射线打到三角面，其他射线无交点时,求射线管与棱交点时出错");
            }

            return nextModels;
        }

        ///// <summary>
        /////获取当三条射线的两个交点在一个三角面，另一个交点在相邻三角面时的下一阶段的射线管模型
        /////对应图4-2
        ///// </summary>
        ///// <param name="sameFaceRay1">交点在一个三角面的射线</param>
        ///// <param name="sameFaceRay2">交点在一个三角面的射线</param>
        /////  <param name="otherFaceRay">交点在另一个三角面的射线</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsWhenTaTaTb_A(OneRayModel sameFaceRay1, OneRayModel sameFaceRay2, OneRayModel otherFaceRay)
        //{
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    AdjacentEdge diffractionEdge = new AdjacentEdge(sameFaceRay1.CrossNode.ReflectionFace, otherFaceRay.CrossNode.ReflectionFace);//求三个面的公共棱
        //    if (diffractionEdge.EndPoint == null)//存在不公共棱，说明两个平面不相邻
        //    {
        //        LogFileManager.ObjLog.error("球当三条射线的两个交点在一个三角面，另一个交点在相邻三角面时的公共棱出错");
        //        return new List<RayTubeModel>();
        //    }
        //    //由有交点的射线与没有交点的射线构成的一个平面，用来求与绕射棱的交点
        //    SpaceFace firstRayFace = new SpaceFace(sameFaceRay1.LaunchNode.Position, sameFaceRay1.CrossNode.Position, otherFaceRay.CrossNode.Position);
        //    SpaceFace secondRayFace = new SpaceFace(sameFaceRay2.LaunchNode.Position, sameFaceRay2.CrossNode.Position, otherFaceRay.CrossNode.Position);
        //    //求绕射棱与射线面的交点，得到交点
        //    Point crossPoint1 = diffractionEdge.GetCrossPointWithFace(firstRayFace);
        //    Point crossPoint2 = diffractionEdge.GetCrossPointWithFace(secondRayFace);
        //    if (crossPoint1 != null && crossPoint2 != null)//存在交点，生成绕射射线管模型和反射模型
        //    {

        //        //若两个三角面相对观察点的形状是凸的
        //        if (sameFaceRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(diffractionEdge.AdjacentTriangles[0], diffractionEdge.AdjacentTriangles[1]))
        //        {
        //            //求反射射线管模型,虽然点是一样，但是所在面会导致反射射线不一样，所以要设置不同的射线模型
        //            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //                new List<Point> { crossPoint1, crossPoint2 }, sameFaceRay1.CrossNode.ReflectionFace,
        //                new List<OneRayModel> { sameFaceRay1, sameFaceRay2 }));
        //            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //                new List<Point> { crossPoint1, crossPoint2 }, otherFaceRay.CrossNode.ReflectionFace,
        //                new List<OneRayModel> { otherFaceRay }));
        //            if (this.DiffractionTracingTimes == 0)
        //            {
        //                //求绕射模型
        //                nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint1, crossPoint2, diffractionEdge));
        //            }
        //        }
        //        else//若两个三角面是凹的,先细分
        //        {
        //            //求反射射线管模型,虽然点是一样，但是所在面会导致反射射线不一样，所以要设置不同的射线模型
        //            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //                new List<Point> { crossPoint1, crossPoint2 }, sameFaceRay1.CrossNode.ReflectionFace,
        //                new List<OneRayModel> { sameFaceRay1, sameFaceRay2 }));
        //            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //                new List<Point> { crossPoint1, crossPoint2 }, otherFaceRay.CrossNode.ReflectionFace,
        //                new List<OneRayModel> { otherFaceRay }));
        //        }
        //    }
        //    else
        //    {
        //        LogFileManager.ObjLog.error("获取当三条射线的两个交点在一个三角面，另一个交点在相邻三角面时公共棱上的交点出错");
        //        return new List<RayTubeModel>();
        //    }
        //    return nextModels;
        //}

        ///// <summary>
        /////获取当三条射线的两个交点在一个三角面，另一个交点在与前一个三角面有一个公共顶点的三角面时的下一阶段的射线管模型
        /////对应图4-3、4-4
        ///// </summary>
        ///// <param name="sameFaceRay1">交点在一个三角面的射线,其射线，交点，交点所在三角面都简称1</param>
        ///// <param name="sameFaceRay2">交点在一个三角面的射线,其射线，交点，交点所在三角面都简称2</param>
        /////  <param name="otherFaceRay">交点在另一个三角面的射线,其射线，交点，交点所在三角面都简称3</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsWhenTaTaTb_C(OneRayModel sameFaceRay1, OneRayModel sameFaceRay2, OneRayModel otherFaceRay)
        //{
        //    //求交点三角面的公共顶点
        //    List<Point> commonPoints = sameFaceRay1.CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(otherFaceRay.CrossNode.ReflectionFace);
        //    if (commonPoints.Count != 1)
        //    {
        //        LogFileManager.ObjLog.error("当三条射线的两个交点在一个三角面，另一个交点在与前一个三角面有一个公共顶点的三角面时,求公共顶点时出错");
        //        return new List<RayTubeModel>();
        //    }
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    //由有交点的射线与没有交点的射线构成的一个平面，用来求与绕射棱的交点
        //    SpaceFace rayFace31 = new SpaceFace(sameFaceRay1.LaunchNode.Position, sameFaceRay1.CrossNode.Position, otherFaceRay.CrossNode.Position);
        //    SpaceFace rayFace32 = new SpaceFace(sameFaceRay2.LaunchNode.Position, sameFaceRay2.CrossNode.Position, otherFaceRay.CrossNode.Position);
        //    //交点组成的棱经过的三角面
        //    List<Face> passingFacesFromCrossNode3To1 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(otherFaceRay.CrossNode, sameFaceRay1.CrossNode, commonPoints[0]);
        //    List<Face> passingFacesFromCrossNode3To2 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(otherFaceRay.CrossNode, sameFaceRay2.CrossNode, commonPoints[0]);
        //    //线段经过的三角面中的公共棱，棱与射线面的交点
        //    List<AdjacentEdge> sameLinesOfPassingFaces31 = new List<AdjacentEdge>(), sameLinesOfPassingFaces32 = new List<AdjacentEdge>();
        //    List<Point> crossPointsOfPassingLines31AndRayFace31 = new List<Point>(), crossPointsOfPassingLines32AndRayFace32 = new List<Point>();
        //    //求线段经过的三角面中的公共棱，棱与射线面的交点
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode3To1, rayFace31, sameLinesOfPassingFaces31, crossPointsOfPassingLines31AndRayFace31);
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode3To2, rayFace32, sameLinesOfPassingFaces32, crossPointsOfPassingLines32AndRayFace32);
        //    //判断求得的值是否正确
        //    if (crossPointsOfPassingLines31AndRayFace31.Count < 1 || crossPointsOfPassingLines32AndRayFace32.Count < 1 ||
        //        crossPointsOfPassingLines31AndRayFace31.Count != sameLinesOfPassingFaces31.Count || crossPointsOfPassingLines32AndRayFace32.Count != sameLinesOfPassingFaces32.Count)
        //    {
        //        LogFileManager.ObjLog.error("当三条射线的两个交点在一个三角面，另一个交点在与前一个三角面有一个公共顶点的三角面时，求棱31,32经过的三角面与棱的交点时出错");
        //        return new List<RayTubeModel>();
        //    }
        //    //交点三角面的公共顶点在XY平面的投影是否在交点构成的三角面投影内
        //    if (new Triangle(sameFaceRay1.CrossNode.Position, sameFaceRay2.CrossNode.Position, otherFaceRay.CrossNode.Position).JudgeIfPointInTriangleInXY(commonPoints[0]))
        //    {
        //        //对应图4-3
        //        //交点1，2所在三角面的反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //            new List<Point> { crossPointsOfPassingLines31AndRayFace31[crossPointsOfPassingLines31AndRayFace31.Count - 1], commonPoints[0], 
        //                crossPointsOfPassingLines32AndRayFace32[crossPointsOfPassingLines32AndRayFace32.Count-1] },
        //            sameFaceRay1.CrossNode.ReflectionFace, new List<OneRayModel> { sameFaceRay1, sameFaceRay2 }));
        //        //交点3所在三角面的反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //          new List<Point> { crossPointsOfPassingLines31AndRayFace31[0], commonPoints[0], crossPointsOfPassingLines32AndRayFace32[0] },
        //          otherFaceRay.CrossNode.ReflectionFace, new List<OneRayModel> { otherFaceRay }));
        //        //从交点3到交点1经过的不包括三角面1，3的反射模型
        //        nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode3To1, crossPointsOfPassingLines31AndRayFace31, commonPoints[0]));
        //        //从交点3到交点2经过的不包括三角面1，3的反射模型
        //        nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode3To2, crossPointsOfPassingLines32AndRayFace32, commonPoints[0]));
        //        if (this.DiffractionTracingTimes == 0)
        //        {
        //            //线段31经过的三角面中的绕射模型
        //            nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces31, crossPointsOfPassingLines31AndRayFace31, otherFaceRay, commonPoints[0]));
        //            //线段32经过的三角面中的绕射模型
        //            nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces32, crossPointsOfPassingLines32AndRayFace32, otherFaceRay, commonPoints[0]));
        //        }
        //    }
        //    else //当交点三角面的公共顶点的投影不在交点组成的三角面内时
        //    {
        //        //对应图4-4
        //        //交点1，2所在三角面的反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //            new List<Point> { crossPointsOfPassingLines31AndRayFace31[crossPointsOfPassingLines31AndRayFace31.Count - 1],
        //                crossPointsOfPassingLines32AndRayFace32[crossPointsOfPassingLines32AndRayFace32.Count-1] },
        //            sameFaceRay1.CrossNode.ReflectionFace, new List<OneRayModel> { sameFaceRay1, sameFaceRay2 }));
        //        //交点3所在三角面的反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //          new List<Point> { crossPointsOfPassingLines31AndRayFace31[0], crossPointsOfPassingLines32AndRayFace32[0] },
        //          otherFaceRay.CrossNode.ReflectionFace, new List<OneRayModel> { otherFaceRay }));
        //        if (crossPointsOfPassingLines31AndRayFace31.Count != crossPointsOfPassingLines32AndRayFace32.Count)
        //        {
        //            return nextModels;
        //        }
        //        //从交点3到交点1经过的不包括三角面3，1的反射模型
        //        for (int i = 0; i < crossPointsOfPassingLines31AndRayFace31.Count - 1; i++)
        //        {
        //            nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //           new List<Point> { crossPointsOfPassingLines31AndRayFace31[i],crossPointsOfPassingLines31AndRayFace31[i+1],
        //               crossPointsOfPassingLines32AndRayFace32[i+1],crossPointsOfPassingLines32AndRayFace32[i] },
        //           passingFacesFromCrossNode3To1[i + 1], new List<OneRayModel>()));
        //        }
        //        if (this.DiffractionTracingTimes == 0)
        //        {
        //            //线段31经过的三角面中的绕射模型
        //            for (int m = 0; m < sameLinesOfPassingFaces31.Count; m++)
        //            {
        //                if (otherFaceRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLinesOfPassingFaces31[m].AdjacentTriangles[0], sameLinesOfPassingFaces31[m].AdjacentTriangles[1]))
        //                {
        //                    nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPointsOfPassingLines31AndRayFace31[m],
        //                        crossPointsOfPassingLines32AndRayFace32[m], sameLinesOfPassingFaces31[m]));
        //                }
        //            }
        //        }

        //    }
        //    return nextModels;
        //}

        ///// <summary>
        /////获取当ray1,ray2,ray3的交点所在的三个平面都有一个公共的点时的下一阶段射线管模型
        /////对应文档3-(4)-d的情况
        ///// </summary>
        ///// <param name="launchRay1">射线1,其交点为1,交点所在三角面也为1</param>
        ///// <param name="launchRay2">射线2,其交点位2,交点所在三角面也为2</param>
        ///// <param name="launchRay3">射线3,其交点为3,交点所在三角面也为3</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsWhenTaTbTc_C(OneRayModel launchRay1, OneRayModel launchRay2, OneRayModel launchRay3)
        //{
        //    //求交点三角面的公共顶点
        //    List<Point> commonPoints = launchRay1.CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(launchRay3.CrossNode.ReflectionFace);
        //    if (commonPoints.Count == 2)
        //    { commonPoints = launchRay2.CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(launchRay3.CrossNode.ReflectionFace); }
        //    if (commonPoints.Count == 2)
        //    { commonPoints = launchRay1.CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(launchRay2.CrossNode.ReflectionFace); }
        //    if (commonPoints.Count != 1)
        //    {
        //        LogFileManager.ObjLog.error("ray1,ray2,ray3的交点所在平面不相邻但是有一个公共的点时,求公共顶点时出错");
        //        return new List<RayTubeModel>();
        //    }
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    //公共顶点的投影在交点1和交点2构成的线段的投影内
        //    if (new LineSegment(launchRay1.CrossNode.Position, launchRay2.CrossNode.Position).JudgeIfPointInLineRangeInXYPlane(commonPoints[0]))
        //    {
        //        nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartA(
        //            launchRay3, launchRay1, launchRay2, commonPoints[0]));
        //    }
        //    //公共顶点的投影在交点1和交点3构成的线段的投影内
        //    else if (new LineSegment(launchRay1.CrossNode.Position, launchRay3.CrossNode.Position).JudgeIfPointInLineRangeInXYPlane(commonPoints[0]))
        //    {
        //        nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartA(
        //            launchRay2, launchRay1, launchRay3, commonPoints[0]));
        //    }
        //    //公共顶点的投影在交点2和交点3构成的线段的投影内
        //    else if (new LineSegment(launchRay2.CrossNode.Position, launchRay3.CrossNode.Position).JudgeIfPointInLineRangeInXYPlane(commonPoints[0]))
        //    {
        //        nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartA(
        //            launchRay1, launchRay2, launchRay3, commonPoints[0]));
        //    }
        //    else //公共顶点的投影不在交点线段的投影内
        //    {
        //        nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartB(launchRay1, launchRay2, launchRay3, commonPoints[0]));

        //    }

        //    return nextModels;
        //}

        ///// <summary>
        /////获取当交点所在三角面的公共点的XY投影在交点2,3构成的线段的投影上时的下一阶段射线管模型
        /////对应文档图4-6
        ///// </summary>
        ///// <param name="launchRay1">射线1,其交点为1,交点所在三角面也为1</param>
        ///// <param name="launchRay2">射线2,其交点位2,交点所在三角面也为2</param>
        ///// <param name="launchRay3">射线3,其交点为3,交点所在三角面也为3</param>
        ///// <param name="commonVertex">交点所在三角面的公共顶点</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsInTaTbTc_C_PartA(OneRayModel launchRay1, OneRayModel launchRay2, OneRayModel launchRay3, Point commonVertex)
        //{
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    //有两条射线构成的射线面
        //    SpaceFace rayFace12 = new SpaceFace(launchRay1.LaunchNode.Position, launchRay1.CrossNode.Position, launchRay2.CrossNode.Position);
        //    SpaceFace rayFace13 = new SpaceFace(launchRay1.LaunchNode.Position, launchRay1.CrossNode.Position, launchRay3.CrossNode.Position);
        //    //交点12,13线段经过的三角面
        //    List<Face> passingFacesFromCrossNode1To2 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(launchRay1.CrossNode, launchRay2.CrossNode, commonVertex);
        //    List<Face> passingFacesFromCrossNode1To3 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(launchRay1.CrossNode, launchRay3.CrossNode, commonVertex);
        //    //线段经过的三角面中的公共棱，棱与射线面的交点
        //    List<AdjacentEdge> sameLinesOfPassingFaces12 = new List<AdjacentEdge>(), sameLinesOfPassingFaces13 = new List<AdjacentEdge>();
        //    List<Point> crossPointsOfPassingLines12AndRayFace12 = new List<Point>(), crossPointsOfPassingLines13AndRayFace13 = new List<Point>();
        //    //求线段经过的三角面中的公共棱，棱与射线面的交点
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode1To2, rayFace12, sameLinesOfPassingFaces12, crossPointsOfPassingLines12AndRayFace12);
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode1To3, rayFace13, sameLinesOfPassingFaces13, crossPointsOfPassingLines13AndRayFace13);
        //    //判断求得的值是否正确
        //    if (crossPointsOfPassingLines12AndRayFace12.Count < 1 || crossPointsOfPassingLines13AndRayFace13.Count < 1
        //        || crossPointsOfPassingLines12AndRayFace12.Count != sameLinesOfPassingFaces12.Count || crossPointsOfPassingLines13AndRayFace13.Count != sameLinesOfPassingFaces13.Count)
        //    {
        //        LogFileManager.ObjLog.error("当射线交点三角面的公共端点在射线2,3的交点组成的棱时，求棱12,13经过的三角面与棱的交点时出错");
        //        return nextModels;
        //    }
        //    //构造反射射线管模型
        //    //三角面的1反射模型
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //        new List<Point> { crossPointsOfPassingLines12AndRayFace12[0], commonVertex, crossPointsOfPassingLines13AndRayFace13[0] },
        //        launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay1 }));
        //    //三角面2的反射模型
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //      new List<Point> { crossPointsOfPassingLines12AndRayFace12[crossPointsOfPassingLines12AndRayFace12.Count - 1], commonVertex },
        //      launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay2 }));
        //    //三角面3的反射模型
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //      new List<Point> { commonVertex, crossPointsOfPassingLines13AndRayFace13[crossPointsOfPassingLines13AndRayFace13.Count - 1] },
        //      launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay3 }));
        //    //从交点1到交点2经过的不包括三角面1，2的反射模型
        //    nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode1To2, crossPointsOfPassingLines12AndRayFace12, commonVertex));
        //    //从交点1到交点3经过的不包括三角面1，2的反射模型
        //    nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode1To3, crossPointsOfPassingLines13AndRayFace13, commonVertex));
        //    if (this.DiffractionTracingTimes == 0)
        //    {
        //        //线段12经过的三角面中的绕射模型
        //        nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces12, crossPointsOfPassingLines12AndRayFace12, launchRay1, commonVertex));
        //        //线段13经过的三角面中的绕射模型
        //        nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces13, crossPointsOfPassingLines13AndRayFace13, launchRay1, commonVertex));
        //    }

        //    return nextModels;
        //}


        ///// <summary>
        /////获取当交点三角面的公共点不在交点构成的线段的XY平面投影上时的下一阶段射线管模型
        /////对应文档图4-5，和图4-7
        ///// </summary>
        ///// <param name="launchRay1">射线1,其交点为1,交点所在三角面也为1</param>
        ///// <param name="launchRay2">射线2,其交点位2,交点所在三角面也为2</param>
        ///// <param name="launchRay3">射线3,其交点为3,交点所在三角面也为3</param>
        /////  <param name="commonVertex">交点所在三角面的公共顶点</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsInTaTbTc_C_PartB(OneRayModel launchRay1, OneRayModel launchRay2, OneRayModel launchRay3, Point commonVertex)
        //{
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    //射线面
        //    SpaceFace rayFace12 = new SpaceFace(launchRay1.LaunchNode.Position, launchRay1.CrossNode.Position, launchRay2.CrossNode.Position);
        //    SpaceFace rayFace13 = new SpaceFace(launchRay1.LaunchNode.Position, launchRay1.CrossNode.Position, launchRay3.CrossNode.Position);
        //    SpaceFace rayFace23 = new SpaceFace(launchRay2.LaunchNode.Position, launchRay2.CrossNode.Position, launchRay3.CrossNode.Position);
        //    //交点组成的棱经过的三角面
        //    List<Face> passingFacesFromCrossNode1To2 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(launchRay1.CrossNode, launchRay2.CrossNode, commonVertex);
        //    List<Face> passingFacesFromCrossNode1To3 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(launchRay1.CrossNode, launchRay3.CrossNode, commonVertex);
        //    List<Face> passingFacesFromCrossNode2To3 = this.GetFacesFromBeginCrossNodeToTargetCrossNode(launchRay2.CrossNode, launchRay3.CrossNode, commonVertex);
        //    //线段经过的三角面中的公共棱，棱与射线面的交点
        //    List<AdjacentEdge> sameLinesOfPassingFaces12 = new List<AdjacentEdge>(),
        //        sameLinesOfPassingFaces13 = new List<AdjacentEdge>(), sameLinesOfPassingFaces23 = new List<AdjacentEdge>();
        //    List<Point> crossPointsOfPassingLines12AndRayFace12 = new List<Point>(),
        //        crossPointsOfPassingLines13AndRayFace13 = new List<Point>(), crossPointsOfPassingLines23AndRayFace23 = new List<Point>();
        //    //求线段经过的三角面中的公共棱，棱与射线面的交点
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode1To2, rayFace12, sameLinesOfPassingFaces12, crossPointsOfPassingLines12AndRayFace12);
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode1To3, rayFace13, sameLinesOfPassingFaces13, crossPointsOfPassingLines13AndRayFace13);
        //    this.AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(passingFacesFromCrossNode2To3, rayFace23, sameLinesOfPassingFaces23, crossPointsOfPassingLines23AndRayFace23);
        //    //判断求得的值是否正确
        //    if (crossPointsOfPassingLines12AndRayFace12.Count < 1 || crossPointsOfPassingLines13AndRayFace13.Count < 1 || crossPointsOfPassingLines23AndRayFace23.Count < 1
        //        || crossPointsOfPassingLines12AndRayFace12.Count != sameLinesOfPassingFaces12.Count || crossPointsOfPassingLines13AndRayFace13.Count != sameLinesOfPassingFaces13.Count
        //        || crossPointsOfPassingLines23AndRayFace23.Count != sameLinesOfPassingFaces23.Count)
        //    {
        //        LogFileManager.ObjLog.error("当射线交点三角面的公共端点在射线2,3的交点组成的棱时，求棱12,13经过的三角面与棱的交点时出错");
        //        return new List<RayTubeModel>();
        //    }
        //    //交点三角面的公共顶点在XY平面的投影是否在交点构成的三角面投影内
        //    if (new Triangle(launchRay1.CrossNode.Position, launchRay2.CrossNode.Position, launchRay3.CrossNode.Position).JudgeIfPointInTriangleInXY(commonVertex))
        //    {
        //        //对应图4-5
        //        //三角面的1反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //            new List<Point> { crossPointsOfPassingLines12AndRayFace12[0], commonVertex, crossPointsOfPassingLines13AndRayFace13[0] },
        //            launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay1 }));
        //        //三角面2的反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //          new List<Point> { crossPointsOfPassingLines12AndRayFace12[crossPointsOfPassingLines12AndRayFace12.Count - 1], commonVertex, crossPointsOfPassingLines23AndRayFace23[0] },
        //          launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay2 }));
        //        //三角面3的反射模型
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //          new List<Point> { crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines23AndRayFace23.Count - 1], commonVertex, crossPointsOfPassingLines13AndRayFace13[crossPointsOfPassingLines13AndRayFace13.Count - 1] },
        //          launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay3 }));
        //        //从交点1到交点2经过的不包括三角面1，2的反射模型
        //        nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode1To2, crossPointsOfPassingLines12AndRayFace12, commonVertex));
        //        //从交点1到交点3经过的不包括三角面1，2的反射模型
        //        nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode1To3, crossPointsOfPassingLines13AndRayFace13, commonVertex));
        //        //从交点2到交点3经过的不包括三角面1，2的反射模型
        //        nextModels.AddRange(this.GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(passingFacesFromCrossNode2To3, crossPointsOfPassingLines23AndRayFace23, commonVertex));
        //        if (this.DiffractionTracingTimes == 0)
        //        {
        //            //线段12经过的三角面中的绕射模型
        //            nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces12, crossPointsOfPassingLines12AndRayFace12, launchRay1, commonVertex));
        //            //线段13经过的三角面中的绕射模型
        //            nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces13, crossPointsOfPassingLines13AndRayFace13, launchRay1, commonVertex));
        //            //线段23经过的三角面中的绕射模型
        //            nextModels.AddRange(this.GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(sameLinesOfPassingFaces23, crossPointsOfPassingLines23AndRayFace23, launchRay1, commonVertex));
        //        }
        //    }
        //    else //当交点三角面的公共顶点的投影不在交点组成的三角面内时,对应图4-7
        //    {
        //        //当棱23经过的三角面的公共棱与射线面的交点最多时
        //        if (crossPointsOfPassingLines23AndRayFace23.Count > crossPointsOfPassingLines12AndRayFace12.Count &&
        //            crossPointsOfPassingLines23AndRayFace23.Count > crossPointsOfPassingLines13AndRayFace13.Count)
        //        {
        //            nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartB_Child(
        //                launchRay1, launchRay2, launchRay3, passingFacesFromCrossNode1To2, passingFacesFromCrossNode1To3,
        //                sameLinesOfPassingFaces12, sameLinesOfPassingFaces13, crossPointsOfPassingLines12AndRayFace12,
        //                crossPointsOfPassingLines13AndRayFace13, crossPointsOfPassingLines23AndRayFace23));
        //        }
        //        //当棱13经过的三角面的公共棱与射线面的交点最多时，以交点2作为基准点
        //        else if (crossPointsOfPassingLines13AndRayFace13.Count > crossPointsOfPassingLines12AndRayFace12.Count &&
        //            crossPointsOfPassingLines13AndRayFace13.Count > crossPointsOfPassingLines23AndRayFace23.Count)
        //        {
        //            //由于之前所求的点，线，面都是从交点1到交点2，所以要反转
        //            passingFacesFromCrossNode1To2.Reverse();
        //            sameLinesOfPassingFaces12.Reverse();
        //            crossPointsOfPassingLines12AndRayFace12.Reverse();
        //            nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartB_Child(
        //              launchRay2, launchRay1, launchRay3, passingFacesFromCrossNode1To2, passingFacesFromCrossNode2To3,
        //              sameLinesOfPassingFaces12, sameLinesOfPassingFaces23, crossPointsOfPassingLines12AndRayFace12,
        //              crossPointsOfPassingLines23AndRayFace23, crossPointsOfPassingLines13AndRayFace13));
        //        }
        //        //当棱12经过的三角面的公共棱与射线面的交点最多时，以交点3作为基准点
        //        else if (crossPointsOfPassingLines12AndRayFace12.Count > crossPointsOfPassingLines13AndRayFace13.Count &&
        //            crossPointsOfPassingLines12AndRayFace12.Count > crossPointsOfPassingLines23AndRayFace23.Count)
        //        {
        //            //由于之前所求的点，线，面都是从交点1到交点3，交点2到交点3，所以要反转
        //            passingFacesFromCrossNode1To3.Reverse();
        //            passingFacesFromCrossNode2To3.Reverse();
        //            sameLinesOfPassingFaces13.Reverse();
        //            sameLinesOfPassingFaces23.Reverse();
        //            crossPointsOfPassingLines13AndRayFace13.Reverse();
        //            crossPointsOfPassingLines23AndRayFace23.Reverse();
        //            nextModels.AddRange(this.GetNextRayTubeModelsInTaTbTc_C_PartB_Child(
        //              launchRay3, launchRay1, launchRay2, passingFacesFromCrossNode1To3, passingFacesFromCrossNode2To3,
        //              sameLinesOfPassingFaces13, sameLinesOfPassingFaces23, crossPointsOfPassingLines13AndRayFace13,
        //              crossPointsOfPassingLines23AndRayFace23, crossPointsOfPassingLines12AndRayFace12));
        //        }
        //        else
        //        {
        //            LogFileManager.ObjLog.error("当交点三角面的公共点在交点组成的三角面的投影外时，求得的棱与射线面的交点个数出错");
        //            return new List<RayTubeModel>();
        //        }
        //    }

        //    return nextModels;
        //}


        ///// <summary>
        /////获取当交点三角面的公共点不在交点构成的线段的XY平面投影上时的下一阶段射线管模型
        /////对应图4-9
        ///// </summary>
        ///// <param name="launchRay1">射线1,其交点为1,交点所在三角面也为1</param>
        ///// <param name="launchRay2">射线2,其交点位2,交点所在三角面也为2</param>
        ///// <param name="launchRay3">射线3,其交点为3,交点所在三角面也为3</param>
        /////  <param name="passingFacesFromCrossNode1To2">交点1到2经过的三角面</param>
        /////  <param name="passingFacesFromCrossNode1To3">交点1到3经过的三角面</param>
        /////  <param name="sameLinesOfPassingFaces12">交点1到2经过的三角面中的公共棱</param>
        /////  <param name="sameLinesOfPassingFaces13">交点1到3经过的三角面中的公共棱</param>
        /////  <param name="crossPointsOfPassingLines12AndRayFace12">交点1到2经过的三角面中的公共棱与射线面12的交点</param>
        /////  <param name="crossPointsOfPassingLines13AndRayFace13">交点1到3经过的三角面中的公共棱与射线面13的交点</param>
        /////  <param name="crossPointsOfPassingLines23AndRayFace23">交点2到3经过的三角面中的公共棱与射线面23的交点,该交点的个数是最多的</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsInTaTbTc_C_PartB_Child(OneRayModel launchRay1, OneRayModel launchRay2, OneRayModel launchRay3,
        //     List<Face> passingFacesFromCrossNode1To2, List<Face> passingFacesFromCrossNode1To3, List<AdjacentEdge> sameLinesOfPassingFaces12, List<AdjacentEdge> sameLinesOfPassingFaces13,
        //     List<Point> crossPointsOfPassingLines12AndRayFace12, List<Point> crossPointsOfPassingLines13AndRayFace13, List<Point> crossPointsOfPassingLines23AndRayFace23)
        //{
        //    //交点2到3经过的三角面中的公共棱与射线面23的交点是最多的，且12的个数+13的个数=23的个数
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    if (crossPointsOfPassingLines12AndRayFace12.Count + crossPointsOfPassingLines13AndRayFace13.Count != crossPointsOfPassingLines23AndRayFace23.Count)
        //    {
        //        LogFileManager.ObjLog.error("当交点三角面的公共顶点的投影不在交点组成的三角面内时，线段12+线段13的交点个数不等于线段23的交点个数");
        //        return new List<RayTubeModel>();
        //    }
        //    //三角面的1反射模型
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //        new List<Point> { crossPointsOfPassingLines12AndRayFace12[0],crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count-1],
        //                crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count], crossPointsOfPassingLines13AndRayFace13[0] },
        //        launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay1 }));
        //    //三角面2的反射模型
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //      new List<Point> { crossPointsOfPassingLines12AndRayFace12[crossPointsOfPassingLines12AndRayFace12.Count - 1], crossPointsOfPassingLines23AndRayFace23[0] },
        //      launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay2 }));
        //    //三角面3的反射模型
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //      new List<Point> { crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines23AndRayFace23.Count - 1], crossPointsOfPassingLines13AndRayFace13[crossPointsOfPassingLines13AndRayFace13.Count - 1] },
        //      launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay3 }));

        //    //从交点1到交点2经过的不包括三角面1，2的反射模型
        //    for (int i = 0; i < crossPointsOfPassingLines12AndRayFace12.Count - 1; i++)
        //    {
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //       new List<Point> { crossPointsOfPassingLines12AndRayFace12[i],crossPointsOfPassingLines12AndRayFace12[i+1],
        //               crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count- i- 2],
        //                crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count-i -1] },
        //       passingFacesFromCrossNode1To2[i + 1], new List<OneRayModel>()));
        //    }
        //    //从交点1到交点3经过的不包括三角面1，2的反射模型
        //    for (int j = 0; j < crossPointsOfPassingLines13AndRayFace13.Count - 1; j++)
        //    {
        //        nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //       new List<Point> { crossPointsOfPassingLines13AndRayFace13[j],crossPointsOfPassingLines13AndRayFace13[j+1],
        //               crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count + j + 1],
        //                crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count+ j] },
        //       passingFacesFromCrossNode1To2[j + 1], new List<OneRayModel>()));
        //    }

        //    if (this.DiffractionTracingTimes == 0)
        //    {
        //        //线段12经过的三角面中的绕射模型
        //        for (int m = 0; m < crossPointsOfPassingLines12AndRayFace12.Count; m++)
        //        {
        //            if (launchRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLinesOfPassingFaces12[m].AdjacentTriangles[0], sameLinesOfPassingFaces12[m].AdjacentTriangles[1]))
        //            {
        //                nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPointsOfPassingLines12AndRayFace12[m],
        //                    crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count - m - 1],
        //                    sameLinesOfPassingFaces12[m]));
        //            }
        //        }
        //        //线段13经过的三角面中的绕射模型
        //        for (int n = 0; n < crossPointsOfPassingLines13AndRayFace13.Count; n++)
        //        {
        //            if (launchRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLinesOfPassingFaces13[n].AdjacentTriangles[0], sameLinesOfPassingFaces13[n].AdjacentTriangles[1]))
        //            {
        //                nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPointsOfPassingLines13AndRayFace13[n],
        //                    crossPointsOfPassingLines23AndRayFace23[crossPointsOfPassingLines12AndRayFace12.Count + n],
        //                    sameLinesOfPassingFaces13[n]));
        //            }
        //        }
        //    }

        //    return nextModels;
        //}



        ///// <summary>
        /////获取当ray1,ray2,ray3的交点所在三角面两两相邻时的下一阶段射线管模型
        /////对应图4-10
        ///// </summary>
        ///// <param name="launchRay1">射线1,其交点为1,交点所在三角面也为1</param>
        ///// <param name="launchRay2">射线2,其交点位2,交点所在三角面也为2</param>
        ///// <param name="launchRay3">射线3,其交点为3,交点所在三角面也为3</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private List<RayTubeModel> GetNextRayTubeModelsWhenTaTbTc_AA(OneRayModel launchRay1, OneRayModel launchRay2, OneRayModel launchRay3)
        //{
        //    List<RayTubeModel> nextModels = new List<RayTubeModel>();
        //    AdjacentEdge sameLineOfCrossFace12 = new AdjacentEdge(launchRay1.CrossNode.ReflectionFace, launchRay2.CrossNode.ReflectionFace);
        //    AdjacentEdge sameLineOfCrossFace13 = new AdjacentEdge(launchRay1.CrossNode.ReflectionFace, launchRay3.CrossNode.ReflectionFace);
        //    AdjacentEdge sameLineOfCrossFace23 = new AdjacentEdge(launchRay2.CrossNode.ReflectionFace, launchRay3.CrossNode.ReflectionFace);
        //    //检查相邻边是否为空
        //    if (sameLineOfCrossFace12.StartPoint == null || sameLineOfCrossFace13.StartPoint == null ||
        //        sameLineOfCrossFace23.StartPoint == null)
        //    {
        //        LogFileManager.ObjLog.error("求ray1,ray2,ray3的交点所在三角面两两相邻时,三角面的公共棱出错");
        //        return new List<RayTubeModel>();
        //    }
        //    SpaceFace rayFace12 = new SpaceFace(launchRay1.LaunchNode.Position, launchRay1.CrossNode.Position, launchRay2.CrossNode.Position);
        //    SpaceFace rayFace13 = new SpaceFace(launchRay1.LaunchNode.Position, launchRay1.CrossNode.Position, launchRay3.CrossNode.Position);
        //    SpaceFace rayFace23 = new SpaceFace(launchRay2.LaunchNode.Position, launchRay2.CrossNode.Position, launchRay3.CrossNode.Position);
        //    Point crossPoint1OfLine12AndRayFace12 = sameLineOfCrossFace12.GetCrossPointWithFace(rayFace12);
        //    Point crossPoint2OfLine13AndRaFace13 = sameLineOfCrossFace13.GetCrossPointWithFace(rayFace13);
        //    Point crossPoint3OfLine23AndRayFace23 = sameLineOfCrossFace23.GetCrossPointWithFace(rayFace23);
        //    List<Point> commonPoints = sameLineOfCrossFace12.GetCommonPointsWithOtherLine(sameLineOfCrossFace13);
        //    //检查交点是否都不为空
        //    if (crossPoint1OfLine12AndRayFace12 == null || crossPoint2OfLine13AndRaFace13 == null
        //        || crossPoint3OfLine23AndRayFace23 == null || commonPoints.Count != 1)
        //    {
        //        LogFileManager.ObjLog.error("求ray1,ray2,ray3的交点所在三角面两两相邻时，公共棱与射线面的交点出错");
        //        return new List<RayTubeModel>();
        //    }
        //    //构造反射射线管模型，虽然点是一样，但是所在面会导致反射射线不一样
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //        new List<Point> { crossPoint1OfLine12AndRayFace12, commonPoints[0], crossPoint2OfLine13AndRaFace13 },
        //        launchRay1.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay1 }));
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //      new List<Point> { crossPoint1OfLine12AndRayFace12, commonPoints[0], crossPoint3OfLine23AndRayFace23 },
        //      launchRay2.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay2 }));
        //    nextModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //      new List<Point> { crossPoint2OfLine13AndRaFace13, commonPoints[0], crossPoint3OfLine23AndRayFace23 },
        //      launchRay3.CrossNode.ReflectionFace, new List<OneRayModel> { launchRay3 }));
        //    if (this.DiffractionTracingTimes == 0)
        //    {
        //        //构造绕射管模型，三角面12的绕射棱
        //        if (launchRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLineOfCrossFace12.AdjacentTriangles[0], sameLineOfCrossFace12.AdjacentTriangles[1]))
        //        {
        //            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint1OfLine12AndRayFace12, commonPoints[0], sameLineOfCrossFace12));
        //        }
        //        //三角面13的绕射棱
        //        if (launchRay1.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLineOfCrossFace13.AdjacentTriangles[0], sameLineOfCrossFace13.AdjacentTriangles[1]))
        //        {
        //            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint2OfLine13AndRaFace13, commonPoints[0], sameLineOfCrossFace13));
        //        }
        //        //三角面23的绕射棱
        //        if (launchRay2.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLineOfCrossFace23.AdjacentTriangles[0], sameLineOfCrossFace23.AdjacentTriangles[1]))
        //        {
        //            nextModels.AddRange(this.GetDiffractionRayTubeModels(crossPoint3OfLine23AndRayFace23, commonPoints[0], sameLineOfCrossFace23));
        //        }
        //    }
        //    return nextModels;

        //}


        /// <summary>
        ///为射线管与棱的交点赋值，设置交点所在棱的编号
        /// </summary>
        /// <param name="faceLines">反射面的棱</param>
        /// <param name="rayFromCrossPointTovirtualPoint">在反射平面上两个交点组成的射线</param>
        /// <param name="crossPoint">交点</param>
        /// <param name="edgeNumber">交点所在棱的编号</param>
        /// <returns>下一阶段的射线管模型</returns>
        private Point GetCrossPointAndAssignEdgeNumber(List<AdjacentEdge> faceLines, RayInfo rayFromCrossPointTovirtualPoint, ref int edgeNumber)
        {
            Point crossPoint = null; ;
            for (int i = 0; i < faceLines.Count; i++)
            {
                crossPoint = rayFromCrossPointTovirtualPoint.GetCrossPointWithOtherRay(faceLines[i].SwitchToRay());
                if (crossPoint != null && faceLines[i].JudgeIfPointInLineRange(crossPoint) &&
                     new SpectVector(rayFromCrossPointTovirtualPoint.Origin, crossPoint).IsParallelAndSamedirection(rayFromCrossPointTovirtualPoint.RayVector))
                {
                    edgeNumber = i;
                    break;

                }
                else
                { crossPoint = null; }
            }

            return crossPoint;
        }

        private Point GetCrossPointOfTwoEdge(AdjacentEdge faceLine, LineSegment lineInQuadrangle)
        {
            return new Point();
        }



        ///// <summary>
        /////获取从开始节点直线到反射节点所经过的三角面
        ///// </summary>
        ///// <param name="beginNode">开始节点</param>
        ///// <param name="targetNode">目标节点</param>
        /////  <param name="commonPoint">所有面的公共点</param>
        ///// <returns>经过的三角面</returns>
        //private List<Face> GetFacesFromBeginCrossNodeToTargetCrossNode(Node beginNode, Node targetNode, Point commonPoint)
        //{
        //    if (beginNode.ReflectionFace == null || targetNode.ReflectionFace == null)
        //    {
        //        LogFileManager.ObjLog.error("获取从开始节点直线到反射节点所经过的三角面时,输入的节点的反射面为空");
        //        return new List<Face>();
        //    }
        //    List<Face> passingFaces = new List<Face> { beginNode.ReflectionFace };
        //    RayInfo rayFromBeginToTarget = new RayInfo(beginNode.Position, targetNode.Position);
        //    LineSegment lineFromBeginToTarget = new LineSegment(beginNode.Position, targetNode.Position);
        //    Stack<Face> currentFaces = new Stack<Face>();
        //    currentFaces.Push(beginNode.ReflectionFace);
        //    while (currentFaces.Count != 0 && !currentFaces.Peek().FaceID.IsSameID(targetNode.ReflectionFace.FaceID))
        //    {
        //        Face paramFace = currentFaces.Pop();
        //        for (int i = 0; i < paramFace.Lines.Count; i++)
        //        {
        //            if (paramFace.Lines[i].JudgeIfPointInLineRange(commonPoint))//该棱是否有一个端点是公共点
        //            {
        //                Point crossPoint = rayFromBeginToTarget.GetCrossPointWtihLineInXYPlane(paramFace.Lines[i]);
        //                //该棱与从开始点到目标点的线段在XY平面的投影相交，说明该三角面是经过的三角面
        //                if (crossPoint != null && lineFromBeginToTarget.JudgeIfPointInLineRangeInXYPlane(crossPoint))
        //                {
        //                    rayFromBeginToTarget = new RayInfo(crossPoint, rayFromBeginToTarget.RayVector);
        //                    //若绕射棱的所在三角面1是目前的三角面，添加另一个三角面到栈上
        //                    if (paramFace.JudgeIsTheSameFace(paramFace.Lines[i].AdjacentTriangles[0]))
        //                    {
        //                        passingFaces.Add(paramFace.Lines[i].AdjacentTriangles[1]);
        //                        currentFaces.Push(paramFace.Lines[i].AdjacentTriangles[1]);
        //                    }
        //                    else
        //                    {
        //                        passingFaces.Add(paramFace.Lines[i].AdjacentTriangles[0]);
        //                        currentFaces.Push(paramFace.Lines[i].AdjacentTriangles[0]);
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    if (passingFaces.Count >= 2)
        //    {
        //        //第一个和最后一个是开始点和目标点所在的三角面
        //        return passingFaces;
        //    }
        //    else
        //    {
        //        LogFileManager.ObjLog.error("获取从开始节点直线到反射节点所经过的三角面时出错");
        //        return new List<Face>();
        //    }
        //}



        ///// <summary>
        /////把从开始点到目标点经过的三角面中的公共棱及公共棱与射线面的交点添加到List中
        ///// </summary>
        ///// <param name="passingFaces">从开始的到目标点经过的三角面</param>
        ///// <param name="rayFace">射线面</param>
        /////  <param name="sameLinesOfPassingFaces">三角面的公共棱</param>
        /////  <param name="crossPointsOfLinesAndRayFace">棱与射线面的交点</param>
        ///// <returns>下一阶段的射线管模型</returns>
        //private void AddAdjacentEdgeAndCrossPointValueToListInEdgePassingFaces(List<Face> passingFaces, SpaceFace rayFace, List<AdjacentEdge> sameLinesOfPassingFaces, List<Point> crossPointsOfLinesAndRayFace)
        //{
        //    for (int i = 0; i < passingFaces.Count - 1; i++)
        //    {
        //        AdjacentEdge sameLine = new AdjacentEdge(passingFaces[i], passingFaces[i + 1]);
        //        sameLinesOfPassingFaces.Add(sameLine);
        //        RayInfo tempRay = new RayInfo(sameLine.StartPoint, sameLine.LineVector);
        //        Point crossPoint = tempRay.GetCrossPointBetweenStraightLineAndFace(rayFace);
        //        //Point crossPoint = sameLine.GetCrossPointWithFace(rayFace);
        //        if (crossPoint == null)
        //        {
        //            LogFileManager.ObjLog.error("求从开始的到目标点经过的三角面的公共棱和棱与射线面交点时出错");
        //            return;
        //        }
        //        crossPointsOfLinesAndRayFace.Add(crossPoint);
        //    }
        //}

        ///// <summary>
        /////获取当交点的三角面的公共点在交点组成的三角面的投影内，线段经过的三角面中的绕射射线管模型
        ///// </summary>
        ///// <param name="sameLinesOfPassingFaces">经过的三角面中的公共棱</param>
        ///// <param name="crossPointsOfPassingLinesAndRayFace">公共棱与射线面的交点</param>
        /////  <param name="launchRay">射线</param>
        /////  <param name="commonVertex">交点三角面的公共顶点</param>
        ///// <returns>下一阶段的绕射射线管模型</returns>
        //private List<RayTubeModel> GetDiffractionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(List<AdjacentEdge> sameLinesOfPassingFaces, List<Point> crossPointsOfPassingLinesAndRayFace, OneRayModel launchRay, Point commonVertex)
        //{
        //    List<RayTubeModel> diffractionModels = new List<RayTubeModel>();
        //    for (int m = 0; m < sameLinesOfPassingFaces.Count; m++)
        //    {
        //        if (sameLinesOfPassingFaces[m].IsDiffractionEdge && launchRay.LaunchNode.Position.JudgeIsConcaveOrConvexToViewPoint(sameLinesOfPassingFaces[m].AdjacentTriangles[0], sameLinesOfPassingFaces[m].AdjacentTriangles[1]))
        //        {
        //            diffractionModels.AddRange(this.GetDiffractionRayTubeModels(crossPointsOfPassingLinesAndRayFace[m], commonVertex, sameLinesOfPassingFaces[m]));
        //        }
        //    }
        //    return diffractionModels;
        //}

        ///// <summary>
        /////获取当交点的三角面的公共点在交点组成的三角面的投影内，线段经过的三角面中的反射射线管模型
        ///// </summary>
        ///// <param name="passingFaces">经过的三角面</param>
        ///// <param name="crossPointsOfPassingLinesAndRayFace">公共棱与射线面的交点</param>
        /////  <param name="launchRay">射线</param>
        /////  <param name="commonVertex">交点三角面的公共顶点</param>
        ///// <returns>下一阶段的反射射线管模型</returns>
        //private List<RayTubeModel> GetReflectionRayTubeModelsInPassingFacesWhenComonVertexInCrossTriangle(List<Face> passingFaces, List<Point> crossPointsOfPassingLinesAndRayFace, Point commonVertex)
        //{
        //    List<RayTubeModel> reflectionModels = new List<RayTubeModel>();
        //    if (crossPointsOfPassingLinesAndRayFace.Count > 1)//若交点个数为1，说明三角面1,2相邻，不需要求其经过的三角面的反射模型
        //    {
        //        for (int j = 0; j < crossPointsOfPassingLinesAndRayFace.Count - 1; j++)
        //        {
        //            reflectionModels.AddRange(this.GetReflectionTriangleRayTubeModeWithLineCrossPoints(
        //                new List<Point> { crossPointsOfPassingLinesAndRayFace[j], commonVertex, crossPointsOfPassingLinesAndRayFace[j + 1] },
        //                passingFaces[j + 1], new List<OneRayModel>()));
        //        }
        //    }
        //    return reflectionModels;
        //}


        /// <summary>
        ///根据打到反射面上的射线以及反射面上的交点求三角形截面射线管的反射射线管模型
        /// </summary>
        /// <param name="crossPoints">在反射面棱上的交点，是射线管与棱的交点或者棱的端点,交点的位置按顺时针或者逆时针顺序存放</param>
        /// <param name="reflectionFace">反射面</param>
        ///  <param name="crossRays">打到反射面上的射线</param>
        /// <returns>反射射线管模型</returns>
        private List<RayTubeModel> GetReflectionTriangleRayTubeModeWithLineCrossPoints(List<Point> crossPoints, Face reflectionFace, List<OneRayModel> crossRays)
        {
            if (crossPoints == null || crossRays == null || crossPoints.Count < 2)
            {
                LogFileManager.ObjLog.error("根据打到反射面上的射线以及反射面上的交点构造反射射线管模型时,输入有错");
                return new List<RayTubeModel>();

            }
            List<Node> crossNodes = new List<Node>();
            for (int i = 0; i < crossPoints.Count; i++)
            {
                crossNodes.Add(this.SwitchPointToReflectionNode(this.oneRayModels[0].LaunchNode, crossPoints[i], reflectionFace));
            }
            List<RayTubeModel> reflectionRayTubeModels = new List<RayTubeModel>();
            if (crossRays.Count == 0 && crossNodes.Count >= 3)//三角面只有棱上的交，内部没有射线的交点
            {
                for (int m = 1; m < crossNodes.Count - 1; m++)
                {
                    RayTubeModel tempTubeModel = this.GetReflectionModels(new List<Node> { crossNodes[0], crossNodes[m], crossNodes[m + 1] }, reflectionFace);
                    if (tempTubeModel != null)
                    { reflectionRayTubeModels.Add(tempTubeModel); }
                }
            }
            if (crossRays.Count >= 1) //根据射到该反射面的射线条数来构造反射模型
            {
                for (int j = 0; j < crossNodes.Count - 1; j++)
                {
                    RayTubeModel tempTubeModel = this.GetReflectionModels(new List<Node> { crossRays[0].CrossNode, crossNodes[j], crossNodes[j + 1] }, reflectionFace);
                    if (tempTubeModel != null)
                    { reflectionRayTubeModels.Add(tempTubeModel); }
                }
                //若打到该反射面的射线有两条，加一个由两个射线的交点和crossPoints最后一个点组成的反射模型
                if (crossRays.Count == 2)
                {
                    RayTubeModel tempTubeModel = this.GetReflectionModels(new List<Node> { crossRays[0].CrossNode, crossRays[1].CrossNode, crossNodes[crossNodes.Count - 1] }, reflectionFace);
                    if (tempTubeModel != null)
                    { reflectionRayTubeModels.Add(tempTubeModel); }
                }
            }
            return reflectionRayTubeModels;
        }

        /// <summary>
        ///求反射射线管模型，使用前保证反射点都在一个面上
        /// </summary>
        /// <param name="crossNode1">与反射面的交点</param>
        /// <param name="reflectionFace">反射面</param>
        /// <returns></returns>
        private RayTubeModel GetReflectionModels(List<Node> crossNodes, Face reflectionFace)
        {
            if (crossNodes.Count != 3)
            {
                LogFileManager.ObjLog.error("构造反射射线管是，射线不是三条");
                return null;
            }
            Point nextLaunchPoint = new Point();
            if (launchPoint != null)
            {
                nextLaunchPoint = this.launchPoint.GetMirrorPoint(reflectionFace);
            }
            else
            {
                return null;
            }
            List<OneRayModel> reflectionRayModels = new List<OneRayModel>();
            if (crossNodes[0].Position.equal(crossNodes[1].Position) || crossNodes[1].Position.equal(crossNodes[2].Position) ||
                crossNodes[2].Position.equal(crossNodes[0].Position)) return null;
            RayTubeModel reflectionTube;
            if (this.tubeType == TubeType.FlatTube)//平面射线管，产生的反射射线管是点发射平面射线管
            {
                for (int i = 0; i < crossNodes.Count; i++)
                {
                    reflectionRayModels.Add(new OneRayModel(crossNodes[i], new RayInfo(crossNodes[i].Position, new SpectVector(nextLaunchPoint, crossNodes[i].Position))));
                }
                reflectionTube = new RayTubeModel(reflectionRayModels, nextLaunchPoint, this.reflectionTimes + 1,
                    this.diffractionTimes, this.tessellationTimes, this.passTimes);
            }
            else if (this.tubeType == TubeType.CurveTube)//曲面射线管产生的反射射线管是棱发射曲面射线管
            {
                AdjacentEdge nextLaunchEdge = new AdjacentEdge(this.launchEdge.StartPoint.GetMirrorPoint(reflectionFace),
                    this.launchEdge.EndPoint.GetMirrorPoint(reflectionFace));
                for (int i = 0; i < crossNodes.Count; i++)
                {
                    Point temp = nextLaunchEdge.GetInfiniteDiffractionPoint(nextLaunchPoint, crossNodes[i].Position);
                    if (temp != null)
                    {
                        reflectionRayModels.Add(new OneRayModel(crossNodes[i], new RayInfo(crossNodes[i].Position,
                        new SpectVector(temp, crossNodes[i].Position))));
                    }
                    else
                    {
                        return null;
                    }
                }
                reflectionTube = new RayTubeModel(reflectionRayModels, nextLaunchPoint, nextLaunchEdge,
                    this.reflectionTimes + 1, this.diffractionTimes, this.tessellationTimes, this.passTimes);
            }
            else
            { return null; }
            return reflectionTube;
        }


        
        /// <summary>
        ///获取从绕射棱发出的射线管模型
        /// </summary>
        /// <param name="leftPoint">射线管与绕射棱的左交点</param>
        /// <param name="rightPoint">射线管与绕射棱的右交点</param>
        ///  <param name="diffractionEdge">绕射棱</param>
        /// <returns></returns>
        private List<RayTubeModel> GetDiffractionRayTubeModels(Point leftPoint, Point rightPoint, AdjacentEdge diffractionEdge)
        {
            if (leftPoint.equal(rightPoint) || this.launchPoint == null) return new List<RayTubeModel>();
            Node leftLaunchNode, rightLaunchNode;
            if (this.launchType == LaunchType.Point && (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.Tx
                || this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode))//从辐射源发出或特殊绕射情况发出
            {
                leftLaunchNode = rightLaunchNode = this.oneRayModels[0].LaunchNode;
            }
            else
            {
                Point leftLaunchPoint, rightLaunchPoint;
                if (this.launchType == LaunchType.Point && this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.ReflectionNode)
                {
                    //从反射面发出，点状发射类型不会从绕射棱发出
                    leftLaunchPoint = new RayInfo(this.launchPoint, leftPoint).GetCrossPointWithFace(this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace());//求出对应的在反射面的发射点
                    rightLaunchPoint = new RayInfo(this.launchPoint, rightPoint).GetCrossPointWithFace(this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace());//求出对应的在反射面的发射点
                }
                else//射线管发射类型是线状发射类型
                {
                    if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.ReflectionNode)
                    {
                        leftLaunchPoint = new RayInfo(this.launchEdge.GetInfiniteDiffractionPoint(this.launchPoint, leftPoint), leftPoint).
                            GetCrossPointWithFace(this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace());
                        rightLaunchPoint = new RayInfo(this.launchEdge.GetInfiniteDiffractionPoint(this.launchPoint, rightPoint), rightPoint).
                            GetCrossPointWithFace(this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace());
                    }
                    else if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode)
                    {
                        leftLaunchPoint = this.launchEdge.GetInfiniteDiffractionPoint(this.launchPoint, leftPoint);
                        rightLaunchPoint = this.launchEdge.GetInfiniteDiffractionPoint(this.launchPoint, rightPoint);
                    }
                    else { return new List<RayTubeModel>(); }
                    if (leftLaunchPoint == null || rightLaunchPoint == null)
                    {
                        LogFileManager.ObjLog.error("求绕射射线时，求绕射点对应的发射点时错误");
                        return new List<RayTubeModel>();
                    }
                }
                if (leftLaunchPoint == null || rightLaunchPoint == null)
                {
                    return new List<RayTubeModel>();
                }
                leftLaunchNode = this.StructureNewLaunchNode(leftLaunchPoint);
                rightLaunchNode = this.StructureNewLaunchNode(rightLaunchPoint);
            }
            //将绕射交点点转成节点
            Node leftNode = this.SwitchPointToDiffractionNode(leftLaunchNode, leftPoint, diffractionEdge);
            Node rightNode = this.SwitchPointToDiffractionNode(rightLaunchNode, rightPoint, diffractionEdge);
            //求出节点的绕射射线
            List<List<RayInfo>> diffractionRays = Rays.GetDiffractionRaysFromTwoDiffractionPoints(leftLaunchNode.Position, rightLaunchNode.Position, leftPoint, rightPoint, diffractionEdge, 12);
            //获取绕射模型
            List<RayTubeModel> diffractionModels = new List<RayTubeModel>();
            if (diffractionRays.Count == 0)
            {
                return diffractionModels;
            }
            if (diffractionRays[0][0].RayVector.IsParallel(diffractionRays[1][0].RayVector))
            {
                //转成射线模型
                List<OneRayModel> leftRayModels = this.ChangeRaysToSpecialOneRayModels(leftNode, diffractionRays[0]);
                List<OneRayModel> rightRayModels = this.ChangeRaysToSpecialOneRayModels(leftNode, diffractionRays[1]);

                for (int j = 0; j < leftRayModels.Count - 1; j++)
                {
                    diffractionModels.Add(new RayTubeModel(new List<OneRayModel> { leftRayModels[j], rightRayModels[j], leftRayModels[j + 1] },
                        leftPoint, this.reflectionTimes, this.diffractionTimes + 1, this.tessellationTimes, this.passTimes));
                    diffractionModels.Add(new RayTubeModel(new List<OneRayModel> { rightRayModels[j + 1], leftRayModels[j + 1], rightRayModels[j] },
                        leftPoint, this.reflectionTimes, this.diffractionTimes + 1, this.tessellationTimes, this.passTimes));
                }
            }
            else
            {
                //转成射线模型
                List<OneRayModel> leftRayModels = this.ChangeRaysToOneRayModels(leftNode, diffractionRays[0]);
                List<OneRayModel> rightRayModels = this.ChangeRaysToOneRayModels(rightNode, diffractionRays[1]);

                for (int j = 0; j < leftRayModels.Count - 1; j++)
                {
                    diffractionModels.Add(new RayTubeModel(new List<OneRayModel> { leftRayModels[j], rightRayModels[j], leftRayModels[j + 1] },
                        leftRayModels[j].LaunchRay.GetCrossPointWithOtherRay(rightRayModels[j].LaunchRay), new AdjacentEdge(leftPoint, rightPoint),
                        this.reflectionTimes, this.diffractionTimes + 1, this.tessellationTimes, this.passTimes));
                    diffractionModels.Add(new RayTubeModel(new List<OneRayModel> { rightRayModels[j + 1], leftRayModels[j + 1], rightRayModels[j] },
                        rightRayModels[j + 1].LaunchRay.GetCrossPointWithOtherRay(leftRayModels[j + 1].LaunchRay), new AdjacentEdge(rightPoint, leftPoint),
                        this.reflectionTimes, this.diffractionTimes + 1, this.tessellationTimes, this.passTimes));
                }
            }
            return diffractionModels;
        }


        /// <summary>
        ///构造新的发射节点
        /// </summary>
        ///  <param name="position">点的位置</param>
        /// <returns></returns>
        private Node StructureNewLaunchNode(Point position)
        {
            Node newLaunchNode = new Node
            {
                FatherNode = this.oneRayModels[0].LaunchNode.FatherNode,
                Position = position,
                NodeStyle = this.oneRayModels[0].LaunchNode.NodeStyle,
                UAN = this.oneRayModels[0].LaunchNode.UAN,
                DistanceToFrontNode = this.oneRayModels[0].LaunchNode.DistanceToFrontNode,
                ReflectionFace = this.oneRayModels[0].LaunchNode.ReflectionFace,
                DiffractionEdge = this.oneRayModels[0].LaunchNode.DiffractionEdge,
                //   RayIn=new RayInfo(this.oneRayModels[0].LaunchNode.FatherNode.Position,position)

            };
            if (this.oneRayModels[0].LaunchNode.FatherNode != null)
            {
                newLaunchNode.RayIn = new RayInfo(this.oneRayModels[0].LaunchNode.FatherNode.Position, position);
                this.oneRayModels[0].LaunchNode.FatherNode.ChildNodes.Add(newLaunchNode);
            }
            return newLaunchNode;
        }

        /// <summary>
        ///将坐标点转换成反射节点
        /// </summary>
        ///  <param name="position">点的位置</param>
        /// <returns></returns>
        private Node SwitchPointToReflectionNode(Node fatherNode, Point position, Face reflectionFace)
        {
            Node newLaunchNode = new Node
            {
                FatherNode = fatherNode,
                Position = position,
                NodeStyle = NodeStyle.ReflectionNode,
                UAN = fatherNode.UAN,
                DistanceToFrontNode = fatherNode.Position.GetDistance(position),
                ReflectionFace = reflectionFace,
                RayIn = new RayInfo(fatherNode.Position, position)

            };
            fatherNode.ChildNodes.Add(newLaunchNode);
            return newLaunchNode;
        }



        /// <summary>
        ///将坐标点转换成绕射节点
        /// </summary>
        /// <param name="fatherNode">父节点</param>
        /// <param name="position">绕射点的位置</param>
        ///  <param name="diffractionEdge">绕射棱</param>
        /// <returns></returns>
        private Node SwitchPointToDiffractionNode(Node fatherNode, Point position, AdjacentEdge diffractionEdge)
        {
            Node diffractionNode = new Node
            {
                Position = position,
                FatherNode = fatherNode,
                DiffractionEdge = diffractionEdge,
                NodeStyle = NodeStyle.DiffractionNode,
                IsReceiver = false,
                UAN = fatherNode.UAN,
                RayIn = new RayInfo(fatherNode.Position, position),
                DistanceToFrontNode = position.GetDistance(fatherNode.Position)
            };
            fatherNode.ChildNodes.Add(diffractionNode);
            return diffractionNode;
        }

        /// <summary>
        ///把射线转成射线模型
        /// </summary>
        /// <param name="launchNode">发射节点</param>
        /// <param name="rays">射线</param>
        /// <returns></returns>
        private List<OneRayModel> ChangeRaysToOneRayModels(Node launchNode, List<RayInfo> rays)
        {
            List<OneRayModel> rayModels = new List<OneRayModel>();
            for (int i = 0; i < rays.Count; i++)
            {
                rayModels.Add(new OneRayModel(launchNode, rays[i]));
            }
            return rayModels;
        }

        /// <summary>
        ///把射线转成特殊射线模型
        /// </summary>
        /// <param name="launchNode">发射节点</param>
        /// <param name="rays">射线</param>
        /// <returns></returns>
        private List<OneRayModel> ChangeRaysToSpecialOneRayModels(Node launchNode, List<RayInfo> rays)
        {
            List<OneRayModel> rayModels = new List<OneRayModel>();
            for (int i = 0; i < rays.Count; i++)
            {
                rayModels.Add(new OneRayModel(launchNode, new RayInfo(launchNode.Position, rays[i].GetPointOnRayVector(1))));
            }
            return rayModels;
        }


        /// <summary>
        ///设置一条到目标的的射线，使用前保证目标点在射线管内
        /// </summary>
        /// <param name="targetPoint">目标点</param>
        /// <returns></returns>
        public OneRayModel StructureRayModelToTargetPoint(Point targetPoint)
        {
            OneRayModel toRxRayModel;
            if (this.tubeType == TubeType.FlatTube)//射线管是平面射线管
            {
                if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.Tx || this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode) //如果该射线管是从辐射源
                {
                    toRxRayModel = new OneRayModel(this.oneRayModels[0].LaunchNode, new RayInfo(this.oneRayModels[0].LaunchNode.Position, targetPoint));
                }
                else//从反射面发出出
                {
                    if (this.launchPoint != null)
                    {
                        Point crossPoint = new RayInfo(this.launchPoint, targetPoint).GetCrossPointWithFace(this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace());//求出对应的在反射面的发射点
                        if (crossPoint != null)
                        {
                            Node crossNode = this.StructureNewLaunchNode(crossPoint);
                            toRxRayModel = new OneRayModel(crossNode, new RayInfo(crossPoint, targetPoint));//构造从反射面发出的射线模型
                        }
                        else { return null; }
                    }
                    else { return null; }
                }
            }
            else//射线管是曲面射线管
            {
                if (this.launchPoint == null) { return null; }
                Point launchPointOnEdge = this.launchEdge.GetInfiniteDiffractionPoint(this.launchPoint, targetPoint);
                if (launchPointOnEdge == null)
                {
                    LogFileManager.ObjLog.error("求目标点对应在曲面射线管的父节点时出错");
                    return null;
                }
                if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.DiffractionNode)
                {
                    Node fatherNode = this.StructureNewLaunchNode(launchPointOnEdge);
                    toRxRayModel = new OneRayModel(fatherNode, new RayInfo(launchPointOnEdge, targetPoint));
                }
                else if (this.oneRayModels[0].LaunchNode.NodeStyle == NodeStyle.ReflectionNode)
                {
                    Point crossPoint = new RayInfo(launchPointOnEdge, targetPoint).GetCrossPointWithFace(this.oneRayModels[0].LaunchNode.ReflectionFace.SwitchToSpaceFace());
                    if (crossPoint == null)
                    {
                        return null;
                    }
                    Node crossNode = this.StructureNewLaunchNode(crossPoint);
                    toRxRayModel = new OneRayModel(crossNode, new RayInfo(crossPoint, targetPoint));
                }
                else
                {
                    LogFileManager.ObjLog.error("求目标点对应在曲面射线管时出现除反射和绕射以外的类型");
                    return null;
                }
            }
            return toRxRayModel;
        }


        /// <summary>
        ///根据点状射线管射线的交点情况，更新射线管的相交类型状态
        /// </summary>
        /// <returns></returns>
        private void UpdataRayTubeCrossFlag()
        {
            bool ray0HasCrossNode = (this.oneRayModels[0].CrossNode != null) ? true : false;
            bool ray1HasCrossNode = (this.oneRayModels[1].CrossNode != null) ? true : false;
            bool ray2HasCrossNode = (this.oneRayModels[2].CrossNode != null) ? true : false;
            //
            if (!ray0HasCrossNode && !ray1HasCrossNode && !ray2HasCrossNode)//射线都没有交点
            {
                this.crossType = RayTubeCrossType.NoneCrossNodes;
            }
            else if (ray0HasCrossNode && ray1HasCrossNode && ray2HasCrossNode)//三条射线都有交点
            {
                this.UpdataRayTubeCrossFlagWhenTTT();
            }
            else if (!ray0HasCrossNode && ray1HasCrossNode && ray2HasCrossNode)//射线0无交点
            {
                //两个交点是否在一个三角面内,同时还要判断无交点的那条射线是否与交点三角面平行
                if (this.oneRayModels[1].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[2].CrossNode.ReflectionFace))
                {
                    if (!this.oneRayModels[0].LaunchRay.RayVector.IsVertical(this.oneRayModels[1].CrossNode.ReflectionFace.NormalVector))
                    {
                        this.crossType = RayTubeCrossType.FTaTa;
                    }
                    else
                    {
                        this.crossType = RayTubeCrossType.TTP;
                    }
                }
                else
                {
                    this.crossType = RayTubeCrossType.FTaTb_D;
                }
            }
            else if (ray0HasCrossNode && !ray1HasCrossNode && ray2HasCrossNode)//射线1无交点
            {
                //两个交点是否在一个三角面内,同时还要判断无交点的那条射线是否与交点三角面平行
                if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[2].CrossNode.ReflectionFace))
                {
                    if (!this.oneRayModels[1].LaunchRay.RayVector.IsVertical(this.oneRayModels[0].CrossNode.ReflectionFace.NormalVector))
                    {
                        this.crossType = RayTubeCrossType.TaFTa;
                    }
                    else
                    {
                        this.crossType = RayTubeCrossType.TTP;
                    }
                }
                else
                {
                    this.crossType = RayTubeCrossType.TaFTb_D;
                }
            }
            else if (ray0HasCrossNode && ray1HasCrossNode && !ray2HasCrossNode)//射线2无交点
            {
                //两个交点是否在一个三角面内,同时还要判断无交点的那条射线是否与交点三角面平行
                if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[1].CrossNode.ReflectionFace))
                {
                    if (!this.oneRayModels[2].LaunchRay.RayVector.IsVertical(this.oneRayModels[0].CrossNode.ReflectionFace.NormalVector))
                    {
                        this.crossType = RayTubeCrossType.TaTaF;
                    }
                    else
                    {
                        this.crossType = RayTubeCrossType.TTP;
                    }
                }
                else
                {
                    this.crossType = RayTubeCrossType.TaTbF_D;
                }
            }
            else if (!ray0HasCrossNode && !ray1HasCrossNode && ray2HasCrossNode)//射线0,1无交点
            {
                this.crossType = RayTubeCrossType.FFT;
            }
            else if (ray0HasCrossNode && !ray1HasCrossNode && !ray2HasCrossNode)//射线1,2无交点
            {
                this.crossType = RayTubeCrossType.TFF;
            }
            else if (!ray0HasCrossNode && ray1HasCrossNode && !ray2HasCrossNode) //射线0,2无交点
            {
                this.crossType = RayTubeCrossType.FTF;
            }
        }


        /// <summary>
        ///当射线管三条射线都有交点时，根据交点所在面的相互关系更新射线管的相交类型状态
        /// </summary>
        /// <returns></returns>
        private void UpdataRayTubeCrossFlagWhenTTT()
        {
            //三个交点在一个三角面或者在建筑物的一个面内
            if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[1].CrossNode.ReflectionFace) &&
               this.oneRayModels[1].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[2].CrossNode.ReflectionFace))
            {
                this.crossType = RayTubeCrossType.TaTaTa_S;
            }
            //两个交点在同一个三角面内
            else if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[1].CrossNode.ReflectionFace))//射线0,1的交点在一个三角面
            {
                //两个三角面是否相邻
                if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeTheFacesAreAdjacent(this.oneRayModels[2].CrossNode.ReflectionFace))
                { this.crossType = RayTubeCrossType.TaTaTb_A; }
                //两个三角面是否有一个公共顶点
                else if (this.oneRayModels[0].CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(this.oneRayModels[2].CrossNode.ReflectionFace).Count == 1)
                { this.crossType = RayTubeCrossType.TaTaTb_C; }
                else
                { this.crossType = RayTubeCrossType.TaTaTb_D; }
            }
            else if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[2].CrossNode.ReflectionFace))//射线0,2的交点在一个三角面
            {
                //两个三角面是否相邻
                if (this.oneRayModels[0].CrossNode.ReflectionFace.JudgeTheFacesAreAdjacent(this.oneRayModels[1].CrossNode.ReflectionFace))
                { this.crossType = RayTubeCrossType.TaTbTa_A; }
                //两个三角面是否有一个公共顶点
                else if (this.oneRayModels[0].CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(this.oneRayModels[1].CrossNode.ReflectionFace).Count == 1)
                { this.crossType = RayTubeCrossType.TaTbTa_C; }
                else
                { this.crossType = RayTubeCrossType.TaTbTa_D; }
            }
            else if (this.oneRayModels[1].CrossNode.ReflectionFace.JudgeIsTheSameFace(this.oneRayModels[2].CrossNode.ReflectionFace))//射线1,2的交点在一个三角面
            {
                //两个三角面是否相邻
                if (this.oneRayModels[1].CrossNode.ReflectionFace.JudgeTheFacesAreAdjacent(this.oneRayModels[0].CrossNode.ReflectionFace))
                { this.crossType = RayTubeCrossType.TaTbTb_A; }
                //两个三角面是否有一个公共顶点
                else if (this.oneRayModels[1].CrossNode.ReflectionFace.GetCommonPointsWithOtherFace(this.oneRayModels[0].CrossNode.ReflectionFace).Count == 1)
                { this.crossType = RayTubeCrossType.TaTbTb_C; }
                else
                { this.crossType = RayTubeCrossType.TaTbTb_D; }
            }
            else//三个交点在三个不同的平面
            {
                this.crossType = RayTubeCrossType.TaTbTc;
            }
        }
        public Object Clone()
        {
            RayTubeModel newRayTube = new RayTubeModel(this.oneRayModels);
            newRayTube.launchType = this.launchType;
            newRayTube.tubeType = this.tubeType;
            newRayTube.reflectionTimes = this.reflectionTimes;
            newRayTube.diffractionTimes = this.diffractionTimes;
            newRayTube.tessellationTimes = this.tessellationTimes;
            newRayTube.launchPoint = this.launchPoint;
            newRayTube.launchEdge = this.launchEdge;
            newRayTube.launchType = this.launchType;
            newRayTube.crossType = this.crossType;
            newRayTube.crossLayerNum = this.crossLayerNum;
            newRayTube.haveTraced = this.haveTraced;
            newRayTube.isReachingRx = this.isReachingRx;
            newRayTube.isReachingArea = this.isReachingArea;
            newRayTube.FatherRayTube = this.FatherRayTube;
            newRayTube.isReachingRx = this.isReachingRx;
            newRayTube.isReachingRx = this.isReachingRx;
            newRayTube.isReachingRx = this.isReachingRx;
            newRayTube.isReachingRx = this.isReachingRx;
            newRayTube.isReachingRx = this.isReachingRx;
            //newRayTube.SetTheTracingFlag();
            return newRayTube;
        }
    }


    /// <summary>
    ///射线模型类
    /// </summary>
    public class OneRayModel : ICloneable
    {
        private Node launchNode;
        private RayInfo launchRay;
        private Node crossNode;
        private bool haveTraced = false;

        public Node LaunchNode
        {
            get { return this.launchNode; }
        }
        public RayInfo LaunchRay
        {
            get { return this.launchRay; }
        }
        public Node CrossNode
        {
            get { return this.crossNode; }
            set { this.crossNode = value; }
        }
        public bool HaveTraced
        {
            get { return this.haveTraced; }
            set { this.haveTraced = value; }
        }

        public OneRayModel(Node launchNode, RayInfo launchRay)
        {
            this.launchNode = launchNode;
            this.launchRay = launchRay;
        }

        /// <summary>
        ///进行射线追踪，只进行直射和反射追踪，并为交点赋值
        /// </summary>
        /// <param name="ter">地形</param>
        ///  <param name="buildings">建筑物</param>
        /// <returns></returns>
        public void TracingThisRay(Terrain ter, City buildings)
        {
            List<Node> crossNodes = new List<Node>();
            List<Rectangle> shadowRect = ter.lineRect(this.launchRay);//记录射线在俯视图投影经过的矩形
            Node crossWithTer = this.launchRay.GetCrossNodeWithTerrainRects(shadowRect);//记录射线与地形交点、与源点距离、所在面
            Node crossWithCity = buildings.GetReflectionNodeWithCity(this.launchRay);//记录射线与建筑物交点、与源点距离、所在面
            //把所有交点放到一个list中
            if (crossWithTer != null)
            { crossNodes.Add(crossWithTer); }
            if (crossWithCity != null)
            { crossNodes.Add(crossWithCity); }
            //
            if (crossNodes.Count == 0)
            { this.crossNode = null; }
            else
            {
                this.crossNode = crossNodes[0];
                if (crossNodes.Count > 1)
                {
                    //若交点大于两个，找出最近的
                    for (int i = 1; i < crossNodes.Count; i++)
                    {
                        if (this.crossNode.DistanceToFrontNode > crossNodes[i].DistanceToFrontNode)
                        {
                            this.crossNode = crossNodes[i];
                        }
                    }
                }
            }
            this.haveTraced = true;
        }

        /// <summary>
        ///深拷贝
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            OneRayModel cloneRay = new OneRayModel((Node)this.launchNode.Clone(), (RayInfo)this.launchRay.Clone());
            if (this.launchNode.FatherNode != null)
            {
                this.launchNode.FatherNode.ChildNodes.Add(cloneRay.launchNode);
            }
            if (this.crossNode != null)
            {
                cloneRay.CrossNode = (Node)this.crossNode.Clone();
            }
            cloneRay.HaveTraced = this.haveTraced;
            return cloneRay;
        }
    }

    /// <summary>
    ///射线管追踪情况类
    /// </summary>
    public enum RayTubeCrossType
    {
        //Flat_,Curve_表明射线管类型
        //T,F代表射线管第i条射线是否与环境有交点,P特指该射线没有交点且与三角面接近平行
        //T,F后面的a,b,c,d代表射线交点所在三角面的编号
        //_S,_D,_A,_C,_AA,代表交点所在三角面的关系，S指三角面相同，A指三角面相邻，C指三角面有一个公共顶点， D指三角面没有关系,AA指三个三角面两两相邻

        NoneCrossNodes,//没有交点

        TFF,//只有射线0有交点
        FTF,//只有射线1有交点
        FFT,//只有射线2有交点
        TaTaF,//射线0,1的有交点且交点在同一个三角面
        TaFTa,//射线0,2的有交点且交点在同一个三角面
        FTaTa,//射线1,2的有交点且交点在同一个三角面
        TTP,//两条射线的交点在同一个三角面，但是最后一条射线与该三角面平行
        TaTbF_D,//射线0,1的有交点但交点不在同一个三角面
        TaFTb_D,//射线0,2的有交点但交点不在同一个三角面
        FTaTb_D,//射线1,2的有交点但交点不在同一个三角面
        TaTaTb_A,//射线0,1的交点在同一个三角面，射线2的交点在相邻的三角面
        TaTbTa_A,//射线0,2的交点在同一个三角面，射线1的交点在相邻的三角面
        TaTbTb_A,//射线1,2的交点在同一个三角面，射线0的交点在相邻的三角面
        TaTaTb_C,//射线0,1的交点在同一个三角面，射线2的交点在不相邻但有一个公共顶点的三角面
        TaTbTa_C,//射线0,2的交点在同一个三角面，射线1的交点在不相邻但有一个公共顶点的三角面
        TaTbTb_C,//射线1,2的交点在同一个三角面，射线0的交点在不相邻但有一个公共顶点的三角面
        TaTaTb_D,//射线0,1的交点在同一个三角面，射线2的交点不在相邻的三角面
        TaTbTa_D,//射线0,2的交点在同一个三角面，射线1的交点不在相邻的三角面
        TaTbTb_D,//射线1,2的交点在同一个三角面，射线0的交点不在相邻的三角面
        TaTbTc,//射线0,1,2的交点在三个两两相邻的三角面
        TaTaTa_S,//射线0,1,2的交点在同一个三角面


    }

    /// <summary>
    ///射线管辐射源类型
    /// </summary>
    public enum LaunchType
    {
        Point,
        Line
    }

    /// <summary>
    ///射线管类型
    /// </summary>
    public enum TubeType
    {
        FlatTube,
        CurveTube
    }

    /// <summary>
    ///射线管中任意两条射线的关系
    /// </summary>
    public enum TwoRayRelationship
    {
        SameLaunchPoint,
        SameFace,
        OppositePoisition
    }
}
