using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;

namespace RayCalInfo
{
   public class DiffractEfiledCal
   {
        #region 现在计算绕射场强的方法


        /// <summary>
        /// 求绕射场场强，文档公式1
        /// </summary>
        public static EField GetDiffractionEField(EField RayEFieldAtDiffractionPoint, RayInfo ray, Face diffractionFace1, Face diffractionFace2, Point diffractionPoint, Point viewPoint, double frequence)
        {
            if ((RayEFieldAtDiffractionPoint == null) || (ray == null) || (diffractionFace1 == null) || (diffractionFace2 == null)
                || (diffractionPoint == null) || (viewPoint == null))
            { throw new Exception("绕射场强计算输入的参数中有参数是null"); }
            //
            AdjacentEdge sameEdge = new AdjacentEdge(diffractionFace1, diffractionFace2);//获取劈边
            double waveLength = 300.0 / frequence;//波长
            Plural PluralOfVerticalEField = EField.GetVerticalE(RayEFieldAtDiffractionPoint, ray.RayVector, sameEdge.LineVector);//获得电场的垂直分量
            Plural PluralOfHorizonalEField = EField.GetHorizonalE(RayEFieldAtDiffractionPoint, ray.RayVector, sameEdge.LineVector);//获得电场的水平分量
            double Ad = GetSpreadFactor(ray.Origin, sameEdge, diffractionPoint, viewPoint, waveLength);//获得空间衰减的扩散因子
            double k = 2 * Math.PI / waveLength;//波矢量
            double s2 = diffractionPoint.GetDistance(viewPoint);//绕射点到观察点的距离
            Plural ejks = new Plural(Math.Cos(k * s2), -Math.Sin(k * s2));//exp(-jks)，相位
            Plural verticalDiffractionFactor = GetDiffractionFactor(ray, diffractionFace1, diffractionFace2,sameEdge, diffractionPoint, viewPoint, waveLength, true); ;//垂直极化波入射时的绕射系数D
            Plural PluralOfVerticalDiffractionEField = Plural.PluralMultiplyDouble(PluralOfVerticalEField * verticalDiffractionFactor * ejks, Ad);//垂直极化波入射时的绕射场
            Plural horizonalDiffractionFactor = GetDiffractionFactor(ray, diffractionFace1, diffractionFace2,sameEdge, diffractionPoint, viewPoint, waveLength, false); ;//水平极化波入射时的绕射系数D
            Plural PluralOfHorizonalDiffractionEField = Plural.PluralMultiplyDouble(horizonalDiffractionFactor * PluralOfHorizonalEField * ejks, Ad);//水平极化波入射时的绕射场
            SpectVector vectorOfDiffractionRay = new SpectVector(diffractionPoint, viewPoint);//绕射波的方向
            //垂直极化波绕射后的绕射场
            SpectVector vectorOfVerticalDiffractionEField = GetVectorOfVerticalEField(vectorOfDiffractionRay, sameEdge.LineVector);//获得电场的垂直分量的方向
            EField verticalDiffractionEField = GetXYZComponentOfTotalEField(PluralOfVerticalDiffractionEField, vectorOfVerticalDiffractionEField);
            //水平极化波绕射后的绕射场
            SpectVector vectorOfHorizonalDiffractionEField = GetVectorOfHorizonalEField(vectorOfDiffractionRay, sameEdge.LineVector);//获得电场的水平分量的方向
            EField horizonalDiffractionEField = GetXYZComponentOfTotalEField(PluralOfHorizonalDiffractionEField, vectorOfHorizonalDiffractionEField);
            EField diffractionEField = new EField(verticalDiffractionEField.X + horizonalDiffractionEField.X, verticalDiffractionEField.Y + horizonalDiffractionEField.Y, verticalDiffractionEField.Z + horizonalDiffractionEField.Z);//绕射场
            return diffractionEField;

        }

