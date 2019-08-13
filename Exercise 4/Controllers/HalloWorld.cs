using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            return Ok("Hallo world!");
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            var requestETag = "";
            const string eTagHeaderName = "ETag";
            
            if (Request.Headers.ContainsKey(eTagHeaderName))
            {
                requestETag = Request.Headers[eTagHeaderName].First();
            }

            var responseETag = JsonConvert.SerializeObject(id);

            if (Request.Headers.ContainsKey(eTagHeaderName) && responseETag == requestETag)
            {
                return StatusCode(304);
            }

            Response.Headers.Add("ETag", responseETag);
            Response.Headers.Add("cache-control", "private, max-age:3600");
            return Ok(id);

        }
    }
}