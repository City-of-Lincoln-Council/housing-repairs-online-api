using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Helpers
{
    public interface IAppointmentConfirmationSender
    {
        public void Execute(Repair repair);
    }
}
