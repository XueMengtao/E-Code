using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using System.IO;

namespace RayCalInfo
{
    public class ReflectEfieldCal
    {
        public const int CSpeed = 300;
        public double permeability = 4 * 0.0000001 * Math.PI;//求磁导率
        RayInfo rayIn;
        SpectVector verticalVector;//法向向量
        Face intersectionFace;
        double inputFrequency;
        double inDistance, outDistance;
        double Epara = 1;//初始化默认相对介电常数为1
        double outerAlphaConstant;
        double reflectAngle;
        EField e;
        //求反射电场强度的垂直分量
        private static EField VerticalEfield(EField e, RayInfo rayIn, SpectVector l, double ReflectAngle, double Conduct, double Epara, double s1, double s2, double f)
        {
            Plural VEi = EField.GetVerticalE(e, rayIn.RayVector, l);
            Plural V = VerticalReflectance(ReflectAngle, Conduct, f, Epara);
            Plural A = new Plural(s1 / (s1 + s2));
            Plural E = VEi * V * A * GetPhase(s1, s2, f);
            EField Vefield = SpectVector.VectorDotMultiply(E, SpectVector.VectorCrossMultiply(rayIn.RayVector, l));
            return Vefield;
        }

        //求反射电场强度的水平分量
        private static EField HorizonalEfield(EField e, RayInfo rayIn, RayInfo rayOut, SpectVector l, double ReflectAngle, double Conduct, double Epara, double s1, double s2, double f)
        {
            Plural HEi = EField.GetHorizonalE(e, rayIn.RayVector, l);
            Plural H = HorizonalReflectance(ReflectAngle, Conduct, f, Epara);
            Plural A = new Plural(s1 / (s1 + s2));
            Plural E = HEi * H * A * GetPhase(s1, s2, f);
            SpectVector l1 = SpectVector.VectorCrossMultiply(rayOut.RayVector, SpectVector.VectorCrossMultiply(rayIn.RayVector, l));
            EField Hefield = SpectVector.VectorDotMultiply(E, l1);
            return Hefield;
        }

        //获得相位
        private static Plural GetPhase(double s1, double s2, double f)
        {
            double WaveLength = CSpeed / f;
            double k = 2 * Math.PI / WaveLength;
            Plural phase = new Plural(Math.Cos(k * (s1 + s2)), Math.Sin(k * (s1 + s2)));
            return phase;
        }


        //求垂直反射系数
        static public Plural VerticalReflectance(double ReflectAngle, double Conduct, double f, double Epara)
        {
            Plural Vreflectance = new Plural();
            double a, b, A, B, C;
            double WaveLength = CSpeed / f;
            Plural temp = new Plural();  //反射系数化简后的复数形式
            temp.Re = Epara - Math.Sin(ReflectAngle * Math.PI / 180) * Math.Sin(ReflectAngle * Math.PI / 180);
            temp.Im = 60 * Conduct * WaveLength;
            a = Plural.PluralSqrt(temp).Re;  //求反射系数的中间量
            b = Plural.PluralSqrt(temp).Im;  //求反射系数的中间量
            A = Math.Cos(ReflectAngle * Math.PI / 180) * Math.Cos(ReflectAngle * Math.PI / 180) - a * a - b * b;
            B = (a + Math.Cos(ReflectAngle * Math.PI / 180)) * (a + Math.Cos(ReflectAngle * Math.PI / 180)) + b * b;
            Vreflectance.Re = A / B;
            C = -2 * b * Math.Cos(ReflectAngle * Math.PI / 180);
            Vreflectance.Im = C / B;
            return Vreflectance;
        }

