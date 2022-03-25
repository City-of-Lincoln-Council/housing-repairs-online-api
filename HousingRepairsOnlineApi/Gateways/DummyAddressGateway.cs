using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;

namespace HousingRepairsOnlineApi.Gateways
{
    public class DummyAddressGateway : IAddressGateway
    {
        public Task<IEnumerable<PropertyAddress>> Search(string postcode)
        {
            return Task.FromResult<IEnumerable<PropertyAddress>>(new[] { new PropertyAddress { PostalCode = postcode } });
        }
    }
}
