using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HACT.Dtos;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.Tests.ControllersTests;

namespace HousingRepairsOnlineApi.UseCases
{
    public class RetrieveAvailableAppointmentsUseCase : IRetrieveAvailableAppointmentsUseCase
    {
        private readonly IAppointmentsGateway appointmentsGateway;
        private readonly ISoREngine sorEngine;

        public RetrieveAvailableAppointmentsUseCase(IAppointmentsGateway appointmentsGateway, ISoREngine sorEngine)
        {
            this.appointmentsGateway = appointmentsGateway;
            this.sorEngine = sorEngine;
        }

        public async Task<IEnumerable<Appointment>> Execute(string repairLocation, string repairProblem, string repairIssue, string uprn)
        {
            Guard.Against.NullOrWhiteSpace(repairLocation, nameof(repairLocation));
            Guard.Against.NullOrWhiteSpace(repairProblem, nameof(repairProblem));
            Guard.Against.NullOrWhiteSpace(repairIssue, nameof(repairIssue));
            Guard.Against.NullOrWhiteSpace(uprn, nameof(uprn));
            var repairCode = sorEngine.MapSorCode(repairLocation, repairProblem, repairIssue);
            var result = await appointmentsGateway.GetAvailableAppointments(repairCode, uprn);
            return null;
        }
    }
}
