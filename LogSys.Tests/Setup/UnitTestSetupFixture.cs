#region Usings

using System.Web.Mvc;
using LogSys.DataAccess;
using LogSys.Tests.Mock.Repository;
using LogSys.Tests.Tools;
using NUnit.Framework;
using Ninject;

#endregion

namespace LogSys.Tests
{
    [SetUpFixture]
    class UnitTestSetupFixture
    {
        [SetUp]
        public void Setup()
        {
            InitKernel();
        }

        protected virtual IKernel InitKernel()
        {
            var kernel = new StandardKernel();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            InitRepository(kernel);
            return kernel;
        }

        protected virtual void InitRepository(StandardKernel kernel)
        {
            kernel.Bind<MockRepository>().To<MockRepository>().InThreadScope();
            kernel.Bind<IRepository>().ToMethod(p => kernel.Get<MockRepository>().Object);
        }
    }
}
