using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class GroupThings
    {
        public string GroupId { get; set; }
        public virtual Group Group { get; set; }

        public string ThingId { get; set; }
        public virtual Thing Thing { get; set; }
    }
}
