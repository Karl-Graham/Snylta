using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Tag
    {
        public string Id { get; set; }
        public string SwedishTag { get; set; }
        public string EnglishTag { get; set; }
        public virtual ICollection<ThingTags> ThingTags { get; set; }


    }
}
