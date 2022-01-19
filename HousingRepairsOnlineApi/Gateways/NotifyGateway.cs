using System.Collections.Generic;
using System.Threading.Tasks;
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

        public void SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            client.SendSms(mobileNumber: number, templateId: templateId, personalisation: personalisation);
        }
        public void SendEmail(string email, string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            client.SendEmail(emailAddress: email, templateId: templateId, personalisation: personalisation);
        }
    }
}
