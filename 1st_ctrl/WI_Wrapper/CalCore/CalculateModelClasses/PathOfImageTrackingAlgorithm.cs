using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    class PathOfImageTrackingAlgorithm
    {
        public List<Node> nodes;
        public PathOfImageTrackingAlgorithm(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        /// <summary>
        /// 从源点到接收点的直射路径
        /// </summary>
        /// <returns></returns>
        private Path GetDirectPath()
        {
            List<Node> nodeOnPath = new List<Node>();
            nodeOnPath.Add(this.nodes[0]);
            nodeOnPath.Add(this.nodes[this.nodes.Count-1]);
            Path directPath = new Path(nodeOnPath);
            return directPath;
        }
        
        /// <summary>
        /// 从所有结点中选出所有含一个结点的路径
        /// </summary>
        /// <returns></returns>
        private List<Path> GetAllPathOfContainOneNode()
        {
            List<Path> paths = new List<Path>();
            for (int i = 1; i < this.nodes.Count-1; i++)
            {
                List<Node> nodeOnPath = new List<Node>();
                nodeOnPath.Add(this.nodes[0]);
                nodeOnPath.Add(this.nodes[i]);
                nodeOnPath.Add(this.nodes[this.nodes.Count-1]);
                Path path = new Path(nodeOnPath);
                paths.Add(path);
            }
            return paths;
        }

        /// 从所有结点中选出所有含两个个结点的路径
        /// </summary>
        /// <returns></returns>
        private List<Path> GetAllPathOfContainTwoNodes()
        {
            List<Path> paths = new List<Path>();
            int i,j;
            for (i = 1; i < this.nodes.Count-1; i++)
            {
                List<Node> nodesOnPath = new List<Node>();
                nodesOnPath.Add(this.nodes[0]);
                nodesOnPath.Add(this.nodes[i]);
                for (j = 1; j < this.nodes.Count-1 ; j++)
                {
                    if (j != i)
                    {
                        nodesOnPath.Add(this.nodes[j]);
                        nodesOnPath.Add(this.nodes[this.nodes.Count - 1]);
                        Path path = new Path(nodesOnPath);
                        paths.Add(path);
                    }
                }
            }
            return paths;
        }

        /// <summary>
        /// 从所有结点中选出所有含三个结点的路径，其中最多含两个绕射点
        /// </summary>
        /// <returns></returns>
        private List<Path> GetAllPathOfContainThreeNodes()
        {
            List<Path> paths = new List<Path>();
            int i, j, k;
            int diffractionNodeNumber = 0;
            for (i = 1; i < this.nodes.Count-1; i++)
            {
                List<Node> nodesOnPath = new List<Node>();
                nodesOnPath.Add(this.nodes[0]);
                if (this.nodes[i].NodeStyle == NodeStyle.DiffractionNode)
                    diffractionNodeNumber++;
                nodesOnPath.Add(this.nodes[i]);
                for (j = 1; j < this.nodes.Count -1; j++)
                {
                    if(j!=i)
                    { 
                        if(this.nodes[j].NodeStyle == NodeStyle.DiffractionNode)
                            diffractionNodeNumber++;
                        nodesOnPath.Add(this.nodes[j]);
                        for (k = 1; k < this.nodes.Count - 1 ; k++)
                       {
                           if (k != i && k != j)
                           {
                               if (diffractionNodeNumber >= 2 && this.nodes[k].NodeStyle == NodeStyle.DiffractionNode)
                               {
                                   continue;
                               }
                               else
                               {
                                   nodesOnPath.Add(this.nodes[k]);
                                   nodesOnPath.Add(this.nodes[this.nodes.Count - 1]);
                                   Path path = new Path(nodesOnPath);
                                   paths.Add(path);
                               }
                           }
                       }
                    }
                }
            }
            return paths;
        }

        /// <summary>
        /// 从所有结点中选出所有含四个结点的路径，其中最多含两个绕射点
        /// </summary>
        /// <returns></returns>
        private List<Path> GetAllPathOfContainFourNodes()
        {
            List<Path> paths = new List<Path>();
            int i, j, k, n;
            int diffractionNodeNumber = 0;
            for (i = 1; i < this.nodes.Count-1; i++)
            {
                List<Node> nodesOnPath = new List<Node>();
                nodesOnPath.Add(this.nodes[0]);
                if (this.nodes[i].NodeStyle == NodeStyle.DiffractionNode)
                    diffractionNodeNumber++;
                nodesOnPath.Add(this.nodes[i]);
                for (j = 1; j < this.nodes.Count-1; j++)
                {
                    if (j != i)
                    {
                        if (this.nodes[j].NodeStyle == NodeStyle.DiffractionNode)
                            diffractionNodeNumber++;
                        nodesOnPath.Add(this.nodes[j]);
                        for (k = 1; k < this.nodes.Count - 1; k++)
                        {
                            if (k != i && k != j)
                            {
                                if (diffractionNodeNumber >= 2 && this.nodes[k].NodeStyle == NodeStyle.DiffractionNode)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (this.nodes[k].NodeStyle == NodeStyle.DiffractionNode)
                                        diffractionNodeNumber++;
                                    nodesOnPath.Add(this.nodes[k]);
                                    for (n = 1; n < this.nodes.Count - 1 ; n++)
                                    {
                                        if (n != k && n != j && n != i)
                                        {
                                            if (diffractionNodeNumber >= 2 && this.nodes[n].NodeStyle == NodeStyle.DiffractionNode)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                nodesOnPath.Add(this.nodes[n]);
                                                nodesOnPath.Add(this.nodes[this.nodes.Count - 1]);
                                                Path path = new Path(nodesOnPath);
                                                paths.Add(path);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return paths;
        }

        public List<Path> GetAllPath()
        {
            List<Path> possiblePaths = new List<Path>();
            List<Path> realPaths=new List<Path>();
            possiblePaths.Add(this.GetDirectPath());
            possiblePaths.AddRange(this.GetAllPathOfContainOneNode());
            possiblePaths.AddRange(this.GetAllPathOfContainTwoNodes());
            possiblePaths.AddRange(this.GetAllPathOfContainThreeNodes());
            possiblePaths.AddRange(this.GetAllPathOfContainFourNodes());
            foreach (Path possiblePath in possiblePaths)
            {
                if (possiblePath.UpdateReversePaths() != null)
                    realPaths.Add(possiblePath.UpdateReversePaths());  
            }
            return realPaths;
        }

    }
}
