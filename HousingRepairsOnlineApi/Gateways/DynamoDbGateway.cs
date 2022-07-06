using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;

namespace HousingRepairsOnlineApi.Gateways;

public class DynamoDbGateway : IRepairStorageGateway
{
    private readonly IDynamoDBContext _dynamoDbContext;

    public DynamoDbGateway(
        IDynamoDBContext dynamoDbContext
    )
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<Repair> AddRepair(Repair repair)
    {
        var saveTask = _dynamoDbContext.SaveAsync(repair);
        var result = await saveTask.ContinueWith(x => repair);
        return result;
    }
}
