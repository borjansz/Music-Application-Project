using RateYourMusicApp.Models;
using System.ComponentModel.DataAnnotations;

namespace RateYourMusicApp.ViewModels
{
    public class SongPictureVM
    {
        public Song? Song { get; set; }

        [Display(Name = "Upload")]
        public IFormFile? SongPictureFile { get; set; }

        [Display(Name = "Picture")]
        public string? SongPictureName { get; set; }
    }
}
