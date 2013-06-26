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
        public List<Report> Reports { get; set; }

        public void GenerateReports()
        {
            Reports = new List<Report>
                          {
                              new Report
                                  {
                                      Id = 1,
                                      EndDate = new DateTime(2013, 6, 15),
                                      Guid = Guid.NewGuid(),
                                      PeriodId = 1,
                                      ProjectId = null,
                                      UserName = "User1"
                                  },
                              new Report
                                  {
                                      Id = 2,
                                      EndDate = new DateTime(2013, 6, 11),
                                      Guid = Guid.NewGuid(),
                                      PeriodId = 2,
                                      ProjectId = null,
                                      UserName = "User1"
                                  },
                              new Report
                                  {
                                      Id = 3,
                                      EndDate = new DateTime(2013, 6, 15),
                                      Guid = Guid.NewGuid(),
                                      PeriodId = 1,
                                      ProjectId = null,
                                      UserName = "User2"
                                  },
                              new Report
                                  {
                                      Id = 4,
                                      EndDate = new DateTime(2013, 6, 10),
                                      Guid = Guid.Parse("C75E8AF4-9759-478A-B3CD-722F657A0841"),
                                      PeriodId = 1,
                                      ProjectId = null,
                                      UserName = "User2"
                                  }
                          };
            Setup(p => p.Reports).Returns(Reports.AsQueryable());
            Setup(p => p.GetReports(It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<int?>(), It.IsAny<DateTime?>()))
                .Returns((int? id, string userName, int? projectId, Guid? guid, int? periodId, DateTime? endDate) => Reports.Where(r => id == null || r.Id == id)
                                                                                                                            .Where(r => string.IsNullOrEmpty(userName) || r.UserName == userName)
                                                                                                                            .Where(r => projectId == null || r.ProjectId == projectId)
                                                                                                                            .Where(r => guid == null || r.Guid == guid)
                                                                                                                            .Where(r => periodId == null || r.PeriodId == periodId)
                                                                                                                            .Where(r => endDate == null || r.EndDate == endDate)
                                                                                                                            .AsQueryable());

            Setup(p => p.ShareReport(It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<short>(), It.IsAny<DateTime>()))
                .Callback((int? projectId, string userName, Guid guid, short periodId, DateTime endDate) => Reports.Add(new Report
                                                                                                                            {
                                                                                                                                ProjectId = projectId,
                                                                                                                                UserName = userName,
                                                                                                                                Guid = guid,
                                                                                                                                PeriodId = periodId,
                                                                                                                                EndDate = endDate
                                                                                                                            }));
        }
    }
}
