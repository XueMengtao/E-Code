using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{

    /// <summary>
    ///地形矩形类
    /// </summary>
    public class Rectangle : ICloneable
    {
        private FaceID rectID;
        private Point leftBottomPoint;//矩形左下角顶点
        private Point rightBottomPoint;//矩形右下角顶点
        private Point leftTopPoint;//矩形左上角顶点
        private Point rightTopPoint;//矩形右上角顶点
        private AdjacentEdge leftLine;//矩形左边
        private AdjacentEdge rightLine;//矩形右边
        private AdjacentEdge topLine;//矩形上边
        private AdjacentEdge bottomLine;//矩形下边
        private AdjacentEdge bevelLine;//矩形侧边
        private Triangle triangleOne;
        private Triangle triangleTwo;


        public FaceID RectID
        {
            get { return this.rectID; }
            set { this.rectID = value; }
        }
        public List<Triangle> RectTriangles
        {
            get { return new List<Triangle> { this.triangleOne, this.triangleTwo }; }
        }

        public Point LeftBottomPoint
        {
            set { this.leftBottomPoint = value; }
            get { return this.leftBottomPoint; }
        }
        public Point RightBottomPoint
        {
            set { this.rightBottomPoint = value; }
            get { return this.rightBottomPoint; }
        }
        public Point LeftTopPoint
        {
            set { this.leftTopPoint = value; }
            get { return this.leftTopPoint; }
        }
        public Point RightTopPoint
        {
            set { this.rightTopPoint = value; }
            get { return this.rightTopPoint; }
        }
        public AdjacentEdge LeftLine
        {
            get { return leftLine; }
            set { leftLine = value; }
        }
        public AdjacentEdge RightLine
        {
            get { return rightLine; }
            set { rightLine = value; }
        }
        public AdjacentEdge TopLine
        {
            get { return topLine; }
            set { topLine = value; }
        }
        public AdjacentEdge BottomLine
        {
            get { return bottomLine; }
            set { bottomLine = value; }
        }
        public AdjacentEdge BevelLine
        {
            get { return bevelLine; }
            set { bevelLine = value; }
        }
        public Triangle TriangleOne
        {
            set { triangleOne = value; }
            get { return triangleOne; }
        }
        public Triangle TriangleTwo
        {
            set { triangleTwo = value; }
            get { return triangleTwo; }
        }

        public Rectangle()
        {
        }
        public Rectangle(Point leftBottomPoint, Point rightBottomPoint, Point leftTopPoint, Point rightTopPoint,FaceID planarID=null)
        {
            this.leftBottomPoint  = leftBottomPoint;
            this.rightBottomPoint = rightBottomPoint;
            this.leftTopPoint  = leftTopPoint;
            this.rightTopPoint = rightTopPoint;
            this.rectID = planarID;
            this.triangleOne = new Triangle(leftBottomPoint, rightBottomPoint, rightTopPoint);
            this.triangleTwo = new Triangle(this.leftBottomPoint, this.leftTopPoint, this.rightTopPoint);
            this.GetRectangleLines();
        }
        public Rectangle(Triangle tri1, Triangle tri2)
        {
            if (JudgeIfIsTheFirstTer(tri1))//判断哪个三角面是右下角的三角面
            {
                tri1.FaceID.ThirdID = 1;
                this.triangleOne = tri1;
                tri2.FaceID.ThirdID = 2;
                this.triangleTwo = tri2;
            }
            else
            {
                tri2.FaceID.ThirdID = 1;
                this.triangleOne = tri2;
                tri1.FaceID.ThirdID = 2;
                this.triangleTwo = tri1;
            }
            GetLeftBottomPoint(tri1);
            GetRightBottomPoint(tri1);
            GetLeftTopPoint(tri2);
            GetRgihtTopPoint(tri2);
            this.rectID =(FaceID)tri1.FaceID.Clone();
            this.rectID.ThirdID = 0;
            this.GetRectangleLines();
        }
        public void add(Triangle tri)
        {
            if (triangleOne == null)
                triangleOne = tri;
            else
                triangleTwo = tri;
        }

        /// <summary>
        /// 判断tri1是否是矩形中右下角的三角面
        /// </summary>
        private bool JudgeIfIsTheFirstTer(Triangle tri1)
        {
            //右下角三角面中有两个点的Y坐标等于矩形的最小Y值
            if ((tri1.Vertices[0].Y == tri1.MinUnitY && tri1.Vertices[1].Y == tri1.MinUnitY) ||
                (tri1.Vertices[0].Y == tri1.MinUnitY && tri1.Vertices[2].Y == tri1.MinUnitY) ||
                (tri1.Vertices[1].Y == tri1.MinUnitY && tri1.Vertices[2].Y == tri1.MinUnitY))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得左下角点
        /// </summary>
        private void GetLeftBottomPoint(Triangle tri1)
        {
            if (tri1.Vertices[0].X == tri1.MinUnitX && tri1.Vertices[0].Y == tri1.MinUnitY)
            { this.leftBottomPoint = tri1.Vertices[0]; }
            else if (tri1.Vertices[1].X == tri1.MinUnitX && tri1.Vertices[1].Y == tri1.MinUnitY)
            { this.leftBottomPoint = tri1.Vertices[1]; }
            else
            { this.leftBottomPoint = tri1.Vertices[2]; }
        }

        /// <summary>
        /// 获得右下角点
        /// </summary>
        private void GetRightBottomPoint(Triangle tri1)
        {
            if (tri1.Vertices[0].X != tri1.MinUnitX && tri1.Vertices[0].Y == tri1.MinUnitY)
            { this.rightBottomPoint = tri1.Vertices[0]; }
            else if (tri1.Vertices[1].X != tri1.MinUnitX && tri1.Vertices[1].Y == tri1.MinUnitY)
            { this.rightBottomPoint = tri1.Vertices[1]; }
            else
            { this.rightBottomPoint = tri1.Vertices[2]; }
        }

        /// <summary>
        /// 获得左上角点
        /// </summary>
        private void GetLeftTopPoint(Triangle tri2)
        {
            if (tri2.Vertices[0].X == tri2.MinUnitX && tri2.Vertices[0].Y != tri2.MinUnitY)
            { this.leftTopPoint = tri2.Vertices[0]; }
            else if (tri2.Vertices[1].X == tri2.MinUnitX && tri2.Vertices[1].Y != tri2.MinUnitY)
            { this.leftTopPoint = tri2.Vertices[1]; }
            else
            { this.leftTopPoint = tri2.Vertices[2]; }
        }

        /// <summary>
        /// 获得右上角点
        /// </summary>
        private void GetRgihtTopPoint(Triangle tri2)
        {
            if (tri2.Vertices[0].X != tri2.MinUnitX && tri2.Vertices[0].Y != tri2.MinUnitY)
            { this.rightTopPoint = tri2.Vertices[0]; }
            else if (tri2.Vertices[1].X != tri2.MinUnitX && tri2.Vertices[1].Y != tri2.MinUnitY)
            { this.rightTopPoint = tri2.Vertices[1]; }
            else
            { this.rightTopPoint = tri2.Vertices[2]; }
        }

        /// <summary>
        /// 获得矩形各个边
        /// </summary>
        private void GetRectangleLines()
        {
            this.leftLine = new AdjacentEdge(leftTopPoint, leftBottomPoint);
            this.bottomLine = new AdjacentEdge(leftBottomPoint, rightBottomPoint);
            this.rightLine = new AdjacentEdge(rightBottomPoint, rightTopPoint);
            this.topLine = new AdjacentEdge(rightTopPoint, leftTopPoint);
            this.bevelLine = new AdjacentEdge(leftBottomPoint, rightTopPoint);
        }

        /// <summary>
        /// 把矩形的边赋给三角面，但是若三角面的边已经设置，则不需要赋给三角面
        /// </summary>
        public void GiveLinesToRectTriangle()
        {
            if (this.triangleOne.Lines.Count == 0)
            {
                this.triangleOne.Lines.Add(this.bottomLine);
                this.triangleOne.Lines.Add(this.rightLine);
                this.triangleOne.Lines.Add(this.bevelLine);
            }
            if (this.triangleTwo.Lines.Count == 0)
            {
                this.triangleTwo.Lines.Add(this.topLine);
                this.triangleTwo.Lines.Add(this.leftLine);
                this.triangleTwo.Lines.Add(this.bevelLine);
            }
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        public Object Clone()
        {
            Triangle triangle1 = (Triangle)this.triangleOne.Clone();
            Triangle triangle2 = (Triangle)this.triangleTwo.Clone();
            Rectangle newQuadrangle = new Rectangle(triangle1,triangle2);
            newQuadrangle.RectID = (FaceID)this.rectID.Clone();
            return newQuadrangle;
        }
    }
}
