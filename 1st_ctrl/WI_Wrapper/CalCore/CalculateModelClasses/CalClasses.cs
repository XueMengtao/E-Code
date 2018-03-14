using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculateModelClasses;

namespace CalculateModelClasses
{

    /// <summary>
    ///记录射线与地形交点、与源点距离、所在面
    /// </summary>
    public class ReflectionPointAndFace
    {
        public Point CrossPoint;
        public Triangle ReflectionFace;
        public double DistanceToFrontPoint;
        public ReflectionPointAndFace(Point onePoint, Triangle tri, double distance = 0)
        {
            CrossPoint = onePoint;
            ReflectionFace = tri;
            DistanceToFrontPoint = distance;
        }

    }


    /// <summary>
    ///绕射转换矩阵，求逆等操作
    /// </summary>
    public class Matrix
    {
        double[,] m;
        int length, width;
        public Matrix(int l, int w)
        {
            length = l; width = w;
            m = new double[l, w];
        }
        public double this[int x, int y]
        {
            get
            {
                return m[x, y];
            }
            set
            {
                m[x, y] = value;
            }
        }
        public int Length
        {
            get { return length; }
        }
        public int Width
        {
            get { return width; }
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            //Console.WriteLine(" a:{0}X{1}  b:{2}X{3}", a.Length, a.Width, b.Length, b.Width);  

            if (a.Width != b.Length)
            {
                //  Console.WriteLine("error a:{0}X{1}  b:{2}X{3}", a.Length, a.Width, b.Length, b.Width);  
                return null;
            }
            Matrix c = new Matrix(a.Length, b.Width);
            for (int i = 0; i < c.Length; i++)
            {
                for (int j = 0; j < c.Width; j++)
                {
                    c[i, j] = 0;
                    for (int k = 0; k < a.Width; k++)
                    {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return c;
        }

        //三阶矩阵的行列式值
        public static double GetValue(Matrix A)
        {
            if (A.Length != A.Width) throw new Exception("非N阶矩阵");
            else if (A.Length != 3) throw new Exception("非三阶矩阵");
            else return (A[0, 0] * A[1, 1] * A[2, 2] + A[0, 1] * A[1, 2] * A[2, 0] + A[1, 0] * A[2, 1] * A[0, 2] - A[2, 0] * A[1, 1] * A[0, 2] - A[1, 0] * A[0, 1] * A[2, 2] - A[2, 1] * A[1, 2] * A[0, 0]);
        }
        //求逆  
        public static Matrix converse(Matrix m)
        {
            if (m.Length != m.Width)
            {
                return null;
            }
            if (GetValue(m) == 0) return null;
            //clone  
            Matrix a = new Matrix(m.Length, m.Width);
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    a[i, j] = m[i, j];
                }
            }
            Matrix c = new Matrix(a.Length, a.Width);
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    if (i == j) { c[i, j] = 1; }
                    else { c[i, j] = 0; }
                }
            }

