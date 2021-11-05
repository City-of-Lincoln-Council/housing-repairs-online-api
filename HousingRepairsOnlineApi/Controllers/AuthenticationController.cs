using System.Threading.Tasks;
using HousingRepairsOnlineApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentifierValidator identifierValidator;
        private readonly IJwtTokenHelper jwtTokenHelper;

        public AuthenticationController(IIdentifierValidator identifierValidator, IJwtTokenHelper jwtTokenHelper)
        {
            this.identifierValidator = identifierValidator;
            this.jwtTokenHelper = jwtTokenHelper;
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
                var token = jwtTokenHelper.Generate();
                result = Ok(token);
            }

            return await Task.FromResult(result);
        }
    }
}
