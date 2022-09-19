using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCarHub.Models;

namespace TheCarHub.Services.FileManager
{
    public interface IFileManager
    {
        Task<List<Image>> NewImages(int id, ICollection<IFormFile> images);
        void RemoveImages(ICollection<Image> images);
    }
}