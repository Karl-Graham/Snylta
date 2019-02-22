using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class GroupUsers
    {
        public string GroupId { get; set; }
        public virtual Group Group { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
