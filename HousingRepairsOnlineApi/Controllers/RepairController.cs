using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepairController : ControllerBase
    {
        private readonly ISaveRepairRequestUseCase saveRepairRequestUseCase;
        private readonly ISendInternalEmailUseCase sendInternalEmailUseCase;

        public RepairController(
            ISaveRepairRequestUseCase saveRepairRequestUseCase,
            ISendInternalEmailUseCase sendInternalEmailUseCase
            )
        {
            this.saveRepairRequestUseCase = saveRepairRequestUseCase;
            this.sendInternalEmailUseCase = sendInternalEmailUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRepair([FromBody] RepairRequest repairRequest)
        {
            try
            {
                var result = await saveRepairRequestUseCase.Execute(repairRequest);

                await sendInternalEmailUseCase.Execute(result.Id, result.Address.LocationId, result.Address.Display, result.SOR, result.Description.Text, result.ContactDetails?.Value, result.Description.Base64Image);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
