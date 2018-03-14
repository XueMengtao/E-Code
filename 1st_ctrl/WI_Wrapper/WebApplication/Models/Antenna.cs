using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Antenna
    {
        public int AntennaID { get; set; }
        /// <summary>
        /// 天线名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 天线类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 最大增益
        /// </summary>
        public double MaxGain { get; set; }
        /// <summary>
        /// 极化方式
        /// </summary>
        public string Polarization { get; set; }
        /// <summary>
        /// 接收门限（dBm）
        /// </summary>
        public double ReceiverThreshold { get; set; }
        /// <summary>
        /// 发射线路损耗（dB）
        /// </summary>
        public double TransmissionLoss { get; set; }
        /// <summary>
        /// VSWR
        /// </summary>
        public double VSWR { get; set; }
        /// <summary>
        /// 噪声温度（K）
        /// </summary>
        public double Temperature { get; set; }
        /// <summary>
        /// 半径（m）
        /// </summary>
        public double Radius { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BlockageRadius { get; set; }
        /// <summary>
        /// 孔径分布方式
        /// </summary>
        public string ApertureDistribution { get; set; }
        /// <summary>
        /// EDGE
        /// </summary>
        public double EdgeTeper { get; set; }
        /// <summary>
        /// 长度（米）
        /// </summary>
        public double Length { get; set; }
        public double Pitch { get; set; }
    }
}