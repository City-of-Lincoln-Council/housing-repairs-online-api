using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;
using HousingRepairsOnlineApi.Helpers;
using Outbound = HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;

namespace HousingRepairsOnlineApi.Mappers;

public class MapRepairsOnlineToRepairsHub : IMapRepairsOnlineToRepairsHub
{
    private readonly ISoREngine sorEngine;

    public MapRepairsOnlineToRepairsHub(ISoREngine sorEngine)
    {
        this.sorEngine = sorEngine;
    }

    public RepairsHubCreationRequest Map(RepairRequest repairRequest)
    {
        Guard.Against.InvalidInput(repairRequest.Description.Text, nameof(repairRequest.Description.Text), d => !string.IsNullOrEmpty(d));

        var sorCode = sorEngine.MapSorCode(repairRequest.Location.Value, repairRequest.Problem.Value, repairRequest.Issue?.Value);

        return new RepairsHubCreationRequest
        {
            Reference = new List<Outbound.Reference> { new Outbound.Reference { Id = Guid.NewGuid().ToString() } },
            DescriptionOfWork = repairRequest.Description.Text,
            Priority = new Outbound.Priority
            {
                PriorityCode = 4,
                PriorityDescription = "5 [N] NORMAL",
                NumberOfDays = 21,
                RequiredCompletionDateTime = DateTime.Now.AddDays(21),
            },
            WorkClass = new Outbound.WorkClass
            {
                WorkClassCode = 0
            },
            BudgetCode = new Outbound.BudgetCode
            {
                Id = "8"
            },
            MultiTradeWorkOrder = false,
            Site = new Outbound.Site
            {
                Property = new List<Outbound.Property>
                {
                    new Outbound.Property
                    {
                        PropertyReference = repairRequest.Address.LocationId,
                        Address = new Outbound.Address
                        {
                            AddressLine = new List<string>
                            {
                                repairRequest.Address.Display
                            },
                            PostalCode = repairRequest.Postcode
                        },
                        Reference = new List<Outbound.Reference>
                        {
                            new Outbound.Reference
                            {
                                Id = repairRequest.Address.LocationId
                            }
                        }
                    }
                }
            },
            AssignedToPrimary = new Outbound.AssignedToPrimary
            {
                Name = "HH General Building Repai",
                Organization = new Outbound.Organization
                {
                    Reference = new List<Outbound.Reference>
                    {
                        new Outbound.Reference
                        {
                            Id = "H01"
                        }
                    }
                }
            },
            InstructedBy = new Outbound.InstructedBy
            {
                Name = "Hackney Housing"
            },
            Customer = new Outbound.Customer
            {
                Name = string.Empty,
                Person = new Outbound.Person
                {
                    Name = new Outbound.Name
                    {
                        Full = ""
                    },
                    Communication = new List<Outbound.Communication>
                    {
                        new Outbound.Communication
                        {
                           Value = repairRequest.ContactPersonNumber,
                           Channel = new Outbound.Channel
                           {
                               Code = "60",
                               Medium = "20"
                           }
                        }
                    }
                }
            },
            WorkElement = new List<Outbound.WorkElement>
            {
                new Outbound.WorkElement
                {
                    RateScheduleItem = new List<Outbound.RateScheduleItem>
                    {
                        new Outbound.RateScheduleItem
                        {
                            Quantity = new Outbound.Quantity
                            {
                                Amount = new List<int>{ 1 }
                            },
                            CustomCode = sorCode,
                            CustomName = "TBC"
                        }
                    }
                }
            }
        };
    }
}
