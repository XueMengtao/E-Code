using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class TaskFileRelation
    {
        public int TaskFileRelationID { get; set; }

        //外键
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskInfoID { get; set; }
        //public virtual TaskInfo TaskInfo { get; set; }
        /// <summary>
        /// 文件ID
        /// </summary>
        public int FileInfoID { get; set; }
        //public virtual FileInfo FileInfo { get; set; }
    }
}