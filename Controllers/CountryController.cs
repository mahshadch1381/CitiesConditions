

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

/*
        private readonly WorldContext _dbContext;

        public CountryController(WorldContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/<CountriesController>
        [HttpGet("GetAllCountries")]
        public async Task<ActionResult<List<Country1>>> Get() {
            var countryDto = await _dbContext.Country1s.Select(c => new Country1
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName,
                
            })
               .ToListAsync();
            return Ok(countryDto);
        }


        // GET api/<CountriesController>/5
        [HttpGet("GetCountry")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CountriesController>
        [HttpPost]
        public void Post([FromBody] Country1 input)
        {
            if (input == null)
            {
                return;
            }

            var newCountry = new Country1
            {
                CountryName = input.CountryName
               
            };

            _dbContext.Country1s.Add(newCountry);
            _dbContext.SaveChanges();

            
        }

        // PUT api/<CountriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CountriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
