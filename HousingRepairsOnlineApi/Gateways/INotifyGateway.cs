using System.Collections.Generic;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface INotifyGateway
    {
        public Task SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation);
        public Task SendEmail(string email, string templateId,
            Dictionary<string, dynamic> personalisation);
    }
}
