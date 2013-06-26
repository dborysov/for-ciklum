#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogSys.DataAccess;
using LogSys.Enums;
using LogSys.Filters;
using LogSys.Models;
using Ninject;

#endregion

namespace LogSys.Controllers
{
    public class LogsController : Controller
    {
        [Inject]
        public IRepository Repository { get; set; }

        [UserNameFilter]
        public ViewResult Index(int projectId, string userName)
        {
            if (!Repository.ValidateProjectBelongsToUserById(projectId, userName))
            {
                ViewBag.ErrorMessage = ErrorMessages.NotYourProject;
            }

            return View(GetLogs(projectId));
        }

        public PartialViewResult CreateLogPopup(int projectId) { return PartialView("_LogTimePopup", new LogModel { ProjectId = projectId, LogDate = DateTime.Today }); }

        [HttpPost]
        [UserNameFilter]
        public ActionResult CreateLog(LogModel log, string userName = null)
        {
            if (ModelState.IsValid)
            {
                #region Validation
                if (!log.ProjectId.HasValue || !Repository.ValidateProjectBelongsToUserById(log.ProjectId.Value, userName))
                {
                    TempData["ErrorMessage"] = ErrorMessages.NotYourProject;
                    return RedirectToAction("AjaxError", "Error");
                }

                if (log.LogDate.Date > DateTime.Today)
                {
                    ViewBag.ErrorMessage = ErrorMessages.WrongDateSelected;
                    return PartialView("_LogsTable", GetLogs(log.ProjectId.Value));
                }

                var currentProjectLogDayLogs = Repository.GetLogs(projectId: log.ProjectId, startLogDate: log.LogDate.Date, endLogDate: log.LogDate.Date);

                if (currentProjectLogDayLogs.Any() && currentProjectLogDayLogs.Sum(l => l.LoggedMinutes) + log.LoggedMinutes >= 24 * 60)
                {
                    ViewBag.ErrorMessage = ErrorMessages.TriedToLogMoreThen24HoursPerDay;
                    return PartialView("_LogsTable", GetLogs(log.ProjectId.Value));
                }
                #endregion

                Repository.LogTime(log.ProjectId.Value, log.Description, log.LogDate.Date, log.LoggedMinutes);

                ViewBag.SuccessMessage = "The log was created successfully";
                return PartialView("_LogsTable", GetLogs(log.ProjectId.Value));
            }

            TempData["ErrorMessage"] = ErrorMessages.InputDataWasIncorrect;
            return RedirectToAction("AjaxError", "Error");
        }

        [HttpPost]
        [UserNameFilter]
        public ActionResult DeleteLog(int logId, string userName = null)
        {
            #region Validation
            if (!Repository.ValidateLogBelongsToUser(logId, userName))
            {
                TempData["ErrorMessage"] = ErrorMessages.NotYourProject;
                return RedirectToAction("AjaxError", "Error");
            }
            #endregion

            var logProjectId = Repository.GetLogProjectId(logId);

            Repository.DeleteLog(logId);

            ViewBag.SuccessMessage = "Your log was successfully deleted";
            return PartialView("_LogsTable", GetLogs(logProjectId));
        }

        [HttpPost]
        [UserNameFilter]
        public ActionResult DeleteMultipleLogs(List<int> logsIds, string userName = null)
        {
            #region Validation
            if (logsIds == null || !logsIds.Any())
            {
                TempData["ErrorMessage"] = ErrorMessages.CheckedNothing;
                return RedirectToAction("AjaxError", "Error");
            }

            if (!Repository.ValidateLogsAreOfOneProject(logsIds))
            {
                TempData["ErrorMessage"] = ErrorMessages.TriedDeleteLogsFromMultipleProjects;
                return RedirectToAction("AjaxError", "Error");
            }

            var projectId = Repository.GetLogProjectId(logsIds.First());

            if (!Repository.ValidateProjectBelongsToUserById(projectId, userName))
            {
                TempData["ErrorMessage"] = ErrorMessages.NotYourProject;
                return RedirectToAction("AjaxError", "Error");
            }
            #endregion

            Repository.DeleteMultipleLogs(logsIds);

            ViewBag.SuccessMessage = "All selected logs were successfully deleted";
            return PartialView("_LogsTable", GetLogs(projectId));
        }

        [NonAction]
        private ProjectLogsModel GetLogs(int projectId)
        {
            var project = Repository.GetProjects(projectId).First();

            return new ProjectLogsModel
                       {
                           ProjectId = projectId,
                           ProjectName = project.Name,
                           Logs = project.WorkLogs
                                         .Select(l => new LogModel
                                                          {
                                                              Id = l.Id,
                                                              ProjectId = l.ProjectId,
                                                              Description = l.WorkDescription,
                                                              LogDate = l.LogDate,
                                                              LoggedMinutes = l.LoggedMinutes,
                                                              Timestamp = l.Timestamp
                                                          })
                                         .OrderByDescending(l => l.LogDate)
                                         .ThenByDescending(l => l.Timestamp)
                                         .ToList()
                       };
        }
    }
}
