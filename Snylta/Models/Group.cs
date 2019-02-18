using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GroupThings>  GroupThings { get; set; }
        public virtual ICollection<GroupUsers> GroupUsers { get; set; }


    }
}
