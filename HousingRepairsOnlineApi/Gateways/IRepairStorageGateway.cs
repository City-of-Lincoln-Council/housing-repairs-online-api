﻿using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface IRepairStorageGateway
    {
        Task<Repair> AddRepair(Repair repair);
    }
}
