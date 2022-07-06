using System;

namespace HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

public class Priority
{
    public int PriorityCode { get; set; }
    public string PriorityDescription { get; set; }
    public DateTime RequiredCompletionDateTime { get; set; }
    public int NumberOfDays { get; set; }
}
