using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadiostationMain.Server.Infrastructure.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public int StationId { get; set; }

        [NotMapped]
        public Station Station { get; set; }

        public ICollection<Track> Tracks { get; set; } = new List<Track>();
    }
}
