using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewwProject.Models;
using System.Diagnostics;
using Aspose.Pdf;
using System.IO;
using Aspose.Pdf.Annotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Aspose.Pdf.Operators;

namespace NewwProject.Controllers
{
    public class OrdersFpsController : Controller
    {
        private readonly ModelContext _context;

        public OrdersFpsController(ModelContext context)
        {
            _context = context;
        }

        // GET: OrdersFps
        public IActionResult Index()
        {
            ViewBag.visa = HttpContext.Session.GetString("visa");
            var id = HttpContext.Session.GetInt32("UserID");
            var order = _context.OrdersFps.Where(x => x.UserFk == id).Include(o => o.HandcraftFkNavigation).Include(o => o.UserFkNavigation);
            var visa = _context.VisaFps.ToList();
            var tuble = Tuple.Create(order, visa);

            ViewBag.cost = _context.OrdersFps.Where(x => x.UserFk == id).Sum(x => x.HandcraftFkNavigation.Price);
            var modelContext = _context.OrdersFps.Where(x => x.UserFk == id).Include(o => o.HandcraftFkNavigation).Include(o => o.UserFkNavigation);
            return View(modelContext);
        }

        // GET: OrdersFps/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersFp = await _context.OrdersFps
                .Include(o => o.HandcraftFkNavigation)
                .Include(o => o.UserFkNavigation)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (ordersFp == null)
            {
                return NotFound();
            }

            return View(ordersFp);
        }

        // GET: OrdersFps/Create
        public IActionResult Create()
        {
            ViewData["HandcraftFk"] = new SelectList(_context.HandcraftFps, "HandId", "Descriptions");
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email");
            return View();
        }

