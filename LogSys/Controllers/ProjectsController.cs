#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogSys.DataAccess;
using LogSys.Enums;
using LogSys.Filters;
using LogSys.Models;
using Ninject;

#endregion

namespace LogSys.Controllers
{
    public class ProjectsController : Controller
    {
        [Inject]
        public IRepository Repository { get; set; }

        [UserNameFilter]
        public ViewResult Index(string userName)
        {
            return View(GetProjects(userName));
        }

        public PartialViewResult CreateProject()
        {
            return PartialView("_AddProjectPopup");
        }

        [HttpPost]
        [UserNameFilter]
        public PartialViewResult CreateProject(ProjectModel project, string userName)
        {
            if (ModelState.IsValid)
            {
                if (Repository.ValidateUserAlreadyHasProjectWithThisName(userName, project.Name))
                {
                    ViewBag.ErrorMessage = ErrorMessages.DublicatedProjects;
                    return PartialView("_ProjectsTable", GetProjects(userName));
                }

                Repository.CreateProject(project.Name, project.Description, userName);

                ViewBag.SuccessMessage = "New project has been successfully added.";
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessages.InputDataWasIncorrect;
            }

            return PartialView("_ProjectsTable", GetProjects(userName));
        }

        [UserNameFilter]
        public ActionResult EditProject(string projectName, string userName)
        {
            var project = Repository.GetProjects(userName: userName, projectName: projectName).FirstOrDefault();

            if (project == null)
            {
                return new EmptyResult(); //Not view any popup
            }

            return PartialView("_EditProjectPopup", new ProjectModel { Name = project.Name, Description = project.Description, Id = project.Id });
        }

        [HttpPost]
        [UserNameFilter]
        public PartialViewResult EditProject(ProjectModel model, string userName)
        {
            if (ModelState.IsValid)
            {
                var project = Repository.GetProjects(id: model.Id).FirstOrDefault();

                #region Validation

                if (project == null || !model.Id.HasValue)
                {
                    ViewBag.ErrorMessage = ErrorMessages.ProjectNotFound;
                    return PartialView("_ProjectsTable", GetProjects(userName));
                }

                if (project.UserName != userName)
                {
                    ViewBag.ErrorMessage = ErrorMessages.NotYourProject;
                    return PartialView("_ProjectsTable", GetProjects(userName));
                }

                if (Repository.ValidateUserAlreadyHasAnotherProjectWithThisName(userName, model.Name, model.Id.Value))
                {
                    ViewBag.ErrorMessage = ErrorMessages.DublicatedProjects;
                    return PartialView("_ProjectsTable", GetProjects(userName));
                }

                #endregion

                Repository.EditProject(model.Id.Value, model.Name, model.Description);

                ViewBag.SuccessMessage = "Project was successfully edited.";
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessages.InputDataWasIncorrect;
            }

            return PartialView("_ProjectsTable", GetProjects(userName));
        }

        [HttpPost]
        [UserNameFilter]
        public PartialViewResult DeleteProject(string projectName, string userName)
        {
            #region Validation
            if (projectName == null)
            {
                ViewBag.ErrorMessage = ErrorMessages.ProjectNameWasNotPassedToServer;
                return PartialView("_ProjectsTable", GetProjects(userName));
            }

            if (!Repository.ValidateProjectBelongsToUserByName(projectName, userName))
            {
                ViewBag.ErrorMessage = ErrorMessages.NotYourProject;
                return PartialView("_ProjectsTable", GetProjects(userName));
            }
            #endregion

            Repository.DeleteProject(projectName);


            ViewBag.SuccessMessage = string.Format("You have successfully deleted project {0}", projectName);
            return PartialView("_ProjectsTable", GetProjects(userName));
        }

        [HttpPost]
        [UserNameFilter]
        public PartialViewResult DeleteMultipleProjects(List<string> projectNames, string userName)
        {
            #region Validation
            if (projectNames == null || !projectNames.Any())
            {
                ViewBag.ErrorMessage = ErrorMessages.CheckedNothing;
                return PartialView("_ProjectsTable", GetProjects(userName));
            }

            if (!Repository.ValidateAllProjectsBelongToUser(userName, projectNames))
            {
                ViewBag.ErrorMessage = ErrorMessages.NotAllSelectedProjectsBelongToYou;
                return PartialView("_ProjectsTable", GetProjects(userName));
            }
            #endregion

            Repository.DeleteMultipleProjects(projectNames, userName);

            ViewBag.SuccessMessage = "All selected projects were successfully deleted";
            return PartialView("_ProjectsTable", GetProjects(userName));
        }

        [NonAction]
        private List<ProjectModel> GetProjects(string userName)
        {
            return Repository.GetProjects(userName: userName)
                             .Select(p => new ProjectModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Description = p.Description
                             })
                             .OrderBy(p => p.Name)
                             .ToList();
        }

    }
}
