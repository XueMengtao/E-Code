using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class TerInfo
    {
        public int TerInfoID { get; set; }
        /// <summary>
        /// 地形文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地形存放路径
        /// </summary>
        public string Path { get; set; }
    }
}