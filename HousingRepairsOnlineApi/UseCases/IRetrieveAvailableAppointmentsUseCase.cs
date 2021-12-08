using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IRetrieveAvailableAppointmentsUseCase
    {
        public Task<List<AppointmentTime>> Execute(string repairLocation, string repairProblem, string repairIssue,
            string uprn);
    }
}
