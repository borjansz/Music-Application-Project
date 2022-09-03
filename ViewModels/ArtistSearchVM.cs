using RateYourMusicApp.Models;

namespace RateYourMusicApp.ViewModels
{
    public class ArtistSearchVM
    {
        public IList<Artist>? Artists { get; set; }
        public string? SearchStringName { get; set; }
    }
}
