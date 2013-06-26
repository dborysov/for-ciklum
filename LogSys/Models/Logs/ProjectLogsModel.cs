#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace LogSys.Models
{
    public class ProjectLogsModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ProjectId { get; set; }

        [Display(Name = "Project name")]
        public string ProjectName { get; set; }

        public List<LogModel> Logs { get; set; } 

    }
}