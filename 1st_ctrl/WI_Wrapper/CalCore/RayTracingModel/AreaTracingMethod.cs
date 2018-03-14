using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using UanFileProceed;
using FileObject;
using TxRxFileProceed;
using System.Configuration;
using CityAndFloor;
using LogFileManager;

namespace RayCalInfo
{
    class AreaPath:PunctiformPath
    {
        private List<Rectangle> areaTopSideShadowRects;//射线可能经过的态势区域顶面矩形List
        private Quadrangle areaLeftSideCrossQuad;//射线可能经过的态势区域左侧四边形
        private Quadrangle areaRightSideCrossQuad;//射线可能经过的态势区域右侧四边形
        private Quadrangle areaFrontSideCrossQuad;//射线可能经过的态势区域前侧四边形
        private Quadrangle areaBackSideCrossQuad;//射线可能经过的态势区域后侧四边形

        public AreaPath()
        { }



        /// <summary>
        ///态势区域时的追踪方法
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        public void SetAreaRayPathNodes(Node currentNode, Terrain ter, ReceiveArea reArea, City cityBuilding, List<FrequencyBand> txFrequencyBand)
        {

            //求交点
            List<Node> crossNodes = GetCrossPointsWithEnvironment(reArea, cityBuilding, txFrequencyBand, 30);
            if (crossNodes.Count == 0)//若射线与各个物体都没有交点,设为停止节点
            {
                if (currentNode.NodeStyle != NodeStyle.Tx)
                { currentNode.IsEnd = true; }
                return;
            }
            else
            {
                for (int i = 0; i < crossNodes.Count; i++)
                {
                    if (crossNodes[i].NodeStyle == NodeStyle.ReflectionNode)//若为反射点
                    {
                        this.SetReflectionNode(currentNode, crossNodes[i], ter, cityBuilding, reArea, txFrequencyBand);
                        break;
                    }
                    else if (crossNodes[i].NodeStyle == NodeStyle.CylinderCrossNode)//若为绕射点,追踪绕射绕射，并继续追踪下一个点
                    {
                        this.SetDiffractionNode(currentNode, crossNodes[i], ter, cityBuilding, reArea, txFrequencyBand);
                    }
                    else //若为与态势区域的交点
                    {
                        this.SetAreaCrossNode(currentNode, crossNodes[i], reArea);
                        if (crossNodes.Count == 1)
                        { crossNodes[i].IsEnd = true; }
                    }

                }
            }

        }

      
        /// <summary>
        ///求射线与地形,态势区域,建筑物的交点,并将交点按照与源点距离进行排序
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <param name="reArea">态势区域</param>
        /// <param name="ter">地形</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        ///  <param name="multiple">绕射圆柱体半径倍数</param>
        /// <returns>返回交点</returns>
        private List<Node> GetCrossPointsWithEnvironment(ReceiveArea reArea, City cityBuilding, List<FrequencyBand> txFrequencyBand, double multiple)
        {
            Node crossWithTer = this.inRay.GetCrossNodeWithTerrainRects(this.terShadowRects);//记录射线与地形交点、与源点距离、所在面
            List<Node> CrossWithArea = this.GetCrossNodesOfRayAndArea(reArea);//记录射线与态势区域交点
            List<Node> CrossWithTerEdge = this.GetCrossNodeWithTerDiffractionEdge( txFrequencyBand, multiple);//记录射线与地形绕射边的交点，绕射等信息
            Node crossWithCity = cityBuilding.GetReflectionNodeWithCity(this.inRay);//记录射线与建筑物交点、与源点距离、所在面
            List<Node> CrossWithCityEdge = cityBuilding.GetDiffractionNodeWithCity(this.inRay, txFrequencyBand, multiple);//记录射线与建筑物绕射边的交点，绕射等信息
            //把所有交点放到一个list中
            List<Node> crossNodes = new List<Node>();
            if (crossWithTer != null)
            { crossNodes.Add(crossWithTer); }
            if (CrossWithArea .Count!=0)
            { crossNodes.AddRange(CrossWithArea); }
            if (crossWithCity != null)
            { crossNodes.Add(crossWithCity); }
            if (CrossWithTerEdge.Count != 0)
            { crossNodes.AddRange(CrossWithTerEdge); }
            if (CrossWithCityEdge.Count != 0)
            { crossNodes.AddRange(CrossWithCityEdge); }
            //
            if (crossNodes.Count >1)//若交点个数大于2，进行排序
            {
                for (int i = 0; i < crossNodes.Count - 1; i++)
                {
                    for (int j = 0; j < crossNodes.Count - i - 1; j++)
                    {
                        if (crossNodes[j].DistanceToFrontNode > crossNodes[j + 1].DistanceToFrontNode)
                        {
                            Node param = crossNodes[j];
                            crossNodes[j] = crossNodes[j + 1];
                            crossNodes[j + 1] = param;
                        }
                    }
                }
            }
            this.DeleteSameNodesInArea(crossNodes);

            return crossNodes;
        }

