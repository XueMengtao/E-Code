using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;

namespace RayCalInfo
{
    public class Rays
    {

        #region 现在求绕射射线的方法

        /// <summary>
        /// 求射线射到棱上后得到的绕射射线List
        /// </summary>
        public static List<RayInfo> GetDiffractionRays(Point originPoint, Point diffractionPoint, AdjacentEdge diffractionEdge, int numberOfRay)
        {
            if ((originPoint == null) || (diffractionPoint == null) || (diffractionEdge == null))
            {
                LogFileManager.ObjLog.debug("求绕射射线的方法中输入的参数有null");
                return new List<RayInfo>();
            }
            double angleOfTwoTers =  GetAngleOfTwoTers(diffractionEdge.AdjacentTriangles[0], diffractionEdge.AdjacentTriangles[1], diffractionEdge);//获得两个三角面的夹角
            if (angleOfTwoTers > 178)
            {
          //      LogFileManager.ObjLog.debug("输入的绕射面的 夹角大于178度");
                return new List<RayInfo>();
            }
            else
            {
                RayInfo inRay = new RayInfo(originPoint, new SpectVector(originPoint, diffractionPoint));//从上一节点到绕射点重新构造一条入射线
                double angleOfRayAndEdge = inRay.GetAngleOfTwoStraightLines(diffractionEdge.SwitchToRay());//获取射线与劈边的夹角
                Point circleCenterPoint = GetCircleCenterPoint(angleOfRayAndEdge, inRay, diffractionEdge, diffractionPoint);//在劈边上选一个与绕射射线同向且不是绕射点的点,一般选公共棱的某个顶点,当夹角为90度时，绕射射线为圆盘，圆心为绕射点
                double circleRadius = GetCircleRadius(angleOfRayAndEdge, diffractionPoint, circleCenterPoint,numberOfRay);//所作圆的半径
                SpectVector circleVectorU = GetVectorInThePlaneOfCircle(diffractionEdge, circleCenterPoint);//先求圆所在平面上的一个向量
                SpectVector circleVectorV = GetQuadratureVectorInTheOfCircle(diffractionEdge, circleVectorU);//再求一个与前面所求向量和劈边向量都正交的向量，该向量也在圆所在的平面上
                //圆与三角面的交点
                Point crossPointOfFace0AndCircle = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[0].SwitchToSpaceFace(), diffractionPoint, circleRadius, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[0].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面1一侧的交点
                Point crossPointOfFace1AndCircle = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[1].SwitchToSpaceFace(), diffractionPoint, circleRadius, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[1].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面2一侧的交点
                if (crossPointOfFace0AndCircle == null || crossPointOfFace1AndCircle == null)
                {
                    LogFileManager.ObjLog.debug("求圆盘与绕射面的交点时有错");
                    return new List<RayInfo>();
                }
                SpectVector circlePointVector ;
                if (circleCenterPoint.equal(diffractionPoint))
                { circlePointVector = diffractionEdge.LineVector.GetNormalizationVector(); }
                else
                { circlePointVector = new SpectVector(diffractionPoint, circleCenterPoint).GetNormalizationVector(); }
                SetUnitVectorVnCirclePlane(ref circleVectorU, ref circleVectorV, circleCenterPoint, crossPointOfFace0AndCircle, crossPointOfFace1AndCircle, circlePointVector, angleOfTwoTers);
                List<Point> circumPoints = GetcircumPointOfTheCircle(diffractionEdge, circleCenterPoint, circleRadius, circleVectorU, circleVectorV,360-angleOfTwoTers, numberOfRay);//以某个角度划分获得所设圆上的点的list
                //根据绕射点和所设圆上的点，就可以求出绕射射线
                List<RayInfo> diffrationRay = GetRayListOfDiffraction(diffractionPoint, circumPoints);//包含绕射射线的list
                return diffrationRay;
            }
           
        }
        
