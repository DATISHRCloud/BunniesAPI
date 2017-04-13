using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BunniesAPI.Controllers
{
    [Route("bunnies-api/v1/[controller]")]
    public class BunniesController : Controller
    {
        private BunniesCollection bunnies;

        ///<summary>
        ///Bunnies controller
        ///</summary>
        public BunniesController(IMemoryCache memoryCache)
        {
            bunnies = new BunniesCollection(memoryCache);
        }

        ///<summary>
        /// Gets a bundle of bunnies!
        ///</summary>
        ///<returns>Cuteness overload!</returns>
        ///<response code="200">Look at all the bunnies! (❤ω❤)</response>
        ///<response code="404">Where did all the bunnies go??</response>
        [HttpGet]
        [ProducesResponseType(typeof(BunniesCollection), 200)]
        [ProducesResponseType(typeof(BunniesCollection), 404)]
        public IActionResult GetBunnies(    )
        {
            return Ok(bunnies);
        }

        ///<summary>
        /// Gets a cute little furball by id
        ///</summary>
        ///<param name="id">The UID of the bunny</param>
        ///<returns>I'll give you 3 guesses</returns>
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

        ///<summary>
        /// Creates a new furry friend
        ///</summary>
        ///<remarks>
        /// You can set the ID property, but I'll just ignore it.
        ///</remarks>
        ///<param name="value">The adorable new baby bunny</param>
        ///<response code="201">A new bunny is born!</response>
        ///<response code="400">You're not very good at making bunnies...</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Post([FromBody, Required]Bunny value)
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
