using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class IncomesController : Controller
    {
        private readonly WebApplication1Context _context;

        public IncomesController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Incomes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Incomes.ToListAsync());
        }

        // GET: Incomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomes = await _context.Incomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incomes == null)
            {
                return NotFound();
            }

            return View(incomes);
        }

        // GET: Incomes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Description,Category,Date")] Incomes incomes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incomes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incomes);
        }

        // GET: Incomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomes = await _context.Incomes.FindAsync(id);
            if (incomes == null)
            {
                return NotFound();
            }
            return View(incomes);
        }

        // POST: Incomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Description,Category,Date")] Incomes incomes)
        {
            if (id != incomes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incomes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomesExists(incomes.Id))
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
            return View(incomes);
        }

        // GET: Incomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomes = await _context.Incomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incomes == null)
            {
                return NotFound();
            }

            return View(incomes);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incomes = await _context.Incomes.FindAsync(id);
            _context.Incomes.Remove(incomes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomesExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}
