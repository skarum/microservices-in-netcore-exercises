using Microsoft.AspNetCore.Mvc;

namespace Exercise_4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalloController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hallo world!";
        }
    }
}