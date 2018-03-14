using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Waveform
    {
        public int WaveformID { get; set; }
        /// <summary>
        /// 波形名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 波形类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 载波频率（MHz）
        /// </summary>
        public double Frequency { get; set; }
        /// <summary>
        /// 带宽/脉冲宽度
        /// </summary>
        public double BandWidth { get; set; }
        /// <summary>
        /// 相位（度）
        /// </summary>
        public double Phase { get; set; }
        /// <summary>
        /// 起始频率（MHz）
        /// </summary>
        public double StartFrequency { get; set; }
        /// <summary>
        /// 截止频率（MHz）
        /// </summary>
        public double EndFrequency { get; set; }
        /// <summary>
        /// 滚降系数
        /// </summary>
        public double RollOffFactor { get; set; }
        /// <summary>
        /// 频率变化率
        /// </summary>
        public string FreChangeRate { get; set; }
    }
}