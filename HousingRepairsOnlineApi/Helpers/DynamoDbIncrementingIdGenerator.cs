using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Ardalis.GuardClauses;

namespace HousingRepairsOnlineApi.Helpers;

public class DynamoDbIncrementingIdGenerator : IIdGenerator
{
    private readonly IAmazonDynamoDB dynamoDb;
    private readonly string partitionKey;
    private readonly string initialIdString;
    private string tableName;

    public DynamoDbIncrementingIdGenerator(IAmazonDynamoDB dynamoDb, string tableName, string partitionKey, int initialId)
    {
        Guard.Against.Null(dynamoDb, nameof(dynamoDb));
        Guard.Against.NullOrWhiteSpace(tableName, nameof(tableName));
        Guard.Against.NullOrWhiteSpace(partitionKey, nameof(partitionKey));
        Guard.Against.OutOfRange(initialId, nameof(initialId), 0, int.MaxValue);

        this.dynamoDb = dynamoDb;
        this.tableName = tableName;
        this.partitionKey = partitionKey;
        initialIdString = initialId.ToString();
    }

    public async Task<string> GenerateAsync()
    {
        var currentIdAttributeName = "CurrentId";
        var updateItemRequest = new UpdateItemRequest
        {
            Key = new Dictionary<string, AttributeValue>() { { partitionKey, new AttributeValue { S = "repairRequestId" } } },
            ExpressionAttributeNames = new Dictionary<string, string> { { "#CurrentId", currentIdAttributeName } },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":start", new AttributeValue { N = initialIdString } },
                { ":increment", new AttributeValue { N = "1" } }
            },
            UpdateExpression = "SET #CurrentId = if_not_exists(#CurrentId, :start) + :increment",
            TableName = tableName,
            ReturnValues = "UPDATED_NEW",
        };

        var response = await dynamoDb.UpdateItemAsync(updateItemRequest);
        var result = response.Attributes[currentIdAttributeName].N;

        return result;
    }

    public string Generate()
    {
        var result = GenerateAsync().GetAwaiter().GetResult();

        return result;
    }
}