        /// <summary>
        ///求启发式绕射系数，文档公式3,5
        /// </summary>
        private static Plural GetDiffractionFactor(RayInfo ray, Face diffractionFace1, Face diffractionFace2,  AdjacentEdge sameEdge,Point diffractionPoint, Point viewPoint, double waveLength, bool judgeVerticallyPolarizedWave)
        {
            Plural D = new Plural();
            //求a1,a2
            Face rayFace = new SpaceFace(sameEdge.StartPoint, sameEdge.EndPoint, ray.Origin);
            Face diffractionRayFace = new SpaceFace(sameEdge.StartPoint, sameEdge.EndPoint, viewPoint);
            Face frontFace = GetTheFrontFace(diffractionFace1, diffractionFace2, rayFace, sameEdge);//先与射线相交的三角面
            Face laterFace = diffractionFace2;
            if (frontFace != diffractionFace1)
            {
                laterFace = diffractionFace1;//后相交的三角面
            }
            double a1 = GetAngleOfTwoTers(frontFace, rayFace,sameEdge) * Math.PI / 180;
            double a2;
            if (ray.Origin.JudgeIsConcaveOrConvexToViewPoint(frontFace, diffractionRayFace))//若两个三角面的形状是凸的
            {
                double angleWithFront = GetAngleOfTwoTers(frontFace, diffractionRayFace, sameEdge);
                double angleWithLater = GetAngleOfTwoTers(laterFace, diffractionRayFace, sameEdge);
                if(angleWithFront<angleWithLater)
                {
                    a2 = angleWithFront * Math.PI / 180;
                }
                else
                {
                    a2 = (360 - angleWithFront) * Math.PI / 180;
                }
            }
            else//若两个三角面的形状是凹的
            { a2 = GetAngleOfTwoTers(frontFace, diffractionRayFace,sameEdge) * Math.PI / 180; }
            //求n
            double angleOfTwoTers = GetAngleOfTwoTers(diffractionFace1, diffractionFace2,sameEdge);
            double n = 2 - (angleOfTwoTers / 180);//n=2-a/pi,其中a为两个三角面的夹角
            //求ri
            double[] r = new double[4];
            r[0] = (Math.PI - (a2 - a1)) / (2 * n);
            r[1] = (Math.PI + (a2 - a1)) / (2 * n);
            r[2] = (Math.PI - (a2 + a1)) / (2 * n);
            r[3] = (Math.PI + (a2 + a1)) / (2 * n);
            //求k
            double k = 2 * Math.PI / waveLength;//波矢量
            //求L
            double L = GetDistanceParameter(ray.Origin, diffractionPoint, sameEdge, viewPoint, waveLength);
            //求Di
            Plural[] Di = new Plural[4];
            Plural minusepi4 = new Plural(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);
            for (int i = 0; i < 4; i++)
            {
                Di[i] = Plural.PluralMultiplyDouble(minusepi4 * GetTransitionFunction(2 * k * L * Math.Pow(n, 2) * Math.Pow(Math.Sin(r[i]), 2)), 1 / (Math.Tan(r[i]) * (2 * n * Math.Sqrt(2 * k * Math.PI))));
            }
            //求反射系数R
            RayInfo diffractionRay = new RayInfo(diffractionPoint, viewPoint);
            
            Plural Ro = GetReflectionCoefficient(ray, frontFace, a1, waveLength, judgeVerticallyPolarizedWave);
            Plural Rn = GetReflectionCoefficient(diffractionRay, laterFace, n * Math.PI - a2, waveLength, judgeVerticallyPolarizedWave);
            D = Di[0] + Di[1] * Ro * Rn + Di[2] * Ro + Di[3] * Rn;
            return D;
        }

        /// <summary>
        /// 求空间衰减的扩散因子Ad，文档公式2
        /// </summary>
        private static double GetSpreadFactor(Point originPoint, AdjacentEdge sameEdge, Point diffrationPoint, Point viewPoint, double waveLength)
        {
            double distanceOfOriginAndSameLine = originPoint.GetDistanceToLine(sameEdge.StartPoint, sameEdge.LineVector);//辐射源到公共棱的距离
            double s1 = originPoint.GetDistance(diffrationPoint);
            double s2 = diffrationPoint.GetDistance(viewPoint);
            double Ad;
            if (s2 == 0)//若观察点为绕射点
            { Ad = 1; }
            else
            {
                if (distanceOfOriginAndSameLine > (10 * waveLength))//源点要绕射点的距离大于10个波长
                {
                    Ad = 1 / (Math.Sqrt(s2));//平面波和柱面波入射时的扩散因子
                }
                else
                {
                    Ad = Math.Sqrt(s1 / (s2 * (s1 + s2)));//球面波入射时的扩散因子
                }
            }
            return Ad;
        }

