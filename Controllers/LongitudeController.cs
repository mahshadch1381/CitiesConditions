using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Db_CitiesProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LongitudeController : ControllerBase
    {
        private readonly WorldContext _Context;

        public LongitudeController(WorldContext dbContext)
        {
            _Context = dbContext;
        }
        // GET: api/<CountriesController>
        [HttpGet("GetAllLongitudes")]
        public async Task<ActionResult<List<OutputTables>>> Get()
        {
            var countryDto = await _Context.Longitudes
             .Select(c => new OutputTables
             {
                 Id = c.LongitudeId,
                 value = c.Longitude1
             })
             .ToListAsync();

            if (countryDto != null)
            {
                return Ok(countryDto);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
