using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface ISaveRepairRequestUseCase
    {
        public Task<string> Execute(RepairRequest repairRequest);
    }
}
