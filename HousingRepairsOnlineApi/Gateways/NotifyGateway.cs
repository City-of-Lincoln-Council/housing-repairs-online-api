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

        public async Task SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            client.SendSms(mobileNumber: number, templateId: templateId, personalisation: personalisation);
        }
        public async Task SendEmail(string email, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            client.SendEmail(emailAddress: email,templateId: templateId,personalisation: personalisation);
        }
    }
}
