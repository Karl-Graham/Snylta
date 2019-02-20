using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Thing
    {
        public string Id { get; set; }

        [Required]
        [Display(Name="Pryl")]
        public string Name { get; set; }

        //Nytt
        //[Required]
        [Display(Name="Ladda upp bild")]
        public string ThingPic { get; set; }

        //[Required]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        //

        public virtual ICollection<GroupThings> GroupThings { get; set; }

        public string UserId { get; set; }
        public virtual User Owner { get; set; }

        public virtual ICollection<Snyltning> Snyltningar { get; set; }

    }
}
