#region Usings

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LogSys.Controllers;
using LogSys.Models;
using LogSys.Tests.Mock.Repository;
using NUnit.Framework;

#endregion

namespace LogSys.Tests
{
    [TestFixture]
    public class LogsControllerTests
    {
        [Test]
        public void Index_Get_NotUsersProject_ErrorMessageIsNotNull()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.Index(5, "User1");

            //Assert
            Assert.IsNotNullOrEmpty(result.ViewBag.ErrorMessage);
        }

        [Test]
        public void CreateLogPopup_Get_ModelIsLogModel()
        {
            //init
            var controller = new LogsController();

            //act
            var result = controller.CreateLogPopup(1);

            //assert
            Assert.IsInstanceOf<LogModel>((result).Model);
        }

        [Test]
        public void CreateLog_Post_ProjectIdIsNull_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.CreateLog(new LogModel());

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.IsTrue(((RedirectToRouteResult)result).RouteValues["controller"].Equals("Error"));
        }

        [Test]
        public void CreateLog_Post_NotUserProject_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.CreateLog(new LogModel
            {
                ProjectId = 1,
            }, "User2");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void CreateLog_Post_LogDateIsInFuture_ReturnsPartialView_ErrorMessageIsNotNull()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.CreateLog(new LogModel { LogDate = DateTime.Today.AddDays(1), LoggedMinutes = 1, ProjectId = 1 }, "User1");

            //assert
            Assert.IsInstanceOf<PartialViewResult>(result);
            Assert.IsNotNullOrEmpty(((PartialViewResult)result).ViewBag.ErrorMessage);
        }

        [Test]
        public void CreateLog_Post_LoggedMinutesMoreThen1440_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.CreateLog(new LogModel
            {
                LogDate = new DateTime(2013, 6, 15),
                LoggedMinutes = 70,
                ProjectId = 1,
            });

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void CreateLog_Post_ViewIsPartial_ModelIsProjectLogsModel_LogCreated()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var logsCountBeforeInsertion = repository.Logs.Count;

            var result = controller.CreateLog(new LogModel
            {
                ProjectId = 5,
                Description = "InsertedLog",
                LogDate = DateTime.Today,
                LoggedMinutes = 30
            }, "User2");

            //assert
            Assert.IsInstanceOf<PartialViewResult>(result);
            Assert.IsInstanceOf<ProjectLogsModel>(((PartialViewResult)result).Model);
            Assert.AreEqual(logsCountBeforeInsertion + 1, repository.Logs.Count);

            //cleaning up
            repository.GenerateLogs();
        }

        [Test]
        public void DeleteLog_Post_NotUsersLog_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.DeleteLog(1, "User2");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void DeleteLog_Post_ValidInput_DeletesLog_SuccessMessageIsNotNullOrEmpty_ReturnsPartialView()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();
            var startLogsCount = repository.Logs.Count;

            //act
            var result = controller.DeleteLog(1, "User1");

            //assert
            Assert.IsInstanceOf<PartialViewResult>(result);
            Assert.IsNotNullOrEmpty(((PartialViewResult)result).ViewBag.SuccessMessage);
            Assert.AreEqual(startLogsCount - 1, repository.Logs.Count);

            //cleaning up
            repository.GenerateLogs();
        }

        [Test]
        public void DeleteMultipleLogs_Post_LogIdsIsNull_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.DeleteMultipleLogs(null, "User1");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void DeleteMultipleLogs_Post_LogIdsIsEmpty_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.DeleteMultipleLogs(new List<int>(), "User1");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void DeleteMultipleLogs_Post_LogsAreNotOfOneProject_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.DeleteMultipleLogs(new List<int> { 1, 2 }, "User1");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void DeleteMultipleLogs_Post_NotUsersLogs_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();

            //act
            var result = controller.DeleteMultipleLogs(new List<int> { 4, 5 }, "User1");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void DeleteMultipleLogs_Post_ValidInput_SuccessMessageIsNotNullOrEmpty_ReturnsPartialView()
        {
            //init
            var controller = DependencyResolver.Current.GetService<LogsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var result = controller.DeleteMultipleLogs(new List<int> { 1, 3 }, "User1");

            //assert
            Assert.IsInstanceOf<PartialViewResult>(result);
            Assert.IsNotNullOrEmpty(((PartialViewResult)result).ViewBag.SuccessMessage);

            //clean up
            repository.GenerateLogs();
        }
    }
}
