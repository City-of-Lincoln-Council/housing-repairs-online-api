﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HACT.Dtos;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Helpers;
using ApplicationTime = HousingRepairsOnlineApi.Domain.AppointmentTime;

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

        public async Task<List<ApplicationTime>> Execute(string repairLocation, string repairProblem,
            string repairIssue, string uprn)
        {
            Guard.Against.NullOrWhiteSpace(repairLocation, nameof(repairLocation));
            Guard.Against.NullOrWhiteSpace(repairProblem, nameof(repairProblem));
            Guard.Against.NullOrWhiteSpace(repairIssue, nameof(repairIssue));
            Guard.Against.NullOrWhiteSpace(uprn, nameof(uprn));
            var repairCode = sorEngine.MapSorCode(repairLocation, repairProblem, repairIssue);

            var convertedResults = new List<ApplicationTime>();

            var result = await appointmentsGateway.GetAvailableAppointments(repairCode, uprn);
            convertedResults.AddRange(result.Select(ConvertToHactAppointment));

            return convertedResults;

            ApplicationTime ConvertToHactAppointment(Appointment appointment)
            {

                return new ApplicationTime
                {
                    Id = appointment.Reference,
                    StartTime = appointment.TimeOfDay.EarliestArrivalTime,
                    EndTime = appointment.TimeOfDay.LatestArrivalTime
                };
            }
        }
    }
}
