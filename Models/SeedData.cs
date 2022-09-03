using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateYourMusicApp.Areas.Identity.Data;
using RateYourMusicApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RateYourMusicApp.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<RateYourMusicAppUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            RateYourMusicAppUser user = await UserManager.FindByEmailAsync("admin@musicapp.com");

            if (user == null)
            {
                var User = new RateYourMusicAppUser();
                User.Email = "admin@musicapp.com";
                User.UserName = "admin@musicapp.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            //Add Guest Role
            roleCheck = await RoleManager.RoleExistsAsync("Guest");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Guest")); }
            user = await UserManager.FindByEmailAsync("guest@musicapp.com");
            if (user == null)
            {
                var User = new RateYourMusicAppUser();
                User.Email = "guest@musicapp.com";
                User.UserName = "guest@musicapp.com";
                string userPWD = "Guest123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Teacher
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Guest"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RateYourMusicAppContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<RateYourMusicAppContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                // Look for any movies.
                if (context.Song.Any() || context.Album.Any() || context.Artist.Any())
                {
                    return; // DB has been seeded
                }
                context.Artist.AddRange(
                new Artist { /*Id = 1, */FullName = "Daft Punk", DateOfBirth = DateTime.Parse("1993-3-6"), Nationality="French"},
                new Artist { /*Id = 2, */FullName = "Amelie Lens", DateOfBirth = DateTime.Parse("1990-5-31"), Nationality = "Belgian" },
                new Artist { /*Id = 3, */FullName = "Joji", DateOfBirth = DateTime.Parse("1992-9-18"), Nationality = "Japanese" },
                new Artist { /*Id = 4, */FullName = "Jimi Hendrix", DateOfBirth = DateTime.Parse("1947-11-27"), Nationality = "American" },
                new Artist { /*Id = 5, */FullName = "Leon Bridges", DateOfBirth = DateTime.Parse("1989-7-13"), Nationality = "American" },
                new Artist { /*Id = 6, */FullName = "Sticky Fingers", DateOfBirth = DateTime.Parse("2008-7-13"), Nationality = "Australian" },
                new Artist { /*Id = 6, */FullName = "Julian Casablancas", DateOfBirth = DateTime.Parse("1978-8-23"), Nationality = "American" },
                new Artist { /*Id = 6, */FullName = "Pharrell Williams", DateOfBirth = DateTime.Parse("1973-4-5"), Nationality = "American" },
                new Artist { /*Id = 6, */FullName = "Nile Rodgers", DateOfBirth = DateTime.Parse("1952-9-19"), Nationality = "American" },
                new Artist { /*Id = 6, */FullName = "Giorgio Moroder", DateOfBirth = DateTime.Parse("1940-4-26"), Nationality = "Italian" }
                );
                context.SaveChanges();
               
                context.Album.AddRange(
                new Album
                {
                    //Id = 1,
                    Name = "Caress Your Soul",
                    YearAlbum = 2013,
                    GenreAlbum = "Indie Rock",
                    ProductionCompany = "Sureshaker",
                    ArtistId = context.Artist.Single(d => d.FullName == "Sticky Fingers").Id
                },
                new Album
                {
                    //Id = 2,
                    Name = "Higher EP",
                    YearAlbum = 2020,
                    GenreAlbum = "Techno",
                    ProductionCompany = "LENSKE",
                    ArtistId = context.Artist.Single(d => d.FullName == "Amelie Lens").Id
                },
                new Album
                {
                    //Id = 3,
                    Name = "Random Access Memories",
                    YearAlbum = 2013,
                    GenreAlbum = "Alternative/Indie",
                    ProductionCompany = "Columbia Records",
                    ArtistId = context.Artist.Single(d => d.FullName == "Daft Punk").Id
                },
                new Album
                {
                    //Id = 4,
                    Name = "Nectar",
                    YearAlbum = 2020,
                    GenreAlbum = "R&B/Soul",
                    ProductionCompany = "88rising Music/Warner Records",
                    ArtistId = context.Artist.Single(d => d.FullName == "Joji").Id
                },
                new Album
                {
                    //Id = 5,
                    Name = "Woodstock",
                    YearAlbum = 1994,
                    GenreAlbum = "Rock",
                    ProductionCompany = "MCA Records",
                    ArtistId = context.Artist.Single(d => d.FullName == "Jimi Hendrix").Id
                },
                new Album
                {
                    //Id = 6,
                    Name = "Texas Sun",
                    YearAlbum = 2020,
                    GenreAlbum = "Alternative/Indie",
                    ProductionCompany = "Dead Oceans",
                    ArtistId = context.Artist.Single(d => d.FullName == "Leon Bridges").Id
                }
                );
                context.SaveChanges();

               context.Song.AddRange(
               new Song
               {
                    //Id = 1,
                   Name = "Texas Sun",
                   YearSong = 2020,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Dead Oceans",
                   AlbumId = context.Album.Single(d => d.Name == "Texas Sun").Id
               },
               new Song
               {
                   //Id = 2,
                   Name = "Midnight",
                   YearSong = 2020,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Dead Oceans",
                   AlbumId = context.Album.Single(d => d.Name == "Texas Sun").Id
               },
               new Song
               {
                   //Id = 3,
                   Name = "C-Side",
                   YearSong = 2020,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Dead Oceans",
                   AlbumId = context.Album.Single(d => d.Name == "Texas Sun").Id
               },
               new Song
               {
                   //Id = 4,
                   Name = "Conversion",
                   YearSong = 2020,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Dead Oceans",
                   AlbumId = context.Album.Single(d => d.Name == "Texas Sun").Id
               },
               new Song
               {
                    //Id = 5,
                    Name = "Higher",
                   YearSong = 2020,
                   GenreSong = "Techno",
                   ProductionCompany = "LENSKE",
                   AlbumId = context.Album.Single(d => d.Name == "Higher EP").Id
               },
               new Song
               {
                   //Id = 6,
                   Name = "L'Obscurité",
                   YearSong = 2020,
                   GenreSong = "Techno",
                   ProductionCompany = "LENSKE",
                   AlbumId = context.Album.Single(d => d.Name == "Higher EP").Id
               },
               new Song
               {
                   //Id = 7,
                   Name = "Higher (FJAAK Remix)",
                   YearSong = 2020,
                   GenreSong = "Techno",
                   ProductionCompany = "LENSKE",
                   AlbumId = context.Album.Single(d => d.Name == "Higher EP").Id
               },
               new Song
               {
                    //Id = 8,
                    Name = "Giorgio by Moroder",
                   YearSong = 2013,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Columbia Records",
                   AlbumId = context.Album.Single(d => d.Name == "Random Access Memories").Id
               },
               new Song
               {
                   //Id = 9,
                   Name = "Instant Crush",
                   YearSong = 2013,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Columbia Records",
                   AlbumId = context.Album.Single(d => d.Name == "Random Access Memories").Id
               },
               new Song
               {
                   //Id = 10,
                   Name = "Lose Yourself to Dance",
                   YearSong = 2013,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Columbia Records",
                   AlbumId = context.Album.Single(d => d.Name == "Random Access Memories").Id
               },
               new Song
               {
                   //Id = 11,
                   Name = "Beyond",
                   YearSong = 2013,
                   GenreSong = "Alternative/Indie",
                   ProductionCompany = "Columbia Records",
                   AlbumId = context.Album.Single(d => d.Name == "Random Access Memories").Id
               },

               new Song
               {
                    //Id = 12,
                   Name = "Gimme Love",
                   YearSong = 2020,
                   GenreSong = "R&B/Soul",
                   ProductionCompany = "88rising Music/Warner Records",
                   AlbumId = context.Album.Single(d => d.Name == "Nectar").Id
               },
               new Song
               {
                   //Id = 13,
                   Name = "Run",
                   YearSong = 2020,
                   GenreSong = "R&B/Soul",
                   ProductionCompany = "88rising Music/Warner Records",
                   AlbumId = context.Album.Single(d => d.Name == "Nectar").Id
               },
               new Song
               {
                   //Id = 14,
                   Name = "Sanctuary",
                   YearSong = 2020,
                   GenreSong = "R&B/Soul",
                   ProductionCompany = "88rising Music/Warner Records",
                   AlbumId = context.Album.Single(d => d.Name == "Nectar").Id
               },
               new Song
               {
                    //Id = 15,
                    Name = "Fire",
                   YearSong = 1994,
                   GenreSong = "Rock",
                   ProductionCompany = "MCA Records",
                   AlbumId = context.Album.Single(d => d.Name == "Woodstock").Id
               },
               new Song
               {
                   //Id = 16,
                   Name = "Red House",
                   YearSong = 1994,
                   GenreSong = "Rock",
                   ProductionCompany = "MCA Records",
                   AlbumId = context.Album.Single(d => d.Name == "Woodstock").Id
               },
               new Song
               {
                   //Id = 17,
                   Name = "Purple Haze",
                   YearSong = 1994,
                   GenreSong = "Rock",
                   ProductionCompany = "MCA Records",
                   AlbumId = context.Album.Single(d => d.Name == "Woodstock").Id
               },
                new Song
                {
                    //Id = 18,
                    Name = "Clouds & Cream",
                    YearSong = 2013,
                    GenreSong = "Indie Rock",
                    ProductionCompany = "Sureshaker",
                    AlbumId = context.Album.Single(d => d.Name == "Caress Your Soul").Id
                },
                new Song
                {
                    //Id = 19,
                    Name = "Bootleg Rascal",
                    YearSong = 2013,
                    GenreSong = "Indie Rock",
                    ProductionCompany = "Sureshaker",
                    AlbumId = context.Album.Single(d => d.Name == "Caress Your Soul").Id
                },
                new Song
                {
                    //Id = 20,
                    Name = "Sex",
                    YearSong = 2013,
                    GenreSong = "Indie Rock",
                    ProductionCompany = "Sureshaker",
                    AlbumId = context.Album.Single(d => d.Name == "Caress Your Soul").Id
                },
                new Song
                {
                    //Id = 21,
                    Name = "Caress Your Soul",
                    YearSong = 2013,
                    GenreSong = "Indie Rock",
                    ProductionCompany = "Sureshaker",
                    AlbumId = context.Album.Single(d => d.Name == "Caress Your Soul").Id
                },
             new Song
             {
                 //Id = 21,
                 Name = "Hypnotized",
                 YearSong = 2019,
                 GenreSong = "Techno, Acid",
                 ProductionCompany = "Second State"
             }
               );
                context.SaveChanges();


                context.ArtistSong.AddRange
                (
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Sticky Fingers").Id, SongId = context.Song.Single (d => d.Name == "Caress Your Soul").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Sticky Fingers").Id, SongId = context.Song.Single(d => d.Name == "Sex").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Sticky Fingers").Id, SongId = context.Song.Single(d => d.Name == "Bootleg Rascal").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Sticky Fingers").Id, SongId = context.Song.Single(d => d.Name == "Clouds & Cream").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Jimi Hendrix").Id, SongId = context.Song.Single(d => d.Name == "Purple Haze").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Jimi Hendrix").Id, SongId = context.Song.Single(d => d.Name == "Red House").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Jimi Hendrix").Id, SongId = context.Song.Single(d => d.Name == "Fire").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Joji").Id, SongId = context.Song.Single(d => d.Name == "Sanctuary").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Joji").Id, SongId = context.Song.Single(d => d.Name == "Run").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Joji").Id, SongId = context.Song.Single(d => d.Name == "Gimme Love").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Daft Punk").Id, SongId = context.Song.Single(d => d.Name == "Beyond").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Daft Punk").Id, SongId = context.Song.Single(d => d.Name == "Lose Yourself to Dance").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Daft Punk").Id, SongId = context.Song.Single(d => d.Name == "Instant Crush").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Daft Punk").Id, SongId = context.Song.Single(d => d.Name == "Giorgio by Moroder").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Julian Casablancas").Id, SongId = context.Song.Single(d => d.Name == "Instant Crush").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Pharrell Williams").Id, SongId = context.Song.Single(d => d.Name == "Lose Yourself to Dance").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Giorgio Moroder").Id, SongId = context.Song.Single(d => d.Name == "Giorgio by Moroder").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Amelie Lens").Id, SongId = context.Song.Single(d => d.Name == "Higher (FJAAK Remix)").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Amelie Lens").Id, SongId = context.Song.Single(d => d.Name == "L'Obscurité").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Amelie Lens").Id, SongId = context.Song.Single(d => d.Name == "Higher").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Leon Bridges").Id, SongId = context.Song.Single(d => d.Name == "Conversion").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Leon Bridges").Id, SongId = context.Song.Single(d => d.Name == "C-Side").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Leon Bridges").Id, SongId = context.Song.Single(d => d.Name == "Midnight").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Leon Bridges").Id, SongId = context.Song.Single(d => d.Name == "Texas Sun").Id },
                new ArtistSong { ArtistId = context.Artist.Single(d => d.FullName == "Amelie Lens").Id, SongId = context.Song.Single(d => d.Name == "Hypnotized").Id }
                );
                context.SaveChanges();
            }
        }

    }
}
