using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IAppointmentsGateway
    {
        Task<IEnumerable<AvailableAppointments>> GetAvailableAppointments(string repairCode, string uprn);
    }
}