        /// <summary>
        /// 求绕射棱与出射射线方向相同的一个点，当绕射点不等于棱的端点时，返回棱的端点，否则返回在对应方向上取的一点
        /// </summary>
        private static Point GetPointAtTheSameSideOfDiffractionRay(RayInfo incidentRay, AdjacentEdge sameEdge, Point diffractionPoint)
        {

            if (diffractionPoint.equal(sameEdge.StartPoint))//绕射点与棱的开始端点重合
            {
                Point centerPoint = new RayInfo(sameEdge.StartPoint,new SpectVector(sameEdge.EndPoint,sameEdge.StartPoint)).GetPointOnRayVector(1);
                if (new SpectVector(sameEdge.StartPoint, incidentRay.Origin).GetPhaseOfVector(new SpectVector(sameEdge.StartPoint, centerPoint)) > 90)
                {
                    return centerPoint;
                }
                else
                { return new RayInfo(sameEdge.StartPoint, sameEdge.EndPoint).GetPointOnRayVector(1); }
            }
            else if (diffractionPoint.equal(sameEdge.EndPoint))//绕射点与棱的结束端点重合
            {
                Point centerPoint = new RayInfo( sameEdge.EndPoint,new SpectVector(sameEdge.StartPoint,sameEdge.EndPoint)).GetPointOnRayVector(1);
                if (new SpectVector(sameEdge.EndPoint, incidentRay.Origin).GetPhaseOfVector(new SpectVector(sameEdge.EndPoint, centerPoint)) > 90)
                {
                    return centerPoint;
                }
                else
                { return new RayInfo(sameEdge.EndPoint, sameEdge.StartPoint).GetPointOnRayVector(1); }
            }
            else
            {

                SpectVector vectorOfDiffrationPointToOriginPoint = new SpectVector(diffractionPoint, incidentRay.Origin);//绕射点到发射点的向量
                SpectVector vectorOfDiffrationPointToTerSamePoint = new SpectVector(diffractionPoint, sameEdge.StartPoint);//绕射点到棱上一个顶点的向量
                if (vectorOfDiffrationPointToOriginPoint.GetPhaseOfVector(vectorOfDiffrationPointToTerSamePoint) > 90)//将圆心设为与绕射方向同向的一个共同点
                {
                    return sameEdge.StartPoint;
                }
                else
                {
                    return sameEdge.EndPoint;
                }
            }
        }

        /// <summary>
        /// 求所设圆的圆心
        /// </summary>
        private static Point GetCircleCenterPoint(double angleOfRayAndSameLine, RayInfo incidentRay, AdjacentEdge sameEdge, Point diffractionPoint)
        {
            if (Math.Abs(angleOfRayAndSameLine - 90) < 0.00001)//若为圆盘，则设绕射点为圆心
            {
                return diffractionPoint;
            }
            else//将圆心设为与绕射射线同一侧的顶点
            {
                Point edgePoint= GetPointAtTheSameSideOfDiffractionRay(incidentRay, sameEdge, diffractionPoint);
                return new RayInfo(diffractionPoint,edgePoint).GetPointOnRayVector(5);
            }
        }

        /// <summary>
        /// 求所设圆的半径
        /// </summary>
        private static double GetCircleRadius(double angleOfRayAndSameLine, Point diffractionPoint, Point circleCenterPoint, double numberOfRay)
        {
            double circleRadius;
            if (Math.Abs(angleOfRayAndSameLine - 90) < 0.00001)//射线垂直打到棱上
            { circleRadius = 1; }
            else
            { 
                circleRadius = diffractionPoint.GetDistance(circleCenterPoint) * Math.Tan(angleOfRayAndSameLine * Math.PI / 180);
                circleRadius = circleRadius / (Math.Cos(Math.PI / numberOfRay));//将内接正多边形变成外接正多边形，避免误差
            }
            return circleRadius;
        }

        /// <summary>
        /// 求在圆的平面上的一个向量
        /// </summary>
        private static SpectVector GetVectorInThePlaneOfCircle(AdjacentEdge sameEdge,Point centerPoint)
        {
            if (sameEdge.LineVector.a == 0 && sameEdge.LineVector.b == 0)
            {
                SpectVector unitCircleVectorU = new SpectVector(1,0,0);
                return unitCircleVectorU;
            }
            else
            {
           //     SpectVector circleVectorU = new SpectVector(sameEdge.LineVector.b, -sameEdge.LineVector.a, 0);//先求圆所在平面上的一个向量
           //     return circleVectorU.GetNormalizationVector();
                return sameEdge.AdjacentTriangles[0].NormalVector.GetNormalizationVector();
            }
        }

        /// <summary>
        /// 求在圆的平面上的一个与平面法向量，平面某个向量都正交的向量
        /// </summary>
        private static SpectVector GetQuadratureVectorInTheOfCircle(AdjacentEdge sameEdge,SpectVector vectorU)
        {
            if (sameEdge.LineVector.a == 0 && sameEdge.LineVector.b == 0)
            {
                SpectVector unitCircleVectorV = new SpectVector(0, 1, 0);
                return unitCircleVectorV;
            }
            else
            {
                //再求一个与1和劈边向量都正交的向量，该向量也在圆所在的平面上
                SpectVector circleVectorV = new SpectVector(sameEdge.LineVector.b * vectorU.c - sameEdge.LineVector.c * vectorU.b, 
                sameEdge.LineVector.c * vectorU.a - sameEdge.LineVector.a * vectorU.c, sameEdge.LineVector.a * vectorU.b - sameEdge.LineVector.b * vectorU.a );
                return circleVectorV.GetNormalizationVector();
            }
        }

