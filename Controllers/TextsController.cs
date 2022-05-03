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
    public class TextsController : Controller
    {
        private readonly MvcTextContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
            public TextsController(MvcTextContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reviews.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var text = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (text == null)
            {
                return NotFound();
            }

            return View(text);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ID,Title,Description,IImage")] Text text, IFormFile IImage)
        {
            if (ModelState.IsValid)
            {
                var textt = await _context.Reviews.FindAsync(id);
                if(textt != null) 
                {
                    _context.Reviews.Remove(textt);
                    await _context.SaveChangesAsync();
                }    
                
                using (var memoryStream = new MemoryStream())
                {
                    if (IImage != null)
                    {
                    await IImage.CopyToAsync(memoryStream);
                    text.Image = memoryStream.ToArray();
                    }
                }
                _context.Add(text);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(text);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var text = await _context.Reviews.FindAsync(id);

            if (text == null)
            {
                return NotFound();
            }
            return View(text);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,Description,IImage")] Text text)
        {

            if (id != text.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {      
                _context.Update<Text>(text);
                await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TextExists(text.Id))
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

            var text = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (text == null)
            {
                return NotFound();
            }

            return View(text);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var text = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(text);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}