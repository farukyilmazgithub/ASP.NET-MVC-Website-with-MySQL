using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace mvc.Controllers
{
    public class ActivePageController : Controller
    {
        private readonly MvcTextContext _context;
        public ActivePageController(MvcTextContext context)
        {
            _context = context;
        }
        [Authorize]
        public ActionResult SecurePage()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new ViewModel();
            model.Team = _context.Team.ToList();
            model.Texts = _context.Reviews.ToList();
            return View(model);
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel()
        {
            return View();
        }
        public Text Text { get; set; }
        public Employee Team { get; set; }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}