        /// <summary>
        ///得到与态势区域相交节点,并加入child节点的子节点List中
        /// </summary>
        /// <param name="fatherNode">父节点</param>
        /// <param name="areaNode">与态势区域的交点</param>
        /// <param name="reArea">态势区域</param>
        /// <returns></returns>
        private void SetAreaCrossNode(Node fatherNode, Node areaNode, ReceiveArea reArea)
        {
            areaNode.LayNum = fatherNode.LayNum;
            areaNode.DiffractionNum = fatherNode.DiffractionNum;
            areaNode.IsReceiver = true;
            areaNode.IsEnd = false;
            areaNode.RxNum = reArea.RxNum;
            areaNode.RayTracingDistance = fatherNode.RayTracingDistance;
            areaNode.RayTracingDistance += areaNode.DistanceToFrontNode;
            areaNode.NodeName = reArea.RxName;
            areaNode.UAN = reArea.UAN;
            //当之前的交点无与态势区域的交点，则把交点放在父节点的childNodes中，若有，则放在之前交点的childNodes中
            if (fatherNode.tempNode==null)
            {
                fatherNode.ChildNodes.Add(areaNode);
                fatherNode.tempNode = areaNode;
            }
            else
            {
                fatherNode.tempNode.ChildNodes.Add(areaNode);
                fatherNode.tempNode = areaNode;
            }
        }



        /// <summary>
        ///得到ReflectionPoint节点,并加入child节点的子节点List中
        /// </summary>
        /// <param name="fatherNode">父节点</param>
        /// <param name="reNode">反射点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private void SetReflectionNode(Node fatherNode, Node reNode, Terrain ter, City cityBuilding, ReceiveArea reArea, List<FrequencyBand> txFrequencyBand)
        {
            reNode.LayNum = fatherNode.LayNum + 1;
            reNode.DiffractionNum = fatherNode.DiffractionNum;
            reNode.IsReceiver = false;
            reNode.UAN = reArea.UAN;
            reNode.RayTracingDistance = fatherNode.RayTracingDistance;
            reNode.RayIn = new RayInfo(fatherNode.Position, reNode.Position);
            reNode.RayTracingDistance += reNode.DistanceToFrontNode;
            if (fatherNode.tempNode == null)
            {
                fatherNode.ChildNodes.Add(reNode);
            }
            else
            {
          //      reNode.DistanceToFrontNode = reNode.Position.GetDistance(fatherNode.tempNode.Position);
                fatherNode.tempNode.ChildNodes.Add(reNode);
            }
    //        reNode.JudgeIfNodeIsInArea(ter,reArea);
            //当新节点的层数加绕射次数不小于4或者绕射次数大于2，说明该路径已经过三次反射（或两次反射一次绕射）或者两次绕射，舍弃并追踪下一条射线,并将该节点设为end
            //但是当该节点在态势区域内时，则继续追踪
            if (((reNode.DiffractionNum >= 2) || ((reNode.LayNum + reNode.DiffractionNum) >= 4)) && !reNode.IsReceiver)
            {
                reNode.IsEnd = true;
            //    reNode.NodeStyle = NodeStyle.FinalNode;
            }
            //否则，递归调用该函数继续追踪射线
            else
            {
                reNode.IsEnd = false;
                RayInfo reflectionRay = reNode.RayIn.GetReflectionRay(reNode.ReflectionFace,reNode.Position);
                //AreaPath areaPath = new AreaPath(reflectionRay, ter, reArea);
               //areaPath.SetAreaRayPathNodes(reNode, ter, reArea, cityBuilding, txFrequencyBand);
                
            }
        }


