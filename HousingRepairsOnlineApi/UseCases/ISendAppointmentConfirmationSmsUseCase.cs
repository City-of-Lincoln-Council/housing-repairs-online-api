using System.Threading.Tasks;
using Notify.Models.Responses;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface ISendAppointmentConfirmationSmsUseCase
    {
        public Task<SendSmsResponse> Execute(string number, string bookingRef, string appointmentTime);
    }
}
