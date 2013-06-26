#region Usings

using System;
using System.Linq;
using LogSys.DataAccess.DataContext;

#endregion

namespace LogSys.DataAccess
{
    static class QueryHelpers
    {
        #region ProjectFilters
        public static IQueryable<Project> ById(this IQueryable<Project> projects, int? id)
        {
            return id.HasValue
                       ? projects.Where(p => p.Id == id)
                       : projects;
        }

        public static IQueryable<Project> ByUserName(this IQueryable<Project> projects, string userName)
        {
            return !string.IsNullOrEmpty(userName)
                       ? projects.Where(p => p.UserName == userName)
                       : projects;
        }

        public static IQueryable<Project> ByProjectName(this IQueryable<Project> projects, string projectName)
        {
            return !string.IsNullOrEmpty(projectName)
                       ? projects.Where(p => p.Name == projectName)
                       : projects;
        }
        #endregion

        #region ReportsFilters
        public static IQueryable<Report> ById(this IQueryable<Report> reports, int? id)
        {
            return id.HasValue
                       ? reports.Where(p => p.Id == id)
                       : reports;
        }

        public static IQueryable<Report> ByUserName(this IQueryable<Report> reports, string userName)
        {
            return !string.IsNullOrEmpty(userName)
                       ? reports.Where(p => p.UserName == userName)
                       : reports;
        }

        public static IQueryable<Report> ByProjectId(this IQueryable<Report> reports, int? projectId)
        {
            return projectId.HasValue
                       ? reports.Where(p => p.ProjectId == projectId)
                       : reports;
        }

        public static IQueryable<Report> ByGuid(this IQueryable<Report> reports, Guid? guid)
        {
            return guid.HasValue
                       ? reports.Where(p => p.Guid == guid)
                       : reports;
        }

        public static IQueryable<Report> ByPeriodId(this IQueryable<Report> reports, int? periodId)
        {
            return periodId.HasValue
                       ? reports.Where(p => p.PeriodId == periodId)
                       : reports;
        }

        public static IQueryable<Report> ByEndDate(this IQueryable<Report> reports, DateTime? endDate)
        {
            return endDate.HasValue
                       ? reports.Where(p => p.EndDate == endDate)
                       : reports;
        }
        #endregion

        #region WorkLogsFilters
        public static IQueryable<WorkLog> ById(this IQueryable<WorkLog> logs, int? id)
        {
            return id.HasValue
                       ? logs.Where(p => p.Id == id)
                       : logs;
        }

        public static IQueryable<WorkLog> ByProjectId(this IQueryable<WorkLog> logs, int? projectId)
        {
            return projectId.HasValue
                       ? logs.Where(p => p.ProjectId == projectId)
                       : logs;
        }

        public static IQueryable<WorkLog> ByStartLogDate(this IQueryable<WorkLog> logs, DateTime? startLogDate)
        {
            return startLogDate.HasValue
                       ? logs.Where(p => p.LogDate >= startLogDate)
                       : logs;
        }

        public static IQueryable<WorkLog> ByEndLogDate(this IQueryable<WorkLog> logs, DateTime? endLogDate)
        {
            return endLogDate.HasValue
                       ? logs.Where(p => p.LogDate <= endLogDate)
                       : logs;
        }
        #endregion
    }
}
