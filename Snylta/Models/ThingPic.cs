using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class ThingPic
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ThingId { get; set; }
        public virtual Thing Thing { get; set; }
    }
}
