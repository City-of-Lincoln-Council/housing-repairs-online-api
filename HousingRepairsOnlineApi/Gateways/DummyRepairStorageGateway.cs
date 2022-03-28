using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;

namespace HousingRepairsOnlineApi.Gateways
{
    public class DummyRepairStorageGateway : IRepairStorageGateway
    {
        private readonly IIdGenerator idGenerator;

        public DummyRepairStorageGateway(IIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator;
        }
        public Task<Repair> AddRepair(Repair repair)
        {
            repair.Id = idGenerator.Generate();
            return Task.FromResult(repair);
        }
    }
}
