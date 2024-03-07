using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AlpataUI.Helpers.FileUploadHelper
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
         
        public async Task<string> SaveFileAsync(IFormFile file, FileStorageLocation storageLocation)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Dosya yok veya boş.");
            }

            switch (storageLocation)
            {
                case FileStorageLocation.Local:
                    return await SaveFileToLocalAsync(file);
                case FileStorageLocation.Database:
                    return await SaveFileToDatabaseAsync(file);
                default:
                    throw new ArgumentException("Geçersiz depolama konumu.");
            }
        }

        private async Task<string> SaveFileToLocalAsync(IFormFile file)
        { 
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Dassets", "images", "users");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        private async Task<string> SaveFileToDatabaseAsync(IFormFile file)
        {
            return "";
        }

        public async Task<bool> DeleteFileAsync(string fileName, FileStorageLocation storageLocation = FileStorageLocation.Local)
        {
            bool result = false;
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Dassets", "images", "users");
            var filePath = Path.Combine(uploadsFolder, fileName);
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                result = true;
            }
            catch (Exception)
            { 
                result = false;
            }
            return await Task.FromResult(result);
        }
    }

}
