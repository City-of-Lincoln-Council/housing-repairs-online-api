using System;
using HACT.Dtos;

namespace HousingRepairsOnlineApi.Domain
{
    public class AppointmentTime
    {
        public Reference Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
