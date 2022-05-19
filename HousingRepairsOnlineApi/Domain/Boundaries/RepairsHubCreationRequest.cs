using System.Collections.Generic;
using HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

namespace HousingRepairsOnlineApi.Domain.Boundaries;

public class RepairsHubCreationRequest
{
    public List<Reference> Reference { get; set; }
    public string DescriptionOfWork { get; set; }
    public Priority Priority { get; set; }
    public WorkClass WorkClass { get; set; }
    public List<WorkElement> WorkElement { get; set; }
    public Site Site { get; set; }
    public InstructedBy InstructedBy { get; set; }
    public AssignedToPrimary AssignedToPrimary { get; set; }
    public Customer Customer { get; set; }
    public BudgetCode BudgetCode { get; set; }
    public bool MultiTradeWorkOrder { get; set; }
}
