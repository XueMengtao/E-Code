using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    public class Cylinder
    {
        private Point bottomCirclePoint;
        private Point topCirclePoint;
        private double radius;

        public Cylinder()
        { }

        public Cylinder(LineSegment line, double radius)
        {
            this.bottomCirclePoint = line.StartPoint;
            this.topCirclePoint = line.EndPoint;
            this.radius = radius;
        }

        public Cylinder(Point bottomCirclePoint,Point topCirclePoint,double radius)
        {
            this.bottomCirclePoint = bottomCirclePoint;
            this.topCirclePoint = topCirclePoint;
            this.radius = radius;
        }

        public Point GetCrossPointWithRay(RayInfo oneRay)
        {
            double high = this.bottomCirclePoint.GetDistance(this.topCirclePoint);
            List<Point> crossPoints = this.Intersect(this.bottomCirclePoint, this.topCirclePoint, this.radius, high, oneRay.Origin, oneRay.GetPointOnRayVector(1));
            if (crossPoints.Count == 1)
            { return crossPoints[0]; }
            if (crossPoints.Count > 1)
            {
                Point nearestPoint = crossPoints[0];
                for (int j = 1; j < crossPoints.Count; j++)
                {
                    if (nearestPoint.GetDistance(oneRay.Origin) > crossPoints[j].GetDistance(oneRay.Origin))
                    { nearestPoint = crossPoints[j]; }
                }
                return nearestPoint;
            }
            return null;

        }



        /// 射线与圆柱体相交算法
        /// </summary>
        /// <param name="bottomCirclePoint">圆柱下底面圆心在世界坐标系的坐标</param>
        /// <param name="topCirclePoint">圆柱上底面圆心在世界坐标系的坐标</param>
        /// <param name="rayStartPoint">射线起点在世界坐标系的坐标</param>
        /// <param name="rayEndPoint">射线终点在世界坐标系的坐标</param>
        /// <param name="radius">圆柱体底面半径</param>
        /// <param name="high">圆柱体的高</param>
        /// <param name="newRayStartPoint">射线起点在观察坐标系的坐标</param>
        /// <param name="newRayEndPoint">射线终点在观察坐标系的坐标</param>
        /// <param name="save1">射线与圆柱体的第一个交点</param>
        /// <param name="save2">射线与圆柱体的第二个交点</param>
        /// <returns></returns>
        public List<Point> Intersect(Point bottomCirclePoint, Point topCirclePoint, double radius, double high, Point rayStartPoint, Point rayEndPoint)
        {
            Point save1 = new Point();
            Point save2 = new Point();
            Point newRayStartPoint = new Point();
            Point newRayEndPoint = new Point();
            newRayStartPoint = CoordinateTransformation(bottomCirclePoint, topCirclePoint, rayStartPoint);//坐标变换后射线起点的坐标
            newRayEndPoint = CoordinateTransformation(bottomCirclePoint, topCirclePoint, rayEndPoint);
            double x0 = newRayStartPoint.X;   //下面的赋值是为了复杂的数学计算
            double y0 = newRayStartPoint.Y;
            double z0 = newRayStartPoint.Z;
            double a = newRayEndPoint.X - newRayStartPoint.X; //向量的坐标变换与点不同，不能直接对向量进行坐标变换，需要先找到射线上的两点，分别进行坐标变换，然后相减得到射线在新坐标系中的方向向量
            double b = newRayEndPoint.Y - newRayStartPoint.Y;
            double c = newRayEndPoint.Z - newRayStartPoint.Z;
            double r = radius;
            double h = high;
            double t1 = 0;             //计算的中间变量，具体内容见设计文档
            double t2 = 0;             //计算的中间变量，具体内容见设计文档
            double outZ1 = 0;          //计算的中间变量，具体内容见设计文档
            double outZ2 = 0;          //计算的中间变量，具体内容见设计文档
            List<Point> intersection = new List<Point>();
            double delta = 4 * Math.Pow(a * x0 + b * y0, 2) - 4 * (Math.Pow(a, 2) + Math.Pow(b, 2)) * (Math.Pow(x0, 2) + Math.Pow(y0, 2) - Math.Pow(r, 2));
            if (delta >= 0 && (Math.Pow(a, 2) + Math.Pow(b, 2)) != 0)//解出两个根
            {
                t1 = ((-2) * (a * x0 + b * y0) - Math.Sqrt(delta)) / (2 * Math.Pow(a, 2) + 2 * Math.Pow(b, 2));
                t2 = ((-2) * (a * x0 + b * y0) + Math.Sqrt(delta)) / (2 * Math.Pow(a, 2) + 2 * Math.Pow(b, 2));
                outZ1 = z0 + t1 * c;
                outZ2 = z0 + t2 * c;
            }


            //情况1 射线在圆柱底面上
            if (c == 0 && (z0 == 0 || z0 == h) && delta > 0 && t1 >= 0 && t2 >= 0)
            {
                save1.X = x0 + t1 * a;
                save1.Y = y0 + t1 * b;
                save1.Z = z0 + t1 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                return intersection;
            }
            //情况2  射线在圆柱侧面上
            else if (a == 0 && b == 0 && (Math.Pow(x0, 2) + Math.Pow(y0, 2) == Math.Pow(r, 2)))
            {
                if (z0 <= 0 && c > 0)
                {
                    save1.X = x0;
                    save1.Y = y0;
                    save1.Z = 0;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else if (z0 >= h && c < 0)
                {
                    save1.X = x0;
                    save1.Y = y0;
                    save1.Z = h;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else
                    return new List<Point>();

            }
            //情况3  射线与在圆柱有两个交点，但与底面不相交
            else if (delta > 0 && !(c == 0 && (z0 == 0 || z0 == h)) && outZ1 >= 0 && outZ1 <= h && outZ2 >= 0 && outZ2 <= h)
            {
                if ((c >= 0 && outZ1 >= z0) || (c <= 0 && outZ1 <= z0))
                {
                    save1.X = x0 + t1 * a;
                    save1.Y = y0 + t1 * b;
                    save1.Z = z0 + t1 * c;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    save2.X = x0 + t2 * a;
                    save2.Y = y0 + t2 * b;
                    save2.Z = z0 + t2 * c;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                    return intersection;
                }
                else
                    return new List<Point>();
            }

            //情况4 射线与在圆柱有两个交点，与底面有且只有一个交点
            else if (delta > 0 && c > 0 && outZ1 > 0 && outZ1 < h && outZ2 >= h && outZ1 >= z0)
            {
                double t3 = (h - z0) / c;
                save1.X = x0 + t1 * a;
                save1.Y = y0 + t1 * b;
                save1.Z = z0 + t1 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0 + t3 * a;
                save2.Y = y0 + t3 * b;
                save2.Z = z0 + t3 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }
            else if (delta > 0 && c < 0 && outZ2 > 0 && outZ2 < h && outZ1 >= h && z0 >= h)
            {
                double t3 = (h - z0) / c;
                save1.X = x0 + t2 * a;
                save1.Y = y0 + t2 * b;
                save1.Z = z0 + t2 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0 + t3 * a;
                save2.Y = y0 + t3 * b;
                save2.Z = z0 + t3 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }
            else if (delta > 0 && c > 0 && outZ2 > 0 && outZ2 < h && outZ1 <= 0 && z0 <= 0)
            {
                double t3 = (-z0) / c;
                save1.X = x0 + t2 * a;
                save1.Y = y0 + t2 * b;
                save1.Z = z0 + t2 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0 + t3 * a;
                save2.Y = y0 + t3 * b;
                save2.Z = z0 + t3 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }
            else if (delta > 0 && c < 0 && outZ1 > 0 && outZ1 < h && outZ2 <= 0 && z0 >= outZ1)
            {
                double t3 = (-z0) / c;
                save1.X = x0 + t1 * a;
                save1.Y = y0 + t1 * b;
                save1.Z = z0 + t1 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0 + t3 * a;
                save2.Y = y0 + t3 * b;
                save2.Z = z0 + t3 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }

            //情况5  射线与在圆柱有两个交点，与底面有两个交点
            else if (delta > 0 && c > 0 && outZ1 <= 0 && outZ2 >= h && z0 <= 0)
            {

                double t3 = -z0 / c;
                double t4 = (h - z0) / c;
                save1.X = x0 + t3 * a;
                save1.Y = y0 + t3 * b;
                save1.Z = z0 + t3 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0 + t4 * a;
                save2.Y = y0 + t4 * b;
                save2.Z = z0 + t4 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }
            else if (delta > 0 && c < 0 && outZ1 >= h && outZ2 <= 0 && z0 >= h)
            {
                double t3 = -z0 / c;
                double t4 = (h - z0) / c;
                save1.X = x0 + t3 * a;
                save1.Y = y0 + t3 * b;
                save1.Z = z0 + t3 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0 + t4 * a;
                save2.Y = y0 + t4 * b;
                save2.Z = z0 + t4 * c;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }
            else if (a == 0 && b == 0 && (Math.Pow(x0, 2) + Math.Pow(y0, 2) < Math.Pow(r, 2)) && ((c > 0 && z0 <= 0) || (c < 0 && z0 >= h)))
            {
                save1.X = x0;
                save1.Y = y0;
                save1.Z = 0;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                save2.X = x0;
                save2.Y = y0;
                save2.Z = h;
                intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save2));
                return intersection;
            }


           //情况6  射线与圆柱体有一个交点，射线与圆柱体相切
            else if (delta == 0 && (Math.Pow(a, 2) + Math.Pow(b, 2)) != 0 && outZ1 >= 0 && outZ1 <= h)
            {
                if ((c >= 0 && outZ1 >= z0) || (c <= 0 && outZ1 <= z0))
                {
                    save1.X = x0 + t1 * a;
                    save1.Y = y0 + t1 * b;
                    save1.Z = z0 + t1 * c;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else
                    return new List<Point>();
            }
            //情况7  射线与圆柱体有一个交点的另一种情况
            else if (delta > 0 && c > 0 && outZ1 == h)
            {
                if (outZ1 >= z0)
                {
                    save1.X = x0 + t1 * a;
                    save1.Y = y0 + t1 * b;
                    save1.Z = h;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else
                    return new List<Point>();

            }
            else if (delta > 0 && c > 0 && outZ2 == 0)
            {
                if (outZ2 >= z0)
                {
                    save1.X = x0 + t2 * a;
                    save1.Y = y0 + t2 * b;
                    save1.Z = 0;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else
                    return new List<Point>();
            }
            else if (delta > 0 && c < 0 && outZ1 == 0)
            {
                if (outZ1 <= z0)
                {
                    save1.X = x0 + t1 * a;
                    save1.Y = y0 + t1 * b;
                    save1.Z = 0;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else
                    return new List<Point>();
            }
            else if (delta > 0 && c < 0 && outZ2 == h)
            {
                if (outZ2 <= z0)
                {
                    save1.X = x0 + t2 * a;
                    save1.Y = y0 + t2 * b;
                    save1.Z = h;
                    intersection.Add(InverseCoordinateTransformation(bottomCirclePoint, topCirclePoint, save1));
                    return intersection;
                }
                else
                    return new List<Point>();
            }
            //情况8 其他情况下返回空
            else
                return new List<Point>();

        }

        /// <summary>
        /// 世界坐标系到观察坐标系的变换
        /// </summary>
        /// <param name="Point">点在世界坐标系的坐标</param>
        /// <param name="outPoint">点在观察坐标系的坐标</param>
        /// <returns></returns>
        private Point CoordinateTransformation(Point bottomCirclePoint, Point topCirclePoint, Point point)
        {
            double x1 = bottomCirclePoint.X;
            double y1 = bottomCirclePoint.Y;
            double z1 = bottomCirclePoint.Z;
            double x2 = topCirclePoint.X;
            double y2 = topCirclePoint.Y;
            double z2 = topCirclePoint.Z;
            double u = point.X;
            double v = point.Y;
            double w = point.Z;
            Point outPoint = new Point();
            double cosA = (x2 - x1) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
            double cosB = (y2 - y1) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
            double cosC = (z2 - z1) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
            if (cosA == 0 && cosB == 0)
            {
                outPoint.X = u - x1;
                outPoint.Y = v - y1;
                outPoint.Z = w - z1;

            }
            else
            {
                double m = (-x1) * cosA * cosC / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) - y1 * cosB * cosC / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + z1 * Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2));
                double n = x1 * cosB / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) - y1 * cosA / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2));
                double l = -x1 * cosA - y1 * cosB - z1 * cosC;
                outPoint.X = u * cosA * cosC / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + v * cosB * cosC / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) - w * Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + m;
                outPoint.Y = u * (-cosB) / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + v * cosA / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + n;
                outPoint.Z = u * cosA + v * cosB + w * cosC + l;
            }
            return outPoint;
        }

        /// <summary>
        /// 观察坐标系到世界坐标系的变换
        /// </summary>
        /// <param name="Point">点在观察坐标系的坐标</param>
        /// <param name="outPoint">点在世界坐标系的坐标</param>
        /// <returns></returns>
        private Point InverseCoordinateTransformation(Point bottomCirclePoint, Point topCirclePoint, Point point)
        {
            double x1 = bottomCirclePoint.X;
            double y1 = bottomCirclePoint.Y;
            double z1 = bottomCirclePoint.Z;
            double x2 = topCirclePoint.X;
            double y2 = topCirclePoint.Y;
            double z2 = topCirclePoint.Z;
            double u = point.X;
            double v = point.Y;
            double w = point.Z;
            Point outPoint = new Point();
            double cosA = (x2 - x1) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
            double cosB = (y2 - y1) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
            double cosC = (z2 - z1) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
            if (cosA == 0 && cosB == 0)
            {
                outPoint.X = u + x1;
                outPoint.Y = v + y1;
                outPoint.Z = w + z1;

            }
            else
            {
                outPoint.X = u * cosA * cosB / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) - v * cosB / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + w * cosA + x1;
                outPoint.Y = u * cosB * cosC / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + v * cosA / Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + w * cosB + y1;
                outPoint.Z = -u * Math.Sqrt(Math.Pow(cosA, 2) + Math.Pow(cosB, 2)) + w * cosC + z1;
            }
            return outPoint;
        }

        /// <summary>
        /// 求被分割一部分的圆柱体体表面的三角面单元，现在不用
        /// </summary>
        /// <param name="divisionNumber">一圈的三角形的个数</param>
        /// <param name="surplsAngle">被切后圆截面所剩的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns>圆柱体体表面的三角面单元</returns>
        public List<List<Triangle>> GetSurfaceDivisionTriangles(int divisionNumber, double surplsAngle, SpectVector unitVectorU, SpectVector unitVectorV)
        {
            List<List<Triangle>> divisionTriangles = new List<List<Triangle>>();
            List<List<Point>> trianglePoints;
            if (this.bottomCirclePoint.GetDistance(this.topCirclePoint) < 0.1)
            {
                trianglePoints = this.GetSmallSurfaceDivisionTrianglePoints(divisionNumber, surplsAngle, unitVectorU, unitVectorV);
            }
            else
            {
                trianglePoints = this.GetSurfaceDivisionTrianglePoints(divisionNumber, surplsAngle, unitVectorU, unitVectorV);
            }
            for (int i = 0; i < trianglePoints.Count - 1; i++)
            {
                List<Triangle> circleTriangles = new List<Triangle>();
                if (i % 2 == 0)//偶数层
                {
                    for (int m = 0; m < trianglePoints[i].Count - 1; m++)//正立的三角形
                    { circleTriangles.Add(new Triangle(trianglePoints[i][m], trianglePoints[i][m + 1], trianglePoints[i + 1][m + 1])); }
                    for (int n = 0; n < trianglePoints[i + 1].Count - 1; n++)//倒立的三角形
                    { circleTriangles.Add(new Triangle(trianglePoints[i + 1][n], trianglePoints[i + 1][n + 1], trianglePoints[i][n])); }
                }
                else//奇数层
                {
                    for (int m = 0; m < trianglePoints[i].Count - 1; m++)//正立的三角形
                    { circleTriangles.Add(new Triangle(trianglePoints[i][m], trianglePoints[i][m + 1], trianglePoints[i + 1][m])); }
                    for (int n = 0; n < trianglePoints[i + 1].Count - 1; n++)//倒立的三角形
                    { circleTriangles.Add(new Triangle(trianglePoints[i + 1][n], trianglePoints[i + 1][n + 1], trianglePoints[i][n + 1])); }
                }
                divisionTriangles.Add(circleTriangles);
            }
            return divisionTriangles;
        }

        /// <summary>
        /// 求被分割一部分的圆柱体表面每一圈的点
        /// </summary>
        /// <param name="divisionNumber">一圈的三角形的个数</param>
        /// <param name="surplsAngle">被切后圆截面所剩的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns></returns>
        private List<List<Point>> GetSurfaceDivisionTrianglePoints(int divisionNumber, double surplsAngle, SpectVector unitVectorU, SpectVector unitVectorV)
        {
            List<List<Point>> divisionTrianglePoints = new List<List<Point>>();
            RayInfo ZVectorRay = new RayInfo(this.bottomCirclePoint, this.topCirclePoint);
            double H = this.topCirclePoint.GetDistance(this.bottomCirclePoint);
            double L =surplsAngle / 360 * 2 * Math.PI * this.radius;
            double a = L / divisionNumber;
            double h = Math.Sqrt(3) * a / 2;
            int j = 0;
            double sumd = 0;
            double beta = (surplsAngle * Math.PI / 180) / (2 * divisionNumber);
            bool flag = true;
            Point currentCenterPoint;
            while (flag)
            {
                if (sumd < H)
                {
                    currentCenterPoint = ZVectorRay.GetPointOnRayVector(j * h);
                }
                else
                {
                    flag = false;
                    currentCenterPoint = this.topCirclePoint;
                }
                List<Point> circlePoints = new List<Point>();
                //第一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, 0.01));
                int i = 1;
                if (j % 2 == 0)//处于偶数层时
                {
                    while (i < divisionNumber )
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, 2 * i * beta));
                        i++;
                    }
                }
                else//处于奇数层，奇数层要比偶数层多一个点
                {
                    while (i <= divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, (2 * i-1) * beta));
                        i++;
                    }
                }
                //最后一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, surplsAngle * Math.PI / 180 - 0.01));
                divisionTrianglePoints.Add(circlePoints);
                j++;
                sumd += h;
            }
            return divisionTrianglePoints;
        }

        /// <summary>
        /// 求细小的被分割一部分的圆柱体表面每一圈的点，只取两个圆的点
        /// </summary>
        /// <param name="divisionNumber">一圈的三角形的个数</param>
        /// <param name="surplsAngle">被切后圆截面所剩的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns></returns>
        private List<List<Point>> GetSmallSurfaceDivisionTrianglePoints(int divisionNumber, double surplsAngle, SpectVector unitVectorU, SpectVector unitVectorV)
        {
            //参考论文：基本体素的三角形面单元剖分方法-李鸿亮
            //现在每个圆上取点，再组成三角面
            List<List<Point>> divisionTrianglePoints = new List<List<Point>>();
            RayInfo ZVectorRay = new RayInfo(this.bottomCirclePoint, this.topCirclePoint);
            double H = this.topCirclePoint.GetDistance(this.bottomCirclePoint);
            double L = surplsAngle / 360 * 2 * Math.PI * this.radius;
            double a = L / divisionNumber;
            double beta = (surplsAngle * Math.PI / 180) / (2 * divisionNumber);
            Point currentCenterPoint = this.bottomCirclePoint;
            for (int j = 0; j < 2; j++)
            {
                if (j == 1)
                { currentCenterPoint = this.topCirclePoint; }
                List<Point> circlePoints = new List<Point>();
                //第一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, 0.01));
                int i = 1;
                if (j % 2 == 0)//处于偶数层时
                {
                    while (i < divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, 2 * i * beta));
                        i++;
                    }
                }
                else//处于奇数层，奇数层要比偶数层多一个点
                {
                    while (i <= divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, (2 * i - 1) * beta));
                        i++;
                    }
                }
                //最后一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, this.radius, unitVectorU, unitVectorV, surplsAngle * Math.PI / 180 - 0.01));
                divisionTrianglePoints.Add(circlePoints);
            }
            return divisionTrianglePoints;
        }



        /// <summary>
        /// 根据输入的角度求圆上的一点
        /// </summary>
        /// <param name="circleCenterPoint">圆心</param>
        /// <param name="circleRadius">半径</param>
        /// <param name="unitVectorU">圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">圆所在平面上一个向量与U垂足的向量V</param>
        /// <param name="arcAngle">角度</param>
        /// <returns></returns>
        private Point GetPointInCircumference(Point circleCenterPoint, double circleRadius, SpectVector unitVectorU, SpectVector unitVectorV, double arcAngle)
        {
            Point circumferencePoint = new Point();
            circumferencePoint.X = circleCenterPoint.X + circleRadius * (unitVectorU.a * Math.Cos(arcAngle) + unitVectorV.a * Math.Sin(arcAngle));
            circumferencePoint.Y = circleCenterPoint.Y + circleRadius * (unitVectorU.b * Math.Cos(arcAngle) + unitVectorV.b * Math.Sin(arcAngle));
            circumferencePoint.Z = circleCenterPoint.Z + circleRadius * (unitVectorU.c * Math.Cos(arcAngle) + unitVectorV.c * Math.Sin(arcAngle));
            return circumferencePoint;
        }
      
    } 
}
