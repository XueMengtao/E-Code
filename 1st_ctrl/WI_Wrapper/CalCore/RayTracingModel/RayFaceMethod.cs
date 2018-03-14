using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using LogFileManager;
using CityAndFloor;

namespace RayCalInfo
{
    class RayFaceMethod
    {
        /// <summary>
        /// 追踪设置路径节点
        /// </summary>
        private void SetPathNode(Node fatherNode, List<IRayTracing> rayModels, Terrain ter, ReceiveBall rxBall, City buildings, double rayBeamAngle, List<FrequencyBand> txFrequencyBand)
        {
            for (int i = 0; i < rayModels.Count; i++)
            {
                //设置接受球半径
                double rxBallDistance = fatherNode.Position.GetDistance(rxBall.Receiver) + fatherNode.RayTracingDistance;
                rxBall.Radius = rxBallDistance * rayBeamAngle / Math.Sqrt(3);
                List<Node> crossNodes = rayModels[i].GetCrossNodes(ter, rxBall, buildings, rayBeamAngle);
                if (crossNodes.Count == 0)//若射线与各个物体都没有交点
                {
                    if (fatherNode.NodeStyle != NodeStyle.Tx)
                    {
                        fatherNode.IsEnd = true;
                    }
                    return;
                }
                else if (crossNodes.Count == 1)
                {
                    if (crossNodes[0].NodeStyle == NodeStyle.Rx)
                    {
                        fatherNode.ChildNodes.Add(crossNodes[0]);
                    }
                    else
                    {
                        List<IRayTracing> nextRayModel = rayModels[i].GetNextRayModel();
                    }
                    
                }
                else
                {
                    List<IRayTracing> nextRayModel = rayModels[i].GetNextRayModel();

                }
            }

        }

    }
}
