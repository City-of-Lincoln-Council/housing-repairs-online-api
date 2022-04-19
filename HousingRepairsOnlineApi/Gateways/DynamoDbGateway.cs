using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;

namespace HousingRepairsOnlineApi.Gateways;

public class DynamoDbGateway : IRepairStorageGateway
{
    private readonly IDynamoDBContext _dynamoDbContext;
    private readonly IIdGenerator idGenerator;

    public DynamoDbGateway(
        IDynamoDBContext dynamoDbContext,
        IIdGenerator idGenerator
        )
    {
        _dynamoDbContext = dynamoDbContext;
        this.idGenerator = idGenerator;
    }

    public async Task<Repair> AddRepair(Repair repair)
    {
        repair.Id = idGenerator.Generate();

        try
        {
            var saveTask = _dynamoDbContext.SaveAsync(repair);
            var result = await saveTask.ContinueWith(x => repair);
            return result;
        }
        catch (Exception ex)
        {
            var newRepair = await AddRepair(repair);
            return newRepair;
        }
    }
}
