using System.Threading.Tasks;
using HousingRepairsOnlineApi.Tests.ControllersTests;
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
        public async Task<IActionResult> AvailableAppointments([FromQuery] string repairLocation, [FromQuery] string repairProblem, [FromQuery] string repairIssue, [FromQuery] string uprn)
        {
            var result = await retrieveAvailableAppointmentsUseCase.Execute(repairLocation, repairProblem, repairIssue, uprn);
            return Ok(result);
        }
    }
}
