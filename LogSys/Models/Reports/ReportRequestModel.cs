#region Usings

using LogSys.Enums;

#endregion

namespace LogSys.Models
{
    public class ReportRequestModel
    {
        public int ProjectId { get; set; }
        public ReportPeriods Period { get; set; }

    }
}