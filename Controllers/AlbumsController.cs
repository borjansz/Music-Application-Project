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
    public class AlbumsController : Controller
    {
        private readonly RateYourMusicAppContext _context;

        public AlbumsController(RateYourMusicAppContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index(string searchStringName, string searchStringArtist, string songZanr)
        {
            IQueryable<Album> albumsQuery = _context.Album.Include(a => a.Artist).AsQueryable();
            IQueryable<string> zanrQuery = _context.Song.OrderBy(m => m.GenreSong).Select(m => m.GenreSong).Distinct();
            
            if (!string.IsNullOrEmpty(searchStringName))
            {
               albumsQuery = albumsQuery.Where(s => s.Name.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringArtist))
            {
                albumsQuery = albumsQuery.Where(s => s.Artist.FullName.Contains(searchStringArtist));
            }
            if (!string.IsNullOrEmpty(songZanr))
            {
                albumsQuery = albumsQuery.Where(s => s.GenreAlbum == songZanr);
            }

            var albumSearchVM = new AlbumSearchVM
            {
                Albums = await albumsQuery.ToListAsync(),
                Zanr = new SelectList(await zanrQuery.ToListAsync()),
            };

            //var rateYourMusicAppContext = _context.Album.Include(a => a.Artist);
            return View(albumSearchVM);
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            AlbumPictureVM viewmodel = new AlbumPictureVM
            {
                Album = album,
                AlbumPictureName = album.profilePicture
            };

            return View(viewmodel);
        }

        // GET: Albums/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ArtistId,YearAlbum,GenreAlbum,ProductionCompany")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id", album.ArtistId);
            return View(album);
        }

        // GET: Albums/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            var album = await _context.Album.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id", album.ArtistId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ArtistId,YearAlbum,GenreAlbum,ProductionCompany")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id", album.ArtistId);
            return View(album);
        }

        // GET: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Album == null)
            {
                return Problem("Entity set 'RateYourMusicAppContext.Album'  is null.");
            }
            var album = await _context.Album.FindAsync(id);
            if (album != null)
            {
                _context.Album.Remove(album);
            }
            
             await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
          return (_context.Album?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPicture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _context.Album.Include(s => s.Artist).Where(s => s.Id == id).First();
            if (album == null)
            {
                return NotFound();
            }

            AlbumPictureVM viewmodel = new AlbumPictureVM
            {
                Album = album,
                AlbumPictureName = album.profilePicture
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPicture(int id, AlbumPictureVM viewmodel)
            
        {
            viewmodel.Album = _context.Album.Include(s =>s.Artist).Include(s=>s.Songs).Where(s => s.Id == id).FirstOrDefault();
            if (id != viewmodel.Album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewmodel.AlbumPictureFile != null)
                    {
                        string uniqueFileName = UploadedFile(viewmodel);
                        viewmodel.Album.profilePicture = uniqueFileName;
                    }
                    else
                    {
                        viewmodel.Album.profilePicture = viewmodel.AlbumPictureName;
                    }

                    _context.Update(viewmodel.Album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(viewmodel.Album.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = viewmodel.Album.Id });
            }
            return View(viewmodel);
        }

        private string UploadedFile(AlbumPictureVM viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.AlbumPictureFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profilePictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.AlbumPictureFile.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewmodel.AlbumPictureFile.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }

    }
}
