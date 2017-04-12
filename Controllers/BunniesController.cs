using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BunniesAPI.Controllers
{
    [Route("bunnies-api/v1/[controller]")]
    public class BunniesController : Controller
    {
        private BunniesCollection bunnies;

        public BunniesController(IMemoryCache memoryCache)
        {
            bunnies = new BunniesCollection(memoryCache);
        }

        [HttpGet]
        public IActionResult GetBunnies()
        {
            return Ok(bunnies);
        }

        [HttpGet("{id}")]
        public IActionResult GetBunny(string id)
        {
            foreach (Bunny bunny in bunnies)
            {
                if (bunny.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase))
                {
                    return Ok(bunny);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody]Bunny value)
        {
            try
            {
                if (bunnies.Contains(value))
                {
                    return StatusCode((int)HttpStatusCode.Conflict);
                }
                value.Id = Guid.NewGuid().ToString();
                bunnies.Add(value);
                return Created(Url.Action("GetBunny"), value.Id);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
