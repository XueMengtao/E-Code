using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using UanFileProceed;
using FileObject;
using TxRxFileProceed;
using System.Configuration;
using log4net;
using CityAndFloor;
using LogFileManager;

namespace RayCalInfo
{
    public class PunctiformPath
    {
        protected RayInfo inRay;//射线
        protected List<Rectangle> terShadowRects;//射线可能经过的地形矩形List

        public PunctiformPath()
        { }

        public PunctiformPath(RayInfo inRay,Terrain newTer)
        {
            this.inRay = inRay;
            terShadowRects = newTer.lineRect(this.inRay);
        }
        


        /// <summary>
        ///获取点状接收机的传播路径信息,功率和传播时延在路径筛选后计算
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="cityBuilding">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <param name="rays">射线</param>
        /// <param name="fatherNode">父节点，发射机则没有父节点</param>
        /// <returns></returns>
        public void SetPunctiformRayPathNodes(Node currentNode, Terrain ter, ReceiveBall rxBall, City cityBuilding, double rayBeamAngle,List<FrequencyBand> txFrequencyBand)
        {
            //设置接受球半径
            double rxBallDistance = currentNode.Position.GetDistance(rxBall.Receiver) + currentNode.RayTracingDistance;
            double errorFactor = 1;
      //      if (rayBeamAngle > 0.5 * Math.PI / 180)
      //      { errorFactor = 2; }
            if (currentNode.NodeStyle == NodeStyle.DiffractionNode)
            { errorFactor = 2; }
            rxBall.Radius = errorFactor * rxBallDistance * rayBeamAngle / Math.Sqrt(3);
            //求交点
            List<Node> crossNodes = GetCrossPointsWithEnvironment( rxBall, ter, cityBuilding, txFrequencyBand, errorFactor*20);
            if (crossNodes.Count == 0)//若射线与各个物体都没有交点
            {
                if (currentNode.NodeStyle != NodeStyle.Tx)
                { 
                    currentNode.IsEnd = true;
                }
                return;
            }
            else
            {
                for (int j = 0; j < crossNodes.Count; j++)
                {
                    if (crossNodes[j].NodeStyle == NodeStyle.ReflectionNode)//若为反射点
                    {
                        this.SetReflectionChildNode(currentNode, crossNodes[j], ter, cityBuilding, rxBall, rayBeamAngle,txFrequencyBand);
                        break;
                    }
                    else if (crossNodes[j].NodeStyle == NodeStyle.CylinderCrossNode)//若为绕射点,追踪绕射绕射，并继续追踪下一个点
                    {
                        this.GetDiffractionNode( currentNode, crossNodes[j], ter, cityBuilding, rxBall, rayBeamAngle,txFrequencyBand);
                    }
                    else//若为Rx点
                    {
                        SetRxChildNode( currentNode,crossNodes[j], rxBall);
                        break;
                    }
                }
            }

            
        }

    

        /// <summary>
        ///求射线与地形,接收球,建筑物的交点,并将交点按照与源点距离进行排序
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <param name="rxBall">接收球</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="multiple">绕射圆柱体半径倍数</param>
        /// <returns>返回交点</returns>
        private List<Node> GetCrossPointsWithEnvironment( ReceiveBall rxBall, Terrain ter, City cityBuilding, List<FrequencyBand> txFrequencyBand,double multiple)
        {
            Node crossWithTer = this.inRay.GetCrossNodeWithTerrainRects(this.terShadowRects);//记录射线与地形交点、与源点距离、所在面
            Node CrossWithReceive = rxBall.GetCrossNodeWithRxBall(this.inRay);//记录射线与接收球交点
            List<Node> CrossWithTerEdge = this.GetCrossNodeWithTerDiffractionEdge(txFrequencyBand,multiple);//记录射线与地形绕射边的交点，绕射等信息
            Node crossWithCity = cityBuilding.GetReflectionNodeWithCity(this.inRay);//记录射线与建筑物交点、与源点距离、所在面
            List<Node> CrossWithCityEdge = cityBuilding.GetDiffractionNodeWithCity(this.inRay, txFrequencyBand, multiple);//记录射线与建筑物绕射边的交点，绕射等信息
            //把所有交点放到一个list中
            List<Node> crossNodes = new List<Node> ();
            if (crossWithTer != null)
            { crossNodes.Add(crossWithTer); }
            if (CrossWithReceive != null)
            { crossNodes.Add(CrossWithReceive); }
            if (crossWithCity != null)
            { crossNodes.Add(crossWithCity); }
            if (CrossWithTerEdge.Count != 0)
            { crossNodes.AddRange(CrossWithTerEdge); }
            if (CrossWithCityEdge.Count != 0)
            { crossNodes.AddRange(CrossWithCityEdge); }
            //
            if (crossNodes.Count >1)//若交点个数大于2，进行排序
            {
                for (int i = 0; i < crossNodes.Count-1; i++)
                {
                    for (int j = 0; j < crossNodes.Count - i -1; j++)
                    {
                        if (crossNodes[j].DistanceToFrontNode > crossNodes[j+1].DistanceToFrontNode)
                        {
                            Node param = crossNodes[j];
                            crossNodes[j] = crossNodes[j + 1];
                            crossNodes[j + 1] = param;
                        }
                    }
                }
            }
    //        this.DeleteSameNode(crossNodes);
            return crossNodes;
        }

