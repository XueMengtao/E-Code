using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using CityAndFloor;
using LogFileManager;

namespace RayCalInfo
{
    /// <summary>
    ///射线追踪接口
    /// </summary>
    public interface IRayTracing
    {
        RayStyle GetRayStyle();
        List<Node> GetCrossNodes(Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle);
        List<IRayTracing> GetNextRayModel();
        void SetChildNode();
    }

    /// <summary>
    ///射线面
    /// </summary>
    public class FaceRay : IRayTracing
    {
        private RayInfo leftRay;
        private RayInfo rightRay;
        private Node leftFatherNode;
        private Node rightFatherNode;
        private Point launchPoint;
        private Face rayFace;
        private List<RayInfo> luanchRay;
        private Dictionary<Node, Node> lineNodes = new Dictionary<Node, Node>();//交点对应的发射点
        private Dictionary<RayInfo, Node> rayCrossNodes = new Dictionary<RayInfo, Node>();

        public FaceRay(Node leftFatherNode, Node rightFatherNode, RayInfo leftRay, RayInfo rightRay)
        {
            this.leftFatherNode = leftFatherNode;
            this.rightFatherNode = rightFatherNode;
            this.leftRay = leftRay;
            this.rightRay = rightRay;
            this.luanchRay = new List<RayInfo> { leftRay, rightRay };
            this.rayFace = new SpaceFace(leftRay.Origin, leftRay.RayVector.CrossMultiplied(rightRay.RayVector));
            this.launchPoint = this.GetFaceRayOriginPoint();
        }

        /// <summary>
        ///获取射线模型类型
        /// </summary>
        public RayStyle GetRayStyle()
        {
            return RayStyle.FaceRay;
        }

        /// <summary>
        ///进行射线追踪
        /// </summary>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <returns>交点</returns>
        public List<Node> GetCrossNodes(Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle)
        {
            //获取左右两条射线的追踪结果
            List<Node> leftCrossNodes = new LineRay(this.leftRay, this.leftFatherNode).GetCrossNodes(ter, rxBall, buildings, rayBeamAngle);
            List<Node> rightCrossNodes = new LineRay(this.rightRay, this.leftFatherNode).GetCrossNodes(ter, rxBall, buildings, rayBeamAngle);
            //把结果放入Dictionary中
            if (leftCrossNodes.Count == 0)
            { rayCrossNodes.Add(this.leftRay, null); }
            else
            { rayCrossNodes.Add(this.leftRay, leftCrossNodes[0]); }

            if (rightCrossNodes.Count == 0)
            { rayCrossNodes.Add(this.rightRay, null); }
            else
            { rayCrossNodes.Add(this.rightRay, rightCrossNodes[0]); }
            //
            HandleCrossNodes(this.luanchRay, this.rayCrossNodes, ter, rxBall, buildings, rayBeamAngle);
            //若射线面打到接收球上，其他交点舍去，不再追踪,同时把空的交点删去
            List<Node> crossNodes = this.rayCrossNodes.Values.ToList();
            for (int i = crossNodes.Count - 1; i >= 0; i--)
            {
                if (crossNodes[i] == null)//交点是null，删去
                {
                    crossNodes.RemoveAt(i);
                    continue;
                }
                if (crossNodes[i].NodeStyle == NodeStyle.Rx)
                { return new List<Node> { crossNodes[i] }; }
            }
            return crossNodes;
        }

        /// <summary>
        ///获取下一次的射线模型
        /// </summary>
        public List<IRayTracing> GetNextRayModel()
        {
            List<IRayTracing> nextRays = new List<IRayTracing>();
            List<Node> crossNodes = this.rayCrossNodes.Values.ToList();
            for (int i = 0; i < crossNodes.Count - 1; i++)
            {
                if (crossNodes[i].NodeStyle == NodeStyle.Rx)
                {
                    //由于目前设置打到接收球后不再对其他射线进行追踪，顾该情况目前不会发生
                }
                else if (crossNodes[i].NodeStyle == NodeStyle.ReflectionNode)//若该点是反射点，与下一节点生成反射射线面
                {
                    nextRays.AddRange(this.GetReflectionModel(crossNodes[i], crossNodes[i + 1]));
                }
                else//绕射点，射成绕射射线面
                {
                    nextRays.AddRange(GetDiffractionModel(crossNodes[i]));
                }
            }
            if (crossNodes[crossNodes.Count - 1].NodeStyle == NodeStyle.DiffractionNode)
            { nextRays.AddRange(GetDiffractionModel(crossNodes[crossNodes.Count - 1])); }
            return nextRays;
        }


