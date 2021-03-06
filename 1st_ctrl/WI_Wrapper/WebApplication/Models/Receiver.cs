﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Receiver
    {
        public int ReceiverID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 天线旋转角度X
        /// </summary>
        public double RotateX { get; set; }
        /// <summary>
        /// 天线旋转角度Y
        /// </summary>
        public double RotateY { get; set; }
        /// <summary>
        /// 天线旋转角度Z
        /// </summary>
        public double RotateZ { get; set; }
        public double Power { get; set; }
        public string AntennaName { get; set; }
    }
}