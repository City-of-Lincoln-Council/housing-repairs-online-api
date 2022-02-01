using System;

namespace HousingRepairsOnlineApi.Domain
{
    public class RepairAvailability
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string Display { get; set; }
    }
}
