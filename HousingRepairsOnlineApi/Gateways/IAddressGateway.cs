﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface IAddressGateway
    {
        Task<IEnumerable<PropertyAddress>> Search(string postcode);
    }
}
