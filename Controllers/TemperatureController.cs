//using Db_CitiesProject2.Models;
using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Db_CitiesProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly WorldContext _Context;

        public TemperatureController(WorldContext dbContext)
        {
            _Context = dbContext;
        }
        // GET: api/<CountriesController>
        [HttpGet("GetAllTemperatures")]
        public async Task<ActionResult<List<OutputTables>>> Get()
        {
            var countryDto = await _Context.Temperatures
             .Select(c => new OutputTables
             {
                 Id = c.TempId,
                 value = Convert.ToString(c.Temperature1)
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
