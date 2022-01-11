using System.Collections.Generic;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.UseCases;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface IGovNotifyGateway
    {
        public Task<SendSmsResponse> SendSms(string number, string templateId,
            Dictionary<string, dynamic> personalisation);
    }
}
