using System;

namespace HousingRepairsOnlineApi.Helpers
{
    public class IdGenerator : IIdGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x").ToUpper();
        }
    }
}
