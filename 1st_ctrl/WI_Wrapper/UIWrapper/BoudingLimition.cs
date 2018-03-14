using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
namespace WF1
{
    class BoudingLimition
    {
        //检查输入的str字符串是否为数字，true表示数字，否则输入的不是数字
        internal static bool IsFigure(string str)
        {
            string patten = @"^\d+\.?\d+$|^\d+$";
            bool b = Regex.IsMatch(str, patten);
            return b;
         }
        internal static bool IsScienceFigure(string str)
        {
            //string patten = @"^[+{0,1}|-]\d+\.?\d+$|^\de[+-]\d+$";
            string patten = @"^[\+\-]?[\d]+([\.][\d]*)?([Ee][+-]?[\d]+)?$";
            //string patten = @"^[\+\-]?[\d]+([\.][\d]*)?([Ee][+-])$";
            bool b = Regex.IsMatch(str, patten);
            return b;
        }
        //波形输入参数限定的方法
        public static bool PhaseLimition(TextBox phaseTextBox)
        {
            bool b = false;
            double Phase = 0.000;
            Phase = double.Parse(phaseTextBox.Text);
            if (Phase>180.000||Phase<-180.000)
            {
                b = true;
            }
            return b;    
        }
        public static bool Roll0ffFactorLimition(TextBox rolloffTextBox)
        {
            bool b = false;
            double RolloffFactor = 0.000;
            RolloffFactor = double.Parse(rolloffTextBox.Text);
            if (RolloffFactor > 1.000 || RolloffFactor < 0.000)
            {
                b = true;
            }
            return b;
        }
        public static bool PluseWidthLimition(TextBox pluseWidthTextBox,TextBox signalTimeTextBox)
        {
            bool b = false;
            double CarrierTime = 0.000;
            double PluseWidth = 0.000;
            CarrierTime = 1/(double.Parse(signalTimeTextBox.Text)*1000000.000);
            PluseWidth = double.Parse(pluseWidthTextBox.Text);
            if (PluseWidth < CarrierTime)
            {
                b = true;
            }
            return b;
        }
        public static bool EffectiveBandwidthLimition(TextBox bandWidthTextBox, TextBox carrierFrequency)
        {
            bool b = false;
            double BandWidth = 0.000;
            double CarrierFrequency = 0.000;
            BandWidth = double.Parse(bandWidthTextBox.Text);
            CarrierFrequency = double.Parse(carrierFrequency.Text)*2;
            if (BandWidth > CarrierFrequency)
            {
                b = true;
            }
            return b;
        }
        public static bool EffectiveBandwidthLimition_Blackman(TextBox plusWidthTextBox, TextBox carrierFrequency)
        {
            bool b = false;
            double BandWidth = 0.000;
            double CarrierFrequency = 0.000;
            BandWidth = 6/double.Parse(plusWidthTextBox.Text);
            CarrierFrequency = double.Parse(carrierFrequency.Text) * 2000000;
            if (BandWidth > CarrierFrequency)
            {
                b = true;
            }
            return b;
        }
        public static bool EffectiveBandwidthLimition_Hamming(TextBox plusWidthTextBox, TextBox carrierFrequency)
        {
            bool b = false;
            double BandWidth = 0.000;
            double CarrierFrequency = 0.000;
            BandWidth = 4/double.Parse(plusWidthTextBox.Text);
            CarrierFrequency = double.Parse(carrierFrequency.Text) * 2000000;
            if (BandWidth > CarrierFrequency)
            {
                b = true;
            }
            return b;
        }
        //天线输入参数限定的方法
        public static bool RotationLimition(TextBox rotationTextBox)
        {
            bool b = false;
            double Rotation = 0.000;
            Rotation = double.Parse(rotationTextBox.Text);
            if (Rotation > 360.000 || Rotation < 0.000)
            {
                b = true;
            }
            return b;
        }
        public static bool VSWRLimition(TextBox vswrTextBox)
        {
            bool b = false;
            double VSWR = 0.000;
            VSWR = double.Parse(vswrTextBox.Text);
            if (VSWR < 1.000)
            {
                b = true;
            }
            return b;
        }
        public static bool RadiusLimition(TextBox radiusTextBox, TextBox blockageRadiusTextBox)
        {
            bool b = false;
            double Radius = 0.000;
            double BlochageRadius = 0.000;
            Radius = double.Parse(radiusTextBox.Text)* 0.20;
            BlochageRadius = double.Parse(blockageRadiusTextBox.Text);
            if (BlochageRadius > Radius)
            {
                b = true;
            }
            return b;
        }
        
    }
   
}
