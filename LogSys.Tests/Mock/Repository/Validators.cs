#region Usings

using System.Collections.Generic;
using System.Linq;
using Moq;

#endregion

namespace LogSys.Tests.Mock.Repository
{
    public partial class MockRepository
    {
        public void SetupValidators()
        {
            Setup(p => p.ValidateProjectBelongsToUserById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns((int projectId, string userName) => Projects.Single(p => p.Id == projectId).UserName == userName);

            Setup(p => p.ValidateProjectBelongsToUserByName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string projectName, string userName) => Projects.Single(p => p.Name == projectName).UserName == userName);

            Setup(p => p.ValidateLogBelongsToUser(It.IsAny<int>(), It.IsAny<string>()))
                .Returns((int logId, string userName) => Projects.Single(p => p.Id == Logs.Single(l => l.Id == logId).ProjectId).UserName == userName);

            Setup(p => p.ValidateLogsAreOfOneProject(It.IsAny<List<int>>()))
                .Returns((List<int> ids) => Logs.Where(l => ids.Contains(l.Id)).Select(l => l.ProjectId).Distinct().Count() == 1);

            Setup(p => p.ValidateUserAlreadyHasProjectWithThisName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string userName, string projectName) => Projects.Any(p => p.UserName == userName && p.Name.Trim().ToUpper() == projectName.Trim().ToUpper()));

            Setup(p => p.ValidateUserAlreadyHasAnotherProjectWithThisName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns((string userName, string projectName, int thisProjectId) => Projects.Any(p => p.UserName == userName && p.Name.Trim().ToUpper() == projectName.Trim().ToUpper() && p.Id != thisProjectId));

            Setup(p => p.ValidateAllProjectsBelongToUser(It.IsAny<string>(), It.IsAny<List<string>>()))
                .Returns((string userName, List<string> projectsNames) => Projects.Where(p => projectsNames.Contains(p.Name)).All(p => p.UserName == userName));
        }
    }
}
