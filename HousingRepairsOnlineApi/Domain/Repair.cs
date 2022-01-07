using System.Text.Json.Serialization;

namespace HousingRepairsOnlineApi.Domain
{
    public class Repair
    {
        [JsonPropertyName("id")]
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