        //求水平反射系数
        static public Plural HorizonalReflectance(double ReflectAngle, double Conduct, double f, double Epara)
        {
            Plural Hreflectance = new Plural();
            double a, b, A, B, C;
            double WaveLength = CSpeed / f;
            Plural temp = new Plural();  //反射系数化简后的复数形式
            temp.Re = Epara - Math.Sin(ReflectAngle * Math.PI / 180) * Math.Sin(ReflectAngle * Math.PI / 180);
            temp.Im = 60 * Conduct * WaveLength;
            double temp1 = 60 * Conduct * WaveLength * Math.Cos(ReflectAngle * Math.PI / 180);
            a = Plural.PluralSqrt(temp).Re;
            b = Plural.PluralSqrt(temp).Im;
            A = Epara * Epara * Math.Cos(ReflectAngle * Math.PI / 180) * Math.Cos(ReflectAngle * Math.PI / 180) - a * a - b * b + temp1 * temp1;
            B = (Epara * Math.Cos(ReflectAngle * Math.PI / 180) + a) * (Epara * Math.Cos(ReflectAngle * Math.PI / 180) + a) + (b - temp1) * (b - temp1);
            Hreflectance.Re = A / B;
            C = -2 * b * Epara * Math.Cos(ReflectAngle * Math.PI / 180) - 2 * temp1 * a;
            Hreflectance.Im = C / B;
            return Hreflectance;
        }

        //求反射电场总值
        static public EField ReflectEfield(EField e, RayInfo rayIn, RayInfo rayOut, SpectVector l, double Conduct, double Epara, double s1, double s2, double f)
        {
            if (rayIn.RayVector.IsParallel(l))
            {
                Plural horiValue = HorizonalReflectance(0, Conduct, f, Epara);
                Plural A = new Plural(s1 / (s1 + s2));
                Plural Xtemp = e.X * horiValue * A * GetPhase(s1, s2, f);
                Plural Ytemp = e.Y * horiValue * A * GetPhase(s1, s2, f);
                Plural Ztemp = e.Z * horiValue * A * GetPhase(s1, s2, f);
                return new EField(Xtemp, Ytemp, Ztemp);
            }
            else
            {
                double ReflectAngle = SpectVector.VectorPhase(rayIn.RayVector, l);
                if (Math.Abs(ReflectAngle) >= 90)
                    ReflectAngle = 180 - Math.Abs(ReflectAngle);
                EField E = new EField();
                EField VerticalE = VerticalEfield(e, rayIn, l, ReflectAngle, Conduct, Epara, s1, s2, f);
                EField HorizonalE = HorizonalEfield(e, rayIn, rayOut, l, ReflectAngle, Conduct, Epara, s1, s2, f);
                E.X.Re = VerticalE.X.Re + HorizonalE.X.Re;
                E.X.Im = VerticalE.X.Im + HorizonalE.X.Im;
                E.Y.Re = VerticalE.Y.Re + HorizonalE.Y.Re;
                E.Y.Im = VerticalE.Y.Im + HorizonalE.Y.Im;
                E.Z.Re = VerticalE.Z.Re + HorizonalE.Z.Re;
                E.Z.Im = VerticalE.Z.Im + HorizonalE.Z.Im;
                //string pathtest = "D:\\renwu\\反射" + DateTime.Today.ToString("yy/MM/dd") + ".txt";
                //File.WriteAllText(pathtest, E.X.Re + "  " + E.X.Im + " | " + E.Y.Re + "  " + E.Y.Im + " | " + E.Z.Re + "  " + E.Z.Im);
                return E;
            }
        }
 

        public ReflectEfieldCal()
        {
        }
        public ReflectEfieldCal(RayInfo rayIn, Face intersectionFace, double inputFrequency, EField efield, double outDistance)
        {
            this.rayIn = rayIn;
            this.intersectionFace = intersectionFace;
            this.inputFrequency = inputFrequency;
            this.inDistance = rayIn.Origin.GetDistance(rayIn.GetCrossPointBetweenStraightLineAndFace(intersectionFace));
            this.outDistance = outDistance;
            this.Epara = intersectionFace.Material.DielectricLayer[1].Permittivity / intersectionFace.Material.DielectricLayer[0].Permittivity;
            this.e = efield;
            //衰减常数公式，见《电磁场与电磁波》（焦其祥主编） 192页,透射波所在介质的衰减常数
            this.outerAlphaConstant = 2 * Math.PI * inputFrequency * Math.Sqrt(permeability * intersectionFace.Material.DielectricLayer[0].Permittivity / 2 * (Math.Sqrt(1 + Math.Pow(intersectionFace.Material.DielectricLayer[0].Conductivity, 2) / (Math.Pow(2 * Math.PI * inputFrequency, 2) * Math.Pow(intersectionFace.Material.DielectricLayer[0].Permittivity, 2)
)) - 1));//入射波所在介质的衰减常数

            this.reflectAngle = rayIn.RayVector.GetPhaseOfVector(intersectionFace.NormalVector);//角度值
            this.verticalVector = intersectionFace.NormalVector;
        }

