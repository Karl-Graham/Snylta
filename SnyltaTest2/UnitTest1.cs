using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace SnyltaTest2
{
    [TestClass]
    public class BasicTests
        : IClassFixture<WebApplicationFactory<Snylta.Startup>>
    {
        private readonly WebApplicationFactory<Snylta.Startup> _factory;

        public BasicTests(WebApplicationFactory<Snylta.Startup> factory)
        {
            _factory = factory;
        }

        //[TestMethod]
        [Theory]
        [InlineData("/")]
        [InlineData("things/index")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Theory]
        [InlineData("none/all")]
        public async Task Get_BadEndpointsReturnUnSuccsess(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(response.StatusCode,HttpStatusCode.NotFound); // Status Code 404
        }

    }
}