        /// <summary>
        /// 这是一个以某个角度划分获得所设圆上的点的方法
        /// </summary>
        private static List<Point> GetcircumPointOfTheCircle(AdjacentEdge sameEdge, Point circleCenterPoint, double circleRadius, SpectVector unitCircleVectorU,  SpectVector unitCircleVectorV,double circleRange, int numberOfCircumPoint)
        {
            double angleOfTwoCircumPoint = circleRange * Math.PI / (numberOfCircumPoint*180);//取点的角度
            List<Point> circumPoints = new List<Point>();
            //第一个点取一个微小角度，避免与面重合
            circumPoints.Add(GetPointInCircumference(circleCenterPoint, circleRadius, unitCircleVectorU, unitCircleVectorV, 0.01));
            for (int i = 1; i < numberOfCircumPoint-1; i++)//在圆上取点，并放到list中
            {
                circumPoints.Add(GetPointInCircumference(circleCenterPoint, circleRadius, unitCircleVectorU, unitCircleVectorV, i * angleOfTwoCircumPoint));
            }
            //最后一个点减少一个微小角度，避免与面重合
            circumPoints.Add(GetPointInCircumference(circleCenterPoint, circleRadius, unitCircleVectorU, unitCircleVectorV, circleRange*Math.PI/180-0.01));
            return circumPoints;

        }

        /// <summary>
        /// 根据输入的角度求圆上的一点
        /// </summary>
        /// <param name="circleCenterPoint">圆心</param>
        /// <param name="circleRadius">半径</param>
        /// <param name="unitVectorU">圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">圆所在平面上一个向量与U垂足的向量V</param>
        /// <param name="arcAngle">角度</param>
        /// <returns></returns>
        private static Point GetPointInCircumference(Point circleCenterPoint, double circleRadius, SpectVector unitVectorU, SpectVector unitVectorV, double arcAngle)
        {
            Point circumferencePoint = new Point();
            circumferencePoint.X = circleCenterPoint.X + circleRadius * (unitVectorU.a * Math.Cos(arcAngle) + unitVectorV.a * Math.Sin(arcAngle));
            circumferencePoint.Y = circleCenterPoint.Y + circleRadius * (unitVectorU.b * Math.Cos(arcAngle) + unitVectorV.b * Math.Sin(arcAngle));
            circumferencePoint.Z = circleCenterPoint.Z + circleRadius * (unitVectorU.c * Math.Cos(arcAngle) + unitVectorV.c * Math.Sin(arcAngle));
            return circumferencePoint;
        }



        /// <summary>
        /// 求圆与三角面的交点
        /// </summary>
        /// <param name="terTri">三角面</param>
        /// <param name="centerPoint">圆心</param>
        /// <param name="radius">圆的半径</param>
        /// <param name="vectorU">圆所在平面的一个向量U，其与平面的法向量垂直</param>
        /// <param name="vectorV">圆所在平面的一个向量V,其与U及平面法向量相互垂直</param>
        /// <param name="triPoint">三角面除了绕射棱两个端点外的另一个点</param>
        /// <returns>返回圆与三角面的交点</returns>
        private static Point GetCrossPointOfCircleWithTer(SpaceFace terTri, Point centerPoint, double radius, SpectVector vectorU, SpectVector vectorV, Point triPoint)
        {
            //将空间圆的参数方程带入到平面方程Ax+By+Cz+D=0中可推导得下列式子
            //空间圆周参数方程参考 http://wenku.baidu.com/link?url=gsI3WsqXqjVOcLsNrLT76erK8g3XD828DxlehCvgBsFcKzLPw0hjQSZjxocv-gYOTtKtYcFOOOMKa4KQ215wCTkNWTApqk3c0mPU_XvkR7q

            //求圆与三角面所在平面的交点
            List<Point> crossPoints = new List<Point>();
            if (radius < 1)//半径太小导致判断有误差，所以设置半径不小于1
            { radius = 1; }
            SpectVector triVector = terTri.NormalVector.GetNormalizationVector();//获取三角面的法向量
            double D = -(triVector.a * terTri.Vertices[0].X + triVector.b * terTri.Vertices[0].Y + triVector.c * terTri.Vertices[0].Z);
            double M = radius * (triVector.a * vectorU.a + triVector.b * vectorU.b + triVector.c * vectorU.c);
            double N = radius * (triVector.a * vectorV.a + triVector.b * vectorV.b + triVector.c * vectorV.c);
            double[] sint = new double[2];
            sint[0] =Math.Round(Math.Sqrt(Math.Pow(N, 2) / (Math.Pow(M, 2) + Math.Pow(N, 2))),10);
            sint[1] = Math.Round(-Math.Sqrt(Math.Pow(N, 2) / (Math.Pow(M, 2) + Math.Pow(N, 2))),10);
            for (int i = 0; i < 2; i++)//由于分辨不清角度t的范围，所以将cost用1-sint^2开根的方法求，并得到4个点，其中两个不在平面内
            {
                if ((-1 <= sint[i]) && (sint[i] <= 1))//解的范围在-1到1之间时
                {
                    Point crossPoint1 = new Point();
                    double[] cost = new double[2];
                    cost[0] = Math.Sqrt(1 - Math.Pow(sint[i], 2));
                    cost[1] = -Math.Sqrt(1 - Math.Pow(sint[i], 2));
                    crossPoint1.X = centerPoint.X + radius * (vectorU.a * sint[i] + vectorV.a * cost[0]);
                    crossPoint1.Y = centerPoint.Y + radius * (vectorU.b * sint[i] + vectorV.b * cost[0]);
                    crossPoint1.Z = centerPoint.Z + radius * (vectorU.c * sint[i] + vectorV.c * cost[0]);
                    crossPoints.Add(crossPoint1);
                    Point crossPoint2 = new Point();
                    crossPoint2.X = centerPoint.X + radius * (vectorU.a * sint[i] + vectorV.a * cost[1]);
                    crossPoint2.Y = centerPoint.Y + radius * (vectorU.b * sint[i] + vectorV.b * cost[1]);
                    crossPoint2.Z = centerPoint.Z + radius * (vectorU.c * sint[i] + vectorV.c * cost[1]);
                    crossPoints.Add(crossPoint2);

                }
                else
                { 
                    LogFileManager.ObjLog.debug("生成绕射射线时，在求园与三角面交点时求sint出错");
                    return new Point();
                }
            }
            //删除不在三角面平面上的交点
            for (int i = crossPoints.Count-1; i>=0; i--)//排除不在平面的点
            {
                if (!terTri.JudgeIfPointInFace(crossPoints[i]))
                {
                    crossPoints.RemoveAt(i);
                }
            }
            //由于当thi角为90度时，求得的点中有两个是重复的点，故要将其删除
            if (crossPoints.Count == 4)
            {
                for (int m = crossPoints.Count - 1; m > 0; m--)
                {
                    for (int n = m - 1; n >= 0; n--)
                    {
                        if (Math.Abs(crossPoints[m].X - crossPoints[n].X) < 0.00001 &&
                            Math.Abs(crossPoints[m].Y - crossPoints[n].Y) < 0.00001 &&
                            Math.Abs(crossPoints[m].Z - crossPoints[n].Z) < 0.00001)
                        {
                            crossPoints.RemoveAt(m);
                            break;
                        }
                    }
                }
            }
            //圆与三角面或者在其一侧的交点
            if (crossPoints.Count == 2)
            {
                if (triPoint.GetDistance(crossPoints[0]) < triPoint.GetDistance(crossPoints[1]))
                { return crossPoints[0]; }
                else
                { return crossPoints[1]; }
            }
            else
            { 
                LogFileManager.ObjLog.debug("生成绕射射线时，在筛选园与三角面交点时出错");
                return new Point();
            }
        }

