using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IInternalEmailSender internalEmailSender;
        private readonly ILogger<RepairController> logger;

        public RepairController(
            ISaveRepairRequestUseCase saveRepairRequestUseCase,
            IInternalEmailSender internalEmailSender,
            IAppointmentConfirmationSender appointmentConfirmationSender,
            IBookAppointmentUseCase bookAppointmentUseCase,
            ILogger<RepairController> logger
        )
        {
            this.saveRepairRequestUseCase = saveRepairRequestUseCase;
            this.internalEmailSender = internalEmailSender;
            this.appointmentConfirmationSender = appointmentConfirmationSender;
            this.bookAppointmentUseCase = bookAppointmentUseCase;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRepair([FromBody] RepairRequest repairRequest)
        {
            logger.LogInformation($"Saving repair for location ID {repairRequest.Address?.LocationId}");

            try
            {
                var result = await saveRepairRequestUseCase.Execute(repairRequest);
                await bookAppointmentUseCase.Execute(result.Id, result.SOR, result.Address.LocationId,
                    result.Time.StartDateTime, result.Time.EndDateTime);
                appointmentConfirmationSender.Execute(result);
                await internalEmailSender.Execute(result);

                logger.LogInformation($"Saved repair ID {result.Id}");

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
