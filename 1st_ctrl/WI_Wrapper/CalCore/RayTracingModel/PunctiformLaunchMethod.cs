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
using UanFileProceed;



namespace RayCalInfo
{
    public class PunctiformLaunchMethod
    {
        protected List<Path> rayPaths = new List<Path>();

        public Node DirectCrossNode;//存放直射射线与地形的交点
        public List<Path> RayPaths
        {
            get { return rayPaths; }
            set { rayPaths = value; }
        }

        public PunctiformLaunchMethod()
        {
          
        }

        public PunctiformLaunchMethod(Node tx, ReceiveBall rxBall, Terrain ter, City cityBuilding, int TessellationFrequency, List<FrequencyBand> txFrequencyBand)
        {
            this.rayPaths = this.LaunchRaysOmnidirectional(tx, rxBall, ter, cityBuilding, TessellationFrequency, txFrequencyBand);
        }

        public PunctiformLaunchMethod(Node tx, ReceiveBall rxBall, Terrain ter, City cityBuilding, int firstTF, int finalTF, List<FrequencyBand> txFrequencyBand)
        {
            this.rayPaths = this.GetPunctiformRxPath(tx, rxBall, ter, cityBuilding, firstTF, finalTF, txFrequencyBand);
        }

        /// <summary>
        ///获取所有射线的路径
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="firstN">初始细分点个数</param>
        ///  <param name="finalN">最终细分点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private List<Path> GetPunctiformRxPath(Node tx, ReceiveBall rxBall, Terrain ter, City cityBuilding, int firstN, int finalN, List<FrequencyBand> txFrequencyBand)
        {
            RayInfo directRay = new RayInfo(tx.Position, rxBall.Receiver);
            Node directCrossNode = directRay.GetCrossNodeWithTerrainRects(ter.lineRect(new RayInfo(rxBall.Receiver, new SpectVector(tx.Position, rxBall.Receiver))));
            if (directCrossNode!=null && directCrossNode.DistanceToFrontNode < tx.Position.GetDistance(rxBall.Receiver))//若直射被阻挡
            {
                DirectCrossNode = directCrossNode;
            }
            List<Path> pathInfo = new List<Path>();
            List<RayTracingModel> originUnits = this.GetOriginUnitsOfIcosahedron(tx);//正二十面体的二十个三角面
            double finalDivideAngle = this.GetInitialAngleOfRayBeam(finalN);//最终细分角度，弧度制
            double firstDivideAngle = this.GetInitialAngleOfRayBeam(firstN);//最初细分角度，弧度制
            for (int i = 0; i < originUnits.Count; i++)
            {
                List<RayTracingModel> firstUnits = this.GetInitialTrianglePath(tx,ter, rxBall,cityBuilding, originUnits[i], firstN, firstDivideAngle,txFrequencyBand);
                this.GetPathOfUnit(firstUnits, pathInfo, tx,ter, rxBall,cityBuilding, finalDivideAngle,txFrequencyBand);
            }
               
                return pathInfo;
        }

        /// <summary>
        ///获初始发射时的射线束夹角
        /// </summary>
        ///  <param name="TessellationFrequency">镶嵌次数，等于三角形每条边的细分次数</param>
        /// <returns></returns>
        protected double GetInitialAngleOfRayBeam(int TessellationFrequency)
        {
            double PA = 4 / (Math.Sqrt(10 + 2 * Math.Sqrt(5)));
            double AW = (PA / 2) / Math.Sin(36 * Math.PI / 180);
            return Math.Asin(0.5 * AW / TessellationFrequency);
        }

