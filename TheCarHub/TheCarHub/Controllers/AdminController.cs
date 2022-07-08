using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCarHub.Data;
using TheCarHub.Models;

namespace TheCarHub.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cars.Include(c => c.Make).Include(c => c.Model).ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["Makes"]  = new SelectList(_context.Makes.ToList(), "Id", "Name");
            ViewData["Models"] = new SelectList(_context.Models.ToList(), "Id", "Name");

            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SaleDate,Status")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.Make = await _context.Makes.FindAsync(car.MakeId);
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Include(c => c.Make).Include(c => c.Model).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (car == null)
            {
                return NotFound();
            }

            ViewData["Makes"] = new SelectList(_context.Makes.ToList(), "Id", "Name", car.Make);
            ViewData["Models"]= new SelectList(_context.Models.ToList(), "Id", "Name", car.Model);
            return View(car);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SaleDate,Status")] Car car)
        {
            if (id != car.Id)
            {
                car.Id = id;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewImage(int id, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(image);

                Image createdImage = new Image
                {
                    CarId = id,
                    FileName = uniqueFileName
                };
                _context.Images.Add(createdImage);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        private string UploadedFile(IFormFile image)
        {
            string uniqueFileName = null;

            if (image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
