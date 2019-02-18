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
        public Group Group { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
