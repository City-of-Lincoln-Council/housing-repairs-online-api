using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

public class WorkElement
{
    public List<RateScheduleItem> RateScheduleItem { get; set; }
    public List<Trade> Trade { get; set; }
}
