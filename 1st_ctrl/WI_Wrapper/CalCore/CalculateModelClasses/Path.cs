using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///路径
    /// </summary>
    public class Path
    {
        private const int MaxNodeNumber = 6;
        public int Txnum;
        public int Rxnum;
        public double pathloss;
        public double Delay;
        public double thetaa, phia, thetab, phib;
        public List<Node> node;
        public Path(List<Node> nodes)
        {
            node = nodes;
            Txnum = nodes[0].TxNum;
            Rxnum = nodes[nodes.Count - 1].RxNum;
        }
        public double GetPathLength()
        {
            double length = 0;
            for (int i = 0; i < node.Count; i++)
            {
                length += node[i].DistanceToFrontNode;
            }
            return length;
        }
        public Path()
        { }

        /// <summary>
        /// 更新节点信息，如果路径不存在返回null
        /// </summary>
        /// <returns>更新后的具有准确节点信息的路径</returns>
        public Path UpdateReversePaths()
        {
            if (this.node.Count > 2)//直射不需要进行反向
            {
                List<Point> pointsOfPath = this.GetPointsOfPath();
                if (pointsOfPath.Count == 0)
                {
                    return null;
                }
                else
                {
                    //更新路径
                    for (int j = 1; j < this.node.Count - 1; j++)
                    {
                        this.node[j].Position = pointsOfPath[j - 1];
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// 利用反向算法获取路径上准确的反射点和绕射点
        /// </summary>
        /// <param name="indexOfPath">所要计算的路径的下标</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        public List<Point> GetPointsOfPath()
        {
            List<Point> tempPointList = new List<Point>();
            int nodeIndexOfDiffractEdge1 = MaxNodeNumber, nodeIndexOfDiffractEdge2 = MaxNodeNumber;
            for (int i = 1; i < this.node.Count - 1; i++)
            {
                if (this.node[i].NodeStyle == NodeStyle.DiffractionNode && nodeIndexOfDiffractEdge1 == MaxNodeNumber)
                {
                    nodeIndexOfDiffractEdge1 = i;//如果是第一条绕射棱更新第一条绕射棱下标信息
                }
                else if (this.node[i].NodeStyle == NodeStyle.DiffractionNode)
                {
                    nodeIndexOfDiffractEdge2 = i;//如果不是第一条绕射棱，更新第二条绕射棱下标信息
                }
                else if (this.node[i].NodeStyle != NodeStyle.DiffractionNode && this.node[i].NodeStyle != NodeStyle.ReflectionNode)
                {
                    return tempPointList;
                }
                else
                {
                    continue;
                }
            }
            if (nodeIndexOfDiffractEdge1 == MaxNodeNumber && nodeIndexOfDiffractEdge2 == MaxNodeNumber)
            {
                tempPointList = this.GetPointsOfOnlyReflectPath();//如果不存在绕射节点，进行全为反射的反向计算
            }
            else if (nodeIndexOfDiffractEdge1 != MaxNodeNumber && nodeIndexOfDiffractEdge2 == MaxNodeNumber)
            {
                tempPointList = this.GetPointsOfContainOneDiffractPath(nodeIndexOfDiffractEdge1);//如果仅存在一个绕射节点，进行含有一次绕射的反向计算
            }
            else if (nodeIndexOfDiffractEdge1 != MaxNodeNumber && nodeIndexOfDiffractEdge2 != MaxNodeNumber
                && Math.Abs(nodeIndexOfDiffractEdge2 - nodeIndexOfDiffractEdge1) == 1)
            {
                //如果存在两个绕射节点，且相邻，进行两次相邻绕射的反向计算
                tempPointList = this.GetPointsOfContainTwoAdjacentDiffractPath(nodeIndexOfDiffractEdge1, nodeIndexOfDiffractEdge2);
            }
            else if (nodeIndexOfDiffractEdge1 != MaxNodeNumber && nodeIndexOfDiffractEdge2 != MaxNodeNumber
                && Math.Abs(nodeIndexOfDiffractEdge2 - nodeIndexOfDiffractEdge1) != 1)
            {
                //如果存在两个绕射节点且不相邻，进行两次不相邻绕射的反向计算
                tempPointList = this.GetPointsOfContainTwoNoAdjacentDiffractPath(nodeIndexOfDiffractEdge1, nodeIndexOfDiffractEdge2);
            }
            return tempPointList;
        }

        /// <summary>
        /// 利用反向算法得到全为反射的路径中的反射点链表
        /// </summary>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfOnlyReflectPath()
        {
            List<Point> pointList = new List<Point>();
            Stack<Face> reflectTriangles = new Stack<Face>(), reflectTrianglesForMirror = new Stack<Face>();
            Stack<Point> mirrorPoints = new Stack<Point>(), frontPoints = new Stack<Point>();

            mirrorPoints.Push(this.node[this.node.Count - 1].Position);
            frontPoints.Push(this.node[0].Position);

            for (int i = 1; i < this.node.Count - 1; i++)
            {
                reflectTriangles.Push(this.node[i].ReflectionFace);//反射三角面顺序入栈
            }

            PushTrianglesAndMirrorPoints(reflectTriangles, reflectTrianglesForMirror, mirrorPoints);

            while (reflectTrianglesForMirror.Count != 0)
            {
                RayInfo tempRay = new RayInfo(frontPoints.Peek(), mirrorPoints.Pop());
                if (tempRay.GetCrossPointWithFace(reflectTrianglesForMirror.Peek()) != null)//若镜像点与前一交点连线与三角面有交点则将交点加入链表
                {
                    frontPoints.Push(tempRay.GetCrossPointWithFace(reflectTrianglesForMirror.Pop()));
                    pointList.Add(frontPoints.Peek());
                }
                else
                {
                    return new List<Point>();
                }
            }
            return pointList;
        }

        /// <summary>
        /// 利用反向算法得到仅含一次绕射的路径中的各个点的详细位置信息
        /// </summary>
        /// <param name="indexOfEdgeOfThisPath">绕射点所在的下标位置信息</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfContainOneDiffractPath(int indexOfEdgeOfThisPath)
        {
            List<Point> pointList = new List<Point>();
            Stack<Face> reflectTrianglesBeforeDiffract = new Stack<Face>(), reflectTrianglesForMirrorBeforeDiffract = new Stack<Face>(),
                reflectTrianglesAfterDiffract = new Stack<Face>(), reflectTrianglesForMirrorAfterDiffract = new Stack<Face>();
            Stack<Point> mirrorPointsBeforeDiffract = new Stack<Point>(), mirrorPointsAfterDiffract = new Stack<Point>();

            //预处理三角面和点
            DealTrianglesAndPointsBeforeAndAfterDiffractEdge(indexOfEdgeOfThisPath, indexOfEdgeOfThisPath,
                reflectTrianglesBeforeDiffract, reflectTrianglesAfterDiffract, mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);

            //将三角面和镜像点按处理顺序压入栈中
            PushTrianglesAndMirrorPoints(reflectTrianglesBeforeDiffract, reflectTrianglesForMirrorBeforeDiffract, mirrorPointsBeforeDiffract);
            PushTrianglesAndMirrorPoints(reflectTrianglesAfterDiffract, reflectTrianglesForMirrorAfterDiffract, mirrorPointsAfterDiffract);

            Point diffractPoint = this.node[indexOfEdgeOfThisPath].DiffractionEdge.GetDiffractionPoint(mirrorPointsBeforeDiffract.Peek(), mirrorPointsAfterDiffract.Peek());
            if (diffractPoint == null)
            {
                return new List<Point>();
            }

            Stack<Point> tempPoints = new Stack<Point>();
            tempPoints.Push(diffractPoint);
            pointList = GetPointListOfOneOrTwoAdjacentDiffractPath(
                tempPoints, reflectTrianglesForMirrorBeforeDiffract, reflectTrianglesForMirrorAfterDiffract,
                mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);//得到路径上的点

            return pointList;
        }

        /// <summary>
        /// 利用反向算法得到含有两次相邻绕射的路径中的各个点详细位置信息
        /// </summary>
        /// <param name="indexOfEdge1OfThisPath">第一次绕射节点所在下标位置信息</param>
        /// <param name="indexOfEdge2OfThisPath">第二次绕射节点所在下标位置信息</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfContainTwoAdjacentDiffractPath(int indexOfEdge1OfThisPath, int indexOfEdge2OfThisPath)
        {
            List<Point> pointList = new List<Point>();
            Stack<Face> reflectTrianglesBeforeDiffract = new Stack<Face>(), reflectTrianglesForMirrorBeforeDiffract = new Stack<Face>(),
                reflectTrianglesAfterDiffract = new Stack<Face>(), reflectTrianglesForMirrorAfterDiffract = new Stack<Face>();
            Stack<Point> mirrorPointsBeforeDiffract = new Stack<Point>(), mirrorPointsAfterDiffract = new Stack<Point>();

            //预处理绕射棱前后的点和三角面
            DealTrianglesAndPointsBeforeAndAfterDiffractEdge(indexOfEdge1OfThisPath, indexOfEdge2OfThisPath,
                reflectTrianglesBeforeDiffract, reflectTrianglesAfterDiffract, mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);

            //求镜像点并将三角面和镜像点按处理顺序压入栈中
            PushTrianglesAndMirrorPoints(reflectTrianglesBeforeDiffract, reflectTrianglesForMirrorBeforeDiffract, mirrorPointsBeforeDiffract);
            PushTrianglesAndMirrorPoints(reflectTrianglesAfterDiffract, reflectTrianglesForMirrorAfterDiffract, mirrorPointsAfterDiffract);

            List<Point> difffractPoints = GetTwoDiffractionPoints(
                this.node[indexOfEdge1OfThisPath].DiffractionEdge, this.node[indexOfEdge2OfThisPath].DiffractionEdge,
                mirrorPointsBeforeDiffract.Peek(), mirrorPointsAfterDiffract.Peek());//获得两个绕射点
            if (difffractPoints.Count == 0)
            {
                return new List<Point>();
            }

            Stack<Point> tempPoints = new Stack<Point>();
            tempPoints.Push(difffractPoints[1]);
            tempPoints.Push(difffractPoints[0]);
            pointList = GetPointListOfOneOrTwoAdjacentDiffractPath(
                tempPoints, reflectTrianglesForMirrorBeforeDiffract, reflectTrianglesForMirrorAfterDiffract,
                mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);//得到路径上的点

            return pointList;
        }

        /// <summary>
        /// 利用反向算法得到含有两次不相邻绕射的路径中的各个点详细位置信息
        /// </summary>
        /// <param name="indexOfPath">所要计算的路径的下标</param>
        /// <param name="indexOfEdge1OfThisPath">第一次绕射节点所在下标位置信息</param>
        /// <param name="indexOfEdge2OfThisPath">第二次绕射节点所在下标位置信息</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfContainTwoNoAdjacentDiffractPath(int indexOfEdge1OfThisPath, int indexOfEdge2OfThisPath)
        {
            List<Point> pointList = new List<Point>();
            for (int i = 0; i < this.node.Count - 2; i++)
            {
                pointList.Add(new Point());
            }
            Stack<Face> reflectTrianglesBeforeDiffract = new Stack<Face>(), reflectTrianglesForMirrorBeforeDiffract = new Stack<Face>(),
                reflectTrianglesAfterDiffract = new Stack<Face>(), reflectTrianglesForMirrorAfterDiffract = new Stack<Face>(),
                reflectTrianglesBetweenDiffract = new Stack<Face>();
            Stack<Point> mirrorPointsBeforeDiffract = new Stack<Point>(), mirrorPointsAfterDiffract = new Stack<Point>(),
                mirrorPointsBetweenDiffract = new Stack<Point>();
            Stack<AdjacentEdge> mirrorDiffractEdge = new Stack<AdjacentEdge>();

            //预处理绕射棱前后的三角面和点
            DealTrianglesAndPointsBeforeAndAfterDiffractEdge(indexOfEdge1OfThisPath, indexOfEdge2OfThisPath,
                reflectTrianglesBeforeDiffract, reflectTrianglesAfterDiffract, mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);

            //求绕射两棱前后的镜像点
            PushTrianglesAndMirrorPoints(reflectTrianglesBeforeDiffract, reflectTrianglesForMirrorBeforeDiffract, mirrorPointsBeforeDiffract);
            PushTrianglesAndMirrorPoints(reflectTrianglesAfterDiffract, reflectTrianglesForMirrorAfterDiffract, mirrorPointsAfterDiffract);
            mirrorPointsBetweenDiffract.Push(mirrorPointsBeforeDiffract.Peek());
            mirrorDiffractEdge.Push(this.node[indexOfEdge1OfThisPath].DiffractionEdge);


            AdjacentEdge updatedDiffractEdge2 = this.node[indexOfEdge2OfThisPath].DiffractionEdge;
            AdjacentEdge tempDiffractEdge2 = GetNewDiffractEdge(this.node[indexOfEdge2OfThisPath - 1].ReflectionFace,
                this.node[indexOfEdge2OfThisPath].DiffractionEdge, mirrorPointsAfterDiffract.Peek());//利用绕射棱前一三角面更新绕射棱信息
            updatedDiffractEdge2.StartPoint = tempDiffractEdge2.StartPoint;
            updatedDiffractEdge2.EndPoint = tempDiffractEdge2.EndPoint;

            //作点和第一绕射棱关于两绕射棱之间的面的镜像
            for (int i = indexOfEdge1OfThisPath + 1; i < indexOfEdge2OfThisPath; i++)
            {
                reflectTrianglesBetweenDiffract.Push(this.node[i].ReflectionFace);
                mirrorPointsBetweenDiffract.Push(mirrorPointsBetweenDiffract.Peek().GetMirrorPoint(reflectTrianglesBetweenDiffract.Peek()));
                mirrorDiffractEdge.Push(new AdjacentEdge(mirrorDiffractEdge.Peek().StartPoint.GetMirrorPoint(reflectTrianglesBetweenDiffract.Peek()),
                        mirrorDiffractEdge.Peek().EndPoint.GetMirrorPoint(reflectTrianglesBetweenDiffract.Peek())));
            }

            List<Point> diffractPoints = GetTwoDiffractionPoints(mirrorDiffractEdge.Pop(), updatedDiffractEdge2,
                mirrorPointsBetweenDiffract.Pop(), mirrorPointsAfterDiffract.Peek());
            if (diffractPoints.Count() == 0)
            {
                return new List<Point>();
            }

            pointList[indexOfEdge2OfThisPath - 1] = diffractPoints[1];//第二绕射棱上绕射点坐标
            pointList[indexOfEdge2OfThisPath - 2] = GetReflectPoint(diffractPoints[0], diffractPoints[1], reflectTrianglesBetweenDiffract.Pop());
            if (pointList[indexOfEdge2OfThisPath - 2] == null)//判断绕射棱前一三角面的反射点是否存在
            {
                return new List<Point>();
            }

            if (reflectTrianglesBetweenDiffract.Count != 0)//两绕射棱间是否有未处理三角面
            {
                pointList[indexOfEdge2OfThisPath - 3] = GetReflectPoint(mirrorDiffractEdge.Pop().GetDiffractionPoint(
                    mirrorPointsBetweenDiffract.Pop(), pointList[indexOfEdge2OfThisPath - 2]),
                    pointList[indexOfEdge2OfThisPath - 2], reflectTrianglesBetweenDiffract.Pop());
                if (pointList[indexOfEdge2OfThisPath - 3] == null)
                {
                    return new List<Point>();
                }
            }
            pointList[indexOfEdge1OfThisPath - 1] = this.node[indexOfEdge1OfThisPath].
                DiffractionEdge.GetDiffractionPoint(mirrorPointsBeforeDiffract.Peek(), pointList[indexOfEdge1OfThisPath]);

            if (reflectTrianglesBeforeDiffract.Count != 0)//第一条棱前是否有反射点
            {
                pointList[indexOfEdge1OfThisPath - 2] = GetReflectPoint(
                    mirrorPointsBeforeDiffract.Pop(), pointList[indexOfEdge1OfThisPath - 1], reflectTrianglesBeforeDiffract.Pop());
                if (pointList[indexOfEdge1OfThisPath - 2] == null)
                {
                    return new List<Point>();
                }
            }

            if (reflectTrianglesAfterDiffract.Count != 0)//第二条棱后是否有反射点
            {
                pointList[indexOfEdge2OfThisPath] = GetReflectPoint(pointList[indexOfEdge2OfThisPath - 1], mirrorPointsAfterDiffract.Pop(),
                    reflectTrianglesAfterDiffract.Pop());
                if (pointList[indexOfEdge2OfThisPath] == null)
                {
                    return new List<Point>();
                }
            }

            return pointList;
        }

        

        /// <summary>
        /// 对原面栈中的三角面依次求镜像点，并将镜像点压入点栈中，并将三角面倒序压入镜像面栈中
        /// </summary>
        /// <param name="reflectTriangles">原面栈</param>
        /// <param name="reflectTrianglesForMirror">镜像点栈</param>
        /// <param name="mirrorPoints">对应镜像点的面栈</param>
        private static void PushTrianglesAndMirrorPoints(Stack<Face> reflectTriangles, Stack<Face> reflectTrianglesForMirror, Stack<Point> mirrorPoints)
        {
            while (reflectTriangles.Count != 0)
            {
                mirrorPoints.Push(mirrorPoints.Peek().GetMirrorPoint(reflectTriangles.Peek()));//镜像点倒序入栈
                reflectTrianglesForMirror.Push(reflectTriangles.Pop());//反射三角面倒序入栈
            }
        }

        /// <summary>
        /// 处理并储存绕射棱前后的点和面信息
        /// </summary>
        /// <param name="indexOfEdge1">第一绕射棱</param>
        /// <param name="indexOfEdge2">第二绕射棱</param>
        /// <param name="reflectTrianglesBeforeDiffract">储存第一绕射棱前的面信息</param>
        /// <param name="reflectTrianglesAfterDiffract">储存第二绕射棱后的面信息</param>
        /// <param name="mirrorPointsBeforeDiffract">储存第一绕射棱前的点信息</param>
        /// <param name="mirrorPointsAfterDiffract">储存第二绕射棱前的点信息</param>
        private void DealTrianglesAndPointsBeforeAndAfterDiffractEdge(int indexOfEdge1, int indexOfEdge2,
            Stack<Face> reflectTrianglesBeforeDiffract, Stack<Face> reflectTrianglesAfterDiffract,
            Stack<Point> mirrorPointsBeforeDiffract, Stack<Point> mirrorPointsAfterDiffract)
        {
            mirrorPointsBeforeDiffract.Push(this.node[0].Position);
            mirrorPointsAfterDiffract.Push(this.node[this.node.Count - 1].Position);

            if (indexOfEdge1 != 1)//判断绕射棱前有无反射面
            {
                for (int i = indexOfEdge1 - 1; i > 0; i--)
                    reflectTrianglesBeforeDiffract.Push(this.node[i].ReflectionFace);
            }
            if (indexOfEdge2 != this.node.Count - 2)//判断绕射棱后有无反射面
            {
                for (int j = indexOfEdge2 + 1; j < this.node.Count - 1; j++)
                    reflectTrianglesAfterDiffract.Push(this.node[j].ReflectionFace);
            }
        }

        /// <summary>
        /// 判断是否有反射点并返回
        /// </summary>
        /// <param name="originPoint">反射面前一点</param>
        /// <param name="targetPoint">反射面后一点</param>
        /// <param name="reflectTriangle">反射面</param>
        /// <returns></returns>
        private static Point GetReflectPoint(Point originPoint, Point targetPoint, Face reflectTriangle)
        {
            RayInfo rayTemp = new RayInfo(originPoint, targetPoint);
            if (rayTemp.GetCrossPointWithFace(reflectTriangle) != null)
            {
                return rayTemp.GetCrossPointWithFace(reflectTriangle);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 用于计算在一次绕射和两次相邻绕射中将各个点的信息添加到pointList中的过程
        /// </summary>
        /// <param name="tempPoints">临时点，栈用于将绕射点前倒序的反射点顺序输出</param>
        /// <param name="beforeTriangles">存放绕射前的三角面</param>
        /// <param name="afterTiangles">存放绕射后的三角面</param>
        /// <param name="beforeMirror">存放绕射前的镜像点</param>
        /// <param name="afterMirror">存放绕射后的镜像点</param>
        /// <param name="pointList"></param>
        /// <returns>顺序存储的各个点的坐标</returns>
        private static List<Point> GetPointListOfOneOrTwoAdjacentDiffractPath(
            Stack<Point> tempPoints, Stack<Face> beforeTriangles, Stack<Face> afterTiangles,
            Stack<Point> beforeMirror, Stack<Point> afterMirror)
        {
            List<Point> pointList = new List<Point>(); 
            while (beforeTriangles.Count != 0)
            {
                RayInfo tempRay = new RayInfo(tempPoints.Peek(), beforeMirror.Pop());
                if (tempRay.GetCrossPointWithFace(beforeTriangles.Peek()) != null)
                {
                    tempPoints.Push(tempRay.GetCrossPointWithFace(beforeTriangles.Pop()));
                }
                else
                {
                    return new List<Point>();
                }
            }
            while (tempPoints.Count != 0)
                pointList.Add(tempPoints.Pop());
            while (afterTiangles.Count != 0)
            {
                RayInfo tempRay = new RayInfo(pointList[pointList.Count - 1], afterMirror.Pop());
                if (tempRay.GetCrossPointWithFace(afterTiangles.Peek()) != null)
                {
                    pointList.Add(tempRay.GetCrossPointWithFace(afterTiangles.Pop()));
                }
                else
                {
                    return new List<Point>();
                }
            }
            return pointList;
        }

        /// <summary>
        /// 更新绕射棱范围
        /// </summary>
        /// <param name="triangle">绕射棱前一三角面</param>
        /// <param name="diffractEdge">要更新的绕射棱</param>
        /// <param name="point">接收点（与三角面共同确定棱的范围）</param>
        /// <returns>更新后的绕射棱</returns>
        private static AdjacentEdge GetNewDiffractEdge(Face triangle, AdjacentEdge diffractEdge, Point point)
        {
            AdjacentEdge newDiffractEdge = new AdjacentEdge();
            Point point1 = diffractEdge.GetInfiniteDiffractionPointWhenInLine(triangle.Vertices[0], point);
            Point point2 = diffractEdge.GetInfiniteDiffractionPointWhenInLine(triangle.Vertices[1], point);
            Point point3 = diffractEdge.GetInfiniteDiffractionPointWhenInLine(triangle.Vertices[2], point);
            double distance12 = point1.GetDistance(point2);
            double distance13 = point1.GetDistance(point3);
            double distance23 = point2.GetDistance(point3);
            if (distance12 >= distance13 && distance12 >= distance23)
            {
                newDiffractEdge.StartPoint = point1;
                newDiffractEdge.EndPoint = point2;
            }
            else if (distance13 >= distance12 && distance13 >= distance23)
            {
                newDiffractEdge.StartPoint = point1;
                newDiffractEdge.EndPoint = point3;
            }
            else
            {
                newDiffractEdge.StartPoint = point2;
                newDiffractEdge.EndPoint = point3;
            }
            return newDiffractEdge;
        }

        /// <summary>
        /// 计算两次绕射的绕射点
        /// </summary>
        /// <param name="edge1">第一条绕射棱</param>
        /// <param name="edge2">第二条绕射棱</param>
        /// <param name="beginPoint">第一条绕射棱前的点</param>
        /// <param name="endPoint">第二条绕射棱后的点</param>
        /// <returns>用List的形式返回两个点的坐标</returns>
        public static List<Point> GetTwoDiffractionPoints(AdjacentEdge edge1, AdjacentEdge edge2, Point beginPoint, Point lastPoint)
        {
            
            //求由第二条棱的两端点确定的第一条棱无限延长线上的两绕射点
            Point p1 = edge1.GetInfiniteDiffractionPoint(beginPoint, edge2.StartPoint);
            Point p2 = edge1.GetInfiniteDiffractionPoint(beginPoint, edge2.EndPoint);
            if (p1 == null || p2 == null)
            {
                return new List<Point>();
            }
            //第二条棱两端点
            Point p21 = edge2.StartPoint;
            Point p22 = edge2.EndPoint;


            AdjacentEdge tempLine = new AdjacentEdge(
                new Point(-10000 * edge2.LineVector.a + lastPoint.X, -10000 * edge2.LineVector.b + lastPoint.Y, -10000 * edge2.LineVector.c + lastPoint.Z),
                new Point(10000 * edge2.LineVector.a + lastPoint.X, 10000 * edge2.LineVector.b + lastPoint.Y, 10000 * edge2.LineVector.c + lastPoint.Z),
                edge2.LineVector);//获得过点endPoint与第二条棱平行共面的长线段
            double angle1 = SpectVector.VectorPhase(new SpectVector(p1, p21), edge2.LineVector);//得到第二条棱的入射角
            double angle2 = SpectVector.VectorPhase(new SpectVector(p2, p22), edge2.LineVector);
            bool isFirst = true;//用来判断是第一条路径，还是第二条路径
            List<RayInfo> rays = edge2.getVectorFromEdge2EdgeWithAngle(p21, angle1, tempLine);
            if (rays.Count==0)
            {
                return new List<Point>();
            }
            RayInfo ray1 = rays[0];//因为tempLine和edge2时平行的，所以只会有一条returnRay默认为returnRay[0]
            RayInfo ray2 = rays[0];           
            SpectVector vector1 = new SpectVector(lastPoint, ray1.Origin);
            SpectVector vector2 = new SpectVector(lastPoint, ray2.Origin);
            if (SpectVector.VectorDotMultiply(vector1, vector2) > 0&&rays.Count>1)  //点乘大于0.表示在棱的外部
            {
                ray1 =rays[1];
                ray2 =rays[1];
                vector1 = new SpectVector(lastPoint, ray1.Origin);
                vector2 = new SpectVector(lastPoint, ray2.Origin);
                if (SpectVector.VectorDotMultiply(vector1,vector2)>0)
                {
                    return new List<Point>();
                }
                else
                {
                    //.. 
                    isFirst = false;//这时候lastPoint在第二条射线所形成的区域。
                }
                
            }

            double distance1 = lastPoint.GetDistanceToLine(p21, ray1.RayVector);
            double distance2 = lastPoint.GetDistanceToLine(p22, ray2.RayVector);

            List<Point> diffractPoints = new List<Point>();

            while (p1.GetDistance(p2) > 0.000001)
            {
                if (distance1 < 0.0001)
                {
                    if (edge1.JudgeIfPointInLineRange(p1))
                    {
                        diffractPoints.Add(p1);
                        diffractPoints.Add(p21);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                if (distance2 < 0.0001)
                {
                    if (edge1.JudgeIfPointInLineRange(p2))
                    {
                        diffractPoints.Add(p2);
                        diffractPoints.Add(p22);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                Point tempPoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                double tempAngle1 = SpectVector.VectorPhase(new SpectVector(beginPoint, tempPoint), edge1.LineVector);
                RayInfo tempRay1 = new RayInfo(new Point(0,0,0),new SpectVector(0,0,1));//初始化射线，等待赋值
                double tempAngle2 = 0;
                List<RayInfo> tempRaysOfTempLine = new List<RayInfo>();
                //RayInfo tempRay2 = new RayInfo(new Point(0, 0, 0), new SpectVector(0, 0, 1));//初始化射线，等待赋值
                double tempDistance = 0;
                List<RayInfo> tempRaysOfEdge2 = edge1.getVectorFromEdge2EdgeWithAngle(tempPoint, tempAngle1, edge2);
                if (isFirst&&tempRaysOfEdge2.Count!=0)
                {
                     tempRay1 = tempRaysOfEdge2[0];
                     tempAngle2 = SpectVector.VectorPhase(tempRay1.RayVector, edge2.LineVector);
                     tempRaysOfTempLine = edge2.getVectorFromEdge2EdgeWithAngle(tempRay1.Origin, tempAngle2, tempLine);
                    if (tempRaysOfTempLine.Count!=0)
                    {
                        tempDistance = lastPoint.GetDistanceToLine(tempRaysOfTempLine[0].Origin, tempRaysOfTempLine[0].RayVector);
                    }
                     
                }
                else if(edge1.getVectorFromEdge2EdgeWithAngle(tempPoint, tempAngle1, edge2).Count==2&&isFirst==false)
                {
                    tempRay1 = tempRaysOfEdge2[1];
                    tempAngle2 = SpectVector.VectorPhase(tempRay1.RayVector, edge2.LineVector);
                     tempRaysOfTempLine = edge2.getVectorFromEdge2EdgeWithAngle(tempRay1.Origin, tempAngle2, tempLine);
                    if (tempRaysOfTempLine.Count != 0)
                    {
                        tempDistance = lastPoint.GetDistanceToLine(tempRaysOfTempLine[0].Origin, tempRaysOfTempLine[0].RayVector);
                    }
                }
                else
                {
                    return new List<Point>();
                }
                RayInfo tempRay2 = tempRaysOfTempLine[0];
                SpectVector tempVector1 = new SpectVector(lastPoint, ray1.Origin);
                SpectVector tempVector2 = new SpectVector(lastPoint, tempRay2.Origin);

                if (SpectVector.VectorDotMultiply(tempVector1, tempVector2) < 0)
                {
                    p2 = tempPoint;
                    p22 = tempRay1.Origin;
                    distance2 = tempDistance;
                    ray2 = tempRay2;
                }
                else
                {
                    p1 = tempPoint;
                    p21 = tempRay1.Origin;
                    distance1 = tempDistance;
                    ray1 = tempRay2;
                }
                
            }
            if (diffractPoints.Count == 0 && p1.GetDistance(p2) <=0.000001)
            {
                Point p3 = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                Point p23 = edge2.GetInfiniteDiffractionPoint(p3, lastPoint);
                double angle11 = SpectVector.VectorPhase(new SpectVector(beginPoint, p3), edge1.LineVector);
                double angle12 = SpectVector.VectorPhase(new SpectVector(p3, p23), edge1.LineVector);
                if (Math.Abs(angle11 - angle12) < 0.001)
                {
                    diffractPoints.Add(p3);
                    diffractPoints.Add(p23);
                }
            }
            return diffractPoints;
        } 
    
    }
}
