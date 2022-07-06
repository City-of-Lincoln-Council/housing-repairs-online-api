using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

public class Person
{
    public Name Name { get; set; }
    public List<Communication> Communication { get; set; }
}
