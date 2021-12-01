﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HACT.Dtos;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IRetrieveAvailableAppointmentsUseCase
    {
        public Task<IEnumerable<Appointment>> Execute(string repairLocation, string repairProblem, string repairIssue, string uprn);
    }
}
