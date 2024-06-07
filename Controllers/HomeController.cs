using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MISCORE2019.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MISCORE2019.Controllers
{
    public class HomeController : Controller
    {
        PatientContext db;
        public HomeController(PatientContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Doctors.ToList() );
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult GetDoctor(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.DoctorID = id;
            return View();
        }
        [HttpPost]
        public void PostDoctor(Doctor doctor)
        {
            db.Doctors.Add(doctor);

            db.SaveChanges();

        }
        [HttpGet]
        public IActionResult GetPatient(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.PatientID = id;
            return View();
        }
        [HttpPost]
        public void PostPatient(PatientClass patient)
        {
            db.Patients.Add(patient);

            db.SaveChanges();

        }
        [HttpGet]
        public IActionResult GetAnalyze(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.AnalyzeID = id;
            return View();
        }
        [HttpPost]
        public void PostAnalyze(Analyze analyze)
        {
            db.Analyzes.Add(analyze);

            db.SaveChanges();

        }
        [HttpGet]
        public IActionResult GetVisit(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.AnalyzeID = id;
            return View();
        }
        [HttpPost]
        public void PostVisit(Visit visit)
        {
            db.Visits.Add(visit);

            db.SaveChanges();

        }
    }

   
}
