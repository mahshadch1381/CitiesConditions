

using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Db_CitiesProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {


        private readonly WorldContext _Context;

        public CountryController(WorldContext dbContext)
        {
            _Context = dbContext;
        }
        // GET: api/<CountriesController>
        [HttpGet("GetAllCountries")]
        public async Task<ActionResult<List<Country1>>> Get() {
           
             var countryDto = await _Context.Country1s
              .Select(c => new OutputTables
              {
                  Id = c.CountryId,
                  value = c.CountryName,
                  
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
        [HttpDelete("DeleteCity")]

        public IActionResult DeleteCountry(int id)
        {
            var country= _Context.Country1s.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            
            _Context.Country1s.Remove(country);

            _Context.SaveChanges();

            return NoContent();
        }
    }
        
    }

