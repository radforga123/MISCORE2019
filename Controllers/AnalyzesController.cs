using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MISCORE2019;
using MISCORE2019.Models;
using Microsoft.AspNetCore.Authorization;
namespace MISCORE2019.Controllers
{
    public class AnalyzesController : Controller
    {
        private readonly PatientContext _context;

        public AnalyzesController(PatientContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Analyzes.ToListAsync());
        }
        [Authorize(Roles ="doctor,admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,weight,diagnos,PatientID")] Analyze analyze)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analyze);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analyze);
        }
        [Authorize(Roles = "doctor,admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analyze = await _context.Analyzes.FindAsync(id);
            if (analyze == null)
            {
                return NotFound();
            }
            return View(analyze);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,weight,diagnos,PatientID")] Analyze analyze)
        {
            if (id != analyze.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analyze);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalyzeExists(analyze.ID))
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
            return View(analyze);
        }
        [Authorize(Roles = "doctor,admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analyze = await _context.Analyzes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (analyze == null)
            {
                return NotFound();
            }

            return View(analyze);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var analyze = await _context.Analyzes.FindAsync(id);
            _context.Analyzes.Remove(analyze);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalyzeExists(int id)
        {
            return _context.Analyzes.Any(e => e.ID == id);
        }
    }
    }
