using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface ICosmosGateway
    {
        Task<string> AddItemToContainerAsync(RepairRequest repairRequest);
    }
}
