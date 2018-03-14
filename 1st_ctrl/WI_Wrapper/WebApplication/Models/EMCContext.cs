using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class EMCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public EMCContext() : base("name=EMCContext")
        {
        }

        public System.Data.Entity.DbSet<WebApplication.Models.Antenna> Antennae { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.FileInfo> FileInfoes { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.Receiver> Receivers { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.TaskFileRelation> TaskFileRelations { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.TaskInfo> TaskInfoes { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.Transmitter> Transmitters { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.Waveform> Waveforms { get; set; }

        public System.Data.Entity.DbSet<WebApplication.Models.TerInfo> TerInfoes { get; set; }
    
    }
}
