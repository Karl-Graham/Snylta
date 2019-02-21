using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Snylta.Services
{
    public class TranslationService
    {
        public string TranslateText(string text)
        {
            string host = "https://api.cognitive.microsofttranslator.com";
            string route = "/translate?api-version=3.0&from=en&to=sv";
            string subscriptionKey = "19e334b60be94395a5626903ab33256a";

            var body = new[] { new { Text = text} };

            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Set the method to POST
                request.Method = HttpMethod.Post;

                // Construct the full URI
                request.RequestUri = new Uri(host + route);

                // Add the serialized JSON object to your request
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                // Add the authorization header
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send request, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Print the response
                return jsonResponse;
            }
            /*
             * The code for your call to the translation service will be added to this
             * function in the next few sections.
             */
        }

    }
}
