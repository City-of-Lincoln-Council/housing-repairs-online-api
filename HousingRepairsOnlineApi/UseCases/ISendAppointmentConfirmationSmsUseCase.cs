using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface ISendAppointmentConfirmationSmsUseCase
    {
        public Task<SendSmsConfirmationResponse> Execute(string number, string bookingRef, string appointmentTime);
    }
}
