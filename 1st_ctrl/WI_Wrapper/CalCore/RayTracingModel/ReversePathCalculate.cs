using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using FileObject;
using TxRxFileProceed;
using System.Configuration;
using log4net;

namespace RayCalInfo
{
    /// <summary>
    /// 反向计算类
    /// </summary>
    class ReversePathsCalculate
    {
        private const int MaxNodeNumber = 6;
        private List<Path> paths;

        public ReversePathsCalculate(List<Path> paths)
        {
            this.paths = paths;
        }

        /// <summary>
        /// 更新路径节点信息（路径存在更新为准确点，不存在从链表中删掉）
        /// </summary>
        private void UpdateReversePaths()
        {
            for (int i = 0; i < this.paths.Count; i++)
            {
                if (this.paths[i].node.Count > 2)//直射不需要进行反向
                {
                    List<Point> pointsOfPath = this.GetPointsOfPath(this.paths[i]);
                    if (pointsOfPath.Count == 0)
                    {
                        //移除路径
                        this.paths.RemoveAt(i);
                    }
                    else
                    {
                        //更新路径
                        for (int j = 1; j < paths[i].node.Count - 1; j++)
                        {
                            paths[i].node[j].Position = pointsOfPath[j - 1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 利用反向算法获取路径上准确的反射点和绕射点
        /// </summary>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfPath(Path path)
        {
            List<Point> tempPointList = new List<Point>();
            int nodeIndexOfDiffractEdge1 = MaxNodeNumber, nodeIndexOfDiffractEdge2 = MaxNodeNumber;
            for (int i = 1; i < path.node.Count - 1; i++)
            {
                if (path.node[i].NodeStyle == NodeStyle.DiffractionNode && nodeIndexOfDiffractEdge1 == MaxNodeNumber)
                {
                    nodeIndexOfDiffractEdge1 = i;//如果是第一条绕射棱更新第一条绕射棱下标信息
                }
                else if (path.node[i].NodeStyle == NodeStyle.DiffractionNode)
                {
                    nodeIndexOfDiffractEdge2 = i;//如果不是第一条绕射棱，更新第二条绕射棱下标信息
                }
                else
                {
                    continue;
                }
            }
            if (nodeIndexOfDiffractEdge1 == MaxNodeNumber && nodeIndexOfDiffractEdge2 == MaxNodeNumber)
            {
                tempPointList = this.GetPointsOfAllReflectPath(path);//如果不存在绕射节点，进行全为反射的反向计算
            }
            else if (nodeIndexOfDiffractEdge1 != MaxNodeNumber && nodeIndexOfDiffractEdge2 == MaxNodeNumber)
            {
                tempPointList = this.GetPointsOfContainOneDiffractPath(path, nodeIndexOfDiffractEdge1);//如果仅存在一个绕射节点，进行含有一次绕射的反向计算
            }
            else if (nodeIndexOfDiffractEdge1 != MaxNodeNumber && nodeIndexOfDiffractEdge2 != MaxNodeNumber
                && Math.Abs(nodeIndexOfDiffractEdge2 - nodeIndexOfDiffractEdge1) == 1)
            {
                //如果存在两个绕射节点，且相邻，进行两次相邻绕射的反向计算
                tempPointList = this.GetPointsOfContainTwoAdjacentDiffractPath(path, nodeIndexOfDiffractEdge1, nodeIndexOfDiffractEdge2);
            }
            else if (nodeIndexOfDiffractEdge1 != MaxNodeNumber && nodeIndexOfDiffractEdge2 != MaxNodeNumber
                && Math.Abs(nodeIndexOfDiffractEdge2 - nodeIndexOfDiffractEdge1) != 1)
            {
                //如果存在两个绕射节点且不相邻，进行两次不相邻绕射的反向计算
                tempPointList = this.GetPointsOfContainTwoNoAdjacentDiffractPath(path, nodeIndexOfDiffractEdge1, nodeIndexOfDiffractEdge2);
            }
            return tempPointList;
        }

        /// <summary>
        /// 利用反向算法得到全为反射的路径中的反射点链表
        /// </summary>
        /// <param name="path">传入要进行反向的路径</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        public List<Point> GetPointsOfAllReflectPath(Path path)
        {
            List<Point> pointList = new List<Point>();
            Stack<Triangle> reflectTriangles = new Stack<Triangle>(), reflectTrianglesForMirror = new Stack<Triangle>();
            Stack<Point> mirrorPoints = new Stack<Point>(), frontPoints = new Stack<Point>();

            mirrorPoints.Push(path.node[path.node.Count - 1].Position);
            frontPoints.Push(path.node[0].Position);

            for (int i = 1; i < path.node.Count - 1; i++)
            {
                reflectTriangles.Push(path.node[i].ReflectionFace);//反射三角面顺序入栈
            }

            this.PushTrianglesAndMirrorPoints(reflectTriangles, reflectTrianglesForMirror, mirrorPoints);

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
        /// <param name="path">传入要进行计算的路径</param>
        /// <param name="indexOfEdge">绕射点所在的下标位置信息</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfContainOneDiffractPath(Path path, int indexOfEdge)
        {
            List<Point> pointList = new List<Point>();
            Stack<Triangle> reflectTrianglesBeforeDiffract = new Stack<Triangle>(), reflectTrianglesForMirrorBeforeDiffract = new Stack<Triangle>(),
                reflectTrianglesAfterDiffract = new Stack<Triangle>(), reflectTrianglesForMirrorAfterDiffract = new Stack<Triangle>();
            Stack<Point> mirrorPointsBeforeDiffract = new Stack<Point>(), mirrorPointsAfterDiffract = new Stack<Point>();

            //预处理三角面和点
            this.DealTrianglesAndPointsBeforeAndAfterDiffractEdge(path, indexOfEdge, indexOfEdge, 
                reflectTrianglesBeforeDiffract, reflectTrianglesAfterDiffract, mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);

            //将三角面和镜像点按处理顺序压入栈中
            this.PushTrianglesAndMirrorPoints(reflectTrianglesBeforeDiffract, reflectTrianglesForMirrorBeforeDiffract, mirrorPointsBeforeDiffract);
            this.PushTrianglesAndMirrorPoints(reflectTrianglesAfterDiffract, reflectTrianglesForMirrorAfterDiffract, mirrorPointsAfterDiffract);

            Point diffractPoint = path.node[indexOfEdge].DiffractionEdge.GetDiffractionPoint(mirrorPointsBeforeDiffract.Peek(), mirrorPointsAfterDiffract.Peek());
            if (diffractPoint == null)
            {
                return new List<Point>();
            }

            Stack<Point> tempPoints = new Stack<Point>();
            tempPoints.Push(diffractPoint);
            pointList = this.GetPointListOfOneOrTwoAdjacentDiffractPath(
                tempPoints, reflectTrianglesForMirrorBeforeDiffract, reflectTrianglesForMirrorAfterDiffract,
                mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);//得到路径上的点

            return pointList;
        }

        /// <summary>
        /// 利用反向算法得到含有两次相邻绕射的路径中的各个点详细位置信息
        /// </summary>
        /// <param name="path">传入要计算的路径</param>
        /// <param name="indexOfEdge1">第一次绕射节点所在下标位置信息</param>
        /// <param name="indexOfEdge2">第二次绕射节点所在下标位置信息</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfContainTwoAdjacentDiffractPath(Path path, int indexOfEdge1, int indexOfEdge2)
        {
            List<Point> pointList = new List<Point>();
            Stack<Triangle> reflectTrianglesBeforeDiffract = new Stack<Triangle>(), reflectTrianglesForMirrorBeforeDiffract = new Stack<Triangle>(),
                reflectTrianglesAfterDiffract = new Stack<Triangle>(), reflectTrianglesForMirrorAfterDiffract = new Stack<Triangle>();
            Stack<Point> mirrorPointsBeforeDiffract = new Stack<Point>(), mirrorPointsAfterDiffract = new Stack<Point>();

            //预处理绕射棱前后的点和三角面
            this.DealTrianglesAndPointsBeforeAndAfterDiffractEdge(path, indexOfEdge1, indexOfEdge2,
                reflectTrianglesBeforeDiffract, reflectTrianglesAfterDiffract, mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);

            //求镜像点并将三角面和镜像点按处理顺序压入栈中
            this.PushTrianglesAndMirrorPoints(reflectTrianglesBeforeDiffract, reflectTrianglesForMirrorBeforeDiffract, mirrorPointsBeforeDiffract);
            this.PushTrianglesAndMirrorPoints(reflectTrianglesAfterDiffract, reflectTrianglesForMirrorAfterDiffract, mirrorPointsAfterDiffract);

            List<Point> difffractPoints = this.GetTwoDiffractionPoints(
                path.node[indexOfEdge1].DiffractionEdge, path.node[indexOfEdge2].DiffractionEdge,
                mirrorPointsBeforeDiffract.Peek(), mirrorPointsAfterDiffract.Peek());//获得两个绕射点

            Stack<Point> tempPoints = new Stack<Point>();
            tempPoints.Push(difffractPoints[1]);
            tempPoints.Push(difffractPoints[0]);
            pointList = this.GetPointListOfOneOrTwoAdjacentDiffractPath(
                tempPoints, reflectTrianglesForMirrorBeforeDiffract, reflectTrianglesForMirrorAfterDiffract,
                mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);//得到路径上的点

            return pointList;
        }

        /// <summary>
        /// 利用反向算法得到含有两次不相邻绕射的路径中的各个点详细位置信息
        /// </summary>
        /// <param name="path">传入要计算的路径</param>
        /// <param name="indexOfEdge1">第一次绕射节点所在下标位置信息</param>
        /// <param name="indexOfEdge2">第二次绕射节点所在下标位置信息</param>
        /// <returns>按路径经过顺序存储的各个点的坐标</returns>
        private List<Point> GetPointsOfContainTwoNoAdjacentDiffractPath(Path path, int indexOfEdge1, int indexOfEdge2)
        {
            List<Point> pointList = new List<Point>(path.node.Count-2);
            Stack<Triangle> reflectTrianglesBeforeDiffract = new Stack<Triangle>(), reflectTrianglesForMirrorBeforeDiffract = new Stack<Triangle>(),
                reflectTrianglesAfterDiffract = new Stack<Triangle>(), reflectTrianglesForMirrorAfterDiffract = new Stack<Triangle>(),
                reflectTrianglesBetweenDiffract = new Stack<Triangle>();
            Stack<Point> mirrorPointsBeforeDiffract = new Stack<Point>(), mirrorPointsAfterDiffract = new Stack<Point>(),
                mirrorPointsBetweenDiffract = new Stack<Point>();
            Stack<AdjacentEdge> mirrorDiffractEdge = new Stack<AdjacentEdge>();

            //预处理绕射棱前后的三角面和点
            this.DealTrianglesAndPointsBeforeAndAfterDiffractEdge(path, indexOfEdge1, indexOfEdge2,
                reflectTrianglesBeforeDiffract, reflectTrianglesAfterDiffract, mirrorPointsBeforeDiffract, mirrorPointsAfterDiffract);

            //求绕射两棱前后的镜像点
            this.PushTrianglesAndMirrorPoints(reflectTrianglesBeforeDiffract, reflectTrianglesForMirrorBeforeDiffract, mirrorPointsBeforeDiffract);
            this.PushTrianglesAndMirrorPoints(reflectTrianglesAfterDiffract, reflectTrianglesForMirrorAfterDiffract, mirrorPointsAfterDiffract);
            mirrorPointsBetweenDiffract.Push(mirrorPointsBeforeDiffract.Peek());
            mirrorDiffractEdge.Push(path.node[indexOfEdge1].DiffractionEdge);


            AdjacentEdge updatedDiffractEdge2 = path.node[indexOfEdge2].DiffractionEdge;
            AdjacentEdge tempDiffractEdge2 = this.GetNewDiffractEdge(path.node[indexOfEdge2 - 1].ReflectionFace,
                path.node[indexOfEdge2].DiffractionEdge, mirrorPointsAfterDiffract.Peek());//利用绕射棱前一三角面更新绕射棱信息
            updatedDiffractEdge2.StartPoint = tempDiffractEdge2.StartPoint;
            updatedDiffractEdge2.EndPoint = tempDiffractEdge2.EndPoint;

            //作点和第一绕射棱关于两绕射棱之间的面的镜像
            for (int i = indexOfEdge1 + 1; i < indexOfEdge2; i++)
            {
                reflectTrianglesBetweenDiffract.Push(path.node[i].ReflectionFace);
                mirrorPointsBetweenDiffract.Push(mirrorPointsBetweenDiffract.Peek().GetMirrorPoint(reflectTrianglesBetweenDiffract.Peek()));
                mirrorDiffractEdge.Push(new AdjacentEdge(mirrorDiffractEdge.Peek().StartPoint.GetMirrorPoint(reflectTrianglesBetweenDiffract.Peek()),
                        mirrorDiffractEdge.Peek().EndPoint.GetMirrorPoint(reflectTrianglesBetweenDiffract.Peek())));
            }

            List<Point> diffractPoints = this.GetTwoDiffractionPoints(mirrorDiffractEdge.Pop(), updatedDiffractEdge2, 
                mirrorPointsBetweenDiffract.Pop(), mirrorPointsAfterDiffract.Peek());
            pointList[indexOfEdge2 - 1] = diffractPoints[1];//第二绕射棱上绕射点坐标
            pointList[indexOfEdge2 - 2] = this.GetReflectPoint(diffractPoints[0], diffractPoints[1], reflectTrianglesBetweenDiffract.Pop());
            if (pointList[indexOfEdge2 - 2] == null)//判断绕射棱前一三角面的反射点是否存在
            {
                return new List<Point>();
            }

            if (reflectTrianglesBetweenDiffract.Count != 0)//两绕射棱间是否有未处理三角面
            {
                pointList[indexOfEdge2 - 3] = this.GetReflectPoint(mirrorDiffractEdge.Pop().GetDiffractionPoint(mirrorPointsBetweenDiffract.Pop(),
                    pointList[indexOfEdge2 - 2]), pointList[indexOfEdge2 - 2], reflectTrianglesBetweenDiffract.Pop());
                if (pointList[indexOfEdge2 - 3] == null)
                {
                    return new List<Point>();
                }
            }
            pointList[indexOfEdge1 - 1] = path.node[indexOfEdge1].
                DiffractionEdge.GetDiffractionPoint(mirrorPointsBeforeDiffract.Peek(), pointList[indexOfEdge1]);

            if (reflectTrianglesBeforeDiffract.Count != 0)//第一条棱前是否有反射点
            {
                pointList[indexOfEdge1 - 2] = this.GetReflectPoint(
                    mirrorPointsBeforeDiffract.Pop(), pointList[indexOfEdge1 - 1], reflectTrianglesBeforeDiffract.Pop());
                if (pointList[indexOfEdge1 - 2] == null)
                {
                    return new List<Point>();
                }
            }

            if (reflectTrianglesAfterDiffract.Count != 0)//第二条棱后是否有反射点
            {
                pointList[indexOfEdge2] = this.GetReflectPoint(pointList[indexOfEdge2 - 1], mirrorPointsAfterDiffract.Pop(),
                    reflectTrianglesAfterDiffract.Pop());
                if (pointList[indexOfEdge2] == null)
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
        private void PushTrianglesAndMirrorPoints(Stack<Triangle> reflectTriangles, Stack<Triangle> reflectTrianglesForMirror, Stack<Point> mirrorPoints)
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
        /// <param name="path">要进行处理的路径</param>
        /// <param name="indexOfEdge1">第一绕射棱</param>
        /// <param name="indexOfEdge2">第二绕射棱</param>
        /// <param name="reflectTrianglesBeforeDiffract">储存第一绕射棱前的面信息</param>
        /// <param name="reflectTrianglesAfterDiffract">储存第二绕射棱后的面信息</param>
        /// <param name="mirrorPointsBeforeDiffract">储存第一绕射棱前的点信息</param>
        /// <param name="mirrorPointsAfterDiffract">储存第二绕射棱前的点信息</param>
        private void DealTrianglesAndPointsBeforeAndAfterDiffractEdge(Path path, int indexOfEdge1, int indexOfEdge2,
            Stack<Triangle> reflectTrianglesBeforeDiffract, Stack<Triangle> reflectTrianglesAfterDiffract,
            Stack<Point> mirrorPointsBeforeDiffract, Stack<Point> mirrorPointsAfterDiffract)
        {
            mirrorPointsBeforeDiffract.Push(path.node[0].Position);
            mirrorPointsAfterDiffract.Push(path.node[path.node.Count - 1].Position);

            if (indexOfEdge1 != 1)//判断绕射棱前有无反射面
            {
                for (int i = indexOfEdge1 - 1; i > 0; i--)
                    reflectTrianglesBeforeDiffract.Push(path.node[i].ReflectionFace);
            }
            if (indexOfEdge2 != path.node.Count - 2)//判断绕射棱后有无反射面
            {
                for (int j = indexOfEdge2 + 1; j < path.node.Count - 1; j++)
                    reflectTrianglesAfterDiffract.Push(path.node[j].ReflectionFace);
            }
        }

        /// <summary>
        /// 判断是否有反射点并返回
        /// </summary>
        /// <param name="originPoint">反射面前一点</param>
        /// <param name="targetPoint">反射面后一点</param>
        /// <param name="reflectTriangle">反射面</param>
        /// <returns></returns>
        private Point GetReflectPoint(Point originPoint, Point targetPoint, Triangle reflectTriangle)
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
        private List<Point> GetPointListOfOneOrTwoAdjacentDiffractPath(
            Stack<Point> tempPoints, Stack<Triangle> beforeTriangles, Stack<Triangle> afterTiangles,
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
        private AdjacentEdge GetNewDiffractEdge(Triangle triangle, AdjacentEdge diffractEdge, Point point)
        {
            AdjacentEdge newDiffractEdge = new AdjacentEdge();
            Point point1 = diffractEdge.GetDiffractionPoint(triangle.Vertices[0], point);
            Point point2 = diffractEdge.GetDiffractionPoint(triangle.Vertices[1], point);
            Point point3 = diffractEdge.GetDiffractionPoint(triangle.Vertices[2], point);
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
        private List<Point> GetTwoDiffractionPoints(AdjacentEdge edge1, AdjacentEdge edge2, Point beginPoint, Point endPoint)
        {

            Point p1 = edge1.GetDiffractionPoint(beginPoint, edge2.StartPoint);
            Point p2 = edge1.GetDiffractionPoint(beginPoint, edge2.EndPoint);

            Point p21 = edge2.StartPoint;
            Point p22 = edge2.EndPoint;


            AdjacentEdge tempLine = new AdjacentEdge(new Point(-10000 * edge2.LineVector.a + endPoint.X, -10000 * edge2.LineVector.b + endPoint.Y, -10000 * edge2.LineVector.c + endPoint.Z),
                                                           new Point(10000 * edge2.LineVector.a + endPoint.X, 10000 * edge2.LineVector.b + endPoint.Y, 10000 * edge2.LineVector.c + endPoint.Z),
                                                           edge2.LineVector);
            double angle1 = SpectVector.VectorPhase(new SpectVector(p1, edge2.StartPoint), edge2.LineVector);
            double angle2 = SpectVector.VectorPhase(new SpectVector(p2, edge2.EndPoint), edge2.LineVector);
            SpectVector sp1 = edge2.getVectorFromEdge2EdgeWithAngle(p1, angle1, tempLine).RayVector;
            SpectVector sp2 = edge2.getVectorFromEdge2EdgeWithAngle(p2, angle2, tempLine).RayVector;
            RayInfo ray1 = new RayInfo(p1, sp1);
            RayInfo ray2 = new RayInfo(p2, sp2);

            Point dropFoot1 = ray1.getDropFoot(endPoint);
            Point dropFoot2 = ray2.getDropFoot(endPoint);

            double distance1 = endPoint.GetDistance(dropFoot1);
            double distance2 = endPoint.GetDistance(dropFoot2);

            List<Point> diffractPoints = new List<Point>();

            int i = 20;
            while (i-- > 0)
            {
                if (distance1 < 0.00001)
                {
                    diffractPoints.Add(p1);
                    diffractPoints.Add(p21);
                    break;
                }
                if (distance2 < 0.00001)
                {
                    diffractPoints.Add(p2);
                    diffractPoints.Add(p22);
                    break;
                }

                Point tempPoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                double tempAngle1 = SpectVector.VectorPhase(new SpectVector(beginPoint, tempPoint), edge1.LineVector);
                RayInfo tempRay1 = edge1.getVectorFromEdge2EdgeWithAngle(tempPoint, tempAngle1, edge2);
                double tempAngle2 = SpectVector.VectorPhase(tempRay1.RayVector, edge2.LineVector);
                RayInfo tempRay2 = edge2.getVectorFromEdge2EdgeWithAngle(tempRay1.Origin, tempAngle2, tempLine);
                Point tempDropFoot = new RayInfo(tempRay1.Origin, tempRay2.RayVector).getDropFoot(endPoint);
                double tempDistance = tempDropFoot.GetDistance(endPoint);

                SpectVector tempSP1 = new SpectVector(dropFoot1, edge2.StartPoint);
                SpectVector tempSP2 = new SpectVector(dropFoot2, edge2.StartPoint);
                SpectVector tempSP3 = new SpectVector(tempDropFoot, edge2.StartPoint);
                SpectVector tempSP4 = new SpectVector(endPoint, edge2.StartPoint);
                double endAngle12 = SpectVector.VectorPhase(tempSP1, tempSP2);
                double endAngle13 = SpectVector.VectorPhase(tempSP1, tempSP3);
                double endAngle23 = SpectVector.VectorPhase(tempSP2, tempSP3);
                double endAngle14 = SpectVector.VectorPhase(tempSP1, tempSP4);

                if (endAngle14 > endAngle13)
                {
                    p1 = tempPoint;
                    angle1 = tempAngle2;
                    p21 = tempRay1.Origin;
                    dropFoot1 = tempDropFoot;
                    distance1 = tempDistance;
                }
                else if (endAngle14 < endAngle13)
                {
                    p2 = tempPoint;
                    angle2 = tempAngle2;
                    p22 = tempRay1.Origin;
                    dropFoot2 = tempDropFoot;
                    distance2 = tempDistance;
                }
                else
                {
                    if (distance1 < 0.1)
                    {
                        diffractPoints.Add(p1);
                        diffractPoints.Add(p21);
                        break;
                    }
                    if (distance2 < 0.1)
                    {
                        diffractPoints.Add(p2);
                        diffractPoints.Add(p22);
                        break;
                    }

                    throw new Exception("二次绕射反向问题");
                }

            }

            return diffractPoints;
        }
    }
}
