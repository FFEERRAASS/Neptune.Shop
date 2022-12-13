using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewwProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace NewwProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger , ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            ViewBag.category = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "category").SingleOrDefault();
            ViewBag.product = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "product").SingleOrDefault();
            ViewBag.test = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "testimon").SingleOrDefault();

            var curs = "curs";
            ViewBag.curs = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0,4) == curs).ToList();
            
            ViewBag.UserId = HttpContext.Session.GetInt32("UserID");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            
            var testmonial = _context.ReviewFps.Include(o=>o.UserFkNavigation).Where(o=>o.Checks == 1).ToList();
            var catagory=_context.CategoryFps.ToList();
            var handcraft=_context.HandcraftFps.ToList();
            var home = _context.HomeFps.ToList();
            var Model = Tuple.Create <IEnumerable<ReviewFp>, IEnumerable<CategoryFp>, IEnumerable<HandcraftFp>, IEnumerable<HomeFp>> (testmonial,catagory,handcraft,home);
            return View(Model);
        }
        public IActionResult Index1()
        {
            ViewBag.footer = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "location").SingleOrDefault();

            ViewBag.category = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "category").SingleOrDefault();
            ViewBag.product = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "product").SingleOrDefault();
            ViewBag.test = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 8) == "testimon").SingleOrDefault();
            //var synthesizer = new SpeechSynthesizer();
            //synthesizer.SetOutputToDefaultAudioDevice();
            //synthesizer.Speak("Welcome to neptune shop");
            var testmonial = _context.ReviewFps.Include(o => o.UserFkNavigation).Where(o => o.Checks == 1).ToList();
            var catagory = _context.CategoryFps.ToList();
            var handcraft = _context.HandcraftFps.ToList();
            var home = _context.HomeFps.ToList();
            var curs1 = "curs";
            ViewBag.curs = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 4) == curs1).ToList();
            var Model = Tuple.Create<IEnumerable<ReviewFp>, IEnumerable<CategoryFp>, IEnumerable<HandcraftFp>, IEnumerable<HomeFp>>(testmonial, catagory, handcraft, home);
            return View(Model);
        }

        public IActionResult About()
        {
            ViewBag.about = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 5) == "About").SingleOrDefault();
            var list=_context.HomeFps.ToList();
            return View(list);
        }
        public IActionResult About1()
        {
            ViewBag.about = _context.HomeFps.Where(x => x.Homeparagraph1.Substring(0, 5) == "About").SingleOrDefault();

            var list = _context.AboutusFps.ToList();
            return View(list);
        }
        public IActionResult Product(int id)
        {
            var xyz = _context.HandcraftFps.Where(x => x.CategoryId == id);
            return View(xyz);
        }
        [HttpPost]
        public async Task<JsonResult> UpdataQty(int id, int qty)
        {

            var cart = _context.OrdersFps.Find((decimal)id);

            cart.Quantity = qty;
            _context.OrdersFps.Update(cart);

            await _context.SaveChangesAsync();

            return Json(cart);
        }

        public IActionResult Shoping()
        {
            var UserID = HttpContext.Session.GetInt32("UserID");
            var modelContext = _context.OrdersFps.Where(x => x.UserFk == UserID).ToList();

            return View(modelContext);
        }
        [HttpGet]
        public IActionResult PreviousPurchases()
        {
            var UserID = HttpContext.Session.GetInt32("UserID");
            var modelContext = _context.OrdersFps.Where(x => x.UserFk == UserID).ToList();

            return View(modelContext);
        }
       
        public IActionResult Profile()
        {
            var id = HttpContext.Session.GetInt32("UserID");
            if (id == null)
            {
                return NotFound();
            }

            var profile =  _context.UserFps.Find((decimal)id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(decimal id, [Bind("UserID,FirstName,LastName,Email,ImagePath,ImageFile")] UserFp profile  ,string Username, string Password)
        {
            if (id != profile.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (profile.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + profile.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            profile.ImageFile.CopyToAsync(fileStream);
                        }
                        profile.ImagePath = fileName;
                    }
                    else
                    {
                        profile.ImagePath = _context.UserFps.Where(x => x.UserId == id).AsNoTracking<UserFp>().SingleOrDefault().ImagePath;
                    }
                    var logininfo = _context.UserloginFps.Where(x => x.UserFk == profile.UserId).SingleOrDefault();
                   
                    
                        if (Username == null || Password == null)
                        {
                            _context.Update(profile);
                            _context.SaveChangesAsync();
                            logininfo.Username = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().Username;
                            logininfo.UserPassword = _context.UserloginFps.Where(x => x.UserFk == id).AsNoTracking<UserloginFp>().SingleOrDefault().UserPassword;
                            _context.Update(logininfo);
                             _context.SaveChangesAsync();
                        }
                        else
                        {
                            _context.Update(profile);
                             _context.SaveChangesAsync();
                            logininfo.Username = Username;
                            logininfo.UserPassword = Password;
                            _context.Update(logininfo);
                            _context.SaveChangesAsync();
                        }
              
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserFppExists(profile.UserId))
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
            
            return View(profile);

            

        }
        public IActionResult Contact1()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact1([Bind("ContId,FullName,Email,Subject,Msg")] ConmsgFp conmsgFp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conmsgFp);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conmsgFp);
        }
        public IActionResult Contact()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact([Bind("ContId,FullName,Email,Subject,Msg")] ConmsgFp conmsgFp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conmsgFp);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conmsgFp);
        }
        private bool UserFppExists(decimal userId)
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //public async Task<JsonResult> AddCart(int HandId)
        //{
        //    OrdersFp order = new OrdersFp();
        //var custId = HttpContext.Session.GetInt32("UserID");
        //    if (custId != null)
        //    {
        //        order.UserFk = custId;
        //        order.HandcraftFk = HandId;
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return Json(1);

        //    }

        //        return Json(0);
        //    }
        public IActionResult Testimonial()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Testimonial(string review , string fav)
        {

            ReviewFp reviewFp = new ReviewFp();
            var custId = HttpContext.Session.GetInt32("UserID");
            if (custId != null)
            {
                reviewFp.Checks = 0;
                reviewFp.UserFk = custId;
                reviewFp.Review = review;
                reviewFp.Rate = fav;
                _context.Add(reviewFp);
                await _context.SaveChangesAsync();
                return View();

            }
            

            return View();
        }
        public IActionResult logout()
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak("Welcome my Shop , Mohammad Obeidat");
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index1));
        }
    }
}