        public Boolean HasRay()
        {
            //如果入射射线和当前平面无交点，则不会有反射波。
            if (rayIn.GetCrossPointWithFace(intersectionFace) == null)
                return false;
            //布鲁斯特角入射时(入射波和布鲁斯特角度比较差值在0.0001范围内可认为相等)，电场的垂直极化分量为零，同样不存在反射波
            else if (Math.Abs(reflectAngle - Math.Atan(Math.Sqrt(Epara) * 180 / Math.PI)) < 0.0001 && this.GetVerticalEfield().X.Re == 0 && this.GetVerticalEfield().Y.Re == 0 && this.GetVerticalEfield().Z.Re == 0)
                return false;
            else
                return true;
        }

        public RayInfo GetRayOut()
        {
            if (rayIn.RayVector.IsParallelAndSamedirection(verticalVector))//判断向量是否和面垂直
            {//垂直时求得入射射线的反向向量即为反射向量
                return new RayInfo(rayIn.GetCrossPointWithFace(intersectionFace), rayIn.RayVector.GetReverseVector());
            }
            if (Math.Abs(rayIn.RayVector.DotMultiplied(verticalVector)) < 0.00001)//判断向量是否和面平行
                return null;
            SpectVector rotationVector = rayIn.RayVector.CrossMultiplied(verticalVector).GetReverseVector();
            double rotationAngle = 180 - 2 * reflectAngle;
            SpectVector vectorOfRfraction = rayIn.RayVector.GetRightRotationVector(rotationVector, rotationAngle);//调用向量绕旋转轴向量右旋公式
            return new RayInfo(rayIn.GetCrossPointWithFace(intersectionFace), vectorOfRfraction);
        }

        private Plural GetPhase()
        {
            double WaveLength = CSpeed / inputFrequency;
            double k = 2 * Math.PI / WaveLength;
            return new Plural(Math.Cos(k * outDistance), Math.Sin(k * outDistance));
        }

        //求垂直反射系数
        public Plural GetVerticalRefractance()
        {
            Plural Vreflectance = new Plural();
            double valueOfRe, valueOfIm, aPart, bPart, cPart;
            double WaveLength = CSpeed / inputFrequency;
            Plural temp = new Plural();  //反射系数化简后的复数形式
            temp.Re = Epara - Math.Sin(reflectAngle * Math.PI / 180) * Math.Sin(reflectAngle * Math.PI / 180);
            temp.Im = 60 * intersectionFace.Material.DielectricLayer[1].Conductivity * WaveLength;
            valueOfRe = Plural.PluralSqrt(temp).Re;  //求反射系数的中间量
            valueOfIm = Plural.PluralSqrt(temp).Im;  //求反射系数的中间量
            aPart = Math.Cos(reflectAngle * Math.PI / 180) * Math.Cos(reflectAngle * Math.PI / 180) - Math.Pow(valueOfRe, 2) - Math.Pow(valueOfIm, 2);//
            bPart = (valueOfRe + Math.Cos(reflectAngle * Math.PI / 180)) * (valueOfRe + Math.Cos(reflectAngle * Math.PI / 180)) + valueOfIm * valueOfIm;
            Vreflectance.Re = aPart / bPart;
            cPart = -2 * valueOfIm * Math.Cos(reflectAngle * Math.PI / 180);
            Vreflectance.Im = cPart / bPart;
            return Vreflectance;
        }

