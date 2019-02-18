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
        public ICollection<Thing> Things { get; set; }
        public ICollection<GroupUsers> GroupUsers { get; set; }
        public ICollection<Snyltning> Snyltningar { get; set; }
    }
}
