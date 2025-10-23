using Microsoft.AspNetCore.Mvc;
using ResumeScreeningSystem.Models;
using ResumeScreeningSystem.Data;
using Microsoft.EntityFrameworkCore;



namespace ResumeScreeningSystem.Controllers
{
    public class JobPostingController : Controller
    {
        public readonly ApplicationDbContext _context;


        public JobPostingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jobs = _context.jobPostings
            .Include(j=> j.Recruiter)
            .ToList();
            return View(jobs);
        }

        public IActionResult Create()
        {
            var recruiters = _context.recruiters.ToList();
            if(recruiters==null || !recruiters.Any())
            {
                TempData["Error"] = "No Recruiter availables. Please add a recruiter first";
                return RedirectToAction("Index", "Recruiter");
            }

            ViewBag.Recruiters = _context.recruiters.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JobPosting job)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Recruiters = _context.recruiters.ToList();
                return View(job);
            }
            _context.jobPostings.Add(job);
            _context.SaveChanges();
            TempData["Sucess"] = "Job posting created successfully";
            return RedirectToAction("Index");


        }

        public IActionResult Edit( int id)
        {
            var job = _context.jobPostings.Find(id);
            if (job == null)
            
                     return NotFound();

            return View(job);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (JobPosting job)
        {
            if (!ModelState.IsValid)
                return View();

            _context.jobPostings.Update(job);
                _context.SaveChanges();
            TempData["Success"] = "Job posting updated sucessfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete (int id)
        {
            var job = _context.jobPostings.Find(id);
            if (job == null)
            {
                return NotFound();
            }
                _context.jobPostings.Remove(job);
                _context.SaveChanges();
                TempData["Success"] = "Job posting Deleted sucessfully";
                return RedirectToAction(nameof(Index));
            
        }
    }
}
