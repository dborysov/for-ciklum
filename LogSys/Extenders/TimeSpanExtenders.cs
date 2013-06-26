#region Usings

using System;

#endregion

namespace LogSys.Extenders
{
    public static class TimeSpanExtenders
    {
        public static string ToHoursAndMinutes(this TimeSpan time)
        {
            return time.TotalMinutes > 0
                       ? time.TotalHours >= 1
                             ? string.Format("{0} hour(s), {1} minute(s)", Math.Floor(time.TotalHours), time.Minutes)
                             : string.Format("{0} minute(s)", time.Minutes)
                       : "-";
        }
    }
}