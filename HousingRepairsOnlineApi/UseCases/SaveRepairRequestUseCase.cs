﻿using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SaveRepairRequestUseCase : ISaveRepairRequestUseCase
    {
        private readonly ICosmosGateway cosmosGateway;

        public SaveRepairRequestUseCase(ICosmosGateway cosmosGateway)

        {
            this.cosmosGateway = cosmosGateway;
        }

        public async Task<string> Execute(RepairRequest repairRequest)
        {
            repairRequest.Id = Guid.NewGuid().ToString().GetHashCode().ToString("x");;
            var savedRequest = await cosmosGateway.AddItemToContainerAsync(repairRequest);

            return savedRequest;
        }
    }
}
