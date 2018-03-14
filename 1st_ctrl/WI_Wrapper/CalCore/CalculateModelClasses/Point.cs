using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogFileManager;

namespace CalculateModelClasses
{
    /// <summary>
    ///点的信息
    /// </summary>
    public class Point : ICloneable
    {
        //用于判断
        protected bool isInDirect = true;
        protected double x;
        protected double y;
        protected double z;
        protected ReflectionPointAndFace pfd;
        protected bool isInTer = false;

        public bool IsInDirect
        {
            get { return isInDirect; }
            set { isInDirect = value; }
        }
        public bool IsInTer
        {
            get { return isInTer; }
            set { isInTer = value; }
        }
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public double Z
        {
            get { return z; }
            set { z = value; }
        }
        public ReflectionPointAndFace Pfd
        {
            get { return pfd; }
            set { pfd = value; }
        }

        public Point(SpectVector vector)
        {
            this.x = vector.a;
            this.y = vector.b;
            this.z = vector.c;
        }
        public Point(Point currentPoint)
        {
            this.x = currentPoint.x;
            this.y = currentPoint.y;
            this.z = currentPoint.z;
        }

        public Point(double a = 0, double b = 0, double c = 0)
        {
            x = a;
            y = b;
            z = c;
        }

        /// <summary>
        ///判断两个点是否不相同
        /// </summary>
        public bool unequal(Point one)
        {
            if (this.equal(one))
            { return false; }
            else
            { return true; }
        }

