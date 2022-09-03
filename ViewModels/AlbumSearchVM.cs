using Microsoft.AspNetCore.Mvc.Rendering;
using RateYourMusicApp.Models;

namespace RateYourMusicApp.ViewModels
{
    public class AlbumSearchVM
    {
        public IList<Album>? Albums { get; set; }

        public string? SearchStringName { get; set; }
        public string? SearchStringArtist { get; set; }
        public SelectList? Zanr { get; set; }
        public string? SongZanr { get; set; }

    }
}
