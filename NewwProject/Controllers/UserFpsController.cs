using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewwProject.Models;

namespace NewwProject.Controllers
{
    public class UserFpsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserFpsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: UserFps
        public async Task<IActionResult> Index()

        {
            var id = HttpContext.Session.GetInt32("UserID");
            var modelContext = _context.UserFps.Include(u => u.RoleFkNavigation).Where(x=>x.UserId == id);
            return View(await modelContext.ToListAsync());
        }

        // GET: UserFps/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFp = await _context.UserFps
                .Include(u => u.RoleFkNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userFp == null)
            {
                return NotFound();
            }

            return View(userFp);
        }

        // GET: UserFps/Create
        public IActionResult Create()
        {
            ViewData["RoleFk"] = new SelectList(_context.RoleFps, "RoleId", "RoleId");
            return View();
        }

        // POST: UserFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Email,ImagePath,Gender,CraftName,RoleFk")] UserFp userFp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userFp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleFk"] = new SelectList(_context.RoleFps, "RoleId", "RoleId", userFp.RoleFk);
            return View(userFp);
        }

        // GET: UserFps/Edit/5
        public async Task<IActionResult> Edit()
        {

            var id = HttpContext.Session.GetInt32("UserID");
            if (id == null)
            {
                return NotFound();
            }

            var userFp = await _context.UserFps.FindAsync((decimal)id);
            var Login = await _context.UserloginFps.FindAsync((decimal)id);
            
            if (userFp == null && Login ==null)
            {
                return NotFound();
            }
            ViewData["RoleFk"] = new SelectList(_context.RoleFps, "RoleId", "RoleId", userFp.RoleFk);
            return View(userFp);
        }

        // POST: UserFps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id,[Bind("UserId,FirstName,LastName,Email,ImagePath,ImageFile,Gender,CraftName,RoleFk")] UserFp userFp , string Username, string Password)
        {

            if (id != userFp.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userFp.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + userFp.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userFp.ImageFile.CopyToAsync(fileStream);
                        }
                        userFp.ImagePath = fileName;
                    }
                    else
                    {
                        userFp.ImagePath = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().ImagePath;
                    } //to keep the image the user already has if he doesnt wann edit it when editing his profile
                      //var uniqe = _context.UserFps.Where(x => x.Email == userFp.Email).FirstOrDefault();
                      //if (uniqe == null)
                    var logininfo = _context.UserloginFps.Where(x => x.UserFk == userFp.UserId).SingleOrDefault();


                    if (Username == null || Password == null)
                    {
                        userFp.RoleFk = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().RoleFk;

                        _context.Update(userFp);
                        await _context.SaveChangesAsync();
                        logininfo.Username = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().Username;
                        logininfo.UserPassword = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().UserPassword;
                         _context.Update(logininfo);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "UserFps");
                    }
                    else
                    {
                        userFp.RoleFk = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().RoleFk;

                        _context.Update(userFp);
                        await _context.SaveChangesAsync();
                        logininfo.Username = Username;
                        logininfo.UserPassword = Password;
                        _context.Update(logininfo);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "UserFps");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserFpExists(userFp.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            ViewData["RoleFk"] = new SelectList(_context.RoleFps, "RoleId", "RoleId", userFp.RoleFk);
            return View(userFp);
        }

        // GET: UserFps/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFp = await _context.UserFps
                .Include(u => u.RoleFkNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userFp == null)
            {
                return NotFound();
            }

            return View(userFp);
        }

        // POST: UserFps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var userFp = await _context.UserFps.FindAsync(id);
            _context.UserFps.Remove(userFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserFpExists(decimal id)
        {
            return _context.UserFps.Any(e => e.UserId == id);
        }
    }
}