        /// <summary>
        ///判断两个点是否相同
        /// </summary>
        public bool equal(Point otherPoint)
        {
            if (Math.Abs(this.x - otherPoint.X) < 0.000001 && Math.Abs(this.y - otherPoint.Y) < 0.000001 && Math.Abs(this.z - otherPoint.Z) < 0.000001)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        ///判断两个点在XY平面的投影是否相同
        /// </summary>
        public bool IsEqualInXYProjection(Point otherPoint)
        {
            if (Math.Abs(this.x - otherPoint.X) < 0.000001 && Math.Abs(this.y - otherPoint.Y) < 0.000001)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// 判断点是否在线段在XY平面投影内
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>点是否在XY平面线段上的布尔值</returns>
        public void AdjustXYZDemicalBitSameAsViewPoint(Point viewPoint)
        {
            int xBit = this.GetBitAfterDecimalPointInDoubleValue(viewPoint.X);
            this.x = Math.Round(this.x, xBit);
            int yBit = this.GetBitAfterDecimalPointInDoubleValue(viewPoint.Y);
            this.y = Math.Round(this.y, yBit);
            int zBit = this.GetBitAfterDecimalPointInDoubleValue(viewPoint.Z);
            this.z = Math.Round(this.z, zBit);
        }


        /// <summary>
        /// 判断点是否在线段在XY平面投影内
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <param name="line">线段</param>
        /// <returns>点是否在XY平面线段上的布尔值</returns>
        public bool JudgeIfPointInLineInXY(LineSegment line)
        {
            if ((this.x == line.StartPoint.X && this.y == line.StartPoint.Y) || (this.x == line.EndPoint.X && this.y == line.EndPoint.Y))
            { return true; }
            else
            {
                SpectVector point1ToviewPoint = new SpectVector(this.x - line.StartPoint.X, this.y - line.StartPoint.Y, 0);
                if (point1ToviewPoint.IsParallelAndSamedirectionInXYPlane(line.LineVector))
                {
                    if (GetDistanceInXY(line.StartPoint) <= line.StartPoint.GetDistanceInXY(line.EndPoint))
                    { return true; }
                }
            }
            return false;
        }

        /// <summary>
        ///获取两个点的距离
        /// </summary>
        public double GetDistance(Point anotherPoint)
        {
            return Math.Sqrt(Math.Pow((this.X - anotherPoint.X), 2) + Math.Pow((this.Y - anotherPoint.Y), 2) + Math.Pow((this.Z - anotherPoint.Z), 2));
        }


        /// <summary>
        ///获取两个点的中点
        /// </summary>
        public Point GetMiddlePointWithOtherPoint(Point otherPoint)
        {
            return new Point((this.x + otherPoint.X) / 2, (this.y + otherPoint.Y) / 2, (this.z + otherPoint.Z) / 2);
        }

        /// <summary>
        /// 判断点是否在一个棱上
        /// </summary>
        public bool isInline(AdjacentEdge edge)
        {
            if (this.equal(edge.StartPoint)) return true;
            double weight1 = (this.X - edge.StartPoint.X) / edge.LineVector.a;
            double weight2 = (this.Y - edge.StartPoint.Y) / edge.LineVector.b;
            double weight3 = (this.Z - edge.StartPoint.Z) / edge.LineVector.c;
            if (Math.Abs(edge.LineVector.a) < 0.000001) weight1 = this.X - edge.StartPoint.X + 1;
            if (Math.Abs(edge.LineVector.b) < 0.000001) weight2 = this.Y - edge.StartPoint.Y + 1;
            if (Math.Abs(edge.LineVector.c) < 0.000001) weight3 = this.Z - edge.StartPoint.Z + 1;

            if (Math.Abs(weight1 / weight2 - 1) < 10e-10 && Math.Abs(weight2 / weight3 - 1) < 10e-10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///获取两个点在XY平面投影的距离
        /// </summary>
        public double GetDistanceInXY(Point anotherPoint)
        {
            return Math.Sqrt(Math.Pow((this.x - anotherPoint.X), 2) + Math.Pow((this.y - anotherPoint.Y), 2));
        }

        /// <summary>
        /// 求点到直线的距离
        /// </summary>
        /// <param name="pointOfLine">直线上一点</param>
        /// <param name="vectorOfLine">直线的一个方向向量</param>
        /// <returns>点到直线的距离</returns>
        public double GetDistanceToLine( Point pointOfLine, SpectVector vectorOfLine)
        {
            //设直线外一点为a,直线上的一点为b
            double abDistance = this.GetDistance(pointOfLine);
            SpectVector baVector = new SpectVector(pointOfLine, this);
            double angle = SpectVector.VectorPhase(baVector, vectorOfLine);
            if ((angle > 90) && (angle < 180))
                angle = angle - 90;
            double distanceToLine = abDistance * Math.Sin((angle * Math.PI / 180));
            if (distanceToLine < 0)
            { 
                LogFileManager.ObjLog.error("点到直线的距离不可能小于0");
                return 0;
            }
            else return distanceToLine;
        }

        /// <summary>
        /// 求点到直线的距离
        /// </summary>
        /// <param name="straightLine">直线</param>
        /// <returns>点到直线的距离</returns>
        public double GetDistanceToLine(RayInfo straightLine)
        {
            return this.GetDistanceToLine(straightLine.Origin, straightLine.RayVector);
        }

        /// <summary>
        ///根据点判断判断两个相邻面的相对于点的形状是否为凸
        /// </summary>
        /// <param name="diffractionFace1">绕射面1</param>
        /// <param name="diffractionFace2">绕射面2</param>
        /// <returns></returns>
        public bool JudgeIsConcaveOrConvexToViewPoint(Face diffractionFace1, Face diffractionFace2)
        {
            AdjacentEdge sameEdge = new AdjacentEdge(diffractionFace1, diffractionFace2);
            if (sameEdge.StartPoint == null)
            {
                LogFileManager.ObjLog.error("判断判断两个相邻面的相对于点的形状时，输入的不是相邻面");
                return false;
            }
            
            Point p0 = diffractionFace1.GetPointsOutOfTheLine(sameEdge)[0];
            Point p1 = diffractionFace2.GetPointsOutOfTheLine(sameEdge)[0];
            int p0top1 = p0.IsOutFace(diffractionFace2);
            int p1top0 = p1.IsOutFace(diffractionFace1);
            int txtoface0 = this.IsOutFace(diffractionFace1);
            int txtoface1 = this.IsOutFace(diffractionFace2);
            if (p0top1 == 0 && p1top0 == 0)
            { return false; }
            else if ((txtoface0 == 0 && txtoface1 == p0top1) || (txtoface1 == 0 && txtoface0 == p1top0) || (txtoface0 == 0 && txtoface1 == 0))
            {
                LogFileManager.ObjLog.error("判断判断两个相邻面的相对于点的形状时，点在两个面的某一面内而不能发生绕射");
                return false;
            }
            else if (txtoface0 == p1top0 && txtoface1 == p0top1)
            { return false; }
            //说明两个面相对于点是凹型的
            else
            { return true; }
        }

        /// <summary>
        ///已知点的XY值和所在面求Z值的函数
        /// </summary>
        /// <param name="inFace">点所在的平面</param>
        /// <returns></returns>
        public double GetZValueByFace(Face inFace)
        {
           double[] faceEquationFactor = inFace.GetFaceEquationFactor();
            return (-faceEquationFactor[3] - faceEquationFactor[0] * this.x - faceEquationFactor[1] * this.y) / faceEquationFactor[2];

        }

        /// <summary>
        ///获取点离地高度
        /// </summary>
        /// <param name="ter">地形</param>
        /// <returns>点离地高度</returns>
        public double GetTerrainClearance(Terrain ter)
        {
            //先求出该中心点在地形矩形的行列序号
            int rowID = (int)((this.x - ter.TerRect[0, 0].TriangleOne.MinUnitX) / Terrain.UnitX);
            int lineID = (int)((this.y - ter.TerRect[0, 0].TriangleOne.MinUnitY) / Terrain.UnitY);
            //对该矩形的三角面进行遍历
            for (int i = 0; i < ter.TerRect[lineID, rowID].RectTriangles.Count; i++)
            {
                if (ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle.Count == 0)//若该三角面没有被细分
                {
                    if (ter.TerRect[lineID, rowID].RectTriangles[i].JudgeIfPointInTriangleInXY(this))//判断中心点是否在该三角面的XY投影上
                    {
                        double Zvalue = this.GetZValueByFace(ter.TerRect[lineID, rowID].RectTriangles[i]);//求中心点的Z坐标
                        return Math.Abs(this.z-Zvalue);
                    }
                }
                else
                {
                    for (int j = 0; j < ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle.Count; j++)//对细分三角面进行遍历
                    {
                        if (ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle[j].JudgeIfPointInTriangleInXY(this))//判断中心点是否在该三角面的XY投影上
                        {
                            double Zvalue = GetZValueByFace(ter.TerRect[lineID, rowID].RectTriangles[i].SubdivisionTriangle[j]);//求中心点的Z坐标
                            return Math.Abs(this.z - Zvalue);
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        ///求点到平面的距离
        /// </summary>
        /// <param name="viewFace">平面</param>
        /// <returns>点到平面的距离</returns>
        public double  GetDistanceToFace(Face viewFace)
        {
            double thi=viewFace.NormalVector.GetPhaseOfVector(new SpectVector(viewFace.Vertices[0],this));
            if(thi>90)
            {
                thi=180-thi;
            }
            return this.GetDistance(viewFace.Vertices[0]) * Math.Cos(thi * Math.PI / 180);

        }

        /// <summary>
        ///根据发射机，接收机，绕射棱的位置求出发射机射向绕射点的位置
        /// </summary>
        ///  <param name="otherPoint">另一个点</param>
        ///  <param name="diffractionEdge">绕射棱</param>
        /// <returns>绕射点</returns>
        public Point GetDiffractionPostion(Point otherPoint, AdjacentEdge diffractionEdge)
        {
            double h1 = diffractionEdge.getDistanceP2Line(this);
            double h2 = diffractionEdge.getDistanceP2Line(otherPoint);
            if (h1 == 0 || h2 == 0)
            {
                return null;
            }
            if (h1 == h2)
            {
                Point originPedal = diffractionEdge.GetPedalPoint(this);
                Point targerPedal = diffractionEdge.GetPedalPoint(otherPoint);
                Point diffractPoint = new Point((originPedal.X + targerPedal.X) / 2, (originPedal.Y + targerPedal.Y) / 2, (originPedal.Z + targerPedal.Z) / 2);
                if (diffractionEdge.JudgeIfPointInLineRange(diffractPoint))
                {
                    return diffractPoint;
                }
                else
                {
                    return null;
                }
            }
            double K = Math.Pow(h2, 2) - Math.Pow(h1, 2);
            double A = 2 * Math.Pow(h1, 2) * otherPoint.X - 2 * Math.Pow(h2, 2) * this.X;
            double B = 2 * Math.Pow(h1, 2) * otherPoint.Y - 2 * Math.Pow(h2, 2) * this.Y;
            double C = 2 * Math.Pow(h1, 2) * otherPoint.Z - 2 * Math.Pow(h2, 2) * this.Z;
            double D = Math.Pow(h1, 2) * (Math.Pow(otherPoint.X, 2) + Math.Pow(otherPoint.Y, 2) + Math.Pow(otherPoint.Z, 2))
                - Math.Pow(h2, 2) * (Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2));
            double M = K * (Math.Pow(diffractionEdge.LineVector.a, 2) + Math.Pow(diffractionEdge.LineVector.b, 2) + Math.Pow(diffractionEdge.LineVector.c, 2));
            double N = (2 * K * diffractionEdge.StartPoint.X + A) * diffractionEdge.LineVector.a + (2 * K * diffractionEdge.StartPoint.Y + B) * diffractionEdge.LineVector.b
                + (2 * K * diffractionEdge.StartPoint.Z + C) * diffractionEdge.LineVector.c;
            double Q = D - K * (Math.Pow(diffractionEdge.StartPoint.X, 2) + Math.Pow(diffractionEdge.StartPoint.Y, 2) + Math.Pow(diffractionEdge.StartPoint.Z, 2))
                - A * diffractionEdge.StartPoint.X - B * diffractionEdge.StartPoint.Y - C * diffractionEdge.StartPoint.Z;
            double judge = Math.Pow(N, 2) + 4 * M * Q;
            if (Math.Abs(Math.Pow(N, 2) + 4 * M * Q) < 0.00000001)
            {
                double t = -N / (2 * M);
                Point dPoint = new Point(diffractionEdge.StartPoint.X + diffractionEdge.LineVector.a * t, diffractionEdge.StartPoint.Y + diffractionEdge.LineVector.b * t,
             diffractionEdge.StartPoint.Z + diffractionEdge.LineVector.c * t);
                return dPoint;
            }
            else if (Math.Pow(N, 2) + 4 * M * Q > 0)
            {
                double t1 = (-N + Math.Sqrt(Math.Pow(N, 2) + 4 * M * Q)) / (2 * M);
                double t2 = (-N - Math.Sqrt(Math.Pow(N, 2) + 4 * M * Q)) / (2 * M);
                Point dPoint1 = new Point(diffractionEdge.StartPoint.X + diffractionEdge.LineVector.a * t1, diffractionEdge.StartPoint.Y + diffractionEdge.LineVector.b * t1,
                    diffractionEdge.StartPoint.Z + diffractionEdge.LineVector.c * t1);
                Point dPoint2 = new Point(diffractionEdge.StartPoint.X + diffractionEdge.LineVector.a * t2, diffractionEdge.StartPoint.Y + diffractionEdge.LineVector.b * t2,
                   diffractionEdge.StartPoint.Z + diffractionEdge.LineVector.c * t2);
                Point txVerticalPoint = diffractionEdge.GetPedalPoint(this);
                Point rxVerticalPoint = diffractionEdge.GetPedalPoint(otherPoint);
                if (new LineSegment(txVerticalPoint, rxVerticalPoint).JudgeIfPointInLineRange(dPoint1))
                {
                    return dPoint1;
                }
                else
                {
                    return dPoint2;
                }
            }
            return null;

        }

        /// <summary>
        ///求镜像点
        /// </summary>
        ///  <param name="mirrorFace">镜像面</param>
        /// <returns>镜像点</returns>
        public  Point GetMirrorPoint( Face mirrorFace)
        {
            if (mirrorFace.SwitchToSpaceFace().JudgeIfPointInFace(this))
            { return this; }
            else
            {

                Point crossPoint = new RayInfo(this, mirrorFace.NormalVector).GetCrossPointBetweenStraightLineAndFace(mirrorFace.SwitchToSpaceFace());
                Point mirrorPoint = new Point();
                mirrorPoint.X = 2 * crossPoint.X - this.x;
                mirrorPoint.Y = 2 * crossPoint.Y - this.y;
                mirrorPoint.Z = 2 * crossPoint.Z - this.z;
                return mirrorPoint;
            }
        }

        /// <summary>
        /// 得到一个点关于某个点的对称点
        /// </summary>
        /// <param name="diffractionPoint"></param>
        /// <returns></returns>
        public Point GetSymmetricPointOnEdge(Point diffractionPoint)
        {
            Point symmetricPoint = new Point();
            symmetricPoint.x = this.x + 2 * (diffractionPoint.x - this.x);
            symmetricPoint.y = this.y + 2 * (diffractionPoint.y - this.y);
            symmetricPoint.z = this.z + 2 * (diffractionPoint.z - this.z);
            return symmetricPoint;
        }

        /// <summary>
        ///拷贝
        /// </summary>
        public Object Clone()
        {

            Point newPoint = new Point();
            newPoint.X = this.X;
            newPoint.Y = this.Y;
            newPoint.Z = this.Z;
            newPoint.IsInTer = this.IsInTer;
            newPoint.IsInDirect = this.IsInDirect;
            newPoint.Pfd = this.Pfd;
            return newPoint;
        }

        /// <summary>
        /// 判断点是否在面的外侧
        /// </summary>
        private int IsOutFace(Face face)
        {
            double[] para = face.GetFaceEquationFactor();
            double temp = para[0] * this.x + para[1] * this.y + para[2] * this.z + para[3];
            if (temp > 0.0000001)
            { return 2; }
            else if (temp < -0.0000001)
            { return 1; }
            else
            { return 0; }
        }

        /// <summary>
        /// 获取double值小数点后的位数
        /// </summary>
        /// <param name="data">.site文件的路径</param>
        /// <returns>double值小数点后的位数</returns>
        private int GetBitAfterDecimalPointInDoubleValue(double data)
        {
            string dataString = data.ToString();
            return dataString.Length - dataString.IndexOf(".") - 1;
        }
    }

    public class AreaNode : Node
    {
        public List<Path> paths;//存储经过态势点的所有路径
        public List<List<Path>> classifiedFrequencyPaths;//存储分频后的各条路径
        public List<EField> totleEfields;//存储态势点在各个频段下的总场强
        public EField totleEfield;//存储态势点的总场强
        public List<double> totlePowers;//存储态势点在各个频段下的总功率
        public double totlePower;//存储态势点的总功率
        public AreaNode()
        {
            paths = new List<Path>();
            classifiedFrequencyPaths = new List<List<Path>>();
            totleEfields = new List<EField>();
            totleEfield = new EField();
            totlePowers = new List<double>();
            totlePower = new double();
        }
    }



    /// <summary>
    ///记录节点信息
    /// </summary>
    public class Node : ICloneable
    {
        public string UAN;//发射机或接收机的UAN文件
        public int LayNum;//该节点所在的层数，发射机节点为1
        public int DiffractionNum;//发生绕射的次数，发射机节点为0
        public int TxNum;//只有发射机节点有,若无置零
        public int RxNum;//只有到达接收点才有，如无置零
        public string NodeName;//只有发射机和接收机有名字
        public double Frequence;//该点的发射频率
        public double FrequenceWidth;//频率带宽
        public double DisranceToEdge;//绕射点对应的反射点到棱的距离
        public Point Position;//该点的位置
        public NodeStyle NodeStyle;//该点的类型
        public bool IsEnd;//当射线反射或绕射三次后仍未到达接收机处时将该点设为终点
        public bool IsReceiver = false;//当到达接收机后设置为true，此时IsEnd也设为true
        public bool IsInTer;//当节点在底面上时，为true
        public double DistanceToFrontNode;//该点与其上一点的距离
        public double RayTracingDistance;//该点之前射线所经过的距离，用于求接收球半径
        public Face ReflectionFace;//反射点时的反射面，其他点时为null
        public RayInfo RayIn;  //入射射线
        public EField TotalE;// 该点的总电场值，可用于结果显示和计算下一点的电场值
        public double Power;//节点功率
        public AdjacentEdge DiffractionEdge;//绕射棱，若发生绕射，则该属性不为空
        public Node FatherNode;
        public List<Node> ChildNodes;//该点后的子节点
        public List<Node> FrontDiffractionNode;//存放该点之前的可能发生绕射的点
        public AdjacentEdge CrossLine;//若该节点与地形三角面的棱相交则记录
        public Node tempNode;
        public double Height = 0;//用来存放节点离地高度

        public Node()
        {
            this.ChildNodes = new List<Node>();
            this.FrontDiffractionNode = new List<Node>();
        }
        public Node(Node t)
        {
            this.UAN = t.UAN;
            this.LayNum = t.LayNum;
            this.TxNum = t.TxNum;
            this.DiffractionNum = t.DiffractionNum;
            this.RxNum = t.RxNum;
            this.NodeName = t.NodeName;
            this.Frequence = t.Frequence;
            this.FrequenceWidth = t.FrequenceWidth;
            this.DisranceToEdge = t.DisranceToEdge;
            this.Position = t.Position;
            this.IsEnd = t.IsEnd;
            this.IsReceiver = t.IsReceiver;
            this.NodeStyle = t.NodeStyle;
            this.DistanceToFrontNode = t.DistanceToFrontNode;
            this.RayTracingDistance = t.RayTracingDistance;
            this.ReflectionFace = t.ReflectionFace;
            this.RayIn = t.RayIn;
            this.Height = t.Height;
            this.Power = t.Power;
            this.TotalE = t.TotalE;
            this.DiffractionEdge = t.DiffractionEdge;
            this.FatherNode = t.FatherNode;
            this.ChildNodes = t.ChildNodes;
            this.FrontDiffractionNode = t.FrontDiffractionNode;
        }

        /// <summary>
        ///判断节点是否在态势区域内
        /// </summary>
        ///  <param name="judgeNode">节点</param>
        /// <returns>节点是否在态势区域内的布尔值</returns>
        public void JudgeIfNodeIsInArea(Terrain ter,ReceiveArea reArea)
        {
            bool flag1 = this.Position.X > reArea.OriginPoint.X && this.Position.X < reArea.OriginPoint.X + reArea.rxLength;
            bool flag2 = this.Position.Y > reArea.OriginPoint.Y && this.Position.Y < reArea.OriginPoint.Y + reArea.rxWidth;
            if (!this.IsInTer && this.Height == 0)//获取节点的离地高度
            { 
                this.Height = this.Position.GetTerrainClearance(ter);
            }
            if (flag1 && flag2 && this.Height <= 5)
            {
                this.IsReceiver = true;
            }
        }


        public Object Clone()
        {
            Node newNode = new Node();
            newNode.UAN = this.UAN;
            newNode.LayNum = this.LayNum;
            newNode.TxNum = this.TxNum;
            newNode.DiffractionNum = this.DiffractionNum;
            newNode.RxNum = this.RxNum;
            newNode.NodeName = this.NodeName;
            newNode.Frequence = this.Frequence;
            newNode.FrequenceWidth = this.FrequenceWidth;
            newNode.DisranceToEdge = this.DisranceToEdge;
            newNode.Position = this.Position;
            newNode.IsEnd = this.IsEnd;
            newNode.IsReceiver = this.IsReceiver;
            newNode.NodeStyle = this.NodeStyle;
            newNode.DiffractionEdge = this.DiffractionEdge;
            newNode.DistanceToFrontNode = this.DistanceToFrontNode;
            newNode.RayTracingDistance = this.RayTracingDistance;
            newNode.ReflectionFace = this.ReflectionFace;
            newNode.RayIn = this.RayIn;
            newNode.Height = this.Height;
            if (this.Power != null)
            {
                newNode.Power = this.Power;
            }
            if (this.TotalE != null)
            {
                newNode.TotalE = (EField)this.TotalE.Clone();
            }
            newNode.FatherNode = this.FatherNode;
            newNode.ChildNodes = this.ChildNodes;
            newNode.FrontDiffractionNode = this.FrontDiffractionNode;
            return newNode;
        }
    }


    /// <summary>
    ///接收区域六个面
    /// </summary>
    public enum Side
    {
        UpSide, DownSide, FrontSide, BackSide, LeftSide, RightSide
    }

    /// <summary>
    ///节点类型
    /// </summary>
    public enum NodeStyle
    {
       Tx,   //发射机
       Rx,   //接收机
       ReflectionNode,   //发射点
       DiffractionNode,   //绕射点
       CylinderCrossNode,  //与绕射圆柱体的交点
       AreaCrossNode,  //与态势区域的交点
       VirtualNode    //虚拟的节点，最后删去
    }
}
