using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace HousingRepairsOnlineApi.UseCases
{
    public class RetrieveAvailableAppointmentsUseCase
    {
        public async Task Execute(string repairCode, string uprn)
        {
            Guard.Against.NullOrWhiteSpace(repairCode, nameof(repairCode));
            Guard.Against.NullOrWhiteSpace(uprn, nameof(uprn));
        }
    }
}
