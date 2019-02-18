using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class UserRoles : IdentityUserRole<string>
    {
        public string GroupId { get; set; }
        public Group Group { get; set; }

    }
}
