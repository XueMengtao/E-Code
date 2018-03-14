using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CalculateModelClasses
{

    /// <summary>
    ///射线信息
    /// </summary>
    public class RayInfo:ICloneable
    {

    
        public Point Origin;
        public SpectVector RayVector;
        public RayInfo(Point one, SpectVector vector)
        {
            Origin = one;
            RayVector = vector;
        }
        public RayInfo(Point one, Point two)
        {
            Origin = one;
            SpectVector temp = new SpectVector(one, two);
            RayVector = temp.GetNormalizationVector();
        }
        public RayInfo(Node one, Node two)
        {
            Origin = one.Position;
            RayVector = new SpectVector(one.Position, two.Position);
        }

        /// <summary>
        /// 求射线与矩形的交点
        /// </summary>
        /// <param name="terRect">矩形</param>
        /// <returns>返回相交节点</returns>
        public Node GetCrossNodeWithRect(Rectangle terRect)
        {
            Node crossPoint1 = this.GetCrossNodeWithOriginTriangle(terRect.TriangleOne);
            Node crossPoint2 = this.GetCrossNodeWithOriginTriangle(terRect.TriangleTwo);
            if (crossPoint1 != null && crossPoint2 == null)
            { return crossPoint1; }
            else if (crossPoint1 == null && crossPoint2 != null)
            { return crossPoint2; }
            else if (crossPoint1 != null && crossPoint2 != null)
            {
                if (crossPoint1.DistanceToFrontNode < crossPoint2.DistanceToFrontNode)
                { return crossPoint1; }
                else
                { return crossPoint2; }
            }

            return null;
        }
        /// <summary>
        ///求直线与平面的夹角
        /// </summary>
        /// <param line="face">平面</param>
        /// <returns>直线与平面的夹角</returns>
        public double GetAngleWithFace( Face face)
        {
            SpectVector normalVector = face.GetNormalVector();//获得平面的法向量
            double crossAngle = this.RayVector.GetPhaseOfVector(normalVector);//获得射线与法向量的夹角
            if ((crossAngle <= 90) && (0 < crossAngle))
            {
               return 90 - crossAngle;
            }
            else
            {
               return crossAngle - 90;
            }
        }

        /// <summary>
        /// 求两条直线的夹角
        /// </summary>
        /// <param line="face">平面</param>
        /// <returns>直线与平面的夹角</returns>
        public double GetAngleOfTwoStraightLines(RayInfo otherRay)
        {
            double angleOfTwoLines =this.RayVector.GetPhaseOfVector( otherRay.RayVector);
            if ((90 < angleOfTwoLines) && (angleOfTwoLines <= 180))
                angleOfTwoLines = 180 - angleOfTwoLines;
            return angleOfTwoLines;
        }


        /// <summary>
        ///判断点是否在射线所在的直线上
        /// </summary>
        /// <param line="viewPoint">观察点</param>
        /// <returns>点是否在射线所在的直线上的布尔值</returns>
        public bool JudgeIfPointIsInStraightLine(Point viewPoint)
        {
            if (viewPoint.equal(this.Origin) || new SpectVector(this.Origin, viewPoint).IsParallel(this.RayVector))
            { 
                return true;
            }
            else 
            {
                return false;
            }

        }


        /// <summary>
        ///判断点是否在射线方向上
        /// </summary>
        /// <param line="viewPoint">观察点</param>
        /// <returns>点是否在射线方向上的布尔值</returns>
        public bool JudgeIfPointIsInRayVector(Point viewPoint)
        {
            if (viewPoint.equal(this.Origin) || new SpectVector(this.Origin, viewPoint).IsParallelAndSamedirection(this.RayVector))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 求两条共面不平行的射线所在的直线的交点
        /// </summary>
        /// <param name="otherRay">另一条射线</param>
        /// <returns>交点</returns>
        public Point GetCrossPointWithOtherRay(RayInfo otherRay)
        {
            if (otherRay==null || Math.Abs(this.GetDistanceOfNonUniplanarRays(otherRay)) > 0.000001 || this.RayVector.IsParallel(otherRay.RayVector))
            {
                LogFileManager.ObjLog.error("求两条共面不平行直线交点时，输入为空，或者是两条异面直线或者平行直线");
                return null;
            }
            double x1 = this.Origin.X, y1 = this.Origin.Y, z1 = this.Origin.Z;
            double x2 = otherRay.Origin.X, y2 = otherRay.Origin.Y, z2 = otherRay.Origin.Z;
            double a1 = this.RayVector.a, b1 = this.RayVector.b, c1 = this.RayVector.c;
            double a2 = otherRay.RayVector.a, b2 = otherRay.RayVector.b, c2 = otherRay.RayVector.c;
            Point crossPoint = new Point();
            if (Math.Abs(a1) > 0.000001 && Math.Abs(a2) < 0.000001)
            {
                crossPoint.X = x2;
                double t1 = (x2 - x1) / a1;
                crossPoint.Y = b1 * t1 + y1;
                crossPoint.Z = c1 * t1 + z1;
                return crossPoint;
            }
            else if (Math.Abs(a1) < 0.000001 && Math.Abs(a2) > 0.000001)
            {
                crossPoint.X = x1;
                double t2 = (x1 - x2) / a2;
                crossPoint.Y = b2 * t2 + y2;
                crossPoint.Z = c2 * t2 + z2;
                return crossPoint;

            }
            else if (Math.Abs(b1) > 0.000001 && Math.Abs(b2) < 0.000001)
            {
                crossPoint.Y = y2;
                double t1 = (y2 - y1) / b1;
                crossPoint.X = a1 * t1 + x1;
                crossPoint.Z = c1 * t1 + z1;
                return crossPoint;
            }
            else if (Math.Abs(b2) > 0.000001 && Math.Abs(b2) < 0.000001)
            {
                crossPoint.Y = y1;
                double t2 = (y1 - y2) / b2;
                crossPoint.X = a2 * t2 + x2;
                crossPoint.Z = c2 * t2 + z2;
                return crossPoint;
            }
            else if (Math.Abs(c1) > 0.000001 && Math.Abs(c2) < 0.000001)
            {
                crossPoint.Z = z2;
                double t1 = (z2 - z1) / c1;
                crossPoint.X = a1 * t1 + x1;
                crossPoint.Y = b1 * t1 + y1;
                return crossPoint;
            }
            else if (Math.Abs(c2) > 0.000001 && Math.Abs(c1) < 0.000001)
            {
                crossPoint.Z = z1;
                double t2 = (z1 - z2) / c2;
                crossPoint.X = a2 * t2 + x2;
                crossPoint.Y = b2 * t2 + y2;
                return crossPoint;
            }
            else if (Math.Abs(a1) < 0.000001 && Math.Abs(a2) < 0.000001)
            {
                if ((b1 == 0 && b2 == 0) || (c1 == 0 && c2 == 0))
                {
                    LogFileManager.ObjLog.error("求两条共面不平行直线时，输入的直线方向向量a,b都为0，两直线平行");
                    return null;
                }
                else
                {
                    double t1 = (y2 - y1 + (b2 / c2) * (z1 - z2)) / (b1 - (b2 / c2) * c1);
                    crossPoint.X = x1;
                    crossPoint.Y = b1 * t1 + y1;
                    crossPoint.Z = c1 * t1 + z1;
                }
                return crossPoint;
            }
            else if (Math.Abs(b1) < 0.000001 && Math.Abs(b2) < 0.000001)
            {
                if (Math.Abs(c1) < 0.000001 && Math.Abs(c2) < 0.000001)//到这里说明a1,a2不为0
                {
                    LogFileManager.ObjLog.error("求两条共面不平行直线时，输入的直线方向向量a,b都为0，两直线平行");
                    return null;
                }
                else
                {
                    double t1 = (x2 - x1 + (a2 / c2) * (z1 - z2)) / (a1 - (a2 / c2) * c1);
                    crossPoint.X = a1 * t1 + x1;
                    crossPoint.Y = y1;
                    crossPoint.Z = c1 * t1 + z1;
                }
                return crossPoint;
            }
            else//到这步说明a1,a2,b1,b2都不为0
            {
                double t1 = Double.NaN;
                if ((a1 - (a2 / b2) * b1) != 0)
                { t1 = (x2 - x1 + (a2 / b2) * (y1 - y2)) / (a1 - (a2 / b2) * b1); } 
                //    double t1 = (y2 - y1 + (b2 / a2) * (x1 - x2)) / (b1 - (a1 / a2) * b2);
                else if ((a1 - (a2 / c2) * c1) != 0)
                { t1 = (x2 - x1 + (a2 / c2) * (z1 - z2)) / (a1 - (a2 / c2) * c1); }
                crossPoint.X = a1 * t1 + x1;
                crossPoint.Y = b1 * t1 + y1;
                crossPoint.Z = c1 * t1 + z1;
                if (t1 == Double.NaN)
                {
                    throw new Exception("求共面不平行两射线交点时出错");
                }
                return crossPoint;
            } 

        }


        /// <summary>
        /// 求两条共面不平行的射线所在的直线的交点
        /// </summary>
        /// <param name="otherRay">另一条射线</param>
        /// <returns>交点</returns>
        public Point GetCrossPointWithAdjacentEdges(List<AdjacentEdge> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Point crossPoint = this.GetCrossPointWithOtherRay(lines[i].SwitchToRay());
                if (crossPoint != null && lines[i].JudgeIfPointInLineRange(crossPoint))
                { return crossPoint; }
            }
            return null;
        }

        /// <summary>
        ///求射线打到一个平面后得到的反射射线
        /// </summary>
        /// <param name="reflectionFace">反射面</param>
        /// <returns>反射射线</returns>
        public RayInfo GetReflectionRay(Face reflectionFace)
        {
            Point crossPoint = this.GetCrossPointBetweenStraightLineAndFace(reflectionFace);
            if (crossPoint == null)
            { return null; }
            else
            {
                Point mirrorPoint = this.Origin.GetMirrorPoint(reflectionFace);
                SpectVector vector = new SpectVector(mirrorPoint, crossPoint);
                return new RayInfo(crossPoint, vector);
            }
        }

        /// <summary>
        ///求射线打到一个平面后得到的反射射线
        /// </summary>
        /// <param name="reflectionFace">反射面</param>
        /// <returns>反射射线</returns>
        public RayInfo GetReflectionRay(Face reflectionFace,Point crossPoint)
        {
            Point mirrorPoint = this.Origin.GetMirrorPoint(reflectionFace);
            SpectVector vector = new SpectVector(mirrorPoint, crossPoint);
            return new RayInfo(crossPoint, vector);
        }

        /// <summary>
        ///求两条异面直线的距离
        /// </summary>
        public double GetDistanceOfNonUniplanarRays(RayInfo otherRay)
        {
            if (this.Origin.equal(otherRay.Origin) || this.RayVector.IsParallel(otherRay.RayVector))//两射线共起点或平行时用此方法无法计算，且不符合条件
            {
                return 0;
            }
            SpectVector verticalLine = SpectVector.VectorCrossMultiply(this.RayVector, otherRay.RayVector);
            double thi = verticalLine.GetPhaseOfVector(new SpectVector(this.Origin, otherRay.Origin));
            if (thi > 90)
            { thi = 180 - thi; }
            return this.Origin.GetDistance(otherRay.Origin) * Math.Cos(thi * Math.PI / 180);
          
        }

        /// <summary>
        /// 求射线与线段在XY平面上的投影的交点
        /// </summary>
        /// <param name="oneLine">线段</param>
        /// <returns>射线于线段在XY平面上的投影的交点</returns>
        public Point GetCrossPointWtihLineInXYPlane(LineSegment oneLine)
        {
            Point crossPoint = new Point();
            double k1, k2;//两条线段在XY平面投影线段的斜率
            if (Math.Abs( this.RayVector.a)>0.00000001 && Math.Abs( oneLine.LineVector.a) > 0.00000001)//若两条线段都不垂直于X轴
            {
               
                k1 =  this.RayVector.b / this.RayVector.a; 
                k2 = (oneLine.StartPoint.Y - oneLine.EndPoint.Y) / (oneLine.StartPoint.X - oneLine.EndPoint.X);
                if (Math.Abs(k1 - k2) > 0.000000001)//两直线不平行或者重合
                {
                    double x = (oneLine.StartPoint.Y - this.Origin.Y + k1 * this.Origin.X - k2 * oneLine.StartPoint.X) / (k1 - k2);//交点的x值
                    if ((x >= oneLine.StartPoint.X && x <= oneLine.EndPoint.X) || (x <= oneLine.StartPoint.X && x >= oneLine.EndPoint.X))
                    {
                        double y = k1 * (x - this.Origin.X) + this.Origin.Y;//交点的y值
                        crossPoint = new Point(x, y, 0);
                        if (this.RayVector.IsParallelAndSamedirectionInXYPlane(new SpectVector(this.Origin, crossPoint)))
                        { return crossPoint; }
                    }
                }
            }
            if (Math.Abs(this.RayVector.a) <= 0.00000001 && Math.Abs(oneLine.LineVector.a) > 0.00000001)//若射线垂直X轴，线段不垂直X轴
            {
                k2 = (oneLine.StartPoint.Y - oneLine.EndPoint.Y) / (oneLine.StartPoint.X - oneLine.EndPoint.X);
                if ((this.Origin.X >= oneLine.StartPoint.X && this.Origin.X <= oneLine.EndPoint.X) || (this.Origin.X <= oneLine.StartPoint.X && this.Origin.X >= oneLine.EndPoint.X))
                {
                    double y = k2 * (this.Origin.X - oneLine.StartPoint.X) + oneLine.StartPoint.Y;//交点的y值
                    if ((y >= oneLine.StartPoint.Y && y <= oneLine.EndPoint.Y) || (y <= oneLine.StartPoint.Y && y >= oneLine.EndPoint.Y))
                    {
                         crossPoint = new Point(this.Origin.X, y, 0);
                         if (this.RayVector.IsParallelAndSamedirectionInXYPlane(new SpectVector(this.Origin, crossPoint)))
                         { return crossPoint; }
                    }
                }
            }
            if (Math.Abs(this.RayVector.a) > 0.00000001 && Math.Abs(oneLine.LineVector.a) <= 0.00000001)//若射线不垂直X轴，线段垂直X轴
            {
                k1 = this.RayVector.b / this.RayVector.a; 
                double y = k1 * (oneLine.StartPoint.X - this.Origin.X) + this.Origin.Y;//交点的y值
                if ((y >= oneLine.StartPoint.Y && y <= oneLine.EndPoint.Y) || (y <= oneLine.StartPoint.Y && y >= oneLine.EndPoint.Y))
                {
                    crossPoint = new Point(oneLine.StartPoint.X, y, 0);
                    if (this.RayVector.IsParallelAndSamedirectionInXYPlane(new SpectVector(this.Origin, crossPoint)))
                    { return crossPoint; }
                }
            }
            return null;
        }


        /// <summary>
        ///获得直线外一点到该直线的垂足
        /// </summary>
        /// <param name="p">直线外一点</param>
        /// <returns>直线外一点在该直线的垂足</returns>
        public Point getDropFoot(Point p)
        {
            Point end = new Point(Origin.X + RayVector.a * 100, Origin.Y + RayVector.b * 100, Origin.Z + RayVector.c * 100);
            double dx = Origin.X - end.X;
            double dy = Origin.Y - end.Y;
            double dz = Origin.Z - end.Z;

            double u = (p.X - Origin.X) * (Origin.X - end.X) + (p.Y - Origin.Y) * (Origin.Y - end.Y) + (p.Z - Origin.Z) * (Origin.Z - end.Z);
            u = u / ((dx * dx) + (dy * dy) + (dz * dz));

            return new Point(Origin.X + u * dx, Origin.Y + u * dy, Origin.Z + u * dz);
        }

        /// <summary>
        ///获得直线与平面的交点
        /// </summary>
        /// <param viewFace="p">平面</param>
        /// <returns>直线与三角面的交点</returns>
        public Point GetCrossPointBetweenStraightLineAndFace(Face viewFace)
        {
            SpectVector vector = viewFace.NormalVector.GetNormalizationVector();//归一化
            double temp = this.RayVector.DotMultiplied(vector);
            if (Math.Abs(temp) < 0.00001)//如果向量和平面平行则返回null
                return null;
            else
            {
                double TriD = vector.a * viewFace.Vertices[0].X + vector.b * viewFace.Vertices[0].Y + vector.c * viewFace.Vertices[0].Z;
                double temp1 = vector.a * this.Origin.X + vector.b * this.Origin.Y + vector.c * this.Origin.Z - TriD;
                double t = -temp1 / temp;
                Point tempPoint = new Point(this.RayVector.a * t + this.Origin.X, this.RayVector.b * t + this.Origin.Y, this.RayVector.c * t + this.Origin.Z);
                if (viewFace.JudgeIfPointInFace(tempPoint))
                    return tempPoint;
                else
                    return null;//如果求出的交点不在三角面内，则返回空
            }

        }

        /// <summary>
        ///获得直线与无限平面的交点
        /// </summary>
        /// <param viewFace="p">平面</param>
        /// <returns>直线与三角面的交点</returns>
        public Point GetCrossPointBetweenStraightLineAndInfiniteFace(Face viewFace)
        {
            SpectVector vector = viewFace.NormalVector.GetNormalizationVector();//归一化
            SpectVector rayVector = this.RayVector.GetNormalizationVector();
            double temp = rayVector.DotMultiplied(vector);
            if (Math.Abs(temp) < 0.0000001)//如果向量和平面平行则返回null
                return null;
            else
            {
                double TriD = vector.a * viewFace.Vertices[0].X + vector.b * viewFace.Vertices[0].Y + vector.c * viewFace.Vertices[0].Z;
                double temp1 = vector.a * this.Origin.X + vector.b * this.Origin.Y + vector.c * this.Origin.Z - TriD;
                double t = -temp1 / temp;
                Point tempPoint = new Point(rayVector.a * t + this.Origin.X, rayVector.b * t + this.Origin.Y, rayVector.c * t + this.Origin.Z);
                return tempPoint;
            }
        }

        /// <summary>
        ///获得射线与平面的交点，要判断点是否在射线方向上
        /// </summary>
        /// <param viewFace="p">平面</param>
        /// <returns>射线与三角面的交点</returns>
        public Point GetCrossPointWithFace(Face viewFace)
        {
            Point crossPoint = this.GetCrossPointBetweenStraightLineAndFace(viewFace);
            if (crossPoint != null )
            {
                if (crossPoint.X == this.Origin.X && crossPoint.Y == this.Origin.Y && crossPoint.Z == this.Origin.Z)
                {
                    return crossPoint;
                }
                else if (this.RayVector.IsParallelAndSamedirection(new SpectVector(this.Origin, crossPoint))) 
                {
                    return crossPoint;
                }
            }
            return null;
        }

        /// <summary>
        ///获得射线与无限大平面的交点，要判断点是否在射线方向上
        /// </summary>
        /// <param viewFace="p">平面</param>
        /// <returns>射线与三角面的交点</returns>
        public Point GetCrossPointWithInfiniteFace(Face viewFace)
        {
            Point crossPoint = this.GetCrossPointBetweenStraightLineAndInfiniteFace(viewFace);
            if (crossPoint != null && this.RayVector.IsParallelAndSamedirection(new SpectVector(this.Origin, crossPoint)))
            {
                return crossPoint;
            }
            return null;
        }

        /// <summary>
        ///获得射线与无限大平面的交点
        /// </summary>
        /// <param viewFace="p">平面</param>
        /// <returns>射线与三角面的交点</returns>
        public Point GetCrossPointOfStraightLineAndInfiniteFace(Face viewFace)
        {
            Point crossPoint = this.GetCrossPointBetweenStraightLineAndInfiniteFace(viewFace);
            return crossPoint;
        }
   


        /// <summary>
        ///求射线与地形的交点，返回反射面，反射点，与源点距离等信息
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <param name="terRect">地形矩形</param>
        /// <returns>返回包含反射面，反射点，与源点距离等信息</returns>
        public Node GetCrossNodeWithTerrainRects(List<Rectangle> terRect)
        {
            for (int i = 0; i < terRect.Count; i++)
            {
                Node reflecionPoint1 = this.GetCrossNodeWithOriginTriangle(terRect[i].TriangleOne);
                Node reflecionPoint2 = this.GetCrossNodeWithOriginTriangle(terRect[i].TriangleTwo);
                if (reflecionPoint1 != null && reflecionPoint2 == null && IsAvilibleCrossNodeOfRay(reflecionPoint1))
                { return reflecionPoint1; }
                else if (reflecionPoint1 == null && reflecionPoint2 != null && IsAvilibleCrossNodeOfRay(reflecionPoint2))
                { return reflecionPoint2; }
                else if (reflecionPoint1 != null && reflecionPoint2 != null)
                {
                    if (reflecionPoint1.DistanceToFrontNode < reflecionPoint2.DistanceToFrontNode && IsAvilibleCrossNodeOfRay(reflecionPoint1))
                    { return reflecionPoint1; }
                    else if(IsAvilibleCrossNodeOfRay(reflecionPoint2))
                    { return reflecionPoint2; }
                }
            }
            return null;
        }

        /// <summary>
        /// 判断一个射线与地形面的交点是否真的存在
        /// </summary>
        /// <param name="crossNode"></param>
        /// <returns></returns>
        public bool IsAvilibleCrossNodeOfRay(Node crossNode)
        {
            Face crossFace= crossNode.ReflectionFace;
            SpectVector normalVector = crossFace.NormalVector;
            if(normalVector.c < 0)
            {
                normalVector = normalVector.GetReverseVector();
            }
            for (int i = 0; i < crossFace.Lines.Count; i++)
            {
                if (crossNode.Position.isInline(crossFace.Lines[i]))
                {
                    Point otherVertice = null;
                    for (int j = 0; j < crossFace.Vertices.Count; j++)
                    {
                        if (!crossFace.Vertices[j].isInline(crossFace.Lines[i]))
                        { otherVertice = crossFace.Vertices[j]; break; }
                    }
                    if(otherVertice == null) return false;
                    if (this.RayVector.GetPhaseOfVector(new SpectVector(crossNode.Position, otherVertice)) < 90 && this.RayVector.GetPhaseOfVector(normalVector) > 90)
                        return true;
                    else
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断射线是否与绕射圆柱体相交,若相交，求得交点
        /// </summary>
        /// <param name="oneCity">建筑物</param>
        /// <returns>返回射线与建筑物绕射棱交点、与源点距离、所在面的信息</returns>
        public Node GetCrossNodeWithCylinder(AdjacentEdge diffractionEdge,double cylinderRadius)
        {
            Cylinder diffractionCylinder = new Cylinder(diffractionEdge, cylinderRadius);
            Point crossPoint = diffractionCylinder.GetCrossPointWithRay(this);
            if (crossPoint != null)
            {
                diffractionEdge.DiffCylinderRadius = cylinderRadius;
                Node diffracionCrossNode = new Node();
                diffracionCrossNode.Position = crossPoint;
                diffracionCrossNode.NodeStyle = NodeStyle.CylinderCrossNode;
                diffracionCrossNode.DistanceToFrontNode = crossPoint.GetDistance(this.Origin);
                diffracionCrossNode.RayIn = this;
                diffracionCrossNode.DiffractionEdge = diffractionEdge;
                return diffracionCrossNode;
            }
            else
            { return null; }
             

        }

        /// <summary>
        /// 求射线与四边形的交点
        /// </summary>
        /// <param oneRay="viewPoint">射线</param>
        /// <returns>返回射线与四边形的交点，若无交点则返回null</returns>
        public Node GetCrossNodeWithAreaQuadrangle(Quadrangle crossQuadrangle)
        {
            if (crossQuadrangle == null)
            { return null; }
            else
            {
                Point crossPoint = crossQuadrangle.GetCrossPointWithRay(this);
                if (crossPoint != null)
                {
                    Node crossNode = new Node();
                    crossNode.Position = crossPoint;
                    crossNode.RayIn = this;
                    crossNode.DistanceToFrontNode = crossPoint.GetDistance(this.Origin);
                    if (crossQuadrangle.FaceStyle == FaceType.Air)
                    {
                        crossNode.NodeStyle = NodeStyle.AreaCrossNode;
                    }
                    else
                    {
                        crossNode.NodeStyle = NodeStyle.ReflectionNode;
                    }
                    return crossNode;

                }
                return null;
            }
        }


        /// <summary>
        ///求射线与三角面的交点，返回反射面，反射点，与源点距离等信息
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <param name="originTri">三角面</param>
        /// <returns>返回包含反射面，反射点，与源点距离等信息</returns>
        public Node GetCrossNodeWithOriginTriangle(Triangle originTri)
        {
            if (originTri == null)
            { return null; }
            if (originTri.SubdivisionTriangle.Count != 0)//若该三角面被细分过
            {
                List<Node> crossPoints = new List<Node>();
                for (int i = 0; i < originTri.SubdivisionTriangle.Count; i++)//求每个细分三角面的交点
                {
                    Node crossPoint = this.GetReflectionNodeWithMinTriangle(originTri);
                    if (crossPoint != null)
                    { crossPoints.Add(crossPoint); }
                }
                if (crossPoints.Count == 0)//没有交点
                {
                    return null;
                }
                else if (crossPoints.Count == 1)//只有一个交点，返回该交点
                {
                    return crossPoints[0];
                }
                else//大于一个交点,选出最近的点
                {
                    Node nearestPoint = crossPoints[0];
                    for (int j = 1; j < crossPoints.Count; j++)
                    {
                        if (nearestPoint.DistanceToFrontNode > crossPoints[j].DistanceToFrontNode)
                        { nearestPoint = crossPoints[j]; }
                    }
                    return nearestPoint;
                }
            }
            else//若该三角面没有进行过细分
            {
                return this.GetReflectionNodeWithMinTriangle(originTri);
            }
        }

        /// <summary>
        /// 通过射线的起点和方向向量获取射线上的另外一点
        /// </summary>
        /// <returns>返回射线上的另外一点</returns>
        public  Point GetPointOnRayVector(double spacing)
        {
            double mag = this.RayVector.Mag();
            return new Point(this.Origin.X + this.RayVector.a / mag * spacing, this.Origin.Y + this.RayVector.b / mag * spacing, this.Origin.Z + this.RayVector.c / mag * spacing);

        }

        
        /// <summary>
        ///求异面直线公垂线在该直线上的垂足
        /// </summary>
        public Point GetFootPointWithSkewLine(RayInfo otherRay)
        {
            SpectVector verticalVector = this.RayVector.CrossMultiplied(otherRay.RayVector);//求两条异面直线的公垂线的向量
            Face viewFace = new SpaceFace(this.Origin, this.RayVector.CrossMultiplied(verticalVector));
            Point crossPoint = otherRay.GetCrossPointBetweenStraightLineAndFace(viewFace);
            return this.getDropFoot(crossPoint);
        }

        /// <summary>
        /// 获得射线与地形绕射棱交点、与源点距离、所在面的信息
        /// </summary>
        /// <param name="rays">射线</param>
        /// <param name="terShadowRects">射线所经过的地形矩形</param>
        /// <param name="txFrequencyBand">发射机频带信息</param>
        /// <param name="multiple">绕射半径系数</param>
        /// <returns>返回射线与地形绕射棱交点、与源点距离、所在面的信息</returns>
        public Node GetCrossNodeWithTerDiffractionEdge(List<Rectangle> terShadowRects,double rayTracingDistance, double rayBeamAngle)
        {
            List<Node> crossNodes = new List<Node>();
            for (int i = 0; i < terShadowRects.Count; i++)//对经过的每个地形矩形进行遍历
            {
                for (int j = 0; j < terShadowRects[i].RectTriangles.Count; j++)//对每个矩形的三角面进行遍历
                {
                    for (int k = 0; k < terShadowRects[i].RectTriangles[j].Lines.Count; k++)//对每个三角面的边进行遍历
                    {
                        //若该条边是一条绕射棱
                        if (!terShadowRects[i].RectTriangles[j].Lines[k].IsDiffractionEdge)
                        {
                            continue;
                        }
                        else
                        {
                            if (terShadowRects[i].RectTriangles[j].Lines[k].HaveTraced)//若该条绕射棱已经被追踪
                            {
                                terShadowRects[i].RectTriangles[j].Lines[k].HaveTraced = false;
                                continue;
                            }
                            else
                            {
                                terShadowRects[i].RectTriangles[j].Lines[k].HaveTraced = true;
                                Node paramNode = this.GetCrossNodeWithDiffractionEdge(terShadowRects[i].RectTriangles[j].Lines[k], rayTracingDistance, rayBeamAngle);
                                if (paramNode != null)
                                { crossNodes.Add(paramNode); }
                            }
                        }
                    }
                }
            }
            if (crossNodes.Count==0)
            { return null; }
            else if (crossNodes.Count == 1)
            { return crossNodes[0]; }
            else
            {
                //若交点个数大于两个找出最近的
                for (int m = 0; m < crossNodes.Count - 1; m++)
                {
                    if (crossNodes[m].DistanceToFrontNode < crossNodes[m + 1].DistanceToFrontNode)
                    {
                        Node param = crossNodes[m];
                        crossNodes[m] = crossNodes[m + 1];
                        crossNodes[m + 1] = param;
                    }
                }
                return crossNodes[crossNodes.Count-1];
            }
        }


        /// <summary>
        /// 深拷贝
        /// </summary>
        public Object Clone()
        {
            return new RayInfo((Point)this.Origin.Clone(), (SpectVector)this.RayVector.Clone());
        }


        /// <summary>
        ///处理绕射相交点
        /// </summary>
        /// <param name="diffractionEdge">绕射棱</param>
        /// <param name="rayTracingDistance">射线已经经过的距离</param>
        ///  <param name="rayBeamAngle">射线束夹角</param>
        /// <returns>处理后的绕射相交点</returns>
        private Node GetCrossNodeWithDiffractionEdge(AdjacentEdge diffractionEdge, double rayTracingDistance, double rayBeamAngle)
        {
            //判断凹凸性
            if (this.Origin.JudgeIsConcaveOrConvexToViewPoint(diffractionEdge.AdjacentTriangles[0], diffractionEdge.AdjacentTriangles[1]))
            {
                double rxBallDistance = this.Origin.GetDistanceToFace(new SpaceFace(diffractionEdge.StartPoint,this.RayVector)) + rayTracingDistance;
                double radius = 0.809 * rxBallDistance * rayBeamAngle / Math.Sqrt(3);
                Node paramNode = this.GetCrossNodeWithCylinder(diffractionEdge, radius);
                if (paramNode != null)//若存在交点
                {
                    //当发射点与绕射棱在一个三角面内,或者发射点在绕射圆柱体内时，该绕射点不算
                    if (!diffractionEdge.AdjacentTriangles[0].JudgeIfPointInFace(this.Origin) &&
                        !diffractionEdge.AdjacentTriangles[1].JudgeIfPointInFace(this.Origin))
                    {
                         return paramNode;
                    }
                }
            }
            return null;
        }




        /// <summary>
        /// 获得射线与三角面交点、与源点距离、所在面的信息
        /// </summary>
        /// <param name="oneTri">三角面</param>
        /// <returns>返回射线与三角面交点、与源点距离、所在面的信息</returns>
        private Node GetReflectionNodeWithMinTriangle(Triangle oneTri)
        {
            Point CrossPoint = this.GetCrossPointWithFace(oneTri);//求与三角面所在的交点
            if (CrossPoint != null)//如果有交点
            {
                if (CrossPoint.GetDistance(this.Origin) > 0.001)//若反射点到源点的距离大于0.1m，该反射点可要
                {
                    Node crossNode = new Node();
                    crossNode.Position = CrossPoint;
                    crossNode.RayIn =this;
                    crossNode.DistanceToFrontNode = CrossPoint.GetDistance(this.Origin);
                    if (oneTri.FaceStyle == FaceType.EFieldArea)//相交面为态势区域的面
                    {
                        crossNode.NodeStyle = NodeStyle.AreaCrossNode;
                    }
                    else if (oneTri.FaceStyle == FaceType.Terrian)//相交面为地形
                    {
                        crossNode.ReflectionFace = oneTri;
                        crossNode.NodeStyle = NodeStyle.ReflectionNode;
                        crossNode.IsInTer = true;
                    }
                    else//相交面为建筑物
                    {
                        crossNode.ReflectionFace = oneTri;
                        crossNode.NodeStyle = NodeStyle.ReflectionNode;
                    }
                    return crossNode;
                        
                    
                }
            }
            return null;
        }

      

    }

    /// <summary>
    ///射线方向向量
    /// </summary>
    public class SpectVector :ICloneable
    {
        public double a, b, c;
        public SpectVector(Point origin, Point target)
        {
            a = target.X - origin.X;
            b = target.Y - origin.Y;
            c = target.Z - origin.Z;
        }
        public SpectVector(double x, double y, double z)
        {
            a = x; b = y; c = z;
        }

        /// <summary>
        ///求向量的模
        /// </summary>
        public double Mag()
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2));
        }

        /// <summary>
        ///求向量在XY平面投影的模
        /// </summary>
        public double GetMagInXY()
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        /// <summary>
        ///判断两个向量是否平行且同向
        /// </summary>
        public bool IsParallelAndSamedirection(SpectVector linevector)
        {
            if (linevector.a == 0 && linevector.b == 0 && linevector.c == 0)
            {
                LogFileManager.ObjLog.error("判断两个向量是否平行且同向时，输入的是0向量");
                return false;
            }
            double temp = (linevector.a * a + linevector.b * b + linevector.c * c) / (Mag() * linevector.Mag());
            if (Math.Abs(temp - 1) <= 0.00000001)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        ///判断两个向量是否平行
        /// </summary>
        public bool IsParallel(SpectVector linevector)
        {
            double temp = (linevector.a * a + linevector.b * b + linevector.c * c) / (Mag() * linevector.Mag());
            if (Math.Abs(temp - 1) <= 0.00000001 || Math.Abs(temp + 1) <= 0.00000001)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        ///判断两个向量是否垂直
        /// </summary>
        public bool IsVertical(SpectVector linevector)
        {
            double temp = linevector.a * a + linevector.b * b + linevector.c * c;
            if (Math.Abs(temp) <= 0.00000001)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        ///判断两个向量在XY平面的投影是否平行且同向
        /// </summary>
        public bool IsParallelAndSamedirectionInXYPlane(SpectVector linevector)
        {
            double temp = (linevector.a * a + linevector.b * b) / (GetMagInXY() * linevector.GetMagInXY());
            if (Math.Abs(temp - 1) <= 0.00000001)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        ///求向量的反向向量
        /// </summary>
        public SpectVector GetReverseVector()
        {
            return new SpectVector(-this.a, -this.b, -this.c);
        }

        /// <summary>
        ///求两个向量的中间向量
        /// </summary>
        public SpectVector GetMiddleVectorOfTwoVectors(SpectVector otherVector)
        {
            SpectVector unitVector1 = this.GetNormalizationVector();
            SpectVector unitVector2 = otherVector.GetNormalizationVector();
            return new SpectVector((unitVector1.a + unitVector2.a) / 2, (unitVector1.b + unitVector2.b) / 2, (unitVector1.c + unitVector2.c) / 2);
        }

        /// <summary>
        ///矢量叉乘运算
        /// </summary>
        public SpectVector CrossMultiplied(SpectVector anotherVector)
        {
            double x, y, z, xTemp, yTemp, zTemp, multipled;
            xTemp = b * anotherVector.c - c * anotherVector.b;
            yTemp = c * anotherVector.a - a * anotherVector.c;
            zTemp = a * anotherVector.b - b * anotherVector.a;
            multipled = Math.Sqrt(Math.Pow(xTemp, 2) + Math.Pow(yTemp, 2) + Math.Pow(zTemp, 2));
            if (multipled == 0)
                x = y = z = 0;
            else
            {
                x = xTemp / multipled;
                y = yTemp / multipled;
                z = zTemp / multipled;
            }
            SpectVector n = new SpectVector(x, y, z);
            return n;
        }

        /// <summary>
        ///矢量叉乘运算(得到的向量为单位向量)
        /// </summary>
        public static SpectVector VectorCrossMultiply(SpectVector k, SpectVector l)
        {
            double x, y, z, x1, y1, z1, m;
            x1 = k.b * l.c - k.c * l.b;
            y1 = k.c * l.a - k.a * l.c;
            z1 = k.a * l.b - k.b * l.a;
            m = Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2) + Math.Pow(z1, 2));
            if (m == 0)
                x = y = z = 0;
            else
            {
                x = x1 / m;
                y = y1 / m;
                z = z1 / m;
            }
            SpectVector n = new SpectVector(x, y, z);
            return n;
        }

        /// <summary>
        ///矢量点乘运算
        /// </summary>
        public static double VectorDotMultiply(SpectVector k, SpectVector l)
        {
            double m = k.a * l.a + k.b * l.b + k.c * l.c;
            return m;
        }

        /// <summary>
        ///复数与矢量的点乘运算
        /// </summary>
        public static EField VectorDotMultiply(Plural p, SpectVector l)
        {
            Plural X = new Plural(l.a * p.Re, l.a * p.Im);
            Plural Y = new Plural(l.b * p.Re, l.b * p.Im);
            Plural Z = new Plural(l.c * p.Re, l.c * p.Im);
            EField e = new EField(X, Y, Z);
            return e;
        }

        /// <summary>
        ///复数向量与矢量的点乘运算
        /// </summary>
        public static Plural VectorDotMultiply(EField e, SpectVector l)
        {
            Plural p = new Plural();
            p.Re = e.X.Re * l.a + e.Y.Re * l.b + e.Z.Re * l.c;
            p.Im = e.X.Im * l.a + e.Y.Im * l.b + e.Z.Im * l.c;
            return p;
        }

        /// <summary>
        ///矢量与标量的点乘
        /// </summary>
        public static SpectVector VectorDotMultiply(double d, SpectVector l)
        {
            SpectVector s = new SpectVector(l.a * d, l.b * d, l.c * d);
            return s;
        }

        /// <summary>
        ///矢量点乘
        /// </summary>
        public double DotMultiplied(SpectVector anotherVector)
        //调用方法：thisVector.DotMultiplied(anotherVector)
        {
            double multipled = a * anotherVector.a + b * anotherVector.b + c * anotherVector.c;
            return multipled;
        }

        //复数与矢量的点乘运算
        // 原函数public static Efield VectorDotMultiply(Plural p, SpectVector l)
        public EField DotMultiplied(Plural pValue)
        //this.DotMultiplied(p)
        {
            Plural X = new Plural(this.a * pValue.Re, this.a * pValue.Im);
            Plural Y = new Plural(this.b * pValue.Re, this.b * pValue.Im);
            Plural Z = new Plural(this.c * pValue.Re, this.c * pValue.Im);
            EField e = new EField(X, Y, Z);
            return e;
        }

        //复数向量与矢量的点乘运算
        //原函数：public static Plural VectorDotMultiply(Efield e, SpectVector l)
        public Plural DotMultiplied(EField eValue)
        {
            Plural p = new Plural();
            p.Re = eValue.X.Re * this.a + eValue.Y.Re * this.b + eValue.Z.Re * this.c;
            p.Im = eValue.X.Im * this.a + eValue.Y.Im * this.b + eValue.Z.Im * this.c;
            return p;
        }

        /// <summary>
        ///矢量夹角计算
        /// </summary>
        public static double VectorPhase(SpectVector k, SpectVector l)
        {
            double m, n;
            m = VectorDotMultiply(k, l);
            double temp = Math.Sqrt(Math.Pow(k.a, 2) + Math.Pow(k.b, 2) + Math.Pow(k.c, 2)) * Math.Sqrt(Math.Pow(l.a, 2) + Math.Pow(l.b, 2) + Math.Pow(l.c, 2));
            if (temp == 0)
                n = 0;
            else
                n = m / temp;
            if (n < -1)
            { n = -1; }
            if (n > 1)
            { n = 1; }
            double phase = Math.Acos(n) / Math.PI * 180;
            return phase;
        }

        /// <summary>
        ///矢量夹角计算
        /// </summary>
        public double GetPhaseOfVector(SpectVector anotherVector)
        {
            double multipliedTemp, nTemp;
            multipliedTemp = this.DotMultiplied(anotherVector);
            double tempValue = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2)) * Math.Sqrt(Math.Pow(anotherVector.a, 2) + Math.Pow(anotherVector.b, 2) + Math.Pow(anotherVector.c, 2));
            if (tempValue == 0)
                nTemp = 0;
            else
                nTemp = multipliedTemp / tempValue;
            if (nTemp < -1)
            { nTemp = -1; }
            if (nTemp > 1)
            { nTemp = 1; }
            double phase = Math.Acos(nTemp) / Math.PI * 180;
            return phase;
        }

        /// <summary>
        ///求单位向量
        /// </summary>
        public SpectVector GetNormalizationVector()
        {
            double sum = Math.Sqrt(Math.Pow(this.a, 2) + Math.Pow(this.b, 2) + Math.Pow(this.c, 2));
            if (sum < 0.000000000000000000001)
                return new SpectVector(0, 0, 0);
            return new SpectVector(this.a / sum, this.b / sum, this.c / sum);
        }


        public SpectVector GetRightRotationVector(SpectVector vector, double rotationAngle)
        {
            double[] normalizations = new double[4];
            double[] rotateparameters = new double[9];
            //normalizations中四个值分别为：旋转轴的单位方向向量三个分量x，y，z，和旋转角度弧度值。
            SpectVector rotationVector = vector.GetNormalizationVector();
            normalizations[0] = rotationVector.a;
            normalizations[1] = rotationVector.b;
            normalizations[2] = rotationVector.c;
            normalizations[3] = rotationAngle / 180 * Math.PI;//转换为弧度值
            //旋转矩阵
            rotateparameters[0] = Math.Cos(normalizations[3]) + normalizations[0] * normalizations[0] * (1 - Math.Cos(normalizations[3]));
            rotateparameters[1] = normalizations[0] * normalizations[1] * (1 - Math.Cos(normalizations[3])) + normalizations[2] * Math.Sin(normalizations[3]);
            rotateparameters[2] = normalizations[0] * normalizations[2] * (1 - Math.Cos(normalizations[3])) - normalizations[1] * Math.Sin(normalizations[3]);
            rotateparameters[3] = normalizations[0] * normalizations[1] * (1 - Math.Cos(normalizations[3])) - normalizations[2] * Math.Sin(normalizations[3]);
            rotateparameters[4] = Math.Cos(normalizations[3]) + normalizations[1] * normalizations[1] * (1 - Math.Cos(normalizations[3]));
            rotateparameters[5] = normalizations[1] * normalizations[2] * (1 - Math.Cos(normalizations[3])) + normalizations[0] * Math.Sin(normalizations[3]);
            rotateparameters[6] = normalizations[0] * normalizations[2] * (1 - Math.Cos(normalizations[3])) + normalizations[1] * Math.Sin(normalizations[3]);
            rotateparameters[7] = normalizations[1] * normalizations[2] * (1 - Math.Cos(normalizations[3])) - normalizations[0] * Math.Sin(normalizations[3]);
            rotateparameters[8] = Math.Cos(normalizations[3]) + normalizations[2] * normalizations[2] * (1 - Math.Cos(normalizations[3]));
            double xtemp = this.a * rotateparameters[0] + this.b * rotateparameters[3] + this.c * rotateparameters[6];
            double ytemp = this.a * rotateparameters[1] + this.b * rotateparameters[4] + this.c * rotateparameters[7];
            double ztemp = this.a * rotateparameters[2] + this.b * rotateparameters[5] + this.c * rotateparameters[8];
            return new SpectVector(xtemp, ytemp, ztemp);

        }


        /// <summary>
        ///深拷贝
        /// </summary>
        public Object Clone()
        {
            return new SpectVector(this.a, this.b, this.c);
        }

    }
}
