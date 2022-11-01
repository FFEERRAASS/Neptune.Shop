using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewwProject.Models;

namespace NewwProject.Controllers
{
    public class ConmsgFpsController : Controller
    {
        private readonly ModelContext _context;

        public ConmsgFpsController(ModelContext context)
        {
            _context = context;
        }

        // GET: ConmsgFps
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConmsgFps.ToListAsync());
        }

        // GET: ConmsgFps/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conmsgFp = await _context.ConmsgFps
                .FirstOrDefaultAsync(m => m.ContId == id);
            if (conmsgFp == null)
            {
                return NotFound();
            }

            return View(conmsgFp);
        }

        // GET: ConmsgFps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConmsgFps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContId,FullName,Email,Subject,Msg")] ConmsgFp conmsgFp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conmsgFp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conmsgFp);
        }

        // GET: ConmsgFps/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conmsgFp = await _context.ConmsgFps.FindAsync(id);
            if (conmsgFp == null)
            {
                return NotFound();
            }
            return View(conmsgFp);
        }

        // POST: ConmsgFps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("ContId,FullName,Email,Subject,Msg")] ConmsgFp conmsgFp)
        {
            if (id != conmsgFp.ContId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conmsgFp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConmsgFpExists(conmsgFp.ContId))
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
            return View(conmsgFp);
        }

        // GET: ConmsgFps/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conmsgFp = await _context.ConmsgFps
                .FirstOrDefaultAsync(m => m.ContId == id);
            if (conmsgFp == null)
            {
                return NotFound();
            }

            return View(conmsgFp);
        }

        // POST: ConmsgFps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var conmsgFp = await _context.ConmsgFps.FindAsync(id);
            _context.ConmsgFps.Remove(conmsgFp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConmsgFpExists(decimal id)
        {
            return _context.ConmsgFps.Any(e => e.ContId == id);
        }
    }
}