        /// <summary>
        /// 根据绕射点和圆上的点得到一个关于绕射射线的list
        /// </summary>
        private static List<RayInfo> GetRayListOfDiffraction(Point diffractionPoint, List<Point> filtratingCircumPoints)
        {
            List<RayInfo> diffrationRay = new List<RayInfo>();//包含绕射射线的list
            for (int j = 0; j < filtratingCircumPoints.Count; j++)
            {
                SpectVector middleParamVector = new SpectVector(diffractionPoint, filtratingCircumPoints[j]).GetNormalizationVector();
                if (middleParamVector.a == 0 && middleParamVector.b == 0 && middleParamVector.c == 0)
                {
                    LogFileManager.ObjLog.debug("生成绕射射线时，生成的绕射射线的方向向量为0");
                }
                RayInfo middleParamRay = new RayInfo(diffractionPoint, middleParamVector);
                diffrationRay.Add(middleParamRay);
            }
            return diffrationRay;
        }

        /// <summary>
        ///求两个三角面的夹角
        /// </summary>
        private static double GetAngleOfTwoTers(Face face1, Face face2, AdjacentEdge sameEdge)
        {
            //相邻三角面另外两个独立点
            Point otherPoint1 = face1.GetPointsOutOfTheLine(sameEdge)[0];
            Point otherPoint2 = face2.GetPointsOutOfTheLine(sameEdge)[0];
            SpectVector face1Vector1 = new SpectVector(otherPoint1, sameEdge.StartPoint);
            SpectVector face1Vector2 = new SpectVector(otherPoint1, sameEdge.EndPoint);
            SpectVector normalVector1 = SpectVector.VectorCrossMultiply(face1Vector1, face1Vector2);//获得第一个三角面的法向量
            SpectVector face2Vector1 = new SpectVector(otherPoint2, sameEdge.StartPoint);
            SpectVector face2Vector2 = new SpectVector(otherPoint2, sameEdge.EndPoint);
            SpectVector normalVector2 = SpectVector.VectorCrossMultiply(face2Vector2, face2Vector1);//获得第二个三角面的法向量
            double angleOfTers =180- SpectVector.VectorPhase(normalVector1, normalVector2);//两个法向量的夹角
            return angleOfTers;
        }

      
        #endregion