        /// <summary>
        ///设置子节点
        /// </summary>
        public void SetChildNode()
        {
            leftFatherNode.ChildNodes.Add(rayCrossNodes[this.leftRay]);
        }


        /// <summary>
        ///求射线面与环境的交点
        /// </summary>
        /// <param name="outRay">从射线面发出的射线</param>
        /// <param name="rayCrossNode">射线与交点的dictionary</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <returns></returns>
        private void HandleCrossNodes(List<RayInfo> outRay, Dictionary<RayInfo, Node> rayCrossNode, Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle)
        {
            for (int i = 0; i < outRay.Count - 1; i++)
            {
                if (rayCrossNode[outRay[i]] != null && rayCrossNode[outRay[i + 1]] != null)//两条射线都有交点
                {
                    this.GetCrossNodeWhenTwoRaysTrue(outRay, rayCrossNode, ter, rxBall, buildings, rayBeamAngle, ref i);
                }
                else if (rayCrossNode[outRay[i]] != null && rayCrossNode[outRay[i + 1]] == null)//一条射线有交点，一个射线无交点
                {
                    this.GetCrossNodeWhenLeftTrueRightFales(outRay, rayCrossNode, ter, rxBall, buildings, rayBeamAngle, ref i);
                }
                else if (rayCrossNode[outRay[i]] == null && rayCrossNode[outRay[i + 1]] != null)//一条射线有交点，一个射线无交点
                {
                    this.GetCrossNodeWhenLeftFalseRightTrue(outRay, rayCrossNode, ter, rxBall, buildings, rayBeamAngle, ref i);
                }
                else//两条射线都没有交点
                {
                    continue;
                }
            }
        }





        /// <summary>
        ///获取反射模型,使用前要先确定两个点在一个面上
        /// </summary>
        ///  <param name="crossNode1">交点1</param>
        /// <param name="crossNode2">交点2</param>
        /// <returns></returns>
        private List<IRayTracing> GetReflectionModel(Node crossNode1, Node crossNode2)
        {

            RayInfo reflectionRay1 = new RayInfo(lineNodes[crossNode1].Position, crossNode1.Position).GetReflectionRay(crossNode1.ReflectionFace, crossNode1.Position);
            RayInfo reflectionRay2 = new RayInfo(lineNodes[crossNode2].Position, crossNode2.Position).GetReflectionRay(crossNode2.ReflectionFace, crossNode2.Position);
            return new List<IRayTracing> { new FaceRay(crossNode1, crossNode2, reflectionRay1, reflectionRay2) };

        }

        /// <summary>
        ///获取绕射模型
        /// </summary>
        ///  <param name="diffractionNode">绕射点</param>
        /// <returns></returns>
        private List<IRayTracing> GetDiffractionModel(Node diffractionNode)
        {
            //绕射范围两个端点的位置
            Point rightPoint = new RayInfo(diffractionNode.Position, diffractionNode.DiffractionEdge.EndPoint).GetPointOnRayVector(diffractionNode.DiffractionEdge.DiffCylinderRadius);
            Point leftPoint = new RayInfo(diffractionNode.Position, diffractionNode.DiffractionEdge.StartPoint).GetPointOnRayVector(diffractionNode.DiffractionEdge.DiffCylinderRadius);
            //将点转成节点
            Node leftNode = this.GetDiffractionNode(leftPoint, diffractionNode);
            Node rightNode = this.GetDiffractionNode(rightPoint, diffractionNode);
            //求出左侧节点的绕射射线
            List<RayInfo> leftRays = Rays.GetDiffractionRays(launchPoint, leftPoint, diffractionNode.DiffractionEdge, 24);
            List<IRayTracing> rayFaces = new List<IRayTracing>();
            //根据左侧绕射射线生成射线模型
            for (int i = 0; i < leftRays.Count; i++)
            {
                Point fatherFacePoint = new SpaceFace(diffractionNode.DiffractionEdge.StartPoint, leftRays[i].RayVector.CrossMultiplied(diffractionNode.DiffractionEdge.LineVector)).GetSubPointInFace(launchPoint);
                Point fatherMirrorPoint = new RayInfo(rightPoint, fatherFacePoint).GetPointOnRayVector(diffractionNode.DiffractionEdge.DiffCylinderRadius);
                RayInfo rightRay = new RayInfo(rightPoint, new SpectVector(fatherMirrorPoint, rightPoint));
                rayFaces.Add(new FaceRay(leftNode, rightNode, leftRays[i], rightRay));
            }
            return rayFaces;

        }

