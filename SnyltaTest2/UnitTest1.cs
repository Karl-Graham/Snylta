using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

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
    {
        private readonly WebApplicationFactory<Snylta.Startup> _factory;

        public BasicTests(WebApplicationFactory<Snylta.Startup> factory)
        {
            _factory = factory;
        }

        //[Theory]
        //[InlineData("/")]
        //[InlineData("/Index")]
        //[InlineData("/About")]
        //[InlineData("/Privacy")]
        //[InlineData("/Contact")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Index");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equals("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
