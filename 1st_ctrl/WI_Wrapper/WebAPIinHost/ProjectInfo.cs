using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPIinHost
{
    public class ProjectInfo
    {
        //这个类名是避免和ProjectLib中的Project类重名
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int ProjectState { get; set; }
        public string Directory { get; set; }
        public string ResultDirectory { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Percentage { get; set; }
    }
}
