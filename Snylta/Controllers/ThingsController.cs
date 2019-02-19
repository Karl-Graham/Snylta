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
using Microsoft.AspNetCore.Identity;


namespace Snylta
{
    public class ThingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ThingsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Things
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Thing.Where(t => t.Owner.Id != user.Id);
            
            ViewData["Title"] = "SnyltIndex";
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> MyThings()
        {
            User user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Thing.Where(t => t.Owner.Id == user.Id);
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

        //ANVÄNDS BARA I TESTNINGSSYFTE, KAN TAS BORT
        public async Task<IActionResult> Snyltningar(string id) 
        {
            User user = await _userManager.GetUserAsync(User);
            return View(user.Snyltningar.Where(x => x.Active).Select(x=> x.Thing).ToList());
        }

        public async Task<IActionResult> Snylta(string id)
        {
            User user = await _userManager.GetUserAsync(User);
            Thing thing = _context.Thing.FirstOrDefault(x => x.Id == id);

            //Kontrollerar att prylen är tillgänglig och att den får lånas av användaren
                if (thing == null)
                    return BadRequest($"Hittade ingen pryl med id {id}");

                if (thing.Snyltningar.Any(x => x.Active))
                    if (thing.Snyltningar.FirstOrDefault(x => x.Active).Snyltare == user)
                        return BadRequest($"Du snyltar redan prylen {thing.Name}");
                    else
                        return BadRequest($"Prylen {thing.Name} är redan snyltad");

                if (thing.Owner == user)
                    return BadRequest($"Du kan inte låna din egen pryl!");


            _context.Add(new Snyltning(user.Id, thing.Id));
            _context.SaveChanges();

            return Ok($"Du {user.UserName} snyltar nu {thing.Name}!");
        }

        public async Task<IActionResult> Return(string id)
        {
            Thing thing = _context.Thing.FirstOrDefault(x => x.Id == id);

           //Kollar att prylen finns och snyltas av dig
                if (thing == null)
                    return BadRequest($"Hittade ingen snyltad pryl med id {id}");

                Snyltning snyltning = thing.Snyltningar.FirstOrDefault(x => x.Active);

                if (snyltning == null)
                    return BadRequest($"Hittade ingen aktiv snyltning på pryl med id {id}");

                User user = await _userManager.GetUserAsync(User);

                if (user != snyltning.Snyltare)
                    return BadRequest($"Du {user.UserName} kan inte lämna tillbaks en pryl som snyltas av någon annan! {thing.Name} snyltas av {snyltning.Snyltare.UserName}");

            snyltning.Active = false;
            _context.SaveChanges();

            return Ok($"Snyltningena av {thing.Name} har avslutats");
        }

        private bool ThingExists(string id)
        {
            return _context.Thing.Any(e => e.Id == id);
        }
    }
}
