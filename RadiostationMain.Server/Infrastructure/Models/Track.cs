using System.ComponentModel.DataAnnotations;

namespace RadiostationMain.Server.Infrastructure.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Artist name cannot be longer than 100 characters.")]
        public string Artist { get; set; }

        [Required]
        public string FilePath { get; set; }
         
        public int StationId { get; set; }

        public Station Station { get; set; }
    }
}