        /// <summary>
        ///获得正二十面体的20个三角形处理单元
        /// </summary>
        ///  <param name="tx">发射机</param>
        /// <returns></returns>
        protected List<RayTracingModel> GetOriginUnitsOfIcosahedron(Node tx)
        {
            //求正二十面体顶点坐标的方法参考网址：http://zhidao.baidu.com/link?url=gYy1D8YyS13ZC20yN2kZvrzBc6Ry8oSnJEEZq864yZLwXkNiWjtPqAwIltCDSpTScZvmPnPhAD2ttA-wiL8PU6lQ9aW5FRwRz-xgX-MlTly
            double PA = 4 / (Math.Sqrt(10 + 2 * Math.Sqrt(5)));
            double AW = (PA/2) / Math.Sin(36 * Math.PI / 180);
            double PW = Math.Sqrt(Math.Pow(PA, 2) - Math.Pow(AW, 2));
            Point[] vertex = new Point[12];//点的顺序为PABCDEQFGKMN
            vertex[0] = new Point(0, 0, 1);//P
            vertex[1] = new Point(0, AW, 1 - PW);//A
            vertex[2] = new Point(AW * Math.Cos(18 * Math.PI / 180), AW * Math.Sin(18 * Math.PI / 180), 1 - PW);//B
            vertex[3] = new Point(AW * Math.Cos(54 * Math.PI / 180), -AW * Math.Sin(54 * Math.PI / 180), 1 - PW);//C
            vertex[4] = new Point(-AW * Math.Cos(54 * Math.PI / 180), -AW * Math.Sin(54 * Math.PI / 180), 1 - PW);//D
            vertex[5] = new Point(-AW * Math.Cos(18 * Math.PI / 180), AW * Math.Sin(18 * Math.PI / 180), 1 - PW);//E
            vertex[6] = new Point(0, 0, -1);//Q
            vertex[7] = new Point(-AW * Math.Sin(36 * Math.PI / 180), AW * Math.Cos(36 * Math.PI / 180), -1 + PW);//F
            vertex[8] = new Point(AW * Math.Cos(54 * Math.PI / 180), AW * Math.Sin(54 * Math.PI / 180), -1 + PW);//G
            vertex[9] = new Point(AW * Math.Sin(72 * Math.PI / 180), -AW * Math.Cos(72 * Math.PI / 180), -1 + PW);//K
            vertex[10] = new Point(0, -AW, -1 + PW);//M
            vertex[11] = new Point(-AW * Math.Cos(18 * Math.PI / 180), -AW * Math.Sin(18 * Math.PI / 180), -1 + PW);//N
            //
            ClassNewRay[] originRay = new ClassNewRay[12];
            for (int i = 0; i < 12; i++)
            {
                originRay[i] = new ClassNewRay(tx.Position, new SpectVector(vertex[i].X, vertex[i].Y, vertex[i].Z), false);
            }
            List<RayTracingModel> originUnits = new List<RayTracingModel>();
            //正二十面体上层三角面
            for (int j = 1; j < 5; j++)
            { originUnits.Add(new RayTracingModel(originRay[0], originRay[j], originRay[j + 1])); }
            originUnits.Add(new RayTracingModel(originRay[0], originRay[5], originRay[1]));
            //正二十面体下层三角面
            for (int k = 7; k < 11; k++)
            { originUnits.Add(new RayTracingModel(originRay[6], originRay[k], originRay[k + 1])); }
            originUnits.Add(new RayTracingModel(originRay[6], originRay[11], originRay[7]));
            //正二十面体中层三角面
            for (int l = 1; l < 5; l++)
            { originUnits.Add(new RayTracingModel(originRay[l], originRay[l+1], originRay[l+7])); }
            for (int m = 7; m < 11; m++)
            { originUnits.Add(new RayTracingModel(originRay[m], originRay[m + 1], originRay[m - 6])); }
            originUnits.Add(new RayTracingModel(originRay[5], originRay[1], originRay[7]));
            originUnits.Add(new RayTracingModel(originRay[11], originRay[7], originRay[5]));
            return originUnits;
        }

