using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HousingRepairsOnlineApi.Domain
{
    public class RepairRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Postcode { get; set; }
        public string SOR { get; set; }
        public RepairAddress Address { get; set; }
        public RepairLocation Location { get; set; }
        public RepairProblem Problem { get; set; }
        public RepairProblemBestDescription ProblemBestDescription { get; set; }
        public string ContactPersonNumber { get; set; }
        public RepairDescription Description { get; set; }
        public RepairContactDetails ContactDetails { get; set; }
        public RepairAvailability Time { get; set; }
    }
    public class RepairAddress
    {
        public string Display { get; set; }
        public string LocationId { get; set; }
    }
    public class RepairLocation
    {
        public string value { get; set; }
        public string display { get; set; }
    }
    public class RepairProblem
    {
        public string value { get; set; }
        public string display { get; set; }
    }
    public class RepairProblemBestDescription
    {
        public string value { get; set; }
        public string display { get; set; }
    }
    public class RepairDescription
    {
        public string photo_url { get; set; }
        public string base64_img { get; set; }
        public string text { get; set; }
    }
    public class RepairContactDetails
    {
        public string type { get; set; }
        public string value { get; set; }
    }
    public class RepairAvailability
    {
        public string value { get; set; }
        public string display { get; set; }
    }
}
