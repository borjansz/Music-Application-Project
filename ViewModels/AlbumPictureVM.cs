using RateYourMusicApp.Models;
using System.ComponentModel.DataAnnotations;

namespace RateYourMusicApp.ViewModels
{
    public class AlbumPictureVM
    {
        public Album? Album  { get; set; }

        [Display(Name = "Upload")]
        public IFormFile? AlbumPictureFile { get; set; }

        [Display(Name = "Picture")]
        public string? AlbumPictureName { get; set; }
    }
}
