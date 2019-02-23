using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snylta.Models.ImageTagGenerator
{
    public class ImageTagComparer : IEqualityComparer<ImageTag>
    {
        public bool Equals(ImageTag x, ImageTag y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(ImageTag obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
