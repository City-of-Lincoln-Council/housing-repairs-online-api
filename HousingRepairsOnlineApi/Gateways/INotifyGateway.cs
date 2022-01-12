using System.Collections.Generic;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface INotifyGateway
    {
        public Task<SendSmsConfirmationResponse> SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation);
        public Task<SendEmailConfirmationResponse> SendEmail(string email, string templateId,
            Dictionary<string, dynamic> personalisation);
    }
}
