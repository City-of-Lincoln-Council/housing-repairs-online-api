using System.Threading.Tasks;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IRetrieveAvailableAppointmentsUseCase retrieveAvailableAppointmentsUseCase;

        public AppointmentsController(IRetrieveAvailableAppointmentsUseCase retrieveAvailableAppointmentsUseCase)
        {
            this.retrieveAvailableAppointmentsUseCase = retrieveAvailableAppointmentsUseCase;
        }

        [HttpGet]
        [Route("AvailableAppointments")]
        public async Task<IActionResult> AvailableAppointments([FromQuery] string repairLocation, [FromQuery] string repairProblem, [FromQuery] string repairIssue, [FromQuery] string locationId)
        {
            var result = await retrieveAvailableAppointmentsUseCase.Execute(repairLocation, repairProblem, repairIssue, locationId);
            return Ok(result);
        }
    }
}