        /// <summary>
        /// 获得射线与地形绕射棱交点、与源点距离、所在面的信息
        /// </summary>
        /// <param name="rays">射线</param>
        /// <param name="terShadowRects">射线所经过的地形矩形</param>
        /// <param name="txFrequencyBand">发射机频带信息</param>
        /// <param name="multiple">绕射半径系数</param>
        /// <returns>返回射线与地形绕射棱交点、与源点距离、所在面的信息</returns>
        protected List<Node> GetCrossNodeWithTerDiffractionEdge( List<FrequencyBand> txFrequencyBand, double multiple)
        {
            List<Node> crossNodes = new List<Node>();
            //获取最大波长
            double maxWaveLength = 300.0 / TxFileProceed.GetMinFrequenceFromList(txFrequencyBand);//300为光速
            //获取最小波长
            double minWaveLength = 300.0 / TxFileProceed.GetMaxFrequenceFromList(txFrequencyBand);//300为光速
            for (int i = 0; i < this.terShadowRects.Count; i++)//对经过的每个地形矩形进行遍历
            {
                for (int j = 0; j < this.terShadowRects[i].RectTriangles.Count; j++)//对每个矩形的三角面进行遍历
                {
                    for (int k = 0; k < this.terShadowRects[i].RectTriangles[j].Lines.Count; k++)//对每个三角面的边进行遍历
                    {
                        if (!this.terShadowRects[i].RectTriangles[j].Lines[k].IsDiffractionEdge)//若该条边是一条绕射棱
                        {
                            continue;
                        }
                        else
                        {
                            if (this.terShadowRects[i].RectTriangles[j].Lines[k].HaveTraced)//若该条绕射棱已经被追踪
                            {
                                this.terShadowRects[i].RectTriangles[j].Lines[k].HaveTraced = false;
                                continue;
                            }
                            else
                            {
                                this.terShadowRects[i].RectTriangles[j].Lines[k].HaveTraced = true;
                                //判断凹凸性
                                if (this.inRay.Origin.JudgeIsConcaveOrConvexToViewPoint(this.terShadowRects[i].RectTriangles[j].Lines[k].AdjacentTriangles[0], this.terShadowRects[i].RectTriangles[j].Lines[k].AdjacentTriangles[1]))
                                {
                                    Node paramNode = this.inRay.GetCrossNodeWithCylinder(this.terShadowRects[i].RectTriangles[j].Lines[k], multiple * maxWaveLength);
                                    if (paramNode != null)//若存在交点
                                    {
                                        //当发射点与绕射棱在一个三角面内,或者发射点在绕射圆柱体内时，该绕射点不算
                                        if (this.terShadowRects[i].RectTriangles[j].Lines[k].getDistanceP2Line(this.inRay.Origin) <= multiple * maxWaveLength ||
                                            this.terShadowRects[i].RectTriangles[j].Lines[k].AdjacentTriangles[0].JudgeIfPointInFace(this.inRay.Origin) || 
                                            this.terShadowRects[i].RectTriangles[j].Lines[k].AdjacentTriangles[1].JudgeIfPointInFace(this.inRay.Origin))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            crossNodes.Add(paramNode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.HandleDiffractionCrossNode(crossNodes,minWaveLength);
            return crossNodes;
        }

        /// <summary>
        ///处理绕射相交点
        /// </summary>
        /// <param name="crossNodes">绕射相交点List</param>
        /// <param name="minWaveLength">最小波长</param>
        /// <returns>处理后的绕射相交点</returns>
        private void HandleDiffractionCrossNode(List<Node> crossNodes, double minWaveLength)
        {
            if (crossNodes.Count <= 1)
            { return; }
            else
            {
                //若交点个数大于两个,则按与源点距离的远近进行排序
                for (int m = 0; m < crossNodes.Count - 1; m++)
                {
                    for (int n = 0; n < crossNodes.Count - m - 1; n++)
                    {
                        if (crossNodes[n].DistanceToFrontNode > crossNodes[n + 1].DistanceToFrontNode)
                        {
                            Node param = crossNodes[n];
                            crossNodes[n] = crossNodes[n + 1];
                            crossNodes[n + 1] = param;
                        }
                    }
                }
                for (int p = 1; p < crossNodes.Count; p++)
                {
                    //若改点到绕射棱的距离比上个绕射点大，则必然在上个点发生绕射，该点不可能发生绕射
                    if (crossNodes[p].DisranceToEdge > crossNodes[p - 1].DisranceToEdge)
                    {
                        crossNodes.RemoveAt(p);
                        p--;
                    }
                    //若该点到棱的距离小于可能发生绕射的最小距离，即该点在任何频段下都会发生绕射，不需继续追踪
                    if (crossNodes[p].DisranceToEdge <= minWaveLength)
                    {
                        crossNodes.RemoveRange(p, crossNodes.Count - p);
                        break;
                    }
                }

                //记录多个绕射点时，每个绕射点之前的所有绕射点
                for (int k = 1; k < crossNodes.Count; k++)
                {
                    for (int l = 0; l < k; l++)
                    {
                        crossNodes[k].FrontDiffractionNode.Add(crossNodes[l]);
                    }
                }
            }
        }

        /// <summary>
        ///得到Rx节点,并加入child节点的子节点List中
        /// </summary>
        private  void SetRxChildNode(Node childNode,Node rxNode, ReceiveBall rxBall)
        {
            rxNode.LayNum = childNode.LayNum + 1;
            rxNode.DiffractionNum = childNode.DiffractionNum;
            rxNode.IsReceiver = true;
            rxNode.IsEnd = true;
            rxNode.RxNum = rxBall.RxNum;
            rxNode.RayTracingDistance = childNode.RayTracingDistance;
            rxNode.RayTracingDistance += rxNode.DistanceToFrontNode;
            rxNode.NodeName = rxBall.RxName;
            rxNode.RayIn = this.inRay;
            rxNode.UAN = rxBall.UAN;
            rxNode.Position = rxBall.Receiver;
            childNode.ChildNodes.Add(rxNode);
        }

        /// <summary>
        ///得到ReflectionPoint节点,并加入child节点的子节点List中
        /// </summary>
        private void SetReflectionChildNode( Node fatherNode, Node reNode, Terrain ter, City cityBuilding, ReceiveBall rxBall, double rayBeamAngle, List<FrequencyBand> txFrequencyBand)
        {
            reNode.LayNum = fatherNode.LayNum + 1;
            reNode.DiffractionNum = fatherNode.DiffractionNum;
            reNode.IsReceiver = false;
            reNode.UAN = rxBall.UAN;
            reNode.RayTracingDistance = fatherNode.RayTracingDistance;
            reNode.RayTracingDistance += reNode.DistanceToFrontNode;
            reNode.RayIn = new RayInfo(fatherNode.Position, reNode.Position);
            //当新节点的层数加绕射次数不小于4或者绕射次数大于2，说明该路径已经过三次反射（或两次反射一次绕射）或者两次绕射，舍弃并追踪下一条射线
            //将该节点设为end
            if ((reNode.DiffractionNum >= 2) || ((reNode.LayNum + reNode.DiffractionNum) >= 4))
            {
                reNode.IsEnd = true;
        //        reNode.NodeStyle = NodeStyle.FinalNode;
                fatherNode.ChildNodes.Add(reNode);
            }
            //否则，递归调用该函数继续追踪射线
            else
            {
                reNode.IsEnd = false;
                fatherNode.ChildNodes.Add(reNode);
                RayInfo ReflectionRay = reNode.RayIn.GetReflectionRay(reNode.ReflectionFace, reNode.Position);
                PunctiformPath newPath = new PunctiformPath(ReflectionRay, ter);
                newPath.SetPunctiformRayPathNodes(reNode, ter, rxBall, cityBuilding, rayBeamAngle, txFrequencyBand);
                
            }
        }

        /// <summary>
        ///得到DiffractionPoint节点,并加入child节点的子节点List中
        /// </summary>
        private void GetDiffractionNode(Node fatherNode, Node cylinderCrossNode, Terrain ter, City cityBuilding, ReceiveBall rxBall, double rayBeamAngle, List<FrequencyBand> txFrequencyBand)
        {
            Point diffractionPoint = new RayInfo(cylinderCrossNode.DiffractionEdge.StartPoint, cylinderCrossNode.DiffractionEdge.LineVector).GetFootPointWithSkewLine(this.inRay);
            if (!cylinderCrossNode.DiffractionEdge.JudgeIfPointInLineRange(diffractionPoint))//若绕射点不在棱内
            {
                return;
            }
            else
            {
                List<Point> diffPostions = this.GetDiffractionNodePoistions(diffractionPoint, cylinderCrossNode.DiffractionEdge);
                for (int i = 0; i < diffPostions.Count; i++)
                {
                    Node diffractionNode = new Node();
                    diffractionNode.Position = diffPostions[i];
                    diffractionNode.DiffractionEdge = cylinderCrossNode.DiffractionEdge;
                    diffractionNode.DisranceToEdge = diffractionPoint.GetDistanceToLine(cylinderCrossNode.DiffractionEdge.StartPoint, cylinderCrossNode.DiffractionEdge.LineVector);
                    diffractionNode.NodeStyle = NodeStyle.DiffractionNode;
                    diffractionNode.LayNum = fatherNode.LayNum + 1;
                    diffractionNode.DiffractionNum = fatherNode.DiffractionNum + 1;
                    diffractionNode.IsReceiver = false;
                    diffractionNode.UAN = rxBall.UAN;
                    diffractionNode.DistanceToFrontNode = diffractionNode.Position.GetDistance(fatherNode.Position);
                    diffractionNode.RayTracingDistance = fatherNode.RayTracingDistance;
                    diffractionNode.RayTracingDistance += diffractionNode.DistanceToFrontNode;
                    diffractionNode.RayIn = new RayInfo(fatherNode.Position, diffractionNode.Position);
                    //当新节点的层数加绕射次数不小于4或者绕射次数大于2，说明该路径已经过三次反射（或两次反射一次绕射）或者两次绕射，舍弃并追踪下一条射线
                    //将该节点设为end
                    if ((diffractionNode.DiffractionNum >= 2) || ((diffractionNode.LayNum + diffractionNode.DiffractionNum) >= 4))
                    {
                        diffractionNode.IsEnd = true;
                    }
                    //否则，递归调用该函数继续追踪射线
                    else
                    {
                        diffractionNode.IsEnd = false;
                        fatherNode.ChildNodes.Add(diffractionNode);
                        List<RayInfo> diffractionRays = Rays.GetDiffractionRays(fatherNode.Position, diffractionNode.Position, diffractionNode.DiffractionEdge, 36);//采用新的方法获得绕射射线
                        for (int j = 0; j < diffractionRays.Count; j++)
                        {
                            PunctiformPath newPath = new PunctiformPath(diffractionRays[j], ter);
                            newPath.SetPunctiformRayPathNodes(diffractionNode, ter, rxBall, cityBuilding, rayBeamAngle, txFrequencyBand);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        ///删除到上一节点距离为0的点
        /// </summary>
        protected void DeleteSameNode(List<Node> crossNodes)
        {
            if (crossNodes.Count == 0)
            { return; }
            for (int i = 0; i < crossNodes.Count; i++)
            {
                if (crossNodes[i].DistanceToFrontNode < 0.00000001)
                {
                    crossNodes.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        ///根据绕射点范围和取点距离取多个绕射点
        /// </summary>
        /// <param name="crossPoint">射线与圆柱体的交点</param>
        /// <param name="diffEdge">绕射棱</param>
        /// <returns>在绕射点范围内一定个数的点的list</returns>
        private List<Point> GetDiffractionNodePoistions(Point crossPoint,AdjacentEdge diffEdge)
        {
            Point rightPoint = new RayInfo(crossPoint, diffEdge.EndPoint).GetPointOnRayVector(diffEdge.DiffCylinderRadius);
            Point leftPoint = new RayInfo(crossPoint, diffEdge.StartPoint).GetPointOnRayVector(diffEdge.DiffCylinderRadius);

            List<Point> diffPoints = new List<Point> { crossPoint};
            diffPoints.AddRange(this.GetSamplingPointsInRange(crossPoint, rightPoint, 5));
            diffPoints.AddRange(this.GetSamplingPointsInRange(crossPoint, leftPoint, 5));
            for (int i = diffPoints.Count - 1; i >= 0; i--)
            {
                if (!diffEdge.JudgeIfPointInLineRange(diffPoints[i]) || diffPoints[i].equal(diffEdge.StartPoint) || diffPoints[i].equal(diffEdge.EndPoint))
                { diffPoints.RemoveAt(i); }
            
            }
            return diffPoints;
            
        }

        /// <summary>
        ///根据射线与圆柱体的交点确定绕射点范围
        /// </summary>
        /// <param name="crossPoint">射线与圆柱体的交点</param>
        /// <param name="diffEdge">绕射棱</param>
        /// <returns>射线与圆柱体的交点确定绕射点范围</returns>
        private Point[] GetDiffractionNodeRange(Point crossPoint,AdjacentEdge diffEdge)
        {
            //a,b,c为棱的方向向量，(x0,y0,z0)为棱上的一点，(x1,y1,z1)为crossPoint
            //A=a^2+b^2+c^2
            //B=2a*(x0-x1)+2b*(y0-y1)+2c*(z0-z1)
            //C=r^2-(x1^2+y1^2+z1^2)-(x0^2+y0^2+z0^2)+2*xo*x1+2*y0*y1+2*z0*z1;
            double A = Math.Pow(diffEdge.LineVector.a, 2) + Math.Pow(diffEdge.LineVector.b, 2) + Math.Pow(diffEdge.LineVector.c, 2);
            double B = 2 * diffEdge.LineVector.a * (diffEdge.StartPoint.X - crossPoint.X) + 2 * diffEdge.LineVector.b * (diffEdge.StartPoint.Y - crossPoint.Y) + 2 * diffEdge.LineVector.c * (diffEdge.StartPoint.Z - crossPoint.Z);
            double C = -(Math.Pow(diffEdge.DiffCylinderRadius, 2) - (Math.Pow(diffEdge.StartPoint.X, 2) + Math.Pow(diffEdge.StartPoint.Y, 2) + Math.Pow(diffEdge.StartPoint.Z, 2)) -
                (Math.Pow(crossPoint.X, 2) + Math.Pow(crossPoint.Y, 2) + Math.Pow(crossPoint.Z, 2)) 
                + 2 * diffEdge.StartPoint.X * crossPoint.X + 2 * diffEdge.StartPoint.Y * crossPoint.Y + 2 * diffEdge.StartPoint.Z * crossPoint.Z);
            double test = Math.Pow(B, 2) - 4 * A * C ;
            if (Math.Pow(B, 2) - 4 * A * C < 0.00000001)
            {
                LogFileManager.ObjLog.error("求绕射点范围时出错");
                return null;
            }
            else
            {
                double t1 = (-B + Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
                double t2 = (-B - Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
                return new Point[2]{new Point(diffEdge.LineVector.a*t1+diffEdge.StartPoint.X,diffEdge.LineVector.b*t1+diffEdge.StartPoint.Y,diffEdge.LineVector.c*t1+diffEdge.StartPoint.Z),
                    new Point(diffEdge.LineVector.a*t2+diffEdge.StartPoint.X,diffEdge.LineVector.b*t2+diffEdge.StartPoint.Y,diffEdge.LineVector.c*t2+diffEdge.StartPoint.Z) };
            }
        }

        /// <summary>
        ///在绕射点范围内取一定个数的点
        /// </summary>
        /// <param name="firstPoint">初始点</param>
        /// <param name="lastPoint">终点</param>
        /// <param name="number">采样点个数</param>
        /// <returns>在绕射点范围内一定个数的点的list</returns>
        private List<Point> GetSamplingPointsInRange(Point firstPoint, Point lastPoint, double number)
        {
            List<Point> samplingPoings = new List<Point>();
            SpectVector vector = new SpectVector(firstPoint, lastPoint).GetNormalizationVector();
            double spacing= lastPoint.GetDistance(firstPoint)/number;
            if (lastPoint.GetDistance(firstPoint) / number == 0)
            { number--; }
            for (int i = 1; i < number; i++)
            {
                samplingPoings.Add(new Point(firstPoint.X + vector.a * spacing*i, firstPoint.Y + vector.b * spacing*i, firstPoint.Z + vector.c * spacing*i));
            }
            samplingPoings.Add(lastPoint);
            return samplingPoings;
        }

    }
}
