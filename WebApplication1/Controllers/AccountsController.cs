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
    public class AccountsController : Controller
    {
     
        //
        private readonly WebApplication1Context _context;

        public AccountsController(WebApplication1Context context)
        {
            _context = context;
        }
       

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            

            var accounts = from a in _context.Account
                           where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                           select a;

            if (accounts.Count() > 0)
            {
                ViewBag.Username = accounts.First().Name;
                ViewBag.Balance = accounts.First().Balance;
                ViewBag.SavingBalance = accounts.First().SavingBalance;
            }
            else
            {
                Console.WriteLine("Problem with Cookie!");
            }
            Updatesaving();
            return View();

        }



       


        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["branchs"] = new SelectList(_context.Branch, nameof(Branch.Id), nameof(Branch.name));
           
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Balance,SavingBalance")] Account account,int [] Branches)
        {

            if (ModelState.IsValid)
            {
                string user_id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                account.UserId = Int32.Parse(user_id);
                account.Name = ((ClaimsIdentity)User.Identity).FindFirst("username").Value;
                account.ExpensesList = new List<Expenses>();
                account.IncomesList = new List<Incomes>();
                account.BranchList = new List<Branch>();
                account.BranchList.AddRange(_context.Branch.Where(x => Branches.Contains(x.Id)));
                _context.Add(account);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }


        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Updatesaving()
        {
            //System.Diagnostics.Debug.WriteLine("This will be displayed in output window");
            DateTime today = DateTime.Today;
            var today_payments = _context.FuturePayment.Where(i => (i.AccountId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value && i.nextpayment.CompareTo(today) <= 0)).ToArray();

            for (int i = 0; i < today_payments.Count(); i++)
            {
                var account = from a in _context.Account
                              where a.UserId == today_payments[i].AccountId
                              select a;
                if (account.Count() > 0)
                {
                    DateTime end_date = today_payments[i].EndDate;
                    DateTime next_date = today;
                    while (today_payments[i].nextpayment.CompareTo(today) <= 0)
                    {
                        double result = account.First().Balance - today_payments[i].SinglePaymentvalue;
                        if (result >= 0)
                        {
                            Expenses exp = new Expenses();
                            exp.AccountId = today_payments[i].AccountId;
                            exp.Amount = today_payments[i].SinglePaymentvalue;
                            exp.Description = today_payments[i].Description;
                            exp.Category = ExCategory.Other;
                            exp.Date = today;
                            _context.Add(exp);
                            account.First().ExpensesList = new List<Expenses>();
                            account.First().ExpensesList.Add(exp);
                            account.First().Balance = result;
                            account.First().SavingBalance += today_payments[i].SinglePaymentvalue;
                            today_payments[i].lastpayment = today;
                            var fre = today_payments[i].Frequency;
                            switch (fre)
                            {
                                case (Frequency)0:
                                    next_date = next_date.AddDays(1);
                                    break;
                                case (Frequency)1:
                                    next_date = next_date.AddDays(7);
                                    break;
                                case (Frequency)2:
                                    next_date = next_date.AddMonths(1);
                                    break;
                                case (Frequency)3:
                                    next_date = next_date.AddMonths(12);
                                    break;
                            }

                            if (next_date.CompareTo(end_date) <= 0)
                            {
                                today_payments[i].nextpayment = next_date;

                                _context.Update(today_payments[i]);
                                Console.WriteLine("amirrrr");

                            }
                            else
                            {
                                _context.FuturePayment.Remove(today_payments[i]);
                            }



                        }
                        else
                        {
                            ViewBag.LowBalanceError = "Exception in balance!";
                            continue;
                        }
                        await _context.SaveChangesAsync();
                    }
                }

            }



            return RedirectToAction(nameof(Index));

        }

        //// GET: Accounts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Account.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(account);
        //}

        //// POST: Accounts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Balance,SavingBalance")] Account account)
        //{
        //    if (id != account.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(account);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AccountExists(account.Id))
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
        //    return View(account);
        //}

        //// GET: Accounts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Account
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        //// POST: Accounts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var account = await _context.Account.FindAsync(id);
        //    _context.Account.Remove(account);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}



        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ConnectBranch(int? id)
        //{
        //    var account = from a in _context.Account
        //                   where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
        //                   select a;

        //    if (account.Count() > 0)
        //    {
        //        account.First().BranchList = new List<Branch>();
        //        account.First().BranchList.Add(_context.Branch.FirstOrDefault(x => x.Id==id));
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("ConnectUser", "Branch", new { id = id });
        //    }
        //    else
        //    {
        //        Console.WriteLine("Problem with connect to branch!");
        //    }
        //    return RedirectToAction("Index", "Branch");



        //}


    }
}