        /// <summary>
        ///获得处理单元及其细分单元中的射线
        /// </summary>
        /// <param name="units">处理单元</param>
        /// <param name="pathInfo">路径</param>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="divideAngle">射线束夹角</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private void GetPathOfUnit(List<RayTracingModel> units, List<Path> pathInfo, Node tx, Terrain ter, ReceiveBall rxBall, City cityBuilding, double divideAngle, List<FrequencyBand> txFrequencyBand)
        {
            if (units == null || units.Count == 0)
            { throw new Exception("处理追踪单元时，输入出错"); }
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].HaveTraced)//该处理单元已完成射线追踪
                {
                    if (units[i].AngleOfRayBeam < divideAngle*180/Math.PI)//若该处理单元的精度已满足要求
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
                            this.GetPathOfUnit(divideUnits, pathInfo, tx, ter, rxBall,cityBuilding, divideAngle,txFrequencyBand); //追踪细分三角形内的射线
                        }
                    }
                }
                else//该处理单元还未进行射线追踪
                {
                    this.TracingUnitRay(units[i], tx, ter, rxBall, cityBuilding,txFrequencyBand);//射线追踪
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
                            this.GetPathOfUnit(divideUnits, pathInfo, tx, ter, rxBall,cityBuilding, divideAngle,txFrequencyBand); //追踪细分三角形内的射线
                        }
                    }
                }
            }
        }

        /// <summary>
        ///初始阶段在每个三角形上发出一定的射线
        /// </summary>
        /// <param name="tx">发射机</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///   <param name="unit">射线处理单元</param>
        ///  <param name="TessellationFrequency">镶嵌次数，等于三角形每条边的细分次数</param>
        ///  <param name="divideAngle">射线束夹角</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private List<RayTracingModel> GetInitialTrianglePath(Node tx, Terrain ter, ReceiveBall rxBall, City cityBuilding, RayTracingModel unit, int TessellationFrequency, double divideAngle, List<FrequencyBand> txFrequencyBand)
        {
            Point vertex1 = new Point(unit.FirstRay.SVector);//三角形的顶点
            Point vertex2 = new Point(unit.SecondRay.SVector);//三角形的顶点
            Point vertex3 = new Point(unit.ThirdRay.SVector);//三角形的顶点
            SpectVector vector12 = new SpectVector(vertex1, vertex2);
            SpectVector vector23 = new SpectVector(vertex2, vertex3);
            double length = vertex1.GetDistance( vertex2) / TessellationFrequency;//每段的距离
            Point[] pointOfEdge12 = new Point[TessellationFrequency]; //存放棱上的点,不包括顶点1
            //    p1[0] = vertex1;
            for (int i = 0; i < TessellationFrequency; i++)
            {
                pointOfEdge12[i] = 
                    new Point(vertex1.X + (i + 1) * (vector12.a / Math.Sqrt(Math.Pow(vector12.a, 2) + Math.Pow(vector12.b, 2) + Math.Pow(vector12.c, 2)) * length),
                        vertex1.Y + (i + 1) * (vector12.b / Math.Sqrt(Math.Pow(vector12.a, 2) + Math.Pow(vector12.b, 2) + Math.Pow(vector12.c, 2)) * length),
                        vertex1.Z + (i + 1) * (vector12.c / Math.Sqrt(Math.Pow(vector12.a, 2) + Math.Pow(vector12.b, 2) + Math.Pow(vector12.c, 2)) * length));

            }
            List<Point>[] vertexPoints = new List<Point>[TessellationFrequency + 1];//存放三角形内每条平行边上的点
            vertexPoints[0] = new List<Point> { vertex1 };//把顶点1放到数组第一位
            for (int j = 1; j <= TessellationFrequency; j++)//得到三角形切分后每条边上的点
            {
                vertexPoints[j] = new List<Point>();
                vertexPoints[j].Add(pointOfEdge12[j-1]);
                this.GetTriangleInternalEdgePoint(vector23, pointOfEdge12[j - 1], length, j, vertexPoints[j]);
            }
            List<ClassNewRay>[] raysPath = this.GetEachTriangleRay(tx,ter, rxBall,cityBuilding, TessellationFrequency, divideAngle,txFrequencyBand, vertexPoints);//根据得到的点构造射线
            List<RayTracingModel> triangleUnits = this.HandleInitialRayToUnit(raysPath);//得到初始追踪完的三角形处理单元
            return triangleUnits;
        }

        /// <summary>
        ///得到三角形切分后每条边上的点，但不包括初始端点
        /// </summary>
        /// <param name="vector">方向向量</param>
        /// <param name="leftPoint">左端点</param>
        /// <param name="length">长度</param>
        ///  <param name="TessellationFrequency">镶嵌次数，等于三角形每条边的细分次数</param>
        ///  <param name="divideAngle">射线束夹角</param>
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
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///   <param name="unit">射线处理单元</param>
        ///  <param name="TessellationFrequency">镶嵌次数，等于三角形每条边的细分次数</param>
        ///  <param name="divideAngle">射线束夹角</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private List<ClassNewRay>[] GetEachTriangleRay(Node tx,Terrain ter, ReceiveBall rxBall,City cityBuilding, int TessellationFrequency,double divideAngle,List<FrequencyBand> txFrequencyBand, List<Point>[] vertexPoints)
        {
            List<ClassNewRay>[] raysPath = new List<ClassNewRay>[TessellationFrequency + 1];
            for (int m = 0; m < vertexPoints.Length; m++)
            {
                raysPath[m] = new List<ClassNewRay>();
                for (int n = 0; n < vertexPoints[m].Count; n++)
                {
                    ClassNewRay temp = new ClassNewRay();//每个点发射一条射线，并进行追踪
                    temp.Origin = tx.Position;
                    SpectVector paramVector = new SpectVector(vertexPoints[m][n].X, vertexPoints[m][n].Y, vertexPoints[m][n].Z);
                    RayInfo paramRay =new RayInfo(tx.Position, paramVector);
                    List<Path> path = GetSingleRayPaths(tx,ter, rxBall,cityBuilding, paramRay, divideAngle,txFrequencyBand);
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
        ///将初始三角形上的各条射线分成处理单元
        /// </summary>
        protected List<RayTracingModel> HandleInitialRayToUnit(List<ClassNewRay>[] raysPath)
        {
            try
            {
                List<RayTracingModel> triangleUnits = new List<RayTracingModel>();
                for (int i = 0; i < raysPath.Length - 1; i++)
                {
                    for (int j = 0; j < raysPath[i].Count; j++)
                    {
                        RayTracingModel param = new RayTracingModel(raysPath[i][j], raysPath[i + 1][j], raysPath[i + 1][j + 1]);
                        param.HaveTraced = true;
                        triangleUnits.Add(param);
                    }
                }
                if (raysPath.Length > 2)//若三角形平分大于2层
                {
                    for (int m = raysPath.Length - 1; m > 2; m--)
                    {
                        for (int n = 1; n < raysPath[m].Count - 1; n++)
                        {
                            RayTracingModel param = new RayTracingModel(raysPath[m][n], raysPath[m-1][n-1], raysPath[m - 1][n]);
                            param.HaveTraced = true;
                            triangleUnits.Add(param);
                        }
                    }
                }

                return triangleUnits;
            }
            catch(Exception)
            {
                LogFileManager.ObjLog.error("在分配初始射线成处理单元时出错");
                return new List<RayTracingModel>();
            }
        }


        /// <summary>
        ///提取路径
        /// </summary>
        protected void ExtractPathOfUnit(RayTracingModel unit, List<Path> pathInfo)
        {
            if (unit.FirstRay.Path != null && unit.FirstRay.Path.Count != 0)
            {
                pathInfo.AddRange(unit.FirstRay.Path);
            }
            if (unit.SecondRay.Path != null && unit.SecondRay.Path.Count != 0)
            {
                pathInfo.AddRange(unit.SecondRay.Path);
            }
            if (unit.ThirdRay.Path != null && unit.ThirdRay.Path.Count != 0)
            {
                pathInfo.AddRange(unit.ThirdRay.Path); 
            }
        }

        /// <summary>
        ///细分处理单元
        /// </summary>
        protected List<RayTracingModel> DivideRayUnit(RayTracingModel unit)
        {
            if (unit == null)
            { 
               LogFileManager.ObjLog.error("在细分处理单元时，输入的处理单元为null");
               return new List<RayTracingModel>();
            }
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            //3条射线都没有到达接收机
            if (unit.FirstRay.Flag == false && unit.SecondRay.Flag == false && unit.ThirdRay.Flag == false)
            { return divideUnits;  }
            //只有射线1到达接收机
            else if (unit.FirstRay.Flag == true && unit.SecondRay.Flag == false && unit.ThirdRay.Flag == false)
            {
                 divideUnits = GetFirstRayUnits(unit);
                return divideUnits;
            }
            //只有射线2到达接收机
            else if (unit.FirstRay.Flag == false && unit.SecondRay.Flag == true && unit.ThirdRay.Flag == false)
            {
                 divideUnits = GetSecongRayUnits(unit);
                 return divideUnits;
            }
            //只有射线3到达接收机
            else if (unit.FirstRay.Flag == false && unit.SecondRay.Flag == false && unit.ThirdRay.Flag == true)
            {
                 divideUnits = GetThirdRayUnits(unit);
                 return divideUnits;
            }
            //射线1和射线2到达接收机
            else if (unit.FirstRay.Flag == true && unit.SecondRay.Flag == true && unit.ThirdRay.Flag == false)
            {
                 divideUnits = GetFirstAndSecondRaysUnits(unit);
                 return divideUnits;
            }
            //射线1和射线3到达接收机
            else if (unit.FirstRay.Flag == true && unit.SecondRay.Flag == false && unit.ThirdRay.Flag == true)
            {
                 divideUnits = GetFirstAndThirdRaysUnits(unit);
                 return divideUnits;
            }
            //射线2和射线3到达接收机
            else if (unit.FirstRay.Flag == false && unit.SecondRay.Flag == true && unit.ThirdRay.Flag == true)
            {
                 divideUnits = GetSecongdAndThirdRaysUnits(unit);
                 return divideUnits;
            }
            else 
            {
                divideUnits = GetAllRaysUnits(unit);
                return divideUnits;
            }

        }

        /// <summary>
        ///获取射线1的细分单元
        /// </summary>
        protected List<RayTracingModel> GetFirstRayUnits(RayTracingModel unit)
        {
            ClassNewRay ray1 = (ClassNewRay)unit.FirstRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.FirstRay.Origin,  GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray1, ray4, ray5));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            divideUnits.Add(new RayTracingModel(unit.SecondRay,ray4,ray6,true));
            divideUnits.Add(new RayTracingModel(unit.ThirdRay, ray5, ray6,true));
            return divideUnits;
        }

        /// <summary>
        ///获取射线2的细分单元
        /// </summary>
        protected List<RayTracingModel> GetSecongRayUnits(RayTracingModel unit)
        {
            ClassNewRay ray2 = (ClassNewRay)unit.SecondRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.SecondRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.SecondRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.SecondRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray2, ray4, ray6));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            divideUnits.Add(new RayTracingModel(unit.FirstRay, ray4, ray5, true));
            divideUnits.Add(new RayTracingModel(unit.ThirdRay, ray5, ray6, true));
            return divideUnits;
        }

        /// <summary>
        ///获取射线3的细分单元
        /// </summary>
        protected List<RayTracingModel> GetThirdRayUnits(RayTracingModel unit)
        {
            ClassNewRay ray3 = (ClassNewRay)unit.ThirdRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray3, ray5, ray6));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            divideUnits.Add(new RayTracingModel(unit.FirstRay, ray4, ray5, true));
            divideUnits.Add(new RayTracingModel(unit.SecondRay, ray4, ray6, true));
            return divideUnits;
        }

        /// <summary>
        ///获取射线1和射线2的细分单元
        /// </summary>
        protected List<RayTracingModel> GetFirstAndSecondRaysUnits(RayTracingModel unit)
        {
            ClassNewRay ray1 = (ClassNewRay)unit.FirstRay.Clone();
            ClassNewRay ray2 = (ClassNewRay)unit.SecondRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray1, ray4, ray5));
            divideUnits.Add(new RayTracingModel(ray2, ray4, ray6));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            divideUnits.Add(new RayTracingModel(unit.ThirdRay, ray5, ray6, true));
            return divideUnits;
        }

        /// <summary>
        ///获取射线1和射线3的细分单元
        /// </summary>
        protected List<RayTracingModel> GetFirstAndThirdRaysUnits(RayTracingModel unit)
        {
            ClassNewRay ray1 = (ClassNewRay)unit.FirstRay.Clone();
            ClassNewRay ray3 = (ClassNewRay)unit.ThirdRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray1, ray4, ray5));
            divideUnits.Add(new RayTracingModel(ray3, ray5, ray6));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            divideUnits.Add(new RayTracingModel(unit.SecondRay, ray4, ray6, true));
            return divideUnits;
        }

        /// <summary>
        ///获取射线2和射线3的细分单元
        /// </summary>
        protected List<RayTracingModel> GetSecongdAndThirdRaysUnits(RayTracingModel unit)
        {
            ClassNewRay ray2 = (ClassNewRay)unit.SecondRay.Clone();
            ClassNewRay ray3 = (ClassNewRay)unit.ThirdRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.ThirdRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray2, ray4, ray6));
            divideUnits.Add(new RayTracingModel(ray3, ray5, ray6));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            divideUnits.Add(new RayTracingModel(unit.FirstRay, ray4, ray5, true));
            return divideUnits;
        }

        /// <summary>
        ///获取三条射线的细分单元
        /// </summary>
        protected List<RayTracingModel> GetAllRaysUnits(RayTracingModel unit)
        {
            ClassNewRay ray1 = (ClassNewRay)unit.FirstRay.Clone();
            ClassNewRay ray2 = (ClassNewRay)unit.SecondRay.Clone();
            ClassNewRay ray3 = (ClassNewRay)unit.ThirdRay.Clone();
            ClassNewRay ray4 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.SecondRay.SVector), false);
            ClassNewRay ray5 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.FirstRay.SVector, unit.ThirdRay.SVector), false);
            ClassNewRay ray6 = new ClassNewRay(unit.FirstRay.Origin, GetMiddleVectorOfTwoVectors(unit.SecondRay.SVector, unit.ThirdRay.SVector), false);
            List<RayTracingModel> divideUnits = new List<RayTracingModel>();
            divideUnits.Add(new RayTracingModel(ray1, ray4, ray5));
            divideUnits.Add(new RayTracingModel(ray2, ray4, ray6));
            divideUnits.Add(new RayTracingModel(ray3, ray5, ray6));
            divideUnits.Add(new RayTracingModel(ray4, ray5, ray6));
            return divideUnits;
        }

        /// <summary>
        ///求两个向量的中间向量
        /// </summary>
        protected SpectVector GetMiddleVectorOfTwoVectors(SpectVector vector1, SpectVector vector2)
        {
            SpectVector unitVector1 = vector1.GetNormalizationVector();
            SpectVector unitVector2 = vector2.GetNormalizationVector();
             return new SpectVector((unitVector1.a+unitVector2.a)/2,(unitVector1.b+unitVector2.b)/2,(unitVector1.c+unitVector2.c)/2);
        }

        /// <summary>
        ///追踪处理单元中的射线
        /// </summary>
        private void TracingUnitRay(RayTracingModel unit, Node tx, Terrain ter, ReceiveBall rxBall, City cityBuilding, List<FrequencyBand> txFrequencyBand)
        {
            double rayBeamAngle = unit.AngleOfRayBeam * Math.PI / 180;
            //追踪射线1
            if (unit.FirstRay.WhetherHaveTraced == false)//若该射线之前还没有追踪
            {
                RayInfo newRay = new RayInfo(unit.FirstRay.Origin, unit.FirstRay.SVector);
                List<Path> pathtemp = this.GetSingleRayPaths(tx, ter, rxBall, cityBuilding, newRay, rayBeamAngle, txFrequencyBand);
                unit.FirstRay.Flag = this.JudgeIfArriveRx(pathtemp);
                unit.FirstRay.Path = pathtemp;
                unit.FirstRay.WhetherHaveTraced = true;
            }
            //追踪射线2
            if (unit.SecondRay.WhetherHaveTraced == false)//若该射线之前还没有追踪
            {
                RayInfo newRay = new RayInfo(unit.SecondRay.Origin, unit.SecondRay.SVector);
                List<Path> pathtemp = this.GetSingleRayPaths(tx, ter, rxBall, cityBuilding, newRay, rayBeamAngle, txFrequencyBand);
                unit.SecondRay.Flag = this.JudgeIfArriveRx(pathtemp);
                unit.SecondRay.Path = pathtemp;
                unit.FirstRay.WhetherHaveTraced = true;
            }
            //追踪射线3
            if (unit.ThirdRay.WhetherHaveTraced == false)//若该射线之前还没有追踪
            {
                RayInfo newRay =  new RayInfo(unit.ThirdRay.Origin, unit.ThirdRay.SVector);
                List<Path> pathtemp = this.GetSingleRayPaths(tx, ter, rxBall,cityBuilding, newRay, rayBeamAngle,txFrequencyBand);
                unit.ThirdRay.Flag = this.JudgeIfArriveRx(pathtemp);
                unit.ThirdRay.Path = pathtemp;
                unit.FirstRay.WhetherHaveTraced = true;
            }
            unit.HaveTraced = true;
            return;
        }

        /// <summary>
        ///求从辐射源发出的一条射线的所有路径
        /// </summary>
        private List<Path> GetSingleRayPaths(Node tx, Terrain ter, ReceiveBall rxBall, City cityBuilding, RayInfo inRay, double rayBeamAngle, List<FrequencyBand> txFrequencyBand)
        {
  //          Point test = new AdjacentEdge(new Point(-1111.08, -3527.01, 5000), new Point(-1111.08, 5563.65, 5000), new SpectVector(new Point(-1111.08, -3527.01, 5000), new Point(-1111.08, 5563.65, 5000))).GetDiffractionPoint(tx.Position,rxBall.Receiver);
  //          inRay.RayVector = new SpectVector(tx.Position,test);
            PunctiformPath pathTracing = new PunctiformPath(inRay,ter);
            pathTracing.SetPunctiformRayPathNodes(tx, ter, rxBall, cityBuilding, rayBeamAngle,txFrequencyBand);
            List<Path> temp = new List<Path>();
            if (tx.ChildNodes.Count != 0)
            { 
                temp = this.GetLegalPaths(tx);
                tx.ChildNodes.Clear();
            }
            return temp;

        }

        /// <summary>
        ///判断Path的list是否为空
        /// </summary>
        protected bool JudgeIfArriveRx(List<Path> temp)
        {
            if (temp.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///提取有效地路径
        /// </summary>
        private  List<Path> GetLegalPaths(Node root)
        {
            //要再精细筛选，路径所经过的面全部相同的要忽略
            List<Path> LegalPath = new List<Path>();
            List<Path> allpath = GetAllPaths(root);
            for (int i = allpath.Count-1; i >=0; i--)
            {
                if (allpath[i].Rxnum == 0)
                {
                    allpath.RemoveAt(i);
                }
            }
            return allpath;
        }

        /// <summary>
        ///根据根节点及其子节点的信息，获得路径
        /// </summary>
        private void GetPaths(Node root, List<Path> AllPaths, List<Node> nodes)
        {
            nodes.Add(root);
            if (root.ChildNodes.Count!=0)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    this.GetPaths(root.ChildNodes[i], AllPaths, new List<Node>(nodes));
                }
                root.ChildNodes.Clear();
            }
            else
            {
                AllPaths.Add(new Path(nodes));
            }
        }

        /// <summary>
        ///提取所有的路径
        /// </summary>
        protected  List<Path> GetAllPaths(Node root)
        {
            List<Path> Allpaths = new List<Path>();
            List<Node> nodes = new List<Node>();
            this.GetPaths(root, Allpaths, nodes);
            return Allpaths;
        }

        #region 传统静态发射发射，主要拿来比较
        /// <summary>
        ///全向静态发射射线
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TessellationFrequency">细分点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private List<Path> LaunchRaysOmnidirectional(Node tx, ReceiveBall rxBall, Terrain ter, City cityBuilding, int TessellationFrequency, List<FrequencyBand> txFrequencyBand)
        {
            RayInfo directRay = new RayInfo(tx.Position, rxBall.Receiver);
            Node directCrossNode = directRay.GetCrossNodeWithTerrainRects(ter.lineRect(new RayInfo(rxBall.Receiver, new SpectVector(tx.Position, rxBall.Receiver))));
            if (directCrossNode != null && directCrossNode.DistanceToFrontNode < tx.Position.GetDistance(rxBall.Receiver))//若直射被阻挡
            {
                DirectCrossNode = directCrossNode;
            }
            List<Path> pathInfo = new List<Path>();
            List<RayTracingModel> originUnits = GetOriginUnitsOfIcosahedron(tx);
            this.HandleEachSurface(originUnits, pathInfo, tx, rxBall, ter, cityBuilding, TessellationFrequency, txFrequencyBand);
            return pathInfo;
        }

        /// <summary>
        ///全向静态发射射线，对每个三角形进行追踪
        /// </summary>
        /// <param name="units">处理单元</param>
        /// <param name="pathInfo">路径</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TessellationFrequency">细分点个数</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private void HandleEachSurface(List<RayTracingModel> units, List<Path> pathInfo, Node tx, ReceiveBall rxBall, Terrain ter, City cityBuilding, int TessellationFrequency, List<FrequencyBand> txFrequencyBand)
        {
            double PA = 4 / (Math.Sqrt(10 + 2 * Math.Sqrt(5)));
            double AW = (PA / 2) / Math.Sin(36 * Math.PI / 180);
            double divideAngle = Math.Asin(0.5 * AW / TessellationFrequency);
            for (int num = 0; num < units.Count; num++)
            {
                List<Point> vertex = GetSubDivisionPoints(units[num],TessellationFrequency);
                for (int i = 0; i < vertex.Count; i++)
                {
                    RayInfo paramRay = new RayInfo(tx.Position, new SpectVector(vertex[i].X, vertex[i].Y, vertex[i].Z));
                    List<Path> path = this.GetSingleRayPaths(tx, ter, rxBall, cityBuilding, paramRay, divideAngle, txFrequencyBand);
                    if ( path.Count != 0)
                    {
                        pathInfo.AddRange(path);
                    }
                }
            }
        }

        /// <summary>
        ///获取每个三角形细分的顶点
        /// </summary>
        protected List<Point> GetSubDivisionPoints(RayTracingModel unit, int TessellationFrequency)
        {
            List<Point> vertex = new List<Point>();
            Point param1 = new Point(unit.FirstRay.SVector);
            Point param2 = new Point(unit.SecondRay.SVector);
            Point param3 = new Point(unit.ThirdRay.SVector);
            vertex.Add(param1);
            vertex.Add(param2);
            vertex.Add(param3);
            SpectVector n1 = new SpectVector(param1, param2);
            SpectVector n2 = new SpectVector(param1, param3);
            double length = param1.GetDistance(param2) / TessellationFrequency;
            Point[] p1 = new Point[TessellationFrequency];
            Point[] p2 = new Point[TessellationFrequency];
            for (int i = 0; i < TessellationFrequency; i++)
            {
                p1[i] = new Point(param1.X + (i + 1) * (n1.a / Math.Sqrt(Math.Pow(n1.a, 2) + Math.Pow(n1.b, 2) + Math.Pow(n1.c, 2)) * length), param1.Y + (i + 1) * (n1.b / Math.Sqrt(Math.Pow(n1.a, 2) + Math.Pow(n1.b, 2) + Math.Pow(n1.c, 2)) * length), param1.Z + (i + 1) * (n1.c / Math.Sqrt(Math.Pow(n1.a, 2) + Math.Pow(n1.b, 2) + Math.Pow(n1.c, 2)) * length));
                p2[i] = new Point(param1.X + (i + 1) * (n2.a / Math.Sqrt(Math.Pow(n2.a, 2) + Math.Pow(n2.b, 2) + Math.Pow(n2.c, 2)) * length), param1.Y + (i + 1) * (n2.b / Math.Sqrt(Math.Pow(n2.a, 2) + Math.Pow(n2.b, 2) + Math.Pow(n2.c, 2)) * length), param1.Z + (i + 1) * (n2.c / Math.Sqrt(Math.Pow(n2.a, 2) + Math.Pow(n2.b, 2) + Math.Pow(n2.c, 2)) * length));
               vertex.Add(p1[i]);
               vertex.Add(p2[i]);
            }
            for (int j = 0; j < TessellationFrequency; j++)
            {
                SpectVector m = new SpectVector(p1[j], p2[j]);
                GetTrianglePoint(m, p1[j], p2[j], length, vertex);
            }
            return vertex;
        }

        /// <summary>
        ///获取三角形内每条平行边上的点
        /// </summary>
        protected void GetTrianglePoint(SpectVector m,Point p1, Point p2,double length, List<Point> vertex)
        {
            Point temp = new Point(p1.X + (m.a / Math.Sqrt(Math.Pow(m.a, 2) + Math.Pow(m.b, 2) + Math.Pow(m.c, 2)) * length), p1.Y + (m.b / Math.Sqrt(Math.Pow(m.a, 2) + Math.Pow(m.b, 2) + Math.Pow(m.c, 2)) * length), p1.Z + (m.c / Math.Sqrt(Math.Pow(m.a, 2) + Math.Pow(m.b, 2) + Math.Pow(m.c, 2)) * length));
            vertex.Add(temp);
            if (Math.Abs(temp.GetDistance(p2)) <= 0.0000001)
            { return; }
            else
            { GetTrianglePoint(m, temp, p2, length, vertex); }
        }
        #endregion


        /// <summary>
        /// 根据不同的频率进行分类和场强，功率的计算
        /// </summary>
        ///  <param name="paths">路径</param>
        ///  <param name="TxFrequencyBand">频段信息</param>
        ///  <param name="multiple">绕射距离倍数</param>
        /// <returns>筛选后的路径</returns>
        public List<List<Path>> ScreenPunctiformPathsByFrequencyAndCalculateEField( List<FrequencyBand> TxFrequencyBand, int multiple)
        {
            List<List<Path>> classifiedPath = new List<List<CalculateModelClasses.Path>>();
            if (this.rayPaths.Count == 0)//若没有射线，加入空的链表到输出结果中
            {
                for (int c = 0; c < TxFrequencyBand.Count; c++)
                { classifiedPath.Add(new List<Path>()); }
            }
            else
            {
                for (int i = 0; i < TxFrequencyBand.Count; i++)//对频段进行遍历
                {
                    double freDistance = multiple * 300 / TxFrequencyBand[i].MidPointFrequence;//波长的倍数，是判断发生绕射的条件
                    List<Path> tempPath = new List<CalculateModelClasses.Path>();
                    for (int j = 0; j < this.rayPaths.Count; j++)
                    {
                        if (JudgeIfDiffractionHappen(this.rayPaths[j], freDistance))//若发生
                        {
                            List<Node> pathNodes = new List<Node>();
                            for (int n = 0; n < this.rayPaths[j].node.Count; n++)//对该射线除了第一个Tx点外的节点进行计算
                            {
                                Node temp = new Node(this.rayPaths[j].node[n]);

                                if (n == 0)
                                {
                                    temp.Power = TxFrequencyBand[i].Power;
                                }
                                temp.Frequence = TxFrequencyBand[i].MidPointFrequence;
                                temp.FrequenceWidth = TxFrequencyBand[i].FrequenceWidth;
                                if (n != 0)
                                {
                                    this.GetEfield(temp.UAN, pathNodes[n - 1], ref temp);//计算场强
                                    //接收点求功率,有问题，电导率和节点常数是面的还是空气的
                                    temp.Power = Power.GetPower(temp, pathNodes[n - 1])[0];//计算功率
                                }
                                pathNodes.Add(temp);
                            }
                            Path midpath = new CalculateModelClasses.Path(pathNodes);

                            if (midpath.Rxnum != 0)
                            {
                                GetLossAndComponentOFPath(midpath);
                                tempPath.Add(midpath);
                            }
                        }
                    }
                    classifiedPath.Add(tempPath);
                    
                }
            }
            return classifiedPath;
        }


        /// <summary>
        /// 计算路径的损耗，延时，相位
        /// </summary>
        protected  void GetLossAndComponentOFPath(Path midpath)
        {
            midpath.pathloss = 10 * Math.Log10(midpath.node[0].Power / midpath.node[midpath.node.Count - 1].Power);
            midpath.Delay = midpath.GetPathLength() / 300000000;
            midpath.thetaa = ReadUan.GetThetaAngle(midpath.node[0].Position, midpath.node[1].Position);
            midpath.thetab = ReadUan.GetThetaAngle(midpath.node[midpath.node.Count - 2].Position, midpath.node[midpath.node.Count - 1].Position);
            midpath.phia = ReadUan.GetPhiAngle(midpath.node[0].Position, midpath.node[1].Position);
            midpath.phib = ReadUan.GetPhiAngle(midpath.node[midpath.node.Count - 2].Position, midpath.node[midpath.node.Count - 1].Position);

        }

        /// <summary>
        /// 判断该绕射射线在该频段下是否发生
        /// </summary>
        ///  <param name="path">路径</param>
        ///  <param name="frequenceDistance">绕射发生判断距离</param>
        /// <returns>该射线产生的路径</returns>
        protected bool JudgeIfDiffractionHappen(Path path, double frequenceDistance)
        {
            if (path.node.Count == 2)//两个点为直射射线
            {
                return true;
            }
            else
            {
                for (int i = 1; i < path.node.Count; i++)
                {
                    if (path.node[i].NodeStyle == NodeStyle.DiffractionNode) //该点是绕射点
                    {
                        if (path.node[i].FrontDiffractionNode.Count != 0)//若该绕射点前还有别的绕射点,当射线在之前的绕射点已经发生绕射，则该路径不存在
                        {
                            for (int j = 0; j < path.node[i].FrontDiffractionNode.Count; j++)
                            {
                                if (path.node[i].FrontDiffractionNode[j].DisranceToEdge < frequenceDistance)//若该射线在这个绕射点之前的绕射点已经发生绕射，则该射线不存在
                                {
                                    return false;
                                }
                            }
                        }
                        if (path.node[i].DisranceToEdge > frequenceDistance) //到绕射边的距离大于该频率下的最大距离，则该射线在该频率下不发生
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        ///根据点类型获得电场值
        /// </summary>
        protected  void GetEfield(string uan, Node fatherNode, ref Node childNode)
        {
            switch (fatherNode.NodeStyle)
            {
                case NodeStyle.Tx:
                    //直射场强计算
                    childNode.TotalE = DirectEfieldCal.EfieldCal(uan, fatherNode.Power, fatherNode.Frequence, fatherNode.Position, childNode.Position);
                    break;
                case NodeStyle.ReflectionNode:
                    childNode.TotalE = ReflectEfieldCal.ReflectEfield(fatherNode.TotalE, fatherNode.RayIn, childNode.RayIn, fatherNode.ReflectionFace.NormalVector, fatherNode.ReflectionFace.Material.DielectricLayer[0].Conductivity, fatherNode.ReflectionFace.Material.DielectricLayer[0].Permittivity, fatherNode.DistanceToFrontNode, childNode.DistanceToFrontNode, fatherNode.Frequence);
                    break;
                case NodeStyle.DiffractionNode:
                    childNode.TotalE = DiffractEfiledCal.GetDiffractionEField(fatherNode.TotalE, fatherNode.RayIn, fatherNode.DiffractionEdge.AdjacentTriangles[0], fatherNode.DiffractionEdge.AdjacentTriangles[1], fatherNode.Position, childNode.Position, childNode.Frequence);
                    break;
                default:
                    break;
            }
        }

    }

   
}
