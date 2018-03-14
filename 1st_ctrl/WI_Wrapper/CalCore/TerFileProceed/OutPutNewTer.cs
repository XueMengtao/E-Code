using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using System.IO;
using System.Text.RegularExpressions;

namespace TerFileProceed
{
    public class OutPutNewTer
    {
        /// <summary>
        /// 输出新的地形
        /// </summary>
        /// <param name="projectPath">.site文件的路径</param>
        /// <param name="terRect">地形矩形数组</param>
        public static void OutPutNewTerrain(string projectPath, Rectangle[,] terRect)
        {
            if (File.Exists(projectPath + "newTer.ter"))
            { return; }
            else
            {
                List<Triangle> nonDivisionTris = new List<Triangle>();
                List<Triangle> subdivisionTris = new List<Triangle>();
                //遍历地形，把没有细分过的三角面放在一个list中，把细分过的三角面的细分三角面list放在另一个List中
                for (int i = 0; i < terRect.GetLength(0); i++)
                {
                    for (int j = 0; j < terRect.GetLength(1); j++)
                    {
                        if ( terRect[i, j].TriangleOne.NewTerTriangle.Count != 0)//若三角面经过细分
                        {
                            subdivisionTris.AddRange(terRect[i, j].TriangleOne.NewTerTriangle);
                        }
                        else//如果三角面不在建筑物内
                        {
                            nonDivisionTris.Add(terRect[i, j].TriangleOne);
                        }
                        if (terRect[i, j].TriangleTwo.NewTerTriangle.Count != 0)//若三角面没有经过
                        {
                            subdivisionTris.AddRange(terRect[i, j].TriangleTwo.NewTerTriangle);
                        }
                        else//如果三角面不在建筑物内
                        {
                            nonDivisionTris.Add(terRect[i, j].TriangleTwo);
                        }
                    }
                }
                OutputNewTerrainFile(projectPath, nonDivisionTris, subdivisionTris);//输出结果
            }
        }

        /// <summary>
        /// 输出新地形的文件
        /// </summary>
        /// <param name="projectPath">工程的路径</param>
        /// <param name="nonDivisionTris">原地形三角面</param>
        /// <param name="subdivisionTris">建筑物三角面</param>
        private static void OutputNewTerrainFile(string projectPath, List<Triangle> nonDivisionTris, List<Triangle> subdivisionTris)
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