            //i表示第几行，j表示第几列  
            for (int j = 0; j < a.Length; j++)
            {
                bool flag = false;
                for (int i = j; i < a.Length; i++)
                {
                    if (a[i, j] != 0)
                    {
                        flag = true;
                        double temp;
                        //交换i,j,两行  
                        if (i != j)
                        {
                            for (int k = 0; k < a.Length; k++)
                            {
                                temp = a[j, k];
                                a[j, k] = a[i, k];
                                a[i, k] = temp;

                                temp = c[j, k];
                                c[j, k] = c[i, k];
                                c[i, k] = temp;
                            }
                        }
                        //第j行标准化  
                        double d = a[j, j];
                        for (int k = 0; k < a.Length; k++)
                        {
                            a[j, k] = a[j, k] / d;
                            c[j, k] = c[j, k] / d;
                        }
                        //消去其他行的第j列  
                        d = a[j, j];
                        for (int k = 0; k < a.Length; k++)
                        {
                            if (k != j)
                            {
                                double t = a[k, j];
                                for (int n = 0; n < a.Length; n++)
                                {
                                    a[k, n] -= (t / d) * a[j, n];
                                    c[k, n] -= (t / d) * c[j, n];
                                }
                            }
                        }
                    }
                }
                if (!flag) return null;
            }
            return c;
        }
    }

    /// <summary>
    ///区域相交工具类
    /// </summary>
    public class AreaMeatUtil
    {
        public Point ray_cube1(RayInfo rayIn, ReceiveArea ra)//求与长方体的前面的交点；
        {
            double x = (ra.OriginPoint.Y - rayIn.Origin.Y) / rayIn.RayVector.b * rayIn.RayVector.a + rayIn.Origin.X;
            double z = (ra.OriginPoint.Y - rayIn.Origin.Y) / rayIn.RayVector.b * rayIn.RayVector.c + rayIn.Origin.Z;
            if (x >= ra.OriginPoint.X && x <= (ra.OriginPoint.X + ra.rxLength) && z >= ra.OriginPoint.Z && z <= (ra.OriginPoint.Z + ra.spacing))
            {
                Point temp = new Point(x, ra.OriginPoint.Y, z);
                return temp;
            }
            else
                return null;
        }
        public Point ray_cube2(RayInfo rayIn, ReceiveArea ra)//求与长方体的后面的交点；
        {

            double x = (ra.OriginPoint.Y + ra.rxWidth - rayIn.Origin.Y) / rayIn.RayVector.b * rayIn.RayVector.a + rayIn.Origin.X;
            double z = (ra.OriginPoint.Y + ra.rxWidth - rayIn.Origin.Y) / rayIn.RayVector.b * rayIn.RayVector.c + rayIn.Origin.Z;
            if (x >= ra.OriginPoint.X && x <= (ra.OriginPoint.X + ra.rxLength) && z >= ra.OriginPoint.Z && z <= (ra.OriginPoint.Z + ra.spacing))
            {
                Point temp = new Point(x, ra.OriginPoint.Y + ra.rxWidth, z);
                return temp;
            }
            else
                return null;
        }
        public Point ray_cube3(RayInfo rayIn, ReceiveArea ra)//求与长方体的左面的交点；
        {
            double y = (ra.OriginPoint.X - rayIn.Origin.X) / rayIn.RayVector.a * rayIn.RayVector.b + rayIn.Origin.Y;
            double z = (ra.OriginPoint.X - rayIn.Origin.X) / rayIn.RayVector.a * rayIn.RayVector.c + rayIn.Origin.Z;
            if (y > ra.OriginPoint.Y && y < (ra.OriginPoint.Y + ra.rxWidth) && z > ra.OriginPoint.Z && z < (ra.OriginPoint.Z + ra.spacing))
            {
                Point temp = new Point(ra.OriginPoint.X, y, z);
                return temp;
            }
            else
                return null;
        }
        public Point ray_cube4(RayInfo rayIn, ReceiveArea ra)//求与长方体的右面的交点；
        {
            double y = (ra.OriginPoint.X + ra.rxLength - rayIn.Origin.X) / rayIn.RayVector.a * rayIn.RayVector.b + rayIn.Origin.Y;
            double z = (ra.OriginPoint.X + ra.rxLength - rayIn.Origin.X) / rayIn.RayVector.a * rayIn.RayVector.c + rayIn.Origin.Z;
            if (y > ra.OriginPoint.Y && y < (ra.OriginPoint.Y + ra.rxWidth) && z > ra.OriginPoint.Z && z < (ra.OriginPoint.Z + ra.spacing))
            {
                Point temp = new Point(ra.OriginPoint.X + ra.rxLength, y, z);
                return temp;
            }
            else
                return null;
        }
        public Point ray_cube5(RayInfo rayIn, ReceiveArea ra)//求与长方体的下面的交点；
        {
            double x = (ra.OriginPoint.Z - rayIn.Origin.Z) / rayIn.RayVector.c * rayIn.RayVector.a + rayIn.Origin.X;
            double y = (ra.OriginPoint.Z - rayIn.Origin.Z) / rayIn.RayVector.c * rayIn.RayVector.b + rayIn.Origin.Y;
            if (x >= ra.OriginPoint.X && x <= (ra.OriginPoint.X + ra.rxLength) && y > ra.OriginPoint.Y && y < (ra.OriginPoint.Y + ra.rxWidth))
            {
                Point temp = new Point(x, y, ra.OriginPoint.Z);
                return temp;
            }
            else
                return null;
        }
        public Point ray_cube6(RayInfo rayIn, ReceiveArea ra)//求与长方体的上面的交点；
        {
            double x = (rayIn.Origin.Z + ra.spacing - rayIn.Origin.Z) / rayIn.RayVector.c * rayIn.RayVector.a + rayIn.Origin.X;
            double y = (rayIn.Origin.Z + ra.spacing - rayIn.Origin.Z) / rayIn.RayVector.c * rayIn.RayVector.b + rayIn.Origin.Y;
            if (x >= ra.OriginPoint.X && x <= (ra.OriginPoint.X + ra.rxLength) && y > ra.OriginPoint.Y && y < (ra.OriginPoint.Y + ra.rxWidth))
            {
                Point temp = new Point(x, y, ra.OriginPoint.Z + ra.spacing);
                return temp;
            }
            else
                return null;
        }
    }

    /// <summary>
    ///复数类
    /// </summary>
    public class Plural:ICloneable
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

        /// <summary>
        ///取模值
        /// </summary>
        public double GetMag()
        {
            return Math.Sqrt(Math.Pow(Re, 2) + Math.Pow(Im, 2));
        }

        /// <summary>
        ///取相位
        /// </summary>
        public double GetPhase()
        {
            if (Re == 0 && Im > 0)
                return Math.PI / 2;
            else if (Re == 0 && Im < 0)
                return Math.PI;
            else
                return (Math.Atan(Im / Re) * 180 / Math.PI);
        }

        /// <summary>
        ///求共轭复数函数
        /// </summary>
        public Plural GetConjugate()
        {
            return new Plural(Re, -Im);
        }

        /// <summary>
        ///复数表示方式转化函数
        /// </summary>
        public Plural Converse(double mag, double phase)
        {
            return new Plural(mag * Math.Cos(phase * Math.PI / 180), mag * Math.Sin(phase * Math.PI / 180));
        }

        /// <summary>
        ///加减乘除函数
        /// </summary>
        static public Plural operator +(Plural one, Plural two)
        {
            return new Plural(one.Re + two.Re, one.Im + two.Im);
        }
        static public Plural operator -(Plural one, Plural two)
        {
            return new Plural(one.Re - two.Re, one.Im - two.Im);
        }
        static public Plural operator *(Plural one, Plural two)
        {
            double temp1, temp2;
            temp1 = one.Re * two.Re - one.Im * two.Im;
            temp2 = one.Im * two.Re + one.Re * two.Im;
            return new Plural(temp1, temp2);
        }
        static public Plural operator /(Plural one, Plural two)
        {
            double temp1, temp2, temp3;
            temp1 = one.Re * two.Re + one.Im * two.Im;
            temp2 = one.Im * two.Re - one.Re * two.Im;
            temp3 = Math.Pow(two.Re, 2) + Math.Pow(two.Im, 2);
            return new Plural(temp1 / temp3, temp2 / temp3);
        }
        static public Plural operator /(Plural one, double two)
        {
            if (two == 0)
            {
                throw new Exception("除数不能为0");
            }
            return new Plural(one.Re / two, one.Im / two);
        }

        /// <summary>
        ///复数开根号运算
        /// </summary>
        public static Plural PluralSqrt(Plural p)
        {
            Plural p1 = new Plural();
            p1.Re = Math.Sqrt((Math.Sqrt(p.Re * p.Re + p.Im * p.Im) + p.Re) / 2);
            p1.Im = -Math.Sqrt((Math.Sqrt(p.Re * p.Re + p.Im * p.Im) - p.Re) / 2);
            return p1;
        }

        /// <summary>
        ///复数与int型数据乘法运算
        /// </summary>
        public static Plural PluralMultiplyDouble(Plural p, double k)
        {
            Plural p1 = new Plural();
            p1.Re = p.Re * k;
            p1.Im = p.Im * k;
            return p1;
        }

        public Object Clone()
        {
            Plural newPlural = new Plural();
            newPlural.Re = this.R;
            newPlural.Im = this.I;
            return newPlural;
        }
    }

    /// <summary>
    ///平面编号
    /// </summary>
    public class FaceID:ICloneable
    {
        private int firstID;//在地形面中表示Row，在建筑物中表示建筑物的编号
        private int secondID;//在地形面中表示Line，在建筑物中表示在单个建筑物中面的编号
        private int thirdID;//在地形面中表示在矩形中的编号，在建筑物中表示在单个建筑物中面内的编号

        public int FirstID
        {
            get { return this.firstID; }
            set { this.firstID = value; }
        }
        public int SecondID
        {
            get { return this.secondID; }
            set { this.secondID = value; }
        }
        public int ThirdID
        {
            get { return this.thirdID; }
            set { this.thirdID = value; }
        }


        public FaceID()
        { }
        public FaceID(int firstID, int secondID)
        {
            this.firstID = firstID;
            this.secondID = secondID;
        }

        public FaceID(int firstID, int secondID, int thirdID)
           : this(firstID,secondID)
        {
            this.thirdID = thirdID;
        }

        /// <summary>
        /// 判断两个编号是否相同
        /// </summary>
        public bool IsSameID(FaceID ohterID)
        {
            if (this.firstID == ohterID.FirstID && this.secondID == ohterID.SecondID && this.thirdID==ohterID.ThirdID)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// 判断两个编号是否相邻
        /// </summary>
        public bool IsAdjacent(FaceID otherID)
        {
            bool leftAdjacent = this.firstID == otherID.firstID - 1 && this.secondID == otherID.secondID;
            bool rightAdjacent = this.firstID == otherID.firstID + 1 && this.secondID == otherID.secondID;
            bool topAdjacent = this.firstID == otherID.firstID  && this.secondID == otherID.secondID - 1;
            bool downAdjacent = this.firstID == otherID.firstID  && this.secondID == otherID.secondID + 1;
            if (leftAdjacent || rightAdjacent || topAdjacent || downAdjacent)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// 判断两个编号是否相邻
        /// </summary>
        public bool IsAdjacent(FaceType faceType,FaceID otherID)
        {
            if (faceType == FaceType.Terrian)
            {

            }
            else if (faceType == FaceType.Building)
            { }
            bool leftAdjacent = this.firstID == otherID.firstID - 1 && this.secondID == otherID.secondID;
            bool rightAdjacent = this.firstID == otherID.firstID + 1 && this.secondID == otherID.secondID;
            bool topAdjacent = this.firstID == otherID.firstID && this.secondID == otherID.secondID - 1;
            bool downAdjacent = this.firstID == otherID.firstID && this.secondID == otherID.secondID + 1;
            return false;
        }

        public Object Clone()
        {
            return new FaceID(this.firstID, this.secondID,this.thirdID);
        }
    }

}