        /// <summary>
        ///获取当左侧射线无交点，右侧射线有交点时的追踪交点
        /// </summary>
        /// <param name="outRay">从射线面发出的射线</param>
        /// <param name="rayCrossNode">射线与交点的dictionary</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <returns></returns>
        private void GetCrossNodeWhenLeftFalseRightTrue(List<RayInfo> outRay, Dictionary<RayInfo, Node> rayCrossNode, Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle, ref int i)
        {
            //先求出两条射线经过的路径的编号个数的公共值
            int minRectNum = (outRay[i].RectID.Count > outRay[i + 1].RectID.Count) ? outRay[i + 1].RectID.Count : outRay[i].RectID.Count;
            bool isDivided = false;
            for (int j = 0; j < minRectNum; j++)//进行遍历，如果有一个矩形不是相同或者相邻的话，说明射线面需要进行细分
            {
                if (!(outRay[i].RectID[j].JudgeIsSameID(outRay[i + 1].RectID[j]) || outRay[i].RectID[j].JudgeIsAdjacent(outRay[i + 1].RectID[j])))
                {
                    isDivided = true;
                    break;
                }
            }
            if (isDivided)//如果需要细分
            {
                this.SetNewMiddleRay(outRay, rayCrossNode, ter, rxBall, buildings, rayBeamAngle, ref i);
            }
            else//如果不需要细分
            {
                if (rayCrossNode[outRay[i]].NodeStyle == NodeStyle.ReflectionNode)
                {
                    if (rayCrossNode[outRay[i]].ReflectionFace.FaceStyle == FaceType.Terrian)
                    { }
                    else
                    { }
                }

            }
        }

        /// <summary>
        ///获取当右侧射线无交点，左侧射线有交点时的追踪交点
        /// </summary>
        /// <param name="outRay">从射线面发出的射线</param>
        /// <param name="rayCrossNode">射线与交点的dictionary</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <returns></returns>
        private void GetCrossNodeWhenLeftTrueRightFales(List<RayInfo> outRay, Dictionary<RayInfo, Node> rayCrossNode, Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle, ref int i)
        {
            int minRectNum = (outRay[i].RectID.Count > outRay[i + 1].RectID.Count) ? outRay[i + 1].RectID.Count : outRay[i].RectID.Count;
            bool isDivided = true;
            for (int j = 0; j < minRectNum; j++)
            {
                if (!(outRay[i].RectID[j].JudgeIsSameID(outRay[i + 1].RectID[j]) || outRay[i].RectID[j].JudgeIsAdjacent(outRay[i + 1].RectID[j])))
                {
                    isDivided = false;
                    break;
                }
            }
            if (isDivided)//如果需要细分
            {
                this.SetNewMiddleRay(outRay, rayCrossNode, ter, rxBall, buildings, rayBeamAngle, ref i);
            }
            else//如果不需要细分
            {
                if (rayCrossNode[outRay[i + 1]].NodeStyle == NodeStyle.ReflectionNode)
                {

                }

            }
        }

