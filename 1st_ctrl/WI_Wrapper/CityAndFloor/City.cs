using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CalculateModelClasses;
using TxRxFileProceed;
using System.Configuration;

namespace CityAndFloor
{
    /// <summary>
    /// 单个建筑物类
    /// </summary>
    public class Building
    {
        protected List<Quadrangle> buildingFace;//存储建筑物的面的属性，包括面的个数、名字、顶点数、以及顶点坐标
        protected int buildingNum;//建筑物的编号
        protected string buildingName;//建筑物名字
        private double floorMinX;//建筑物地面的最小X值
        private double floorMinY;//建筑物地面的最小Y值
        private double floorMaxX;//建筑物地面的最大X值
        private double floorMaxY;//建筑物地面的最大Y值
        private Quadrangle floor;//建筑物底面
        private Quadrangle roofTop;//建筑物顶面
        private List<Quadrangle> flank;//建筑物侧面
        private List<AdjacentEdge> diffractionEdges = new List<AdjacentEdge>();//绕射棱

        public List<Quadrangle> BuildingFace
        {
            set { buildingFace = value; }
            get { return buildingFace; }
        }
        public int BuildingNum
        {
            set { buildingNum = value; }
            get { return buildingNum; }
        }
        public string BuildingName
        {
            set { buildingName = value; }
            get { return buildingName; }
        }
        public double FloorMinX
        { get { return floorMinX; } }
        public double FloorMinY
        { get { return floorMinY; } }
        public double FloorMaxX
        { 
            get { return floorMaxX; }
        }
        public double FloorMaxY
        {
            get { return floorMaxY; }
        }
        public Quadrangle Floor
        {
            get { return floor; } 
        }
        public Quadrangle RoofTop
        { 
            get { return roofTop; } 
        }
        public List<Quadrangle> Flank
        { 
            get { return flank; }
        }
        public List<AdjacentEdge> DffractionEdge
        {
            get { return diffractionEdges; }
        }

        public Building()
        {}


        /// <summary>
        /// 获取建筑物各个面及地面的XY坐标范围
        /// </summary>
        public void GetBuildingFaceAndRangeOfFloor()
        {
            if (buildingFace != null && buildingFace.Count != 0)//建筑物面的List不为空
            {
                int faceCount = 1;
                flank=new List<Quadrangle>();
                for (int i = 0; i < buildingFace.Count; i++)
                {
                    buildingFace[i].FaceID.SecondID = faceCount;
                    faceCount++;
                    if (buildingFace[i].BuildingFaceType == "RoofTop")//若是建筑物顶面
                    { roofTop = buildingFace[i]; }
                    if (buildingFace[i].BuildingFaceType == "Flank")//若是建筑物侧面
                    { flank.Add(buildingFace[i]); }
                    if (buildingFace[i].BuildingFaceType == "Floor")//若是建筑物的底面
                    {
                        floor = buildingFace[i];
                        floorMinX = buildingFace[i].Vertices[0].X;
                        floorMaxX = buildingFace[i].Vertices[0].X;
                        floorMinY = buildingFace[i].Vertices[0].Y;
                        floorMaxY = buildingFace[i].Vertices[0].Y;
                        for (int j = 1; j < buildingFace[i].Vertices.Count; j++)//对建筑物地面的点进行便利遍历
                        {
                            //求最小X值
                            if (floorMinX > buildingFace[i].Vertices[j].X)
                            { floorMinX = buildingFace[i].Vertices[j].X; }
                            //求最小Y值
                            if (floorMinY > buildingFace[i].Vertices[j].Y)
                            { floorMinY = buildingFace[i].Vertices[j].Y; }
                            //求最大X值
                            if (floorMaxX < buildingFace[i].Vertices[j].X)
                            { floorMaxX = buildingFace[i].Vertices[j].X; }
                            //求最大Y值
                            if (floorMaxY < buildingFace[i].Vertices[j].Y)
                            { floorMaxY = buildingFace[i].Vertices[j].Y; }
                        }
                    }
                }
                GetDiffractionEdge();
            }
        }

        /// <summary>
        /// 获取建筑物的绕射棱
        /// </summary>
        private void GetDiffractionEdge()
        {
            //获取顶面与侧面的绕射棱
            for (int i = 0; i < flank.Count; i++)
            {
                AdjacentEdge diffractionEdge = new AdjacentEdge(roofTop, flank[i]);
                if (diffractionEdge.StartPoint != null)
                { this.diffractionEdges.Add(diffractionEdge); }
            }
            //求侧面的绕射棱
            for (int m = 0; m < flank.Count - 1; m++)
            {
                for (int n = m + 1; n < flank.Count; n++)
                {
                    AdjacentEdge diffractionEdge = new AdjacentEdge(flank[m].TriangleOne, flank[n].TriangleOne);
                    if (diffractionEdge.StartPoint != null)
                    { this.diffractionEdges.Add(diffractionEdge); }
                }
            }

        }
    }

    public class FloorStruct
    {
        protected string buildingName;
        private List<Floor> nFloor = new List<Floor>();

        public string BuildingName
        {
            set { buildingName = value; }
            get { return buildingName; }
        }
        public List<Floor> NFloor
        {
            set { nFloor = value; }
            get { return nFloor; }
        }
    }

    public class Floor
    { 
        protected int floorNum;//楼层编号
        private List<Material> materials = new List<Material>(); //楼层材料
        private List<Quadrangle> floorFace = new List<Quadrangle>(); //楼层底面
        private List<Quadrangle> roofTop = new List<Quadrangle>(); //楼层顶面
        private List<Quadrangle> inDoor = new List<Quadrangle>(); //楼层内部侧面
        private List<Quadrangle> outDoor = new List<Quadrangle>(); //楼层外部侧面

        public int FloorNum
        {
            set { floorNum = value; }
            get { return floorNum; }
        }
        public List<Material> Materials
        {
            set { materials = value; }
            get { return materials; }
        }
        public List<Quadrangle> FloorFace
        {
            get { return floorFace; } 
        }
        public List<Quadrangle> RoofTop
        { 
            get { return roofTop; } 
        }
        public List<Quadrangle> InDoor
        { 
            get { return inDoor; }
        }
        public List<Quadrangle> OutDoor
        {
            get { return outDoor; }
        }

        public Floor()
        {}
    }

    /// <summary>
    /// 建筑物群类
    /// </summary>
    public class City
    {
        //材料读取位置常量
        private const int MT = 1;//材料模块中，材料类型距起始处的相对位置
        private const int MN = 3;//材料模块中，材料编号的相对位置
        private const int CA = 7;//由材料起始处跳转到color第一个属性ambient的相对位置
        private const int CC = 4;//color模块中，ambient、diffuse、specular、emission中分别包含的数据个数
        private const int CD = 5;//color模块中，由一个属性第一个数据跳转到下一个属性第一个数据的相对距离
        private const int SL = 3;//shininess与nlayer之间的相对距离
        private const int LL = 2;//层数nlayer与层结构的名称之间相对距离
        private const int DN = 11;//一个Dielectriclayer模块中字符串的总数，用于多层循环读入数据
        private const int DC = 2;//DielectricLayer与conductivity之间的相对距离
        private const int DP = 4;//DielectricLayer与permittivity之间的相对距离
        private const int DR = 6;//DielectricLayer与roughness之间的相对距离
        private const int DT = 8;//DielectricLayer与thickness之间的相对距离
        //建筑物读取位置常量
        private const int CM = 1;//structure_group起始处与cityName间相对距离
        private const int SS = 2;//structure_group起始处与structure起始处相对距离
        private const int BN = 1;//structure中buildingName的相对位置
        private const int FN = 6;//一个建筑物中buildingFace中包含的面的个数
        private const int FNC = 19;//一个face模块中字符串的总数，用于多建筑物面循环读入数据
        private const int FT = 4;//建筑物面类型的相对位置
        private const int FMN = 6;//建筑物面材料对应的编号所在的相对位置
        private const int VN = 8;//建筑物面定点个数的相对位置
        private const int V11 = 9;//顶点1的x坐标相对位置
        private const int V12 = 10;//顶点1的y坐标相对位置
        private const int V13 = 11;//顶点1的z坐标相对位置
        private const int V21 = 12;//顶点2的x坐标相对位置
        private const int V22 = 13;//顶点2的y坐标相对位置
        private const int V23 = 14;//顶点2的z坐标相对位置
        private const int V31 = 15;//顶点3的x坐标相对位置
        private const int V32 = 16;//顶点3的y坐标相对位置
        private const int V33 = 17;//顶点3的z坐标相对位置
        private const int V41 = 18;//顶点4的x坐标相对位置
        private const int V42 = 19;//顶点4的y坐标相对位置
        private const int V43 = 20;//顶点4的z坐标相对位置
        private const int SN = 119;//一个structure模块中字符串的总数，用于多建筑物循环读入数据
        private const int initIndex = -1;//建筑物在列表中索引的初始值
        private const int buildingNameInPath = 3;//建筑物名称在文件路径中的倒数位置
        private const int floorInPath = 2;//建筑物楼层数在文件路径中的倒数位置
        private const int FSS = 1;//structure_group起始处与structure起始处相对距离
        private const int ST = 3;//structure与面类型相对位置
        private const int TM = 3;//面类型与材料编号的相对位置
        private const int TV = 5;//面类型与顶点个数的相对位置
        private const int FV11 = 6;//顶点1的x坐标相对位置
        private const int FV12 = 7;//顶点1的y坐标相对位置
        private const int FV13 = 8;//顶点1的z坐标相对位置
        private const int FV21 = 9;//顶点2的x坐标相对位置
        private const int FV22 = 10;//顶点2的y坐标相对位置
        private const int FV23 = 11;//顶点2的z坐标相对位置
        private const int FV31 = 12;//顶点3的x坐标相对位置
        private const int FV32 = 13;//顶点3的y坐标相对位置
        private const int FV33 = 14;//顶点3的z坐标相对位置
        private const int FV41 = 15;//顶点4的x坐标相对位置
        private const int FV42 = 16;//顶点4的y坐标相对位置
        private const int FV43 = 17;//顶点4的z坐标相对位置
        private const int END = 5;//建筑物文件结束标签的相对位置
        private const string CEILING = "Ceiling";
        private const string FLOOR = "Floor";
        private const string INDOOR = "Indoor";
        private const string OUTDOOT = "Outdoor";

        List<Material> materialList;
        protected List<Building> build;//city中的建筑物群，包括每一个建筑物的属性信息
        protected string cityName;//建筑物群的名字
        protected int buildingNum;//建筑物群中建筑物的个数
        List<FloorStruct> floorsList = new List<FloorStruct>();
        public List<Building> Build
        {
            get { return build; }
        }
        public string CityName
        {
            get { return cityName; }
        }
        public int BuildingNum
        {
            get { return buildingNum; }
        }

        public City() { }

        /// <summary>
        /// 获取.city文件中建筑物群的信息
        /// </summary>
        /// <param name="path">.city文件的存放路径</param>
        public City(string path)
        {
            if (File.Exists(path))
            {
                if (build == null)
                {
                    build = new List<Building>();
                    materialList = new List<Material>();
                    FileStream fs = new FileStream(path, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string str = sr.ReadToEnd();
                    if (sr.EndOfStream)
                    {
                        //sr.Close();
                        //fs.Close();
                        using (sr)
                        { }
                        using (fs)
                        { }
                    }
                    string[] strArray = str.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);//拆分文件内容
                    ReadCityGroup(strArray);
                }
            }
        }

