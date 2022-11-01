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
    public class CategoryFpsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryFpsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: CategoryFps
        public async Task<IActionResult> Index()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            return View(await _context.CategoryFps.ToListAsync());
        }

        // GET: CategoryFps/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id == null)
            {
                return NotFound();
            }

            var categoryFp = await _context.CategoryFps
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categoryFp == null)
            {
                return NotFound();
            }

            return View(categoryFp);
        }

        // GET: CategoryFps/Create
        public IActionResult Create()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            return View();
        }

        // POST: CategoryFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,ImageFile,CategoryImage")] CategoryFp categoryFp)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (ModelState.IsValid)
            {
                if(categoryFp.ImageFile != null)
                {

                
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + categoryFp.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await categoryFp.ImageFile.CopyToAsync(fileStream);

                }
                categoryFp.CategoryImage = fileName;
                
                _context.Add(categoryFp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
            }
            return View(categoryFp);
        }

        // GET: CategoryFps/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id == null)
            {
                return NotFound();
            }

            var categoryFp = await _context.CategoryFps.FindAsync(id);
            if (categoryFp == null)
            {
                return NotFound();
            }
            return View(categoryFp);
        }

        // POST: CategoryFps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("CategoryId,CategoryName,ImageFile,CategoryImage")] CategoryFp categoryFp)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id != categoryFp.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(categoryFp.CategoryImage != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + categoryFp.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await categoryFp.ImageFile.CopyToAsync(fileStream);

                        }
                        categoryFp.CategoryImage = fileName;
                    }
                   
                    else
                    {
                        categoryFp.CategoryImage = _context.CategoryFps.Where(x => x.CategoryId == id).AsNoTracking<CategoryFp>().SingleOrDefault().CategoryImage;
                    }
                    _context.Update(categoryFp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryFpExists(categoryFp.CategoryId))
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
            return View(categoryFp);
        }

        // GET: CategoryFps/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (id == null)
            {
                return NotFound();
            }

            var categoryFp = await _context.CategoryFps
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categoryFp == null)
            {
                return NotFound();
            }

            return View(categoryFp);
        }

        // POST: CategoryFps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var categoryFp = await _context.CategoryFps.FindAsync(id);
            _context.CategoryFps.Remove(categoryFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryFpExists(decimal id)
        {
            return _context.CategoryFps.Any(e => e.CategoryId == id);
        }
    }
}
