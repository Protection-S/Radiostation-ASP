using System.ComponentModel.DataAnnotations;

namespace RadiostationMain.Server.Infrastructure.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public ICollection<Track> Tracks { get; set; } = new List<Track>();

    }
}
