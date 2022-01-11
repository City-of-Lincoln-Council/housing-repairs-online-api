using System.Collections.Generic;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.UseCases;
using Notify.Interfaces;
using HousingRepairsOnlineApi.Factories;

namespace HousingRepairsOnlineApi.Gateways
{
    public class GovNotifyGateway : IGovNotifyGateway
    {
        private readonly INotificationClient client;

        public GovNotifyGateway(INotificationClient client)
        {
            this.client = client;
        }

        public async Task<SendSmsResponse> SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            var result = client.SendSms(
                    mobileNumber: number,
                    templateId: templateId,
                    personalisation: personalisation
                );
            var response = result
                .ToResponse(number, templateId, personalisation);
            return response;
        }

    }
}
