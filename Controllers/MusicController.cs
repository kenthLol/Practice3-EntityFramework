using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using thirdPractice.Models;

namespace thirdPractice.Controllers
{
    public class MusicController : Controller
    {
        private readonly AppDbContext _context;

        public MusicController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Music
        public async Task<IActionResult> Index()
        {
              return _context.Musics != null ?
                          View(await _context.Musics.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Musics'  is null.");
        }

        // GET: Music
        public async Task<IActionResult> Search(string search)
        {
            IQueryable<Music> musicQuery = _context.Musics;

            if (!string.IsNullOrEmpty(search))
            {
                musicQuery = musicQuery.Where(c =>
                    (c.AlbumName != null && c.AlbumName.Contains(search)) ||
                    (c.Artist != null && c.Artist.Contains(search)) ||
                    (c.Genre != null && c.Genre.Contains(search))
                    );
            }

            var musics = await musicQuery.ToListAsync();

            return View("Index", musics); // Redirige a la vista "Index" con los resultados de la búsqueda
        }

        // GET: Music/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        // GET: Music/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Music/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumName,Artist,Genre,ReleaseDate,Description,UrlCover")] Music music)
        {
            if (ModelState.IsValid)
            {
                _context.Add(music);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(music);
        }

        // GET: Music/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }
            return View(music);
        }

        // POST: Music/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumName,Artist,Genre,ReleaseDate,Description,UrlCover")] Music music)
        {
            if (id != music.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(music);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicExists(music.Id))
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
            return View(music);
        }

        // GET: Music/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        // POST: Music/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Musics == null)
            {
                return Problem("Entity set 'AppDbContext.Musics'  is null.");
            }
            var music = await _context.Musics.FindAsync(id);
            if (music != null)
            {
                _context.Musics.Remove(music);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicExists(int id)
        {
          return (_context.Musics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
