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
        private readonly IRetrieveImageLinkUseCase retrieveImageLinkUseCase;

        public RepairController(
            ISaveRepairRequestUseCase saveRepairRequestUseCase,
            ISendInternalEmailUseCase sendInternalEmailUseCase,
            IRetrieveImageLinkUseCase retrieveImageLinkUseCase
            )
        {
            this.saveRepairRequestUseCase = saveRepairRequestUseCase;
            this.sendInternalEmailUseCase = sendInternalEmailUseCase;
            this.retrieveImageLinkUseCase = retrieveImageLinkUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRepair([FromBody] RepairRequest repairRequest)
        {
            try
            {
                var result = await saveRepairRequestUseCase.Execute(repairRequest);
                var imageLink = await retrieveImageLinkUseCase.Execute(result.Description.PhotoUrl);
                await sendInternalEmailUseCase.Execute(result.Id, result.Address.LocationId, result.Address.Display, result.SOR, result.Description.Text, result.ContactDetails?.Value, imageLink );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