        /// <summary>
        ///获取当两侧射线都有交点时的追踪交点
        /// </summary>
        /// <param name="outRay">从射线面发出的射线</param>
        /// <param name="rayCrossNode">射线与交点的dictionary</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <returns></returns>
        private void GetCrossNodeWhenTwoRaysTrue(List<RayInfo> outRay, Dictionary<RayInfo, Node> rayCrossNode, Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle, ref int i)
        {
            //两个交点都是反射点
            if (rayCrossNode[outRay[i]].NodeStyle == NodeStyle.ReflectionNode && rayCrossNode[outRay[i]].NodeStyle == NodeStyle.ReflectionNode)
            {
                //是同一个反射面
                if (rayCrossNode[outRay[i]].ReflectionFace.Equals(rayCrossNode[outRay[i + 1]].ReflectionFace))
                {
                    return;
                }
                else
                {
                    if (rayCrossNode[outRay[i]].ReflectionFace.FaceStyle == FaceType.Terrian && rayCrossNode[outRay[i + 1]].ReflectionFace.FaceStyle == FaceType.Terrian)
                    {
                        if (rayCrossNode[outRay[i]].ReflectionFace.FaceID.JudgeIsSameID(rayCrossNode[outRay[i + 1]].ReflectionFace.FaceID))
                        {
                            AdjacentEdge diffractionEdge = new AdjacentEdge(rayCrossNode[outRay[i]].ReflectionFace, rayCrossNode[outRay[i + 1]].ReflectionFace);
                            Point diffractionPoint = new RayInfo(diffractionEdge.StartPoint, diffractionEdge.EndPoint).GetCrossPointWithFace(this.rayFace);
                            if (diffractionEdge.JudgeIfPointInLineRange(diffractionPoint))//若绕射点在棱上
                            {
                                RayInfo inRay = new RayInfo(this.launchPoint, diffractionPoint);
                                Node lineNode = this.GetLineNode(inRay);
                                Node diffractionNode = this.GetDiffractionNode(lineNode, diffractionPoint, diffractionEdge);
                                rayCrossNode.Add(inRay, diffractionNode);
                                i++;
                            }
                        }
                        else
                        {
                            this.SetNewMiddleRay(outRay, rayCrossNode, ter, rxBall, buildings, rayBeamAngle, ref i);
                        }
                    }
                    else if (rayCrossNode[outRay[i]].ReflectionFace.FaceStyle == FaceType.Terrian && rayCrossNode[outRay[i + 1]].ReflectionFace.FaceStyle == FaceType.Building)
                    { }
                    else if (rayCrossNode[outRay[i]].ReflectionFace.FaceStyle == FaceType.Building && rayCrossNode[outRay[i + 1]].ReflectionFace.FaceStyle == FaceType.Terrian)
                    { }
                    else
                    { }

                }
            }
            //一个交点是反射点，一个交点是绕射点
            else if (rayCrossNode[outRay[i]].NodeStyle == NodeStyle.ReflectionNode && rayCrossNode[outRay[i]].NodeStyle == NodeStyle.DiffractionNode)
            {

            }
            //一个交点是反射点，一个交点是绕射点
            else if (rayCrossNode[outRay[i]].NodeStyle == NodeStyle.DiffractionNode && rayCrossNode[outRay[i]].NodeStyle == NodeStyle.ReflectionNode)
            { }
            else//两个交点都是绕射点
            { }
        }

        /// <summary>
        ///在两条射线中间生成一条新的射线
        /// </summary>
        /// <param name="outRay">从射线面发出的射线</param>
        /// <param name="rayCrossNode">射线与交点的dictionary</param>
        /// <param name="ter">地形</param>
        /// <param name="rxBall">接收球</param>
        ///  <param name="buildings">建筑物</param>
        /// <param name="rayBeamAngle">射线束的夹角</param>
        /// <returns></returns>
        private void SetNewMiddleRay(List<RayInfo> outRay, Dictionary<RayInfo, Node> rayCrossNode, Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle, ref int i)
        {
            Point lineMiddlePoint =rayCrossNode[outRay[i]].Position.GetMiddlePointInTwoPoints(rayCrossNode[outRay[i + 1]].Position);
            RayInfo newRay = new RayInfo(this.launchPoint, lineMiddlePoint);
            Node lineNode = this.GetLineNode(newRay);
            List<Node> newCrossNode = new LineRay(newRay, lineNode).GetCrossNodes(ter, rxBall, buildings, rayBeamAngle);
            if (newCrossNode.Count == 0)
            { rayCrossNode.Add(newRay, null); }
            else
            { 
                rayCrossNode.Add(newRay, newCrossNode[0]);
                this.lineNodes.Add(newCrossNode[0], lineNode);
            }
            outRay.Insert(i + 1, newRay);
            i--;
        }



