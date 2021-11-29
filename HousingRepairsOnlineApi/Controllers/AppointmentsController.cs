using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> AvailableAppointments([FromQuery] string repairCode, [FromQuery] string uprn)
        {
            return Ok();
        }
    }
}
