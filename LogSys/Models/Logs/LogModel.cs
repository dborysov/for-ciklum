#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace LogSys.Models
{
    public class LogModel
    {
        [StringLength(100, ErrorMessage = "{0} must be from {2} to {1} characters long.", MinimumLength = 4)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Log date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LogDate { get; set; }

        [Required]
        [Display(Name = "Time (minutes)")]
        [Range(1, 60 * 24, ErrorMessage = "Time should be between {2} and {1} minutes per day.")]
        public int LoggedMinutes { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? Timestamp { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? ProjectId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

    }
}