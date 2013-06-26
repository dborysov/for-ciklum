#region Usings

using LogSys.DataAccess;
using Moq;

#endregion

namespace LogSys.Tests.Mock.Repository
{
    public partial class MockRepository : Mock<IRepository>
    {
        public MockRepository(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            GenerateLogs();
            GenerateProjects();
            GenerateReports();
            SetupValidators();
        }
    }
}
