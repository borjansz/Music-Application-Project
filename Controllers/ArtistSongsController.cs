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

namespace RateYourMusicApp.Controllers
{
    public class ArtistSongsController : Controller
    {
        private readonly RateYourMusicAppContext _context;

        public ArtistSongsController(RateYourMusicAppContext context)
        {
            _context = context;
        }

        // GET: ArtistSongs
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var rateYourMusicAppContext = _context.ArtistSong.Include(a => a.Artist).Include(a => a.Song);
            return View(await rateYourMusicAppContext.ToListAsync());
        }

        // GET: ArtistSongs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ArtistSong == null)
            {
                return NotFound();
            }

            var artistSong = await _context.ArtistSong
                .Include(a => a.Artist)
                .Include(a => a.Song)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistSong == null)
            {
                return NotFound();
            }

            return View(artistSong);
        }

        // GET: ArtistSongs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id");
            ViewData["SongId"] = new SelectList(_context.Song, "Id", "Id");
            return View();
        }

        // POST: ArtistSongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistId,SongId")] ArtistSong artistSong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistSong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id", artistSong.ArtistId);
            ViewData["SongId"] = new SelectList(_context.Song, "Id", "Id", artistSong.SongId);
            return View(artistSong);
        }

        // GET: ArtistSongs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ArtistSong == null)
            {
                return NotFound();
            }

            var artistSong = await _context.ArtistSong.FindAsync(id);
            if (artistSong == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id", artistSong.ArtistId);
            ViewData["SongId"] = new SelectList(_context.Song, "Id", "Id", artistSong.SongId);
            return View(artistSong);
        }

        // POST: ArtistSongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistId,SongId")] ArtistSong artistSong)
        {
            if (id != artistSong.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistSong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistSongExists(artistSong.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Id", artistSong.ArtistId);
            ViewData["SongId"] = new SelectList(_context.Song, "Id", "Id", artistSong.SongId);
            return View(artistSong);
        }

        // GET: ArtistSongs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ArtistSong == null)
            {
                return NotFound();
            }

            var artistSong = await _context.ArtistSong
                .Include(a => a.Artist)
                .Include(a => a.Song)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistSong == null)
            {
                return NotFound();
            }

            return View(artistSong);
        }

        // POST: ArtistSongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ArtistSong == null)
            {
                return Problem("Entity set 'RateYourMusicAppContext.ArtistSong'  is null.");
            }
            var artistSong = await _context.ArtistSong.FindAsync(id);
            if (artistSong != null)
            {
                _context.ArtistSong.Remove(artistSong);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistSongExists(int id)
        {
          return (_context.ArtistSong?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
