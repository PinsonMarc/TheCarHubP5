using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cars.Include(c => c.Model).ThenInclude(m => m.Make).ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars
                .Include(c => c.Model).ThenInclude(m => m.Make)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null) return NotFound();

            return View(car);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["Makes"]  = new SelectList(_context.Makes.ToList(), "Id", "Name");

            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFiles,VIN,Year,ModelId,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SaleDate,Status")] CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                var images = await NewImages(_context.Cars.Max(c => c.Id), carViewModel.ImageFiles);

                Car car = _mapper.Map<CarViewModel, Car>(carViewModel);
                car.Images = images;

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carViewModel);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars.Where(c => c.Id == id).Include(c => c.Model).ThenInclude(m => m.Make).FirstOrDefaultAsync();
            if (car == null) return NotFound();

            ViewData["Makes"] = new SelectList (_context.Makes.ToList(), "Id", "Name", car.Model.Make);
            ViewData["Models"] = new SelectList(_context.Models.Where(m => m.MakeId == car.Model.MakeId).ToList(), "Id", "Name", car.Model);
            CarViewModel carViewModel = _mapper.Map<Car, CarViewModel>(car);
            return View(carViewModel);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageFiles,VIN,Year,ModelId,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SaleDate,Status")] CarViewModel carViewModel)
        {
            if (id != carViewModel.Id) carViewModel.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    Car car = _mapper.Map<CarViewModel, Car>(carViewModel);
                    var images = await NewImages(_context.Cars.Max(c => c.Id), carViewModel.ImageFiles);
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(carViewModel.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carViewModel);
        }

        private async Task<List<Image>> NewImages(int id, List<IFormFile> images)
        {
            if (images == null) return null;
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

                createdImages.Add(createdImage);
            }
            await _context.SaveChangesAsync();

            return createdImages;
        }

        private string UploadFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = GetFullPath(uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteFile(string fileName)
        {
            string filePath = GetFullPath(fileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
        }

        private string GetFullPath(string fileName)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            return Path.Combine(uploadsFolder, fileName);
        }


        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null) return NotFound();

            return View(car);
        }

        public async Task<JsonResult> GetModels(int? id)
        {
            if (id == null) return new JsonResult(BadRequest());

            var models = await _context.Models.Where(m => m.MakeId == id).ToListAsync();

            return new JsonResult(Ok(JsonSerializer.Serialize(models)));
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.Include(c => c.Images).Where(c => c.Id == id).FirstAsync();

            foreach (Image image in car.Images)
            {
                _context.Images.Remove(image);
                DeleteFile(image.FileName);
            }

            _context.Cars.Remove(car);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
            =>  _context.Cars.Any(e => e.Id == id);
    }
}
