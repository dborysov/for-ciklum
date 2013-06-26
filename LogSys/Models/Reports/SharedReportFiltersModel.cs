#region Usings

using System;
using LogSys.Enums;

#endregion

namespace LogSys.Models
{
    public class SharedReportFiltersModel
    {
        public string UserName { get; set; }
        public string ProjectName { get; set; }
        public ReportPeriods Period { get; set; }
        public DateTime EndDate { get; set; }

    }
}