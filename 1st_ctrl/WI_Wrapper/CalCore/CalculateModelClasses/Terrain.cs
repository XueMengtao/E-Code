using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace CalculateModelClasses
{
    /// <summary>
    ///地形
    /// </summary>
    public class Terrain
    {
        List<Material> materialList;
        public const double UnitX = 70.54011649840004;
        public const double UnitY = 92.76178115039988;

        //public const double UnitX = 50;
        //public const double UnitY = 50;
        public Rectangle[,] TerRect;
        public double MinX;
        public double MinY;
        public double MaxX;
        public double MaxY;
        public double MinZ;
        public double MaxZ;
        public List<Triangle> terTris;
        public Terrain()
        { }
        public Terrain(string path)
        {
            if (terTris == null)
            {
                terTris = new List<Triangle>();
                materialList = new List<Material>();
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string str = sr.ReadToEnd();
                if (sr.EndOfStream)
                {
                    sr.Close();
                    fs.Close();
                }
                string[] strArray = str.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Equals("begin_<Material>"))
                    {
                        Material temp = 
                            new TerMaterial(strArray[i + 32], Convert.ToDouble(strArray[i + 34]), 
                                Convert.ToDouble(strArray[i + 36]), Convert.ToDouble(strArray[i + 38]), 
                                Convert.ToDouble(strArray[i + 40]));
                        temp.MaterialNum = Convert.ToInt32(strArray[i + 4]);
                        temp.MaterialType = strArray[i + 5];
                        temp.Ambient.Add(Convert.ToDouble(strArray[i + 8]));
                        temp.Ambient.Add(Convert.ToDouble(strArray[i + 9]));
                        temp.Ambient.Add(Convert.ToDouble(strArray[i + 10]));
                        temp.Ambient.Add(Convert.ToDouble(strArray[i + 11]));
                        temp.Diffuse.Add(Convert.ToDouble(strArray[i + 13]));
                        temp.Diffuse.Add(Convert.ToDouble(strArray[i + 14]));
                        temp.Diffuse.Add(Convert.ToDouble(strArray[i + 15]));
                        temp.Diffuse.Add(Convert.ToDouble(strArray[i + 16]));
                        temp.Specular.Add(Convert.ToDouble(strArray[i + 18]));
                        temp.Specular.Add(Convert.ToDouble(strArray[i + 19]));
                        temp.Specular.Add(Convert.ToDouble(strArray[i + 20]));
                        temp.Specular.Add(Convert.ToDouble(strArray[i + 21]));
                        temp.Emission.Add(Convert.ToDouble(strArray[i + 23]));
                        temp.Emission.Add(Convert.ToDouble(strArray[i + 24]));
                        temp.Emission.Add(Convert.ToDouble(strArray[i + 25]));
                        temp.Emission.Add(Convert.ToDouble(strArray[i + 26]));
                        temp.Shininess = Convert.ToDouble(strArray[i + 28]);
                        materialList.Add(temp);
                        i += 40;
                    }
                    if (strArray[i].Equals("begin_<face>"))
                    {
                        Point p1 = new Point(Convert.ToDouble(strArray[i + 5]), Convert.ToDouble(strArray[i + 6]), Convert.ToDouble(strArray[i + 7]));
                        Point p2 = new Point(Convert.ToDouble(strArray[i + 8]), Convert.ToDouble(strArray[i + 9]), Convert.ToDouble(strArray[i + 10]));
                        Point p3 = new Point(Convert.ToDouble(strArray[i + 11]), Convert.ToDouble(strArray[i + 12]), Convert.ToDouble(strArray[i + 13]));
                        Triangle temp = new Triangle(p1, p2, p3);
                        temp.MaterialNum = Convert.ToInt32(strArray[i + 2]);
                        temp.Material = materialList[Convert.ToInt32(strArray[i + 2])];
                        temp.FaceStyle = FaceType.Terrian;
                        terTris.Add(temp);
                        i += 13;
                    }
                }

                //获取地形矩形
                GetTerRect();
                //获取地形绕射棱
                GetTerRectEdge();
            }
        }

        private void GetTerRect()
        {
            MinX = terTris[0].Vertices[0].X;
            MinY = terTris[0].Vertices[0].Y;
            MaxX = terTris[0].Vertices[0].X;
            MaxY = terTris[0].Vertices[0].X; 
            MinZ = terTris[0].Vertices[0].Z;
            MaxZ = terTris[0].Vertices[0].Z;
            foreach (Triangle temp in terTris)
            {
                double minUnitX = temp.Vertices[0].X;
                double minUnitY = temp.Vertices[0].Y;
                double maxUnitX = temp.Vertices[0].X;
                double maxUnitY = temp.Vertices[0].Y;
                double minUnitZ = temp.Vertices[0].Z;
                double maxUnitZ = temp.Vertices[0].Z;
                //关于X的判断
                if (temp.Vertices[1].X < minUnitX)
                { minUnitX = temp.Vertices[1].X; }
                if (temp.Vertices[2].X < minUnitX)
                {    minUnitX = temp.Vertices[2].X;}
                if (temp.Vertices[1].X > maxUnitX)
                {   maxUnitX = temp.Vertices[1].X;}
                if (temp.Vertices[2].X > maxUnitX)
                {    maxUnitX = temp.Vertices[2].X;}
                //关于Y的判断
                if (temp.Vertices[1].Y < minUnitY)
                {   minUnitY = temp.Vertices[1].Y;}
                if (temp.Vertices[2].Y < minUnitY)
                {   minUnitY = temp.Vertices[2].Y;}
                if (temp.Vertices[1].Y > maxUnitY)
                {  maxUnitY = temp.Vertices[1].Y;}
                if (temp.Vertices[2].Y > maxUnitY)
                {   maxUnitY = temp.Vertices[2].Y;}
                //关于Z的判断
                if (temp.Vertices[1].Z < minUnitZ)
                {   minUnitZ = temp.Vertices[1].Z;}
                if (temp.Vertices[2].Z < minUnitZ)
                {   minUnitZ = temp.Vertices[2].Z;}
                if (temp.Vertices[1].Z > maxUnitZ)
                {  maxUnitZ = temp.Vertices[1].Z;}
                if (temp.Vertices[2].Z > maxUnitZ)
                {   maxUnitZ = temp.Vertices[2].Z;}
                temp.MinUnitX = minUnitX;
                temp.MinUnitY = minUnitY;
                temp.MaxUnitX = maxUnitX;
                temp.MaxUnitY = maxUnitY;
                //与地形的XYZ值进行比较
                if (minUnitX < MinX)
                { MinX = minUnitX; }
                if (maxUnitX > MaxX)
                { MaxX = maxUnitX; }
                if (minUnitY < MinY)
                { MinY = minUnitY; }
                if (maxUnitY > MaxY)
                { MaxY = maxUnitY; }
                if (minUnitZ < MinZ)
                { MinZ = minUnitZ; }
                if (maxUnitZ > MaxZ)
                { MaxZ = maxUnitZ; }
                MaxZ += 1000;
            }
            foreach (Triangle temp in terTris)
            {
                temp.FaceID.SecondID = Convert.ToInt32((temp.MinUnitY - MinY) / UnitY);
                temp.FaceID.FirstID = Convert.ToInt32((temp.MinUnitX - MinX) / UnitX);
            }
            TerRect = new Rectangle[terTris[terTris.Count - 1].FaceID.SecondID + 1, terTris[terTris.Count - 1].FaceID.FirstID + 1];
            for (int i = 0; i < terTris[terTris.Count - 1].FaceID.SecondID + 1; i++)
            {
                for (int j = 0; j < terTris[terTris.Count - 1].FaceID.FirstID + 1; j++)
                {
                    TerRect[i, j] = new Rectangle(terTris[i * (terTris[terTris.Count - 1].FaceID.FirstID + 1) * 2 + j * 2], terTris[i * (terTris[terTris.Count - 1].FaceID.FirstID + 1) * 2 + j * 2 + 1]);
                }
            }
        }

        /// <summary>
        ///求地形矩形每条绕射边的信息
        /// </summary>
        private void GetTerRectEdge()
        {
            if (TerRect == null)
            { return; }
            else
            {
                for (int i = 0; i < TerRect.GetLength(0); i++)
                {
                    for (int j = 0; j < TerRect.GetLength(1); j++)
                    {
                        //任意矩形的斜边都能发生绕射
                        TerRect[i, j].BevelLine=new AdjacentEdge(TerRect[i, j].BevelLine, TerRect[i, j].TriangleOne, TerRect[i, j].TriangleTwo);
                        //左下角的一个矩形
                        if (i == 0 && j == 0)
                        {
                            TerRect[i, j].RightLine = new AdjacentEdge(TerRect[i, j].RightLine, TerRect[i, j].TriangleOne, TerRect[i, j + 1].TriangleTwo);
                            TerRect[i, j].TopLine = new AdjacentEdge(TerRect[i, j].TopLine, TerRect[i, j].TriangleTwo, TerRect[i + 1, j].TriangleOne);
                        }
                        //右下角的一个矩形
                        else if (i == 0 && j == TerRect.GetLength(1) - 1)
                        {
                            TerRect[i, j].LeftLine = TerRect[i, j - 1].RightLine;
                            TerRect[i, j].TopLine = new AdjacentEdge(TerRect[i, j].TopLine, TerRect[i, j].TriangleTwo, TerRect[i + 1, j].TriangleOne);
                        }
                        //左上角的一个矩形
                        else if (i == TerRect.GetLength(0) - 1 && j == 0)
                        {
                            TerRect[i, j].BottomLine = TerRect[i - 1, j].TopLine;
                            TerRect[i, j].RightLine = new AdjacentEdge(TerRect[i, j].RightLine, TerRect[i, j].TriangleOne, TerRect[i, j + 1].TriangleTwo);
                        }
                        //右上角的一个矩形
                        else if (i == TerRect.GetLength(0) - 1 && j == TerRect.GetLength(1) - 1)
                        {
                            TerRect[i, j].LeftLine = TerRect[i, j - 1].RightLine;
                            TerRect[i, j].BottomLine = TerRect[i - 1, j].TopLine;
                        }
                        //地形底部一行除了左右两个底角外的矩形
                        else if (i == 0 && j > 0 && j < TerRect.GetLength(1) - 1)
                        {
                            TerRect[i, j].LeftLine = TerRect[i, j - 1].RightLine;
                            TerRect[i, j].RightLine = new AdjacentEdge(TerRect[i, j].RightLine, TerRect[i, j].TriangleOne, TerRect[i, j + 1].TriangleTwo);
                            TerRect[i, j].TopLine = new AdjacentEdge(TerRect[i, j].TopLine, TerRect[i, j].TriangleTwo, TerRect[i + 1, j].TriangleOne);
                        }
                        //地形顶部一行除了左右两个底角外的矩形
                        else if (i == TerRect.GetLength(0) - 1 && j > 0 && j < TerRect.GetLength(1) - 1)
                        {
                            TerRect[i, j].LeftLine = TerRect[i, j - 1].RightLine;
                            TerRect[i, j].BottomLine = TerRect[i - 1, j].TopLine;
                            TerRect[i, j].RightLine = new AdjacentEdge(TerRect[i, j].RightLine, TerRect[i, j].TriangleOne, TerRect[i, j + 1].TriangleTwo);
                        }
                        //地形左部一行除了上下两个底角外的矩形
                        else if (j == 0 && i > 0 && i < TerRect.GetLength(0) - 1)
                        {
                            TerRect[i, j].BottomLine = TerRect[i - 1, j].TopLine;
                            TerRect[i, j].RightLine = new AdjacentEdge(TerRect[i, j].RightLine, TerRect[i, j].TriangleOne, TerRect[i, j + 1].TriangleTwo);
                            TerRect[i, j].TopLine = new AdjacentEdge(TerRect[i, j].TopLine, TerRect[i, j].TriangleTwo, TerRect[i + 1, j].TriangleOne);
                        }
                        //地形右部一行除了上下两个底角外的矩形
                        else if (j == TerRect.GetLength(1) - 1 && i > 0 && i < TerRect.GetLength(0) - 1)
                        {
                            TerRect[i, j].LeftLine = TerRect[i, j - 1].RightLine;
                            TerRect[i, j].BottomLine = TerRect[i - 1, j].TopLine;
                            TerRect[i, j].TopLine = new AdjacentEdge(TerRect[i, j].TopLine, TerRect[i, j].TriangleTwo, TerRect[i + 1, j].TriangleOne);
                        }
                        else
                        {
                            TerRect[i, j].LeftLine = TerRect[i, j - 1].RightLine;
                            TerRect[i, j].BottomLine = TerRect[i - 1, j].TopLine;
                            TerRect[i, j].RightLine = new AdjacentEdge(TerRect[i, j].RightLine, TerRect[i, j].TriangleOne, TerRect[i, j + 1].TriangleTwo);
                            TerRect[i, j].TopLine = new AdjacentEdge(TerRect[i, j].TopLine, TerRect[i, j].TriangleTwo, TerRect[i + 1, j].TriangleOne);
                        }
                        TerRect[i, j].GiveLinesToRectTriangle();
                    }
                }
            }
        }



        /// <summary>
        /// 将射线投影到三角面上，找出射线所经过的矩形
        /// </summary>
        public List<Rectangle> lineRect(RayInfo ray)
        {
            //RayInfo ray = (RayInfo)originRay.Clone();
            if (ray.Origin.X <= MinX || ray.Origin.X >= MaxX || ray.Origin.Y <= MinY || ray.Origin.Y >= MaxY)
            {
                LogFileManager.ObjLog.error("将射线投影到三角面上，找出射线所经过的矩形,输入的射线从地形外或者地形边缘发射");
                return new List<Rectangle>();
            }
            List<Rectangle> tempRect = new List<Rectangle>();
            int length1 = TerRect.GetLength(0);
            int length2 = TerRect.GetLength(1);
            int originRow = Math.Min((int)((ray.Origin.X - MinX) / UnitX), length2 - 1);
            int originLine = Math.Min((int)((ray.Origin.Y - MinY) / UnitY), length1 - 1);
            //if ((originRow < TerRect.GetLength(1) - 1) && (Math.Round(ray.Origin.X, 11) >= TerRect[originLine, originRow + 1].TriangleOne.MinUnitX))
            //{ originRow++; }
            //if ((originLine < TerRect.GetLength(0) - 1) && (Math.Round(ray.Origin.Y, 11) >= TerRect[originLine + 1, originRow].TriangleOne.MinUnitY))
            //{ originLine++; }
            tempRect.Add(TerRect[originLine, originRow]);
            int row = originLine;
            int line = originRow;
            //判断射线的方向
            int i = (ray.RayVector.b > 0 ? 1 : -1);//行变换
            int j = (ray.RayVector.a > 0 ? 1 : -1);//列方向
            double initX = 0;
            double thisPointPositionX = 0;
            double nextPointPositionX = 0;
            int thisLatticeX = 0;
            int nextLatticeX = 0;
            bool notReachBoundary = true;
            bool isIntersection = false;
            //if (Math.Abs(ray.Origin.Y - MinY - Convert.ToInt32(((ray.Origin.Y - MinY) / UnitY)) * UnitY) < 0.000001) { ray.Origin.Y += 1; }
            //先处理射线与横线没有交点的情况
            if (Math.Abs(ray.RayVector.b) < 0.0000001)
            {
                return GetRectWhenParallel(ray, tempRect, length1, length2, row, ref line, j);
            }
            double travelXDistance;
            //处理第一行，得到第一行的矩形
            notReachBoundary = GetFirstRowRect(ray, tempRect, originRow, originLine, row, i, j, out initX, out nextPointPositionX, out thisLatticeX, out nextLatticeX, notReachBoundary, out travelXDistance);
            //处理完第一行之后，改变初始点的位置。
            thisPointPositionX = initX;
            //while循环一行一行往下算,直到越界
            while (notReachBoundary)
            {
                //求下一个点的位置
                nextPointPositionX = thisPointPositionX + j * travelXDistance;
                row += i;
                //如果nextLattice是在特殊点，在交点上，我们需要把交点附近的四个矩形都加上
                if (Math.Abs(nextPointPositionX - (Convert.ToInt32((nextLatticeX - MinX) / UnitX) * UnitX)) < 0.000001)
                {
                    isIntersection = true;
                    nextPointPositionX = nextPointPositionX + j;//接下面的额外添加一个偏外面的那个矩形
                }
                if (row > length1 - 1 || row < 0 || nextPointPositionX > MaxX || nextPointPositionX < MinX)//判断是否越界及如何处理
                {
                    notReachBoundary = false;
                    nextPointPositionX = nextPointPositionX > MaxX ? MaxX - 1 : nextPointPositionX;//这里+-1是为了能够避免double计算所带来的误差，例如：9.999999997和10.000000001
                    nextPointPositionX = nextPointPositionX < MinX ? MinX + 1 : nextPointPositionX;
                    if (row > length1 - 1 || row < 0)
                    {
                        break;
                    }
                }
                //计算下一个点的位置
                nextLatticeX = (int)((nextPointPositionX - MinX) / UnitX);
                //循环添加矩形，补全
                int differenceNum = 0;
                if (j >= 0)
                {
                    differenceNum = nextLatticeX - thisLatticeX > 0 ? nextLatticeX - thisLatticeX : 0;
                }
                else
                { differenceNum = thisLatticeX - nextLatticeX > 0 ? thisLatticeX - nextLatticeX : 0; }
                for (int d = 0; d <= differenceNum; d++)
                {
                    tempRect.Add(TerRect[row, thisLatticeX]);
                    thisLatticeX = thisLatticeX + j;
                }
                if (isIntersection && !notReachBoundary && row > 0 && row < length1 - 1)
                {
                    tempRect.Add(TerRect[row + i, nextLatticeX - j]);
                    isIntersection = false;
                }
                //更新thisLattice和thisPosition
                thisLatticeX = nextLatticeX;
                thisPointPositionX = nextPointPositionX;
            }

            return tempRect;
        }
        //得到第一行的矩形，并且返回有没有到达边界isReachBoundary。
        private bool GetFirstRowRect(RayInfo ray, List<Rectangle> tempRect, int originRow, int originLine, int row, int i, int j, out double initX, out double nextPointPositionX, out int thisLatticeX, out int nextLatticeX, bool notReachBoundary, out double travelXDistance)
        {
            if (ray.RayVector.b > 0)//求得初始的x与第一条横线相交的位置
            {
                initX = (((originLine + i) * UnitY + MinY - Math.Round( ray.Origin.Y,3)) / ray.RayVector.b) * ray.RayVector.a + ray.Origin.X;
                if (initX > MaxX - 0.000001 || initX < MinX + 0.000001) { notReachBoundary = false; }
                initX = initX > MaxX - 0.000001 ? MaxX - 1 : initX;//不让initX超出边界
                initX = initX < MinX + 0.000001 ? MinX + 1 : initX;
            }
            else
            {
                initX = (((originLine * UnitY + MinY) - ray.Origin.Y) / ray.RayVector.b) * ray.RayVector.a + ray.Origin.X;
                if (initX > MaxX - 0.000001 || initX < MinX + 0.000001) { notReachBoundary = false; }
                initX = initX > MaxX - 0.000001 ? MaxX - 1 : initX;
                initX = initX < MinX + 0.000001 ? MinX + 1 : initX;
            }
            //求走固定unityY的距离，x所走的长度。
            double k = UnitY / ray.RayVector.b;
            travelXDistance = Math.Abs(k * ray.RayVector.a);
            //先处理第一行
            nextPointPositionX = initX;
            thisLatticeX = originRow;
            nextLatticeX = (int)((nextPointPositionX - MinX) / UnitX);
            int num = 0;
            if (j >= 0)
            {
                num = nextLatticeX - thisLatticeX > 0 ? nextLatticeX - thisLatticeX : 0;
            }
            else
            { num = thisLatticeX - nextLatticeX > 0 ? thisLatticeX - nextLatticeX : 0; }
            while (num > 0)
            {
                thisLatticeX = thisLatticeX + j;
                tempRect.Add(TerRect[row, thisLatticeX]);
                num--;
            }

            return notReachBoundary;
        }

        private List<Rectangle> GetRectWhenParallel(RayInfo ray, List<Rectangle> tempRect, int length1, int length2, int row, ref int line, int j)
        {
            if (Math.Abs(UnitY * (Convert.ToInt32((ray.Origin.Y - MinY) / UnitY)) - ray.Origin.Y) < 0.000001)//当射线在横线上
            {
                int isUpOrDown = ((int)((ray.Origin.X - MinX) / UnitX)) < Convert.ToInt32((ray.Origin.X - MinX) / UnitX) ? 1 : -1;//判断射线在横线的下方还是上方
                while (line + j >= 0 && line + j <= length1 - 1)
                {
                    line = line + j;
                    tempRect.Add(TerRect[row, line]);
                    if (row != 0 && row != length1 - 1)
                    {
                        tempRect.Add(TerRect[row + isUpOrDown, line]);
                    }
                }
            }
            else
            {

                while (line + j >= 0 && line + j <= length2 - 1)
                {
                    line = line + j;
                    tempRect.Add(TerRect[row, line]);
                }
            }
            return tempRect;
        }

        /// <summary>
        ///求出射线与环境的交点投影到XY平面后所在的矩形
        /// </summary>
        /// <param name="ray">射线</param>
        /// <returns></returns>
        private Rectangle minimizeScale(RayInfo ray)
        {
            double yposition = 0;//初始化
            double zposition = 0;
            double xPos = ray.Origin.X;
            double yPos = ray.Origin.Y;
            ray.RayVector.a = Math.Round(ray.RayVector.a, 11);
            ray.RayVector.b = Math.Round(ray.RayVector.b, 11);
            ray.RayVector.c = Math.Round(ray.RayVector.c, 11);
            if (ray.RayVector.a > 0)// x方向向量为正
            {
                yposition = ray.RayVector.b / ray.RayVector.a * (terTris[terTris.Count - 1].MinUnitX + UnitX - ray.Origin.X) + ray.Origin.Y;//x取最大值时y的取值

                if (yposition >= terTris[0].MinUnitY && yposition <= (terTris[terTris.Count - 1].MinUnitY + UnitY))
                {
                    zposition = ray.RayVector.c / ray.RayVector.a * (terTris[terTris.Count - 1].MinUnitX + UnitX - ray.Origin.X) + ray.Origin.Z;//x取最大值时z的取值)
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和右侧面交点
                    {
                        xPos = terTris[terTris.Count - 1].MinUnitX + UnitX;
                        yPos = yposition;
                    }
                }
                else if (yposition < terTris[0].MinUnitY)
                {
                    zposition = ray.RayVector.c / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.Z;//y取最小值时z的取值
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和后侧面交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                        yPos = terTris[0].MinUnitY;
                    }
                }
                else
                {
                    zposition = ray.RayVector.c / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.Z;//y取最大值时z的取值
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和前侧面交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                        yPos = terTris[terTris.Count - 1].MinUnitY + UnitY;
                    }
                }
            }
            else if (ray.RayVector.a == 0)//x方向向量为零
            {
                if (ray.RayVector.b > 0)
                {
                    zposition = ray.RayVector.c / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.Z;
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和前侧面交点
                    {
                        xPos = ray.Origin.X;
                        yPos = terTris[terTris.Count - 1].MinUnitY + UnitY;
                    }
                }
                else if (ray.RayVector.b < 0)
                {
                    zposition = ray.RayVector.c / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.Z;
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和后侧面交点
                    {
                        xPos = ray.Origin.X;
                        yPos = terTris[0].MinUnitY;
                    }
                }
                else//x,y方向向量均为零，此时和发射点重合
                {
                    xPos = ray.Origin.X;
                    yPos = ray.Origin.Y;
                }
            }
            else//x方向向量为负
            {
                yposition = ray.RayVector.b / ray.RayVector.a * (terTris[0].MinUnitX - ray.Origin.X) + ray.Origin.Y;//x取最小值时y的取值
                if (yposition >= terTris[0].MinUnitY && yposition <= (terTris[terTris.Count - 1].MinUnitY + UnitY))
                {
                    zposition = ray.RayVector.c / ray.RayVector.a * (terTris[0].MinUnitX - ray.Origin.X) + ray.Origin.Z;//x取最小值时z的取值
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和左侧面交点
                    {
                        xPos = terTris[0].MinUnitX;
                        yPos = yposition;
                    }
                }
                else if (yposition < terTris[0].MinUnitY)
                {
                    zposition = ray.RayVector.c / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.Z;//y取最小值时z的取值
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和后侧面交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                        yPos = terTris[0].MinUnitY;
                    }
                }
                else
                {
                    zposition = ray.RayVector.c / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.Z;//y取最大值时z的取值
                    if (zposition > MaxZ)//和顶面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else if (zposition < MinZ)//和底面有交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                        yPos = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                    }
                    else//和前侧面交点
                    {
                        xPos = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                        yPos = terTris[terTris.Count - 1].MinUnitY + UnitY;
                    }
                }
            }
            xPos = Math.Round(xPos, 11);
            yPos = Math.Round(yPos, 11);
            int row = (int)((xPos - terTris[0].MinUnitX) / UnitX);
            int line = (int)((yPos - terTris[0].MinUnitY) / UnitY);
            if (row >= TerRect.GetLength(1))
            { row = TerRect.GetLength(1) - 1; }
            if (line >= TerRect.GetLength(0))
            { line = TerRect.GetLength(0) - 1; }
            if (row < TerRect.GetLength(1) - 1 && xPos >= TerRect[line, row + 1].TriangleOne.MinUnitX)
            { row++; }
            if (line < TerRect.GetLength(0) - 1 && yPos >= TerRect[line + 1, row].TriangleOne.MinUnitY)
            { line++; }
            return TerRect[line, row];
        }


        /// <summary>
        ///求出射线与环境的交点投影到XY平面后所在的矩形的编号
        /// </summary>
        /// <param name="ray">射线</param>
        /// <returns></returns>
        private List<FaceID> GetFaceIDOfEndRectangle(RayInfo ray)
        {
            List<FaceID> rectID = new List<FaceID>();//存放截止矩形的编号，当交点在棱附近或者棱上时，存放多个编号
            double yLimitingValuve = 0;//极限值
            double zLimitingValue = 0;
            double xPosition = ray.Origin.X;//交点的X坐标
            double yPosition = ray.Origin.Y;//交点的Y坐标
            if (ray.RayVector.a > 0)// x方向向量为正
            {
                yLimitingValuve = ray.RayVector.b / ray.RayVector.a * (terTris[terTris.Count - 1].MinUnitX + UnitX - ray.Origin.X) + ray.Origin.Y;//x取最大值时y的取值

                if (yLimitingValuve >= terTris[0].MinUnitY && yLimitingValuve <= (terTris[terTris.Count - 1].MinUnitY + UnitY))
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.a * (terTris[terTris.Count - 1].MinUnitX + UnitX - ray.Origin.X) + ray.Origin.Z;//x取最大值时z的取值
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和右侧面交点
                        {
                            xPosition = terTris[terTris.Count - 1].MinUnitX + UnitX;
                            yPosition = yLimitingValuve;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else
                        {
                            xPosition = terTris[terTris.Count - 1].MinUnitX + UnitX;
                            yPosition = yLimitingValuve;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和右侧面交点
                        {
                            xPosition = terTris[terTris.Count - 1].MinUnitX + UnitX;
                            yPosition = yLimitingValuve;
                        }
                    }
                }
                else if (yLimitingValuve < terTris[0].MinUnitY)
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.Z;//y取最小值时z的取值
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                }
                else
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.Z;//y取最大值时z的取值
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                }
            }
            else if (ray.RayVector.a == 0)//x方向向量为零
            {
                if (ray.RayVector.b > 0)
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.Z;
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                }
                else if (ray.RayVector.b < 0)
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.Z;
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                }
                else//x,y方向向量均为零，此时和发射点重合
                {
                    xPosition = ray.Origin.X;
                    yPosition = ray.Origin.Y;
                }
            }
            else//x方向向量为负
            {
                yLimitingValuve = ray.RayVector.b / ray.RayVector.a * (terTris[0].MinUnitX - ray.Origin.X) + ray.Origin.Y;//x取最小值时y的取值
                if (yLimitingValuve >= terTris[0].MinUnitY && yLimitingValuve <= (terTris[terTris.Count - 1].MinUnitY + UnitY))
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.a * (terTris[0].MinUnitX - ray.Origin.X) + ray.Origin.Z;//x取最小值时z的取值
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和左侧面交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = yLimitingValuve;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else//和左侧面交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = yLimitingValuve;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和左侧面交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = yLimitingValuve;
                        }
                    }
                }
                else if (yLimitingValuve < terTris[0].MinUnitY)
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.Z;//y取最小值时z的取值
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和后侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[0].MinUnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[0].MinUnitY;
                        }
                    }
                }
                else
                {
                    zLimitingValue = ray.RayVector.c / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.Z;//y取最大值时z的取值
                    if (ray.Origin.Z >= MinZ && ray.Origin.Z <= MaxZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                    else if (ray.Origin.Z < MinZ)
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MaxZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                    else
                    {
                        if (zLimitingValue > MaxZ)//和顶面有交点
                        {
                            xPosition = terTris[0].MinUnitX;
                            yPosition = terTris[0].MinUnitY;
                        }
                        else if (zLimitingValue < MinZ)//和底面有交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.X;
                            yPosition = ray.RayVector.b / ray.RayVector.c * (MinZ - ray.Origin.Z) + ray.Origin.Y;
                        }
                        else//和前侧面交点
                        {
                            xPosition = ray.RayVector.a / ray.RayVector.b * (terTris[terTris.Count - 1].MinUnitY + UnitY - ray.Origin.Y) + ray.Origin.X;
                            yPosition = terTris[terTris.Count - 1].MinUnitY + UnitY;
                        }
                    }
                }
            }
            int row = (int)((xPosition - terTris[0].MinUnitX) / UnitX);
            int line = (int)((yPosition - terTris[0].MinUnitY) / UnitY);
            if (row >= TerRect.GetLength(1))
            {
                LogFileManager.ObjLog.error("求射线与环境的交点时，求得的交点投影在XY平面后row值大于最大值");
                row = TerRect.GetLength(1) - 1;
            }
            if (line >= TerRect.GetLength(0))
            {
                LogFileManager.ObjLog.error("求射线与环境的交点时，求得的交点投影在XY平面后line值大于最大值");
                line = TerRect.GetLength(0) - 1;
            }
            if (line < 0 || row < 0 || line >= TerRect.GetLength(0) || row >= TerRect.GetLength(1))
                return rectID;
            rectID.Add(this.TerRect[line, row].RectID);
            //若交点的X坐标大于等于下一矩形的最小X值，说明X值可能在下一个矩形
            if (row < TerRect.GetLength(1) - 1 && xPosition >= TerRect[line, row + 1].TriangleOne.MinUnitX)
            { rectID.Add(this.TerRect[line, row + 1].RectID); }
            //若交点的Y坐标大于等于下一矩形的最小X值，说明Y值在下一个矩形
            if (line < TerRect.GetLength(0) - 1 && yPosition >= TerRect[line + 1, row].TriangleOne.MinUnitY)
            { rectID.Add(this.TerRect[line+1, row].RectID); }
            if (rectID.Count == 3)//说明上述两个判断都成立，交点在矩形右上角处，故把对角的矩形也加进来
            { rectID.Add(this.TerRect[line+1, row+1].RectID); }
            return rectID;
        }


        /// <summary>
        /// 判断平面的编号是否与目标编号相同
        /// </summary>
        /// <param name="viewFaceID">观察矩形的编号</param>
        /// <param name="targetFaceID">截止矩形的编号list</param>
        /// <returns></returns>
        private bool JudgeTheFaceIDIsEqual(FaceID viewFaceID, List<FaceID> endFaceID)
        {
            for (int i = 0; i < endFaceID.Count; i++)
            {
                if (viewFaceID.IsSameID(endFaceID[i]))
                { return true; }
            }
            return false;
        }




        /// <summary>
        /// 输出新的地形
        /// </summary>
        /// <param name="projectPath">.site文件的路径</param>
        /// <param name="TerRect">地形矩形数组</param>
        /// <returns></returns>
        public  void OutPutNewTerrain(string projectPath)
        {
            if (File.Exists(projectPath + "newTer.ter"))
            { return; }
            else
            {
                List<Triangle> nonDivisionTris = new List<Triangle>();
                List<Triangle> subdivisionTris = new List<Triangle>();
                //遍历地形，把没有细分过的三角面放在一个list中，把细分过的三角面的细分三角面list放在另一个List中
                for (int i = 0; i < this.TerRect.GetLength(0); i++)
                {
                    for (int j = 0; j < this.TerRect.GetLength(1); j++)
                    {
                        if (this.TerRect[i, j].TriangleOne.SubdivisionTriangle.Count != 0)//若三角面经过细分
                        {
                            subdivisionTris.AddRange(this.TerRect[i, j].TriangleOne.SubdivisionTriangle);
                        }
                        else//如果三角面不在建筑物内
                        {
                            nonDivisionTris.Add(this.TerRect[i, j].TriangleOne);
                        }
                        if (this.TerRect[i, j].TriangleTwo.SubdivisionTriangle.Count != 0)//若三角面没有经过
                        {
                            subdivisionTris.AddRange(this.TerRect[i, j].TriangleTwo.SubdivisionTriangle);
                        }
                        else//如果三角面不在建筑物内
                        {
                            nonDivisionTris.Add(this.TerRect[i, j].TriangleTwo);
                        }
                    }
                }
                this.OutputNewTerrainFile(projectPath, nonDivisionTris, subdivisionTris);//输出结果
            }
        }

        /// <summary>
        /// 输出新地形的文件
        /// </summary>
        /// <param name="projectPath">工程的路径</param>
        /// <param name="nonDivisionTris">原地形三角面</param>
        /// <param name="subdivisionTris">建筑物三角面</param>
        /// <returns></returns>
        private  void OutputNewTerrainFile(string projectPath, List<Triangle> nonDivisionTris, List<Triangle> subdivisionTris)
        {
            StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(projectPath + "newTer" + ".ter", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //地形文件前段字符
            sb.AppendLine("Format type:keyword version: 1.1.0");
            sb.AppendLine("begin_<terrain> Untitled Terrain");
            sb.AppendLine("SmoothRender No");
            sb.AppendLine("SmoothEdgeStudyArea 1000000");
            sb.AppendLine("begin_<reference> ");
            sb.AppendLine("cartesian");
            sb.AppendLine("longitude -0.0000000000");
            sb.AppendLine("latitude 0.0000000000");
            sb.AppendLine("visible no");
            sb.AppendLine("sealevel");
            sb.AppendLine("end_<reference>");
            sb.AppendLine("begin_<Material> Wet earth");
            sb.AppendLine("Material 0");
            sb.AppendLine("DielectricHalfspace");
            sb.AppendLine("begin_<Color> ");
            sb.AppendLine("ambient 0.350000 0.600000 0.350000 1.000000");
            sb.AppendLine("diffuse 0.350000 0.600000 0.350000 1.000000");
            sb.AppendLine("specular 0.350000 0.600000 0.350000 1.000000");
            sb.AppendLine("emission 0.000000 0.000000 0.000000 0.000000");
            sb.AppendLine("shininess 5.000000");
            sb.AppendLine("end_<Color>");
            sb.AppendLine("begin_<DielectricLayer> Wet earth");
            sb.AppendLine("conductivity 2.000e-002");
            sb.AppendLine("permittivity 25.000000");
            sb.AppendLine("roughness 0.000e+000");
            sb.AppendLine("thickness 0.000e+000");
            sb.AppendLine("end_<DielectricLayer>");
            sb.AppendLine("end_<Material>");
            sb.AppendLine("begin_<structure_group> ");
            sb.AppendLine("begin_<structure> ");
            sb.AppendLine("begin_<sub_structure> ");
            //
            for (int i = 0; i < nonDivisionTris.Count; i++)
            {
                sb.AppendLine("begin_<face>");
                sb.AppendLine("Material 0");
                sb.AppendLine("nVertices 3");
                sb.AppendLine(nonDivisionTris[i].Vertices[0].X.ToString("F10") + " " + nonDivisionTris[i].Vertices[0].Y.ToString("F10") + " " + nonDivisionTris[i].Vertices[0].Z.ToString("F10"));
                sb.AppendLine(nonDivisionTris[i].Vertices[1].X.ToString("F10") + " " + nonDivisionTris[i].Vertices[1].Y.ToString("F10") + " " + nonDivisionTris[i].Vertices[1].Z.ToString("F10"));
                sb.AppendLine(nonDivisionTris[i].Vertices[2].X.ToString("F10") + " " + nonDivisionTris[i].Vertices[2].Y.ToString("F10") + " " + nonDivisionTris[i].Vertices[2].Z.ToString("F10"));
                sb.AppendLine("end_<face>");
            }
            //
            for (int i = 0; i < subdivisionTris.Count; i++)
            {
                sb.AppendLine("begin_<face>");
                sb.AppendLine("Material 0");
                sb.AppendLine("nVertices 3");
                sb.AppendLine(subdivisionTris[i].Vertices[0].X.ToString("F10") + " " + subdivisionTris[i].Vertices[0].Y.ToString("F10") + " " + subdivisionTris[i].Vertices[0].Z.ToString("F10"));
                sb.AppendLine(subdivisionTris[i].Vertices[1].X.ToString("F10") + " " + subdivisionTris[i].Vertices[1].Y.ToString("F10") + " " + subdivisionTris[i].Vertices[1].Z.ToString("F10"));
                sb.AppendLine(subdivisionTris[i].Vertices[2].X.ToString("F10") + " " + subdivisionTris[i].Vertices[2].Y.ToString("F10") + " " + subdivisionTris[i].Vertices[2].Z.ToString("F10"));
                sb.AppendLine("end_<face>");
            }
            sb.AppendLine("end_<sub_structure>");
            sb.AppendLine("end_<structure>");
            sb.AppendLine("end_<structure_group>");
            sb.AppendLine("end_<terrain>");
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }


        /// <summary>
        /// 获取double值小数点后的位数
        /// </summary>
        /// <param name="data">.site文件的路径</param>
        /// <returns>double值小数点后的位数</returns>
        private int GetBitAfterDecimalPointInDouble(double data)
        {
            string dataString = data.ToString();
            return dataString.Length - dataString.IndexOf(".") - 1;
        }


        /// <summary>
        /// 输出新地形的文件
        /// </summary>
        /// <param name="projectPath">工程的路径</param>
        /// <param name="nonDivisionTris">原地形三角面</param>
        /// <param name="subdivisionTris">建筑物三角面</param>
        /// <returns></returns>
        public static void OutputTriangles(string projectPath, List<Triangle> nonDivisionTris, List<Triangle> subdivisionTris)
        {
            StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(projectPath + "newTer" + ".ter", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //地形文件前段字符
            sb.AppendLine("Format type:keyword version: 1.1.0");
            sb.AppendLine("begin_<terrain> Untitled Terrain");
            sb.AppendLine("SmoothRender No");
            sb.AppendLine("SmoothEdgeStudyArea 1000000");
            sb.AppendLine("begin_<reference> ");
            sb.AppendLine("cartesian");
            sb.AppendLine("longitude -0.0000000000");
            sb.AppendLine("latitude 0.0000000000");
            sb.AppendLine("visible no");
            sb.AppendLine("sealevel");
            sb.AppendLine("end_<reference>");
            sb.AppendLine("begin_<Material> Wet earth");
            sb.AppendLine("Material 0");
            sb.AppendLine("DielectricHalfspace");
            sb.AppendLine("begin_<Color> ");
            sb.AppendLine("ambient 0.350000 0.600000 0.350000 1.000000");
            sb.AppendLine("diffuse 0.350000 0.600000 0.350000 1.000000");
            sb.AppendLine("specular 0.350000 0.600000 0.350000 1.000000");
            sb.AppendLine("emission 0.000000 0.000000 0.000000 0.000000");
            sb.AppendLine("shininess 5.000000");
            sb.AppendLine("end_<Color>");
            sb.AppendLine("begin_<DielectricLayer> Wet earth");
            sb.AppendLine("conductivity 2.000e-002");
            sb.AppendLine("permittivity 25.000000");
            sb.AppendLine("roughness 0.000e+000");
            sb.AppendLine("thickness 0.000e+000");
            sb.AppendLine("end_<DielectricLayer>");
            sb.AppendLine("end_<Material>");
            sb.AppendLine("begin_<structure_group> ");
            sb.AppendLine("begin_<structure> ");
            sb.AppendLine("begin_<sub_structure> ");
            //
            for (int i = 0; i < nonDivisionTris.Count; i++)
            {
                sb.AppendLine("begin_<face>");
                sb.AppendLine("Material 0");
                sb.AppendLine("nVertices 3");
                sb.AppendLine(nonDivisionTris[i].Vertices[0].X.ToString("F10") + " " + nonDivisionTris[i].Vertices[0].Y.ToString("F10") + " " + nonDivisionTris[i].Vertices[0].Z.ToString("F10"));
                sb.AppendLine(nonDivisionTris[i].Vertices[1].X.ToString("F10") + " " + nonDivisionTris[i].Vertices[1].Y.ToString("F10") + " " + nonDivisionTris[i].Vertices[1].Z.ToString("F10"));
                sb.AppendLine(nonDivisionTris[i].Vertices[2].X.ToString("F10") + " " + nonDivisionTris[i].Vertices[2].Y.ToString("F10") + " " + nonDivisionTris[i].Vertices[2].Z.ToString("F10"));
                sb.AppendLine("end_<face>");
            }
            //
            for (int i = 0; i < subdivisionTris.Count; i++)
            {
                sb.AppendLine("begin_<face>");
                sb.AppendLine("Material 0");
                sb.AppendLine("nVertices 3");
                sb.AppendLine(subdivisionTris[i].Vertices[0].X.ToString("F10") + " " + subdivisionTris[i].Vertices[0].Y.ToString("F10") + " " + subdivisionTris[i].Vertices[0].Z.ToString("F10"));
                sb.AppendLine(subdivisionTris[i].Vertices[1].X.ToString("F10") + " " + subdivisionTris[i].Vertices[1].Y.ToString("F10") + " " + subdivisionTris[i].Vertices[1].Z.ToString("F10"));
                sb.AppendLine(subdivisionTris[i].Vertices[2].X.ToString("F10") + " " + subdivisionTris[i].Vertices[2].Y.ToString("F10") + " " + subdivisionTris[i].Vertices[2].Z.ToString("F10"));
                sb.AppendLine("end_<face>");
            }
            sb.AppendLine("end_<sub_structure>");
            sb.AppendLine("end_<structure>");
            sb.AppendLine("end_<structure_group>");
            sb.AppendLine("end_<terrain>");
            sw.Write(sb);
            sb.Clear();
            sw.Close();
            fs.Close();
        }
    }

}
