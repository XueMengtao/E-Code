using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectLib
{
    public class Plural
    {
        double R;
        double I;
        public double Re
        {
            get { return R; }
            set { R = value; }
        }//实部
        public double Im
        {
            get { return I; }
            set { I = value; }
        }//虚部

        //构造函数

        public Plural()
        {
            Re = Im = 0;
        }
        public Plural(double r, double i)
        {
            Re = r; Im = i;
        }
        public Plural(double r)
        {
            Re = r; Im = 0;
        }

        //取模值

        public double GetMag()
        {
            return Math.Sqrt(Math.Pow(Re, 2) + Math.Pow(Im, 2));
        }

        //取相位

        public double GetPhase()
        {
            if (Re == 0 && Im > 0)
                return Math.PI / 2;
            else if (Re == 0 && Im < 0)
                return Math.PI;
            else
                return (Math.Atan(Im / Re) * 180 / Math.PI);
        }

        //求共轭复数函数

        public Plural GetConjugate()
        {
            return new Plural(Re, -Im);
        }

        //复数表示方式转化函数

        public Plural Converse(double mag, double phase)
        {
            return new Plural(mag * Math.Cos(phase * Math.PI / 180), mag * Math.Sin(phase * Math.PI / 180));
        }

        //加减乘除函数

        static public Plural operator +(Plural one, Plural two)
        {
            return new Plural(one.Re + two.Re, one.Im + two.Im);
        }
    }
}
