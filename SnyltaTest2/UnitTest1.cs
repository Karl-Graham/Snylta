using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace SnyltaTest2
{
    //[TestClass]
    //public class UnitTest1
    //{
    //    [TestMethod]
    //    public void TestMethod1()
    //    {
    //    }
    //}

    public class BasicTests
        : IClassFixture<WebApplicationFactory<Snylta.Startup>>
    {
        private readonly WebApplicationFactory<Snylta.Startup> _factory;

        public BasicTests(WebApplicationFactory<Snylta.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("things/index")]
        [InlineData("none/all")]
        //[InlineData("/Privacy")]
        //[InlineData("/Contact")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            //Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Equals("text/html; charset=utf-8",
            //response.Content.Headers.ContentType.ToString());
        }

    }
}