using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Helpers
{
    public interface IInternalEmailSender
    {
        public Task Execute(Repair repair);
    }
}
