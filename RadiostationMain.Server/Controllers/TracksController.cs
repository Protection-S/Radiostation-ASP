using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadiostationMain.Server.Infrastructure.Models;
using RadiostationMain.Server.Infrastructure;
using RadiostationMain.Server.Infrastructure.Database;

namespace RadiostationMain.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TracksController : ControllerBase
    {
        private readonly RadiostationContext _context;

        public TracksController(RadiostationContext context)
        {
            _context = context;
        }

        [HttpGet("{stationId}")]
        public async Task<IActionResult> GetTracksByStation(int stationId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Tracks/";
            var tracks = await _context.Tracks
                .Where(t => t.StationId == stationId)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Artist,
                    FileUrl = baseUrl + Path.GetFileName(t.FilePath),
                    t.StationId
                })
                .ToListAsync();

            return Ok(tracks);
        }


        [HttpPost]
        public async Task<IActionResult> AddTrack([FromForm] IFormFile file, [FromForm] string title, [FromForm] string artist, [FromForm] int stationId)
        {
            if (file == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(artist))
                return BadRequest("Все поля должны быть заполнены.");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Tracks");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var track = new Track
            {
                Title = title,
                Artist = artist,
                FilePath = filePath,
                StationId = stationId
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            return Ok(track);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null) return NotFound();

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
