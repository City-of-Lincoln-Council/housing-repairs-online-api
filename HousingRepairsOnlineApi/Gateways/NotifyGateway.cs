using System.Collections.Generic;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using Notify.Interfaces;

namespace HousingRepairsOnlineApi.Gateways
{
    public class NotifyGateway : INotifyGateway
    {
        private readonly INotificationClient client;

        public NotifyGateway(INotificationClient client)
        {
            this.client = client;
        }

        public async Task<SendSmsConfirmationResponse> SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            var result = client.SendSms(
                    mobileNumber: number,
                    templateId: templateId,
                    personalisation: personalisation
                );
            var response = result
                .ToSendSmsResponse(number, templateId, personalisation);
            return response;
        }
        public async Task<SendEmailConfirmationResponse> SendEmail(string email, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            var result = client.SendEmail(
                emailAddress: email,
                templateId: templateId,
                personalisation: personalisation
            );
            var response = result
                .ToSendEmailResponse(email, templateId, personalisation);
            return response;
        }
    }
}
