using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Gateways
{
    public class DummyNotifyGateway : INotifyGateway
    {
        public void SendSms(string number, string templateId, Dictionary<string, dynamic> personalisation)
        {
            // Do nothing
        }

        public void SendEmail(string email, string templateId, Dictionary<string, dynamic> personalisation)
        {
            // Do nothing
        }
    }
}
