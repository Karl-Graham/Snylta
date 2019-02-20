using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Thing> Things { get; set; }
        public virtual ICollection<GroupUsers> GroupUsers { get; set; }
        public virtual ICollection<Snyltning> Snyltningar { get; set; }
    }
}
