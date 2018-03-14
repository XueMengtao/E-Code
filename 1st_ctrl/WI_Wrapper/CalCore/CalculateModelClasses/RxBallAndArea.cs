using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///接收球，用于判断射线是否到达接收点
    /// </summary>
    public class ReceiveBall : ICloneable
    {
        public double frequence;//接收机频率，目前没用到
        public double FrequenceWidth;//频率带宽，目前没用到
        public string UAN;//接收机的UAN文件
        public int RxNum;
        public string RxName;
        public Point Receiver;
        public double Radius;//接收球半径
        public double temp;
        public bool isTx = false;//判断是否是发射机
        public ReceiveBall(Point final, int num, string name, double r = 15)
        {
            Receiver = final;
            RxNum = num;
            Radius = r;
            RxName = name;
        }
        public ReceiveBall()
        {

        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public virtual void Instance(double rxLength, double rxWidth, double spacing, Point origen)
        {

        }


        /// <summary>
        ///求射线与接收球的交点
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <param name="rxBall">接收球</param>
        /// <returns>返回射线与接收球的交点</returns>
        public Node GetCrossNodeWithRxBall(RayInfo oneRay)
        {
            Node rxNode = new Node();
            rxNode.RxNum = this.RxNum;
            rxNode.NodeStyle = NodeStyle.Rx;
            double A, B, C, t;
            A = Math.Pow(oneRay.RayVector.Mag(), 2);
            B = 2 * (oneRay.RayVector.a * (oneRay.Origin.X - Receiver.X) + oneRay.RayVector.b * (oneRay.Origin.Y - Receiver.Y) + oneRay.RayVector.c * (oneRay.Origin.Z - Receiver.Z));
            C = Math.Pow(oneRay.Origin.GetDistance(Receiver), 2) - Radius * Radius;
            double temp = B * B - 4 * A * C;
            if (temp < 0)
            { return null; }
            else if (temp == 0)
            {
                t = -(B / (2 * A));
                Point crosspoint = new Point(oneRay.RayVector.a * t + oneRay.Origin.X, oneRay.RayVector.b * t + oneRay.Origin.Y, oneRay.RayVector.c * t + oneRay.Origin.Z);
                rxNode.Position = crosspoint;
                rxNode.RayIn = oneRay;
                rxNode.DistanceToFrontNode = oneRay.Origin.GetDistance(crosspoint);
            }
            else
            {
                t = (-B + Math.Sqrt(temp)) / (2 * A);
                Point crosspoint1 = new Point(oneRay.RayVector.a * t + oneRay.Origin.X, oneRay.RayVector.b * t + oneRay.Origin.Y, oneRay.RayVector.c * t + oneRay.Origin.Z);
                t = (-B - Math.Sqrt(temp)) / (2 * A);
                Point crosspoint2 = new Point(oneRay.RayVector.a * t + oneRay.Origin.X, oneRay.RayVector.b * t + oneRay.Origin.Y, oneRay.RayVector.c * t + oneRay.Origin.Z);
                rxNode.RayIn = oneRay;
                if (crosspoint1.GetDistance(oneRay.Origin) < crosspoint2.GetDistance(oneRay.Origin))
                { rxNode.Position = crosspoint1; }
                else
                { rxNode.Position = crosspoint2; }
                rxNode.DistanceToFrontNode = oneRay.Origin.GetDistance(rxNode.Position);
            }
            //若有交点，判断交点是否在射线方向上
            if (rxNode.Position != null && oneRay.RayVector.IsParallelAndSamedirection(new SpectVector(oneRay.Origin, rxNode.Position)))
            { return rxNode; }
            else
            { return null; }
        }

    }

    /// <summary>
    ///接收区域
    /// </summary>
    public class ReceiveArea : ReceiveBall
    {

        //私有属性
        private int minRowNum;
        private int maxRowNum;
        private int minLineNum;
        private int maxLineNum;
        private Point originPoint;
        private double heightOfLayer;
        private Rectangle bottomRect;
        public List<Rectangle> areaSituationRect;
        public List<AreaNode> areaSituationNodes;
        //公共属性
        public double rxLength;
        public double rxWidth;
        public double spacing;
        public const double UnitX = 70.54011649840004;
        public const double UnitY = 92.76178115039988;

        public int MinRowNum
        {
            set { minRowNum = value; }
            get { return minRowNum; }
        }
        public int MaxRowNum
        {
            set { maxRowNum = value; }
            get { return maxRowNum; }
        }
        public int MinLineNum
        {
            set { minLineNum = value; }
            get { return minLineNum; }
        }
        public int MaxLineNum
        {
            set { maxLineNum = value; }
            get { return maxLineNum; }
        }
        public Point OriginPoint
        {
            set { originPoint = value; }
            get { return originPoint; }
        }
        public double HeightOfLayer
        {
            set { heightOfLayer = value; }
            get { return heightOfLayer; }
        }
        public Rectangle BottomRect
        {
            set { bottomRect = value; }
            get { return bottomRect; }
        }


        public ReceiveArea()
        {

        }
        public override void Instance(double rxLength, double rxWidth, double spacing, Point originPoint)
        {
            this.rxLength = rxLength;
            this.rxWidth = rxWidth;
            this.spacing = spacing;
            this.OriginPoint = originPoint;
        }

        /// <summary>
        ///判断节点是否在态势区域内
        /// </summary>
        ///  <param name="judgeNode">节点</param>
        /// <returns>节点是否在态势区域内的布尔值</returns>
        public bool JudgeIfNodeIsInArea(Node judgeNode)
        {
            bool flag1 = judgeNode.Position.X > this.originPoint.X && judgeNode.Position.X < this.originPoint.X + this.rxLength;
            bool flag2 = judgeNode.Position.Y > this.originPoint.Y && judgeNode.Position.Y < this.originPoint.Y + this.rxWidth;
            bool flag3 = (judgeNode.Height <= 5) || judgeNode.IsInTer;
            if (flag1 && flag2 && flag3)
            {
                judgeNode.IsReceiver = true;
                return true;
            }
            else
            {
                return false;
            }
        }


        #region 新的生成接收区域的方法

  

        /// <summary>
        /// 生成接收区域的下底面，以矩形为单元
        /// </summary>
        /// <param name="terRect">地形矩形</param>
        /// <param name="recArea">接收区域</param>
        /// <returns>接收区域下底面的矩形数组</returns>
        private Rectangle[,] GetAreaBottomSideRectangles(Rectangle[,] terRect)
        {
            int rowRange = 0, lineRange = 0;
            //从地下的矩形数组中提取接收区域的矩形，同样组成一个二维数组
            if (maxRowNum - minRowNum != 0)
            { rowRange = maxRowNum - minRowNum; }
            if (maxLineNum - minLineNum != 0)
            { lineRange = maxLineNum - minLineNum; }
            Rectangle[,] areaBottomSideRect = new Rectangle[lineRange + 1, rowRange + 1];
            for (int i = 0; i < lineRange + 1; i++)
            {
                for (int j = 0; j < rowRange + 1; j++)
                {
                    areaBottomSideRect[i, j] = terRect[minLineNum + i, minRowNum + j];
                }
            }
            return areaBottomSideRect;
        }

        /// <summary>
        /// 生成接收区域的上顶面
        /// </summary>
        /// <param name="areaBottomSideRect">接收区域下底面矩形数组</param>
        /// <param name="recArea">接收区域</param>
        /// <returns>接收区域上顶面的矩形数组</returns>
        private Rectangle[,] GetAreaTopSideRectangles(Rectangle[,] areaBottomSideRect)
        {
            Rectangle[,] areaTopSideRect = new Rectangle[areaBottomSideRect.GetLength(0), areaBottomSideRect.GetLength(1)];
            for (int i = 0; i < areaTopSideRect.GetLength(0); i++)
            {
                for (int j = 0; j < areaTopSideRect.GetLength(1); j++)
                {
                    areaTopSideRect[i, j] = (Rectangle)areaBottomSideRect[i, j].Clone();
                    for (int k = 0; k < areaTopSideRect[i, j].RectTriangles.Count; k++)
                    {
                        areaTopSideRect[i, j].RectTriangles[k].FaceStyle = FaceType.EFieldArea;
                        if (areaTopSideRect[i, j].RectTriangles[k].SubdivisionTriangle.Count != 0)
                        {
                            for (int l = 0; l < areaTopSideRect[i, j].RectTriangles[k].SubdivisionTriangle.Count; l++)
                            {
                                areaTopSideRect[i, j].RectTriangles[k].SubdivisionTriangle[l].FaceStyle = FaceType.EFieldArea;
                                areaTopSideRect[i, j].RectTriangles[k].SubdivisionTriangle[l].Vertices[0].Z += 5;
                                areaTopSideRect[i, j].RectTriangles[k].SubdivisionTriangle[l].Vertices[1].Z += 5;
                                areaTopSideRect[i, j].RectTriangles[k].SubdivisionTriangle[l].Vertices[2].Z += 5;
                            }
                        }
                        else
                        {
                            areaTopSideRect[i, j].RectTriangles[k].Vertices[0].Z += 5;
                            areaTopSideRect[i, j].RectTriangles[k].Vertices[1].Z += 5;
                            areaTopSideRect[i, j].RectTriangles[k].Vertices[2].Z += 5;
                        }
                    }
                }
            }
            return areaTopSideRect;
        }

        /// <summary>
        /// 生成接收区域的左侧面
        /// </summary>
        /// <param name="bottomRect">接收区域下底面</param>
        /// <param name="topRect">接收区域上顶面</param>
        /// <param name="recArea">接收区域</param>
        /// <returns>接收区域左侧面的四边形数组</returns>
        private Quadrangle[] GetAreaLeftSideRectangles(Rectangle[,] areaBottomSideRect, Rectangle[,] areaTopSideRect)
        {
            Quadrangle[] areaLeftSideRect = new Quadrangle[maxLineNum - minLineNum + 1];
            for (int i = 0; i < areaBottomSideRect.GetLength(0); i++)
            {
                areaLeftSideRect[i] = new Quadrangle(areaBottomSideRect[i, 0].LeftTopPoint, areaBottomSideRect[i, 0].LeftBottomPoint, areaTopSideRect[i, 0].LeftBottomPoint, areaTopSideRect[i, 0].LeftTopPoint, FaceType.EFieldArea, areaBottomSideRect[i, 0].RectID);
            }
            return areaLeftSideRect;
        }

        /// <summary>
        /// 生成接收区域的后侧面
        /// </summary>
        /// <param name="bottomRect">接收区域下底面</param>
        /// <param name="topRect">接收区域上顶面</param>
        /// <param name="recArea">接收区域</param>
        /// <returns>接收区域后侧面的四边形数组</returns>
        private Quadrangle[] GetAreaBackSideRectangles(Rectangle[,] areaBottomSideRect, Rectangle[,] areaTopSideRect)
        {
            Quadrangle[] areaBackSideRect = new Quadrangle[maxRowNum - minRowNum + 1];
            for (int i = 0; i < areaBottomSideRect.GetLength(1); i++)
            {
                areaBackSideRect[i] = new Quadrangle(areaBottomSideRect[0, i].LeftBottomPoint, areaBottomSideRect[0, i].RightBottomPoint, areaTopSideRect[0, i].RightBottomPoint, areaTopSideRect[0, i].LeftBottomPoint, FaceType.EFieldArea, areaBottomSideRect[0, i].RectID);
            }
            return areaBackSideRect;
        }

        /// <summary>
        /// 生成接收区域的右侧面
        /// </summary>
        /// <param name="bottomRect">接收区域下底面</param>
        /// <param name="topRect">接收区域上顶面</param>
        /// <param name="recArea">接收区域</param>
        /// <returns>接收区域右侧面的四边形数组</returns>
        private Quadrangle[] GetAreaRightSideRectangles(Rectangle[,] areaBottomSideRect, Rectangle[,] areaTopSideRect)
        {
            Quadrangle[] areaRightSideRect = new Quadrangle[maxLineNum - minLineNum + 1];
            for (int i = 0; i < areaBottomSideRect.GetLength(0); i++)
            {
                areaRightSideRect[i] = new Quadrangle(areaBottomSideRect[i, areaBottomSideRect.GetLength(1) - 1].RightTopPoint, areaBottomSideRect[i, areaBottomSideRect.GetLength(1) - 1].RightBottomPoint, areaTopSideRect[i, areaTopSideRect.GetLength(1) - 1].RightBottomPoint, areaTopSideRect[i, areaTopSideRect.GetLength(1) - 1].RightTopPoint, FaceType.EFieldArea, areaBottomSideRect[i, areaBottomSideRect.GetLength(1) - 1].RectID);
            }
            return areaRightSideRect;
        }

        /// <summary>
        /// 生成接收区域的前侧面
        /// </summary>
        /// <param name="bottomRect">接收区域下底面</param>
        /// <param name="topRect">接收区域上顶面</param>
        /// <param name="recArea">接收区域</param>
        /// <returns>接收区域前侧面的四边形数组</returns>
        private Quadrangle[] GetAreaFrontSideRectangles(Rectangle[,] areaBottomSideRect, Rectangle[,] areaTopSideRect)
        {
            Quadrangle[] areaFrontSideRect = new Quadrangle[maxRowNum - minRowNum + 1];
            for (int i = 0; i < areaBottomSideRect.GetLength(1); i++)
            {
                areaFrontSideRect[i] = new Quadrangle(areaBottomSideRect[areaBottomSideRect.GetLength(0) - 1, i].LeftTopPoint, areaBottomSideRect[areaBottomSideRect.GetLength(0) - 1, i].RightTopPoint, areaTopSideRect[areaTopSideRect.GetLength(0) - 1, i].RightTopPoint, areaTopSideRect[areaTopSideRect.GetLength(0) - 1, i].LeftTopPoint, FaceType.EFieldArea, areaBottomSideRect[areaBottomSideRect.GetLength(0) - 1, i].RectID);
            }
            return areaFrontSideRect;
        }


        /// <summary>
        /// 判断点是否在矩形内,该方法在点在棱上时会有误差
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="rect">矩形单元</param>
        /// <returns>点是否在矩形内的布尔值</returns>
        private static bool JudgeIfPointInRect(Point point, Rectangle rect)
        {
            if (point.X >= rect.LeftBottomPoint.X && point.Y >= rect.LeftBottomPoint.Y && point.X < rect.RightTopPoint.X && point.Y < rect.RightTopPoint.Y)
                return true;
            else return false;
        }

        /// <summary>
        /// 得到包含接收区域的矩形单元的Row和Line范围
        /// </summary>
        /// <param name="terRect">地形的矩形数组</param>
        /// <returns></returns>
        private void GetRangeOfAreaRect(Rectangle[,] terRect)
        {
            //接收区域右上角的点
            Point areaRightTopPoint = new Point(this.originPoint.X + this.rxLength, this.originPoint.Y + this.rxWidth, this.originPoint.Z);
            //求接收区域左下角的顶点所在矩形的位置
            minRowNum = (int)((originPoint.X - terRect[0, 0].TriangleOne.MinUnitX) / UnitX);
            minLineNum = (int)((originPoint.Y - terRect[0, 0].TriangleOne.MinUnitY) / UnitY);
            if (minRowNum < terRect.GetLength(1) - 1 && originPoint.X >= terRect[minLineNum, minRowNum + 1].TriangleOne.MinUnitX)
            { minRowNum++; }
            if (minLineNum < terRect.GetLength(0) - 1 && originPoint.Y >= terRect[minLineNum + 1, minRowNum].TriangleOne.MinUnitY)
            { minLineNum++; }
            //求接收区域右上角的顶点所在矩形的位置
            maxRowNum = (int)((areaRightTopPoint.X - terRect[0, 0].TriangleOne.MinUnitX) / UnitX);
            maxLineNum = (int)((areaRightTopPoint.Y - terRect[0, 0].TriangleOne.MinUnitY) / UnitY);
            if (maxRowNum >= terRect.GetLength(1))
            { maxRowNum = terRect.GetLength(1) - 1; }
            if (maxLineNum >= terRect.GetLength(0))
            { maxLineNum = terRect.GetLength(0) - 1; }
            if (maxRowNum < terRect.GetLength(1) - 1 && areaRightTopPoint.X >= terRect[maxLineNum, maxRowNum + 1].TriangleOne.MinUnitX)
            { maxRowNum++; }
            if (maxLineNum < terRect.GetLength(0) - 1 && areaRightTopPoint.Y >= terRect[maxLineNum + 1, maxRowNum].TriangleOne.MinUnitY)
            { maxLineNum++; }
        }

        #endregion

        /// <summary>
        /// 生成态势接收区域
        /// </summary>
        /// <param name="terRect">地形矩形</param>
        /// <param name="recArea">接收区域</param>
        /// <returns></returns>
        public void CreateAreaSituation(Rectangle[,] terRect)
        {
            this.areaSituationRect = new List<Rectangle> { };//分层的态势区域
            this.areaSituationNodes = new List<AreaNode>();//存储态势点
            this.GetRangeOfAreaRect(terRect); //根据用户所画区域的顶点坐标确定接收态势区域的row和line的范围
            int layerNum = 4;//将态势区域分层4层
            this.bottomRect = this.GetAreaBottomRectangle(terRect, layerNum);//根据地面矩形确定态势区域的下底面，态势区域的下底面的z为所有节点的最小的z值
            this.areaSituationRect.Add(this.bottomRect);//将态势区域下底面添加到态势层中
            for (int i = 1; i < layerNum + 1; i++)
            {
                this.areaSituationRect.Add(this.GetAreaSituationRectangles(bottomRect, i, heightOfLayer));//结合态势区域下底面和态势层的高度，生成态势层
            }
            this.rxLength = this.bottomRect.RightBottomPoint.X - this.bottomRect.LeftBottomPoint.X; //构建的态势层比用户所画的态势区域要大
            this.rxWidth = this.bottomRect.LeftTopPoint.Y - this.bottomRect.LeftBottomPoint.Y;
            originPoint = this.bottomRect.LeftBottomPoint;
        }

        /// <summary>
        /// 生成态势区域的下底面
        /// </summary>
        /// <param name="terRect">地形矩形</param>
        /// <param name="layerNum">态势层数</param>
        /// <returns>态势区域下底面的矩形</returns>
        private Rectangle GetAreaBottomRectangle(Rectangle[,] terRect, int layerNum)
        {
            int rowRange = 0, lineRange = 0;
            //从地下的矩形数组中提取接收区域的矩形，同样组成一个二维数组
            if (maxRowNum - minRowNum != 0)
            { rowRange = maxRowNum - minRowNum; }
            if (maxLineNum - minLineNum != 0)
            { lineRange = maxLineNum - minLineNum; }
            //结合用户选中的态势区域和地形三角面的大小，实际创建的态势区域比用户画的范围大一点
            Rectangle areaBottomSideRect = new Rectangle();
            double zMaxInAreaBottom = terRect[minLineNum, minRowNum].LeftBottomPoint.Z;//存储下底面各个点的Z坐标的最大值
            double zMinInAreaBottom = terRect[minLineNum, minRowNum].LeftBottomPoint.Z;//存储下底面各个点的Z坐标的最小值
                                                                                       //找到地形最低点和最高点
            for (int i = 0; i < lineRange + 1; i++)
            {
                for (int j = 0; j < rowRange + 1; j++)
                {
                    if (zMaxInAreaBottom < terRect[minLineNum + i, minRowNum + j].LeftBottomPoint.Z)
                        zMaxInAreaBottom = terRect[minLineNum + i, minRowNum + j].LeftBottomPoint.Z;
                    if (zMaxInAreaBottom < terRect[minLineNum + i, minRowNum + j].RightBottomPoint.Z)
                        zMaxInAreaBottom = terRect[minLineNum + i, minRowNum + j].RightBottomPoint.Z;
                    if (zMaxInAreaBottom < terRect[minLineNum + i, minRowNum + j].LeftTopPoint.Z)
                        zMaxInAreaBottom = terRect[minLineNum + i, minRowNum + j].LeftTopPoint.Z;
                    if (zMaxInAreaBottom < terRect[minLineNum + i, minRowNum + j].RightTopPoint.Z)
                        zMaxInAreaBottom = terRect[minLineNum + i, minRowNum + j].RightTopPoint.Z;
                    if (zMinInAreaBottom > terRect[minLineNum + i, minRowNum + j].LeftBottomPoint.Z)
                        zMinInAreaBottom = terRect[minLineNum + i, minRowNum + j].LeftBottomPoint.Z;
                    if (zMinInAreaBottom > terRect[minLineNum + i, minRowNum + j].RightBottomPoint.Z)
                        zMinInAreaBottom = terRect[minLineNum + i, minRowNum + j].RightBottomPoint.Z;
                    if (zMinInAreaBottom > terRect[minLineNum + i, minRowNum + j].LeftTopPoint.Z)
                        zMinInAreaBottom = terRect[minLineNum + i, minRowNum + j].LeftTopPoint.Z;
                    if (zMinInAreaBottom > terRect[minLineNum + i, minRowNum + j].RightTopPoint.Z)
                        zMinInAreaBottom = terRect[minLineNum + i, minRowNum + j].RightTopPoint.Z;
                }
            }
            //态势区域下底面内各个点的z坐标为最小值，即将下底面转化为一个平面
            Point currentLeftBottom = new Point(terRect[minLineNum, minRowNum].LeftBottomPoint);
            Point currentLeftTop = new Point(terRect[maxLineNum, minRowNum].LeftTopPoint);
            Point currentRightBottom = new Point(terRect[minLineNum, maxRowNum].RightBottomPoint);
            Point currentRightTop = new Point(terRect[maxLineNum, maxRowNum].RightTopPoint);
            currentLeftBottom.Z = currentLeftTop.Z = currentRightBottom.Z = currentRightTop.Z = zMinInAreaBottom;
            areaBottomSideRect = new Rectangle(currentLeftBottom, currentRightBottom, currentLeftTop, currentRightTop);
            heightOfLayer = (zMaxInAreaBottom - zMinInAreaBottom) / layerNum;
            return areaBottomSideRect;
        }

        /// <summary>
        /// 根据态势区域的下底面获取态势层
        /// </summary>
        /// <param name="areaBottomSideRect">态势区域下底面</param>
        /// <param name="currentlayer">当前层数</param>
        /// <param name="heightOfLayer">层高</param>
        /// <returns>生成的当前态势层</returns>
        private Rectangle GetAreaSituationRectangles(Rectangle areaBottomSideRect, int currentlayer, double heightOfLayer)
        {
            Rectangle areaSituationLayer = new Rectangle();//当前态势层
            Point currentLeftBottom = new Point(areaBottomSideRect.LeftBottomPoint);
            Point currentLeftTop = new Point(areaBottomSideRect.LeftTopPoint);
            Point currentRightBottom = new Point(areaBottomSideRect.RightBottomPoint);
            Point currentRightTop = new Point(areaBottomSideRect.RightTopPoint);
            currentLeftBottom.Z = currentLeftTop.Z = currentRightBottom.Z = currentRightTop.Z += heightOfLayer * currentlayer;
            areaSituationLayer = new Rectangle(currentLeftBottom, currentRightBottom, currentLeftTop, currentRightTop);
            return areaSituationLayer;
        }
    }
}



