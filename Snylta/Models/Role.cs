using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Role : IdentityRole
    {
        public virtual ICollection<GroupUsers> GroupUsers { get; set; }

        public Role() : base()
        {

        }

        public Role(string roleName) : base(roleName)
        {

        }
    }
}