        /// <summary>
        /// 求从两个绕射点点发出的绕射射线
        /// </summary>
        /// <param name="originPoint1">辐射源1</param>
        ///  <param name="originPoint1">辐射源2</param>
        /// <param name="diffractionPoint1">绕射点1</param>
        /// <param name="diffractionPoint">绕射点2</param>
        /// <param name="diffractionEdge">绕射棱</param>
        /// <param name="numberOfRay">细分系数</param>
        /// <returns></returns>
        public static List<List<RayInfo>> GetDiffractionRaysFromTwoDiffractionPoints(Point originPoint1, Point originPoint2, Point diffractionPoint1, Point diffractionPoint2, AdjacentEdge diffractionEdge, int numberOfRay)
        {
            if ((originPoint1 == null) || (originPoint2 == null) || (diffractionPoint1 == null) || (diffractionPoint2 == null) || (diffractionEdge == null))
            {
                LogFileManager.ObjLog.debug("求绕射射线的方法中输入的参数有null");
                return new List<List<RayInfo>>();
            }
            double angleOfTwoTers = GetAngleOfTwoTers(diffractionEdge.AdjacentTriangles[0], diffractionEdge.AdjacentTriangles[1], diffractionEdge);//获得两个三角面的夹角
            if (angleOfTwoTers > 178)
            {
                //      LogFileManager.ObjLog.debug("输入的绕射面的 夹角大于178度");
                return new List<List<RayInfo>>();
            }
            else
            {
                Point symmetricPointOfOriginPoint1 = originPoint1.GetSymmetricPointOnEdge(diffractionPoint1);//得到发射点关于绕射点的对称点
                Point targetCenterPoint1 = diffractionEdge.SwitchToRay().getDropFoot(symmetricPointOfOriginPoint1);//对称点在棱上对应的垂足
                //Point circleCenterPoint = GetCircleCenterPoint(angleOfRayAndEdge, inRay, diffractionEdge, diffractionPoint);//在劈边上选一个与绕射射线同向且不是绕射点的点,一般选公共棱的某个顶点,当夹角为90度时，绕射射线为圆盘，圆心为绕射点
                double circleRadius1 = originPoint1.GetDistance(targetCenterPoint1);
                SpectVector circleVectorU = GetVectorInThePlaneOfCircle(diffractionEdge, targetCenterPoint1);//先求圆所在平面上的一个向量
                SpectVector circleVectorV = GetQuadratureVectorInTheOfCircle(diffractionEdge, circleVectorU);//再求一个与前面所求向量和劈边向量都正交的向量，该向量也在圆所在的平面上
                //圆与三角面的交点
                Point crossPointOfFace0AndCircle = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[0].SwitchToSpaceFace(), targetCenterPoint1, circleRadius1, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[0].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面1一侧的交点
                Point crossPointOfFace1AndCircle = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[1].SwitchToSpaceFace(), targetCenterPoint1, circleRadius1, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[1].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面2一侧的交点
                if (crossPointOfFace0AndCircle == null || crossPointOfFace1AndCircle == null)
                {
                    LogFileManager.ObjLog.debug("求圆盘与绕射面的交点时有错");
                    return new List<List<RayInfo>>();
                }
                SpectVector circlePointVector;
                if (targetCenterPoint1.equal(diffractionPoint1))
                { circlePointVector = diffractionEdge.LineVector.GetNormalizationVector(); }
                else
                { circlePointVector = new SpectVector(targetCenterPoint1, diffractionPoint1).GetNormalizationVector(); }
                SetUnitVectorVnCirclePlane(ref circleVectorU, ref circleVectorV, targetCenterPoint1, crossPointOfFace0AndCircle, crossPointOfFace1AndCircle, circlePointVector, angleOfTwoTers);
                //
                List<RayInfo> diffractionRay1 = new List<RayInfo>(), diffractionRay2 = new List<RayInfo>();
                if (originPoint1.equal(originPoint2))
                {
                    List<Point> targetPoints = GetcircumPointOfTheCircle(diffractionEdge, targetCenterPoint1, circleRadius1, circleVectorU, circleVectorV, 360 - angleOfTwoTers, numberOfRay);//以某个角度划分获得所设圆上的点的list
                    //根据绕射点和所设圆上的点，就可以求出绕射射线
                    for (int i = 0; i < targetPoints.Count; i++)
                    {
                        diffractionRay1.Add(new RayInfo(diffractionPoint1, new SpectVector(diffractionPoint1, targetPoints[i])));
                        diffractionRay2.Add(new RayInfo(diffractionPoint2, new SpectVector(diffractionPoint2, targetPoints[i])));
                    }
                }
                else
                {
                    Point symmetricPointOfOriginPoint2 = originPoint2.GetSymmetricPointOnEdge(diffractionPoint2);//得到发射点2关于绕射点的对称点
                    Point targetCenterPoint2 = diffractionEdge.SwitchToRay().getDropFoot(symmetricPointOfOriginPoint2);//发射点2在棱上对应的垂足
                    double circleRadius2 = originPoint1.GetDistance(targetCenterPoint2);
                    List<Point> targetPoints1 = GetcircumPointOfTheCircle(diffractionEdge, targetCenterPoint1, circleRadius1, circleVectorU, circleVectorV, 360 - angleOfTwoTers, numberOfRay);//以某个角度划分获得所设圆上的点的list
                    List<Point> targetPoints2 = GetcircumPointOfTheCircle(diffractionEdge, targetCenterPoint2, circleRadius2, circleVectorU, circleVectorV, 360 - angleOfTwoTers, numberOfRay);//以某个角度划分获得所设圆上的点的list
                    for (int i = 0; i < targetPoints1.Count; i++)
                    {
                        diffractionRay1.Add(new RayInfo(diffractionPoint1, new SpectVector(diffractionPoint1, targetPoints1[i])));
                        diffractionRay2.Add(new RayInfo(diffractionPoint2, new SpectVector(diffractionPoint2, targetPoints2[i])));
                    }
                }
                return new List<List<RayInfo>> { diffractionRay1, diffractionRay2 };
            }
        }




