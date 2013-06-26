#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using LogSys.DataAccess.DataContext;
using Moq;

#endregion

namespace LogSys.Tests.Mock.Repository
{
    public partial class MockRepository
    {
        public List<WorkLog> Logs { get; set; }

        public void GenerateLogs()
        {
            Logs = new List<WorkLog>
                       {
                           new WorkLog
                               {
                                   Id = 1,
                                   LogDate = new DateTime(2013, 6, 11),
                                   LoggedMinutes = 20,
                                   ProjectId = 1,
                                   Timestamp = new DateTime(2013, 6, 11),
                                   WorkDescription = "WorkDescription1"
                               },
                           new WorkLog
                               {
                                   Id = 2,
                                   LogDate = new DateTime(2013, 6, 12),
                                   LoggedMinutes = 120,
                                   ProjectId = 2,
                                   Timestamp = new DateTime(2013, 6, 12),
                                   WorkDescription = "WorkDescription1"
                               },
                           new WorkLog
                               {
                                   Id = 3,
                                   LogDate = DateTime.Today,
                                   LoggedMinutes = 60 * 23,
                                   ProjectId = 1,
                                   Timestamp = new DateTime(2013, 6, 14),
                                   WorkDescription = "WorkDescription1"
                               },
                           new WorkLog
                               {
                                   Id = 4,
                                   LogDate = new DateTime(2013, 6, 15),
                                   LoggedMinutes = 20,
                                   ProjectId = 5,
                                   Timestamp = new DateTime(2013, 6, 15),
                                   WorkDescription = "WorkDescription1"
                               },
                           new WorkLog
                               {
                                   Id = 5,
                                   LogDate = new DateTime(2013, 6, 15),
                                   LoggedMinutes = 20,
                                   ProjectId = 5,
                                   Timestamp = new DateTime(2013, 6, 15),
                                   WorkDescription = "WorkDescription1"
                               },
                       };

            Setup(p => p.Logs).Returns(Logs.AsQueryable());
            Setup(p => p.GetLogs(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns((int? id, int? projectId, DateTime? startLogDate, DateTime? endLogDate) => Logs.Where(l => id == null || l.Id == id)
                                                                                                        .Where(l => projectId == null || l.ProjectId == projectId)
                                                                                                        .Where(l => startLogDate == null || l.LogDate >= startLogDate)
                                                                                                        .Where(l => endLogDate == null || l.LogDate <= endLogDate)
                                                                                                        .AsQueryable());
            Setup(p => p.GetLogProjectId(It.IsAny<int>()))
                .Returns((int logId) => Logs.Where(l => l.Id == logId).Select(l => l.ProjectId).First());

            Setup(p => p.LogTime(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Callback((int projectId, string workDescription, DateTime logDate, int loggedMinutes) => Logs.Add(new WorkLog
                {
                    LogDate = DateTime.Today,
                    LoggedMinutes = loggedMinutes,
                    ProjectId = projectId,
                    WorkDescription = workDescription,
                    Timestamp = DateTime.Now
                }));
            Setup(p => p.DeleteLog(It.IsAny<int>()))
                .Callback((int id) => Logs.Remove(Logs.First(l => l.Id == id)));
            Setup(p => p.DeleteMultipleLogs(It.IsAny<List<int>>()))
                .Callback((List<int> ids) => Logs.Where(l => ids.Contains(l.Id)).ToList().ForEach(l => Logs.Remove(l)));
        }
    }
}
