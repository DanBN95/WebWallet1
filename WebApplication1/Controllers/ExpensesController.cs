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
    public class ExpensesController : Controller
    {
        private readonly WebApplication1Context _context;

        public ExpensesController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            try
            {
                var expenses = _context.Expenses.Where(i => i.AccountId == account.First().Id).ToList();
                return View(expenses);
            }

            catch { return RedirectToAction("PageNotFound", "Home"); }
        }


        [HttpGet]
        public async Task<IActionResult> Index(string AccountSearch, string sortby, string check)
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
                var expenses = _context.Expenses.Where(i => i.AccountId == account.First().Id).ToList();

                switch (sortby)
                {
                    case "Amount":
                        expenses = string.IsNullOrEmpty(ViewData["SortingByAmount"].ToString()) ? expenses.OrderBy(i => i.Amount).ToList() : expenses.OrderByDescending(i => i.Amount).ToList();
                        break;
                    case "Description":
                        expenses = string.IsNullOrEmpty(ViewData["SortingByDescription"].ToString()) ? expenses.OrderBy(i => i.Description).ToList() : expenses.OrderByDescending(i => i.Description).ToList();
                        break;
                    case "Category":
                        expenses = string.IsNullOrEmpty(ViewData["SortingByCategory"].ToString()) ? expenses.OrderBy(i => i.Category).ToList() : expenses.OrderByDescending(i => i.Category).ToList();
                        break;
                    case "Date":
                        expenses = string.IsNullOrEmpty(ViewData["SortingByDate"].ToString()) ? expenses.OrderBy(i => i.Date).ToList() : expenses.OrderByDescending(i => i.Date).ToList();
                        break;
                    default:
                        expenses = expenses.OrderByDescending(i => i.Date).ToList();
                        break;
                }

                if (!String.IsNullOrEmpty(AccountSearch))
                {
                    expenses = expenses.Where(i => i.Description.Contains(AccountSearch)).ToList();
                }
                return View(expenses);
            }

            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        public IActionResult Index(List<Expenses> expenses)
        {
            return View(expenses);
        }

        public IActionResult SortingByAmount(string id)
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

                        var expenses = _context.Expenses.Where(i => i.AccountId == account.First().Id &&
                        i.Amount >= min && i.Amount <= max).ToList();

                        return View("index", expenses);
                    }

                    else
                    {
                        min = 1000000;
                        var expenses = _context.Expenses.Where(i => i.AccountId == account.First().Id &&
                        i.Amount >= min).ToList();

                        return View("index", expenses);
                    }

                }
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }

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
                var expenses = _context.Expenses.Where(i => i.AccountId == account.First().Id &&
                            i.Category.Equals(category)).ToList();
                return View("index", expenses);
            }


            return RedirectToAction("PageNotFound", "Home");

        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses
                .Include(e => e.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Description,Category,Date")] Expenses expenses)
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;

            if (ModelState.IsValid)
            {
                if (account.Count() > 0)
                {
                    string user_id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                    expenses.AccountId = Int32.Parse(user_id);

                    _context.Add(expenses);
                    account.First().ExpensesList = new List<Expenses>();
                    account.First().ExpensesList.Add(expenses);

                    double result = account.First().Balance - expenses.Amount;
                    if (result >= 0)
                    {
                        account.First().Balance = result;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), "Accounts");
                    }
                    else
                    {
                        ViewBag.LowBalanceError = "Exception in balance!";
                    }
                    
                }
                
            }
            else
            {
                ViewData["Error"] = "Unable to comply; Wrong input for expenses";
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", expenses.AccountId);
            return View(expenses);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", expenses.AccountId);
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Description,Category,Date,AccountId")] Expenses expenses)
        {
            if (id != expenses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensesExists(expenses.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", expenses.AccountId);
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses
                .Include(e => e.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
