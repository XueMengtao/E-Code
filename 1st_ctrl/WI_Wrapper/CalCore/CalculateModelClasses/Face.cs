using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///面的信息
    /// </summary>
    public abstract class Face
    {
        protected int verticesCount;//点的个数 
        protected List<Point> vertices;//在面上的点
        protected List<AdjacentEdge> lines;//由点构成的边
        protected SpectVector normalVector;//平面的法向量
        protected FaceID faceID = new FaceID();//平面的编号
        protected FaceType faceStyle;//平面的类型
        protected int materialNumber;//所用材料编号
        protected Material material;//平面的材料
        protected List<Triangle> subdivisionTriangle = new List<Triangle>();//存放平面细分后的细分平面List

        public int VerticeCount
        {
            get { return this.verticesCount; }
            set { this.verticesCount = value; }
        }
        public List<Point> Vertices
        {
            get { return this.vertices; }
            set { this.vertices = value; }
        }
        public List<AdjacentEdge> Lines
        {
            get { return this.lines; }
            set { this.lines = value; }
        }
        public Material Material
        {
            get { return this.material; }
            set { this.material = value; }
        }
        public SpectVector NormalVector
        {
            get { return normalVector; }
            set { normalVector = value; }
        }
        public FaceID FaceID
        {
            set { this.faceID = value; }
            get { return this.faceID; }
        }
        public int MaterialNum
        {
            get { return materialNumber; }
            set { this.materialNumber = value; }
        }
        public FaceType FaceStyle
        {
            get { return faceStyle; }
            set { this.faceStyle = value; }
        }
        public List<Triangle> SubdivisionTriangle
        {
            get { return subdivisionTriangle; }
            set { subdivisionTriangle = value; }
        }

        /// <summary>
        /// 判断点是否在平面内中
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>返回点是否在面内的布尔值</returns>
        public abstract bool JudgeIfPointInFace(Point viewPoint);

        /// <summary>
        /// 判断两个平面是否相同
        /// </summary>
        /// <param name="otherFace">另一个平面</param>
        /// <returns>判断两个平面是否相同的布尔值</returns>
        public bool JudgeIsTheSameFace(Face otherFace)
        {
            if (this.vertices.Count == otherFace.Vertices.Count)
            {
                List<Point> commonPoints = this.GetCommonPointsWithOtherFace(otherFace);
                if (commonPoints.Count == this.vertices.Count)
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// 判断两个平面是否相邻
        /// </summary>
        /// <param name="otherFace">另一个平面</param>
        /// <returns>判断两个平面是否相邻的布尔值</returns>
        public bool JudgeTheFacesAreAdjacent(Face otherFace)
        {
            List<Point> commonPoint = this.GetCommonPointsWithOtherFace(otherFace);
            if (commonPoint.Count == 2)//为相邻三角面
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 求射线与平面的交点
        /// </summary>
        /// <param oneRay="viewPoint">射线</param>
        /// <returns>返回射线与平面的交点，若无交点则返回null</returns>
        public Point GetCrossPointWithRay(RayInfo oneRay)
        {
            SpectVector unitVector = this.normalVector.GetNormalizationVector();//归一化
            double temp = oneRay.RayVector.DotMultiplied(unitVector);
            if (Math.Abs(temp) < 0.00001)//如果向量和平面平行则返回null
                return null;
            else
            {
                double TriD = unitVector.a * this.vertices[0].X + unitVector.b * this.vertices[0].Y + unitVector.c * this.vertices[0].Z;
                double temp1 = unitVector.a * oneRay.Origin.X + unitVector.b * oneRay.Origin.Y + unitVector.c * oneRay.Origin.Z - TriD;
                double t = -temp1 / temp;
                Point tempPoint = new Point(oneRay.RayVector.a * t + oneRay.Origin.X, oneRay.RayVector.b * t + oneRay.Origin.Y, oneRay.RayVector.c * t + oneRay.Origin.Z);
                if (!oneRay.RayVector.IsParallelAndSamedirection(new SpectVector(oneRay.Origin, tempPoint)))
                    return null;
                if (this.JudgeIfPointInFace(tempPoint))
                    return tempPoint;
                else
                    return null;//如果求出的交点不在三角面内，则返回空
            }

        }
        /// <summary>
        ///获得平面的法向量
        /// </summary>
        public SpectVector GetNormalVector()
        {
            SpectVector vector12 = new SpectVector(this.vertices[0], this.vertices[1]);
            SpectVector vector13 = new SpectVector(this.vertices[0], this.vertices[2]);
            return vector12.CrossMultiplied(vector13);
        }

        /// <summary>
        ///获取两个三角面的公共点的list
        /// </summary>
        public List<Point> GetCommonPointsWithOtherFace(Face otherFace)
        {
            List<Point> commonPoints = new List<Point>();
            for (int i = 0; i < this.vertices.Count; i++)
            {
                for (int j = 0; j < otherFace.Vertices.Count; j++)
                {
                    if(this.vertices[i].equal(otherFace.Vertices[j]))
                    {
                        commonPoints.Add(this.vertices[i]);
                    }
                }
            }
            return commonPoints;
        }

        /// <summary>
        /// 找到平面面的非棱点
        /// </summary>
        public List<Point> GetPointsOutOfTheLine(LineSegment triLine)
        {
            List<Point> unequalPoints = new List<Point>();
            for (int i = 0; i < this.vertices.Count; i++)
            {
                if (this.vertices[i].unequal(triLine.StartPoint) && this.vertices[i].unequal(triLine.EndPoint))
                {
                    unequalPoints.Add(this.vertices[i]);
                }
            }
            return unequalPoints;
            
        }


        /// <summary>
        /// 判断该线段是否三角面的一条边
        /// </summary>
        /// <param name="viewLine">线段</param>
        /// <returns>返回线段是否三角面的一条边的布尔值</returns>
        public bool JudgeIfHaveThisLine(LineSegment viewLine)
        {
            for (int i = 0; i < this.lines.Count; i++)
            {
                if (this.lines[i].JudgeIfTheSameLine(viewLine))
                { return true; }
            }
            return false;
        }

        
        /// <summary>
        ///求平面面方程Ax+By+Cz+D=0的四个系数A,B,C,D
        /// </summary>
        public double[] GetFaceEquationFactor()
        {
            double[] para = new double[4];
            SpectVector unitVector = this.normalVector.GetNormalizationVector();
            para[0] = unitVector.a;
            para[1] = unitVector.b;
            para[2] = unitVector.c;
            para[3] = -(unitVector.a * this.vertices[0].X + unitVector.b * this.vertices[0].Y + unitVector.c * this.vertices[0].Z); 
            return para;
        }

        /// <summary>
        ///求平面外一条直线在该平面上的投影
        /// </summary>
        /// <param name="viewPoint">平面外一条直线</param>
        /// <returns>平面外一条直线在该平面上的投影</returns>
        public RayInfo GetProjectionRayInFace(RayInfo viewRay)
        {
            double dotProduct = this.normalVector.a * viewRay.RayVector.a + 
                this.normalVector.b * viewRay.RayVector.b + this.normalVector.c * viewRay.RayVector.c;
            if (Math.Abs(dotProduct) < 0.00000001)
            {
                return new RayInfo(this.GetProjectionPointInFace(viewRay.Origin), viewRay.RayVector);
            }
            else
            {
                Point rayPointInFace = this.GetProjectionPointInFace(viewRay.Origin);
                Point crossPoint = viewRay.GetCrossPointBetweenStraightLineAndFace(this);
                return new RayInfo(rayPointInFace, crossPoint);
            }
        }

        /// <summary>
        ///求平面外一点在该平面上的投影点
        /// </summary>
        /// <param name="viewPoint">平面外一点</param>
        /// <returns>平面外一点在该平面上的投影点</returns>
        public Point GetProjectionPointInFace(Point viewPoint)
        {
            if (this.JudgeIfPointInFace(viewPoint))
            { return viewPoint; }
            else
            {
                double[] faceFactor = this.GetFaceEquationFactor();
                double t = -(faceFactor[0] * viewPoint.X + faceFactor[1] * viewPoint.Y + faceFactor[2] * viewPoint.Z + faceFactor[3])
                    / (Math.Pow(faceFactor[0], 2) + Math.Pow(faceFactor[1], 2) + Math.Pow(faceFactor[2], 2));
                return new Point(viewPoint.X + faceFactor[0] * t, viewPoint.Y + faceFactor[1] * t, viewPoint.Z + faceFactor[2] * t);
            }
        }


        /// <summary>
        /// 把三角面或者四边形转换成空间平面
        /// </summary>
        /// <returns>空间平面</returns>
        public SpaceFace SwitchToSpaceFace()
        {
            return new SpaceFace(this.vertices[0], this.vertices[1], this.vertices[2], FaceType.Air, this.faceID, this.material, this.materialNumber);
        }

    }



    /// <summary>
    ///空间平面
    /// </summary>
    public class SpaceFace : Face
    {
        public SpaceFace(Point facePoint, SpectVector normalVector)
        {
            this.vertices = new List<Point>();
            this.vertices.Add(facePoint);
            this.normalVector = normalVector;
            double D = -(normalVector.a * facePoint.X + normalVector.b * facePoint.Y + normalVector.c * facePoint.Z);
            if (normalVector.c != 0)
            {
                this.vertices.Add( new Point(facePoint.X + 1, facePoint.Y + 1, -(normalVector.a * facePoint.X + normalVector.b * facePoint.Y + normalVector.a + normalVector.b + D) / normalVector.c));
                this.vertices.Add( new Point(facePoint.X - 1, facePoint.Y + 1, (-normalVector.a * facePoint.X - normalVector.b * facePoint.Y + normalVector.a - normalVector.b - D) / normalVector.c));
            }
            else if (normalVector.b != 0)
            {
                this.vertices.Add( new Point(facePoint.X + 1,-(D+normalVector.a*facePoint.X+normalVector.a)/normalVector.b,facePoint.Z ));
                this.vertices.Add( new Point(facePoint.X + 1,-(D+normalVector.a*facePoint.X+normalVector.a)/normalVector.b, facePoint.Z-1));
            }
            else
            {
                this.vertices.Add( new Point(facePoint.X , facePoint.Y+50, facePoint.Z));
                this.vertices.Add( new Point(facePoint.X , facePoint.Y , facePoint.Z+50));
            }
        }
        public SpaceFace(Point facePoint, SpectVector normalVector, FaceType faceStyle , FaceID faceID , Material material , int materialNumber )
            : this(facePoint, normalVector)
        {
            this.faceStyle = faceStyle;
            this.faceID = faceID;
            this.material = material;
            this.materialNumber = materialNumber;
        }
          public SpaceFace(Point vertex1, Point vertex2, Point vertex3)
        {
            this.vertices = new List<Point> { vertex1, vertex2, vertex3 };
            this.lines = new List<AdjacentEdge>();
            this.normalVector = this.GetNormalVector();
        }
          public SpaceFace(Point vertex1, Point vertex2, Point vertex3, FaceType faceStyle, FaceID faceID, Material material, int materialNumber)
            : this(vertex1, vertex2, vertex3)
        {
            this.faceStyle = faceStyle;
            this.faceID = faceID;
            this.material = material;
            this.materialNumber = materialNumber;
        }



        /// <summary>
        /// 判断点是否在平面内
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>返回点是否在平面内的布尔值</returns>
        public override bool JudgeIfPointInFace(Point viewPoint)
        {
            double[] FaceFactor = this.GetFaceEquationFactor();
            double faceEquation = FaceFactor[0] * viewPoint.X + FaceFactor[1] * viewPoint.Y + FaceFactor[2] * viewPoint.Z + FaceFactor[3];
            if (Math.Abs(faceEquation) < 0.0001)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    /// <summary>
    /// 三角面信息
    /// </summary>
    public class Triangle : Face, ICloneable
    {
        private double minUnitX;//三角面的最小X值
        private double minUnitY;//三角面的最小Y值
        private double maxUnitX;//三角面的最大X值
        private double maxUnitY;//三角面的最大Y值

        public double MinUnitX
        {
            get { return minUnitX; }
            set { minUnitX = value; }
        }
        public double MinUnitY
        {
            get { return minUnitY; }
            set { minUnitY = value; }
        }
        public double MaxUnitX
        {
            get { return this.maxUnitX; }
            set { this.maxUnitX = value; }
        }
        public double MaxUnitY
        {
            get { return this.maxUnitY; }
            set { this.maxUnitY = value; }
        }


        //构造函数
        public Triangle()
        {
            this.vertices = new List<Point>();
            this.lines = new List<AdjacentEdge>();
        }
        public Triangle(Point vertex1, Point vertex2, Point vertex3)
        {
            this.vertices = new List<Point> { vertex1, vertex2, vertex3 };
            this.lines = new List<AdjacentEdge>();
       //     this.SetLinesByVertice();
            this.normalVector = this.GetNormalVector();
        }
        public Triangle(Point vertex1, Point vertex2, Point vertex3, FaceType faceStyle, FaceID faceID, Material material,int materialNumber )
            : this(vertex1, vertex2, vertex3)
        {
            this.faceStyle = faceStyle;
            this.faceID = faceID;
            this.material = material;
            this.materialNumber = materialNumber;
        }


        /// <summary>
        /// 判断点是否在三角面中
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>返回点是否在三角面内的布尔值</returns>
        public override bool JudgeIfPointInFace(Point viewPoint)
        {
            if (viewPoint.equal( this.vertices[0]) || viewPoint .equal( this.vertices[1]) || viewPoint .equal( this.vertices[2]))
                return true;
            else
            {
                SpectVector Vda = new SpectVector(viewPoint, this.vertices[0]);
                SpectVector Vdb = new SpectVector(viewPoint, this.vertices[1]);
                SpectVector Vdc = new SpectVector(viewPoint, this.vertices[2]);
                double Sdab = 0.5 * Vda.Mag() * Vdb.Mag() * Math.Sin(Vda.GetPhaseOfVector(Vdb) * Math.PI / 180);//三角形ABD的面积
                double Sdac = 0.5 * Vda.Mag() * Vdc.Mag() * Math.Sin(Vda.GetPhaseOfVector(Vdc) * Math.PI / 180);//三角形ACD的面积
                double Sdbc = 0.5 * Vdb.Mag() * Vdc.Mag() * Math.Sin(Vdb.GetPhaseOfVector(Vdc) * Math.PI / 180);//三角行BCD的面积
                double Sabc = GetTriangleArea();//三角面ABC的面积
                double Dvalue = Sabc - Sdab - Sdac - Sdbc;//若点在三角面内，则Sabc+Sabc+Sacd=Sbcd;
                if (Math.Abs(Dvalue) < 0.000001)
                    return true;
                else return false;
            }
        }

      

        /// <summary>
        /// 获取三角面的面积
        /// </summary>
        public double GetTriangleArea()
        {
            SpectVector Vab = new SpectVector(this.vertices[0], this.vertices[1]);
            SpectVector Vac = new SpectVector(this.vertices[0], this.vertices[2]);
            return  0.5 * Vab.Mag() * Vac.Mag() * Math.Sin(Vab.GetPhaseOfVector(Vac) * Math.PI / 180);//三角面ABC的面积
        }


        /// <summary>
        /// 根据三角面的顶点生成三角面的边
        /// </summary>
        public void SetLinesByVertice()
        {
            this.lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[1]));
            this.lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[2]));
            this.lines.Add(new AdjacentEdge(this.vertices[1], this.vertices[2]));
        }

        /// <summary>
        ///判断三角面三个顶点在XY平面投影在一条直线上
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <returns>三角面三个顶点在XY平面投影在一条直线上的布尔值</returns>
        public bool JudgeIfTriangleVerticesIsLineInXY()
        {
            SpectVector vector12 = new SpectVector(this.Vertices[0], this.Vertices[1]);
            SpectVector vector13 = new SpectVector(this.Vertices[0], this.Vertices[2]);
            vector12.c = vector13.c = 0;
            double temp = (vector12.a * vector13.a + vector12.b * vector13.b) / (vector12.Mag() * vector13.Mag());
            if ((Math.Abs(temp - 1) <= 0.00000001) || (Math.Abs(temp + 1) <= 0.00000001))
            { return true; }
            else { return false; }
        }

        /// <summary>
        /// 求三角面在XY平面投影的面积
        /// </summary>
        /// <param name="tri">三角面</param>
        /// <returns>三角面投影的面积</returns>
        public double GetTriangleAreaInXY()
        {
            double triSide1 = Math.Sqrt((this.Vertices[0].X - this.Vertices[1].X) * (this.Vertices[0].X - this.Vertices[1].X) + (this.Vertices[0].Y - this.Vertices[1].Y) * (this.Vertices[0].Y - this.Vertices[1].Y));
            double triSide2 = Math.Sqrt((this.Vertices[2].X - this.Vertices[1].X) * (this.Vertices[2].X - this.Vertices[1].X) + (this.Vertices[2].Y - this.Vertices[1].Y) * (this.Vertices[2].Y - this.Vertices[1].Y));
            double triSide3 = Math.Sqrt((this.Vertices[2].X - this.Vertices[0].X) * (this.Vertices[2].X - this.Vertices[0].X) + (this.Vertices[2].Y - this.Vertices[0].Y) * (this.Vertices[2].Y - this.Vertices[0].Y));
            double sideHalfSum = (triSide1 + triSide2 + triSide3) / 2;
            return Math.Sqrt(sideHalfSum * (sideHalfSum - triSide1) * (sideHalfSum - triSide2) * (sideHalfSum - triSide3));
        }

        /// <summary>
        /// 判断一个点在XY平面投影是否在一个三角面的投影内部
        /// </summary>
        /// <param name="viewPoint"></param>
        /// <returns>点的投影是否在一个三角面的投影内部的布尔值</returns>
        public bool JudgeIfPointInTriangleInXY(Point viewPoint)
        {
            Triangle tri1 = new Triangle(viewPoint, this.Vertices[0], this.Vertices[1]);
            Triangle tri2 = new Triangle(viewPoint, this.Vertices[0], this.Vertices[2]);
            Triangle tri3 = new Triangle(viewPoint, this.Vertices[1], this.Vertices[2]);
            double sTriangleArea = this.GetTriangleAreaInXY();
            double tri1Sum = tri1.GetTriangleAreaInXY();
            double tri2Sum = tri2.GetTriangleAreaInXY();
            double tri3Sum = tri3.GetTriangleAreaInXY();
            if (Math.Abs(sTriangleArea - tri1Sum - tri2Sum - tri3Sum) < 0.0000001)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得三角面没有上述交点的边,用于求绕射棱
        /// </summary>
        /// <param name="crossPoint">边上的点</param>
        /// <returns></returns>
        public List<AdjacentEdge> GetNoneCrossPointLines(List<Point> crossPoint)
        {
            List<AdjacentEdge> cleanLines = new List<AdjacentEdge>();
            for(int i=0;i<lines.Count;i++)
            {
                if (!lines[i].JudgeIfPointsInLineInXYPlane(crossPoint))
                { cleanLines.Add(lines[i]); }
            }
            return cleanLines;
        }

        /// <summary>
        /// 获得正三角面的重心
        /// </summary>
        /// <returns></returns>
        public Point GetTriangleBaryCenterPoint()
        {
            Point middlePoint = this.vertices[0].GetMiddlePointWithOtherPoint(this.vertices[1]);
            return new RayInfo(this.vertices[2], middlePoint).GetPointOnRayVector(this.vertices[2].GetDistance(middlePoint) * 2 / 3);
        }


        /// <summary>
        /// 拷贝
        /// </summary>
        public Object Clone()
        {
            Triangle newTri = new Triangle();
            newTri.FaceID = (FaceID)this.faceID.Clone();
            newTri.MinUnitX = minUnitX;
            newTri.MinUnitY = minUnitY;
            newTri.MaterialNum = this.materialNumber;
            newTri.faceStyle = this.faceStyle;
            newTri.Material = this.material;
            newTri.normalVector = this.normalVector;
            for (int i = 0; i < this.vertices.Count; i++)
            {
                newTri.Vertices.Add((Point)this.vertices[i].Clone());
            }
            newTri.SetLinesByVertice();
            if (this.subdivisionTriangle.Count != 0)
            {
                for (int i = 0; i < this.subdivisionTriangle.Count; i++)
                {
                    newTri.subdivisionTriangle.Add((Triangle)this.subdivisionTriangle[i].Clone());
                }
            }
            return newTri;
        }
    }

    /// <summary>
    ///四边形类
    /// </summary>
    public class Quadrangle : Face
    {
        private string buildingFaceType;
        private Triangle triangleOne;
        private Triangle triangleTwo;

        public string BuildingFaceType
        {
            get { return this.buildingFaceType; }
            set { this.buildingFaceType = value; }
        }
        public Triangle TriangleOne
        {
            get { return triangleOne; }
        }
        public Triangle TriangleTwo
        {
            get { return triangleTwo; }
        }
        //空构造函数
        public Quadrangle()
        {
            this.vertices = new List<Point>();
            this.lines = new List<AdjacentEdge>();
        }
        //构造函数
        public Quadrangle(Point vertice1, Point vertice2, Point vertice3, Point vertice4)
        {
            this.vertices = new List<Point> { vertice1, vertice2, vertice3, vertice4 };
            this.normalVector = this.GetNormalVector();
            this.lines = new List<AdjacentEdge>();
            this.SetLinesAndTriangles();
        }
        public Quadrangle(Point vertice1, Point vertice2, Point vertice3, Point vertice4,
            FaceType faceStyle, FaceID faceID, Material material = null, int materialNumber = 0)
            : this(vertice1, vertice2, vertice3, vertice4)
        {
            this.faceStyle = faceStyle;
            this.faceID = faceID;
            this.material = material;
            this.materialNumber = materialNumber;
        }

        /// <summary>
        /// 判断点是否在平面内中
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>返回点是否在面内的布尔值</returns>
        public override bool JudgeIfPointInFace(Point viewPoint)
        {
            if (this.triangleOne.JudgeIfPointInFace(viewPoint) || this.triangleTwo.JudgeIfPointInFace(viewPoint))
            { return true; }
            else { return false; }
        }

        /// <summary>
        /// 求射线与四边形的交点
        /// </summary>
        /// <param oneRay="viewPoint">射线</param>
        /// <returns>返回射线与四边形的交点，若无交点则返回null</returns>
        public Point GetCrossPointWithRay(RayInfo oneRay)
        {
            SpectVector unitVector = this.normalVector.GetNormalizationVector();//归一化
            double temp = oneRay.RayVector.DotMultiplied(unitVector);
            if (Math.Abs(temp) < 0.00001)//如果向量和平面平行则返回null
                return null;
            else
            {
                double TriD = unitVector.a * this.vertices[0].X + unitVector.b * this.vertices[0].Y + unitVector.c * this.vertices[0].Z;
                double temp1 = unitVector.a * oneRay.Origin.X + unitVector.b * oneRay.Origin.Y + unitVector.c * oneRay.Origin.Z - TriD;
                double t = -temp1 / temp;
                Point tempPoint = new Point(oneRay.RayVector.a * t + oneRay.Origin.X, oneRay.RayVector.b * t + oneRay.Origin.Y, oneRay.RayVector.c * t + oneRay.Origin.Z);
                if (!oneRay.RayVector.IsParallelAndSamedirection(new SpectVector(oneRay.Origin, tempPoint)))
                    return null;
                if (this.JudgeIfPointInFace(tempPoint))
                    return tempPoint;
                else
                    return null;//如果求出的交点不在三角面内，则返回空
            }

        }



        /// <summary>
        /// 判断点是否在四边形在XY平面投影内
        /// </summary>
        /// <param name="viewPoint">点</param>
        /// <returns>点在四边形投影内，返回true，否则返回false</returns>
        public bool JudeIfPointInQuadrangleInXY(Point viewPoint)
        {
            if (this.triangleOne.JudgeIfPointInTriangleInXY(viewPoint) || this.triangleTwo.JudgeIfPointInTriangleInXY(viewPoint))
            { return true; }
            else { return false; }

        }

        /// <summary>
        ///把输入的四个点放入三角面中
        /// </summary>
        public void SetLinesAndTriangles()
        {
            this.lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[1]));
            this.lines.Add(new AdjacentEdge(this.vertices[1], this.vertices[2]));
            this.lines.Add(new AdjacentEdge(this.vertices[2], this.vertices[3]));
            this.lines.Add(new AdjacentEdge(this.vertices[3], this.vertices[0]));
            //this.triangleOne = new Triangle(this.vertices[0], this.vertices[3], this.vertices[2]);
            //this.triangleTwo = new Triangle(this.vertices[0], this.vertices[1], this.vertices[2]);
            //因为建筑物材料丢失，所以换个构造函数来试
            this.triangleOne = new Triangle(this.vertices[0], this.vertices[3], this.vertices[2], this.FaceStyle, this.FaceID, this.Material, this.MaterialNum);
            //添加绕射棱信息。备注：即使斜边不发生绕射，也必须添加！！不添加会在后续计算中Lines的索引越界
            this.triangleOne.Lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[3],true));
            this.triangleOne.Lines.Add(new AdjacentEdge(this.vertices[2], this.vertices[3],true));
            this.triangleOne.Lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[2]));
            
            this.triangleTwo = new Triangle(this.vertices[0], this.vertices[1], this.vertices[2], this.FaceStyle, this.FaceID, this.Material, this.MaterialNum);
            this.triangleTwo.Lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[1],true));
            this.triangleTwo.Lines.Add(new AdjacentEdge(this.vertices[1], this.vertices[2],true));
            this.triangleTwo.Lines.Add(new AdjacentEdge(this.vertices[0], this.vertices[2]));
        }
    }




    /// <summary>
    /// 面的类型
    /// </summary>
    public enum FaceType
    {
        Terrian,
        EFieldArea,
        Building,
        Air,
    }
}
