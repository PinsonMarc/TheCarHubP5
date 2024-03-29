﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TheCarHub.Data;
using TheCarHub.Models;

namespace TheCarHub.Services.FileManager
{
    //Manage file inside the application
    [Authorize]
    public class FileManager : IFileManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileManager(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<List<Image>> NewImages(int id, ICollection<IFormFile> images)
        {
            var createdImages = new List<Image>();
            if (images == null) return createdImages;
            foreach (var image in images)
            {
                string uniqueFileName = UploadFile(image);

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

        public void RemoveImages(ICollection<Image>? images)
        {
            if (images == null) return;
            foreach (Image image in images)
            {
                DeleteFile(image.FileName);
            }
        }

        private void DeleteFile(string fileName)
        {
            string filePath = GetFullPath(fileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

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

        private string GetFullPath(string fileName)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            return Path.Combine(uploadsFolder, fileName);
        }
    }
}