        /// <summary>
        /// 求绕射模型表面的细分三角形
        /// </summary>
        /// <param name="originPoint">辐射源</param>
        /// <param name="diffractionPoint1">绕射点1</param>
        /// <param name="diffractionPoint">绕射点2</param>
        /// <param name="diffractionEdge">绕射棱</param>
        /// <param name="divisionNumber">细分系数</param>
        /// <returns></returns>
        public static List<List<Triangle>> GetDiffractionModelSurfaceDivisionTriangles(Point originPoint, Point diffractionPoint1, Point diffractionPoint2, AdjacentEdge diffractionEdge, int divisionNumber)
        {
            if ((originPoint == null) || (diffractionPoint1 == null) || (diffractionPoint2 == null) || (diffractionEdge == null) || (divisionNumber<1))
            {
                LogFileManager.ObjLog.debug("求绕射射线的方法中输入的参数有null或者有错");
                return new List<List<Triangle>>();
            }
            double angleOfTwoTers =  GetAngleOfTwoTers(diffractionEdge.AdjacentTriangles[0], diffractionEdge.AdjacentTriangles[1], diffractionEdge);//获得两个三角面的夹角
            if (angleOfTwoTers > 178)
            {
                return new List<List<Triangle>>();
            }
            Point middlePoint = diffractionPoint1.GetMiddlePointWithOtherPoint(diffractionPoint2);
            if (new SpectVector(originPoint, middlePoint).GetPhaseOfVector(diffractionEdge.LineVector) > 88)
            {
                return GetCylinderSurfaceDivisionTriangles(originPoint, diffractionPoint1, diffractionPoint2, diffractionEdge, divisionNumber, angleOfTwoTers);
            }
            else
            {
                return GetCirclarTruncatedConeSurfaceDivisionTriangles(originPoint, diffractionPoint1, diffractionPoint2, diffractionEdge, divisionNumber, angleOfTwoTers);
            }

        }


        /// <summary>
        /// 求圆柱体绕射模型表面的细分三角形
        /// </summary>
        /// <param name="originPoint">辐射源</param>
        /// <param name="diffractionPoint1">绕射点1</param>
        /// <param name="diffractionPoint">绕射点2</param>
        /// <param name="diffractionEdge">绕射棱</param>
        /// <param name="divisionNumber">细分系数</param>
        /// <param name="angleOfTwoTers">两个绕射面的夹角</param>
        /// <returns></returns>
        private static List<List<Triangle>> GetCylinderSurfaceDivisionTriangles(Point originPoint, Point diffractionPoint1, Point diffractionPoint2, AdjacentEdge diffractionEdge, int divisionNumber, double angleOfTwoTers)
        {
            double circleRadius = 1;
            //求圆盘面上的一对相互垂直的方向向量
            SpectVector circleVectorU = GetVectorInThePlaneOfCircle(diffractionEdge, diffractionPoint1);//先求圆所在平面上的一个向量
            SpectVector circleVectorV = GetQuadratureVectorInTheOfCircle(diffractionEdge, circleVectorU);//再求一个与前面所求向量和劈边向量都正交的向量，该向量也在圆所在的平面上
            //圆与三角面的交点
            Point crossPointOfFace0AndCircle1 = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[0].SwitchToSpaceFace(), diffractionPoint1, circleRadius, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[0].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面1一侧的交点
            Point crossPointOfFace1AndCircle1 = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[1].SwitchToSpaceFace(), diffractionPoint1, circleRadius, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[1].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面2一侧的交点
            if (crossPointOfFace0AndCircle1 == null || crossPointOfFace1AndCircle1 == null)
            {
                LogFileManager.ObjLog.debug("求圆盘与绕射面的交点时有错");
                return new List<List<Triangle>>();
            }
            SpectVector circlePointVector = new SpectVector(diffractionPoint1, diffractionPoint2).GetNormalizationVector();
            SetUnitVectorVnCirclePlane(ref circleVectorU, ref circleVectorV, diffractionPoint1, crossPointOfFace0AndCircle1, crossPointOfFace1AndCircle1, circlePointVector, angleOfTwoTers);
            Cylinder diffractionCylinder = new Cylinder(diffractionPoint1, diffractionPoint2, circleRadius);
            return diffractionCylinder.GetSurfaceDivisionTriangles(8, 360 - angleOfTwoTers, circleVectorU, circleVectorV);
        }

