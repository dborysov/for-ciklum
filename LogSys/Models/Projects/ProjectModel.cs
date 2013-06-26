#region Usings

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace LogSys.Models
{
    public class ProjectModel
    {
        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Project name must be from 4 to 100 chars length!", MinimumLength = 4)]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The project description must be from 4 to 500 chars length!", MinimumLength = 4)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Project description")]
        public string Description { get; set; }

    }
}