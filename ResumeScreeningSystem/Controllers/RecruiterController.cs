using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;

namespace ResumeScreeningSystem.Controllers
{
    public class RecruiterController : Controller
    {
        public readonly ApplicationDbContext _context;


        public RecruiterController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Recruiters = _context.recruiters.ToList();
            return View(Recruiters);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recruiter recruiter)
        {
            if (!ModelState.IsValid)
            {
                return View(recruiter);
            }
            _context.recruiters.Add(recruiter);
            _context.SaveChanges();
            TempData["Success"] = "Recruiter created successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var recruiter = _context.recruiters.Find(id);
            if (recruiter == null)
                return NotFound();
            return View(recruiter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Recruiter recruiter)
        {
            if (!ModelState.IsValid)
            {
                return View(recruiter);
            }
            _context.recruiters.Update(recruiter);
            _context.SaveChanges();
            TempData["Success"] = "Recruiter updated successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var recruiter = _context.recruiters
                .Include(r => r.JobPostings)
                .FirstOrDefault(r => r.Id == id);

            if (recruiter == null)

                return NotFound();

            if (recruiter.JobPostings != null && recruiter.JobPostings.Any())
            {
                TempData["Error"] = "Cannot delete recruiter with existing job postings.";
                return RedirectToAction(nameof(Index));

            }

            _context.recruiters.Remove(recruiter);
            _context.SaveChanges();
            TempData["Success"] = "Recruiter deleted successfully";
            return RedirectToAction(nameof(Index));


        }

    }

 }