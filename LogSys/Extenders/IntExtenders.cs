#region Usings

using System;

#endregion

namespace LogSys.Extenders
{
    public static class IntExtenders
    {
        public static string MinutesToHoursAndMinutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes).ToHoursAndMinutes();
        }
    }
}