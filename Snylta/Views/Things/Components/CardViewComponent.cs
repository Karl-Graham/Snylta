using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snylta.Models;
using Snylta.Models.ViewModels;


namespace Snylta.Views.Things.Components
{
    public class CardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Thing thing)
        {
            return View(thing);
        }
    }
}
