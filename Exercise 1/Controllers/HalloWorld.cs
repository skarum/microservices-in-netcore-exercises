using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_1.Controllers
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