#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using LogSys.DataAccess.DataContext;

#endregion

namespace LogSys.DataAccess
{
    public interface IRepository
    {
        IQueryable<Project> Projects { get; }
        IQueryable<Report> Reports { get; }
        IQueryable<WorkLog> Logs { get; }
        IQueryable<Project> GetProjects(int? id = null, string projectName = null, string userName = null);
        IQueryable<Report> GetReports(int? id = null, string userName = null, int? projectId = null, Guid? guid = null, int? periodId = null, DateTime? endDate = null);
        IQueryable<WorkLog> GetLogs(int? id = null, int? projectId = null, DateTime? startLogDate = null, DateTime? endLogDate = null);
        int GetLogProjectId(int logId);

        bool ValidateProjectBelongsToUserById(int projectId, string userName);
        bool ValidateProjectBelongsToUserByName(string projectName, string userName);
        bool ValidateLogBelongsToUser(int logId, string userName);
        bool ValidateLogsAreOfOneProject(List<int> logsIds);
        bool ValidateUserAlreadyHasProjectWithThisName(string userName, string projectName);
        bool ValidateUserAlreadyHasAnotherProjectWithThisName(string userName, string projectName, int thisProjectId);
        bool ValidateAllProjectsBelongToUser(string userName, List<string> projectsNames);

        void ShareReport(int? projectId, string userName, Guid guid, short periodId, DateTime endDate);
        void LogTime(int projectId, string workDescription, DateTime logDate, int loggedMinutes);
        void CreateProject(string name, string description, string userName);
        void EditProject(int id, string newName = null, string newDescription = null);
        void DeleteLog(int id);
        void DeleteProject(string projectName);
        void DeleteMultipleLogs(List<int> ids);
        void DeleteMultipleProjects(List<string> projectsNames, string userName);
    }
}
