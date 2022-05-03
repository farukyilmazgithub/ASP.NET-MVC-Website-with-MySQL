using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace mvc.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly MvcTextContext _context;
        public AdminPanelController(MvcTextContext context)
        {
            _context = context;
        }
        
        [Authorize]
        public ActionResult Index()
        {
            var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            var model = new ViewModel();
            model.Team = _context.Team.ToList();
            model.Texts = _context.Reviews.ToList();
            return View(model);
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