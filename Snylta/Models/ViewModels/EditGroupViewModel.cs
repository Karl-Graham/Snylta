using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snylta.Models.ViewModels
{
    public class EditGroupViewModel
    {
        [Required]
        [Display(Name = "Gruppnamn")]
        public string GroupName { get; set; }

        public IEnumerable<SelectListItem> UsersInGroup { get; set; }

        [Display(Name = "Välj gruppmedlemmar")]
        public string User { get; set; }
    }
}
