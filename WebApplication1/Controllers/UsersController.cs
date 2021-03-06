using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
        public class UsersController : Controller
        {
            private readonly WebApplication1Context _context;

            public UsersController(WebApplication1Context context)
            {
                _context = context;
            }

            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            // GET: Users/Register
            public IActionResult Register()
            {
                return View();
            }

            // POST: Users/Register
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Register([Bind("Id,User_first_name,User_last_name,Email,Password")] User user)
            {
                if (ModelState.IsValid)
                {
                    var q = _context.User.FirstOrDefault(u =>  u.Email == user.Email);
                if (q == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    var u = _context.User.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

                    Signin(u);
                    return RedirectToAction("Create", "Accounts");
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; Email adress or username are already exist";
                }

                }
                return View(user);
            }


            public IActionResult Login()
            {
                return View();
            }

            // POST: Users/Login
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login([Bind("Id,Email,Password")] User user)
            {

                var q = from u in _context.User
                        where u.Password == user.Password && u.Email == user.Email
                        select u;

                if (q.Count() > 0) {

                    Signin(q.First());

                    return RedirectToAction(nameof(Index), "Accounts");
                }
                else
                {
                    ViewData["Error"] = "Incorrect Email or Password";
                }

                return View(user);
            }
            private async void Signin(User account)
            {
            var claims = new List<Claim>
            {
                //  Account Id
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                
                //  Account permissions
                new Claim(ClaimTypes.Role, account.Type.ToString()),

                //  Username
                new Claim("username",account.User_first_name),

          

            };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                     authProperties);
            }




            // GET: Users
            //public async Task<IActionResult> Index()
            //{
            //    return View(await _context.User.ToListAsync());
            //}

            //// GET: Users/Details/5
            //public async Task<IActionResult> Details(int? id)
            //{
            //    if (id == null)
            //    {
            //        return NotFound();
            //    }

            //    var user = await _context.User
            //        .FirstOrDefaultAsync(m => m.Id == id);
            //    if (user == null)
            //    {
            //        return NotFound();
            //    }

            //    return View(user);
            //}



            //// GET: Users/Edit/5
            //public async Task<IActionResult> Edit(int? id)
            //{
            //    if (id == null)
            //    {
            //        return NotFound();
            //    }

            //    var user = await _context.User.FindAsync(id);
            //    if (user == null)
            //    {
            //        return NotFound();
            //    }
            //    return View(user);
            //}

            //// POST: Users/Edit/5
            //// To protect from overposting attacks, enable the specific properties you want to bind to.
            //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password,Type,AccountId")] User user)
            //{
            //    if (id != user.Id)
            //    {
            //        return NotFound();
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        try
            //        {
            //            _context.Update(user);
            //            await _context.SaveChangesAsync();
            //        }
            //        catch (DbUpdateConcurrencyException)
            //        {
            //            if (!UserExists(user.Id))
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
            //    return View(user);
            //}

            //// GET: Users/Delete/5
            //public async Task<IActionResult> Delete(int? id)
            //{
            //    if (id == null)
            //    {
            //        return NotFound();
            //    }

            //    var user = await _context.User
            //        .FirstOrDefaultAsync(m => m.Id == id);
            //    if (user == null)
            //    {
            //        return NotFound();
            //    }

            //    return View(user);
            //}

            //// POST: Users/Delete/5
            //[HttpPost, ActionName("Delete")]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> DeleteConfirmed(int id)
            //{
            //    var user = await _context.User.FindAsync(id);
            //    _context.User.Remove(user);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            //private bool UserExists(int id)
            //{
            //    return _context.User.Any(e => e.Id == id);
            //}
        }
}
