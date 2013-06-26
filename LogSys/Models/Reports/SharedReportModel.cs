#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogSys.Enums;

#endregion

namespace LogSys.Models
{
    public class SharedReportModel
    {
        [Display(Name = "Project name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project owner's name")]
        public string UserName { get; set; }

        public ReportPeriods Period { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public List<ReportRowModel> Report { get; set; }

    }
}