        /// <summary>
        ///求射线面的公共源点
        /// </summary>
        private Point GetFaceRayOriginPoint()
        {
            return null;
        }

        /// <summary>
        ///得到射线面棱上的节点
        /// </summary>
        private Node GetLineNode(RayInfo newRay)
        {
            Point position = new Point();
            Node lineNode = new Node(this.leftFatherNode);
            lineNode.Position = position;
            return lineNode;
        }


        /// <summary>
        ///得到反射节点
        /// </summary>
        private Node GetReflectionNode(Node fatherNode, Point position, Triangle reflectionFace)
        {
            Node reflectionNode = new Node();
            reflectionNode.Position = position;
            reflectionNode.ReflectionFace = reflectionFace;
            reflectionNode.NodeStyle = NodeStyle.ReflectionNode;
            reflectionNode.LayNum = fatherNode.LayNum + 1;
            reflectionNode.DiffractionNum = fatherNode.DiffractionNum;
            reflectionNode.IsReceiver = false;
            reflectionNode.UAN = fatherNode.UAN;
            reflectionNode.DistanceToFrontNode = reflectionNode.Position.GetDistance(fatherNode.Position);
            reflectionNode.RayTracingDistance = fatherNode.RayTracingDistance;
            reflectionNode.RayTracingDistance += reflectionNode.DistanceToFrontNode;
            reflectionNode.RayIn = new RayInfo(fatherNode.Position, reflectionNode.Position);
            return reflectionNode;
        }

        /// <summary>
        ///得到绕射节点
        /// </summary>
        private Node GetDiffractionNode(Point position, Node cylinderNode)
        {
            Node diffractionNode = new Node();
            diffractionNode.Position = position;
            diffractionNode.DiffractionEdge = cylinderNode.DiffractionEdge;
            diffractionNode.NodeStyle = NodeStyle.DiffractionNode;
            diffractionNode.LayNum = lineNodes[cylinderNode].LayNum + 1;
            diffractionNode.DiffractionNum = lineNodes[cylinderNode].DiffractionNum + 1;
            diffractionNode.IsReceiver = false;
            diffractionNode.UAN = cylinderNode.UAN;
            diffractionNode.DistanceToFrontNode = diffractionNode.Position.GetDistance(lineNodes[cylinderNode].Position);
            diffractionNode.RayTracingDistance = lineNodes[cylinderNode].RayTracingDistance;
            diffractionNode.RayTracingDistance += diffractionNode.DistanceToFrontNode;
            diffractionNode.RayIn = new RayInfo(lineNodes[cylinderNode].Position, diffractionNode.Position);
            return diffractionNode;
        }

        /// <summary>
        ///得到绕射节点
        /// </summary>
        private Node GetDiffractionNode(Node fathterNode, Point position, AdjacentEdge diffractionEdge)
        {
            Node diffractionNode = new Node();
            diffractionNode.Position = position;
            diffractionNode.DiffractionEdge = diffractionEdge;
            diffractionNode.NodeStyle = NodeStyle.DiffractionNode;
            diffractionNode.LayNum = fathterNode.LayNum + 1;
            diffractionNode.DiffractionNum = fathterNode.DiffractionNum + 1;
            diffractionNode.IsReceiver = false;
            diffractionNode.UAN = fathterNode.UAN;
            diffractionNode.DistanceToFrontNode = diffractionNode.Position.GetDistance(fathterNode.Position);
            diffractionNode.RayTracingDistance = fathterNode.RayTracingDistance;
            diffractionNode.RayTracingDistance += diffractionNode.DistanceToFrontNode;
            diffractionNode.RayIn = new RayInfo(fathterNode.Position, diffractionNode.Position);
            return diffractionNode;
        }
    }


    /// <summary>
    ///射线
    /// </summary>
    public class LineRay : IRayTracing
    {
        private RayInfo inRay;
        private List<Node> crossNodes = new List<Node>();
        private Node fatherNode;
        public LineRay()
        { }
        public LineRay(RayInfo ray, Node fatherNode)
        {
            this.inRay = ray;
            this.fatherNode = fatherNode;
        }

