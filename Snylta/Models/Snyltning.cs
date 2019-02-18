using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models
{
    public class Snyltning
    {
        public string Id { get; set; }

        public bool Active { get; set; }

        public string ThingId { get; set; }
        public virtual Thing Thing { get; set; }

        public string UserId { get; set; }
        public virtual User Snyltare { get; set; }

        public Snyltning(User user, Thing thing)
        {
            Snyltare = user;
            Thing = thing;
            Active = true;
        }

        public void EndSnyltning()
        {
            Active = false;
        }
    }
}