        /// <summary>
        /// 求圆柱体绕射模型表面的细分三角形
        /// </summary>
        /// <param name="originPoint">辐射源</param>
        /// <param name="diffractionPoint1">绕射点1</param>
        /// <param name="diffractionPoint">绕射点2</param>
        /// <param name="diffractionEdge">绕射棱</param>
        /// <param name="divisionNumber">细分系数</param>
        /// <param name="angleOfTwoTers">两个绕射面的夹角</param>
        /// <returns></returns>
        private static List<List<Triangle>> GetCirclarTruncatedConeSurfaceDivisionTriangles(Point originPoint, Point diffractionPoint1, Point diffractionPoint2, AdjacentEdge diffractionEdge, int divisionNumber, double angleOfTwoTers)
        {
            RayInfo launchRay1 = new RayInfo(originPoint, diffractionPoint1);//从上一节点到绕射点1重新构造一条入射线
            RayInfo launchRay2 = new RayInfo(originPoint, diffractionPoint2);//从上一节点到绕射点2重新构造一条入射线
            double angleOfRay1AndEdge = launchRay1.GetAngleOfTwoStraightLines(diffractionEdge.SwitchToRay());//获取射线1与劈边的夹角
            double angleOfRay2AndEdge = launchRay2.GetAngleOfTwoStraightLines(diffractionEdge.SwitchToRay());//获取射线2与劈边的夹角
            double distanceToDiffractionPoint1 = originPoint.GetDistance(diffractionPoint1);
            double distanceToDiffractionPoint2 = originPoint.GetDistance(diffractionPoint2);
            double circleRadius1, circleRadius2;
            if (distanceToDiffractionPoint1 > distanceToDiffractionPoint2)
            {
                circleRadius1 = (distanceToDiffractionPoint1 - distanceToDiffractionPoint2) * Math.Sin(angleOfRay1AndEdge * Math.PI / 180);
                circleRadius2 = 2 * (distanceToDiffractionPoint1 - distanceToDiffractionPoint2) * Math.Sin(angleOfRay2AndEdge * Math.PI / 180);
            }
            else 
            {
                circleRadius2 = (distanceToDiffractionPoint2 - distanceToDiffractionPoint1) * Math.Sin(angleOfRay2AndEdge * Math.PI / 180);
                circleRadius1 = 2 * (distanceToDiffractionPoint2 - distanceToDiffractionPoint1) * Math.Sin(angleOfRay1AndEdge * Math.PI / 180);
            }
            Point circleCenterPoint1 = GetCircleCenterPoint(launchRay1, diffractionPoint1, angleOfRay1AndEdge, diffractionEdge, circleRadius1);//在劈边上选一个与绕射射线同向且不是绕射点的点,作为圆盘的圆心
            Point circleCenterPoint2 = GetCircleCenterPoint(launchRay2, diffractionPoint2, angleOfRay2AndEdge, diffractionEdge, circleRadius2);
            //求圆盘面上的一对相互垂直的方向向量
            SpectVector circleVectorU = GetVectorInThePlaneOfCircle(diffractionEdge, circleCenterPoint1);//先求圆所在平面上的一个向量
            SpectVector circleVectorV = GetQuadratureVectorInTheOfCircle(diffractionEdge, circleVectorU);//再求一个与前面所求向量和劈边向量都正交的向量，该向量也在圆所在的平面上
            //圆与三角面的交点
            Point crossPointOfFace0AndCircle1 = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[0].SwitchToSpaceFace(), circleCenterPoint1, circleRadius1, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[0].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面1一侧的交点
            Point crossPointOfFace1AndCircle1 = GetCrossPointOfCircleWithTer(diffractionEdge.AdjacentTriangles[1].SwitchToSpaceFace(), circleCenterPoint1, circleRadius1, circleVectorU, circleVectorV, diffractionEdge.AdjacentTriangles[1].GetPointsOutOfTheLine(diffractionEdge)[0]);//圆与三角面2一侧的交点
            if (crossPointOfFace0AndCircle1 == null || crossPointOfFace1AndCircle1 == null)
            {
                LogFileManager.ObjLog.debug("求圆盘与绕射面的交点时有错");
                return new List<List<Triangle>>();
            }
            SpectVector circlePointVector;//从大的圆盘的圆心指向小的圆盘的圆心
            CirclarTruncatedCone circlarTrimcatedCone;
            if (distanceToDiffractionPoint1 > distanceToDiffractionPoint2)
            {
                circlePointVector = new SpectVector(circleCenterPoint2, circleCenterPoint1).GetNormalizationVector();
                SetUnitVectorVnCirclePlane(ref circleVectorU, ref circleVectorV, circleCenterPoint1, crossPointOfFace0AndCircle1, crossPointOfFace1AndCircle1, circlePointVector, angleOfTwoTers);
                circlarTrimcatedCone = new CirclarTruncatedCone(circleCenterPoint2, circleRadius2, circleCenterPoint1, circleRadius1);
            }
            else
            {
                circlePointVector = new SpectVector(circleCenterPoint1, circleCenterPoint2).GetNormalizationVector();
                SetUnitVectorVnCirclePlane(ref circleVectorU, ref circleVectorV, circleCenterPoint1, crossPointOfFace0AndCircle1, crossPointOfFace1AndCircle1, circlePointVector, angleOfTwoTers);
                circlarTrimcatedCone = new CirclarTruncatedCone(circleCenterPoint1, circleRadius1, circleCenterPoint2, circleRadius2);
            }
            return circlarTrimcatedCone.GetConeSurfaceDivisionTriangles(divisionNumber, 360 - angleOfTwoTers, circleVectorU, circleVectorV);
        }



