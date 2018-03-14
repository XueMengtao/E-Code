using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPIinHost
{
    public class ProjectResult
    {
        public int projectID;
        public string projectName;
        public string resultDirectory;
        public ProjectResult(int projectId, string projectName, string resultDirectory)
        {
            this.projectID = projectId;
            this.projectName = projectName;
            this.resultDirectory = resultDirectory;
        }
    }
}
