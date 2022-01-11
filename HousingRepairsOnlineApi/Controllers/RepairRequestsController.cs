using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepairRequestsController : ControllerBase
    {
        private readonly ISaveRepairRequestUseCase saveRepairRequestUseCase;

        public RepairRequestsController(ISaveRepairRequestUseCase saveRepairRequestUseCase)
        {
            this.saveRepairRequestUseCase = saveRepairRequestUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRepairRequests([FromBody] RepairRequest repairRequest)
        {
            try
            {
                var result = await saveRepairRequestUseCase.Execute(repairRequest);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