        /// <summary>
        ///获取射线模型类型
        /// </summary>
        public RayStyle GetRayStyle()
        {
            return RayStyle.lineRay;
        }

        /// <summary>
        ///进行射线追踪
        /// </summary>
        public List<Node> GetCrossNodes(Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle)
        {
            List<Rectangle> shadowRect = ter.lineRect(this.inRay);//记录射线在俯视图投影经过的矩形
            Node crossWithTer = this.inRay.GetCrossNodeWithTerrainRects(shadowRect);//记录射线与地形交点、与源点距离、所在面
            Node CrossWithReceive = rxBall.GetCrossNodeWithRxBall(this.inRay);//记录射线与接收球交点
            Node CrossWithTerEdge = this.inRay.GetCrossNodeWithTerDiffractionEdge(shadowRect, this.fatherNode.RayTracingDistance, rayBeamAngle);//记录射线与地形绕射边的交点，绕射等信息
            Node crossWithCity = buildings.GetReflectionNodeWithCity(this.inRay);//记录射线与建筑物交点、与源点距离、所在面
            Node CrossWithCityEdge = buildings.GetDiffractionNodeWithBuildings(this.inRay, this.fatherNode.RayTracingDistance, rayBeamAngle);//记录射线与建筑物绕射边的交点，绕射等信息
            //把所有交点放到一个list中
            if (crossWithTer != null)
            { this.crossNodes.Add(crossWithTer); }
            if (CrossWithReceive != null)
            { this.crossNodes.Add(CrossWithReceive); }
            if (crossWithCity != null)
            { this.crossNodes.Add(crossWithCity); }
            if (CrossWithTerEdge != null)
            { this.crossNodes.Add(CrossWithTerEdge); }
            if (CrossWithCityEdge != null)
            { this.crossNodes.Add(CrossWithCityEdge); }
            if (crossNodes.Count == 0)
            {
                return crossNodes;
            }
            else if (crossNodes.Count == 1)
            {
                return new List<Node> { this.crossNodes[0] };
            }
            else
            {
                //若交点大于两个，排序
                for (int i = 0; i < this.crossNodes.Count - 1; i++)
                {
                    for (int j = i + 1; j < this.crossNodes.Count; j++)
                    {
                        if (this.crossNodes[j].DistanceToFrontNode > this.crossNodes[j + 1].DistanceToFrontNode)
                        {
                            Node param = this.crossNodes[j];
                            this.crossNodes[j] = this.crossNodes[j + 1];
                            this.crossNodes[j + 1] = param;
                        }
                    }
                }
                return new List<Node> { this.crossNodes[0] };
            }

        }

        /// <summary>
        ///进行射线追踪
        /// </summary>
        public List<IRayTracing> GetNextRayModel()
        {
            if (this.crossNodes[0].NodeStyle == NodeStyle.ReflectionNode)//若第一个交点是反射点
            {
                return this.GetReflectionModel(this.crossNodes[0]);
            }
            else//若第一个交点是绕射点
            {
                List<IRayTracing> nextRay = new List<IRayTracing>();
                if (this.crossNodes.Count == 1)//只有一个绕射点
                {
                    nextRay.AddRange(this.GetDiffractionModel(this.crossNodes[0]));
                }
                if (this.crossNodes[1].NodeStyle == NodeStyle.ReflectionNode)//绕射点后还是绕射点
                {
                    return this.GetDiffractionModel(this.crossNodes[0]);
                }
                return nextRay;
            }
        }

        /// <summary>
        ///设置子节点
        /// </summary>
        public void SetChildNode()
        {
            fatherNode.ChildNodes.Add(crossNodes[0]);
        }


        /// <summary>
        ///获取反射模型
        /// </summary>
        private List<IRayTracing> GetReflectionModel(Node reflectionNode)
        {
            if (reflectionNode.ReflectionFace == null)
            {
                LogFileManager.ObjLog.error("求反射射线时没有得到反射面");
                return new List<IRayTracing>();
            }
            else
            {
                List<IRayTracing> reflectionModel = new List<IRayTracing>();
                RayInfo reflectionRay = this.inRay.GetReflectionRay(reflectionNode.ReflectionFace, reflectionNode.Position);
                reflectionModel.Add(new LineRay(reflectionRay, reflectionNode));
                return reflectionModel;
            }
        }


