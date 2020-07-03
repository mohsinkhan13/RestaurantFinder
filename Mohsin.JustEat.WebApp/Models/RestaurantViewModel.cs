using System.Collections.Generic;

namespace Mohsin.JustEat.WebApp.Models
{
    public class RestaurantViewModel
    {
        public string Name { get; set; }
        public double RatingStars { get; set; }
        public IList<CuisineTypeViewModel> CuisineTypes { get; set; }
    }

}
