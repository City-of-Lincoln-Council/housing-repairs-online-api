using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepairController : ControllerBase
    {
        private readonly ISaveRepairRequestUseCase saveRepairRequestUseCase;
        private readonly ISendAppointmentConfirmationEmailUseCase sendAppointmentConfirmationEmailUseCase;
        private readonly ISendAppointmentConfirmationSmsUseCase sendAppointmentConfirmationSmsUseCase;

        public RepairController(
            ISaveRepairRequestUseCase saveRepairRequestUseCase,
            ISendAppointmentConfirmationEmailUseCase sendAppointmentConfirmationEmailUseCase,
            ISendAppointmentConfirmationSmsUseCase sendAppointmentConfirmationSmsUseCase
            )
        {
            this.saveRepairRequestUseCase = saveRepairRequestUseCase;
            this.sendAppointmentConfirmationEmailUseCase = sendAppointmentConfirmationEmailUseCase;
            this.sendAppointmentConfirmationSmsUseCase = sendAppointmentConfirmationSmsUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRepair([FromBody] RepairRequest repairRequest)
        {
            try
            {
                var result = await saveRepairRequestUseCase.Execute(repairRequest);
                switch (repairRequest?.ContactDetails?.Type)
                {
                    case "email":
                        await sendAppointmentConfirmationEmailUseCase.Execute(repairRequest.ContactDetails.Value, result,
                            repairRequest.Time.Display);
                        break;
                    case "sms":
                        await sendAppointmentConfirmationSmsUseCase.Execute(repairRequest.ContactDetails.Value, result,
                            repairRequest.Time.Display);
                        break;
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
