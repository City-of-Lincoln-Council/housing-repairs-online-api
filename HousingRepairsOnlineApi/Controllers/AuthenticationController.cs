using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HousingRepairsOnlineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string identifier;

        public AuthenticationController(string identifier)
        {
            this.identifier = identifier;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string identifier)
        {
            IActionResult result;
            if (string.IsNullOrWhiteSpace(identifier) || this.identifier != identifier)
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
