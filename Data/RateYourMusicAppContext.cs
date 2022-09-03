using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RateYourMusicApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RateYourMusicApp.Areas.Identity.Data;


namespace RateYourMusicApp.Data
{
    public class RateYourMusicAppContext : IdentityDbContext<RateYourMusicAppUser>
    {
        public RateYourMusicAppContext (DbContextOptions<RateYourMusicAppContext> options)
            : base(options)
        {
        }

        public DbSet<RateYourMusicApp.Models.Song> Song { get; set; }

        public DbSet<RateYourMusicApp.Models.Artist> Artist { get; set; }

        public DbSet<RateYourMusicApp.Models.Album> Album { get; set; }
        public DbSet<RateYourMusicApp.Models.ArtistSong> ArtistSong { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ArtistSong>()
            .HasOne<Artist>(p => p.Artist)
            .WithMany(p => p.Songs)
            .HasForeignKey(p => p.ArtistId);
            //.HasPrincipalKey(p => p.Id);
            builder.Entity<ArtistSong>()
            .HasOne<Song>(p => p.Song)
            .WithMany(p => p.Artists)
            .HasForeignKey(p => p.SongId);
            //.HasPrincipalKey(p => p.Id);
            builder.Entity<Album>()
            .HasOne<Artist>(p => p.Artist)
            .WithMany(p => p.Albums)
            .HasForeignKey(p => p.ArtistId);
            //.HasPrincipalKey(p => p.Id);
            builder.Entity<Song>()
            .HasOne<Album>(p => p.Album)
            .WithMany(p => p.Songs)
            .HasForeignKey(p => p.AlbumId);
        }

    }
}
