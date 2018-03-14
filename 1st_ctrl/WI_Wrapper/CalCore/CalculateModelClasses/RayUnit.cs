using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///记录从某一个点发出的一条射线与其传播路径
    /// </summary>
    public class ClassNewRay : ICloneable
    {
        public ClassNewRay(Point point, SpectVector sv, bool fg, List<Path> path = null)
        {
            this.Origin = point;
            this.SVector = sv;
            this.Flag = fg;
            this.Path = path;
        }
        public ClassNewRay()
        { }
        Point origin;
        public Point Origin
        {
            set { this.origin = value; }
            get { return this.origin; }
        }
        SpectVector sVector;
        public SpectVector SVector
        {
            set { this.sVector = value; }
            get { return this.sVector; }
        }
        bool flag;
        public bool Flag
        {
            set { this.flag = value; }
            get { return this.flag; }
        }
        List<Path> path;
        public List<Path> Path
        {
            set { this.path = value; }
            get { return this.path; }
        }

        bool whetherHaveTraced;
        public bool WhetherHaveTraced
        {
            get { return this.whetherHaveTraced; }
            set { this.whetherHaveTraced = value; }
        }

        public Object Clone()
        {
            ClassNewRay param = new ClassNewRay();
            param.Origin = this.origin;
            param.SVector = this.sVector;
            param.WhetherHaveTraced = false;
            param.Flag = false;
            return param;
        }
    }


    /// <summary>
    ///射线处理单元，一个单元有3条射线，分别位于正三角形的3个顶点
    /// </summary>
    public class RayTracingModel
    {
        ClassNewRay firstRay;
        ClassNewRay secondRay;
        ClassNewRay thirdRay;
        double angleOfRayBeam;
        bool haveTraced;
        public ClassNewRay FirstRay
        {
            set { this.firstRay = value; }
            get { return this.firstRay; }
        }
        public ClassNewRay SecondRay
        {
            set { this.secondRay = value; }
            get { return this.secondRay; }
        }
        public ClassNewRay ThirdRay
        {
            set { this.thirdRay = value; }
            get { return this.thirdRay; }
        }

        public double AngleOfRayBeam
        {
            set { this.angleOfRayBeam = value; }
            get { return this.angleOfRayBeam; }
        }
        public bool HaveTraced
        {
            get { return this.haveTraced; }
            set { this.haveTraced = value; }
        }

        //构造函数
        public RayTracingModel()
        { }
        //构造函数
        public RayTracingModel(ClassNewRay firstRay, ClassNewRay secondRay, ClassNewRay thirdRay, bool haveTraced)
        {
            this.thirdRay = thirdRay;
            this.firstRay = firstRay;
            this.secondRay = secondRay;
            this.haveTraced = haveTraced;
            GetAngleOfRayBeam();
        }
        //构造函数
        public RayTracingModel(ClassNewRay firstRay, ClassNewRay secondRay, ClassNewRay thirdRay)
        {
            this.thirdRay = thirdRay;
            this.firstRay = firstRay;
            this.secondRay = secondRay;
            GetAngleOfRayBeam();
            JudgeIfUnitHaveTraced();
        }
        private void GetAngleOfRayBeam()
        {
            angleOfRayBeam = SpectVector.VectorPhase(firstRay.SVector, secondRay.SVector);

        }
        private void JudgeIfUnitHaveTraced()
        {
            if (firstRay.WhetherHaveTraced && secondRay.WhetherHaveTraced && thirdRay.WhetherHaveTraced)
                haveTraced = true;
            else haveTraced = false;
        }
    }
}
