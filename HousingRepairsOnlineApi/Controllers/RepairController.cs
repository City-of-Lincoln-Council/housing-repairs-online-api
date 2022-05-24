﻿using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route($"{Constants.ROUTE_PREFIX}[controller]")]
    [ApiVersion("1.0")]
    public class RepairController : ControllerBase
    {
        private readonly ISaveRepairRequestUseCase saveRepairRequestUseCase;
        private readonly IAppointmentConfirmationSender appointmentConfirmationSender;
        private readonly IBookAppointmentUseCase bookAppointmentUseCase;
        private readonly IMigrationToRepairHubUseCase migrationToRepairHubUseCase;
        private readonly IInternalEmailSender internalEmailSender;

        public RepairController(
            ISaveRepairRequestUseCase saveRepairRequestUseCase,
            IInternalEmailSender internalEmailSender,
            IAppointmentConfirmationSender appointmentConfirmationSender,
            IBookAppointmentUseCase bookAppointmentUseCase,
            IMigrationToRepairHubUseCase migrationToRepairHubUseCase
        )
        {
            this.saveRepairRequestUseCase = saveRepairRequestUseCase;
            this.internalEmailSender = internalEmailSender;
            this.appointmentConfirmationSender = appointmentConfirmationSender;
            this.bookAppointmentUseCase = bookAppointmentUseCase;
            this.migrationToRepairHubUseCase = migrationToRepairHubUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRepair([FromBody] RepairRequest repairRequest)
        {
            try
            {
                var migrationToRepairHubUseCaseResponse = await migrationToRepairHubUseCase.Execute(repairRequest);
                if (!migrationToRepairHubUseCaseResponse.Succeeded)
                {
                    SentrySdk.CaptureMessage($"Unable to migrate work order to Repairs Hub.");
                }

                var result = await saveRepairRequestUseCase.Execute(repairRequest, migrationToRepairHubUseCaseResponse.Id);
                var appointmentReponse = await bookAppointmentUseCase.Execute(result.Id, result.SOR, result.Address.LocationId,
                    result.Time.StartDateTime, result.Time.EndDateTime);

                var token = appointmentReponse.TokenId;

                appointmentConfirmationSender.Execute(result);
                await internalEmailSender.Execute(result);
                return Ok(result.Id);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
