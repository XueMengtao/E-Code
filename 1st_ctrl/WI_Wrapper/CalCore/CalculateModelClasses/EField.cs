using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///电场强度类
    /// </summary>
    public class EField : ICloneable
    {
        Plural Xvertical;
        Plural Yvertical;
        Plural Zvertical;
        public Plural X
        {
            get { return Xvertical; }
            set { Xvertical = value; }
        }
        public Plural Y
        {
            get { return Yvertical; }
            set { Yvertical = value; }
        }
        public Plural Z
        {
            get { return Zvertical; }
            set { Zvertical = value; }
        }
        public EField()
        {
            X = new Plural(0, 0);
            Y = new Plural(0, 0);
            Z = new Plural(0, 0);

        }
        public EField(Plural x, Plural y, Plural z)
        {
            X = x; Y = y; Z = z;
        }

        public SpectVector GetTotal()
        {
            return new SpectVector(X.Re, Y.Re, Z.Re);
        }
        //求得电场的垂直分量
        public static Plural GetVerticalE(EField e, SpectVector k, SpectVector l)  //k为射线传播方向，l为反射中的反射面的法向量或绕射中的与棱平行的向量
        {
            SpectVector n = SpectVector.VectorCrossMultiply(k, l);
            Plural p = SpectVector.VectorDotMultiply(e, n);
            return p;
        }
        //求得电场的水平分量
        public static Plural GetHorizonalE(EField e, SpectVector k, SpectVector l)
        {
            SpectVector n = SpectVector.VectorCrossMultiply(k, l);   //入射面的法向量
            SpectVector m = SpectVector.VectorCrossMultiply(k, n);
            Plural p = SpectVector.VectorDotMultiply(e, m);
            return p;
        }
        //求得电场的垂直分量
        //调用方法：thisEfield.GetVerticalE(k,l)
        public Plural GetVerticalE(SpectVector k, SpectVector l)  //k为射线传播方向，l为反射中的反射面的法向量或绕射中的与棱平行的向量
        {
            SpectVector n = k.CrossMultiplied(l).GetNormalizationVector();
            Plural p = n.DotMultiplied(this);
            return p;
        }
        //求得电场的水平分量
        //调用方法：thisEfield.GetVerticalE(k,l)
        public Plural GetHorizonalE(SpectVector k, SpectVector l)
        {
            SpectVector n = k.CrossMultiplied(l);   //入射面的法向量
            SpectVector m = k.CrossMultiplied(n).GetNormalizationVector();
            Plural p = m.DotMultiplied(this);
            return p;
        }
        //求电场幅值new
        public double Mag()
        {
            return Math.Sqrt(Math.Pow(Xvertical.GetMag(), 2) + Math.Pow(Yvertical.GetMag(), 2) + Math.Pow(Zvertical.GetMag(), 2));
        }
        public Plural GetTolEx(List<CalculateModelClasses.Path> RxPaths)
        {
            Plural TolEx = new Plural();
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                TolEx += path.node[path.node.Count - 1].TotalE.X;
            }
            return TolEx;
        }
        public Plural GetTolEy(List<CalculateModelClasses.Path> RxPaths)
        {
            Plural TolEy = new Plural();
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                TolEy += path.node[path.node.Count - 1].TotalE.Y;
            }
            return TolEy;
        }
        public Plural GetTolEz(List<CalculateModelClasses.Path> RxPaths)
        {
            Plural TolEz = new Plural();
            foreach (CalculateModelClasses.Path path in RxPaths)
            {
                TolEz += path.node[path.node.Count - 1].TotalE.Z;
            }
            return TolEz;
        }


        public Object Clone()
        {
            EField newEfield = new EField();
            newEfield.X = (Plural)this.Xvertical.Clone();
            newEfield.Y = (Plural)this.Y.Clone();
            newEfield.Z = (Plural)this.Zvertical.Clone();
            return newEfield;
        }

    }
}
