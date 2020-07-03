using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mohsin.JustEat.Services
{
    public class RestaurantFinder : IRestaurantFinder
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<RelativeUrlsOptions> _options;

        public RestaurantFinder(HttpClient httpClient, IOptions<RelativeUrlsOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<IEnumerable<Restaurant>> FindByPostCodeAsync(string postCode)
        {
            if (string.IsNullOrEmpty(postCode))
                throw new ApplicationException("Postcode was not provided. Please provide a valid postcode");

            var requestUrl = string.Format(_options.Value.RestaurantsByPostCode, postCode);
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"{response.StatusCode} - {response.ReasonPhrase}");
            
            var listOfRestaurants = await MapToRestaurants(response);
            return listOfRestaurants;
        }

        private async Task<IEnumerable<Restaurant>> MapToRestaurants(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            JObject result = JObject.Parse(content);
            var res = JsonConvert.SerializeObject(result["Restaurants"].Children().ToList());

            var listOfRestaurants = JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(res);
            return listOfRestaurants;
        }
    }
}
