using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;

namespace HousingRepairsOnlineApi.Domain
{
    [DynamoDBTable($"{Constants.DYNAMODB_TABLE_NAME}", LowerCamelCaseProperties = true)]
    public class Repair
    {
        [JsonProperty($"{Constants.DYNAMODB_TABLE_PARTITION_KEY}")]
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Postcode { get; set; }
        public string SOR { get; set; }
        public RepairAddress Address { get; set; }
        public RepairLocation Location { get; set; }
        public RepairProblem Problem { get; set; }
        public RepairIssue Issue { get; set; }
        public string ContactPersonNumber { get; set; }
        public RepairDescription Description { get; set; }
        public RepairContactDetails ContactDetails { get; set; }
        public RepairAvailability Time { get; set; }
    }
}
