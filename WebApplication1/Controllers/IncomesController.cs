using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public IActionResult Index()
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            try
            {
                var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id).ToList();
                return View(incomes);
            }
            
            catch { return RedirectToAction("PageNotFound", "Home"); }
   
        }

        [HttpGet]
        public async Task<IActionResult> Index(string AccountSearch, string sortinginc)
        {
            ViewData["AccountDetails"] = AccountSearch;
            ViewData["SortingBy"] = string.IsNullOrEmpty(sortinginc) ? "" : sortinginc;
 
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            try
            {
                var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id).ToList();

                switch(sortinginc)
                {
                    case "Date":
                        incomes = incomes.OrderBy(i => i.Date).ToList();
                        break;
                    case "Amount":
                        incomes = incomes.OrderBy(i => i.Amount).ToList();
                        break;
                    case "Description":
                        incomes = incomes.OrderBy(i => i.Description).ToList();
                        break;
                    case "Category":
                        incomes = incomes.OrderBy(i => i.Category).ToList();
                        break;
                    default:
                        incomes = incomes.OrderByDescending(i => i.Date).ToList();
                        break;
                }

                if (!String.IsNullOrEmpty(AccountSearch))
                {
                    incomes = incomes.Where(i => i.Description.Contains(AccountSearch)).ToList();
                }
                return View(incomes);
            }

            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Incomes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var incomes = await _context.Incomes
        //        .Include(i => i.Account)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (incomes == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(incomes);
        //}

        // GET: Incomes/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: Incomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Description,Category,Date")] Incomes incomes)
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;

            if (ModelState.IsValid)
            {
                if (account.Count() > 0)
                {
                    string user_id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                    incomes.AccountId = Int32.Parse(user_id);

                    _context.Add(incomes);
                    account.First().IncomesList = new List<Incomes>();

                    account.First().IncomesList.Add(incomes);
                    account.First().Balance += incomes.Amount;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Accounts");
                }
            }
            else
            {
                ViewData["Error"] = "Unable to comply; Wrong input for income";
            }

            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", incomes.AccountId);
            return View(incomes);
        }

        // GET: Incomes/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var incomes = await _context.Incomes.FindAsync(id);
        //    if (incomes == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", incomes.AccountId);
        //    return View(incomes);
        //}

        // POST: Incomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Description,Category,Date,AccountId")] Incomes incomes)
        //{
        //    if (id != incomes.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(incomes);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!IncomesExists(incomes.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", incomes.AccountId);
        //    return View(incomes);
        //}

        //GET: Incomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomes = await _context.Incomes
                .Include(i => i.Account)
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
            var account = from a in _context.Account
                          where a.UserId == incomes.AccountId
                          select a;
            account.First().Balance -= incomes.Amount;
            _context.Incomes.Remove(incomes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool IncomesExists(int id)
        //{
        //    return _context.Incomes.Any(e => e.Id == id);
        //}
    }
}
