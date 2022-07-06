using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

public class Property
{
    public string PropertyReference { get; set; }
    public Address Address { get; set; }
    public List<Reference> Reference { get; set; }
}
