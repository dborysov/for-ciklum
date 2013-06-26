#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using LogSys.DataAccess.DataContext;
using Ninject;

#endregion

namespace LogSys.DataAccess
{
    public class DataRepository : IRepository
    {
        [Inject]
        public LoggyContext Db { get; set; }

        #region Getters

        public IQueryable<Project> Projects
        {
            get
            {
                return Db.Projects;
            }
        }

        public IQueryable<Report> Reports
        {
            get
            {
                return Db.Reports;
            }
        }

        public IQueryable<WorkLog> Logs
        {
            get
            {
                return Db.WorkLogs;
            }
        }

        public IQueryable<Project> GetProjects(int? id = null, string projectName = null, string userName = null)
        {
            return Db.Projects
                     .ById(id)
                     .ByUserName(userName)
                     .ByProjectName(projectName);
        }

        public IQueryable<Report> GetReports(int? id = null, string userName = null, int? projectId = null, Guid? guid = null, int? periodId = null, DateTime? endDate = null)
        {
            return Db.Reports
                     .ById(id)
                     .ByUserName(userName)
                     .ByProjectId(projectId)
                     .ByGuid(guid)
                     .ByPeriodId(periodId)
                     .ByEndDate(endDate);
        }

        public IQueryable<WorkLog> GetLogs(int? id = null, int? projectId = null, DateTime? startLogDate = null, DateTime? endLogDate = null)
        {
            return Db.WorkLogs
                     .ById(id)
                     .ByProjectId(projectId)
                     .ByStartLogDate(startLogDate)
                     .ByEndLogDate(endLogDate);
        }

        public int GetLogProjectId(int logId)
        {
            return Db.WorkLogs
                     .Where(l => l.Id == logId)
                     .Select(l => l.ProjectId)
                     .First();
        }
        #endregion

        #region Validators
        public bool ValidateProjectBelongsToUserById(int projectId, string userName)
        {
            return Db.Projects
                     .Any(p => p.Id == projectId && p.UserName == userName);
        }

        public bool ValidateProjectBelongsToUserByName(string projectName, string userName)
        {
            return Db.Projects
                     .Any(p => p.Name == projectName && p.UserName == userName);
        }

        public bool ValidateLogBelongsToUser(int logId, string userName)
        {
            return Db.WorkLogs
                     .Any(p => p.Id == logId && p.Project.UserName == userName);
        }

        public bool ValidateLogsAreOfOneProject(List<int> logsIds)
        {
            return Db.WorkLogs.Where(l => logsIds.Contains(l.Id)).Select(l => l.ProjectId).Distinct().Count() == 1;
        }

        public bool ValidateUserAlreadyHasProjectWithThisName(string userName, string projectName)
        {
            return Db.Projects.Any(p => p.UserName == userName && p.Name.Trim().ToUpper() == projectName.Trim().ToUpper());
        }

        public bool ValidateUserAlreadyHasAnotherProjectWithThisName(string userName, string projectName, int thisProjectId)
        {
            return Db.Projects.Any(p => p.UserName == userName && p.Name.Trim().ToUpper() == projectName.Trim().ToUpper() && p.Id != thisProjectId);
        }

        public bool ValidateAllProjectsBelongToUser(string userName, List<string> projectsNames)
        {
            return Db.Projects
                     .Where(p => projectsNames.Contains(p.Name))
                     .All(p => p.UserName == userName);
        }
        #endregion

        #region Modifiers
        public void ShareReport(int? projectId, string userName, Guid guid, short periodId, DateTime endDate)
        {
            Db.Reports.Add(new Report
            {
                ProjectId = projectId,
                UserName = userName,
                Guid = guid,
                PeriodId = periodId,
                EndDate = endDate
            });
            Db.SaveChanges();
        }

        public void LogTime(int projectId, string workDescription, DateTime logDate, int loggedMinutes)
        {
            Db.WorkLogs.Add(new WorkLog
            {
                LogDate = logDate,
                LoggedMinutes = loggedMinutes,
                ProjectId = projectId,
                WorkDescription = workDescription,
                Timestamp = DateTime.Now
            });
            Db.SaveChanges();
        }

        public void CreateProject(string name, string description, string userName)
        {
            Db.Projects.Add(new Project
            {
                Name = name.Trim(),
                Description = description != null ? description.Trim() : null,
                UserName = userName
            });

            Db.SaveChanges();
        }

        public void EditProject(int id, string newName = null, string newDescription = null)
        {
            var projectToEdit = Db.Projects.ById(id).First();

            if (newName != null)
                projectToEdit.Name = newName;

            if (newDescription != null)
                projectToEdit.Description = newDescription;

            Db.SaveChanges();
        }

        public void DeleteLog(int id)
        {
            var logToDelete = Db.WorkLogs.First(l => l.Id == id);

            Db.WorkLogs.Remove(logToDelete);

            Db.SaveChanges();
        }

        public void DeleteProject(string projectName)
        {
            var projectToDelete = Db.Projects.Single(p => p.Name == projectName);

            Db.Projects.Remove(projectToDelete);

            Db.SaveChanges();
        }

        public void DeleteMultipleLogs(List<int> ids)
        {
            foreach (var log in Db.WorkLogs.Where(l => ids.Contains(l.Id)))
            {
                Db.WorkLogs.Remove(log);
            }
            Db.SaveChanges();
        }

        public void DeleteMultipleProjects(List<string> projectsNames, string userName)
        {
            foreach (var project in Db.Projects.Where(p => projectsNames.Contains(p.Name)).Where(p => p.UserName == userName))
            {
                Db.Projects.Remove(project);
            }
            Db.SaveChanges();
        }
        #endregion
    }
}
