using System.Collections.Generic;

namespace Mohsin.JustEat.Services
{
    public class Restaurant
    {
        public string Name { get; set; }
        public double RatingStars { get; set; }
        public IList<CuisineType> CuisineTypes { get; set; }
    }

}
