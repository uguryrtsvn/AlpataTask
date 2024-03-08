using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Services.EmailService;
using AlpataEntities.Dtos.InventoryDtos;
using AlpataUI.Helpers.ClientHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AlpataUI.Helpers.FileUploadHelper
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAlpataClient _alpataClient;

        public FileUploadService(IWebHostEnvironment webHostEnvironment, IEmailService emailService, IAlpataClient alpataClient)
        {
            _webHostEnvironment = webHostEnvironment; 
            _alpataClient = alpataClient;
        }

        public async Task<IDataResult<bool>> SaveFileAsync(InventoryDto file, FileStorageLocation storageLocation)
        {
            if (file == null || file.FormFile.Length == 0)
            { 
                return new ErrorDataResult<bool>(false, "Dosya yok veya boş.");
            }

            switch (storageLocation)
            {
                case FileStorageLocation.Local:
                    return await SaveFileToLocalAsync(file.FormFile);
                case FileStorageLocation.Database:
                    return await SaveFileToDatabaseAsync(file);
                default:
                    return new ErrorDataResult<bool>(false, "Geçersiz depolama konumu."); 
            }
        }

        private async Task<IDataResult<bool>> SaveFileToLocalAsync(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
            {
                return new ErrorDataResult<bool>(false, "Sadece JPEG, JPG ve PNG dosyaları kabul edilir.");
            }
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Dassets", "images", "users");
            var uniqueFileName = string.Empty;
            try
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                };
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<bool>(false, ex.Message);
            } 
            return new SuccessDataResult<bool>(true,uniqueFileName);
        }

        private async Task<IDataResult<bool>> SaveFileToDatabaseAsync(InventoryDto dto)
        { 
            return new SuccessDataResult<bool>(true,"");
        }

        public async Task<IDataResult<bool>> DeleteFileAsync(string fileName, FileStorageLocation storageLocation = FileStorageLocation.Local)
        { 
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Dassets", "images", "users");
            var filePath = Path.Combine(uploadsFolder, fileName);
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
               return await Task.FromResult(new SuccessDataResult<bool>(true));
            }
            catch (Exception)
            {
                return await Task.FromResult(new ErrorDataResult<bool>(false));
            } 
        }
    }

}
