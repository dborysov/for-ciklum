#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogSys.Enums;

#endregion

namespace LogSys.Models
{
    public class ReportRowModel
    {
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class ProjectReportFiltersModel
    {
        public int? ProjectId { get; set; }

        public ReportPeriods Period { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "User projects")]
        public List<ProjectModel> UserProjects { get; set; }
    }
}