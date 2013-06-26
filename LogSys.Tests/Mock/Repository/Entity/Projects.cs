#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LogSys.DataAccess.DataContext;
using Moq;

#endregion

namespace LogSys.Tests.Mock.Repository
{
    public partial class MockRepository
    {
        public List<Project> Projects { get; set; }

        public void GenerateProjects()
        {
            Projects = new List<Project>
                           {
                               new Project
                                   {
                                       Id = 1,
                                       Name = "FirstProject",
                                       Description = "Description1",
                                       UserName = "User1",
                                   },
                               new Project
                                   {
                                       Id = 2,
                                       Name = "SecondProject",
                                       Description = "Description2",
                                       UserName = "User1",
                                   },
                               new Project
                                   {
                                       Id = 3,
                                       Name = "ThirdProject",
                                       Description = "Description3",
                                       UserName = "User1",
                                   },
                               new Project
                                   {
                                       Id = 4,
                                       Name = "FourthProject",
                                       Description = "Description4",
                                       UserName = "User1",
                                   },
                               new Project
                                   {
                                       Id = 5,
                                       Name = "FifthProject",
                                       Description = "Description5",
                                       UserName = "User2",
                                   },
                               new Project
                                   {
                                       Id = 6,
                                       Name = "SixthProject",
                                       Description = "Description6",
                                       UserName = "User2",
                                   }
                           };

            Setup(p => p.Projects).Returns(Projects.AsQueryable());
            Setup(p => p.GetProjects(It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((int? id, string projectName, string userName) => Projects.Where(p => id == null || p.Id == id)
                                                                                   .Where(p => string.IsNullOrEmpty(projectName) || p.Name == projectName)
                                                                                   .Where(p => string.IsNullOrEmpty(userName) || p.UserName == userName)
                                                                                   .Select(p => new Project
                                                                                                    {
                                                                                                        Id = p.Id,
                                                                                                        Description = p.Description,
                                                                                                        Name = p.Name,
                                                                                                        UserName = p.UserName,
                                                                                                        WorkLogs = new Collection<WorkLog>(Logs.Where(l => l.ProjectId == p.Id).ToList())
                                                                                                    })
                                                                                   .AsQueryable());

            Setup(p => p.CreateProject(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string name, string description, string userName) => Projects.Add(new Project
                                                                                                 {
                                                                                                     Name = name.Trim(),
                                                                                                     Description = description != null ? description.Trim() : null,
                                                                                                     UserName = userName,
                                                                                                 }));
            Setup(p => p.EditProject(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback((int id, string newName, string newDescription) =>
                              {
                                  Projects.First(p => p.Id == id).Name = newName;
                                  Projects.First(p => p.Id == id).Description = newDescription;
                              });

            Setup(p => p.DeleteProject(It.IsAny<string>()))
                .Callback((string projectName) => Projects.Remove(Projects.First(p => p.Name == projectName)));
            Setup(p => p.DeleteMultipleProjects(It.IsAny<List<string>>(), It.IsAny<string>()))
                .Callback((List<string> projectNames, string userName) => Projects.Where(p => projectNames.Contains(p.Name)).Where(p => p.UserName == userName).ToList().ForEach(p => Projects.Remove(p)));
        }
    }
}