        /// <summary>
        /// 求反射系数R，文档公式4
        /// </summary>
        private static Plural GetReflectionCoefficient(RayInfo ray, Face diffractionFace, double reflectionAngle, double waveLength, bool judgeWhetherIsVerticallyPolarizedWave)
        {
            Plural R = new Plural();
            if (judgeWhetherIsVerticallyPolarizedWave)//垂直极化波
            {
                double a, b, A, B, C;
                Plural temp = new Plural();  //反射系数化简后的复数形式
                temp.Re = diffractionFace.Material.DielectricLayer[0].Permittivity - Math.Sin(reflectionAngle) * Math.Sin(reflectionAngle);
                temp.Im = 60 * diffractionFace.Material.DielectricLayer[0].Conductivity * waveLength;
                a = Plural.PluralSqrt(temp).Re;  //求反射系数的中间量
                b = Plural.PluralSqrt(temp).Im;  //求反射系数的中间量
                A = Math.Cos(reflectionAngle) * Math.Cos(reflectionAngle) - a * a - b * b;
                B = (a + Math.Cos(reflectionAngle)) * (a + Math.Cos(reflectionAngle)) + b * b;
                if (Math.Abs(A) < 0.000001)
                {
                    A = 0;
                }
                R.Re = A / B;
                C = -2 * b * Math.Cos(reflectionAngle);
                if (Math.Abs(A) < 0.000001)
                {
                    C = 0;
                }
                R.Im = C / B;
            }
            else//水平极化波
            {
                double a, b, A, B, C;
                Plural temp = new Plural();  //反射系数化简后的复数形式
                temp.Re = diffractionFace.Material.DielectricLayer[0].Permittivity - Math.Sin(reflectionAngle) * Math.Sin(reflectionAngle);
                temp.Im = 60 * diffractionFace.Material.DielectricLayer[0].Conductivity * waveLength;
                double temp1 = 60 * diffractionFace.Material.DielectricLayer[0].Conductivity * waveLength * Math.Cos(reflectionAngle);
                a = Plural.PluralSqrt(temp).Re;
                b = Plural.PluralSqrt(temp).Im;
                A = diffractionFace.Material.DielectricLayer[0].Permittivity * diffractionFace.Material.DielectricLayer[0].Permittivity * Math.Cos(reflectionAngle) * Math.Cos(reflectionAngle) - a * a - b * b + temp1 * temp1;
                B = (diffractionFace.Material.DielectricLayer[0].Permittivity * Math.Cos(reflectionAngle) + a) * (diffractionFace.Material.DielectricLayer[0].Permittivity * Math.Cos(reflectionAngle) + a) + (b - temp1) * (b - temp1);
                if (Math.Abs(A) < 0.000001)
                {
                    A = 0;
                }
                R.Re = A / B; 
                R.Re = A / B;
                C = -2 * b * diffractionFace.Material.DielectricLayer[0].Permittivity * Math.Cos(reflectionAngle) - 2 * temp1 * a;
                if (Math.Abs(A) < 0.000001)
                {
                    C = 0;
                }
                R.Im = C / B;
            }
            return R;

        }

