using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IBookAppointmentUseCase
    {
        Task<SchedulingApiBookingResponse> Execute(string bookingReference, string sorCode, string locationId, DateTime startDateTime,
            DateTime endDateTime);
    }
}
