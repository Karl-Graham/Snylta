﻿using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Required(ErrorMessage = "Du måste ange gruppnamn")]
        [Display(Name = "Gruppnamn")]
        public string Name { get; set; }

        public string Pic { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }



        public virtual ICollection<GroupThings> GroupThings { get; set; }

        [Display(Name = "Gruppmedlemmar")]
        public virtual ICollection<GroupUsers> GroupUsers { get; set; }
    }
}