        /// <summary>
        /// 求修正Keller非一致性解的过渡函数，文档公式6
        /// </summary>
        private static Plural GetTransitionFunction(double x)
        {
            Plural Fx = new Plural();
            if ((0 <= x) && (x < 0.001))//当x在0-0.001时
            {
                double a = Math.Cos(Math.PI / 4 + Math.Pow(x, 2));
                double b = Math.Sin(Math.PI / 4 + Math.Pow(x, 2));
                Fx.Re = Math.Sqrt(Math.PI) * a * x - Math.Sqrt(2) * Math.Pow(x, 2) * a - Math.Sqrt(2) * a * Math.Pow(x, 4) / 3 + Math.Sqrt(2) * Math.Pow(x, 2) * b - Math.Sqrt(2) * b * Math.Pow(x, 4) / 3;
                Fx.Im = Math.Sqrt(2) * Math.Pow(x, 4) * a / 3 - Math.Sqrt(2) * Math.Pow(x, 2) * a + Math.Sqrt(Math.PI) * b * x - Math.Sqrt(2) * Math.Pow(x, 2) * b - Math.Sqrt(2) * Math.Pow(x, 4) * b / 3;
            }
            else if ((0.001 <= x) && (x <= 10))//当x在0.001-10之间时
            {
                Plural expi4 = new Plural(Math.Cos(Math.PI / 4 + x), Math.Sin(Math.PI / 4 + x));
                Plural ex = new Plural(Math.Cos(x), Math.Sin(x));
                double re = 0, im = 0, temp = 1;
                for (int times = 0; times < 30; times++)
                {
                    if (times != 0)
                    {
                        temp *= times;
                    }
                    if (times % 2 == 0)
                    {
                        if (times % 4 == 0)
                        {
                            re = re + (Math.Pow(x, times) * Math.Sqrt(x) / temp / (2 * times + 1));
                        }
                        else
                        {
                            re = re - (Math.Pow(x, times) * Math.Sqrt(x) / temp / (2 * times + 1));
                        }
                    }
                    else
                    {
                        if ((times + 1) % 4 == 0)
                        {
                            im = im + (Math.Pow(x, times) * Math.Sqrt(x) / temp / (2 * times + 1));
                        }
                        else
                        {
                            im = im - (Math.Pow(x, times) * Math.Sqrt(x) / temp / (2 * times + 1));
                        }
                    }
                }
                Plural integralejx2 = new Plural(re, im);
                Plural exMultiplyintegralejx2 = ex * integralejx2;
                Plural j2sqrtxMultiplyexMultiplyintegralejx2 = new Plural(-2 * Math.Sqrt(x) * exMultiplyintegralejx2.Re, 2 * Math.Sqrt(x) * exMultiplyintegralejx2.Im);
                Fx = Plural.PluralMultiplyDouble(expi4, Math.Sqrt(Math.PI * x)) - j2sqrtxMultiplyexMultiplyintegralejx2;
            }
            else if (10 < x)//当x大于10时
            {
                Fx.Re = 1 - 0.75 / Math.Pow(x, 4) + 105 / (Math.Pow(x, 8) * 16);
                Fx.Im = 0.5 / Math.Pow(x, 2) - 15 / (Math.Pow(x, 6) * 8);
            }
            else//若x小于0时
            {
                Fx = null;
            }
            return Fx;
        }

        /// <summary> 
        /// 求距离参数L，文档公式7
        /// </summary>
        private static double GetDistanceParameter(Point originPoint, Point diffractionPoint, AdjacentEdge sameEdge, Point viewPoint, double waveLength)
        {
            SpectVector rayVector = new SpectVector(originPoint, diffractionPoint);
            double angle = SpectVector.VectorPhase(rayVector, sameEdge.LineVector) * Math.PI / 180;//入射射线与公共棱的夹角
            if (((Math.PI / 2) < angle) && (angle < Math.PI))
            { angle = Math.PI - angle; }
            double s1 = originPoint.GetDistance(diffractionPoint);//辐射源到绕射点的距离
            double s2 = diffractionPoint.GetDistance(viewPoint);//绕射点到观察点的距离
            double distanceOfOriginAndSameLine = originPoint.GetDistanceToLine(sameEdge.StartPoint, sameEdge.LineVector);//辐射源到公共棱的距离
            double L;//距离参数
            if (distanceOfOriginAndSameLine > (waveLength * 10))
            {
                L = s2 * Math.Pow(Math.Sin(angle), 2);//平面波入射时的距离参数
            }
            else
            {
                L = s1 * s2 * Math.Pow(Math.Sin(angle), 2) / (s1 + s2);//球面波入射时的距离参数
            }
            return L;
            //柱面波没有写，以后需要可补上
        }

