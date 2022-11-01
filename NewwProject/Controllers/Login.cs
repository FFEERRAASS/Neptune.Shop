using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewwProject.Models;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NewwProject.Controllers
{
    public class Login : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Login(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult LoginUser()
        {
            
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword([Bind("Email")] UserFp user)
        {
            try{

                    
                    var x = _context.UserFps.Where(x => x.Email == user.Email).FirstOrDefault();
                    var id = x.UserId; 
                    var pass = _context.UserloginFps.Where(y=> y.UserFk == id).FirstOrDefault();
                    if (x != null)
                    {
                        SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential("Neptune-Shoop@outlook.com", "feras123");
                        UserloginFp userlog = new UserloginFp();
                        smtp.SendMailAsync("Neptune-Shoop@outlook.com", x.Email, "Very important and confidential mail","Password : "+ pass.UserPassword.ToString() +" \n" + "Neptune technical team");
                    ViewBag.ok = "A message containing the password has been sent";
                    }
            }
            catch
            {
                    
                    ModelState.AddModelError("notfount", "             Email not found , Try Again");
            }

            return View(user);
        }
        [HttpPost]

        public IActionResult LoginUser([Bind("Username , UserPassword")] UserloginFp userLogin)

        {
            try
            {
                var verf = _context.UserloginFps.Where(x => x.Username.ToUpper() == userLogin.Username.ToUpper() && x.UserPassword == userLogin.UserPassword).FirstOrDefault();


                //var user = _context.UserloginFps.Where(x => x.Username == userLogin.Username && x.UserPassword == userLogin.UserPassword).SingleOrDefault();
                if (verf != null)
                {

                    var name = _context.UserFps.Where(x => x.UserId == verf.UserFk).SingleOrDefault();
                    if (verf.IsAccepted == 1)
                    {
                        switch (verf.RoleFk)
                        {
                            case 1:
                                HttpContext.Session.SetInt32("AdminId", (int)verf.UserFk);
                                HttpContext.Session.SetString("Email", name.Email);
                                HttpContext.Session.SetString("image", name.ImagePath);
                                HttpContext.Session.SetString("Name", name.FirstName);
                                return RedirectToAction("Index", "Admin");

                            case 2:
                                HttpContext.Session.SetInt32("VendorID", (int)verf.UserFk);
                                HttpContext.Session.SetString("Email1", name.Email);
                                HttpContext.Session.SetString("image1", name.ImagePath);
                                HttpContext.Session.SetString("Name1", name.FirstName);
                                return RedirectToAction("Index", "Vendor");

                            case 3:
                                HttpContext.Session.SetInt32("UserID", (int)verf.UserFk);
                                HttpContext.Session.SetString("Email2", name.Email);
                                HttpContext.Session.SetString("Username", userLogin.Username);
                                //HttpContext.Session.SetString("image2", name.ImagePath);
                                HttpContext.Session.SetString("Name2", name.FirstName);
                                return RedirectToAction("Index", "Home");
                            case 4:
                                ModelState.AddModelError("Again", "Unfortunately, you have been banned from accessing the site");
                                break;


                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Again", "Admin Not Accepted");
                    }

                }
                else
                {
                    ModelState.AddModelError("Again", "Try Again , Wrong password or username ");

                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        

        [HttpPost]

        public async Task<IActionResult> Register([Bind("UserId,FirstName,LastName,Email,ImagePath,Gender,IsAccepted,CraftName,RoleFk,ImageFile")] UserFp user, string Username, string Password)
        {
            if (ModelState.IsValid)
            {

                UserloginFp logins = new UserloginFp();

                var Uniqe = _context.UserloginFps.Where(x => x.Username == Username).FirstOrDefault();
                var Uniqe_Email = _context.UserFps.Where(x => x.Email == user.Email).FirstOrDefault();
                if (Uniqe == null && Uniqe_Email == null)
                {
                    if (user.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await user.ImageFile.CopyToAsync(fileStream);

                        }
                        user.RoleFk = 3;
                        user.ImagePath = fileName;
                        _context.Add(user);
                        await _context.SaveChangesAsync();

                        logins.Username = Username;
                        logins.UserPassword = Password;
                        logins.UserFk = user.UserId;
                        logins.RoleFk = 3;
                        logins.IsAccepted = 1;
                        _context.Add(logins);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        user.RoleFk = 3;
                        user.ImagePath = "istockphoto-1223671392-612x612.jpg";
                        _context.Add(user);
                        await _context.SaveChangesAsync();

                        logins.Username = Username;
                        logins.UserPassword = Password;
                        logins.UserFk = user.UserId;
                        logins.RoleFk = 3;
                        logins.IsAccepted = 1;

                        _context.Add(logins);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(LoginUser));



                    }

                }
                else
                {
                    ModelState.AddModelError("Again", "The username you are trying to register with or the email is already in use, try again");
                }




            }

            return View(user);

        }






        public IActionResult RegisterVendor()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> RegisterVendor([Bind("UserId,FirstName,LastName,Email,ImagePath,Gender,IsAccepted,CraftName,RoleFk,ImageFile")] UserFp user, string username, string password)
        {
            var Uniqe = _context.UserloginFps.Where(x => x.Username == username).FirstOrDefault();
            var Uniqe_Email = _context.UserFps.Where(x => x.Email == user.Email).FirstOrDefault();
            if(Uniqe == null && Uniqe_Email == null) { 
            if (ModelState.IsValid)
            {
                if (user.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);

                    }
                    user.RoleFk = 2;

                    user.ImagePath = fileName;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    UserloginFp logins = new UserloginFp();
                    logins.Username = username;
                    logins.UserPassword = password;
                    logins.UserFk = user.UserId;
                    logins.RoleFk = 2;
                    logins.IsAccepted = 0;

                    _context.Add(logins);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    user.RoleFk = 2;

                    user.ImagePath = "istockphoto-1223671392-612x612.jpg";
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    UserloginFp logins = new UserloginFp();
                    logins.Username = username;
                    logins.UserPassword = password;
                    logins.UserFk = user.UserId;
                    logins.RoleFk = 2;
                    logins.IsAccepted = 0;

                    _context.Add(logins);
                    await _context.SaveChangesAsync();

                }
               
                return RedirectToAction(nameof(LoginUser));
            }
            
            }
            else
            {
                ModelState.AddModelError("Again", "The username you are trying to register with or the email is already in use, try again");
            }
            return View(user);
        }



    }
}

