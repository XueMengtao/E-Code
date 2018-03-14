using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;
using System.IO;

namespace RayCalInfo
{
    public class RefractEfieldCal
    {
        public const int CSpeed = 300;
        public double permeability = 4 * 0.0000001 * Math.PI;//求磁导率
        RayInfo rayIn;
        SpectVector verticalVector;//法向向量
        Face intersectionFace;
        double inputFrequency;
        double inDistance, outDistance;
        double Epara = 1;//初始化默认相对介电常数为1
        double innerAlphaConstant, outerAlphaConstant;
        double reflectAngle;
        EField e;
        public RefractEfieldCal(RayInfo rayIn, Face intersectionFace, double inputFrequency, EField efield, double outDistance)
        {
            this.rayIn = rayIn;
            this.intersectionFace = intersectionFace;
            this.inputFrequency = inputFrequency;
            this.inDistance = rayIn.Origin.GetDistance(rayIn.GetCrossPointWithFace(intersectionFace));
            this.outDistance = outDistance;
            this.Epara = intersectionFace.Material.DielectricLayer[1].Permittivity / intersectionFace.Material.DielectricLayer[0].Permittivity;
            this.e = efield;
            this.innerAlphaConstant = 2 * Math.PI * inputFrequency * Math.Sqrt(permeability * intersectionFace.Material.DielectricLayer[1].Permittivity / 2 * (Math.Sqrt(1 + Math.Pow(intersectionFace.Material.DielectricLayer[1].Conductivity, 2) / (Math.Pow(2 * Math.PI * inputFrequency, 2) * Math.Pow(intersectionFace.Material.DielectricLayer[1].Permittivity, 2)
)) - 1));//衰减常数公式，见《电磁场与电磁波》（焦其祥主编） 192页,透射波所在介质的衰减常数
            this.outerAlphaConstant = 2 * Math.PI * inputFrequency * Math.Sqrt(permeability * intersectionFace.Material.DielectricLayer[0].Permittivity / 2 * (Math.Sqrt(1 + Math.Pow(intersectionFace.Material.DielectricLayer[0].Conductivity, 2) / (Math.Pow(2 * Math.PI * inputFrequency, 2) * Math.Pow(intersectionFace.Material.DielectricLayer[0].Permittivity, 2)
)) - 1));//入射波所在介质的衰减常数

            this.reflectAngle = rayIn.RayVector.GetPhaseOfVector(intersectionFace.NormalVector);//角度值
            this.verticalVector = intersectionFace.NormalVector;
        }

        public Boolean HasRay()
        {
            //如果入射射线和当前平面无交点，则不会有透射波。
            if (rayIn.GetCrossPointWithFace(intersectionFace) == null)
                return false;
            //如果由光密介质到光疏介质入射并且入射波大于入射角，或者入射的介质为金属(导电率为百万量级)，则不存在透射波。
            else if (intersectionFace.Material.DielectricLayer[1].Conductivity > 1000000 || (intersectionFace.Material.DielectricLayer[0].Permittivity > intersectionFace.Material.DielectricLayer[1].Permittivity) && (reflectAngle > Math.Asin(Math.Sqrt(Epara)) * 180 / Math.PI))
                return false;
            //布鲁斯特角入射时(入射波和布鲁斯特角度比较差值在0.0001范围内可认为相等)，电场的水平极化分量为零，同样不存在透射波
            else if (Math.Abs(reflectAngle - Math.Atan(Math.Sqrt(Epara) * 180 / Math.PI)) < 0.0001 && this.GetHorizonalEfield().X.Re == 0 && this.GetHorizonalEfield().Y.Re == 0 && this.GetHorizonalEfield().Z.Re == 0)
                return false;
            else
            {
                return true;
            }
        }

