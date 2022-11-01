using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewwProject.Models;

namespace NewwProject.Controllers
{
    public class HomeFpsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeFpsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: HomeFps
        public async Task<IActionResult> Index()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            return View(await _context.HomeFps.ToListAsync());
        }

        // GET: HomeFps/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id == null)
            {
                return NotFound();
            }

            var homeFp = await _context.HomeFps
                .FirstOrDefaultAsync(m => m.HomeId == id);
            if (homeFp == null)
            {
                return NotFound();
            }

            return View(homeFp);
        }

        // GET: HomeFps/Create
        public IActionResult Create()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            return View();
        }

        // POST: HomeFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeId,Homeparagraph1,Homeparagraph2,ImageFile,Homeparagraph3,Location1,Location2,Email,Photo1,Photo2,Photo3,Photo4,Photo5,Photo6,Photo7,Footerparagraph2,Footerparagraph3")] HomeFp homeFp)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homeFp.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homeFp.ImageFile.CopyToAsync(fileStream);

                }
                homeFp.Photo1 = fileName;
                _context.Add(homeFp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeFp);
        }

        // GET: HomeFps/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id == null)
            {
                return NotFound();
            }

            var homeFp = await _context.HomeFps.FindAsync(id);
            if (homeFp == null)
            {
                return NotFound();
            }
            return View(homeFp);
        }

        // POST: HomeFps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("HomeId,Homeparagraph1,Homeparagraph2,Homeparagraph3,Location1,Location2,Email,Photo1,Photo2,Photo3,Photo4,Photo5,Photo6,Photo7,Footerparagraph2,Footerparagraph3")] HomeFp homeFp)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id != homeFp.HomeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(homeFp.Photo1 != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + homeFp.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await homeFp.ImageFile.CopyToAsync(fileStream);

                        }
                        homeFp.Photo1 = fileName;
                    }
                    
                    
                  else
                    {
                        homeFp.Photo1 = _context.HomeFps.Where(x => x.HomeId == id).AsNoTracking<HomeFp>().SingleOrDefault().Photo1;
                    }
                    _context.Update(homeFp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeFpExists(homeFp.HomeId))
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
            return View(homeFp);
        }

        // GET: HomeFps/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id == null)
            {
                return NotFound();
            }

            var homeFp = await _context.HomeFps
                .FirstOrDefaultAsync(m => m.HomeId == id);
            if (homeFp == null)
            {
                return NotFound();
            }

            return View(homeFp);
        }

        // POST: HomeFps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var homeFp = await _context.HomeFps.FindAsync(id);
            _context.HomeFps.Remove(homeFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeFpExists(decimal id)
        {
            return _context.HomeFps.Any(e => e.HomeId == id);
        }
    }
}
