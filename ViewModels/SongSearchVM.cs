using Microsoft.AspNetCore.Mvc.Rendering;
using RateYourMusicApp.Models;

namespace RateYourMusicApp.ViewModels
{
    public class SongSearchVM
    {
        public IList<Song>? Songs { get; set; }
        public IList<Album>? Albums{ get; set; }
        public string? SearchStringName { get; set; }
        public string? SearchStringAlbum { get; set; }
        public SelectList? Zanr { get; set; }
        public string? SongZanr { get; set; }
    }
}
