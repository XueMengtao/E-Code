using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class TaskInfo
    {
        public int TaskInfoID { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public int TaskState { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 节点IP信息
        /// </summary>
        public string NodeIP { get; set; }

        //外键
        /// <summary>
        /// 工程ID
        /// </summary>
        public int ProjectID { get; set; }
        //public Project Project { get; set; }
        //导航属性
        //public virtual IList<TaskFileRelation> TaskFileRelations { get; set; }
    }
}