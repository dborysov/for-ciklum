#region Usings

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LogSys.Controllers;
using LogSys.Enums;
using LogSys.Models;
using LogSys.Tests.Mock.Repository;
using NUnit.Framework;

#endregion

namespace LogSys.Tests
{
    [TestFixture]
    class ReportsControllerTest
    {
        [Test]
        public void Index_NotUsersProject_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ReportsController>();

            //act
            var result = controller.Index(5, null, null, "User1");

            //assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["controller"], "Error");
        }

        [Test]
        public void Index_ValidInput_ReturnsView_ModelIsProjectReportFiltersModel()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ReportsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var result = controller.Index(1, ReportPeriods.Week, DateTime.Today, "User1");

            //assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<ProjectReportFiltersModel>(((ViewResult)result).Model);

            //cleaning up
            repository.GenerateReports();
        }

        [Test]
        public void LoadReport_ModelIsProjectReportFiltersModel()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ReportsController>();

            //act
            var result = controller.LoadReport(null, ReportPeriods.Week, DateTime.Today, "User1");

            //assert
            Assert.IsInstanceOf<List<ReportRowModel>>(result.Model);
        }

        [Test]
        public void ShareReport_ModelIsGuid()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ReportsController>();

            //act
            var result = controller.ShareReport(null, ReportPeriods.Week, DateTime.Today, "User1");

            //assert
            Assert.IsInstanceOf<Guid>(result.Model);
        }

        [Test]
        public void SharedReport_ReportDoesNotExist_ErrorMessageViewed_ModelIsNull()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ReportsController>();

            //act
            var result = controller.SharedReport(Guid.NewGuid());

            //assert
            Assert.IsNotNullOrEmpty(result.ViewBag.ErrorMessage);
            Assert.IsNull(result.Model);
        }

        [Test]
        public void SharedReport_ValidInput_ModelIsSharedReportModel()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ReportsController>();

            //act
            var result = controller.SharedReport(Guid.Parse("C75E8AF4-9759-478A-B3CD-722F657A0841")); //exists in moq

            //assert
            Assert.IsInstanceOf<SharedReportModel>(result.Model);
        }
    }
}
