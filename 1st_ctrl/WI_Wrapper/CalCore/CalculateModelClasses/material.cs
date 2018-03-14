using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateModelClasses
{
    /// <summary>
    ///材料类
    /// </summary>
    public abstract class Material
    {
        protected int materialNum;//材料编号
        protected string materialType;//材料类型，即名称
        protected List<double> ambient;
        protected List<double> diffuse;
        protected List<double> specular;
        protected List<double> emission;
        protected double shininess;
        protected int layerNum;//材料分层的层数
        protected List<DielectricLayer> dielectricLayer;//分层材料属性
        

        public int MaterialNum
        {
            set { materialNum = value; }
            get { return materialNum; }
        }
        public string MaterialType
        {
            set { materialType = value; }
            get { return materialType; }
        }
        public List<double> Ambient
        {
            set { ambient = value; }
            get { return ambient; }
        }
        public List<double> Diffuse
        {
            set { diffuse = value; }
            get { return diffuse; }
        }
        public List<double> Specular
        {
            set { specular = value; }
            get { return specular; }
        }
        public List<double> Emission
        {
            set { emission = value; }
            get { return emission; }
        }
        public double Shininess
        {
            set { shininess = value; }
            get { return shininess; }
        }
        public int LayerNum
        {
            get { return this.layerNum; }
            set { this.layerNum = value; }
        }
        public List<DielectricLayer> DielectricLayer
        {
            get { return this.dielectricLayer; }
            set { this.dielectricLayer = value; }
        }
    }

    /// <summary>
    ///地形材料类
    /// </summary>
    public class TerMaterial : Material
    {
        public TerMaterial(string layerName,double conductivity ,double permittivity, double roughness, double thickness)
        {
            this.ambient = new List<double>();
            this.diffuse = new List<double>();
            this.specular = new List<double>();
            this.emission = new List<double>();
            this.dielectricLayer = new List<DielectricLayer>();
            this.dielectricLayer.Add(new DielectricLayer(layerName, conductivity, permittivity, roughness, thickness));
        }
    }

    /// <summary>
    ///建筑物材料类
    /// </summary>
    public class BuildingMaterial : Material
    {
        public BuildingMaterial()
        {
            this.ambient = new List<double>();
            this.diffuse = new List<double>();
            this.specular = new List<double>();
            this.emission = new List<double>();
            this.dielectricLayer = new List<DielectricLayer>();
        }
        
    }

    /// <summary>
    /// 分层材料属性类
    /// </summary>
    public class DielectricLayer
    {
        private string layerName;//分层材料的材料名称
        private double conductivity;//电导率
        private double permittivity;//介电常数
        private double roughness;//粗糙度
        private double thickness;//厚度
        public string LayerName
        {
            get { return layerName; }
        }
        public double Conductivity
        {
            get { return conductivity; }
        }
        public double Permittivity
        {
            get { return permittivity; }
        }
        public double Roughness
        {
            get { return roughness; }
        }
        public double Thickness
        {
            get { return thickness; }
        }
        public DielectricLayer()
        { }
        public DielectricLayer(string layerName, double conductivity, double permittivity, double roughness, double thickness)
        {
            this.layerName = layerName;
            this.permittivity = permittivity;
            this.conductivity = conductivity;
            this.roughness = roughness;
            this.thickness = thickness;
        }
    }
}