        /// <summary> 
        /// 将一个有方向的场强用XYZ坐标表示
        /// </summary>
        private static EField GetXYZComponentOfTotalEField(Plural totalEField, SpectVector vectorOfEField)
        {
            EField componentEField = new EField();
            double XComponent = vectorOfEField.a / (Math.Sqrt(Math.Pow(vectorOfEField.a, 2) + Math.Pow(vectorOfEField.b, 2) + Math.Pow(vectorOfEField.c, 2)));
            double YComponent = vectorOfEField.b / (Math.Sqrt(Math.Pow(vectorOfEField.a, 2) + Math.Pow(vectorOfEField.b, 2) + Math.Pow(vectorOfEField.c, 2)));
            double ZComponent = vectorOfEField.c / (Math.Sqrt(Math.Pow(vectorOfEField.a, 2) + Math.Pow(vectorOfEField.b, 2) + Math.Pow(vectorOfEField.c, 2)));
            componentEField.X = Plural.PluralMultiplyDouble(totalEField, XComponent);
            componentEField.Y = Plural.PluralMultiplyDouble(totalEField, YComponent);
            componentEField.Z = Plural.PluralMultiplyDouble(totalEField, ZComponent);
            return componentEField;
        }

        /// <summary>
        ///求得电场的垂直分量的方向
        /// </summary>
        private static SpectVector GetVectorOfVerticalEField(SpectVector k, SpectVector l)  //k为射线传播方向，l为反射中的反射面的法向量或绕射中的与棱平行的向量
        {
            SpectVector n = SpectVector.VectorCrossMultiply(k, l);
            return n;
        }

        /// <summary>
        ///求得电场的水平分量的方向
        /// </summary>
        private static SpectVector GetVectorOfHorizonalEField(SpectVector k, SpectVector l)
        {
            SpectVector n = SpectVector.VectorCrossMultiply(k, l);   //入射面的法向量
            SpectVector m = SpectVector.VectorCrossMultiply(k, n);
            return m;
        }
       
        /// <summary>
        ///求两个三角面的夹角
        /// </summary>
        private static double GetAngleOfTwoTers(Face face1, Face face2, AdjacentEdge sameEdge)
        {
            //相邻三角面另外两个独立点
            Point otherPoint1 = face1.GetPointsOutOfTheLine( sameEdge)[0];
            Point otherPoint2 = face2.GetPointsOutOfTheLine(sameEdge)[0];
            SpectVector face1Vector1 = new SpectVector(otherPoint1, sameEdge.StartPoint);
            SpectVector face1Vector2 = new SpectVector(otherPoint1, sameEdge.EndPoint);
            SpectVector normalVector1 = SpectVector.VectorCrossMultiply(face1Vector1, face1Vector2);//获得第一个三角面的法向量
            SpectVector face2Vector1 = new SpectVector(otherPoint2, sameEdge.StartPoint);
            SpectVector face2Vector2 = new SpectVector(otherPoint2, sameEdge.EndPoint);
            SpectVector normalVector2 = SpectVector.VectorCrossMultiply(face2Vector1, face2Vector2);//获得第二个三角面的法向量
            double angleOfTers = SpectVector.VectorPhase(normalVector1, normalVector2);//两个法向量的夹角
            return angleOfTers;
        }

       /// <summary>
       /// 求绕射的入射面
       /// </summary>
       /// <param name="diffractionFace1">绕射棱的第一个面</param>
       /// <param name="diffractionFace2">绕射棱的第二个面</param>
       /// <param name="diffractionRayFace">入射射线与棱构成的面</param>
       /// <param name="sameEdge">棱</param>
       /// <returns>入射面</returns>
        private static Face GetTheFrontFace(Face diffractionFace1, Face diffractionFace2, Face rayFace, AdjacentEdge sameEdge)
        {
            double angleOfDiffraction1FaceAndRayFace = GetAngleOfTwoTers(diffractionFace1, rayFace, sameEdge);
            double angleOfDiffraction2FaceAndRayFace = GetAngleOfTwoTers(diffractionFace2, rayFace, sameEdge);
            if (angleOfDiffraction1FaceAndRayFace < angleOfDiffraction2FaceAndRayFace)
            {
                return diffractionFace1;
            }
            return diffractionFace2;
        }

        #endregion
   }
}
