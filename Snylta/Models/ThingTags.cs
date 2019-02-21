using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class ThingTags
    {
        public string ThingId { get; set; }
        public virtual Thing Thing { get; set; }

        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
