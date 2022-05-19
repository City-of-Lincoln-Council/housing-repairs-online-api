using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

public class Address
{
    public List<string> AddressLine { get; set; }
    public string PostalCode { get; set; }
}
