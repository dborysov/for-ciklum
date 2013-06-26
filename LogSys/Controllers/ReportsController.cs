#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogSys.DataAccess;
using LogSys.DataAccess.DataContext;
using LogSys.Enums;
using LogSys.Filters;
using LogSys.Models;
using Ninject;

#endregion

namespace LogSys.Controllers
{
    public class ReportsController : Controller
    {
        [Inject]
        public IRepository Repository { get; set; }

        [UserNameFilter]
        public ActionResult Index(int? projectId, ReportPeriods? period, DateTime? endDate, string userName)
        {
            period = period ?? ReportPeriods.Week;
            endDate = endDate == null || endDate > DateTime.Today ? DateTime.Today : endDate;

            #region Validation

            if (projectId.HasValue && !Repository.ValidateProjectBelongsToUserById(projectId.Value, userName))
            {
                TempData["ErrorMessage"] = ErrorMessages.NotYourProject;
                return RedirectToAction("AjaxError", "Error");
            }

            #endregion

            return View(GetReportFilters(projectId, period.Value, endDate.Value, userName));
        }

        [UserNameFilter]
        public PartialViewResult LoadReport(int? projectId, ReportPeriods period, DateTime endDate, string userName)
        {
            return PartialView("_ReportTable", GetReportTable(projectId, period, endDate <= DateTime.Today ? endDate : DateTime.Today, userName));
        }

        [UserNameFilter]
        public PartialViewResult ShareReport(int? projectId, ReportPeriods period, DateTime endDate, string userName)
        {
            var guid = Guid.NewGuid();

            Repository.ShareReport(projectId, userName, guid, (short)period, endDate);

            return PartialView("_ShareReport", guid);
        }

        [AllowAnonymous]
        public ViewResult SharedReport(Guid guid)
        {
            var reportInfo = Repository.GetReports(guid: guid).FirstOrDefault();

            if (reportInfo == null)
            {
                ViewBag.ErrorMessage = ErrorMessages.ReportDoesNotExistOrWasDeleted;
                return View();
            }

            return View(new SharedReportModel
            {
                UserName = reportInfo.UserName,
                ProjectName = reportInfo.ProjectId != null ? reportInfo.Project.Name : "All projects",
                Period = (ReportPeriods)reportInfo.PeriodId,
                EndDate = reportInfo.EndDate,
                Report = GetReportTable(reportInfo.ProjectId, (ReportPeriods)reportInfo.PeriodId, reportInfo.EndDate, reportInfo.UserName)
            });
        }

        [NonAction]
        private ProjectReportFiltersModel GetReportFilters(int? projectId, ReportPeriods period, DateTime endDate, string userName)
        {
            return new ProjectReportFiltersModel
            {
                ProjectId = projectId,
                Period = period,
                EndDate = endDate,
                UserProjects = Repository.GetProjects(userName: userName)
                                         .Where(p => p.UserName == userName)
                                         .OrderBy(p => p.Name)
                                         .Select(p => new ProjectModel { Id = p.Id, Name = p.Name, Description = p.Description })
                                         .ToList()
            };
        }

        [NonAction]
        private List<ReportRowModel> GetReportTable(int? projectId, ReportPeriods period, DateTime endDate, string userName)
        {
            if (endDate > DateTime.Today)
                endDate = DateTime.Today;

            var gLogs = new List<IGrouping<DateTime, WorkLog>>();

            var logs = Repository.GetProjects(id: projectId, userName: userName)
                                  .SelectMany(p => p.WorkLogs)
                                  .GroupBy(l => l.LogDate);

            var extraDates = new List<ReportRowModel>();

            switch (period)
            {
                case ReportPeriods.Week:

                    for (var i = endDate; i > endDate.AddDays(-7); i = i.AddDays(-1))
                    {
                        extraDates.Add(new ReportRowModel { Date = i, Duration = new TimeSpan(0) });
                    }
                    gLogs = logs.ToList().Where(l => l.Key > endDate.AddDays(-7) && l.Key <= endDate).ToList();

                    break;

                case ReportPeriods.Month:
                    for (var i = endDate; i > endDate.AddMonths(-1); i = i.AddDays(-1))
                    {
                        extraDates.Add(new ReportRowModel { Date = i, Duration = new TimeSpan(0) });
                    }
                    gLogs = logs.ToList().Where(l => l.Key > endDate.AddMonths(-1) && l.Key <= endDate).ToList();
                    break;
            }

            return gLogs.Select(l => new ReportRowModel
            {
                Date = l.Key,
                Duration = TimeSpan.FromMinutes(l.Sum(log => log.LoggedMinutes)),
            })
                        .Union(extraDates)
                        .GroupBy(d => d.Date)
                        .Select(g => new ReportRowModel { Date = g.Key, Duration = TimeSpan.FromMinutes(g.Sum(d => d.Duration.TotalMinutes)) })
                        .OrderBy(g => g.Date)
                        .ToList();
        }

    }
}
