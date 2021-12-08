using System;

namespace HousingRepairsOnlineApi.Domain
{
    public class AppointmentTime
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