        public RayInfo GetRayOut()
        {
            if (rayIn.RayVector.IsParallelAndSamedirection(verticalVector))//判断向量是否和面垂直
                return new RayInfo(rayIn.GetCrossPointWithFace(intersectionFace), rayIn.RayVector);
            if (rayIn.RayVector.DotMultiplied(verticalVector) == 0)//判断向量是否和面平行
                return null;
            SpectVector rotationVector = rayIn.RayVector.CrossMultiplied(verticalVector);
            double temp = Math.Sin(reflectAngle * Math.PI / 180) / Math.Sqrt(Epara);
            double refractionAngle = Math.Asin(temp) / Math.PI * 180;
            double rotationAngle = reflectAngle - refractionAngle;
            SpectVector vectorOfRfraction = rayIn.RayVector.GetRightRotationVector(rotationVector, rotationAngle);//调用向量绕旋转轴向量右旋公式
            return new RayInfo(rayIn.GetCrossPointWithFace(intersectionFace), vectorOfRfraction);
        }
        private Plural GetPhase()
        {
            double WaveLength = CSpeed / inputFrequency;
            double k = 2 * Math.PI / WaveLength;
            return new Plural(Math.Cos(k * outDistance), Math.Sin(k * outDistance));
        }
        //透射系数公式参看http://192.168.1.160/svn/repos1/Documents/射线计算模块设计和理论文档/透射文档1.0.docx 第一页
        //求垂直透射系数
        public Plural GetVerticalRefractance()
        {
            Plural Vreflectance = new Plural();
            double valueOfRe, valueOfIm, aPart, bPart, cPart;
            double WaveLength = CSpeed / inputFrequency;
            Plural temp = new Plural();  //反射系数化简后的复数形式
            temp.Re = Epara - Math.Sin(reflectAngle * Math.PI / 180) * Math.Sin(reflectAngle * Math.PI / 180);
            temp.Im = 60 * intersectionFace.Material.DielectricLayer[1].Conductivity * WaveLength;
            valueOfRe = Plural.PluralSqrt(temp).Re;  //求透射系数的中间量
            valueOfIm = Plural.PluralSqrt(temp).Im;  //求透射系数的中间量
            aPart = 2 * Math.Cos(reflectAngle * Math.PI / 180) * (Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe);//
            bPart = (valueOfRe + Math.Cos(reflectAngle * Math.PI / 180)) * (valueOfRe + Math.Cos(reflectAngle * Math.PI / 180)) + valueOfIm * valueOfIm;
            Vreflectance.Re = aPart / bPart;
            cPart = -2 * valueOfIm * Math.Cos(reflectAngle * Math.PI / 180);
            Vreflectance.Im = cPart / bPart;
            return Vreflectance;
        }

        //求水平透射系数
        public Plural GetHorizonalRefractance()
        {
            Plural Hreflectance = new Plural();
            double valueOfRe, valueOfIm, aPart, bPart, cPart;
            double WaveLength = CSpeed / inputFrequency;
            Plural temp = new Plural();  //透射系数化简后的复数形式
            temp.Re = Epara - Math.Sin(reflectAngle * Math.PI / 180) * Math.Sin(reflectAngle * Math.PI / 180);
            temp.Im = 60 * intersectionFace.Material.DielectricLayer[1].Conductivity * WaveLength;
            double temp1 = 60 * intersectionFace.Material.DielectricLayer[1].Conductivity * WaveLength * Math.Cos(reflectAngle * Math.PI / 180);
            valueOfRe = Plural.PluralSqrt(temp).Re;
            valueOfIm = Plural.PluralSqrt(temp).Im;
            aPart = 2 * (valueOfRe * (Epara * Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe) + valueOfIm * (valueOfIm - temp1));//
            bPart = (Epara * Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe) * (Epara * Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe) + (valueOfIm - temp1) * (valueOfIm - temp1);
            Hreflectance.Re = aPart / bPart;
            cPart = 2 * (valueOfIm * (Epara * Math.Cos(reflectAngle * Math.PI / 180) + valueOfRe) - valueOfRe * (valueOfIm - temp1));//
            Hreflectance.Im = cPart / bPart;
            return Hreflectance;
        }

        //求透射电场强度的垂直分量by Zhuo Liu 2015,3,24:
        //private Efield GetVerticalEfield(Efield e, RayInfo rayIn, RayInfo rayOut, VectorInSpace lVector, double Conduct, double Epara, double inDistance, double outDistance, double inputFrequency)
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

        //求透射电场强度的水平分量
        //private Efield GetHorizonalEfield(Efield e, RayInfo rayIn, RayInfo rayOut, VectorInSpace lVector, double Conduct, double Epara, double inDistance, double outDistance, double inputFrequency)
        public EField GetHorizonalEfield()
        {
            //当入射波和相交面垂直时，求水平分量和垂直分量没有意义
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
            //当入射波和相交面垂直时，即和面法向向量平行，此时需单独讨论
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
