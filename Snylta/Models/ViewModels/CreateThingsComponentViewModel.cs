using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models.ViewModels
{
    public class CreateThingsComponentViewModel
    {
        public Thing Thing { get; set; }
        public GroupSelection[] GroupSelections { get; set; }
    }


    public class GroupSelection
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