        /// <summary>
        /// 求绕射圆盘的圆心
        /// </summary>
        /// <param name="launchRay">入射射线</param>
        /// <param name="diffractionPoint">绕射点</param>
        /// <param name="angleOfDiffraction">射线与绕射棱的夹角</param>
        /// <param name="diffractionEdge">绕射棱</param>
        /// <param name="circleRadius">圆盘的半径</param>
        /// <returns></returns>
        private static Point GetCircleCenterPoint(RayInfo launchRay, Point diffractionPoint, double angleOfDiffraction, AdjacentEdge diffractionEdge, double circleRadius)
        {
             if (Math.Abs(angleOfDiffraction - 90) < 0.00001)//若为圆盘，则设绕射点为圆心
            {
                return diffractionPoint;
            }
            else//将圆心设为与绕射射线同一侧的点
            {
                Point edgePoint = GetPointAtTheSameSideOfDiffractionRay(launchRay, diffractionEdge, diffractionPoint);
                return new RayInfo(diffractionPoint, edgePoint).GetPointOnRayVector(circleRadius/Math.Tan(angleOfDiffraction*Math.PI/180));
            }
        }


        /// <summary>
        /// 根据在圆盘上绕射面的分布设置坐标系
        /// </summary>
        /// <param name="unitVectorU">圆所在平面上一个向量U</param>
        /// <param name="unitVectorV">圆所在平面上一个向量V</param>
        /// <param name="circleCenterPoint">圆盘的圆心</param>
        /// <param name="crossPointOfFace0AndCircle">圆盘与绕射面0的交点</param>
        /// <param name="crossPointOfFace1AndCircle">圆盘与绕射面1的交点</param>
        /// <param name="verticalVector">圆所在平面的法向量，由下底圆心指向上底圆心</param>
        /// <param name="angleOfTriangles">两个绕射面的夹角</param>
        /// <returns></returns>
        private static void SetUnitVectorVnCirclePlane(ref SpectVector unitVectorU,ref SpectVector unitVectorV, Point circleCenterPoint,
            Point crossPointOfFace0AndCircle,Point crossPointOfFace1AndCircle,SpectVector verticalVector,double angleOfTriangles)
        {
            unitVectorU = new SpectVector(circleCenterPoint, crossPointOfFace0AndCircle).GetNormalizationVector();
            unitVectorV = unitVectorU.CrossMultiplied(verticalVector).GetReverseVector().GetNormalizationVector();
            if (! unitVectorU.CrossMultiplied(unitVectorV).IsParallelAndSamedirection(verticalVector))
            {
                unitVectorV = unitVectorV.GetReverseVector();
            }

            SpectVector otherVectorU = new SpectVector(circleCenterPoint, crossPointOfFace1AndCircle).GetNormalizationVector();
            if (angleOfTriangles < 90)
            {
                if (Math.Abs(unitVectorV.GetPhaseOfVector(otherVectorU) + angleOfTriangles - 90)<0.00001)
                {

                    unitVectorU = otherVectorU;
                    unitVectorV = verticalVector.CrossMultiplied(unitVectorU);
                }
            }
            else if(angleOfTriangles>90)
            {
                if (Math.Abs(angleOfTriangles - unitVectorV.GetPhaseOfVector(otherVectorU) - 90 ) < 0.00001)
                {
                    unitVectorU = otherVectorU;
                    unitVectorV = verticalVector.CrossMultiplied(unitVectorU);
                }
            }
            else 
            {
                if (unitVectorV.IsParallelAndSamedirection(otherVectorU))
                {
                    unitVectorV = unitVectorU.GetReverseVector();
                    unitVectorU=otherVectorU;
                }
            }
        }

    }
}
