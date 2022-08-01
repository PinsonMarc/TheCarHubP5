using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
            return View(await _context.Cars.Include(c => c.Model).ThenInclude(m => m.Make).ToListAsync());
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

            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFiles,VIN,Year,ModelId,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SaleDate,Status")] CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                var images = await NewImages(carViewModel.Id, carViewModel.ImageFiles);

                Car car = new Car
                {
                    Id = carViewModel.Id,
                    VIN = carViewModel.VIN,
                    Year = carViewModel.Year,
                    ModelId = carViewModel.ModelId,
                    Trim = carViewModel.Trim,
                    PurchaseDate = carViewModel.PurchaseDate,
                    PurchasePrice = carViewModel.PurchasePrice,
                    Repairs = carViewModel.Repairs,
                    RepairCost = carViewModel.RepairCost,
                    LotDate = carViewModel.LotDate,
                    SaleDate = carViewModel.SaleDate,
                    Status = carViewModel.Status,
                    Images = images
                };
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carViewModel);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Include(c => c.Model).ThenInclude(m => m.Make).FirstOrDefaultAsync();
            if (car == null)
            {
                return NotFound();
            }

            ViewData["Makes"] = new SelectList (_context.Makes.ToList(), "Id", "Name", car.Model.Make);
            ViewData["Models"] = new SelectList(_context.Models.Where(m => m.MakeId == car.Model.MakeId).ToList(), "Id", "Name", car.Model);
            return View(car);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VIN,Year,ModelId,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SaleDate,Status")] Car car)
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

        private async Task<List<Image>> NewImages(int id, List<IFormFile> images)
        {
            var createdImages = new List<Image>(images.Count);
            for (int i = 0; i < images.Count; i++)
            {
                string uniqueFileName = UploadFile(images[i]);

                Image createdImage = new Image
                {
                    CarId = id,
                    FileName = uniqueFileName
                };
                _context.Images.Add(createdImage);
                await _context.SaveChangesAsync();

                createdImages.Add(createdImage);
            }

            return createdImages;
        }

        private string UploadFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
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

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public async Task<JsonResult> GetModels(int? id)
        {
            if (id == null)
            {
                return new JsonResult(BadRequest());
            }
            var models = await _context.Models.Where(m => m.MakeId == id).ToListAsync();

            return new JsonResult(Ok(JsonSerializer.Serialize(models)));
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
