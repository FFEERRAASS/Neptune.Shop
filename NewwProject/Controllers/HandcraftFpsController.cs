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
using Microsoft.Extensions.Hosting;
using NewwProject.Models;

namespace NewwProject.Controllers
{
    public class HandcraftFpsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HandcraftFpsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: HandcraftFps


        public IActionResult Index()
        {
            var modelContext = _context.HandcraftFps.Include(h => h.Category).Include(h => h.UserFkNavigation);
            return View(modelContext.ToList());
        }
        public IActionResult Index1()
        {
            var modelContext = _context.HandcraftFps.Include(h => h.Category).Include(h => h.UserFkNavigation);
            return View(modelContext.ToList());
        }
        public IActionResult search1()
        {
            var modelcontext = _context.HandcraftFps.ToList();
            return View(modelcontext);
        }
        [HttpPost]
        public async Task<IActionResult> search1(string name)
        {
            var modelcontext = _context.HandcraftFps.Include(x => x.Category);
            if (name != null)
            {

                var result = await modelcontext.Where(x => x.Handcraft.ToLower().Contains(name.ToLower())).ToListAsync();

                return View(result);
            }
            else
            {
                return View(modelcontext);
            }
        }

        public async Task<IActionResult> IndexCategory(decimal? id)
        {
            
            var modelContext = _context.HandcraftFps.Where(x => x.CategoryId == id).Include(h => h.Category).Include(h => h.UserFkNavigation);
            return View(await modelContext.ToListAsync());
        }
        public async Task<IActionResult> IndexCategory1(decimal? id)
        {

            var modelContext = _context.HandcraftFps.Where(x => x.CategoryId == id).Include(h => h.Category).Include(h => h.UserFkNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: HandcraftFps/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handcraftFp = await _context.HandcraftFps
                .Include(h => h.Category)
                .Include(h => h.UserFkNavigation)
                .FirstOrDefaultAsync(m => m.HandId == id);
            if (handcraftFp == null)
            {
                return NotFound();
            }

            return View(handcraftFp);
        }

        // GET: HandcraftFps/Create
        public IActionResult Create()
        {
            
            ViewData["CategoryId"] = new SelectList(_context.CategoryFps, "CategoryId", "CategoryId");
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email");
            return View();
        }

        // POST: HandcraftFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HandId,Handcraft,Price,Sale,Descriptions,ImagePath,ImageFile,UserFk,CategoryId")] HandcraftFp handcraftFp)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + handcraftFp.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await handcraftFp.ImageFile.CopyToAsync(fileStream);

                }
                handcraftFp.ImagePath = fileName;
                _context.Add(handcraftFp);
                await _context.SaveChangesAsync();
            
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryFps, "CategoryId", "CategoryId", handcraftFp.CategoryId);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", handcraftFp.UserFk);
            return View(handcraftFp);
        }

        // GET: HandcraftFps/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handcraftFp = await _context.HandcraftFps.FindAsync(id);
            if (handcraftFp == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryFps, "CategoryId", "CategoryId", handcraftFp.CategoryId);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", handcraftFp.UserFk);
            return View(handcraftFp);
        }

        // POST: HandcraftFps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("HandId,Handcraft,Price,Sale,Descriptions,ImagePath,ImageFile,UserFk,CategoryId")] HandcraftFp handcraftFp)
        {
            if (id != handcraftFp.HandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (handcraftFp.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +handcraftFp.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path,FileMode.Create))
                        {
                            await handcraftFp.ImageFile.CopyToAsync(fileStream);
                        }
                        handcraftFp.ImagePath = fileName;
                    }
                    _context.Update(handcraftFp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandcraftFpExists(handcraftFp.HandId))
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
            ViewData["CategoryId"] = new SelectList(_context.CategoryFps, "CategoryId", "CategoryId", handcraftFp.CategoryId);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", handcraftFp.UserFk);
            return View(handcraftFp);
        }

        // GET: HandcraftFps/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handcraftFp = await _context.HandcraftFps
                .Include(h => h.Category)
                .Include(h => h.UserFkNavigation)
                .FirstOrDefaultAsync(m => m.HandId == id);
            if (handcraftFp == null)
            {
                return NotFound();
            }

            return View(handcraftFp);
        }

        // POST: HandcraftFps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var handcraftFp = await _context.HandcraftFps.FindAsync(id);
            _context.HandcraftFps.Remove(handcraftFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult search()
        {
            var modelcontext = _context.HandcraftFps.ToList();
            return View(modelcontext);
        }
        [HttpPost]
        public async Task<IActionResult> search(string name)
        {
            var modelcontext = _context.HandcraftFps.Include(x=>x.Category);
            if (name != null)
            {

                var result = await modelcontext.Where(x => x.Handcraft.ToLower().Contains(name.ToLower())).ToListAsync();
                
                return View(result);
            }
            else
            {
                return View(modelcontext);
            }
        }
        public async Task<JsonResult> AddCart(int HandId )
        {
            OrdersFp order = new OrdersFp();
            var custId = HttpContext.Session.GetInt32("UserID");
            if (custId != null)
            {
                order.Quantity = 1;
                order.UserFk = custId;
                order.HandcraftFk = HandId;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return Json(1);

            }

            return Json(0);
        }
        public async Task<JsonResult> AddCart1(int Handfk)
        {
            OrdersFp order = new OrdersFp();
            var custId = HttpContext.Session.GetInt32("UserID");
            if (custId != null)
            {
                order.Quantity = 1;
                order.UserFk = custId;
                order.HandcraftFk = Handfk;
                _context.Add(order);
                await _context.SaveChangesAsync();
                
                return Json(1);

            }

            return Json(0);
        }
        public IActionResult delfCart(decimal id)
        {
            FavouriteFp favourite = new FavouriteFp();
            var custId = HttpContext.Session.GetInt32("UserID");
            if (custId != null)
            {
                favourite.FavId = id;
                _context.Remove(favourite);
                 _context.SaveChangesAsync();

                return RedirectToAction("favourite", "HandcraftFps");
                ;

            }

            return RedirectToAction("favourite", "HandcraftFps");
            ;
        }
        public async Task<JsonResult> favCart(int HandId)
        {
            FavouriteFp fav = new FavouriteFp();
            var custId = HttpContext.Session.GetInt32("UserID");
            if (custId != null)
            {
                fav.Userfk = custId;
                fav.Handfk = HandId;
                
                _context.Add(fav);
                await _context.SaveChangesAsync();
                return Json(1);

            }

            return Json(0);
        }
        public IActionResult PreviousPurchases()
        {
            var UserID = HttpContext.Session.GetInt32("UserID");
            var modelContext = _context.OrdersdoneFps.Where(x => x.UserFk == UserID).OrderByDescending(x => x.HandcraftFkNavigation.Handcraft).Take(5).Include(o=>o.HandcraftFkNavigation).ToList();

            return View(modelContext);
        }
        public IActionResult favourite()
        {
            var UserID = HttpContext.Session.GetInt32("UserID");
            var modelContext = _context.FavouriteFps.Where(x => x.Userfk == UserID).Include(o =>o.HandfkNavigation).ToList();

            return View(modelContext);
        }

        private bool HandcraftFpExists(decimal id)
        {
            return _context.HandcraftFps.Any(e => e.HandId == id);
        }
    }
}
