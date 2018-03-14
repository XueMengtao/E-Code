using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工程状态
        /// </summary>
        public int ProjectState { get; set; }
        /// <summary>
        /// 工程存储路径
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// 结果文件存储路径
        /// </summary>
        public string ResultDirectory { get; set; }
        /// <summary>
        /// 工程开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 工程结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 工程完成百分比
        /// </summary>
        public string Percentage { get; set; }

        //导航属性
        //public IList<TaskInfo> TaskInfos { get; set; }
    }
}