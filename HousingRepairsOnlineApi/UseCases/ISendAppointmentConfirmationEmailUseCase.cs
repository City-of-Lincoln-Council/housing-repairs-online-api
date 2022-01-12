using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface ISendAppointmentConfirmationEmailUseCase
    {
        public Task<SendEmailConfirmationResponse> Execute(string email, string bookingRef, string appointmentTime);

    }
}
