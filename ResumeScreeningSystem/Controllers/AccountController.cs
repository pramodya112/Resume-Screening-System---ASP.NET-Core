using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ResumeScreeningSystem.Models;
using ResumeScreeningSystem.Data;
using System.Linq;

namespace ResumeScreeningSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Hardcoded admin credentials
        private const string AdminUsername = "admin";
        private const string AdminPassword = "Admin@123";

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            // If already logged in, redirect appropriately
            if (HttpContext.Session.GetString("IsAuthenticated") == "true")
            {
                var role = HttpContext.Session.GetString("UserRole");
                if (role == "Admin")
                    return RedirectToAction("Index", "JobPosting");
                else
                    return RedirectToAction("Index", "Resume");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if admin login
            if (model.Username == AdminUsername && model.Password == AdminPassword)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", model.Username);
                HttpContext.Session.SetString("UserRole", "Admin");

               TempData["ShowAlert"] = "Login successful!";
                TempData["Success"] = "Welcome Admin!";
                return RedirectToAction("Index", "JobPosting");
            }

            // Check if recruiter login (using email as username)
            var recruiter = _context.recruiters
                .FirstOrDefault(r => r.Email == model.Username && r.password == model.Password);

            if (recruiter != null)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", recruiter.RecruiterName);
                HttpContext.Session.SetString("UserRole", "Recruiter");
                HttpContext.Session.SetInt32("RecruiterId", recruiter.Id);

                TempData["Success"] = $"Welcome {recruiter.RecruiterName}!";
                return RedirectToAction("Index", "Resume");
            }

            ModelState.AddModelError("", "Invalid username/email or password");
            TempData["Error"] = "Invalid credentials. Please try again.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}