        /// <summary>
        ///得到DiffractionPoint节点,并加入child节点的子节点List中
        /// </summary>
        /// <param name="fatherNode">父节点</param>
        /// <param name="cylinderCrossNode">绕射相交点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private void SetDiffractionNode( Node fatherNode, Node cylinderCrossNode, Terrain ter, City cityBuilding, ReceiveArea reArea, List<FrequencyBand> txFrequencyBand)
        {
            Node diffractionNode = new Node();
            diffractionNode.Position = cylinderCrossNode.DiffractionEdge.GetPedalPoint(cylinderCrossNode.Position);
            //若求出的绕射点位置在绕射棱外部或者与绕射棱两个端点重合
            if (!cylinderCrossNode.DiffractionEdge.JudgeIfPointInLineRange(diffractionNode.Position)||
                diffractionNode.Position.equal(cylinderCrossNode.DiffractionEdge.StartPoint)||
                diffractionNode.Position.equal(cylinderCrossNode.DiffractionEdge.EndPoint))
            {
                return ;
            }
            else
            {
                diffractionNode.DiffractionEdge = cylinderCrossNode.DiffractionEdge;
                diffractionNode.DisranceToEdge = cylinderCrossNode.DisranceToEdge;
                diffractionNode.NodeStyle = NodeStyle.DiffractionNode;
                diffractionNode.LayNum = fatherNode.LayNum;
                diffractionNode.DiffractionNum = fatherNode.DiffractionNum + 1;
                diffractionNode.IsReceiver = false;
                diffractionNode.UAN = reArea.UAN;
                diffractionNode.DistanceToFrontNode = diffractionNode.Position.GetDistance(fatherNode.Position);
                diffractionNode.RayTracingDistance = fatherNode.RayTracingDistance;
                diffractionNode.RayTracingDistance += diffractionNode.DistanceToFrontNode;
                diffractionNode.RayIn = new RayInfo(fatherNode.Position, diffractionNode.Position);

                if (fatherNode.tempNode == null)
                {
                    fatherNode.ChildNodes.Add(diffractionNode);
                }
                else
                {
                    //        diffractionNode.DistanceToFrontNode = diffractionNode.Position.GetDistance(fatherNode.tempNode.Position);
                    fatherNode.tempNode.ChildNodes.Add(diffractionNode);
                }
                //         diffractionNode.JudgeIfNodeIsInArea(ter,reArea);
                //当新节点的层数加绕射次数不小于4或者绕射次数大于2，说明该路径已经过三次反射（或两次反射一次绕射）或者两次绕射，舍弃并追踪下一条射线,并将该节点设为end
                //但是当该节点在态势区域内时，则继续追踪
                if (((diffractionNode.DiffractionNum >= 2) || ((diffractionNode.LayNum + diffractionNode.DiffractionNum) >= 4)) && !diffractionNode.IsReceiver)
                {
                    diffractionNode.IsEnd = true;
                }
                //否则继续追踪射线
                else
                {
                    diffractionNode.IsEnd = false;
                    List<RayInfo> DiffractionRays = Rays.GetDiffractionRays(fatherNode.Position, diffractionNode.Position, diffractionNode.DiffractionEdge, 12);
                    for (int i = 0; i < DiffractionRays.Count; i++)
                    {
                        //AreaPath areaPath = new AreaPath(DiffractionRays[i], ter, reArea);
                        //areaPath.SetAreaRayPathNodes(diffractionNode, ter, reArea, cityBuilding, txFrequencyBand);
                    }
                }
            }
        }


        /// <summary>
        ///删除射线与态势区域交点中重复的点
        /// </summary>
        /// <param name="currentNode">节点</param>
        /// <param name="ter">地形</param>
        /// <param name="reArea">态势区域</param>
        ///  <param name="cityBuilding">建筑物</param>
        ///  <param name="TxFrequencyBand">发射机频段信息</param>
        /// <returns></returns>
        private void DeleteSameNodesInArea(List<Node> crossNodes)
        {
            this.DeleteSameNode(crossNodes);
            if (crossNodes.Count == 1)
            { return; }
            else
            {
                for (int i = 1; i < crossNodes.Count; i++)
                {
                    if (crossNodes[i].Position.equal((crossNodes[i - 1]).Position))
                    {
                        //如果射线打在态势区域边界上，与下底面和侧面都有交点，取侧面的交点
                        if (crossNodes[i].NodeStyle == NodeStyle.AreaCrossNode)
                        {
                            crossNodes.RemoveAt(i -1);
                            i--;
                        }
                        else
                        {
                            crossNodes.RemoveAt(i);
                            i--;
                        }
                    }
                }
                return;
            }
        }

