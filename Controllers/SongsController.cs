using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RateYourMusicApp.Data;
using RateYourMusicApp.Models;
using RateYourMusicApp.ViewModels;

namespace RateYourMusicApp.Controllers
{
    public class SongsController : Controller
    {
        private readonly RateYourMusicAppContext _context;

        public SongsController(RateYourMusicAppContext context)
        {
            _context = context;
        }

        // GET: Songs
        public async Task<IActionResult> Index(string searchStringName, string searchStringAlbum, string songZanr)
        {
            IQueryable<Song> songsQuery = _context.Song.Include(s => s.Album).Include(s => s.Artists).ThenInclude(s => s.Artist).AsQueryable();
            IQueryable<string> zanrQuery = _context.Song.OrderBy(m => m.GenreSong).Select(m => m.GenreSong).Distinct();
   
            if (!string.IsNullOrEmpty(searchStringName))
            {
                songsQuery = songsQuery.Where(s => s.Name.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringAlbum))
            {
                songsQuery = songsQuery.Where(s => s.Album.Name.Contains(searchStringAlbum));
            }
            if (!string.IsNullOrEmpty(songZanr))
            {
                songsQuery = songsQuery.Where(s => s.GenreSong == songZanr);
            }
            var songSearchVM = new SongSearchVM
            {
                Songs = await songsQuery.ToListAsync(),
                Zanr = new SelectList(await zanrQuery.ToListAsync()),
            };
            //var rateYourMusicAppContext = _context.Song.Include(s => s.Album).Include(s => s.Artists).ThenInclude(m => m.Artist);
            return View(songSearchVM);
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Song == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .Include(s => s.Album)
                .Include(s => s.Artists).ThenInclude(s => s.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }
            SongPictureVM viewmodel = new SongPictureVM
            {
                Song = song,
                SongPictureName = song.profilePicture
            };

            return View(viewmodel);
        }

        // GET: Songs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Set<Album>(), "Id", "Id");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Single,AlbumId,GenreSong,YearSong,ProductionCompany")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Set<Album>(), "Id", "Id", song.AlbumId);
            return View(song);
        }

        // GET: Songs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Song == null)
            {
                return NotFound();
            }

            //var song = await _context.Song.FindAsync(id);
            var song = await _context.Song.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Set<Album>(), "Id", "Id", song.AlbumId);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Single,AlbumId,GenreSong,YearSong,ProductionCompany")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Set<Album>(), "Id", "Id", song.AlbumId);
            return View(song);
        }

        // GET: Songs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Song == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .Include(s => s.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Song == null)
            {
                return Problem("Entity set 'RateYourMusicAppContext.Song'  is null.");
            }
            var song = await _context.Song.FindAsync(id);
            if (song != null)
            {
                _context.Song.Remove(song);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SongsByArtist(int? id, string searchStringName, string searchStringAlbum, string songZanr)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artist = await _context.Artist
            .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = artist.FullName;

            IQueryable<Song> songsQuery = _context.ArtistSong.Include(x => x.Song.Album).Where(x => x.ArtistId == id).Select(x => x.Song);

           
            IQueryable<string> zanrQuery = _context.Song.OrderBy(m => m.GenreSong).Select(m => m.GenreSong).Distinct();
            await _context.SaveChangesAsync();

            if (artist == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(searchStringName))
            {
                songsQuery = songsQuery.Where(s => s.Name.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringAlbum))
            {
                songsQuery = songsQuery.Where(s => s.Album.Name.Contains(searchStringAlbum));
            }
            if (!string.IsNullOrEmpty(songZanr))
            {
                songsQuery = songsQuery.Where(s => s.GenreSong == songZanr);
            }
 
            var songSearchVM = new SongSearchVM
            {
                Songs = await songsQuery.ToListAsync(),
                Zanr = new SelectList(await zanrQuery.ToListAsync())
            };

            return View(songSearchVM);
        }


        public async Task<IActionResult> SongsByAlbum(int? id, string searchStringName, string searchStringAlbum, string songZanr)
        {
            if (id == null)
            {
                return NotFound();
            }
            var album = await _context.Album
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = album.Name;

            IQueryable<Song> songsQuery = _context.Song.Where(x => x.AlbumId == id);
            IQueryable<string> zanrQuery = _context.Song.OrderBy(m => m.GenreSong).Select(m => m.GenreSong).Distinct();

            if (!string.IsNullOrEmpty(searchStringName))
            {
                songsQuery = songsQuery.Where(s => s.Name.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringAlbum))
            {
                songsQuery = songsQuery.Where(s => s.Album.Name.Contains(searchStringAlbum));
            }
            if (!string.IsNullOrEmpty(songZanr))
            {
                songsQuery = songsQuery.Where(s => s.GenreSong == songZanr);
            }
            songsQuery = songsQuery.Include(s => s.Album);

            var songSearchVM = new SongSearchVM
            {
                Songs = await songsQuery.ToListAsync(),
                Zanr = new SelectList(await zanrQuery.ToListAsync())
            };

            return View(songSearchVM);
        }
        private bool SongExists(int id)
        {
          return (_context.Song?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPicture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = _context.Song.Include(s =>s.Album).ThenInclude(s =>s.Songs).Where(s => s.Id == id).First();
            if (song == null)
            {
                return NotFound();
            }

            SongPictureVM viewmodel = new SongPictureVM
            {
                Song = song,
                SongPictureName = song.profilePicture
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPicture(int id, SongPictureVM viewmodel)
        {
            viewmodel.Song= _context.Song.Include(s=>s.Album).Where(s => s.Id == id).FirstOrDefault();
            if (id != viewmodel.Song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewmodel.SongPictureFile != null)
                    {
                        string uniqueFileName = UploadedFile(viewmodel);
                        viewmodel.Song.profilePicture = uniqueFileName;
                    }
                    else
                    {
                        viewmodel.Song.profilePicture = viewmodel.SongPictureName;
                    }

                    _context.Update(viewmodel.Song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(viewmodel.Song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = viewmodel.Song.Id });
            }
            return View(viewmodel);
        }

        private string UploadedFile(SongPictureVM viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.SongPictureFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profilePictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.SongPictureFile.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewmodel.SongPictureFile.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }


    }
}