        // POST: OrdersFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,HandcraftFk,UserFk")] OrdersFp ordersFp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordersFp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HandcraftFk"] = new SelectList(_context.HandcraftFps, "HandId", "Descriptions", ordersFp.HandcraftFk);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", ordersFp.UserFk);
            return View(ordersFp);
        }

        // GET: OrdersFps/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersFp = await _context.OrdersFps.FindAsync(id);
            if (ordersFp == null)
            {
                return NotFound();
            }
            ViewData["HandcraftFk"] = new SelectList(_context.HandcraftFps, "HandId", "Descriptions", ordersFp.HandcraftFk);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", ordersFp.UserFk);
            return View(ordersFp);
        }

        // POST: OrdersFps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("OrderId,HandcraftFk,UserFk")] OrdersFp ordersFp)
        {
            if (id != ordersFp.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordersFp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersFpExists(ordersFp.OrderId))
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
            ViewData["HandcraftFk"] = new SelectList(_context.HandcraftFps, "HandId", "Descriptions", ordersFp.HandcraftFk);
            ViewData["UserFk"] = new SelectList(_context.UserFps, "UserId", "Email", ordersFp.UserFk);
            return View(ordersFp);
        }

        private bool OrdersFpExists(decimal orderId)
        {
            throw new NotImplementedException();
        }

        // GET: OrdersFps/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersFp = await _context.OrdersFps
                .Include(o => o.HandcraftFkNavigation)
                .Include(o => o.UserFkNavigation)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (ordersFp == null)
            {
                return NotFound();
            }

            return View(ordersFp);
        }

        // POST: OrdersFps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var ordersFp = await _context.OrdersFps.FindAsync(id);
            _context.OrdersFps.Remove(ordersFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CheckOut(string cart, string numbC, string datee, string cvv, OrdersFp orders,string total)
        {

            // هون بتصير عملية الدفع + عملية انشاء الفاتورة بصيغة بي دي اف + ارسال الفاتورة عبر الأيميل
            
            var custId = HttpContext.Session.GetInt32("UserID");
            var email = HttpContext.Session.GetString("Email2");
            Random rnd = new Random();
            int num = rnd.Next();
            var order = _context.OrdersFps.Include(o => o.HandcraftFkNavigation).Include(o => o.UserFkNavigation).Where(x => x.UserFk == custId).ToList();
            var visa = _context.VisaFps.Where(x => x.FullName == cart && x.Iban == numbC && x.ExpiredDate == datee && x.Cvv == cvv).SingleOrDefault();
            if (visa != null)
            {
                if((float)visa.Balance >= float.Parse(total))
                {
                    OrdersdoneFp ord = new OrdersdoneFp();
                    for (int i = 0; i < order.Count; i++)
                    {
                        ord.Odone = num++;
                        ord.Quantity = order[i].Quantity;
                        ord.HandcraftFk = order[i].HandcraftFk;
                        ord.UserFk = order[i].UserFk;
                        ord.DateShop = DateTime.Now;


                        //ord.HandcraftFkNavigation.Handcraft = order[i].HandcraftFkNavigation.;
                        _context.Add(ord);
                        _context.SaveChangesAsync();

                    }
                    visa.Balance = visa.Balance - decimal.Parse(total);
                    _context.Update(visa);
                    _context.SaveChangesAsync();






                    Document document = new Document();
                    Page page = document.Pages.Add();

                    Aspose.Pdf.Table table = new Aspose.Pdf.Table();
                    table.Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Aqua));
                    // Add text to new page
                    table.DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Green));
                    //var invoice = _context.OrdersdoneFps.Where(x => x.UserFk == custId).ToList();
                    var card1 = _context.OrdersFps.Where(x => x.UserFk == custId).Include(o => o.HandcraftFkNavigation).ToList();
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Neptune Shop"));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("------------------------------------------------------------------------------------------------------------------------"));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
                    page.ArtBox.Center();


                    //for (int row_count = 1; row_count < 1; row_count++)
                    {
                        // Add row to table
                        Aspose.Pdf.Row row = table.Rows.Add();
                        // Add table cells
                        row.Cells.Add("The Date Of Purchase");
                        row.Cells.Add("Product Name");
                        row.Cells.Add("Product Price");
                        row.Cells.Add("Quantity");

                    }


                    foreach (var item in card1)
                    {
                        Aspose.Pdf.Row row = table.Rows.Add();
                        // Add table cells  ==> Here Edit
                        row.Cells.Add(DateTime.Now.ToString());
                        row.Cells.Add(item.HandcraftFkNavigation.Handcraft);
                        row.Cells.Add(item.HandcraftFkNavigation.Sale.ToString());
                        row.Cells.Add(item.Quantity.ToString());
                    }


                    // Add table to the page
                    page.Paragraphs.Add(table);
                    //var sum = _context.OrdersFps.Where(x => x.UserFk == custId).Sum(x => x.HandcraftFkNavigation.Price);

                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("------------------------------------------------------------------------------------------------------------------------"));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Total Price : " + total));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Thank you for shopping, Neptune "));

                    document.Save(num + "document.pdf");
                    SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com", 587);
                    mail.From = new MailAddress("Neptune-Shoop@outlook.com");
                    mail.To.Add(email);
                    mail.Subject = "Purchase Invoice";
                    mail.Body = "FERAS";
                    smtp.Credentials = new NetworkCredential("Neptune-Shoop@outlook.com", "feras123");
                    Attachment data = new Attachment(num + "document.pdf");
                    smtp.EnableSsl = true;

                    mail.Attachments.Add(data);
                    smtp.Send(mail);

                    for (int i = 0; i < order.Count; i++)
                    {
                        _context.Remove(order[i]);
                        _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));












                }
                else
                {
                    HttpContext.Session.SetString("visa", "Balance is not enough");
                    return RedirectToAction(nameof(Index));
                }
            }

               
            else
            {
                HttpContext.Session.SetString("visa", "Card information is incorrect");
                ViewBag.visa = "Card information is incorrect";
                return RedirectToAction(nameof(Index));
            }
            
           


        }
        public async Task<IActionResult> DeleteItems(decimal id)
        {
            var ordersFp = await _context.OrdersFps.FindAsync(id);
            _context.OrdersFps.Remove(ordersFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
    
}