        /// <summary>
        ///获取绕射模型
        /// </summary>
        private List<IRayTracing> GetDiffractionModel(Node diffractionNode)
        {
            Point diffractionPoint = 
                new RayInfo(
                    diffractionNode.DiffractionEdge.StartPoint, 
                    diffractionNode.DiffractionEdge.LineVector
                    ).GetFootPointWithSkewLine(this.inRay);
            if (!diffractionNode.DiffractionEdge.JudgeIfPointInLineRange(diffractionPoint))//若绕射点不在棱内
            {
                return new List<IRayTracing>();
            }
            else
            {
                List<IRayTracing> rayFaces = this.GetDiffractionRayFaces(diffractionPoint, diffractionNode.DiffractionEdge);
                return rayFaces;
            }
        }

        /// <summary>
        ///根据绕射棱两侧的绕射射线生成绕射追踪模型
        /// </summary>
        /// <param name="fatherPoint">父节点</param>
        /// <param name="leftRays">左侧的绕射射线</param>
        /// <param name="rightRays">右侧的绕射射线</param>
        /// <returns>绕射追踪模型</returns>
        private List<IRayTracing> GetDiffractionRayFaces(Point diffractionPoint, AdjacentEdge diffractionEdge)
        {
            //绕射范围两个端点的位置
            Point rightPoint = new RayInfo(diffractionPoint, diffractionEdge.EndPoint).GetPointOnRayVector(diffractionEdge.DiffCylinderRadius);
            Point leftPoint = new RayInfo(diffractionPoint, diffractionEdge.StartPoint).GetPointOnRayVector(diffractionEdge.DiffCylinderRadius);
            //将点转成节点
            Node leftNode = this.GetDiffractionChildNode(this.fatherNode, leftPoint, diffractionEdge);
            Node rightNode = this.GetDiffractionChildNode(this.fatherNode, rightPoint, diffractionEdge);
            //求出左侧节点的绕射射线
            List<RayInfo> leftRays = Rays.GetDiffractionRays(this.fatherNode.Position, leftPoint, diffractionEdge, 24);
            List<IRayTracing> rayFaces = new List<IRayTracing>();
            //根据左侧绕射射线生成射线模型
            for (int i = 0; i < leftRays.Count; i++)
            {
                Point fatherFacePoint = new SpaceFace(diffractionEdge.StartPoint, leftRays[i].RayVector.CrossMultiplied(diffractionEdge.LineVector)).GetSubPointInFace(this.fatherNode.Position);
                Point fatherMirrorPoint = new RayInfo(rightPoint, fatherFacePoint).GetPointOnRayVector(diffractionEdge.DiffCylinderRadius);
                RayInfo rightRay = new RayInfo(rightPoint, new SpectVector(fatherMirrorPoint, rightPoint));
                rayFaces.Add(new FaceRay(leftNode, rightNode, leftRays[i], rightRay));
            }
            return rayFaces;

        }

        /// <summary>
        ///得到绕射节点
        /// </summary>
        private Node GetDiffractionChildNode(Node fatherNode, Point position, AdjacentEdge diffractionEdge)
        {
            Node diffractionNode = new Node();
            diffractionNode.Position = position;
            diffractionNode.DiffractionEdge = diffractionEdge;
            diffractionNode.NodeStyle = NodeStyle.DiffractionNode;
            diffractionNode.LayNum = fatherNode.LayNum + 1;
            diffractionNode.DiffractionNum = fatherNode.DiffractionNum + 1;
            diffractionNode.IsReceiver = false;
            diffractionNode.UAN = fatherNode.UAN;
            diffractionNode.DistanceToFrontNode = diffractionNode.Position.GetDistance(fatherNode.Position);
            diffractionNode.RayTracingDistance = fatherNode.RayTracingDistance;
            diffractionNode.RayTracingDistance += diffractionNode.DistanceToFrontNode;
            diffractionNode.RayIn = new RayInfo(fatherNode.Position, diffractionNode.Position);
            return diffractionNode;
        }

    }

    public enum RayStyle
    {
        lineRay,
        FaceRay,
    }

}
