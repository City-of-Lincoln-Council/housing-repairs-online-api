using System.Threading.Tasks;
using HousingRepairsOnlineApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentifierValidator identifierValidator;

        public AuthenticationController(IIdentifierValidator identifierValidator)
        {
            this.identifierValidator = identifierValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string identifier)
        {
            IActionResult result;
            if (!identifierValidator.Validate(identifier))
            {
                result = Unauthorized();
            }
            else
            {
                result =  Ok("token");
            }

            return await Task.FromResult(result);
        }
    }
}
