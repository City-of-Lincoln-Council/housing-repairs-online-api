using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public class DummyAppointmentsGateway : IAppointmentsGateway
    {
        public Task<IEnumerable<Appointment>> GetAvailableAppointments(string sorCode, string locationId, DateTime? fromDate = null)
        {
            var dateTime = new DateTime(2022, 1, 1);
            return Task.FromResult<IEnumerable<Appointment>>(new[]
            {
                new Appointment
                {
                    Date = dateTime,
                    TimeOfDay = new TimeOfDay
                    {
                        EarliestArrivalTime = dateTime.AddHours(8),
                        LatestArrivalTime = dateTime.AddHours(12),
                    }
                }
            });
        }

        public Task<SchedulingApiBookingResponse> BookAppointment(string bookingReference, string sorCode, string locationId, DateTime startDateTime,
            DateTime endDateTime)
        {
            return Task.FromResult(new SchedulingApiBookingResponse());
        }
    }
}
