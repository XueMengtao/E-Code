using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPIinHost
{
    public class TaskInfo
    {
        public int TaskInfoID { get; set; }
        public int TaskState { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string NodeIP { get; set; }
        public int ProjectID { get; set; }
    }
}