        //求水平反射系数
        public Plural GetHorizonalRefractance()
        {
            Plural Hreflectance = new Plural();
            double valueOfRe, valueOfIm, aPart, bPart, cPart;
            double WaveLength = CSpeed / inputFrequency;
            Plural temp = new Plural();  //反射系数化简后的复数形式
            temp.Re = Epara - Math.Sin(reflectAngle * Math.PI / 180) * Math.Sin(reflectAngle * Math.PI / 180);
            temp.Im = 60 * intersectionFace.Material.DielectricLayer[1].Conductivity * WaveLength;
            double temp1 = 60 * intersectionFace.Material.DielectricLayer[1].Conductivity * WaveLength * Math.Cos(reflectAngle * Math.PI / 180);
            valueOfRe = Plural.PluralSqrt(temp).Re;
            valueOfIm = Plural.PluralSqrt(temp).Im;
            aPart = Epara * Epara * Math.Cos(reflectAngle * Math.PI / 180) * Math.Cos(reflectAngle * Math.PI / 180) - Math.Pow(valueOfRe, 2) - Math.Pow(valueOfIm, 2) + temp1 * temp1;
            bPart = (Epara * Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe) * (Epara * Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe) + (valueOfIm - temp1) * (valueOfIm - temp1);
            Hreflectance.Re = aPart / bPart;
            cPart = -2 * valueOfIm * Epara * Math.Cos(reflectAngle * Math.PI / 180) - 2 * temp1 * valueOfRe;
            Hreflectance.Im = cPart / bPart;
            return Hreflectance;
        }

        public EField GetVerticalEfield()
        {
            //当入射波和相交面垂直时，求水平分量和垂直分量没有意义
            if (rayIn.RayVector.IsParallelAndSamedirection(intersectionFace.NormalVector))
                return null;
            //原static语句：double reflectAngle = SpectVector.VectorPhase(rayIn.RayVector, l);
            Plural verticalOfE = e.GetVerticalE(rayIn.RayVector, verticalVector);
            Plural vertiValue = GetVerticalRefractance();
            Plural A = new Plural(inDistance / (inDistance + outDistance));
            Plural E = verticalOfE * vertiValue * A * GetPhase();

            // 原static语句：Efield Vefield = SpectVector .VectorDotMultiply (E, VectorInSpace.VectorCrossMultiply (rayIn .RayVector ,l ));
            SpectVector tempVector = rayIn.RayVector.CrossMultiplied(verticalVector).GetNormalizationVector();
            return tempVector.DotMultiplied(E);
        }
        public EField GetHorizonalEfield()
        {
            if (rayIn.RayVector.IsParallelAndSamedirection(intersectionFace.NormalVector))
                return null;
            Plural horizonOfE = e.GetHorizonalE(rayIn.RayVector, verticalVector);
            Plural horiValue = GetHorizonalRefractance();
            Plural A = new Plural(inDistance / (inDistance + outDistance));
            Plural E = horizonOfE * horiValue * A * GetPhase();

            //原static语句：VectorInSpace l1 = VectorInSpace.VectorCrossMultiply(rayOut.RayVector, VectorInSpace.VectorCrossMultiply(rayIn.RayVector, l));
            SpectVector temp1 = rayIn.RayVector.CrossMultiplied(verticalVector);
            SpectVector temp2 = GetRayOut().RayVector;
            SpectVector tempVector = temp2.CrossMultiplied(temp1).GetNormalizationVector();
            return tempVector.DotMultiplied(E);
        }

        public EField GetEfield()
        {
            if (rayIn.RayVector.IsParallelAndSamedirection(intersectionFace.NormalVector))
            {
                Plural horiValue = GetHorizonalRefractance();
                Plural A = new Plural(inDistance / (inDistance + outDistance));
                Plural Xtemp = e.X * horiValue * A * GetPhase();
                Plural Ytemp = e.Y * horiValue * A * GetPhase();
                Plural Ztemp = e.Z * horiValue * A * GetPhase();
                return new EField(Xtemp, Ytemp, Ztemp);
            }
            else
            {
                Plural Xtemp = new Plural();
                Xtemp.Re = GetVerticalEfield().X.Re + GetHorizonalEfield().X.Re;
                Xtemp.Im = GetVerticalEfield().X.Im + GetHorizonalEfield().X.Im;
                Plural Ytemp = new Plural();
                Ytemp.Re = GetVerticalEfield().Y.Re + GetHorizonalEfield().Y.Re;
                Ytemp.Im = GetVerticalEfield().Y.Im + GetHorizonalEfield().Y.Im;
                Plural Ztemp = new Plural();
                Ztemp.Re = GetVerticalEfield().Z.Re + GetHorizonalEfield().Z.Re;
                Ztemp.Im = GetVerticalEfield().Z.Im + GetHorizonalEfield().Z.Im;
                return new EField(Xtemp, Ytemp, Ztemp);
            }
        }

    }
}
