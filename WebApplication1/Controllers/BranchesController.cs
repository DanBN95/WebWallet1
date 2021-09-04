using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BranchesController : Controller
    {
        private readonly WebApplication1Context _context;

        public BranchesController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            var accounts = from a in _context.Account
                           where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                           select a;
            ViewData["branches"] = new List<Branch>(_context.Branch.Where(x=>x.Accounts.Contains(accounts.First())));
            return View(await _context.Branch.ToListAsync());
        }

        // GET: Branches/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }
        [Authorize(Roles = "Admin")]
        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,city,lat,lng")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,city,lat,lng")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            return View(branch);
        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConnectUser(int? id)
        {
            var account = from a in _context.Account
                           where a.UserId.ToString() == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value
                           select a;

            if (account.Count() > 0)
            {
                account.First().BranchList = new List<Branch>();
               account.First().BranchList.Add(_context.Branch.FirstOrDefault(x => x.Id == id));
                await _context.SaveChangesAsync();
            }
             return RedirectToAction("Index", "Branches");


        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveUser(int? id)
        //{
        //    var acount_id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
           
        //    var accounts = from a in _context.Account
        //                   where a.UserId.ToString() == acount_id
        //                   select a;
        //    var account_id1 = Int32.Parse(acount_id);
        //    var branch = await _context.Branch.FindAsync(id);

        //    if (accounts.Count() > 0)
        //    {

        //        Branch nwbranch = new Branch();
        //        nwbranch.Accounts = new List<Account>();
        //        nwbranch.Accounts = branch.Accounts;
        //        //nwbranch.Accounts.Remove(accounts.First());
        //        nwbranch.name =branch.name;
        //        nwbranch.city =branch.city;
        //        nwbranch.lat =branch.lat;
        //        nwbranch.lng =branch.lng;
        //        int temp_id =branch.Id;
        //       _context.Branch.Remove(branch);
        //        nwbranch.Id = temp_id;
        //        _context.Add(nwbranch);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction("Index", "Branches");


        //}
    }
}
