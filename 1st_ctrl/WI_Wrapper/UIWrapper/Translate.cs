using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.IO;
namespace WF1
{
    class Translate
    {
        //将combox中选择的项（既含有中文又含有英文）转化为英文字符串
        public static string KeyWordsDictionary(ComboBox combox)
        {
            string item = null;
            switch ((String)combox.SelectedItem)
            {
                //插入波形时 转化波形所对应的combox中的选择项
                case "正弦波<Sinusoid>":
                    item = "Sinusoid";
                    break;
                case "升余弦<Raised cosine>":
                    item = "RaisedCosine";
                    break;
                case "根号升余弦<Root raised cosine>":
                    item = "RootRaisedCosine";
                    break;
                case "高斯信号<Gaussian>":
                    item = "Gaussian";
                    break;
                case "离散高斯信号<Gaussian derivative>":
                    item = "GaussianDerivative";
                    break;
                case "布莱曼波<Blackman>":
                    item = "Blackman";
                    break;
                case "线性调频波<Chirp>":
                    item = "Chirp";
                    break;
                case "海明波<Hamming>":
                    item = "Hamming";
                    break;
                case "线性变化<Linear>":
                    item = "Linear";
                    break;
                case "指数变化<Exponential>":
                    item = "Exponential";
                    break;



                case "偶极子天线<Short dipole>":
                    item = "type HalfWaveDipole";
                    break;
                case "单极天线<Short monopole>":
                    item = "type linear_monopole";
                    break;
                case "抛物面反射天线<Parabolic reflector>":
                    item = "type ParabolicReflector";
                    break;
                case "螺旋天线<Helical>":
                    item = "type Helical";
                    break;
                case "对数周期天线<Log-periodical>":
                    item = "type Log-periodical";
                    break;

                case "水平极化<Horizontal>":
                    item = "horizontal";
                    break;
                case "垂直极化<Vertical>":
                    item = "vertical";
                    break;
                    


                case "左旋圆极化Left-hand circular":
                    item = "LeftCircular";
                    break;
                case "右旋圆极化Right-hand circular":
                    item = "RightCircular";
                    break;


                case "地平面":
                    item = "terrain";
                    break;
                case "海平面":
                    item = "sealevel";
                    break;

                case "点状<Points>":
                    item = "begin_<points> ";
                    break;
                case "区域状<XYgrid>":
                    item = "begin_<grid> ";
                    break;
                
            }
            return item;
        }
        public static string KeyWordsDictionary_DB(string transStr)
        {
            string Item = null;
            switch (transStr)
            {               
                case "水平极化<Horizontal>":
                    Item = "horizontal";
                    break;

                case "垂直极化<Vertical>":
                    Item = "vertical";
                    break;
                case "horizontal":
                    Item = "水平极化<Horizontal>";
                    break;

                case "左旋圆极化Left-hand circular":
                    Item = "LeftCircular";

                    break;

                case "vertical":
                    Item = "垂直极化<Vertical>";
                    break;

                case "右旋圆极化Right-hand circular":
                    Item = "RightCircular";
                    break;



                case "LeftCircular":
                    Item = "左旋圆极化Left-hand circular";
                    break;
                case "RightCircular":
                    Item = "右旋圆极化Right-hand circular";
                    break;


                case"type HalfWaveDipole":
                    Item = "偶极子天线<Short dipole>";
                        break;
                case "type linear_monopole":
                        Item = "单极天线<Short monopole>";
                    break;
                case "type ParabolicReflector":
                    Item = "抛物面反射天线<Parabolic reflector>";
                    break;
                case "type Helical":
                    Item = "螺旋天线<Helical>";
                    break;
                case "type Log-periodical":
                    Item = "对数周期天线<Log-periodical>";
                    break;
            }
            return Item;
        }
    }
}
