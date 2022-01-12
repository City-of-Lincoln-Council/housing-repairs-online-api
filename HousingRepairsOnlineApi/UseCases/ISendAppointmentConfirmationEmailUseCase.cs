using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface ISendAppointmentConfirmationEmailUseCase
    {
        public Task<SendEmailConfirmationResponse> Execute(string number, string bookingRef, string appointmentTime);

    }
}
