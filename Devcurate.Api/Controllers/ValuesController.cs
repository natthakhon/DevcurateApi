using Devcurate.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devcurate.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok();
        }

        //baseurl/api/add/
        //json sample => {"value":"whatever"}        
        [HttpPost("add")]
        public async Task<ActionResult<long>> Add(Model value)
        {
            return Ok(await Main.Add(value.Value));
        }

        //baseurl/api/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(long id)
        {
            return Ok(await Main.Get(id));
        }

        //baseurl/api/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await Main.Delete(id);
            return Ok();
        }
    }
}
