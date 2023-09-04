using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DBFIRST_Cities3.Controllers
{
    [Authorize]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("ApiForAdmin")]
        public IActionResult ApiForAdmin()
        {
            // This action can be accessed by administrators only.
            return Ok("This is an API for administrators.");
        }
    }
}
