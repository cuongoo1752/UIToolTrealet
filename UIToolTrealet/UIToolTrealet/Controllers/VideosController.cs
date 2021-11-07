using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UIToolTrealet.Data;
using UIToolTrealet.Models;

namespace UIToolTrealet.Controllers
{
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index(int? id)
        {
            int? itemId = id;
            if (itemId == null)
            {
                return NotFound();
            }

            var videos = await _context.Video.Where(m => m.ItemId.Equals(itemId)).ToListAsync<Video>();
            if (videos == null)
            {
                return NotFound();
            }

            ViewData["ItemId"] = id;

            return View(videos);
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .Include(v => v.Item)
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId");
                Video video = new Video()
                {
                    ItemId = (int)id,
                };
                return View(video);
            }
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,Title,Url,Type,ItemId")] Video video)
        {
            if (ModelState.IsValid)
            {
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = video.ItemId });
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", video.ItemId);
            return View(video);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", video.ItemId);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoId,Title,Url,Type,ItemId")] Video video)
        {
            if (id != video.VideoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.VideoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = video.ItemId });
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", video.ItemId);
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .Include(v => v.Item)
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int itemId)
        {
            var video = await _context.Video.FindAsync(id);
            _context.Video.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = itemId });
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }
    }
}
