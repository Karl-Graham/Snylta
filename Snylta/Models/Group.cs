using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Group
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Gruppnamn")]
        public string Name { get; set; }

        public virtual ICollection<GroupThings>  GroupThings { get; set; }

        public virtual ICollection<Users> GroupUsers { get; set; }
    }
}
