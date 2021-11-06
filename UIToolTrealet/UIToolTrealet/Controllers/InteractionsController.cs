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
    public class InteractionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InteractionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Interactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Interaction.Include(i => i.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Interactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaction = await _context.Interaction
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.InteractionId == id);
            if (interaction == null)
            {
                return NotFound();
            }

            return View(interaction);
        }

        // GET: Interactions/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId");
            return View();
        }

        // POST: Interactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InteractionId,Question,AnswerA,AnswerB,AnswerC,AnswerD,TrueAnswer,ItemId")] Interaction interaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", interaction.ItemId);
            return View(interaction);
        }

        // GET: Interactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaction = await _context.Interaction.FindAsync(id);
            if (interaction == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", interaction.ItemId);
            return View(interaction);
        }

        // POST: Interactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InteractionId,Question,AnswerA,AnswerB,AnswerC,AnswerD,TrueAnswer,ItemId")] Interaction interaction)
        {
            if (id != interaction.InteractionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InteractionExists(interaction.InteractionId))
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
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", interaction.ItemId);
            return View(interaction);
        }

        // GET: Interactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaction = await _context.Interaction
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.InteractionId == id);
            if (interaction == null)
            {
                return NotFound();
            }

            return View(interaction);
        }

        // POST: Interactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interaction = await _context.Interaction.FindAsync(id);
            _context.Interaction.Remove(interaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InteractionExists(int id)
        {
            return _context.Interaction.Any(e => e.InteractionId == id);
        }
    }
}
