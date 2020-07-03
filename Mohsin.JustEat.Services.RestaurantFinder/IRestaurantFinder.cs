using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mohsin.JustEat.Services
{
    public interface IRestaurantFinder
    {
        Task<IEnumerable<Restaurant>> FindByPostCodeAsync(string postCode);
    }

}
