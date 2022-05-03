using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.Controllers;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
namespace mvc.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MvcTextContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
            public EmployeesController(MvcTextContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Team.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ID,FullName,Description,IImage")] Employee employee, IFormFile IImage)
        {
            if (ModelState.IsValid)
            {
                var employeee = await _context.Team.FindAsync(id);
                if(employeee != null) 
                {
                    _context.Team.Remove(employeee);
                    await _context.SaveChangesAsync();
                }    
                
                using (var memoryStream = new MemoryStream())
                {
                    if (IImage != null)
                    {
                    await IImage.CopyToAsync(memoryStream);
                    employee.Image = memoryStream.ToArray();
                    }
                }
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Team.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,Description,IImage")] Employee employee)
        {

            if (id != employee.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {      
                _context.Update<Employee>(employee);
                await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Team.FindAsync(id);
            _context.Team.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}