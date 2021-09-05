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
        private readonly WebApplication1Context _incomes;


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
        public async Task<IActionResult> Index(string AccountSearch, string sortby,string check)
        {
            ViewData["AccountDetails"] = AccountSearch;
            ViewData["SortingByAmount"] = string.IsNullOrEmpty(check) ? "byDesecnding" : "";
            ViewData["SortingByDescription"] = string.IsNullOrEmpty(check) ? "byDesecnding" : "";
            ViewData["SortingByCategory"] = string.IsNullOrEmpty(check) ? "byDesecnding" : "";
            ViewData["SortingByDate"] = string.IsNullOrEmpty(check) ? "byDesecnding" : "";

            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            try
            {
                var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id).ToList();

                switch(sortby)
                {
                    case "Amount":
                        incomes = string.IsNullOrEmpty(ViewData["SortingByAmount"].ToString()) ? incomes.OrderBy(i => i.Amount).ToList() : incomes.OrderByDescending(i => i.Amount).ToList();
                        break;
                    case "Description":
                        incomes = string.IsNullOrEmpty(ViewData["SortingByDescription"].ToString()) ? incomes.OrderBy(i => i.Description).ToList() : incomes.OrderByDescending(i => i.Description).ToList();
                        break;
                    case "Category":
                        incomes = string.IsNullOrEmpty(ViewData["SortingByCategory"].ToString()) ? incomes.OrderBy(i => i.Category).ToList() : incomes.OrderByDescending(i => i.Category).ToList();
                        break;
                    case "Date":
                        incomes = string.IsNullOrEmpty(ViewData["SortingByDate"].ToString()) ? incomes.OrderBy(i => i.Date).ToList() : incomes.OrderByDescending(i => i.Date).ToList();
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

        public IActionResult Index(List<Incomes> incomes)
        {
            return View(incomes);
        }

        public IActionResult SortingByAmount (string id)
        {
            int max, min;
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            
            try
            {
                if (id != null)
                {
                    string[] range = id.Split("_");
                    if (!range[0].Equals("above"))
                    {
                        min = Int32.Parse(range[0]);
                        max = Int32.Parse(range[1]);

                        var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id &&
                        i.Amount >= min && i.Amount <= max).ToList();

                        return View("index", incomes);
                    }

                    else
                    {
                        min = 1000000;
                        var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id &&
                        i.Amount >= min).ToList();
                      
                        return View("index",incomes);
                    }

                }
            } catch { return RedirectToAction("PageNotFound", "Home"); }

            return RedirectToAction("PageNotFound", "Home");

        }

        public IActionResult SortingByCategory(string id)
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;

            InCategory category;
            if (Enum.TryParse(id, out category))
            {
                var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id &&
                            i.Category.Equals(category)).ToList();
                return View("index", incomes);
            }


            return RedirectToAction("PageNotFound", "Home");

        }
        public IActionResult SortingByDate(string id)
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;

            //string [] monthArray = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            try
            {
                var incomes = _context.Incomes.Where(i => i.AccountId == account.First().Id &&
                i.Date.Month.ToString() == id).ToList();

                return View("index", incomes);
            }
            catch {  return RedirectToAction("PageNotFound", "Home"); }

        }
        //GET: Incomes/Details/5
        public async Task<IActionResult> Details(int? id)
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



        //GET: Incomes/Edit/5
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", incomes.AccountId);
            return View(incomes);
        }

        //POST: Incomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Description,Category,Date,AccountId")] Incomes incomes)
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", incomes.AccountId);
            return View(incomes);
        }

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

        private bool IncomesExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}
