namespace RateYourMusicApp.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Single { get; set; }
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }
        public string? GenreSong { get; set; }
        public int? YearSong { get; set; }
        public string? ProductionCompany { get; set; }
        public string? profilePicture { get; set; }
        public string? SongUrl { get; set; }

        public ICollection<ArtistSong>? Artists { get; set; }
    }
}
