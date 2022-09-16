using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCarHub.Models;

namespace TheCarHub.Services.FileManager
{
    public interface IFileManager
    {
        void DeleteFile(string fileName);
        Task<List<Image>> NewImages(int id, List<IFormFile> images);
    }
}