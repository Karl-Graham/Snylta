using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snylta.Data;
using Snylta.Models;
using System.Security.Claims;

namespace Snylta
{
    public class ThingsController : Controller
    {
        private readonly ApplicationDbContext _context;
       

        public ThingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Things
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Thing;
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ShowMyThings()
        {
            var applicationDbContext = _context.Thing.Include(t => t.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Things/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .Include(t => t.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }

        // GET: Things/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Things/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Thing thing)
        {
            if (ModelState.IsValid)
            {
                thing.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                _context.Add(thing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);
            return View(thing);
        }

        // GET: Things/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing.FindAsync(id);
            if (thing == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);
            return View(thing);
        }

        // POST: Things/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,UserId")] Thing thing)
        {
            if (id != thing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThingExists(thing.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);
            return View(thing);
        }

        // GET: Things/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .Include(t => t.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }

        // POST: Things/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thing = await _context.Thing.FindAsync(id);
            _context.Thing.Remove(thing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThingExists(string id)
        {
            return _context.Thing.Any(e => e.Id == id);
        }
    }
}
