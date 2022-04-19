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
        Console.WriteLine("Constructing DynamoDbGateway");

        _dynamoDbContext = dynamoDbContext;
        this.idGenerator = idGenerator;
    }

    public async Task<Repair> AddRepair(Repair repair)
    {
        repair.Id = idGenerator.Generate();

        Console.WriteLine($"DynamoDbGateway.AddRepair: Generated ID: {repair.Id}");

        try
        {
            Console.WriteLine("DynamoDbGateway.AddRepair: Calling SaveAsync");

            var saveTask = _dynamoDbContext.SaveAsync(repair);
            saveTask.Wait();
            var result = await saveTask.ContinueWith(x =>
            {
                Console.WriteLine("DynamoDbGateway.AddRepair: SaveAsync...returning repair");

                return repair;
            });
            Console.WriteLine("DynamoDbGateway.AddRepair: Completed SaveAsync");

            return result;
        }
        catch (Exception ex)
        {
            var newRepair = await AddRepair(repair);
            return newRepair;
        }
    }
}