        /// <summary>
        ///求射线与接收区域的交点
        /// </summary>
        /// <param name="areaRect">射线可能经过的矩形的范围</param>
        /// <returns>射线与接收区域的交点</returns>
        private  List<Node> GetCrossNodesOfRayAndArea( ReceiveArea recArea)
        {
            List<Node> crossNodes = new List<Node>();
            //与态势区域上顶面的交点
            for (int i = 0; i < this.areaTopSideShadowRects.Count; i++)
            {
                Node crossNode1 = this.inRay.GetCrossNodeWithOriginTriangle(this.areaTopSideShadowRects[i].TriangleOne);
                if (crossNode1 != null)
                { crossNodes.Add(crossNode1); }
                Node crossNode2 = this.inRay.GetCrossNodeWithOriginTriangle(this.areaTopSideShadowRects[i].TriangleTwo);
                if (crossNode2 != null)
                { crossNodes.Add(crossNode2); }
            }
            //与态势区域左侧面的交点
            Node crossNodeInLeftSide = this.inRay.GetCrossNodeWithAreaQuadrangle(this.areaLeftSideCrossQuad);
            if (crossNodeInLeftSide != null)
            { crossNodes.Add(crossNodeInLeftSide); }
            //与态势区域右侧面的交点
            Node crossNodeInRightSide = this.inRay.GetCrossNodeWithAreaQuadrangle(this.areaRightSideCrossQuad);
            if (crossNodeInRightSide != null)
            { crossNodes.Add(crossNodeInRightSide); }
            //与态势区域前侧面的交点
            Node crossNodeInFrontSide = this.inRay.GetCrossNodeWithAreaQuadrangle(this.areaFrontSideCrossQuad);
            if (crossNodeInFrontSide != null)
            { crossNodes.Add(crossNodeInFrontSide); }
            //与态势区域后侧面的交点
            Node crossNodeInBackSide = this.inRay.GetCrossNodeWithAreaQuadrangle(this.areaBackSideCrossQuad);
            if (crossNodeInBackSide != null)
            { crossNodes.Add(crossNodeInBackSide); }

            return crossNodes;

        }
    
        /// <summary>
        /// 求射线在态势区域上顶面可能经过的矩形
        /// </summary>
        /// <param name="recArea">接收区域</param>
        /// <returns>射线在态势区域上顶面可能经过的矩形list</returns>
        private List<Rectangle> GetAreaTopSideShadowRects(ReceiveArea recArea)
        {
            List<Rectangle> areaBottomSideShadowRects = new List<Rectangle>();
            List<Rectangle> areaTopSideShadowRects = new List<Rectangle>();

            for (int i = 0; i < this.terShadowRects.Count; i++)//获取射线在接收区域底面可能经过的矩形list
            {
                //若该矩形在接收区域内
                if (this.terShadowRects[i].RectID.FirstID >= recArea.MinRowNum && this.terShadowRects[i].RectID.FirstID <= recArea.MaxRowNum && this.terShadowRects[i].RectID.SecondID >= recArea.MinLineNum && this.terShadowRects[i].RectID.SecondID <= recArea.MaxLineNum)
                {
                    areaBottomSideShadowRects.Add(this.terShadowRects[i]);
                }
            }

            for (int j = 0; j < areaBottomSideShadowRects.Count; j++)//获取射线在接收区域顶面可能经过的矩形list
            {
                //areaTopSideShadowRects.Add(recArea.TopSideRect[areaBottomSideShadowRects[j].RectID.SecondID - recArea.MinLineNum, areaBottomSideShadowRects[j].RectID.FirstID - recArea.MinRowNum]);
            }

            return areaTopSideShadowRects;
        }

    }
}