        public void ReadFlpFiles(string flpPath)
        {
            int index = initIndex;
            if (File.Exists(flpPath))
            {
                string[] flpArr = flpPath.Split(new string[] { "\\", "_", "." }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < floorsList.Count; i++)
                {
                    if (floorsList[i].BuildingName == flpArr[flpArr.Length-buildingNameInPath])
                    {
                        foreach (Floor floor in floorsList[i].NFloor)
                        {
                            if (floor.FloorNum == Convert.ToInt32(flpArr[flpArr.Length - floorInPath]))
                                return;
                        }
                        floorsList[i].NFloor.Add(new Floor());
                        index = i;
                        break;
                    }
                }
                if (index == initIndex)
                {
                    floorsList.Add(new FloorStruct());
                    index = floorsList.Count - 1;
                    floorsList[index].BuildingName = flpArr[flpArr.Length - buildingNameInPath];
                    floorsList[index].NFloor.Add(new Floor());
                }
                FileStream fs = new FileStream(flpPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string str = sr.ReadToEnd();
                if (sr.EndOfStream)
                {
                    using (sr)
                    { }
                    using (fs)
                    { }
                }
                string[] strArray = str.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);//拆分文件内容
                floorsList[index].NFloor[floorsList[index].NFloor.Count - 1].FloorNum = Convert.ToInt32(flpArr[flpArr.Length - floorInPath]);
                materialList = new List<Material>();
                ReadFlpGroup(strArray,index);
               
            }
        }

