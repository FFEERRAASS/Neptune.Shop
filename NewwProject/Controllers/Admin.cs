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

namespace NewwProject.Controllers
{
	public class Admin : Controller
	{
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Admin(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            ViewBag.totalsold = _context.OrdersdoneFps.Sum(x => x.Quantity);

            ViewBag.Firstn=HttpContext.Session.GetString("Name");
            ViewBag.Image =HttpContext.Session.GetString("image");
            ViewBag.sumUser = _context.UserFps.Where(x => x.RoleFk == 3).Count();
            ViewBag.sumVendor = _context.UserFps.Where(x => x.RoleFk == 2).Count();
            ViewBag.userblocked = _context.UserFps.Where(x=>x.RoleFk == 4).Count();
            ViewBag.sumProduct = _context.HandcraftFps.Count();
            ViewBag.money = _context.OrdersdoneFps.Sum(x => x.HandcraftFkNavigation.Sale * x.Quantity);
            ViewBag.test = _context.ReviewFps.Count();
            var m1 = _context.Emails.Where(x=> x.SenderReciver == 0).Include(x=>x.UserFkNavigation).OrderByDescending(x=>x.EmailId).Take(5).ToList();
            var m2 = _context.ConmsgFps.OrderByDescending(x => x.ContId).Take(5).ToList();
   

            var tuble = Tuple.Create<IEnumerable<Email>, IEnumerable<ConmsgFp>>(m1, m2);

            return View(tuble);
		}
        public IActionResult Vendor()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model1 = _context.UserFps.Where(x => x.RoleFk == 2).ToList();
            return View(model1);
        }
        public IActionResult VendorRequest()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model2 = _context.UserloginFps.Where(x => x.IsAccepted == 0).Include(o => o.UserFkNavigation).ToList();
            return View(model2);
        }
        public IActionResult fainance()
        {

            var sumproduct = _context.OrdersdoneFps.Count();

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            ViewBag.totalsold = _context.OrdersdoneFps.Sum(x=>x.Quantity);
            ViewBag.totalrevenue = _context.OrdersdoneFps.Sum(x => x.HandcraftFkNavigation.Sale * x.Quantity);
            ViewBag.VendorsMoney = _context.OrdersdoneFps.Sum(x => x.HandcraftFkNavigation.Price * x.Quantity);
            if(ViewBag.VendorsMoney > ViewBag.totalrevenue)
            {
                ViewBag.losses = ViewBag.VendorsMoney - ViewBag.totalrevenue;
            }
            else
            {
                ViewBag.losses = 0;
            }
            var model = _context.OrdersdoneFps.Include(o => o.HandcraftFkNavigation).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult fainance(DateTime? startDate, DateTime? endDate)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var date = _context.OrdersdoneFps.Include(o => o.HandcraftFkNavigation).ToList();
           
            if (startDate == null || endDate == null)
            {
                return View(date);
            }
            else if (startDate == null && endDate != null)
            {
                var result = date.Where(x => x.DateShop == endDate).ToList();
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                var result = date.Where(x => x.DateShop == startDate).ToList();
                return View(result);
            }
            else
            {
                var result = date.Where(x => x.DateShop >= startDate && x.DateShop <= endDate).ToList();
                return View(result);
            }
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Loginuser", "Login");
        }
        public IActionResult Accept(decimal? id, UserFp user, UserloginFp userlogin)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var x = _context.UserloginFps.Where(x => x.UserFk == id).SingleOrDefault();
            x.IsAccepted = 1;
            _context.Update(x);
            _context.SaveChanges();
            var email = _context.UserFps.Where(x => x.UserId == id).SingleOrDefault();
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("Neptune-Shoop@outlook.com", "feras123");
            UserloginFp userlog = new UserloginFp();
            smtp.SendMailAsync("Neptune-Shoop@outlook.com", email.Email , "Neptune Shop", "Your request to join the Neptune Shop family of sellers has been approved");
            return RedirectToAction("VendorRequest", "Admin");
        }

        public IActionResult Customer()
        {


            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model = _context.UserFps.Where(x=>x.RoleFk == 3).ToList();
			
            return View(model);
        }
        public IActionResult contact()
        {


            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model = _context.ConmsgFps.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult contact( string sname)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model = _context.ConmsgFps.Where(x=>x.FullName.Contains(sname)).ToList();
            return View(model);
        }
        public IActionResult contactV()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");

            var model = _context.Emails.Where(x=>x.SenderReciver == 0).Include(c=>c.UserFkNavigation).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult contactV(string sname)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model = _context.Emails.Where(x => x.UserFkNavigation.FirstName.Contains(sname)).Where(x => x.SenderReciver == 0).Include(c => c.UserFkNavigation).ToList();
            return View(model);
        }
        public IActionResult reply(decimal id)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");

            return View();

        }
        [HttpPost]
        public IActionResult reply(decimal id,string EmailSubject, string EmailMsg, Email email)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            if (ModelState.IsValid)
            {
                email.UserFk = id;
                email.SenderReciver = 1;
                _context.Add(email);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                
            return View();
                
        }
        
        public IActionResult Block(decimal? id, UserFp user , UserloginFp userlogin)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var x = _context.UserloginFps.Where(x => x.UserFk == id).SingleOrDefault();
            if(x.RoleFk == 3)
            {
                x.RoleFk = 4;
                _context.Update(x);
                _context.SaveChanges();
                var block1 = _context.UserFps.Where(x => x.UserId == id).SingleOrDefault();
                block1.RoleFk = 4;
                _context.Update(x);
                _context.SaveChanges();
                return RedirectToAction("Customer", "Admin");
            }
            x.RoleFk = 4;
            _context.Update(x);
            _context.SaveChanges();
            var block = _context.UserFps.Where(x => x.UserId == id).SingleOrDefault();
            block.RoleFk = 4;
            _context.Update(x);
            _context.SaveChanges();
            return RedirectToAction("Vendor", "Admin");

        }
        public async Task<IActionResult> Edit()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");

            var id = HttpContext.Session.GetInt32("AdminId");
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
        public async Task<IActionResult> Edit(decimal id, [Bind("UserId,FirstName,LastName,Email,ImagePath,ImageFile,Gender,CraftName,RoleFk")] UserFp userFp, string Username, string Password , string oldPassword)
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");

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
                    if(logininfo.UserPassword == oldPassword)
                    {
                        if(Username == null || Password == null)
                        {
                            _context.Update(userFp);
                            await _context.SaveChangesAsync();
                            logininfo.Username = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().Username;
                            logininfo.UserPassword = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().UserPassword;
                            _context.Update(logininfo);
                            await _context.SaveChangesAsync();
                        }
                        else {
                            userFp.ImagePath = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().ImagePath;

                            _context.Update(userFp);
                        await _context.SaveChangesAsync();
                        logininfo.Username = Username ;
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
                    
                    return RedirectToAction(nameof(Edit));
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
        public IActionResult Profile()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var id = HttpContext.Session.GetInt32("AdminId");
            var Data = _context.UserFps.Where(x => x.UserId == id).Include(o => o.UserloginFps).ToList();
            var data1 = _context.UserloginFps.Where(x => x.UserFk == id).Include(o=>o.UserFkNavigation).ToList();
           
            return View(data1);
        }
        public IActionResult product()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var product = _context.HandcraftFps.Include(o => o.UserFkNavigation).Include(p=>p.Category).ToList();

            return View(product);
        }
        public IActionResult deleteProduct(decimal id)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var handDelete = _context.HandcraftFps.Where(x => x.HandId == id).SingleOrDefault();
            _context.HandcraftFps.Remove(handDelete);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(product));
        }
        public IActionResult unBlock()
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model = _context.UserFps.Where(x => x.RoleFk == 4).ToList();
            return View(model);
        }
        public IActionResult noblock(decimal id) 
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var x = _context.UserloginFps.Where(o=>o.UserFk == id).Include(x=>x.UserFkNavigation).SingleOrDefault();
            
            if (x.UserFkNavigation.CraftName == null)
            {
                x.RoleFk = 3;
                _context.Update(x);
                _context.SaveChanges();
                var block1 = _context.UserFps.Where(x => x.UserId == id).SingleOrDefault();
                block1.RoleFk = 3;
                _context.Update(x);
                _context.SaveChanges();
                return RedirectToAction("unBlock", "Admin");
            }
            x.RoleFk = 2;
            _context.Update(x);
            _context.SaveChanges();
            var block = _context.UserFps.Where(x => x.UserId == id).SingleOrDefault();
            block.RoleFk = 2;
            _context.Update(x);
            _context.SaveChanges();
            return RedirectToAction("unBlock", "Admin");
        }
     
        public IActionResult review()
        {
            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");
            var model = _context.ReviewFps.Include(x => x.UserFkNavigation).Where(o => o.Checks == 0).ToList();
            return View(model);
        }
     
        public IActionResult acceptt(decimal? id , ReviewFp reviewf)
        {

            ViewBag.Firstn = HttpContext.Session.GetString("Name");
            ViewBag.Image = HttpContext.Session.GetString("image");

            var x = _context.ReviewFps.Where(x => x.ReviewId == id).SingleOrDefault();
            x.Checks = 1;
            _context.Update(x);
            _context.SaveChanges();
            return RedirectToAction("review", "Admin");
        }

        private bool UserFpExists(decimal userId)
        {
            throw new NotImplementedException();
        }
    }
}
