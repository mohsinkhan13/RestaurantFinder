using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mohsin.JustEat.Services.UnitTests
{
    [TestClass]
    public class RestaurantFinderTests

    {
        private Mock<HttpMessageHandler> _mockHandler;
        private Mock<IOptions<RelativeUrlsOptions>> _mockOptions;
        public RestaurantFinderTests()
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            _mockOptions = new Mock<IOptions<RelativeUrlsOptions>>();
        }

        [TestMethod]
        public async Task FindByPostCode_Returns_Exception_If_No_PostCode()
        {
            var expectedMessage = "Postcode was not provided. Please provide a valid postcode";
            using var client = new HttpClient();
            var sut = new RestaurantFinder(client, _mockOptions.Object);
            var exception = await Assert.ThrowsExceptionAsync<ApplicationException>(
            async () => await sut.FindByPostCodeAsync(null), "");

            Assert.AreEqual(expectedMessage, exception.Message);

        }

        [TestMethod]
        public async Task FindByPostCode_Returns_IEnumerable_Of_Type_Restaurant()
        {
            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get), 
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'Restaurants':[{'Name': 'EZee Pizza','RatingStars': 5.0,'CuisineTypes': []}]}"),
                });
            var relUrl = new RelativeUrlsOptions { RestaurantsByPostCode = "testurl/" };
            _mockOptions.Setup(x => x.Value).Returns(relUrl);

            using var client = new HttpClient(_mockHandler.Object);
            client.BaseAddress = new Uri("http://testbaseurl/");
            var sut = new RestaurantFinder(client, _mockOptions.Object);
            var result = await sut.FindByPostCodeAsync("SE19");
            Assert.IsInstanceOfType(result,typeof(IEnumerable<Restaurant>));
        }

        [TestMethod]
        public async Task FindByPostCode_Returns_Empty_List_If_No_Restaurants_Found()
        {
            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'Restaurants':[]}"),
                });
            var relUrl = new RelativeUrlsOptions { RestaurantsByPostCode = "testurl/" };
            _mockOptions.Setup(x => x.Value).Returns(relUrl);

            using var client = new HttpClient(_mockHandler.Object);
            client.BaseAddress = new Uri("http://testbaseurl/");
            var sut = new RestaurantFinder(client, _mockOptions.Object);
            var result = await sut.FindByPostCodeAsync("SE19");
            Assert.IsTrue(!result.Any());

        }

    }
}
