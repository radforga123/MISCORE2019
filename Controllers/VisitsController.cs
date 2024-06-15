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
    public class VisitsController : Controller
    {
        private readonly PatientContext _context;

        public VisitsController(PatientContext context)
        {
            _context = context;
        }

        // GET: Visits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Visits.Include(v => v.doctors).ToListAsync());
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.doctors)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }
        [Authorize(Roles = "doctor,admin")]
        // GET: Visits/Create
        public IActionResult Create()
        {
            PopulateDoctorsDropDownList();

            return View();
        }

        [Authorize(Roles = "doctor,admin")]

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits.Include(v => v.doctors).FirstOrDefaultAsync(m => m.ID == id);
            if (visit == null)
            {
                return NotFound();
            }

            PopulateDoctorsDropDownList(visit.doctors.Select(d => d.ID).ToList());
            return View(visit);
        }


        // POST: Visits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Time,Doctors,PatientID")] Visit visit, int[] selectedDoctors)
        {
            if (id != visit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var visitToUpdate = await _context.Visits.Include(v => v.doctors).FirstOrDefaultAsync(m => m.ID == id);
                    if (visitToUpdate == null)
                    {
                        return NotFound();
                    }

                    visitToUpdate.time = visit.time;
                    visitToUpdate.doctors = new List<Doctor>();

                    foreach (var doctorId in selectedDoctors)
                    {
                        var doctor = await _context.Doctors.FindAsync(doctorId);
                        if (doctor != null)
                        {
                            visitToUpdate.doctors.Add(doctor);
                        }
                    }

                    _context.Update(visitToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.ID))
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
            PopulateDoctorsDropDownList(visit.doctors.Select(d => d.ID).ToList());
            return View(visit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Time,Doctors,PatientID")] Visit visit, int[] selectedDoctors)
        {
            if (ModelState.IsValid)
            {
                visit.doctors = new List<Doctor>();

                foreach (var doctorId in selectedDoctors)
                {
                    var doctor = await _context.Doctors.FindAsync(doctorId);
                    if (doctor != null)
                    {
                        visit.doctors.Add(doctor);
                    }
                }

                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDoctorsDropDownList(selectedDoctors.ToList());
            return View(visit);
        }
        [Authorize(Roles = "doctor,admin")]
        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .FirstOrDefaultAsync(m => m.ID == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private void PopulateDoctorsDropDownList(List<int> selectedDoctors = null)
        {
            var doctorsQuery = from d in _context.Doctors
                               orderby d.FIO
                               select new SelectListItem
                               {
                                   Value = d.ID.ToString(),
                                   Text = d.FIO,
                                   Selected = selectedDoctors != null && selectedDoctors.Contains(d.ID)
                               };
            ViewBag.Doctors = doctorsQuery.ToList();
        }
        
        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.ID == id);
        }

    }
}
