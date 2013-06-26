#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

#endregion

namespace LogSys.DataAccess.DataContext
{
    public class LoggyContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
    }

    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserName { get; set; }

        public virtual ICollection<WorkLog> WorkLogs { get; set; }
    }

    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public int? ProjectId { get; set; }

        public Guid Guid { get; set; }

        public int PeriodId { get; set; }

        public DateTime EndDate { get; set; }

        public virtual Project Project { get; set; }
    }

    public class WorkLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string WorkDescription { get; set; }

        public DateTime LogDate { get; set; }

        public int LoggedMinutes { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual Project Project { get; set; }
    }
}
