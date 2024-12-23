using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadiostationMain.Server.Infrastructure.Database;
using RadiostationMain.Server.Infrastructure.Models;

namespace RadiostationMain.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly RadiostationContext _context;

        public PlaylistsController(RadiostationContext context)
        {
            _context = context;
        }

        [HttpGet("{playlistId}/tracks")]
        public async Task<IActionResult> GetTracksByPlaylist(int playlistId)
        {
            var tracks = await _context.Tracks
                .Where(t => t.PlaylistId == playlistId)
                .Select(t => new { t.Id, t.Title, t.Artist })
                .ToListAsync();

            return Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] Playlist playlist)
        {
            if (playlist == null || string.IsNullOrWhiteSpace(playlist.Name) || playlist.StationId <= 0)
            {
                return BadRequest("Имя плейлиста и ID радиостанции обязательны.");
            }

            var station = await _context.Stations.FindAsync(playlist.StationId);
            if (station == null)
            {
                return BadRequest($"Радиостанция с ID {playlist.StationId} не найдена.");
            }

            try
            {
                _context.Playlists.Add(playlist);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreatePlaylist), new { id = playlist.Id }, playlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании плейлиста: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
                return NotFound();

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
