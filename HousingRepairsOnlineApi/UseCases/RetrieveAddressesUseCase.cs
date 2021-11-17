using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class RetrieveAddressesUseCase : IRetrieveAddressesUseCase
    {
        private readonly IAddressGateway addressGateway;

        public RetrieveAddressesUseCase(IAddressGateway addressGateway)
        {
            this.addressGateway = addressGateway;
        }

        public async Task<IEnumerable<Address>> Execute(string postcode)
        {
            if (postcode == null)
            {
                throw new ArgumentNullException(nameof(postcode));
            }
            var result = new List<Address>();
            if (!string.IsNullOrEmpty(postcode))
            {
                var addresses = await addressGateway.Search(postcode);
                result.AddRange(addresses.Select(address => new Address()
                {
                    Uprn = "",
                    AddressLine1 = address.AddressLine?.First(),
                    AddressLine2 = address.CityName,
                    PostCode = address.PostalCode
                }
                ));
            }
            return result;
        }
    }
}
