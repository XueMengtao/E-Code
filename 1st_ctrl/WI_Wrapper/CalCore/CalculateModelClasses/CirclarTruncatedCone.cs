using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{


    /// <summary>
    /// 圆台体
    /// </summary>
    public  class CirclarTruncatedCone
    {
        private Point bottomCenterPoint;//下底面圆心
        private Point topCenterPoint;//上底面圆心
        private double bottomCircleRadius;//下底面半径
        private double topCircleRadius;//上底面半径

        public Point BottomCenterPoint
        {
            get { return this.bottomCenterPoint; }
        }
        public Point TopCenterPoint
        {
            get { return this.topCenterPoint; }
        }
        public double BottomCircleRadius
        {
            get { return this.bottomCircleRadius; }
        }
        public double TopCircleRadius
        {
            get { return this.topCircleRadius; }
        }

        public CirclarTruncatedCone(Point bottomCenterPoint, double bottomCircleRadius, Point topCenterPoint, double topCircleRadius)
        {
            this.bottomCenterPoint = bottomCenterPoint;
            this.bottomCircleRadius = bottomCircleRadius;
            this.topCenterPoint = topCenterPoint;
            this.topCircleRadius = topCircleRadius;
        }


        /// <summary>
        /// 求被分割一部分的圆台体表面的三角面单元
        /// </summary>
        /// <param name="divisionNumber">圆弧角的细分次数</param>
        /// <param name="surplsAngle">下底面所剩部分的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns></returns>
        public  List<List<Triangle>> GetTruncatedSurfaceDivisionTriangles( int divisionNumber, double surplsAngle,SpectVector unitVectorU, SpectVector unitVectorV)
        {
            //参考论文：基本体素的三角形面单元剖分方法
            List<List<Triangle>> divisinTriangles = new List<List<Triangle>>();
            RayInfo circleCenterPointsRay = new RayInfo(this.bottomCenterPoint, this.topCenterPoint);
            double h = this.bottomCenterPoint.GetDistance(this.topCenterPoint);//圆台体的高
            double l = Math.Sqrt((h * h + Math.Pow(this.bottomCircleRadius - this.topCircleRadius, 2)) );
            double L = this.bottomCircleRadius * l / (this.bottomCircleRadius - this.topCircleRadius);
            double sinB = h / l;
            double theta = 2 * Math.PI * this.bottomCircleRadius / L;//弧度制
            double particalTheta = surplsAngle / 360 * theta;
            int j = 0;
            bool flag = true;
            double alpha = particalTheta / divisionNumber;
            double triangleSideLength = 2 * L * Math.Sin(alpha / 2);//正三角形初始边长
            double d = Math.Sqrt(3) * triangleSideLength / 2 + L - L * Math.Cos(alpha / 2);
            double beta = (surplsAngle * Math.PI / 180) / (2 * divisionNumber - 1);
            double L1 = L;
            double sumd = d;
            double r1 = L1 * theta / (2 * Math.PI);// 弧长除以2*PI
            Point currentCenterPoint = this.bottomCenterPoint;
            while (flag)
            {
                double L2, r2, i = 0;
                Point nextCenterPoint;
                if (sumd < l)
                {
                    L2 = L1 - d;
                    nextCenterPoint = circleCenterPointsRay.GetPointOnRayVector(sumd * sinB);
                }
                else
                {
                    L2 = L - l;
                    flag = false;
                    nextCenterPoint = this.topCenterPoint;
                }
                r2 = L2 * theta / (2 * Math.PI);
                double k = 1, r3;
                Point tempPoint;
                if (j % 2 == 0)//r1和r2交换
                {
                    r3 = r1;
                    r1 = r2;
                    r2 = r3;
                    k = -1;
                    tempPoint = currentCenterPoint;
                    currentCenterPoint = nextCenterPoint;
                    nextCenterPoint = tempPoint;
                }
                List<Triangle> oneCircleTriangles = new List<Triangle>();
                while (i < 2 * divisionNumber)
                {
                    Point vertex1 = new Point(), vertex2 = new Point(), vertex3 = new Point();//三角面的三个点
                    vertex1 = GetPointInCircumference(nextCenterPoint, r2, unitVectorU, unitVectorV, i * beta);
                    if (j % 2 == 1 && i == 0)//在单数的开始边界时
                    {
                       vertex2 = GetPointInCircumference(currentCenterPoint, r1, unitVectorU, unitVectorV, i * beta+0.01);
                       
                    }
                    else if (j % 2 == 1 && i == 2 * divisionNumber - 1)//在单数的结束边界时
                    { 
                        vertex2 = GetPointInCircumference(currentCenterPoint, r1, unitVectorU, unitVectorV, i * beta -0.01); 
                    }
                    else
                    {
                        vertex2 = GetPointInCircumference(currentCenterPoint, r1, unitVectorU, unitVectorV, (i - k) * beta);
                    }
                    if (j % 2 == 0 && i == 0 )//在双数开始边界时
                    {
                        vertex3 = GetPointInCircumference(currentCenterPoint, r1, unitVectorU, unitVectorV, i * beta+0.01);
                    }
                    else if (j % 2 == 0 && i == 2 * divisionNumber - 1)//在双数结束边界时
                    {
                        vertex3 = GetPointInCircumference(currentCenterPoint, r1, unitVectorU, unitVectorV, i * beta -0.01);
                    }
                    else
                    {
                        vertex3 = GetPointInCircumference(currentCenterPoint, r1, unitVectorU, unitVectorV, (i + k) * beta);
                    }
                    oneCircleTriangles.Add(new Triangle(vertex1, vertex2, vertex3));
                    r3 = r1;
                    r1 = r2;
                    r2 = r3;
                    k = -k;
                    tempPoint = currentCenterPoint;
                    currentCenterPoint = nextCenterPoint;
                    nextCenterPoint = tempPoint;
                    i++;
                }
                divisinTriangles.Add(oneCircleTriangles);
                //
                if (this.bottomCenterPoint.GetDistance(currentCenterPoint) < this.bottomCenterPoint.GetDistance(nextCenterPoint))
                { currentCenterPoint = nextCenterPoint; }
                L1 = Math.Min(L1, L2);
                r1 = L1 * theta / (Math.PI * 2);
                triangleSideLength = 2 * L1 * Math.Sin(alpha / 2);
                d = Math.Sqrt(3) * triangleSideLength / 2 + L1 - L1 * Math.Cos(alpha / 2);
                sumd += d;
                j++;
                

            }

            return divisinTriangles;
        }



        /// <summary>
        /// 求被分割一部分的圆台体表面的三角面单元
        /// </summary>
        /// <param name="divisionNumber">一圈的三角形的个数</param>
        /// <param name="surplsAngle">被切后圆截面所剩的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns>圆柱体体表面的三角面单元</returns>
        public List<List<Triangle>> GetConeSurfaceDivisionTriangles(int divisionNumber, double surplsAngle, SpectVector unitVectorU, SpectVector unitVectorV)
        {
            List<List<Triangle>> divisionTriangles = new List<List<Triangle>>();
            List<List<Point>> trianglePoints;
            if (this.bottomCenterPoint.GetDistance(this.topCenterPoint) < 0.1)
            {
                trianglePoints = this.GetSmallConeSurfaceDivisionTrianglePoints(divisionNumber, surplsAngle, unitVectorU, unitVectorV);
            }
            else
            {
                trianglePoints = this.GetConeSurfaceDivisionTrianglePoints(divisionNumber, surplsAngle, unitVectorU, unitVectorV);
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
        /// 求被分割一部分的圆台体表面每个分割圆上的点
        /// </summary>
        /// <param name="divisionNumber">圆弧角的细分次数</param>
        /// <param name="surplsAngle">下底面所剩部分的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns></returns>
        private List<List<Point>> GetConeSurfaceDivisionTrianglePoints(int divisionNumber, double surplsAngle, SpectVector unitVectorU, SpectVector unitVectorV)
        {
            //参考论文：基本体素的三角形面单元剖分方法-李鸿亮
            //现在每个圆上取点，再组成三角面
            List<List<Point>> divisionTrianglePoints = new List<List<Point>>();
            RayInfo circleCenterPointsRay = new RayInfo(this.bottomCenterPoint, this.topCenterPoint);
            double h = this.bottomCenterPoint.GetDistance(this.topCenterPoint);//圆台体的高
            double l = Math.Sqrt((h * h + Math.Pow(this.bottomCircleRadius - this.topCircleRadius, 2)));
            double L = this.bottomCircleRadius * l / (this.bottomCircleRadius - this.topCircleRadius);
            double sinB = h / l;
            double theta = 2 * Math.PI * this.bottomCircleRadius / L;//弧度制
            double particalTheta = surplsAngle / 360 * theta;
            int j = 0;
            int I = 2 * divisionNumber + 1;
            bool flag = true;
            double alpha = particalTheta / divisionNumber;
            double triangleSideLength = 2 * L * Math.Sin(alpha / 2);//正三角形初始边长
            double d = Math.Sqrt(3) * triangleSideLength / 2 + L - L * Math.Cos(alpha / 2);
            double beta = (surplsAngle * Math.PI / 180) / (2 * divisionNumber);
            double currentL = L;
            double sumd = 0;
            double r = currentL * theta / (2 * Math.PI);// 弧长除以2*PI
            while (flag)
            {
                Point currentCenterPoint;
                if (sumd < l)
                {
                    currentCenterPoint = circleCenterPointsRay.GetPointOnRayVector(sumd * sinB);
                }
                else
                {
                    flag = false;
                    currentCenterPoint = this.topCenterPoint;
                }
                List<Point> circlePoints = new List<Point>();
                int i = 1;
                //第一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, 0.01));
                if (j % 2 == 0)//处于偶数层时
                {
                    while (i < divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, 2 * i * beta));
                        i++;
                    }
                }
                else//处于奇数层，奇数层要比偶数层多一个点
                {
                    while (i <= divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, (2 * i - 1) * beta));
                        i++;
                    }
                }
                //最后一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, surplsAngle * Math.PI / 180 - 0.01));
                divisionTrianglePoints.Add(circlePoints);
                //
                sumd += d;
                currentL = currentL-d;
                r = currentL * theta / (Math.PI * 2);
                triangleSideLength = 2 * currentL * Math.Sin(alpha / 2);
                d = Math.Sqrt(3) * triangleSideLength / 2 + currentL - currentL * Math.Cos(alpha / 2);
             //   sumd += d;
                j++;
            }
            return divisionTrianglePoints;
        }


        /// <summary>
        /// 求被分割一部分的细小圆台体表面每个分割圆上的点，只有底面和顶面两个圆
        /// </summary>
        /// <param name="divisionNumber">圆弧角的细分次数</param>
        /// <param name="surplsAngle">下底面所剩部分的角度</param>
        /// <param name="unitVectorU">下底圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">下底圆所在平面上一个向量与U垂足的向量V</param>
        /// <returns></returns>
        private List<List<Point>> GetSmallConeSurfaceDivisionTrianglePoints(int divisionNumber, double surplsAngle, SpectVector unitVectorU, SpectVector unitVectorV)
        {
            //参考论文：基本体素的三角形面单元剖分方法-李鸿亮
            //现在每个圆上取点，再组成三角面
            List<List<Point>> divisionTrianglePoints = new List<List<Point>>();
            RayInfo circleCenterPointsRay = new RayInfo(this.bottomCenterPoint, this.topCenterPoint);
            double h = this.bottomCenterPoint.GetDistance(this.topCenterPoint);//圆台体的高
            double l = Math.Sqrt((h * h + Math.Pow(this.bottomCircleRadius - this.topCircleRadius, 2)));
            double L = this.bottomCircleRadius * l / (this.bottomCircleRadius - this.topCircleRadius);
            double sinB = h / l;
            double theta = 2 * Math.PI * this.bottomCircleRadius / L;//弧度制
            double particalTheta = surplsAngle / 360 * theta;
            int j = 0;
            int I = 2 * divisionNumber + 1;
            bool flag = true;
            double alpha = particalTheta / divisionNumber;
            double triangleSideLength = 2 * L * Math.Sin(alpha / 2);//正三角形初始边长
            double d = Math.Sqrt(3) * triangleSideLength / 2 + L - L * Math.Cos(alpha / 2);
            double beta = (surplsAngle * Math.PI / 180) / (2 * divisionNumber);
            double currentL = L;
            double sumd = 0;
            double r = currentL * theta / (2 * Math.PI);// 弧长除以2*PI
            Point currentCenterPoint = this.bottomCenterPoint;
            while (flag)
            {
                if (j==1)
                {
                    flag = false;
                    currentCenterPoint = this.topCenterPoint;
                }
                List<Point> circlePoints = new List<Point>();
                int i = 1;
                //第一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, 0.01));
                if (j % 2 == 0)//处于偶数层时
                {
                    while (i < divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, 2 * i * beta));
                        i++;
                    }
                }
                else//处于奇数层，奇数层要比偶数层多一个点
                {
                    while (i <= divisionNumber)
                    {
                        circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, (2 * i - 1) * beta));
                        i++;
                    }
                }
                //最后一个点
                circlePoints.Add(this.GetPointInCircumference(currentCenterPoint, r, unitVectorU, unitVectorV, surplsAngle * Math.PI / 180 - 0.01));
                divisionTrianglePoints.Add(circlePoints);
                //
                currentL = currentL - d;
                r = currentL * theta / (Math.PI * 2);
                triangleSideLength = 2 * currentL * Math.Sin(alpha / 2);
                d = Math.Sqrt(3) * triangleSideLength / 2 + currentL - currentL * Math.Cos(alpha / 2);
                sumd += d;
                j++;
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
        private  Point GetPointInCircumference(Point circleCenterPoint, double circleRadius, SpectVector unitVectorU, SpectVector unitVectorV, double arcAngle)
        {
            Point circumferencePoint = new Point();
            circumferencePoint.X = circleCenterPoint.X + circleRadius * (unitVectorU.a * Math.Cos(arcAngle) + unitVectorV.a * Math.Sin(arcAngle));
            circumferencePoint.Y = circleCenterPoint.Y + circleRadius * (unitVectorU.b * Math.Cos(arcAngle) + unitVectorV.b * Math.Sin(arcAngle));
            circumferencePoint.Z = circleCenterPoint.Z + circleRadius * (unitVectorU.c * Math.Cos(arcAngle) + unitVectorV.c * Math.Sin(arcAngle));
            return circumferencePoint;
        }



    }
}
