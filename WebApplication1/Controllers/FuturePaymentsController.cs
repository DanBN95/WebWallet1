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
    public class FuturePaymentsController : Controller
    {
        private readonly WebApplication1Context _context;

        public FuturePaymentsController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: FuturePayments
        public async Task<IActionResult> Index()
        {
            var webApplication1Context = _context.FuturePayment.Include(f => f.Account);
            return View(await webApplication1Context.ToListAsync());
        }

        // GET: FuturePayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var futurePayment = await _context.FuturePayment
                .Include(f => f.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (futurePayment == null)
            {
                return NotFound();
            }

            return View(futurePayment);
        }

        // GET: FuturePayments/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: FuturePayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Description,Goalvalue,SinglePaymentvalue,StartDate,EndDate,lastpayment,nextpayment,Frequency,AccountId")] FuturePayment futurePayment)
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            if (ModelState.IsValid)
            {
                if (account.Count() > 0)
                {
                    if (futurePayment.StartDate.CompareTo(futurePayment.EndDate) <= 0)
                    {
                        string user_id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                        futurePayment.AccountId = Int32.Parse(user_id);
                        futurePayment.nextpayment = futurePayment.StartDate;
                        futurePayment.Goalvalue = -1;
                        futurePayment.Type = (Types)0;
                        //time = ((int)(n_f_p.StartDate - n_f_p.EndDate).TotalDays);
                        _context.Add(futurePayment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["Error"] = "cant choose end day beafore start day";
                    }
                }
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", futurePayment.AccountId);
            return View(futurePayment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("Id,Type,Description,Goalvalue,SinglePaymentvalue,StartDate,EndDate,lastpayment,nextpayment,Frequency,AccountId")] FuturePayment futurePayment)
        {
            var account = from a in _context.Account
                          where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                          select a;
            if (ModelState.IsValid)
            {
                if (account.Count() > 0)
                {
                    if (futurePayment.StartDate.CompareTo(futurePayment.EndDate) <= 0)
                    {
                        string user_id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                        futurePayment.AccountId = Int32.Parse(user_id);
                        futurePayment.nextpayment = futurePayment.StartDate;
                        futurePayment.Type = (Types)1;
                        int time = (int)(futurePayment.StartDate - futurePayment.EndDate).TotalDays;
                        switch (futurePayment.Frequency)
                        {
                            case (Frequency)0:
                                break;
                            case (Frequency)1:
                                time = (time / 7) + 1;
                                break;
                            case (Frequency)2:
                                time = (time / 30) + 1;
                                break;
                            case (Frequency)3:
                                time = (time / 365) + 1;
                                break;

                        }
                        futurePayment.SinglePaymentvalue = time != 0 ? futurePayment.Goalvalue / time : futurePayment.Goalvalue;


                        _context.Add(futurePayment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["Error"] = "cant choose end day beafore start day";
                    }
                }
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", futurePayment.AccountId);
            return View(futurePayment);
        }
        // GET: FuturePayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var futurePayment = await _context.FuturePayment.FindAsync(id);
            if (futurePayment == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", futurePayment.AccountId);
            return View(futurePayment);
        }

        // POST: FuturePayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id ,[Bind("Id,Type,Description,Goalvalue,SinglePaymentvalue,StartDate,EndDate,lastpayment,nextpayment,Frequency,AccountId")] FuturePayment futurePayment)
        {
            if (id != futurePayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(futurePayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuturePaymentExists(futurePayment.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Id", futurePayment.AccountId);
            return View(futurePayment);
        }

        // GET: FuturePayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var futurePayment = await _context.FuturePayment
                .Include(f => f.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (futurePayment == null)
            {
                return NotFound();
            }

            return View(futurePayment);
        }

        // POST: FuturePayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var futurePayment = await _context.FuturePayment.FindAsync(id);
            _context.FuturePayment.Remove(futurePayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuturePaymentExists(int id)
        {
            return _context.FuturePayment.Any(e => e.Id == id);
        }
    }
}
