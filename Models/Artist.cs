using System.ComponentModel.DataAnnotations;

namespace RateYourMusicApp.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        [Display(Name = "Date of Birth/Formation")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
               
        public ICollection<Album>? Albums { get; set; }
        public ICollection<ArtistSong>? Songs { get; set; }  

       

    }
}
