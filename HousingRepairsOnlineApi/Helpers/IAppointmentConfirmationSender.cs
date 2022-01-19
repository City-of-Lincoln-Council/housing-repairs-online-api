using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Helpers
{
    public interface IAppointmentConfirmationSender
    {
        public Task Execute(Repair repair);
    }
}
