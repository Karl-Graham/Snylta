using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Thing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GroupThings> GroupThings { get; set; }

        public string UserId { get; set; }
        public virtual User Owner { get; set; }

        public virtual ICollection<Snyltning> Snyltningar { get; set; }

    }
}
