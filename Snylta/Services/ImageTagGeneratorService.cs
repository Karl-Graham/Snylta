using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Snylta.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snylta.Services
{
    public class ImageTagGeneratorService
    {
        private string subscriptionKey;
        private const int thumbnailWidth = 288*3;
        private const int thumbnailHeight = 200*3;
        private const bool writeThumbnailToDisk = false;

        public ImageTagGeneratorService(AppSettings aPIKeys)
        {
            subscriptionKey = aPIKeys.ComputerVisionKey;
        }


        internal async Task<List<ImageAnalysis>> GetTagsForImages(List<string> filePaths)
        {
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
            //VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            //VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            //VisualFeatureTypes.Objects,
            VisualFeatureTypes.Tags
            };

            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = "https://northeurope.api.cognitive.microsoft.com";

            List<ImageAnalysis> analysises = new List<ImageAnalysis>();

            foreach (var item in filePaths)
            {
                
                string thumbnailFilePath =
                        item.Insert(item.Length - 4, "_thumb");
                using (Stream imageStream = File.OpenRead(item))
                {
                    Stream thumbnail = await computerVision.GenerateThumbnailInStreamAsync(
                        thumbnailWidth, thumbnailHeight, imageStream, true);
                    
                    using (Stream file = File.Create(thumbnailFilePath))
                    {
                        thumbnail.CopyTo(file);
                    }
                }
                using (Stream imageStream = File.OpenRead(thumbnailFilePath))
                {
                    ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                        imageStream, features);

                    analysises.Add(analysis);
                }
            }
            return analysises;
        }
    }
}
