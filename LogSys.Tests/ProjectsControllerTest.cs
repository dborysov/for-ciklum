#region Usings

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
    class ProjectsControllerTest
    {
        [Test]
        public void Index_Get_ModelIsListOfProjectModels()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.Index("User1");

            //assert
            Assert.IsInstanceOf<List<ProjectModel>>(result.Model);
        }

        [Test]
        public void CreateProject_Post_UserHasAnotherProjectWithTheSameName_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var result = controller.CreateProject(new ProjectModel { Name = "FirstProject" }, "User1");

            //assert
            Assert.IsInstanceOf<List<ProjectModel>>(result.Model);
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.DublicatedProjects);
        }

        [Test]
        public void CreateProject_Post_ValidInput_ProjectCreated()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();
            var projectsStartCount = repository.Projects.Count;

            //act
            controller.CreateProject(new ProjectModel { Name = "new project" }, "User1");

            //assert
            Assert.AreEqual(projectsStartCount + 1, repository.Projects.Count);
            repository.GenerateProjects();
        }

        [Test]
        public void EditProject_Get_NotUsersProject_ReturnsEmptyResult()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.EditProject("Sixth project", "User1");

            //assert
            Assert.IsInstanceOf<EmptyResult>(result);
        }

        [Test]
        public void EditProject_Get_ValidInput_ReturnsPartialView_ModelIsProjectModel()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var result = controller.EditProject("FirstProject", "User1");

            //assert
            Assert.IsInstanceOf<PartialViewResult>(result);
            Assert.IsInstanceOf<ProjectModel>(((PartialViewResult)result).Model);

            //cleaning up
            repository.GenerateProjects();
        }

        [Test]
        public void EditProject_Post_ProjectIdIsNull_ReturnsErrorProjectNotFound()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.EditProject(new ProjectModel(), "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.ProjectNotFound);
        }

        [Test]
        public void EditProject_Post_ProjectDoesNotExist_ReturnsErrorProjectNotFound()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.EditProject(new ProjectModel { Id = 100, Name = "Test" }, "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.ProjectNotFound);
        }

        [Test]
        public void EditProject_Post_NotUsertProject_ReturnsErrorNotYourProject()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.EditProject(new ProjectModel { Id = 5 }, "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.NotYourProject);
        }

        [Test]
        public void EditProject_Post_ExistsAnotherProjectWithNewName_ReturnsErrorDublicatedProjects()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.EditProject(new ProjectModel { Id = 1, Name = "SecondProject" }, "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.DublicatedProjects);
        }

        [Test]
        public void EditProject_Post_ValidInput_SuccessMessageIsNotNullOrEmpty()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var result = controller.EditProject(new ProjectModel { Id = 3, Name = "NewThirdProject" }, "User1");

            //alert
            Assert.IsNotNullOrEmpty(result.ViewBag.SuccessMessage);

            //cleaning up
            repository.GenerateProjects();
        }

        [Test]
        public void DeleteProject_Post_ProjectNameIsNull_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.DeleteProject(null, "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.ProjectNameWasNotPassedToServer);
        }

        [Test]
        public void DeleteProject_Post_NotUserProject_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.DeleteProject("FifthProject", "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.NotYourProject);
        }

        [Test]
        public void DeleteProject_Post_ValidInput_SuccessMessageIsNotNullOrEmpty()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();

            //act
            var result = controller.DeleteProject("FirstProject", "User1");

            //assert
            Assert.IsNotNullOrEmpty(result.ViewBag.SuccessMessage);
            repository.GenerateProjects();
        }

        [Test]
        public void DeleteMultipleProjects_Post_ProjectNamesIsNull_ReturnsErrorCheckedNothing()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.DeleteMultipleProjects(null, "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.CheckedNothing);
        }

        [Test]
        public void DeleteMultipleProjects_Post_ProjectNamesIsEmpty_ReturnsErrorCheckedNothing()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.DeleteMultipleProjects(new List<string>(), "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.CheckedNothing);
        }

        [Test]
        public void DeleteMultipleProjects_Post_NotAllUsersProjects_ReturnsError()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();

            //act
            var result = controller.DeleteMultipleProjects(new List<string> { "FourthProject", "FifthProject" }, "User1");

            //assert
            Assert.AreEqual(result.ViewBag.ErrorMessage, ErrorMessages.NotAllSelectedProjectsBelongToYou);
        }

        [Test]
        public void DeleteMultipleProjects_Post_Valid_CountDecreases_SuccessMessageIsNotNullOrEmpty()
        {
            //init
            var controller = DependencyResolver.Current.GetService<ProjectsController>();
            var repository = DependencyResolver.Current.GetService<MockRepository>();
            var projectsStartCount = repository.Projects.Count;

            //act
            var result = controller.DeleteMultipleProjects(new List<string> { "FirstProject", "SecondProject" }, "User1");

            //assert
            Assert.AreEqual(projectsStartCount - 2, repository.Projects.Count);
            Assert.IsNotNullOrEmpty(result.ViewBag.SuccessMessage);

            //cleaning up
            repository.GenerateProjects();
        }
    }
}
