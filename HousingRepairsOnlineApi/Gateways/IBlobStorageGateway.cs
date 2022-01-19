﻿using System;
using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface IBlobStorageGateway
    {
        Task<string> UploadBlob(string base64Img, string fileExtension);
        Task<string> GetServiceSasUriForBlob(string blobName, int daysUntilExpiry, string storedPolicyName = null);
    }
}
