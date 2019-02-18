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
        public ICollection<GroupThings> GroupThings { get; set; }
        public string UserId { get; set; }
        public User Owner { get; set; }


    }
}
