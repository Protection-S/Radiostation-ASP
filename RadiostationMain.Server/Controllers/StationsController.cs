using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadiostationMain.Server.Infrastructure.Database;
using RadiostationMain.Server.Infrastructure.Models;

namespace RadiostationMain.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly RadiostationContext _dbContext;

        public StationsController(RadiostationContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStations()
        {
            var stations = await _dbContext.Stations
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            return Ok(stations);
        }

        [HttpPost]
        public async Task<ActionResult<Station>> CreateStation([FromBody] StationCreateDTO newStation)
        {
            if (string.IsNullOrWhiteSpace(newStation.Name))
            {
                return BadRequest("Название радиостанции не может быть пустым.");
            }

            var station = new Station
            {
                Name = newStation.Name
            };

            try
            {
                _dbContext.Stations.Add(station);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStations), new { id = station.Id }, station);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании радиостанции: {ex.Message}");
            }
        }

    }
}
