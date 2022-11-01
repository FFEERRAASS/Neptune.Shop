using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewwProject.Models;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using Aspose.Pdf.Operators;


namespace NewwProject.Controllers
{
    public class Vendor : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Vendor(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            ViewBag.totalspro = _context.HandcraftFps.Where(x => x.UserFk == id).Count();
            ViewBag.totalsmon = _context.OrdersdoneFps.Where(x => x.HandcraftFkNavigation.UserFk == id).Sum(x => x.HandcraftFkNavigation.Price * x.Quantity);
            ViewBag.email = HttpContext.Session.GetString("Email2");
            var model = _context.HandcraftFps.Where(x => x.UserFk == id).ToList();
            return View(model);
        }
        public IActionResult AddCraft()
        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            ViewBag.email = HttpContext.Session.GetString("Email2");
            ViewData["CategoryId"] = new SelectList(_context.CategoryFps, "CategoryId", "CategoryName");
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email");
            return View();
        }

        // POST: HandcraftFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCraft([Bind("HandId,Handcraft,Price,Sale,Descriptions,ImagePath,ImageFile,UserFk,CategoryId")] HandcraftFp handcraftFp)
        {
            var id = HttpContext.Session.GetInt32("VendorID");
            if (ModelState.IsValid)
            {
                if (handcraftFp.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + handcraftFp.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await handcraftFp.ImageFile.CopyToAsync(fileStream);

                    }
                    handcraftFp.Sale = handcraftFp.Price + handcraftFp.Price / 10;
                    handcraftFp.UserFk = id;
                    handcraftFp.ImagePath = fileName;
                    _context.Add(handcraftFp);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }


                else
                {
                    ModelState.AddModelError("TRyy", "Try Again , All fields are required !");
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.CategoryFps, "CategoryId", "CategoryName", handcraftFp.CategoryId);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", handcraftFp.UserFk);
            return View(handcraftFp);
        }

        public IActionResult Revenues()
        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            ViewBag.email = HttpContext.Session.GetString("Email2");
            var model = _context.OrdersdoneFps.Where(x => x.HandcraftFkNavigation.UserFk == id).Include(p => p.HandcraftFkNavigation).ToList();
            ViewBag.totalspro = _context.OrdersdoneFps.Where(x => x.HandcraftFkNavigation.UserFk == id).Sum(x => x.Quantity);
            ViewBag.totalsmon = _context.OrdersdoneFps.Where(x => x.HandcraftFkNavigation.UserFk == id).Sum(x => x.HandcraftFkNavigation.Price * x.Quantity);
            return View(model);
        }
        [HttpPost]
        public IActionResult Revenues(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            ViewBag.email = HttpContext.Session.GetString("Email2");
            ViewBag.totalspro = _context.OrdersdoneFps.Where(x => x.HandcraftFkNavigation.UserFk == id).Sum(x => x.Quantity);
            ViewBag.totalsmon = _context.OrdersdoneFps.Where(x => x.HandcraftFkNavigation.UserFk == id).Sum(x => x.HandcraftFkNavigation.Price * x.Quantity);
            var date = _context.OrdersdoneFps.Include(o => o.HandcraftFkNavigation).ToList();
            if (startDate == null || endDate == null)
            {
                return View(date);
            }
            else if (startDate == null && endDate != null)
            {
                var result = date.Where(x => x.DateShop == endDate && x.HandcraftFkNavigation.UserFk == id).ToList();
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                var result = date.Where(x => x.DateShop == startDate && x.HandcraftFkNavigation.UserFk == id).ToList();
                return View(result);
            }
            else
            {
                var result = date.Where(x => x.DateShop >= startDate && x.DateShop <= endDate && x.HandcraftFkNavigation.UserFk == id).ToList();
                return View(result);
            }


        }

        public IActionResult Technicalsupport()
        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            ViewBag.email = HttpContext.Session.GetString("Email2");
            if (_context.Emails.Where(x => x.SenderReciver == 1).Count() != 0)
            {
                var em = _context.Emails.Where(x => x.SenderReciver == 1).Where(o => o.UserFk == id).ToList();
                return View(em);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Technicalsupport([Bind("UserFk,EmailSubject,EmailMsg")] Email em)
        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            ViewBag.email = HttpContext.Session.GetString("Email2");
            if (ModelState.IsValid)
            {
                em.UserFk = id;
                em.SenderReciver = 0;
                _context.Add(em);
                _context.SaveChangesAsync();

            }
            else
            {
                ModelState.AddModelError("Em", "Try Again");
            }
            if (_context.Emails.Where(x => x.SenderReciver == 1).Count() != 0)
            {
                var emm = _context.Emails.Where(x => x.SenderReciver == 1).Where(o => o.UserFk == id).ToList();
                return View(emm);
            }

            return View();
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Loginuser", "Login");
        }
        //public IActionResult Setting()
        //{
        //    var id = HttpContext.Session.GetInt32("VendorID");
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var profile = _context.UserFps.Find((decimal)id);
        //    if (profile == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(profile);
        //}
        //[HttpPost]
        //public IActionResult Setting(decimal id, [Bind("UserID,FirstName,LastName,Email,ImagePath,ImageFile")] UserFp profile, string Username, string Password)
        //{
        //    if (id != profile.UserId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (profile.ImageFile != null)
        //            {
        //                string wwwRootPath = _webHostEnvironment.WebRootPath;
        //                string fileName = Guid.NewGuid().ToString() + "_" + profile.ImageFile.FileName;
        //                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
        //                using (var fileStream = new FileStream(path, FileMode.Create))
        //                {
        //                    profile.ImageFile.CopyToAsync(fileStream);
        //                }
        //                profile.ImagePath = fileName;
        //            }
        //            else
        //            {
        //                profile.ImagePath = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().ImagePath;
        //            }
        //            var logininfo = _context.UserloginFps.Where(x => x.UserFk == profile.UserId).SingleOrDefault();


        //            if (Username == null || Password == null)
        //            {
        //                _context.Update(profile);
        //                _context.SaveChangesAsync();
        //                logininfo.Username = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().Username;
        //                logininfo.UserPassword = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().UserPassword;
        //                _context.Update(logininfo);
        //                _context.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                _context.Update(profile);
        //                _context.SaveChangesAsync();
        //                logininfo.Username = Username;
        //                logininfo.UserPassword = Password;
        //                _context.Update(logininfo);
        //                _context.SaveChangesAsync();
        //            }

        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserFppExists(profile.UserId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Indexuser));
        //    }

        //    return RedirectToAction(nameof(Indexuser));
        //}
        //public IActionResult EditProfile()
        //{
        //    var id = HttpContext.Session.GetInt32("VendorID");
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var profile = _context.UserFps.Find((decimal)id);
        //    if (profile == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(profile);
        //}
      

        public async Task<IActionResult> Setting()
        {

            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            if (id == null)
            {
                return NotFound();
            }

            var userFp = await _context.UserFps.FindAsync((decimal)id);
            var Login = await _context.UserloginFps.FindAsync((decimal)id);

            if (userFp == null && Login == null)
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
        public async Task<IActionResult> Setting(decimal id, [Bind("UserId,FirstName,LastName,Email,ImagePath,ImageFile,Gender,CraftName,RoleFk")] UserFp userFp, string Username, string Password, string oldPassword)
        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");

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
                    }

                    var logininfo = _context.UserloginFps.Where(x => x.UserFk == userFp.UserId).SingleOrDefault();
                    if (logininfo.UserPassword == oldPassword)
                    {
                        if (Username == null || Password == null)
                        {
                            userFp.RoleFk = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().RoleFk;

                            _context.Update(userFp);
                            await _context.SaveChangesAsync();
                            logininfo.Username = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().Username;
                            logininfo.UserPassword = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().UserPassword;
                            _context.Update(logininfo);
                            await _context.SaveChangesAsync();
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
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("error", "The old password is wrong");
                        return View();
                    }

                    return RedirectToAction(nameof(Indexuser));
                    //}

                    //else
                    //{
                    //    ModelState.AddModelError("Again", "The username you are trying to register with or the email is already in use, try again");

                    //}

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

        private bool UserFpExists(decimal userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Indexuser()

        {
            ViewBag.Image = HttpContext.Session.GetString("image1");
            ViewBag.Name = HttpContext.Session.GetString("Name1");
            var id = HttpContext.Session.GetInt32("VendorID");
            var modelContext = _context.UserFps.Include(u => u.RoleFkNavigation).Where(x => x.UserId == id);
            return View(await modelContext.ToListAsync());
        }
        private bool UserFppExists(decimal userId)
        {
            throw new NotImplementedException();
        }
    }
}
