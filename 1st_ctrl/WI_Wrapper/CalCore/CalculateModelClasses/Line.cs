using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///线段类
    /// </summary>
    public class LineSegment
    {
        protected Point startPoint;//线段端点1
        protected Point endPoint;//线段端点2
        protected SpectVector lineVector;//线段的方向向量

        public Point StartPoint
        {
            get { return startPoint; }
            set { startPoint = value; }
        }
        public Point EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; }
        }
        public SpectVector LineVector
        {
            get { return lineVector; }
            set { lineVector = value; }
        }
        public LineSegment()
        { }
        public LineSegment(Point point1, Point point2)
        {
            startPoint = point1;
            endPoint = point2;
            lineVector = new SpectVector(point1, point2);
        }

        /// <summary>
        /// 判断两条线段是否相同
        /// </summary>
        /// <param name="otherLine">另外一条线段</param>
        /// <returns>返回两条线段是否相同的布尔值</returns>
        public bool JudgeIfTheSameLine(LineSegment otherLine)
        {
            if ((this.startPoint.equal(otherLine.startPoint) && this.endPoint.equal(otherLine.endPoint)) || (this.startPoint.equal(otherLine.endPoint) && this.endPoint.equal(otherLine.startPoint)))
            { return true; }
            else
            { return false;}
        }

        /// <summary>
        /// 求两条线段的公共点
        /// </summary>
        /// <param name="otherLine">另外一条线段</param>
        /// <returns>两条线段的公共点</returns>
        public List<Point> GetCommonPointsWithOtherLine(LineSegment otherLine)
        {
            List<Point> commonPoint = new List<Point>();
            if ((this.startPoint.equal(otherLine.StartPoint) || this.startPoint.equal(otherLine.EndPoint)))
            { commonPoint.Add(this.startPoint); }
             if ((this.endPoint.equal(otherLine.EndPoint) || this.endPoint.equal(otherLine.StartPoint)))
             {commonPoint.Add(this.endPoint);}
             return commonPoint;
        }

        /// <summary>
        /// 返回不平行线段的公共点
        /// </summary>
        /// <param name="otherLine"></param>
        /// <returns></returns>
        public Point GetCommonPointWithNotParallelLine(LineSegment otherLine)
        {
            Point commonPoint = new Point();
            if ((this.startPoint.equal(otherLine.StartPoint) || this.startPoint.equal(otherLine.EndPoint)))
                commonPoint = startPoint;
            else
                commonPoint = endPoint;
            return commonPoint;
        }

        /// <summary>
        /// 求线段的中点
        /// </summary>
        /// <returns>线段的中点</returns>
        public Point GetMiddlePoint()
        {
            return new Point((this.startPoint.X + this.endPoint.X) / 2, (this.startPoint.Y + this.endPoint.Y) / 2, (this.startPoint.Z + this.endPoint.Z) / 2);
        }


        /// <summary>
        /// 将线段转成射线
        /// </summary>
        /// <returns>射线</returns>
        public RayInfo SwitchToRay()
        {
            return new RayInfo(this.startPoint, this.endPoint);
 
        }

        /// <summary>
        /// 判断一点是否在线段内
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>返回点是否在线段内的布尔值</returns>
        public bool JudgeIfPointInLineRange(Point viewPoint)
        {
   //         if (this.SwitchToRay().JudgeIfPointIsInStraightLine(viewPoint))//点在线段所在直线上
   //         {
   //             if (((viewPoint.X >= this.startPoint.X && viewPoint.X <= this.endPoint.X) || (viewPoint.X <= this.startPoint.X && viewPoint.X >= this.endPoint.X)) &&
   //                 ((viewPoint.Y >= this.startPoint.Y && viewPoint.Y <= this.endPoint.Y) || (viewPoint.Y <= this.startPoint.Y && viewPoint.Y >= this.endPoint.Y)) &&
   //                 ((viewPoint.Z >= this.startPoint.Z && viewPoint.Z <= this.endPoint.Z) || (viewPoint.Z <= this.startPoint.Z && viewPoint.Z >= this.endPoint.Z)))
   //             { return true; }
   //         }
  //          return false;

            if (viewPoint.equal(this.startPoint) || viewPoint.equal(this.endPoint))
            { return true; }
            SpectVector vectorFromStartPointToViewPoint = new SpectVector(this.startPoint, viewPoint);
            SpectVector vectorFromViewPointToEndPoint = new SpectVector(viewPoint, this.endPoint);
            SpectVector vectorFromEndPointToViewPoint = new SpectVector(this.endPoint, viewPoint);
            SpectVector vectorFromViewPointToStartPoint = new SpectVector(viewPoint, this.startPoint);
            if (vectorFromStartPointToViewPoint.IsParallelAndSamedirection(vectorFromViewPointToEndPoint) &&
                vectorFromEndPointToViewPoint.IsParallelAndSamedirection(vectorFromViewPointToStartPoint))
            { return true; }
            else
            { return false; }
        }



        /// <summary>
        /// 判断一点在XY平面的投影是否在一条线段在xY平面的投影上
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>返回点是否在线段内的布尔值</returns>
        public bool JudgeIfPointInLineRangeInXYPlane(Point viewPoint)
        {
            if (viewPoint.IsEqualInXYProjection(this.startPoint) || viewPoint.IsEqualInXYProjection(this.endPoint))
            { return true; }
            SpectVector vectorFromStartPointToViewPoint = new SpectVector(this.startPoint, viewPoint);
            SpectVector vectorFromViewPointToEndPoint = new SpectVector(viewPoint, this.endPoint);
            SpectVector vectorFromEndPointToViewPoint = new SpectVector(this.endPoint, viewPoint);
            SpectVector vectorFromViewPointToStartPoint = new SpectVector(viewPoint, this.startPoint);
            if (vectorFromStartPointToViewPoint.IsParallelAndSamedirectionInXYPlane(vectorFromViewPointToEndPoint) &&
                vectorFromEndPointToViewPoint.IsParallelAndSamedirectionInXYPlane(vectorFromViewPointToStartPoint))
            { return true; }
            else
            { return false; }
        }


        /// <summary>
        /// 求线段与平面的交点
        /// </summary>
        /// <param name="viewFace">平面</param>
        /// <returns>线段与平面的交点</returns>
        public Point GetCrossPointWithFace(Face viewFace)
        {
            Point crossPoint = this.SwitchToRay().GetCrossPointBetweenStraightLineAndFace(viewFace);
            if (crossPoint != null && this.JudgeIfPointInLineRange(crossPoint))
            {
                return crossPoint;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 求两条线段在XY平面的投影的交点
        /// </summary>
        /// <param name="otherLine">另外一条线段</param>
        /// <returns>返回两条线段在XY平面的投影的交点</returns>
        public Point GetCrossPointWithOtherLineInXY(LineSegment otherLine)
        {
            double k1, k2;//两条线段在XY平面投影线段的斜率
            if (lineVector.a != 0 && otherLine.LineVector.a != 0)//若两条线段都不垂直于X轴
            {
                k1 = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);
                k2 = (otherLine.StartPoint.Y - otherLine.EndPoint.Y) / (otherLine.StartPoint.X - otherLine.EndPoint.X);
                if (Math.Abs(k1- k2)<0.000000001)//两直线平行或者重合
                { return null; }
                else
                {
                    double x = (otherLine.StartPoint.Y - startPoint.Y + k1 * startPoint.X - k2 * otherLine.StartPoint.X) / (k1 - k2);//交点的x值
                    if (((x >= startPoint.X && x <= endPoint.X) || (x <= startPoint.X && x >= endPoint.X)) && ((x >= otherLine.StartPoint.X && x <= otherLine.EndPoint.X) || (x <= otherLine.StartPoint.X && x >= otherLine.EndPoint.X)))
                    {
                        double y = k1 * (x - startPoint.X) + startPoint.Y;//交点的y值
                        return new Point(x, y, 0);
                    }
                }
            }
            else if (lineVector.a == 0 && otherLine.LineVector.a != 0)//若线段1垂直X轴，线段2不垂直X轴
            {
                k2 = (otherLine.StartPoint.Y - otherLine.EndPoint.Y) / (otherLine.StartPoint.X - otherLine.EndPoint.X);
                if ((startPoint.X >= otherLine.StartPoint.X && startPoint.X <= otherLine.EndPoint.X) || (startPoint.X <= otherLine.StartPoint.X && startPoint.X >= otherLine.EndPoint.X))
                {
                    double y = k2 * (startPoint.X - otherLine.StartPoint.X) + otherLine.StartPoint.Y;//交点的y值
                    if (((y >= startPoint.Y && y <= endPoint.Y) || (y <= startPoint.Y && y >= endPoint.Y)) && ((y >= otherLine.StartPoint.Y && y <= otherLine.EndPoint.Y) || (y <= otherLine.StartPoint.Y && y >= otherLine.EndPoint.Y)))
                    {
                        return new Point(startPoint.X, y, 0);
                    }
                }
            }
            else if (lineVector.a != 0 && otherLine.LineVector.a == 0)//若线段1不垂直X轴，线段2垂直X轴
            {
                k1 = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);
                if ((otherLine.StartPoint.X >= startPoint.X && otherLine.StartPoint.X <= endPoint.X) || (otherLine.StartPoint.X <= startPoint.X && otherLine.StartPoint.X >= endPoint.X))
                {
                    double y = k1 * (otherLine.StartPoint.X - startPoint.X) + startPoint.Y;//交点的y值
                    if (((y >= startPoint.Y && y <= endPoint.Y) || (y <= startPoint.Y && y >= endPoint.Y)) && ((y >= otherLine.StartPoint.Y && y <= otherLine.EndPoint.Y) || (y <= otherLine.StartPoint.Y && y >= otherLine.EndPoint.Y)))
                    {
                        return new Point(otherLine.StartPoint.X, y, 0);
                    }
                }

            }
            return null;
        }


        /// <summary>
        /// 判断两条线段在XY平面的投影是否相交
        /// </summary>
        /// <param name="otherLine">另外一条线段</param>
        /// <returns>两条线段在XY平面的投影是否相交的布尔值</returns>
        public Boolean JudgeIfCrossWithOtherLineInXYPlane(LineSegment otherLine)
        {
            Point crossPoint = this.GetCrossPointWithOtherLineInXY(otherLine);
            if (crossPoint != null)
            { return true; }
            else
            { return false; }
        }


        /// <summary>
        /// 判断由线段与一个list中的线段在XY平面是否相交
        /// </summary>
        /// <param name="otherLines">线段的list</param>
        /// <param name="vertexNum">建筑物底面端点的编号</param>
        public bool JudgeIfCrossWithOtherLinesInXYPlane(List<LineSegment> otherLines)
        {
            for (int i = 0; i < otherLines.Count; i++)
            {
                Point crossPoint = GetCrossPointWithOtherLineInXY(otherLines[i]);
                //若交点不为空，且交点不是四边形和三角形的端点
                if (crossPoint != null && !crossPoint.IsEqualInXYProjection(startPoint) && !crossPoint.IsEqualInXYProjection(endPoint))
                { return true; }
            }
            return false;
        }
        /// <summary>
        /// 判断由线段与一个list中的线段在XY平面是否相交
        /// </summary>
        /// <param name="otherLines">线段的list</param>
        /// <param name="vertexNum">建筑物底面端点的编号</param>
        public bool JudgeIfCrossWithOtherLinesInXYPlane(List<AdjacentEdge> otherLines)
        {
            for (int i = 0; i < otherLines.Count; i++)
            {
                Point crossPoint = GetCrossPointWithOtherLineInXY(otherLines[i]);
                //若交点不为空，且交点不是四边形和三角形的端点
                if (crossPoint != null && !crossPoint.IsEqualInXYProjection(startPoint) && !crossPoint.IsEqualInXYProjection(endPoint))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// 求线段与一个线段list在XY平面的投影的交点
        /// </summary>
        /// <param name="otherLine">线段的list</param>
        /// <returns>返线段与其他线段在XY平面的投影的交点</returns>
        public List<Point> GetCrossPointsWithOtherLinesInXYPlane(List<LineSegment> otherLines)
        {
            List<Point> crossPoints = new List<Point>();
            for (int i = 0; i < otherLines.Count; i++)
            {
                Point crossPoint = GetCrossPointWithOtherLineInXY(otherLines[i]);
                //若交点不为空，且交点不是四边形和三角形的端点
                if (crossPoint != null && !crossPoint.IsEqualInXYProjection(startPoint) && !crossPoint.IsEqualInXYProjection(endPoint))
                { crossPoints.Add(crossPoint); }
            }
            return crossPoints;
        }

        /// <summary>
        /// 求线段与一个线段list在XY平面的投影的交点
        /// </summary>
        /// <param name="otherLine">线段的list</param>
        /// <returns>返线段与其他线段在XY平面的投影的交点</returns>
        public List<Point> GetCrossPointsWithOtherLinesInXYPlane(List<AdjacentEdge> otherLines)
        {
            List<Point> crossPoints = new List<Point>();
            for (int i = 0; i < otherLines.Count; i++)
            {
                Point crossPoint = GetCrossPointWithOtherLineInXY(otherLines[i]);
                //若交点不为空，且交点不是四边形和三角形的端点
                if (crossPoint != null && !crossPoint.IsEqualInXYProjection(startPoint) && !crossPoint.IsEqualInXYProjection(endPoint))
                { crossPoints.Add(crossPoint); }
            }
            return crossPoints;
        }

        /// <summary>
        /// 判断点的lsit中是否有点在线段在XY平面投影内
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <param name="line">线段</param>
        /// <returns>点是否在XY平面线段上的布尔值</returns>
        public bool JudgeIfPointsInLineInXYPlane(List<Point> viewPoints)
        {
            for (int i = 0; i < viewPoints.Count; i++)
            {
                if ((viewPoints[i].X == startPoint.X && viewPoints[i].Y == startPoint.Y) || (viewPoints[i].X == endPoint.X && viewPoints[i].Y == endPoint.Y))
                { return true; }
                else
                {
                    SpectVector point1ToviewPoint = new SpectVector(viewPoints[i].X - startPoint.X, viewPoints[i].Y - startPoint.Y, 0);
                    if (point1ToviewPoint.IsParallelAndSamedirectionInXYPlane(lineVector))
                    {
                        if (startPoint.GetDistanceInXY(viewPoints[i]) <= startPoint.GetDistanceInXY(endPoint))
                        { return true; }
                    }
                }
            }
            return false;
        }

    }

    /// <summary>
    ///两个三角面的公共棱
    /// </summary>
    public class AdjacentEdge : LineSegment
    {
        private double diffCylinderRaduis;
        private bool isDiffractionEdge = false;

        public double DiffCylinderRadius
        {
            get { return diffCylinderRaduis; }
            set { diffCylinderRaduis = value; }
        }

        public List<Face> AdjacentTriangles=new List<Face>();
        public bool HaveTraced = false;
        public bool IsDiffractionEdge
        {
            get { return this.isDiffractionEdge; }
        }
        //构造函数
        public AdjacentEdge()
            : base()
        { }
        public AdjacentEdge(Point point1, Point point2)
            : base(point1, point2)
        { }
        public AdjacentEdge(Point point1, Point point2, bool isDiffractionEdge)
            : base(point1, point2)
        {
            this.isDiffractionEdge = true;
        }
        public AdjacentEdge(Point point1, Point point2,Triangle tri1)
            : base(point1, point2)
        {
            this.AdjacentTriangles.Add(tri1);
            this.isDiffractionEdge = true;
        }

        public AdjacentEdge(Point point1, Point point2, Triangle tri1,Triangle tri2)
            : this(point1, point2, tri1)
        {
            this.AdjacentTriangles.Add(tri2);
        }

        public AdjacentEdge(LineSegment line)
        {
            this.startPoint = line.StartPoint;
            this.endPoint = line.EndPoint;
            this.lineVector = line.LineVector;
        }

        public AdjacentEdge(LineSegment line,Triangle tri1,Triangle tri2)
            :this(line)
        {
            AdjacentTriangles.Add(tri1);
            AdjacentTriangles.Add(tri2);
            this.isDiffractionEdge = true;
        }

        public AdjacentEdge(Face face1, Face face2)
        {
            List<Point> commonPoint = face1.GetCommonPointsWithOtherFace(face2);
            if (commonPoint.Count == 2)//为相邻三角面
            {
                this.startPoint = commonPoint[0];
                this.endPoint = commonPoint[1];
                this.lineVector = new SpectVector(startPoint, EndPoint);
                this.AdjacentTriangles.Add(face1);
                this.AdjacentTriangles.Add(face2);
                this.isDiffractionEdge = true;
            }
            if (commonPoint.Count != 2)
            {
                Console.WriteLine("构建绕射棱时，三角面不相邻！Line.cs public AdjacentEdge(Face face1, Face face2)");
            }
        }
        public AdjacentEdge(Triangle triangle1, Triangle triangle2)
        {
            List<Point> commonPoint = triangle1.GetCommonPointsWithOtherFace(triangle2);
            if (commonPoint.Count == 2)//为相邻三角面
            {
                this.startPoint = commonPoint[0];
                this.endPoint = commonPoint[1];
                this.lineVector = new SpectVector(startPoint, EndPoint);
                this.AdjacentTriangles.Add(triangle1);
                this.AdjacentTriangles.Add(triangle2);
                this.isDiffractionEdge = true;
            }
        }

        //
        public AdjacentEdge(Point p1, Point p2, SpectVector sp)
        {
            startPoint = p1;
            endPoint = p2;
            lineVector = sp;
        }

        /// <summary>
        ///将绕射面加入list中
        /// </summary>
        public void AddDiffractionFaceToEdge(Triangle tri)
        { 
            this.AdjacentTriangles.Add(tri);
            this.isDiffractionEdge = true;
        }

        /// <summary>
        ///获取单绕射棱上点的坐标
        /// </summary>
        public Point GetDiffractionPoint(Point origin, Point target)
        {
            double h1 = this.getDistanceP2Line(origin);
            double h2 = this.getDistanceP2Line(target);
            Point pedal1 = this.GetPedalPoint(origin);
            Point pedal2 = this.GetPedalPoint(target);
            if (h1 == 0 || h2 == 0)
            {
                return null;
            }
            if (Math.Abs(pedal1.X - pedal2.X) < 0.000001 && Math.Abs(pedal1.Y - pedal2.Y) < 0.000001 && Math.Abs(pedal1.Z - pedal2.Z) < 0.000001)
            {
                return pedal1;
            }
            double ratio = h1 / (h1 + h2); ;
            SpectVector tempVector = new SpectVector(pedal1, pedal2);
            Point diffractPoint = new Point(tempVector.a * ratio + pedal1.X, tempVector.b * ratio + pedal1.Y, tempVector.c * ratio + pedal1.Z);
            if (this.JudgeIfPointInLineRange(diffractPoint))
            {
                return diffractPoint;
            }
            return null;
            //double h1 = this.getDistanceP2Line(origin);
            //double h2 = this.getDistanceP2Line(target);
            //if (h1 == 0 || h2 == 0)
            //{
            //    return null;
            //}
            //if (h1 == h2)
            //{
            //    Point originPedal = this.GetPedalPoint(origin);
            //    Point targerPedal = this.GetPedalPoint(target);
            //    Point diffractPoint = new Point((originPedal.X + targerPedal.X) / 2, (originPedal.Y + targerPedal.Y) / 2, (originPedal.Z + targerPedal.Z) / 2);
            //    if (this.JudgeIfPointInLineRange(diffractPoint))
            //    {
            //        return diffractPoint;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            //double K = Math.Pow(h2, 2) - Math.Pow(h1, 2);
            //double A = 2 * Math.Pow(h1, 2) * target.X - 2 * Math.Pow(h2, 2) * origin.X;
            //double B = 2 * Math.Pow(h1, 2) * target.Y - 2 * Math.Pow(h2, 2) * origin.Y;
            //double C = 2 * Math.Pow(h1, 2) * target.Z - 2 * Math.Pow(h2, 2) * origin.Z;
            //double D = Math.Pow(h1, 2) * (Math.Pow(target.X, 2) + Math.Pow(target.Y, 2) + Math.Pow(target.Z, 2))
            //    - Math.Pow(h2, 2) * (Math.Pow(origin.X, 2) + Math.Pow(origin.Y, 2) + Math.Pow(origin.Z, 2));
            //double M = K * (Math.Pow(this.LineVector.a, 2) + Math.Pow(this.LineVector.b, 2) + Math.Pow(this.LineVector.c, 2));
            //double N = (2 * K * this.StartPoint.X + A) * this.LineVector.a + (2 * K * this.StartPoint.Y + B) * this.LineVector.b
            //    + (2 * K * this.StartPoint.Z + C) * this.LineVector.c;
            //double Q = D - K * (Math.Pow(this.StartPoint.X, 2) + Math.Pow(this.StartPoint.Y, 2) + Math.Pow(this.StartPoint.Z, 2))
            //    - A * this.StartPoint.X - B * this.StartPoint.Y - C * this.StartPoint.Z;
            //double judge = Math.Pow(N, 2) + 4 * M * Q;
            //if (Math.Abs(Math.Pow(N, 2) + 4 * M * Q) < 0.00000001)
            //{
            //    double t = -N / (2 * M);
            //    Point dPoint = new Point(this.StartPoint.X + this.LineVector.a * t, this.StartPoint.Y + this.LineVector.b * t,
            // this.StartPoint.Z + this.LineVector.c * t);
            //    return dPoint;
            //}
            //else if (Math.Pow(N, 2) + 4 * M * Q > 0)
            //{
            //    double t1 = (-N + Math.Sqrt(Math.Pow(N, 2) + 4 * M * Q)) / (2 * M);
            //    double t2 = (-N - Math.Sqrt(Math.Pow(N, 2) + 4 * M * Q)) / (2 * M);
            //    Point dPoint1 = new Point(this.StartPoint.X + this.LineVector.a * t1, this.StartPoint.Y + this.LineVector.b * t1,
            //        this.StartPoint.Z + this.LineVector.c * t1);
            //    Point dPoint2 = new Point(this.StartPoint.X + this.LineVector.a * t2, this.StartPoint.Y + this.LineVector.b * t2,
            //       this.StartPoint.Z + this.LineVector.c * t2);
            //    Point txVerticalPoint = this.GetPedalPoint(origin);
            //    Point rxVerticalPoint = this.GetPedalPoint(target);
            //    if (new LineSegment(txVerticalPoint, rxVerticalPoint).JudgeIfPointInLineRange(dPoint1))
            //    {
            //        return dPoint1;
            //    }
            //    else
            //    {
            //        return dPoint2;
            //    }
            //}
            //return null;
        }

        /// <summary>
        /// 得到无限长棱上的绕射点
        /// </summary>
        /// <param name="origin">起始点</param>
        /// <param name="target">观测点</param>
        /// <returns>绕射点</returns>
        public Point GetInfiniteDiffractionPoint(Point origin, Point target)
        {
            double h1 = this.getDistanceP2Line(origin);
            double h2 = this.getDistanceP2Line(target);
            Point pedal1 = this.GetPedalPoint(origin);
            Point pedal2 = this.GetPedalPoint(target);
            if (h1 == 0 || h2 == 0)
            {
                return null;
            }
            if (Math.Abs(pedal1.X - pedal2.X) < 0.000001 && Math.Abs(pedal1.Y - pedal2.Y) < 0.000001 && Math.Abs(pedal1.Z - pedal2.Z) < 0.000001)
            {
                return pedal1;
            }
            double ratio = h1 / (h1 + h2); ;
            SpectVector tempVector = new SpectVector(pedal1, pedal2);
            Point diffractPoint = new Point(tempVector.a * ratio + pedal1.X, tempVector.b * ratio + pedal1.Y, tempVector.c * ratio + pedal1.Z);
            return diffractPoint;
        }

        /// <summary>
        /// 当点在直线上得到无限长棱上的绕射点
        /// </summary>
        /// <param name="origin">起始点</param>
        /// <param name="target">观测点</param>
        /// <returns>绕射点</returns>
        public Point GetInfiniteDiffractionPointWhenInLine(Point origin, Point target)
        {
            double h1 = this.getDistanceP2Line(origin);
            double h2 = this.getDistanceP2Line(target);
            Point pedal1 = this.GetPedalPoint(origin);
            Point pedal2 = this.GetPedalPoint(target);
            if (h1 == 0)
            {
                return origin;
            }
            if (h2 == 0)
            {
                return target;
            }
            if (Math.Abs(pedal1.X - pedal2.X) < 0.000001 && Math.Abs(pedal1.Y - pedal2.Y) < 0.000001 && Math.Abs(pedal1.Z - pedal2.Z) < 0.000001)
            {
                return pedal1;
            }
            double ratio = h1 / (h1 + h2); ;
            SpectVector tempVector = new SpectVector(pedal1, pedal2);
            Point diffractPoint = new Point(tempVector.a * ratio + pedal1.X, tempVector.b * ratio + pedal1.Y, tempVector.c * ratio + pedal1.Z);
            return diffractPoint;
        }   
        /// <summary>
        /// 得到一个点到此直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double getDistanceP2Line(Point point)
        {
            if (isInline(point))
            {
                return 0;
            }
            else
            {
                return DistanceOfPoints(point, GetPedalPoint(point));
            }
        }

        /// <summary>
        /// 获取棱外一点到这条棱的垂足，调用前先判断这个点是否在棱上，用isInline方法
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point GetPedalPoint(Point point)
        {
            double d = -lineVector.a * point.X - lineVector.b * point.Y - lineVector.c * point.Z;
            double temp = (-d - lineVector.a * startPoint.X - lineVector.b * startPoint.Y - lineVector.c * startPoint.Z) / (lineVector.a * lineVector.a + lineVector.b * lineVector.b + lineVector.c * lineVector.c);



            return new Point(lineVector.a * temp + startPoint.X, lineVector.b * temp + startPoint.Y, lineVector.c * temp + startPoint.Z);
        }

        private double DistanceOfPoints(Point point1, Point point2)
        {
            return Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.Z - point2.Z) * (point1.Z - point2.Z));
        }

        /// <summary>
        ///判断一个点是否在这条棱上
        /// </summary>
        private bool isInline(Point point)
        {
            double weight1 = (point.X - startPoint.X) / lineVector.a;
            double weight2 = (point.Y - startPoint.Y) / lineVector.b;
            double weight3 = (point.Z - startPoint.Z) / lineVector.c;

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
        /// 已知一条棱上的一个点和这条棱，求一条射线从这个点出发（与这个棱的角度一定）到达另一个已知棱的方向向量
        /// </summary>
        /// <param name="beginPoint">起始点</param>
        /// <param name="angle">与起始点所在棱的夹角</param>
        /// <param name="targetEdge">另一条已知棱</param>
        /// <returns>返回参数中方向向量为求得的方向向量，点为在另一个已知棱上的点</returns>
        public List< RayInfo> getVectorFromEdge2EdgeWithAngle(Point beginPoint, double angle, AdjacentEdge targetEdge)
        {
            List<RayInfo> returnRays = new List<RayInfo>();
            double x1 = targetEdge.startPoint.X - beginPoint.X;
            double y1 = targetEdge.startPoint.Y - beginPoint.Y;
            double z1 = targetEdge.startPoint.Z - beginPoint.Z;
            double a1 = this.lineVector.a * targetEdge.lineVector.a + this.lineVector.b * targetEdge.lineVector.b + this.lineVector.c * targetEdge.lineVector.c;
            double b1 = this.lineVector.a * x1 + this.lineVector.b * y1 + this.lineVector.c * z1;
            double p = Math.Cos(angle * Math.PI / 180) * Math.Cos(angle * Math.PI / 180) * 
                (this.lineVector.a * this.lineVector.a + this.lineVector.b * this.lineVector.b + this.lineVector.c * this.lineVector.c);
            double a = p * (targetEdge.lineVector.a * targetEdge.lineVector.a + targetEdge.lineVector.b * targetEdge.lineVector.b + targetEdge.lineVector.c * targetEdge.lineVector.c) - a1 * a1;
            double b = p * 2 * (targetEdge.lineVector.a * x1 + targetEdge.lineVector.b * y1 + targetEdge.lineVector.c * z1) - 2 * a1 * b1;
            double c = p * (x1 * x1 + y1 * y1 + z1 * z1) - b1 * b1;

            RayInfo returnRay = null;
            SpectVector returnSV = null;
            Point returnPoint = null;

            double t;
            if (Math.Abs(a)<0.000001)//此时方程只有一个解，且一定正确，只需判断是否在棱上即可
            {
                t = -c / b;
                double x = targetEdge.lineVector.a * t + targetEdge.startPoint.X;
                if ((x - targetEdge.startPoint.X) * (x - targetEdge.endPoint.X) <= 0)
                {
                    returnPoint = new Point(targetEdge.lineVector.a * t + targetEdge.startPoint.X, 
                        targetEdge.lineVector.b * t + targetEdge.startPoint.Y, targetEdge.lineVector.c * t + targetEdge.startPoint.Z);
                    returnSV = new SpectVector(targetEdge.lineVector.a * t + targetEdge.startPoint.X - beginPoint.X, 
                        targetEdge.lineVector.b * t + targetEdge.startPoint.Y - beginPoint.Y, 
                        targetEdge.lineVector.c * t + targetEdge.startPoint.Z - beginPoint.Z);
                    returnRay = new RayInfo(returnPoint, returnSV);
                    returnRays.Add(returnRay);
                }
            }
            else//此时方程有两个解，存在两种情况，一种是其中一个可能是由cos的平方产生的，需对所产生的点进行判断，舍去错误点；
                //另一种情况是，两个解都在追面上，但其中一个不在棱上而在棱延长线上，这种情况也应该舍去
                //{
                //    double delta = b * b - 4 * a * c;
                //    if (delta >= 0)
                //    {
                //        t = (-b + Math.Sqrt(delta)) / 2 / a;
                //        returnPoint = new Point(targetEdge.lineVector.a * t + targetEdge.startPoint.X,
                //            targetEdge.lineVector.b * t + targetEdge.startPoint.Y, targetEdge.lineVector.c * t + targetEdge.startPoint.Z);
                //        SpectVector tempVector=new SpectVector(beginPoint,returnPoint);
                //        if (Math.Abs(SpectVector.VectorPhase(this.lineVector, tempVector) - angle) > 0.0001)//如果所求解需舍去
                //        {
                //            t = (-b - Math.Sqrt(delta)) / 2 / a;
                //            returnPoint.X = targetEdge.lineVector.a * t + targetEdge.startPoint.X;
                //            returnPoint.Y = targetEdge.lineVector.b * t + targetEdge.startPoint.Y;
                //            returnPoint.Z = targetEdge.lineVector.c * t + targetEdge.startPoint.Z;
                //        }
                //        double x = targetEdge.lineVector.a * t + targetEdge.startPoint.X;
                //        double y = targetEdge.lineVector.b * t + targetEdge.startPoint.Y;
                //        double z = targetEdge.lineVector.c * t + targetEdge.startPoint.Z;
                //        if ((x - targetEdge.startPoint.X) * (x - targetEdge.endPoint.X) - 0.0001 <= 0
                //            && (y - targetEdge.startPoint.Y) * (y - targetEdge.endPoint.Y) - 0.0001 <= 0
                //            && (z - targetEdge.startPoint.Z) * (z - targetEdge.endPoint.Z) - 0.0001 <= 0)//判断点是否在第二条棱上
                //        {
                //            returnSV = new SpectVector(targetEdge.lineVector.a * t + targetEdge.startPoint.X - beginPoint.X,
                //                targetEdge.lineVector.b * t + targetEdge.startPoint.Y - beginPoint.Y,
                //                targetEdge.lineVector.c * t + targetEdge.startPoint.Z - beginPoint.Z);
                //            returnRay = new RayInfo(returnPoint, returnSV);
                //            returnRays.Add(returnRay);
                //        }
                //        else
                //        {
                //            t = (-b - Math.Sqrt(delta)) / 2 / a;
                //            x = targetEdge.lineVector.a * t + targetEdge.startPoint.X;
                //            y = targetEdge.lineVector.b * t + targetEdge.startPoint.Y;
                //            z = targetEdge.lineVector.c * t + targetEdge.startPoint.Z;
                //            if ((x - targetEdge.startPoint.X) * (x - targetEdge.endPoint.X) - 0.0001 <= 0 
                //                && (y - targetEdge.startPoint.Y) * (y - targetEdge.endPoint.Y) - 0.0001 <= 0
                //                && (z - targetEdge.startPoint.Z) * (z - targetEdge.endPoint.Z) - 0.0001 <= 0)
                //            {
                //                returnSV = new SpectVector(targetEdge.lineVector.a * t + targetEdge.startPoint.X - beginPoint.X,
                //                targetEdge.lineVector.b * t + targetEdge.startPoint.Y - beginPoint.Y,
                //                targetEdge.lineVector.c * t + targetEdge.startPoint.Z - beginPoint.Z);
                //                returnRay = new RayInfo(returnPoint, returnSV);
                //            }
                //        }
                //    }
            {
                double delta = b * b - 4 * a * c;
                //这是两个交点的第一个解
                t = (-b + Math.Sqrt(delta)) / 2 / a;
                double x = targetEdge.lineVector.a * t + targetEdge.startPoint.X;
                double y = targetEdge.lineVector.b * t + targetEdge.startPoint.Y;
                double z = targetEdge.lineVector.c * t + targetEdge.startPoint.Z;
                returnPoint = new Point(x,y,z);
                if ((x - targetEdge.startPoint.X) * (x - targetEdge.endPoint.X) - 0.0001 <= 0
                             && (y - targetEdge.startPoint.Y) * (y - targetEdge.endPoint.Y) - 0.0001 <= 0
                             && (z - targetEdge.startPoint.Z) * (z - targetEdge.endPoint.Z) - 0.0001 <= 0)//判断点是否在第二条棱上
                {
                    returnSV = new SpectVector(targetEdge.lineVector.a * t + targetEdge.startPoint.X - beginPoint.X,
                        targetEdge.lineVector.b * t + targetEdge.startPoint.Y - beginPoint.Y,
                        targetEdge.lineVector.c * t + targetEdge.startPoint.Z - beginPoint.Z);
                    returnRay = new RayInfo(returnPoint, returnSV);
                    //判断交点在不在反向延长线上，如果不在舍去，在的话添加到returnRays中
                    double angle2 = SpectVector.VectorPhase(new SpectVector(beginPoint, returnRay.Origin), this.lineVector);
                    if (Math.Abs(angle2 - angle) < 0.00001)
                    {
                        returnRays.Add(returnRay);
                    }
                  
                    
                }
                //两个交点的第二个解
                t = (-b - Math.Sqrt(delta)) / 2 / a;
                x = targetEdge.lineVector.a * t + targetEdge.startPoint.X;
                y = targetEdge.lineVector.b * t + targetEdge.startPoint.Y;
                z = targetEdge.lineVector.c * t + targetEdge.startPoint.Z;
                if ((x - targetEdge.startPoint.X) * (x - targetEdge.endPoint.X) - 0.0001 <= 0
                    && (y - targetEdge.startPoint.Y) * (y - targetEdge.endPoint.Y) - 0.0001 <= 0
                    && (z - targetEdge.startPoint.Z) * (z - targetEdge.endPoint.Z) - 0.0001 <= 0)
                {
                    returnSV = new SpectVector(targetEdge.lineVector.a * t + targetEdge.startPoint.X - beginPoint.X,
                    targetEdge.lineVector.b * t + targetEdge.startPoint.Y - beginPoint.Y,
                    targetEdge.lineVector.c * t + targetEdge.startPoint.Z - beginPoint.Z);
                    returnPoint = new Point(x, y, z);
                    returnRay = new RayInfo(returnPoint, returnSV);
                    double angle2 = SpectVector.VectorPhase(new SpectVector(beginPoint, returnRay.Origin), this.lineVector);
                    if (Math.Abs(angle2 - angle) < 0.00001)
                    {
                        returnRays.Add(returnRay);
                    }
                    
                }


            }
            //判断交点在不在射线的反向延长线上
            //for (int i=0;i<returnRays.Count;i++)
            //{
            //    double angle2 = SpectVector.VectorPhase(new SpectVector(beginPoint,returnRays[i].Origin), this.lineVector);
            //    if (Math.Abs(angle2 - angle) > 0.00001)
            //    {
            //        returnRays.RemoveAt(i);
            //    }
            //}
            
            return returnRays;

        }

        /// <summary>
        /// 判断两个棱是否相同
        /// </summary>
        public bool isSameAdjacent(AdjacentEdge edge)
        {
            return ((this.startPoint.equal(edge.startPoint) && this.endPoint.equal(edge.endPoint)) ||
                (this.startPoint.equal(edge.endPoint) && this.endPoint.equal(edge.startPoint)));
        }

        /// <summary>
        /// 修改三角面细分后，原来绕射棱对应的绕射面
        /// </summary>
        /// <param name="quondamTriangle">原来的三角面</param>
        /// <param name="newTriangles">新的三角面List，里面有一个边对应的三角面</param>
        /// <returns></returns>
        public void AlterLineDiffractionFace(Triangle quondamTriangle, List<Triangle> newTriangles)
        {
            //先把对应原来三角面的绕射面删除
            for (int j = 0; j < this.AdjacentTriangles.Count; j++)
            {
                if (this.AdjacentTriangles[j]==quondamTriangle)
                {
                    this.AdjacentTriangles.RemoveAt(j);
                    j--;
                }
            }
            //得到新的对应的绕射面
            for (int k = 0; k < newTriangles.Count; k++)
            {
                if (newTriangles[k].JudgeIfHaveThisLine(this))
                {
                    this.AdjacentTriangles.Add(newTriangles[k]);
                }
            }
            if (this.AdjacentTriangles.Count > 2)
            { throw new Exception("求绕射边三角面时出错"); }
        }

    }
}
