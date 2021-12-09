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
    public class InfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Infoes
        public async Task<IActionResult> Index(int? id)
        {
            int? itemId = id;

            if (itemId == null)
            {
                return NotFound();
            }

            var infos = await _context.Info.Where(m => m.ItemId.Equals(itemId)).ToListAsync<Info>();
            if (infos == null)
            {
                return NotFound();
            }

            ViewData["ItemId"] = id;

            return View(infos);
        }

        // GET: Infoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var info = await _context.Info
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.InfoId == id);
            if (info == null)
            {
                return NotFound();
            }

            return View(info);
        }

        // GET: Infoes/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId");
                Info info = new Info()
                {
                    ItemId = (int)id,
                };
                return View(info);
            }
        }

        // POST: Infoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InfoId,Key,Value,ItemId")] Info info)
        {
            if (ModelState.IsValid)
            {
                _context.Add(info);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = info.ItemId });
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", info.ItemId);
            return View(info);
        }

        // GET: Infoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var info = await _context.Info.FindAsync(id);
            if (info == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", info.ItemId);
            return View(info);
        }

        // POST: Infoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InfoId,Key,Value,ItemId")] Info info)
        {
            if (id != info.InfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(info);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfoExists(info.InfoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = info.ItemId });
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", info.ItemId);
            return View(info);
        }

        // GET: Infoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var info = await _context.Info
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.InfoId == id);
            if (info == null)
            {
                return NotFound();
            }

            return View(info);
        }

        // POST: Infoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int itemId)
        {
            var info = await _context.Info.FindAsync(id);
            _context.Info.Remove(info);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = itemId });
        }

        private bool InfoExists(int id)
        {
            return _context.Info.Any(e => e.InfoId == id);
        }
    }
}
