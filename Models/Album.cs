namespace RateYourMusicApp.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ArtistId { get; set; }
        public Artist? Artist { get; set; }  
        public int? YearAlbum { get; set; }
        public string? GenreAlbum { get; set; }
        public string? ProductionCompany { get; set; }
        public string? profilePicture { get; set; }
        public ICollection<Song>? Songs { get; set; }

    }
}
