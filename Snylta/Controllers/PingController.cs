using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        // GET api/ping
        [HttpGet]
        public string Get()
        {
            return "Ping";
        }
    }
}