        /// <summary>
        /// 将字符串中的信息读取存入
        /// </summary>
        /// <param name="strArray">从.city文件中提取的字符串数组</param>
        private void ReadCityGroup(string[] strArray)
        {
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Equals("begin_<Material>"))//读取材料信息
                {
                    BuildingMaterial materialtemp = new BuildingMaterial();
                    i = GetMaterial(strArray, i, materialtemp);
                    materialList.Add(materialtemp);
                    i += DT + DN * (materialtemp.LayerNum - 1);//数量减一为编号
                }
                else if (strArray[i].Equals("begin_<structure_group>"))//读取建筑物群信息
                {
                    cityName = strArray[i + CM];
                    build = new List<Building>();
                    i += SS;
                    buildingNum = 0;
                    i = GetBuildings(strArray, i);
                }
            }
        }

        private void ReadFlpGroup(string[] strArray, int index)
        {
            int floorIndex = floorsList[index].NFloor.Count - 1;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Equals("begin_<Material>"))//读取材料信息
                {
                    BuildingMaterial materialtemp = new BuildingMaterial();
                    i = GetMaterial(strArray, i, materialtemp);
                    floorsList[index].NFloor[floorIndex].Materials.Add(materialtemp);
                    i += DT + DN * (materialtemp.LayerNum - 1);//数量减一为编号
                }
                else if (strArray[i].Equals("begin_<structure_group>"))//读取建筑物群信息
                {
                     buildingNum = 0;
                     i = GetFlpFloor(strArray, i += FSS, index);
                }
            }
        }

        private int GetFlpFloor(string[] strArray, int i, int index)
        {
            int offset = ST;
            int floorIndex = floorsList[index].NFloor.Count - 1;
            for (int j = 0; ; j++)//读取建筑物信息
            {
                Quadrangle facetemp = new Quadrangle();
                facetemp.BuildingFaceType = strArray[i + offset];
                if(facetemp.BuildingFaceType == "Ceiling")
                    offset++;
                facetemp.FaceID.FirstID = buildingNum++;
                facetemp.MaterialNum = Convert.ToInt32(strArray[i + TM + offset]);
                facetemp.VerticeCount = Convert.ToInt32(strArray[i + TV + offset]);
                facetemp.Vertices.Add(new Point(Convert.ToDouble(strArray[i + FV11 + offset]), Convert.ToDouble(strArray[i + FV12 + offset]), Convert.ToDouble(strArray[i + FV13 + offset])));
                facetemp.Vertices.Add(new Point(Convert.ToDouble(strArray[i + FV21 + offset]), Convert.ToDouble(strArray[i + FV22 + offset]), Convert.ToDouble(strArray[i + FV23 + offset])));
                facetemp.Vertices.Add(new Point(Convert.ToDouble(strArray[i + FV31 + offset]), Convert.ToDouble(strArray[i + FV32 + offset]), Convert.ToDouble(strArray[i + FV33 + offset])));
                facetemp.Vertices.Add(new Point(Convert.ToDouble(strArray[i + FV41 + offset]), Convert.ToDouble(strArray[i + FV42 + offset]), Convert.ToDouble(strArray[i + FV43 + offset])));
                facetemp.SetLinesAndTriangles();
                switch(facetemp.BuildingFaceType)
                {
                    case CEILING:
                        floorsList[index].NFloor[floorIndex].RoofTop.Add(facetemp);
                        break;
                    case FLOOR:
                        floorsList[index].NFloor[floorIndex].FloorFace.Add(facetemp);
                        break;
                    case INDOOR:
                        floorsList[index].NFloor[floorIndex].InDoor.Add(facetemp);
                        break;
                    case OUTDOOT:
                        floorsList[index].NFloor[floorIndex].OutDoor.Add(facetemp);
                        break;
                    default:
                        break;
                }
                offset += FV43;
                if(strArray[i + offset + END].Equals("end_<floorplan>"))
                {
                    i = i + offset + END;
                    break;
                }
                for(; i + offset < strArray.Length; offset++)
                {
                    if(strArray[i + offset] == "begin_<face>")
                    {
                        offset++;
                        break;
                    }
                }
            }
            return i;
        }

        /// <summary>
        /// 读取建筑物群中所有建筑物的信息
        /// </summary>
        /// <param name="strArray">.city文件拆分成的字符串组</param>
        /// <param name="i">建筑物组信息在字符串组中的位置</param>
        /// <returns>完成建筑物读取后指向字符串组的位置</returns>
        private int GetBuildings(string[] strArray, int i)
        {
            for (int j = 0; ; j++)//读取建筑物信息
            {
                buildingNum++;

                Building buildtemp = new Building();
                buildtemp.BuildingNum = buildingNum;
                buildtemp.BuildingName = strArray[i + BN];
                buildtemp.BuildingFace = new List<Quadrangle>();

                for (int k = 0; k < FN; k++)
                {
                    Quadrangle facetemp = new Quadrangle();
                    facetemp.FaceID.FirstID = buildingNum++;
                    facetemp.BuildingFaceType = strArray[i + FT + FNC * k];
                    facetemp.MaterialNum = Convert.ToInt32(strArray[i + FMN + FNC * k]);
                    facetemp.Material = this.materialList[0];
                    facetemp.VerticeCount = Convert.ToInt32(strArray[i + VN + FNC * k]);
                    facetemp.Vertices.Add(new Point(Convert.ToDouble(strArray[i + V11 + FNC * k]), Convert.ToDouble(strArray[i + V12 + FNC * k]), Convert.ToDouble(strArray[i + V13 + FNC * k])));
                    facetemp.Vertices.Add(new Point(Convert.ToDouble(strArray[i + V21 + FNC * k]), Convert.ToDouble(strArray[i + V22 + FNC * k]), Convert.ToDouble(strArray[i + V23 + FNC * k])));
                    facetemp.Vertices.Add( new Point(Convert.ToDouble(strArray[i + V31 + FNC * k]), Convert.ToDouble(strArray[i + V32 + FNC * k]), Convert.ToDouble(strArray[i + V33 + FNC * k])));
                    facetemp.Vertices.Add( new Point(Convert.ToDouble(strArray[i + V41 + FNC * k]), Convert.ToDouble(strArray[i + V42 + FNC * k]), Convert.ToDouble(strArray[i + V43 + FNC * k])));
                    facetemp.SetLinesAndTriangles();
                    buildtemp.BuildingFace.Add(facetemp);
                }
                build.Add(buildtemp);

                i += SN;
                if (strArray[i].Equals("begin_<structure>"))//判断是否还有未读建筑物
                { continue; }
                else
                { break; }
            }
            SetBuildingEdge();
            return i;
        }

        /// <summary>
        /// 读取建筑物材料信息
        /// </summary>
        /// <param name="strArray">.city文件中提取的字符串组</param>
        /// <param name="i">"begin_<Material>"信息在字符串组中的位置</param>
        /// <param name="materialtemp">存储材料信息的临时实例化对象</param>
        /// <returns>完成材料读取后指向字符串组的位置</returns>
        private static int GetMaterial(string[] strArray, int i, BuildingMaterial materialtemp)
        {
            materialtemp.MaterialType = strArray[i + MT];
            materialtemp.MaterialNum = Convert.ToInt32(strArray[i + MN]);
            i += CA;
            for (int j = 0; j < CC; j++)
            { materialtemp.Ambient.Add(Convert.ToDouble(strArray[i + j])); }
            i += CD;
            for (int j = 0; j < CC; j++)
            { materialtemp.Diffuse.Add(Convert.ToDouble(strArray[i + j])); }
            i += CD;
            for (int j = 0; j < CC; j++)
            { materialtemp.Specular.Add(Convert.ToDouble(strArray[i + j])); }
            i += CD;
            for (int j = 0; j < CC; j++)
            { materialtemp.Emission.Add(Convert.ToDouble(strArray[i + j])); }
            i += CD;
            materialtemp.Shininess = Convert.ToDouble(strArray[i]);
            i += SL;
            materialtemp.LayerNum = Convert.ToInt32(strArray[i]);
            i += LL;
            for (int j = 0; j < materialtemp.LayerNum; j++)
            {
                DielectricLayer layertemp = 
                    new DielectricLayer(strArray[i + (DN * j)], Convert.ToDouble(strArray[i + DC + (DN * j)]),
                        Convert.ToDouble(strArray[i + DP + DN * j]), Convert.ToDouble(strArray[i + DR + DN * j]),
                        Convert.ToDouble(strArray[i + DT + DN * j]));
                materialtemp.DielectricLayer.Add(layertemp);
            }
            return i;
        }


        /// <summary>
        /// 获得射线与建筑物交点、与源点距离、所在面的信息
        /// </summary>
        /// <param name="rays">射线</param>
        /// <returns>返回射线与建筑物交点、与源点距离、所在面的信息</returns>
        public Node GetReflectionNodeWithCity(RayInfo oneRay)
        {
            if (this.Build == null || this.Build.Count == 0)//若不存在建筑物
            { return null; }
            else
            {
                List<Quadrangle> buildingsQuad = new List<Quadrangle>();
                for (int i = 0; i < this.Build.Count; i++)//把所有建筑物的面都提取出来
                {
                    buildingsQuad.Add(this.Build[i].RoofTop);
                    buildingsQuad.AddRange(this.Build[i].Flank);
                }

                List<Node> crossPoints = new List<Node>();
                for (int i = 0; i < buildingsQuad.Count; i++)//求每个三角面的交点
                {
                    Node crossPoint1 = oneRay.GetCrossNodeWithOriginTriangle(buildingsQuad[i].TriangleOne);
                    if (crossPoint1 != null)
                    { crossPoints.Add(crossPoint1); }
                    Node crossPoint2 = oneRay.GetCrossNodeWithOriginTriangle(buildingsQuad[i].TriangleTwo);
                    if (crossPoint2 != null)
                    { crossPoints.Add(crossPoint2); }
                }
                if (crossPoints.Count == 0)//没有交点
                {
                    return null;
                }
                else if (crossPoints.Count == 1)//只有一个交点，返回该交点
                {
                    return crossPoints[0];
                }
                else//大于一个交点,选出最近的点
                {
                    Node nearestPoint = crossPoints[0];
                    for (int j = 1; j < crossPoints.Count; j++)
                    {
                        if (nearestPoint.DistanceToFrontNode > crossPoints[j].DistanceToFrontNode)
                        { nearestPoint = crossPoints[j]; }
                    }
                    return nearestPoint;
                }
            }
        }

        /// <summary>
        /// 获得射线与建筑物绕射棱交点、与源点距离、所在面的信息
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <returns>返回射线与建筑物绕射棱交点、与源点距离、所在面的信息</returns>
        public List<Node> GetDiffractionNodeWithCity(RayInfo oneRay, List<FrequencyBand> txFrequencyBand, double multiple)
        {
            if (this.build == null || this.build.Count == 0)
            {
                return new List<Node>();
            }
            else
            {
                //获取最大波长
                double maxWaveLength = 300.0 / TxFileProceed.GetMinFrequenceFromList(txFrequencyBand);//300为光速
                //获取最小波长
                double minWaveLength = 300.0 / TxFileProceed.GetMaxFrequenceFromList(txFrequencyBand);//300为光速
                List<Node> crossNodes = new List<Node>();
                for (int i = 0; i < this.build.Count; i++)
                {
                    for (int j = 0; j < this.build[i].DffractionEdge.Count; j++)
                    {
                        if (oneRay.Origin.JudgeIsConcaveOrConvexToViewPoint(this.build[i].DffractionEdge[j].AdjacentTriangles[0], this.build[i].DffractionEdge[j].AdjacentTriangles[1]))
                        {
                            Node crossNode = oneRay.GetCrossNodeWithCylinder(this.build[i].DffractionEdge[j], multiple * maxWaveLength);
                            if (crossNode != null)
                            {
                                //当发射点与绕射棱在一个三角面内时，该绕射点不算
                                if (this.build[i].DffractionEdge[j].AdjacentTriangles[0].JudgeIfPointInFace(oneRay.Origin) || this.build[i].DffractionEdge[j].AdjacentTriangles[1].JudgeIfPointInFace(oneRay.Origin))
                                {
                                    continue;
                                }
                                else
                                {
                                    crossNodes.Add(crossNode);
                                }
                            }
                        }
                    }
                }

                return crossNodes;
            }
        }

        /// <summary>
        /// 获得射线与建筑物绕射棱交点、与源点距离、所在面的信息
        /// </summary>
        /// <param name="oneRay">射线</param>
        /// <returns>返回射线与建筑物绕射棱交点、与源点距离、所在面的信息</returns>
        public Node GetDiffractionNodeWithBuildings(RayInfo oneRay, double rayTracingDistance, double rayBeamAngle)
        {
            if (this.build == null || this.build.Count == 0)
            {
                return null;
            }
            else
            {
                List<Node> crossNodes = new List<Node>();
                for (int i = 0; i < this.build.Count; i++)
                {
                    for (int j = 0; j < this.build[i].DffractionEdge.Count; j++)
                    {
                        if (oneRay.Origin.JudgeIsConcaveOrConvexToViewPoint(this.build[i].DffractionEdge[j].AdjacentTriangles[0], this.build[i].DffractionEdge[j].AdjacentTriangles[1]))
                        {
                            double rxBallDistance = oneRay.Origin.GetDistanceToFace(new SpaceFace(this.build[i].DffractionEdge[j].StartPoint,oneRay.RayVector)) + rayTracingDistance;
                            double radius = 0.809 * rxBallDistance * rayBeamAngle / Math.Sqrt(3);
                            Node crossNode = oneRay.GetCrossNodeWithCylinder(this.build[i].DffractionEdge[j],radius);
                            if (crossNode != null)
                            {
                                //当发射点与绕射棱在一个三角面内时，该绕射点不算
                                if (this.build[i].DffractionEdge[j].AdjacentTriangles[0].JudgeIfPointInFace(oneRay.Origin) || this.build[i].DffractionEdge[j].AdjacentTriangles[1].JudgeIfPointInFace(oneRay.Origin))
                                {
                                    continue;
                                }
                                else
                                {
                                    crossNodes.Add(crossNode);
                                }
                            }
                        }
                    }
                }
                if (crossNodes.Count == 0)
                { return null; }
                else if (crossNodes.Count == 1)
                { return crossNodes[0]; }
                else
                {
                    //若交点个数大于两个找出最近的
                    for (int m = 0; m < crossNodes.Count - 1; m++)
                    {
                        if (crossNodes[m].DistanceToFrontNode < crossNodes[m + 1].DistanceToFrontNode)
                        {
                            Node param = crossNodes[m];
                            crossNodes[m] = crossNodes[m + 1];
                            crossNodes[m + 1] = param;
                        }
                    }
                    return crossNodes[crossNodes.Count - 1];
                }
            }

        }

        /// <summary>
        /// 根据建筑物重新构造地形三角面
        /// </summary>
        /// <param name="terRect">地形矩形数组</param>
        /// <returns>返回重构后的地形</returns>
        public void RestructTerrainByBuildings(Rectangle[,] terRect)
        {
            if (build != null && build.Count != 0 && terRect!=null && terRect.Length !=0)//若存在建筑物和地形
            {
                for (int i = 0; i < build.Count; i++)
                {
                    build[i].GetBuildingFaceAndRangeOfFloor();
                    Rectangle[,] buildingRect = this.GetTerRecRangetOfBuilding(terRect, build[i]);//获取建筑物所在地形矩形
                    this.ReconstructBuildingTerRects(buildingRect, build[i].Floor);//根据建筑物底面细分三角面
                }
                //把三角面多次细分后的所有最小三角面放在原三角面的子List中
                for (int i = 0; i < terRect.GetLength(0); i++)
                {
                    for (int j = 0; j < terRect.GetLength(1); j++)
                    {
                        //把三角面多次细分后的所有最小三角面放在原三角面的子List中
                        for (int k = 0; k < terRect[i, j].RectTriangles.Count; k++)
                        {
                            if (terRect[i, j].RectTriangles[k].SubdivisionTriangle.Count != 0)
                            {
                                terRect[i, j].RectTriangles[k].SubdivisionTriangle = this.ExtractNewTerTriangles(terRect[i, j].RectTriangles[k]);
                            }
                        }
                    }
                }
            }
            else
            { return; }
        }

        /// <summary>
        /// 获取建筑物所在的地形矩形数组
        /// </summary>
        /// <param name="terRect">地形矩形数组</param>
        /// <param name="building">建筑物</param>
        /// <returns>返回建筑物所在的矩形的数组</returns>
        private Rectangle[,] GetTerRecRangetOfBuilding(Rectangle[,] terRect, Building building)
        {
            //double unitX = 70.54011649840004;//70是地形三角面X方向的长度
            //double unitY = 92.76178115039988;//90是地形三角面Y方向的长度

            double unitX = 50;//70是地形三角面X方向的长度
            double unitY = 50;//90是地形三角面Y方向的长度

            //求出建筑物所在地形矩形的范围
            int minRow = (int)((building.FloorMinX - terRect[0, 0].TriangleOne.MinUnitX) / unitX);
            int minLine = (int)((building.FloorMinY - terRect[0, 0].TriangleOne.MinUnitY) / unitY);
            int maxRow = (int)((building.FloorMaxX - terRect[0, 0].TriangleOne.MinUnitX) / unitX);
            int maxLine = (int)((building.FloorMaxY - terRect[0, 0].TriangleOne.MinUnitY) / unitY);
            if (maxLine >= terRect.GetLength(0))
            { 
                maxLine = terRect.GetLength(0) - 1;
              
            }
            if (maxRow >= terRect.GetLength(1))
            {
                maxRow = terRect.GetLength(1) - 1;
            }
            //
            Rectangle[,] buildingRect = new Rectangle[maxLine-minLine+1,maxRow-minRow+1];
            for (int i = 0; i < maxLine - minLine + 1; i++)
            {
                for (int j = 0; j < maxRow - minRow + 1; j++)
                {
                    buildingRect[i, j] = terRect[minLine + i, minRow + j];
                }
            }
            return buildingRect;
        }

        /// <summary>
        /// 重构建筑物所在的地形
        /// </summary>
        /// <param name="buildingRect">建筑物所在地形矩形数组</param>
        /// <param name="building">建筑物</param>
        ///   /// <returns>返回重构后的地形</returns>
        private void ReconstructBuildingTerRects(Rectangle[,] buildingRect, Quadrangle floor)
        {
            for (int i = 0; i < buildingRect.GetLength(0); i++)
            {
                for (int j = 0; j < buildingRect.GetLength(1); j++)
                {
                    this.FindLatestTriangleAndRestructIt(buildingRect[i, j].TriangleOne,floor);
                    this.FindLatestTriangleAndRestructIt(buildingRect[i, j].TriangleTwo, floor);
                }
            }
        }

        /// <summary>
        /// 判断该三角面是否经过细分，若没有，根据建筑物细分，若有，对细分后的每个三角面再次进行细分
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <param name="floor">建筑物底面</param>
        /// <returns>返回细分后的三角面</returns>
        private void FindLatestTriangleAndRestructIt(Triangle triangle, Quadrangle floor)
        {
            if (triangle.SubdivisionTriangle.Count!=0)
            {
                for (int i = 0; i < triangle.SubdivisionTriangle.Count; i++)
                {
                    FindLatestTriangleAndRestructIt(triangle.SubdivisionTriangle[i], floor);
                }
            }
            else
            {
                triangle.SubdivisionTriangle = this.RestructSingleTriangleByBuilding(triangle, floor);
            }
        }

        /// <summary>
        /// 提取细分后的三角面，由于有些三角面进行了多次细分，需要递归得到,用之前需要保证原三角面有细分的三角面
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> ExtractNewTerTriangles(Triangle triangle)
        {
            List<Triangle> divisionTri = new List<Triangle>();
            Stack<Triangle> triangleList = new Stack<Triangle>();
            triangleList.Push(triangle);
            while (triangleList.Count > 0)
            {
                Triangle param = triangleList.Pop();
                if (param.SubdivisionTriangle.Count != 0)
                {
                    for (int i = 0; i < param.SubdivisionTriangle.Count; i++)
                    { 
                        triangleList.Push(param.SubdivisionTriangle[i]);
                    }
                }
                else
                {
                    divisionTri.Add(param);
                }
            }
            return divisionTri;

        }

        /// <summary>
        /// 重新构建每个三角面
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <param name="floor">建筑物底面</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> RestructSingleTriangleByBuilding(Triangle triangle, Quadrangle floor)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            List<Point> insidePoint = new List<Point>();//在建筑物底面投影内的点
            List<Point> outsidePoint = new List<Point>();//在建筑物底面投影外的点
            bool point1InTri = floor.JudeIfPointInQuadrangleInXY(triangle.Vertices[0]);//点X是否在底面投影内
            bool point2InTri = floor.JudeIfPointInQuadrangleInXY(triangle.Vertices[1]);//点Y是否在底面投影内
            bool point3InTri = floor.JudeIfPointInQuadrangleInXY(triangle.Vertices[2]);//点Z是否在底面投影内
            if (point1InTri)//若顶点1在建筑物底面投影内
            {
                insidePoint.Add(triangle.Vertices[0]); 
            }
            else
            {
                outsidePoint.Add(triangle.Vertices[0]);
            }
            if (point2InTri)//若顶点2在建筑物底面投影内
            {
                insidePoint.Add(triangle.Vertices[1]); 
            }
            else
            {
                outsidePoint.Add(triangle.Vertices[1]);
            }
            if (point3InTri)//若顶点3在建筑物底面投影内
            {
                insidePoint.Add(triangle.Vertices[2]);
            }
            else
            {
                outsidePoint.Add(triangle.Vertices[2]);
            }
            //
            newTerTriangle = this.SubdivideTriangleByFloorVertices(triangle, floor, insidePoint, outsidePoint);
            this.DeleteTriangleWhichIsLineInXY(newTerTriangle);
            
            return newTerTriangle;
        }



        /// <summary>
        /// 根据建筑物底面矩形顶点是否在三角面内对三角面进行剖分,用之前要先得到三角面顶点在建筑物底面的情况
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <param name="floor">建筑物底面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> SubdivideTriangleByFloorVertices(Triangle triangle, Quadrangle floor,List<Point> triInsidePoint, List<Point>triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            bool vertice1InTri = triangle.JudgeIfPointInTriangleInXY(floor.Vertices[0]);//点1是否在三角面投影内
            bool vertice2InTri = triangle.JudgeIfPointInTriangleInXY(floor.Vertices[1]);//点2是否在三角面投影内
            bool vertice3InTri = triangle.JudgeIfPointInTriangleInXY(floor.Vertices[2]);//点3是否在三角面投影内
            bool vertice4InTri = triangle.JudgeIfPointInTriangleInXY(floor.Vertices[3]);//点4是否在三角面投影内
            
            //根据4个布尔值的相互组合共有16种情况
            if (vertice1InTri && vertice2InTri && vertice3InTri && vertice4InTri)//四个顶点都在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenFourQuadVerticesInTri(floor, triangle, triInsidePoint, triOutsidePoint);
            }
            else if(vertice1InTri && !vertice2InTri && !vertice3InTri && !vertice4InTri)//顶点1在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenOneQuadVertexInTri(floor.Vertices[0], floor.Vertices[1], floor.Vertices[3], floor.Vertices[2], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && vertice2InTri && !vertice3InTri && !vertice4InTri)//顶点2在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenOneQuadVertexInTri(floor.Vertices[1], floor.Vertices[0], floor.Vertices[2], floor.Vertices[3], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && !vertice2InTri && vertice3InTri && !vertice4InTri)//顶点3在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenOneQuadVertexInTri(floor.Vertices[2], floor.Vertices[1], floor.Vertices[3], floor.Vertices[0], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && !vertice2InTri && !vertice3InTri && vertice4InTri)//顶点4在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenOneQuadVertexInTri(floor.Vertices[3], floor.Vertices[2], floor.Vertices[0], floor.Vertices[1], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (vertice1InTri && vertice2InTri && !vertice3InTri && !vertice4InTri)//顶点12在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenTwoQuadVerticesInTri(floor.Vertices[0], floor.Vertices[1], floor.Vertices[3], floor.Vertices[2], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (vertice1InTri && !vertice2InTri && vertice3InTri && !vertice4InTri)//顶点13在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenTwoQuadVerticesInTri(floor.Vertices[0], floor.Vertices[2], floor.Vertices[3], floor.Vertices[1], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (vertice1InTri && !vertice2InTri && !vertice3InTri && vertice4InTri)//顶点14在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenTwoQuadVerticesInTri(floor.Vertices[0], floor.Vertices[3], floor.Vertices[1], floor.Vertices[2], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && vertice2InTri && vertice3InTri && !vertice4InTri)//顶点23在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenTwoQuadVerticesInTri(floor.Vertices[1], floor.Vertices[2], floor.Vertices[0], floor.Vertices[3], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && vertice2InTri && !vertice3InTri && vertice4InTri)//顶点24在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenTwoQuadVerticesInTri(floor.Vertices[1], floor.Vertices[3], floor.Vertices[0], floor.Vertices[2], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && !vertice2InTri && vertice3InTri && vertice4InTri)//顶点34在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenTwoQuadVerticesInTri(floor.Vertices[2], floor.Vertices[3], floor.Vertices[1], floor.Vertices[0], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (vertice1InTri && vertice2InTri && vertice3InTri && !vertice4InTri)//顶点123在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenThreeQuadVerticesInTri(floor.Vertices[1], floor.Vertices[2], floor.Vertices[0], floor.Vertices[3], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (vertice1InTri && vertice2InTri && !vertice3InTri && vertice4InTri)//顶点124在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenThreeQuadVerticesInTri(floor.Vertices[0], floor.Vertices[2], floor.Vertices[1], floor.Vertices[3], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (vertice1InTri && !vertice2InTri && vertice3InTri && vertice4InTri)//顶点134在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenThreeQuadVerticesInTri(floor.Vertices[3], floor.Vertices[1], floor.Vertices[0], floor.Vertices[2], triangle, triInsidePoint, triOutsidePoint);
            }
            else if (!vertice1InTri && vertice2InTri && vertice3InTri && vertice4InTri)//顶点234在三角面投影内
            {
                newTerTriangle = this.GetNewTrisWhenThreeQuadVerticesInTri(floor.Vertices[2], floor.Vertices[0], floor.Vertices[1], floor.Vertices[3], triangle, triInsidePoint, triOutsidePoint);
            }
            else//顶点1234在三角面投影外
            {
                newTerTriangle = this.GetNewTrisWhenNoneQuadVertesInTri(triangle, floor, triInsidePoint, triOutsidePoint);
            }
           
            return newTerTriangle;
        }

        /// <summary>
        /// 细分三角面当底面4个顶点都在三角面内时，对应情况（15）
        /// </summary>
        /// <param name="floor">建筑物底面</param>
        /// <param name="triangle">三角面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenFourQuadVerticesInTri(Quadrangle floor,Triangle triangle, List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            if (triOutsidePoint.Count != 3)
            {
                throw new Exception("当建筑物底面在一个三角面内时,三角面的三个顶点不在建筑物底面外");
            }
            else
            {
                List<Triangle> newTerTriangle = new List<Triangle>();
                List<AdjacentEdge> insideLines = new List<AdjacentEdge>(floor.Lines);//存放在原三角面内的所有线段，包括新的三角面的线段，但是不包括原三角面的三条边，初始化时存放建筑物底面的4条边
                //建筑物底面四边形每条边与一个三角面顶点组成一个新的三角面，共得4个三角面
                for (int i = 0; i < 4; i++)//insideLine前4个元素是底面的4条边
                {
                    for (int j = 0; j < triOutsidePoint.Count; j++)
                    {
                        newTerTriangle.AddRange(this.GetNewTriangleByLine(insideLines[i], triOutsidePoint[j], insideLines, triangle));
                    }
                }
                //三角面的三条边与底面四边形的一个顶点组成一个新的三角面，共得3个三角面
                for (int k = 0; k < triangle.Lines.Count; k++)
                {
                    for (int l = 0; l < floor.Vertices.Count; l++)
                    {
                        newTerTriangle.AddRange(this.GetNewTriangleByLine(triangle.Lines[k], floor.Vertices[l], insideLines, triangle));
                    }
                }
                //
                this.AlterTriangleLineDiffractionFace(triangle, triangle.Lines, newTerTriangle);//修改原三角面边的绕射面
                this.AddDiffractionEdgesToInternalTriangles(insideLines, newTerTriangle, floor.Lines.Count); //得到内部三角面的绕射棱
                //
                newTerTriangle.Add(new Triangle(floor.Vertices[0], floor.Vertices[1], floor.Vertices[2], FaceType.Terrian, triangle.FaceID,triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(floor.Vertices[0], floor.Vertices[2], floor.Vertices[3], FaceType.Terrian, triangle.FaceID,triangle.Material, triangle.MaterialNum));
                if (newTerTriangle.Count != 9)
                { 
                    throw new Exception("当建筑物底面在一个三角面内时细分的三角面数量出错"); 
                }

                return newTerTriangle;
            }
        }

        /// <summary>
        /// 细分三角面当底面3个顶点在三角面内时，对应情况（13）（14）
        /// </summary>
        /// <param name="quadInsidePoint1">在三角面内的建筑物底面顶点1</param>
        /// <param name="quadOutsidePoint">在三角面外的建筑物底面内的点</param>
        /// <param name="quadInsidePoint2">在三角面内的建筑物底面顶点2,且与在三角面外的点相邻</param>
        /// <param name="quadInsidePoint3">在三角面内的建筑物底面顶点3，且与在三角面外的点相邻</param>
        /// <param name="triangle">三角面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenThreeQuadVerticesInTri(Point quadInsidePoint1, Point quadOutsidePoint, Point quadInsidePoint2, Point quadInsidePoint3, Triangle triangle, List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            AdjacentEdge floorLine1 = new AdjacentEdge(quadOutsidePoint, quadInsidePoint2);//一个在三角面内的点和一个在三角面外的点组成的线段
            AdjacentEdge floorLine2 = new AdjacentEdge(quadOutsidePoint, quadInsidePoint3);//一个在三角面内的点和一个在三角面外的点组成的线段
            List<Point> crossPoint1 = floorLine1.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint2 = floorLine2.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
           //
            if ( crossPoint1.Count > 1 ||crossPoint2.Count > 1)
            { 
                throw new Exception("当建筑物底面3个顶点在三角面内时,求建筑物底面与三角面在边上的交点时出错"); 
            }
            crossPoint1[0].Z = quadInsidePoint1.Z;//修改Z坐标，之前的Z坐标是0
            crossPoint2[0].Z = quadInsidePoint1.Z;
            //
            List<Point> crossPointWithTriLine = new List<Point>{crossPoint1[0],crossPoint2[0]};//存放在三角面边上的交点
            //存放在原三角面内的所有线段，包括新的三角面的线段，但是不包括原三角面的三条边,初始化时存放的是建筑物底面4个切割后的线段
            List<AdjacentEdge> insideLines = new List<AdjacentEdge> 
            { 
                new AdjacentEdge(crossPoint1[0], quadInsidePoint2),  new AdjacentEdge(crossPoint2[0], quadInsidePoint3),
                new AdjacentEdge(quadInsidePoint1, quadInsidePoint2),new AdjacentEdge(quadInsidePoint1, quadInsidePoint3)
            };
            List<Point> insidePoint = new List<Point> { quadInsidePoint1, quadInsidePoint2, quadInsidePoint3, crossPoint1[0], crossPoint2[0] };//存在建筑物底面在三角面内的顶点
            int originInsideLineNum = insideLines.Count;
            //
            newTerTriangle.AddRange(this.GetNewTriangleInOldTriangle(triangle, triOutsidePoint, crossPointWithTriLine, insideLines, insidePoint, originInsideLineNum));
            //
            this.AlterTriangleLineDiffractionFace(triangle, triangle.GetNoneCrossPointLines(crossPointWithTriLine), newTerTriangle);//修改原三角面边的绕射面
            this.AddDiffractionEdgesToInternalTriangles(insideLines, newTerTriangle, originInsideLineNum);//得到内部三角面的绕射棱
            //
            if (triOutsidePoint.Count == 3)//当三角面的三个端点都在建筑物底面外时，对应情况（13）
            {
                //建筑物底面在三角面内部分的细分三角面，有三个
                newTerTriangle.Add(new Triangle(crossPoint1[0], crossPoint2[0], quadInsidePoint2, FaceType.Terrian, triangle.FaceID,triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoint1[0], crossPoint2[0], quadInsidePoint3, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint1, quadInsidePoint2, quadInsidePoint3, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            }
            else if (triOutsidePoint.Count == 2)//当三角面的两个端点都在建筑物底面外时，对应情况（14）
            {
                triInsidePoint[0].Z = quadInsidePoint1.Z;
                //建筑物底面在三角面内部分的细分三角面，有四个
                newTerTriangle.Add(new Triangle(quadInsidePoint2, crossPoint1[0], triInsidePoint[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint2, triInsidePoint[0], crossPoint2[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint2, crossPoint2[0], quadInsidePoint3, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint2, quadInsidePoint3, quadInsidePoint1, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            }
            else 
            {
                throw new Exception("当建筑物底面三个点在三角面内时，三角面顶点出问题");
            }
            if (newTerTriangle.Count != 9)
            { 
                throw new Exception("当建筑物底面一个顶点在三角面内时细分的三角面数量出错"); 
            }
            
            return newTerTriangle;
        }

        /// <summary>
        /// 细分三角面当底面2个顶点在三角面内时
        /// </summary>
        /// <param name="quadInsidePoint1">在三角面内的建筑物底面顶点1</param>
        /// <param name="quadInsidePoint2">在三角面内的建筑物底面顶点2</param>
        /// <param name="quadOutsidePoint1">在三角面外的建筑物底面顶点1,其与quadInsidePoint1相邻</param>
        /// <param name="quadOutsidePoint2">在三角面外的建筑物底面内的点2，其与quadInsidePoint2相邻</param>
        /// <param name="triangle">三角面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenTwoQuadVerticesInTri(Point quadInsidePoint1, Point quadInsidePoint2, Point quadOutsidePoint1, Point quadOutsidePoint2,Triangle triangle, List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            LineSegment floorInsideLine1 = new LineSegment(quadInsidePoint1, quadInsidePoint2);//建筑物底面两个在三角面内的点组成的线段
            LineSegment floorOtusideLine = new LineSegment(quadOutsidePoint1, quadOutsidePoint2);//建筑物底面两个在三角面外的点组成的线段
            if (floorInsideLine1.GetCrossPointWithOtherLineInXY(floorOtusideLine)==null)//判断是否是建筑物底面的两个对顶点在三角面内，若否
            {
                this.GetNewTrisWhenTwoQuadVerticesInTri01(quadInsidePoint1, quadInsidePoint2, quadOutsidePoint1, quadOutsidePoint2, triangle, triInsidePoint, triOutsidePoint, newTerTriangle);
            }
            else //建筑物底面的两个对顶点在三角面内，对应情况（12）
            {
                this.GetNewTrisWhenTwoQuadVerticesInTri02(quadInsidePoint1, quadInsidePoint2, quadOutsidePoint1, quadOutsidePoint2, triangle, triOutsidePoint, newTerTriangle);
            }
            if (newTerTriangle.Count != 9 && newTerTriangle.Count !=7 )
            { 
                throw new Exception("当建筑物底面两个顶点在三角面内时细分的三角面数量出错");
            }
            
            return newTerTriangle;

        }

        /// <summary>
        /// 细分三角面当底面2个顶点在三角面内时,对应情况(12)
        /// </summary>
        /// <param name="quadInsidePoint1">在三角面内的建筑物底面顶点1</param>
        /// <param name="quadInsidePoint2">在三角面内的建筑物底面顶点2</param>
        /// <param name="quadOutsidePoint1">在三角面外的建筑物底面顶点1,其与quadInsidePoint1相邻</param>
        /// <param name="quadOutsidePoint2">在三角面外的建筑物底面内的点2，其与quadInsidePoint2相邻</param>
        /// <param name="triangle">三角面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <param name="newTerTriangle">存放细分后的三角面的List</param>
        private void GetNewTrisWhenTwoQuadVerticesInTri02(Point quadInsidePoint1, Point quadInsidePoint2, Point quadOutsidePoint1, Point quadOutsidePoint2, Triangle triangle, List<Point> triOutsidePoint, List<Triangle> newTerTriangle)
        {
            AdjacentEdge floorLine1 = new AdjacentEdge(quadInsidePoint1, quadOutsidePoint1);//一个在三角面内的点1和一个在三角面外的a点组成的线段
            AdjacentEdge floorLine2 = new AdjacentEdge(quadInsidePoint1, quadOutsidePoint2);//一个在三角面内的点1和一个在三角面外的点b组成的线段
            AdjacentEdge floorLine3 = new AdjacentEdge(quadInsidePoint2, quadOutsidePoint1);//一个在三角面内的点2和一个在三角面外的点a组成的线段
            AdjacentEdge floorLine4 = new AdjacentEdge(quadInsidePoint2, quadOutsidePoint2);//一个在三角面内的点2和一个在三角面外的点b组成的线段
            List<Point> crossPoint1 = floorLine1.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint2 = floorLine2.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint3 = floorLine3.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint4 = floorLine4.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            if (crossPoint1.Count > 1 ||  crossPoint2.Count > 1 || crossPoint3.Count > 1 ||  crossPoint4.Count > 1)
            { 
                throw new Exception("当建筑物底面2个顶点在三角面内时,求情况（16）建筑物底面与三角面在边上的交点时出错");
            }
            crossPoint1[0].Z = quadInsidePoint1.Z;//修改Z坐标，之前的Z坐标是0
            crossPoint2[0].Z = quadInsidePoint1.Z;
            crossPoint3[0].Z = quadInsidePoint1.Z;
            crossPoint4[0].Z = quadInsidePoint1.Z;
            //
            List<Point> crossPointWithTriLine = new List<Point> { crossPoint1[0], crossPoint2[0], crossPoint3[0], crossPoint4[0] };//存放在三角面边上的交点
            //存放在原三角面内的所有线段，包括新的三角面的线段，但是不包括原三角面的三条边,初始化时存放的是建筑物底面4个切割后的线段
            List<AdjacentEdge> insideLines = new List<AdjacentEdge> 
            { 
                new AdjacentEdge(quadInsidePoint1, crossPoint1[0]), new AdjacentEdge(quadInsidePoint1, crossPoint2[0]),
                new AdjacentEdge(quadInsidePoint2, crossPoint3[0]),new AdjacentEdge(quadInsidePoint2, crossPoint4[0])
            };
            List<Point> insidePoint = new List<Point> { quadInsidePoint1, quadInsidePoint2, crossPoint1[0], crossPoint2[0], crossPoint3[0], crossPoint4[0] };//存在建筑物底面在三角面内的顶点
            int originInsideLineNum = insideLines.Count;
            //
            newTerTriangle.AddRange(this.GetNewTriangleInOldTriangle(triangle, triOutsidePoint, crossPointWithTriLine, insideLines, insidePoint, originInsideLineNum));
            //
            this.AlterTriangleLineDiffractionFace(triangle, triangle.GetNoneCrossPointLines(crossPointWithTriLine), newTerTriangle);//修改原三角面边的绕射面
            this.AddDiffractionEdgesToInternalTriangles(insideLines, newTerTriangle, originInsideLineNum);//得到内部三角面的绕射棱
            
            //建筑物底面在三角面内部分的细分三角面，有四个
            newTerTriangle.Add(new Triangle(quadInsidePoint1, crossPoint1[0], crossPoint2[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            newTerTriangle.Add(new Triangle(quadInsidePoint2, crossPoint3[0], crossPoint4[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            newTerTriangle.Add(new Triangle(crossPoint1[0], crossPoint2[0], crossPoint3[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            newTerTriangle.Add(new Triangle(crossPoint2[0], crossPoint3[0], crossPoint4[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
           
        }

        /// <summary>
        /// 细分三角面当底面2个顶点在三角面内时,对应情况(9)(10)(11)
        /// </summary>
        /// <param name="quadInsidePoint1">在三角面内的建筑物底面顶点1</param>
        /// <param name="quadInsidePoint2">在三角面内的建筑物底面顶点2</param>
        /// <param name="quadOutsidePoint1">在三角面外的建筑物底面顶点1,其与quadInsidePoint1相邻</param>
        /// <param name="quadOutsidePoint2">在三角面外的建筑物底面内的点2，其与quadInsidePoint2相邻</param>
        /// <param name="triangle">三角面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <param name="newTerTriangle">存放细分后的三角面的List</param>
        private void GetNewTrisWhenTwoQuadVerticesInTri01(Point quadInsidePoint1, Point quadInsidePoint2, Point quadOutsidePoint1, Point quadOutsidePoint2, Triangle triangle, List<Point> triInsidePoint, List<Point> triOutsidePoint, List<Triangle> newTerTriangle)
        {
            AdjacentEdge floorLine1 = new AdjacentEdge(quadInsidePoint1, quadOutsidePoint1);//一个在三角面内的点和一个在三角面外的点组成的线段
            AdjacentEdge floorLine2 = new AdjacentEdge(quadInsidePoint2, quadOutsidePoint2);//一个在三角面内的点和一个在三角面外的点组成的线段
            AdjacentEdge floorLine3 = new AdjacentEdge(quadOutsidePoint1, quadOutsidePoint2);//两个在三角面外的点组成的线段
            List<Point> crossPoint1 = floorLine1.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint2 = floorLine2.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint3 = floorLine3.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            if (crossPoint1.Count > 1 || crossPoint2.Count > 1)
            { 
                throw new Exception("当建筑物底面2个顶点在三角面内时,求建筑物底面与三角面在边上的交点时出错"); 
            }
            crossPoint1[0].Z = quadInsidePoint1.Z;//修改Z坐标，之前的Z坐标是0
            crossPoint2[0].Z = quadInsidePoint1.Z;

            List<Point> crossPointWithTriLine = new List<Point> { crossPoint1[0], crossPoint2[0] };//存放在三角面边上的交点
            crossPointWithTriLine.AddRange(crossPoint2);
            //存放在原三角面内的所有线段，包括新的三角面的线段，但是不包括原三角面的三条边,初始化时存放的是建筑物底面3个切割后的线段
            List<AdjacentEdge> insideLines = new List<AdjacentEdge>
            { 
                new AdjacentEdge(quadInsidePoint1, quadInsidePoint2), 
                new AdjacentEdge(quadInsidePoint1, crossPoint1[0]),new AdjacentEdge(quadInsidePoint2, crossPoint2[0])
            };
            List<Point> insidePoint = new List<Point> { quadInsidePoint1, quadInsidePoint2, crossPoint1[0], crossPoint2[0] };//存在建筑物底面在三角面内的顶点
            insidePoint.AddRange(crossPoint3);
            if (crossPoint3.Count == 2)//若对应情况(10)
            {
                crossPoint3[0].Z = quadInsidePoint1.Z;
                crossPoint3[1].Z = quadInsidePoint1.Z;
                insideLines.Add(new AdjacentEdge(crossPoint3[0], crossPoint3[1]));
                crossPointWithTriLine.AddRange(crossPoint3);
            }
            int originInsideLineNum = insideLines.Count;
            //
            newTerTriangle.AddRange(this.GetNewTriangleInOldTriangle(triangle, triOutsidePoint, crossPointWithTriLine, insideLines, insidePoint, originInsideLineNum));

            //修改原三角面边的绕射面
            this.AlterTriangleLineDiffractionFace(triangle, triangle.GetNoneCrossPointLines(crossPointWithTriLine), newTerTriangle);
            this.AddDiffractionEdgesToInternalTriangles(insideLines, newTerTriangle, originInsideLineNum);//得到内部三角面的绕射棱
            //
            if (triOutsidePoint.Count == 3)//当三角面的三个端点都在建筑物底面外时，对应情况（9）（10）
            {
                if (crossPoint3 == null || crossPoint3.Count == 0)//对应情况(9)
                {
                    //建筑物底面在三角面内部分的细分三角面，有2个
                    newTerTriangle.Add(new Triangle(quadInsidePoint1, quadInsidePoint2, crossPoint2[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle.Add(new Triangle(quadInsidePoint1, crossPoint1[0], crossPoint2[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                }
                if (crossPoint3 != null && crossPoint3.Count == 2) //对应情况（110）
                {
                    //建筑物底面在三角面内部分的细分三角面，有四个
                    newTerTriangle.Add(new Triangle(crossPoint3[0], crossPoint3[1], crossPoint1[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle.Add(new Triangle(crossPoint3[0], crossPoint3[1], quadInsidePoint1, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle.Add(new Triangle(crossPoint3[0], quadInsidePoint1, quadInsidePoint2, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle.Add(new Triangle(crossPoint3[0], quadInsidePoint1, crossPoint1[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                }
            }
            else if (triOutsidePoint.Count == 2)//对应情况（11）
            {
                triInsidePoint[0].Z = quadInsidePoint1.Z;
                //建筑物底面在三角面内部分的细分三角面，有3个
                newTerTriangle.Add(new Triangle(quadInsidePoint1, crossPoint1[0], triInsidePoint[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint2, crossPoint2[0], triInsidePoint[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint1, quadInsidePoint2, triInsidePoint[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            }
            else
            {
                throw new Exception("当建筑物底面2个点在三角面内时，三角面顶点出问题");
            }
        }

        /// <summary>
        /// 细分三角面当底面1个顶点在三角面内时，对应情况（7）（8）
        /// </summary>
        /// <param name="quadInsidePoint">在三角面内的建筑物底面顶点</param>
        /// <param name="quadOutsidePoint1">在三角面外的建筑物底面顶点1,与quadInsidePoint相邻</param>
        /// <param name="quadOutsidePoint2">在三角面外的建筑物底面顶点2，与quadInsidePoint相邻</param>
        /// <param name="quadOutsidePoint3">在三角面外的建筑物底面内的点3</param>
        /// <param name="triangle">三角面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenOneQuadVertexInTri(Point quadInsidePoint, Point quadOutsidePoint1, Point quadOutsidePoint2, Point quadOutsidePoint3, Triangle triangle,List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            if(triInsidePoint.Count==3)//三角面的三个点都在建筑物底面的覆盖范围内，不需要对三角面做处理
            {
                return newTerTriangle;
            }
            if (triInsidePoint.Count == 2 && (quadInsidePoint.equal(triInsidePoint[0]) || quadInsidePoint.equal(triInsidePoint[1])))//三角面只有一个顶点与矩形面投影重合，不作处理
            {
                return newTerTriangle;
            }
            if (triInsidePoint.Count == 1 && quadInsidePoint.equal(triInsidePoint[0]))//三角面只有一个顶点与矩形面投影重合，不作处理
            {
                return newTerTriangle;
            }
            LineSegment floorLine1 = new LineSegment(quadInsidePoint, quadOutsidePoint1);//一个在三角面内的点和一个在三角面外的点组成的线段
            LineSegment floorLine2 = new LineSegment(quadInsidePoint, quadOutsidePoint2);//一个在三角面内的点和一个在三角面外的点组成的线段
            List<Point> crossPoint1 = floorLine1.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            List<Point> crossPoint2 = floorLine2.GetCrossPointsWithOtherLinesInXYPlane(triangle.Lines);//在XY平面的交点
            if ( crossPoint1.Count != 1 ||  crossPoint2.Count != 1)
            { 
                throw new Exception("当建筑物底面3个顶点在三角面内时,求建筑物底面与三角面在边上的交点时出错");
            }
            crossPoint1[0].Z = quadInsidePoint.Z;//修改Z坐标，之前的Z坐标是0
            crossPoint2[0].Z = quadInsidePoint.Z;
            List<Point> crossPointWithTriLine = new List<Point> { crossPoint1[0], crossPoint2[0] };//存放在三角面边上的交点
            //存放在原三角面内的所有线段，包括新的三角面的线段，但是不包括原三角面的三条边,初始化时存放的是建筑物底面4个切割后的线段
            List<AdjacentEdge> insideLines = new List<AdjacentEdge> { new AdjacentEdge(quadInsidePoint, crossPoint1[0]), new AdjacentEdge(quadInsidePoint, crossPoint2[0]) };
            List<Point> insidePoint = new List<Point> { quadInsidePoint, crossPoint1[0], crossPoint2[0] };//存在建筑物底面在三角面内的顶点
            int originInsideLineNum = insideLines.Count;
            //
            newTerTriangle.AddRange(this.GetNewTriangleInOldTriangle(triangle, triOutsidePoint, crossPointWithTriLine, insideLines, insidePoint, originInsideLineNum));
            //修改原三角面边的绕射面
            this.AlterTriangleLineDiffractionFace(triangle, triangle.GetNoneCrossPointLines(crossPointWithTriLine), newTerTriangle);
            this.AddDiffractionEdgesToInternalTriangles(insideLines, newTerTriangle, originInsideLineNum);//得到内部三角面的绕射棱
            //
            if (triOutsidePoint.Count == 3)//当三角面的三个端点都在建筑物底面外时，对应情况（7）
            {
                //建筑物底面在三角面内部分的细分三角面，有1个
                newTerTriangle.Add(new Triangle(quadInsidePoint, crossPoint1[0], crossPoint2[0], FaceType.Terrian, triangle.FaceID,triangle.Material, triangle.MaterialNum));
          //      newTerTriangle[4].SetLinesByVertice();//射线三角面的边
            }
            else if (triOutsidePoint.Count == 2)//当三角面的两个端点都在建筑物底面外时，对应情况（8）
            {
                triInsidePoint[0].Z = quadInsidePoint.Z;
                //建筑物底面在三角面内部分的细分三角面，有2个
                newTerTriangle.Add(new Triangle(quadInsidePoint, crossPoint1[0], triInsidePoint[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(quadInsidePoint, triInsidePoint[0], crossPoint2[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
           //     newTerTriangle[4].SetLinesByVertice();
          //      newTerTriangle[5].SetLinesByVertice();
            }
            else
            {
                throw new Exception("当建筑物底面一个顶点在三角面内时，三角面顶点出问题");
            }
            if (newTerTriangle.Count != 5)
            { 
                throw new Exception("当建筑物底面一个顶点在三角面内重新剖分的三角面数量错误");
            }
            
            return newTerTriangle;

        }

        /// <summary>
        /// 细分三角面当底面没有顶点在三角面内时
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <param name="floor">建筑物底面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenNoneQuadVertesInTri(Triangle triangle,Quadrangle floor, List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            if (triInsidePoint.Count == 3)//三角面在建筑物底面内时，只需要将三角面的Z坐标修改
            {
          //      Triangle newTriangle = (Triangle)triangle.Clone();
         //       newTriangle.Vertex1.Z = floor.Vertice1.Z;
         //       newTriangle.Vertex2.Z = floor.Vertice1.Z;
         //       newTriangle.Vertex3.Z = floor.Vertice1.Z;
        //        newTerTriangle.Add(newTriangle);
                triangle.Vertices[0].Z = floor.Vertices[0].Z;
                triangle.Vertices[1].Z = floor.Vertices[0].Z;
                triangle.Vertices[2].Z = floor.Vertices[0].Z;
            }
            else if (triInsidePoint.Count == 2)//建筑物底面没有顶点在三角面，但三角面有两个顶点在底面投影内时
            {
                newTerTriangle = this.GetNewTriWhenTwoTriVerticesInQuard(triangle, floor, triInsidePoint, triOutsidePoint);
            }
            else if (triInsidePoint.Count == 1)//建筑物底面没有顶点在三角面，但三角面有一个顶点在底面投影内时
            {
                newTerTriangle = this.GetNewTriWhenOneTriVertexInQuard(triangle, floor, triInsidePoint, triOutsidePoint);
            }
            return newTerTriangle;
        }



        /// <summary>
        /// 根据建筑物底面在三角面内的分布情况细分三角面，但是没有细分建筑物底面在三角面内的区域
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的点</param>
        /// <param name="newTerTriangle">存放新生成的三角面的List</param>
        /// <param name="crossPointWithTriLine">原来三角面边上的所有交点</param>
        /// <param name="insideLines">三角面内的所有线段，但不包括三角面的三条边</param>
        /// <param name="insidePoint">建筑物底面在三角面内的端点</param>
        /// <param name="originInsideLineNum">初始在三角面内的线段条数</param>
        private List<Triangle> GetNewTriangleInOldTriangle(Triangle triangle, List<Point> triOutsidePoint, List<Point> crossPointWithTriLine, List<AdjacentEdge> insideLines, List<Point> insidePoint, int originInsideLineNum)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            //建筑物底面在三角面内的线段与一个三角面顶点组成一个新的三角面
            for (int i = 0; i < originInsideLineNum; i++)
            {
                for (int j = 0; j < triOutsidePoint.Count; j++)
                {
                    newTerTriangle.AddRange(this.GetNewTriangleByLine(insideLines[i], triOutsidePoint[j], insideLines, triangle));
                }
            }

            //三角面的三条边与建筑物底面在三角面内的一个顶点组成一个新的三角面
            for (int k = 0; k < triangle.Lines.Count; k++)
            {
                if (triangle.Lines[k].JudgeIfPointsInLineInXYPlane(crossPointWithTriLine))
                { continue; }
                else
                {
                    for (int l = 0; l < insidePoint.Count; l++)
                    {
                        newTerTriangle.AddRange(GetNewTriangleByLine(triangle.Lines[k], insidePoint[l], insideLines, triangle));
                    }
                }
            }
            return newTerTriangle;
        }


        /// <summary>
        /// 根据线段上两端点与目标点组成的线段是否与三角面内其他线段相交来生成新的三角面
        /// </summary>
        /// <param name="EndPoint1">线段</param>
        /// <param name="targetPoint">目标点</param>
        /// <param name="insideLines">在三角面内的线段，不包括三角面的三条边</param>
        /// <param name="newTerTriangle">新的三角面的List</param>
        /// <param name="triangle">原来的三角面</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTriangleByLine(AdjacentEdge line, Point targetPoint, List<AdjacentEdge> insideLines, Triangle triangle)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            AdjacentEdge paramLine1 = new AdjacentEdge(line.StartPoint, targetPoint);
            AdjacentEdge paramLine2 = new AdjacentEdge(line.EndPoint, targetPoint);
            //若两条线段都没有与三角面内的其他线段相交
            if (!paramLine1.JudgeIfCrossWithOtherLinesInXYPlane(insideLines) && !paramLine2.JudgeIfCrossWithOtherLinesInXYPlane(insideLines))
            {
                Triangle param = new Triangle(line.StartPoint, line.EndPoint, targetPoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum);
                param.Lines.Add(line);//把该线段放入三角面的边List中
                newTerTriangle.Add(param);
                insideLines.Add(paramLine1);
                insideLines.Add(paramLine2);
            }
            return newTerTriangle;
        }

        /// <summary>
        /// 细分三角面当建筑物底面没有顶点在三角面，而三角面有两个顶点在建筑物底面内时
        /// </summary>
        /// <param name="triangle">三角面</param>
        ///  <param name="floor">建筑物底面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的顶点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的顶点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTriWhenTwoTriVerticesInQuard(Triangle triangle, Quadrangle floor, List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            for (int k = 0; k < floor.Lines.Count; k++)
            {
                if (triInsidePoint[0].JudgeIfPointInLineInXY(floor.Lines[k]) && triInsidePoint[1].JudgeIfPointInLineInXY(floor.Lines[k]))//建筑物底面的边与三角面的边平行
                {
                    triInsidePoint[0].Z = floor.Vertices[0].Z;//把在建筑物底面投影边上的两个三角面顶点的Z坐标改成与建筑物底面相同
                    triInsidePoint[1].Z = floor.Vertices[0].Z;
               //     newTerTriangle.Add(new TerTriangles(triInsidePoint[0], triInsidePoint[1], triOutsidePoint[0], triangle.Row, triangle.Line, triangle.MaterialNum, triangle.Conductivity, triangle.Permittivity));
                    return newTerTriangle;
                }
                else if (triInsidePoint[0].JudgeIfPointInLineInXY(floor.Lines[k]) && !triInsidePoint[1].JudgeIfPointInLineInXY( floor.Lines[k]))//三角面在建筑物底面投影内的点有一个点在建筑物底面投影的边上
                {
                    newTerTriangle = GetNewTrisWhenOneTriVertexOnQuardLine(triangle, floor.Lines[k], triInsidePoint[1], triInsidePoint[0], triOutsidePoint[0]);
                    break;
                }
                else if (!triInsidePoint[0].JudgeIfPointInLineInXY(floor.Lines[k]) && triInsidePoint[1].JudgeIfPointInLineInXY(floor.Lines[k]))//三角面在建筑物底面投影内的点有一个点在建筑物底面投影的边上
                {
                    newTerTriangle = GetNewTrisWhenOneTriVertexOnQuardLine(triangle, floor.Lines[k], triInsidePoint[0], triInsidePoint[1], triOutsidePoint[0]);
                    break;
                }
            }
            if (newTerTriangle.Count == 0)//若三角面的顶点不在建筑物底面投影的各个边上，则为情况（5）
            {
                triInsidePoint[0].Z = floor.Vertices[0].Z;
                triInsidePoint[1].Z = floor.Vertices[0].Z;
                newTerTriangle = this.GetNewTrisWhenNoneQuadPointInTriButTwoTriVerticeInQuad(triangle, floor, triOutsidePoint[0], triInsidePoint);
            }
            return newTerTriangle;
        }

        /// <summary>
        /// 细分三角面当建筑物底面没有顶点在三角面，而三角面有一个顶点在建筑物底面内时
        /// </summary>
        /// <param name="triangle">三角面</param>
        ///  <param name="floor">建筑物底面</param>
        /// <param name="triInsidePoint">三角面在建筑物底面内的顶点</param>
        /// <param name="triOutsidePoint">三角面在建筑物底面外的顶点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTriWhenOneTriVertexInQuard(Triangle triangle, Quadrangle floor, List<Point> triInsidePoint, List<Point> triOutsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            for (int k = 0; k < floor.Lines.Count; k++)
            {
                if (triInsidePoint[0].JudgeIfPointInLineInXY(floor.Lines[k]))//三角面的顶点是否在建筑物底面的边在XY平面投影上
                {
                    triInsidePoint[0].Z = floor.Vertices[0].Z;
                    return new List<Triangle>();
                }
            }
            //
            AdjacentEdge triLine = new AdjacentEdge(triOutsidePoint[0], triOutsidePoint[1]);//由两个三角面在建筑物底面投影外的顶点组成的线段
            if (triLine.JudgeIfCrossWithOtherLinesInXYPlane(floor.Lines))//若该线段与建筑物底面投影的边有交点，则为文档上的情况（6）
            {
                newTerTriangle = GetNewTrisWhenNoneQuadPointInTriButOneTriVertexInQuad02(triangle, floor, triInsidePoint[0], triOutsidePoint);
            }
            //
            if (newTerTriangle != null && newTerTriangle.Count == 0)//都上述情况都不是，则为情况（5）
            {
                triInsidePoint[0].Z = floor.Vertices[0].Z;
                newTerTriangle = this.GetNewTrisWhenNoneQuadPointInTriButOneTriVertexInQuad01(triangle, floor, triInsidePoint[0], triOutsidePoint);
            }
            return newTerTriangle;
        }

        /// <summary>
        /// 细分三角面当建筑物底面没有顶点在三角面，三角面一个顶点在建筑物底面内，一个顶点在建筑物底面边上时，对应情况（4）
        /// </summary>
        /// <param name="triangle">三角面</param>
        ///  <param name="floor">建筑物底面</param>
        /// <param name="oneSidePoint">被建筑物底面边分割后，三角面一侧的顶点</param>
        /// <param name="otherSidePoint">被建筑物底面边分割后，三角面一侧的顶点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenOneTriVertexOnQuardLine(Triangle triangle, LineSegment floorLine, Point inSidePoint,Point onSidePoint, Point outsidePoint)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            LineSegment triLine = new LineSegment(inSidePoint, outsidePoint);
            Point crossPoint = floorLine.GetCrossPointWithOtherLineInXY(triLine);
            if (crossPoint != null)
            {
                crossPoint.Z = floorLine.StartPoint.Z;
                inSidePoint.Z = floorLine.StartPoint.Z; ;
                onSidePoint.Z = floorLine.StartPoint.Z;
                Point triVertexOnFloor = (Point)onSidePoint.Clone();//三角面在建筑物底面XY投影边上的顶点在建筑物底面上的投影点
                triVertexOnFloor.Z = floorLine.StartPoint.Z;
                newTerTriangle.Add(new Triangle(inSidePoint, crossPoint, onSidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoint, onSidePoint, outsidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                //修改原三角面绕射棱对应的绕射面
                this.AlterTriangleLineDiffractionFace(triangle, triangle.GetNoneCrossPointLines(new List<Point> { crossPoint }), newTerTriangle); 
                return newTerTriangle;
            }
            else
            { 
                throw new Exception("当建筑物底面没有顶点在三角面，三角面一个顶点在建筑物底面内，一个顶点在建筑物底面边上时出错");
            }
        }

        /// <summary>
        /// 细分三角面当建筑物底面没有顶点在三角面，而三角面有两个点在底面内时，对应文档情况（5）
        /// </summary>
        /// <param name="triangle">三角面</param>
        ///  <param name="floor">建筑物底面</param>
        /// <param name="outSidePoint">在底面外放一个顶点</param>
        /// <param name="inSidePoints">在底面内的两个顶点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenNoneQuadPointInTriButTwoTriVerticeInQuad(Triangle triangle, Quadrangle floor, Point outSidePoint, List<Point> inSidePoints)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            for (int i = 0; i < floor.Lines.Count; i++)
            {
                LineSegment crossLine1 = new LineSegment(outSidePoint, inSidePoints[0]);//一个在三角面内的点和一个在三角面外的点组成的线段
                LineSegment crossLine2 = new LineSegment(outSidePoint, inSidePoints[1]);//一个在三角面内的点和一个在三角面外的点组成的线段
                Point crossPoint1 = crossLine1.GetCrossPointWithOtherLineInXY(floor.Lines[i]);//在XY平面的交点
                Point crossPoint2 = crossLine2.GetCrossPointWithOtherLineInXY(floor.Lines[i]);//在XY平面的交点

                if (crossPoint1 != null && crossPoint2 != null)
                {
                    crossPoint1.Z = floor.Vertices[0].Z;
                    crossPoint2.Z = floor.Vertices[0].Z;
                    //在底面外的三角面
                    newTerTriangle.Add(new Triangle(crossPoint1, crossPoint2, outSidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle[0].Lines.Add(new AdjacentEdge(crossPoint1, crossPoint2));
                    newTerTriangle[0].Lines.Add(new AdjacentEdge(crossPoint1, outSidePoint, newTerTriangle[0]));
                    newTerTriangle[0].Lines.Add(new AdjacentEdge(crossPoint1, outSidePoint, newTerTriangle[0]));
                    //
                    newTerTriangle.Add(new Triangle(crossPoint1, crossPoint2, inSidePoints[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle.Add(new Triangle(crossPoint2, inSidePoints[0], inSidePoints[1], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    break;
                }
            }
            if (newTerTriangle.Count != 3)
            { 
                throw new Exception("求情况（5）时出错");
            }
            return newTerTriangle;
        }

        /// <summary>
        /// 细分三角面当建筑物底面没有顶点在三角面，而三角面有一个点在底面内，对应文档情况（5）
        /// </summary>
        /// <param name="triangle">三角面</param>
        ///  <param name="floor">建筑物底面</param>
        /// <param name="inSidePoint">在底面内一个顶点</param>
        /// <param name="outSidePoints">在底面外的两个顶点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenNoneQuadPointInTriButOneTriVertexInQuad01(Triangle triangle, Quadrangle floor, Point inSidePoint, List<Point> outSidePoints)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            for (int i = 0; i < floor.Lines.Count; i++)
            {
                LineSegment crossLine1 = new LineSegment(inSidePoint, outSidePoints[0]);//一个在三角面内的点和一个在三角面外的点组成的线段
                LineSegment crossLine2 = new LineSegment(inSidePoint, outSidePoints[1]);//一个在三角面内的点和一个在三角面外的点组成的线段
                Point crossPoint1 = crossLine1.GetCrossPointWithOtherLineInXY(floor.Lines[i]);//在XY平面的交点
                Point crossPoint2 = crossLine2.GetCrossPointWithOtherLineInXY(floor.Lines[i]);//在XY平面的交点

                if (crossPoint1 != null && crossPoint2 != null)
                {
                    crossPoint1.Z = floor.Vertices[0].Z;
                    crossPoint2.Z = floor.Vertices[0].Z;
                    //底面内的三角面
                    newTerTriangle.Add(new Triangle(crossPoint1, crossPoint2, inSidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    //底面外的三角面
                    newTerTriangle.Add(new Triangle(crossPoint1, crossPoint2, outSidePoints[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                    newTerTriangle[1].Lines.Add(new AdjacentEdge(crossPoint1,crossPoint2));
                    newTerTriangle[1].Lines.Add(new AdjacentEdge(crossPoint1, outSidePoints[0], newTerTriangle[1]));
                    //
                    newTerTriangle.Add(new Triangle(crossPoint2, outSidePoints[0], outSidePoints[1], FaceType.Terrian, triangle.FaceID,triangle.Material, triangle.MaterialNum));
                    newTerTriangle[2].Lines.Add(new AdjacentEdge(crossPoint2, outSidePoints[1], newTerTriangle[2]));
                    newTerTriangle[2].Lines.AddRange(triangle.GetNoneCrossPointLines(new List<Point> { crossPoint1, crossPoint2 }));
                    //修改原三角面绕射棱对应的绕射面
                    this.AlterTriangleLineDiffractionFace(triangle, triangle.GetNoneCrossPointLines(new List<Point> { crossPoint1, crossPoint2 }), newTerTriangle);
                    //设置三角面1和2的绕射棱
                    AdjacentEdge line12 = new AdjacentEdge(crossPoint2, outSidePoints[0], newTerTriangle[1], newTerTriangle[2]);
                    newTerTriangle[1].Lines.Add(line12);
                    newTerTriangle[2].Lines.Add(line12);
                    break;
                }
            }
            if (newTerTriangle.Count != 3)
            {
                throw new Exception("求情况（5）时出错");
            }
            return newTerTriangle;
        }



        /// <summary>
        /// 细分三角面当建筑物底面没有顶点在三角面，三角面一个顶点在建筑物底面内，对应情况（6）
        /// </summary>
        /// <param name="triangle">三角面</param>
        ///  <param name="floor">建筑物底面</param>
        /// <param name="oneSidePoint">被建筑物底面边分割后，三角面一侧的顶点</param>
        /// <param name="otherSidePoint">被建筑物底面边分割后，三角面一侧的顶点</param>
        /// <returns>返回新的地形三角面的list</returns>
        private List<Triangle> GetNewTrisWhenNoneQuadPointInTriButOneTriVertexInQuad02(Triangle triangle, Quadrangle floor, Point insidePoint, List<Point> outsidePoints)
        {
            List<Triangle> newTerTriangle = new List<Triangle>();
            LineSegment triLine1 = new LineSegment(insidePoint, outsidePoints[0]);//由一个在建筑物底面内的点与一个在建筑物底面外的点组成的三角面边
            LineSegment triLine2 = new LineSegment(insidePoint, outsidePoints[1]);//由一个在建筑物底面内的点与一个在建筑物底面外的点组成的三角面边
            LineSegment triLine3 = new LineSegment(outsidePoints[0], outsidePoints[1]);//由两个在建筑物底面外的点组成的三角面边
            List<Point> crossPoints1 = triLine1.GetCrossPointsWithOtherLinesInXYPlane(floor.Lines);//三角面的边与建筑物底面的边在XY平面投影的交点
            List<Point> crossPoints2 = triLine2.GetCrossPointsWithOtherLinesInXYPlane(floor.Lines);//三角面的边与建筑物底面的边在XY平面投影的交点
            List<Point> crossPoints3 = triLine3.GetCrossPointsWithOtherLinesInXYPlane(floor.Lines);//三角面的边与建筑物底面的边在XY平面投影的交点
            if (crossPoints1.Count != 1 || crossPoints2.Count != 1 ||  crossPoints3.Count != 2)
            { 
                throw new Exception(" 求情况（6）的交点时出错"); 
            }
            crossPoints1[0].Z = floor.Vertices[0].Z;//修改Z坐标
            crossPoints2[0].Z = floor.Vertices[0].Z;
            crossPoints3[0].Z = floor.Vertices[0].Z;
            crossPoints3[1].Z = floor.Vertices[0].Z;
            insidePoint.Z = floor.Vertices[0].Z;
            newTerTriangle.Add(new Triangle(crossPoints3[0], crossPoints3[1], insidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            if (outsidePoints[0].GetDistance(crossPoints3[0]) < outsidePoints[0].GetDistance(crossPoints3[1]))//若crossPoint1[0]与crossPoint3[0]是文档情况（6）中三角面4的两个顶点
            {
                newTerTriangle.Add(new Triangle(crossPoints1[0], crossPoints3[0], insidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoints2[0], crossPoints3[1], insidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoints1[0], crossPoints3[0], outsidePoints[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoints2[0], crossPoints3[1], outsidePoints[1], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            }
            else
            {
                newTerTriangle.Add(new Triangle(crossPoints1[0], crossPoints3[1], insidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoints2[0], crossPoints3[0], insidePoint, FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoints1[0], crossPoints3[1], outsidePoints[0], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
                newTerTriangle.Add(new Triangle(crossPoints2[0], crossPoints3[0], outsidePoints[1], FaceType.Terrian, triangle.FaceID, triangle.Material, triangle.MaterialNum));
            }
            return newTerTriangle;
          
        }

        /// <summary>
        ///遍历三角面的list，将三角面三个顶点在XY平面投影在一条直线上的三角面删去
        /// </summary>
        /// <param name="triangles">三角面的List</param>
        /// <returns>将三角面三个顶点在XY平面投影在一条直线上的三角面删去后的list</returns>
        private  void DeleteTriangleWhichIsLineInXY(List<Triangle> triangles)
        {
            if (triangles == null || triangles.Count == 0)
            { return; }
            else
            {
                for (int i = triangles.Count - 1; i >= 0; i--)
                {
                    if (triangles[i].JudgeIfTriangleVerticesIsLineInXY())
                    { triangles.RemoveAt(i); }
                }
            }
        }

        /// <summary>
        /// 修改三角面经过细分后，原来的绕射棱对应的绕射面
        /// </summary>
        /// <param name="tri">三角面</param>
        /// <param name="triLines">三角面需要修改绕射面的边</param>
        /// <param name="newTriangles">细分的三角面，不包括矩形部分</param>
        /// <returns></returns>
        private void AlterTriangleLineDiffractionFace(Triangle triangle, List<AdjacentEdge> triLines, List<Triangle> newTriangles)
        {
            for (int i = 0; i < triLines.Count; i++)
            {
                if (triLines[i].IsDiffractionEdge)
                { triLines[i].AlterLineDiffractionFace(triangle, newTriangles); }
            }
        }

        /// <summary>
        ///获取三角面细分后内部的绕射棱
        /// </summary>
        /// <param name="insideLines">三角面内部的线段</param>
        /// <param name="newTriangles">细分的三角面，不包括矩形部分</param>
        /// <param name="rectLineNum">矩形线段在三角面内的数量</param>
        /// <returns>返回删除相同线段后的list</returns>
        private void AddDiffractionEdgesToInternalTriangles(List<AdjacentEdge> insideLines, List<Triangle> newTriangles,int rectLineNum )
        {
            List<AdjacentEdge> diffractionLines = this.ExtractDiffractionEdgesInTriLines(insideLines, rectLineNum);
            this.DeleteSameLines(diffractionLines);//删除重复的线段
            for (int m = 0; m < diffractionLines.Count; m++)
            {
                for (int n = 0; n < newTriangles.Count; n++)
                {
                    if (newTriangles[n].JudgeIfHaveThisLine(diffractionLines[m]))
                    {
                        newTriangles[n].Lines.Add(diffractionLines[m]);
                        diffractionLines[m].AddDiffractionFaceToEdge(newTriangles[n]);
                    }
                    if (newTriangles[n].Lines.Count > 3)
                    { throw new Exception("把绕射边加入三角面时出错"); }
                }
                if (diffractionLines[m].AdjacentTriangles.Count > 2)
                { throw new Exception("求绕射边三角面时出错"); }
            }
        }

        /// <summary>
        /// 将线段list中相同的线段删除
        /// </summary>
        /// <param name="tri">线段List</param>
        /// <returns>返回删除相同线段后的list</returns>
        private void DeleteSameLines(List<AdjacentEdge> lines)
        {
            if (lines.Count == 0 || lines.Count == 1)
            { return; }
            else
            {
                for (int i = lines.Count-1; i>0; i--)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (lines[i].JudgeIfTheSameLine(lines[j]))
                        {
                            lines.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 提取三角面内的线段中的绕射棱
        /// </summary>
        /// <param name="tri">线段List</param>
        /// <param name="rectLineNum">矩形部分在三角面内的线段条数</param>
        /// <returns>返回绕射棱</returns>
        private List<AdjacentEdge> ExtractDiffractionEdgesInTriLines(List<AdjacentEdge> triLines, int rectLineNum)
        {
            if (rectLineNum == triLines.Count)
            { return new List<AdjacentEdge>(); }
            else
            {
                List<AdjacentEdge> diffractionEdges = new List<AdjacentEdge>();
                for (int i = rectLineNum ; i < triLines.Count; i++)
                {
                    diffractionEdges.Add(triLines[i]);
                }
                return diffractionEdges;
            }
        }

        private void SetBuildingEdge()
        {
            if (build == null)
            { return; }
            else 
            {
                //build.Count个建筑物
                for (int i = 0; i < build.Count; i++)
                {
                    //每个建筑物有8条绕射棱，每条绕射棱都要被棱所在的面记录，所以记录16次
                    //FLU-BLU棱
                    build[i].BuildingFace[0].TriangleOne.Lines[0] = new AdjacentEdge(build[i].BuildingFace[0].TriangleOne, build[i].BuildingFace[1].TriangleTwo);
                    build[i].BuildingFace[1].TriangleTwo.Lines[0] = new AdjacentEdge(build[i].BuildingFace[0].TriangleOne, build[i].BuildingFace[1].TriangleTwo);
                    //FLU-FRU棱
                    build[i].BuildingFace[0].TriangleTwo.Lines[0] = new AdjacentEdge(build[i].BuildingFace[0].TriangleTwo, build[i].BuildingFace[4].TriangleTwo);
                    build[i].BuildingFace[4].TriangleTwo.Lines[0] = new AdjacentEdge(build[i].BuildingFace[0].TriangleTwo, build[i].BuildingFace[4].TriangleTwo);
                    //FRU-BRU棱
                    build[i].BuildingFace[0].TriangleTwo.Lines[1] = new AdjacentEdge(build[i].BuildingFace[0].TriangleTwo, build[i].BuildingFace[3].TriangleTwo);
                    build[i].BuildingFace[3].TriangleTwo.Lines[0] = new AdjacentEdge(build[i].BuildingFace[0].TriangleTwo, build[i].BuildingFace[3].TriangleTwo);
                    //BLU-BRU棱
                    build[i].BuildingFace[0].TriangleOne.Lines[1] = new AdjacentEdge(build[i].BuildingFace[0].TriangleOne, build[i].BuildingFace[2].TriangleOne);
                    build[i].BuildingFace[2].TriangleOne.Lines[0] = new AdjacentEdge(build[i].BuildingFace[0].TriangleOne, build[i].BuildingFace[2].TriangleOne);
                    //FLU-FLD
                    build[i].BuildingFace[4].TriangleOne.Lines[0] = new AdjacentEdge(build[i].BuildingFace[4].TriangleOne, build[i].BuildingFace[1].TriangleOne);
                    build[i].BuildingFace[1].TriangleOne.Lines[0] = new AdjacentEdge(build[i].BuildingFace[4].TriangleOne, build[i].BuildingFace[1].TriangleOne);
                    //BLU-BLD
                    build[i].BuildingFace[1].TriangleTwo.Lines[1] = new AdjacentEdge(build[i].BuildingFace[1].TriangleTwo, build[i].BuildingFace[2].TriangleOne);
                    build[i].BuildingFace[2].TriangleOne.Lines[1] = new AdjacentEdge(build[i].BuildingFace[1].TriangleTwo, build[i].BuildingFace[2].TriangleOne);
                    //BRU-BRD
                    build[i].BuildingFace[2].TriangleTwo.Lines[0] = new AdjacentEdge(build[i].BuildingFace[2].TriangleTwo, build[i].BuildingFace[3].TriangleOne);
                    build[i].BuildingFace[3].TriangleOne.Lines[0] = new AdjacentEdge(build[i].BuildingFace[2].TriangleTwo, build[i].BuildingFace[3].TriangleOne);
                    //FRU-FRD
                    build[i].BuildingFace[4].TriangleTwo.Lines[1] = new AdjacentEdge(build[i].BuildingFace[4].TriangleTwo, build[i].BuildingFace[3].TriangleTwo);
                    build[i].BuildingFace[3].TriangleTwo.Lines[1] = new AdjacentEdge(build[i].BuildingFace[4].TriangleTwo, build[i].BuildingFace[3].TriangleTwo);
                    //底面四条棱将绕射棱标志位设回false，之前设为true是为了让本方法上文中的标志位正确
                    build[i].BuildingFace[1].TriangleOne.Lines[1] = new AdjacentEdge(build[i].BuildingFace[1].Vertices[2], build[i].BuildingFace[1].Vertices[3]);
                    build[i].BuildingFace[2].TriangleTwo.Lines[1] = new AdjacentEdge(build[i].BuildingFace[2].Vertices[1], build[i].BuildingFace[2].Vertices[2]);
                    build[i].BuildingFace[3].TriangleOne.Lines[1] = new AdjacentEdge(build[i].BuildingFace[3].Vertices[2], build[i].BuildingFace[3].Vertices[3]);
                    build[i].BuildingFace[4].TriangleOne.Lines[1] = new AdjacentEdge(build[i].BuildingFace[4].Vertices[2], build[i].BuildingFace[4].Vertices[3]);
                }
            }
        }

    }   
}
