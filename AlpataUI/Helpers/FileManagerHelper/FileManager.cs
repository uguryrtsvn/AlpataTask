using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes; 
using AlpataEntities.Dtos.InventoryDtos;
using AlpataUI.Helpers.ClientHelper; 
using System.IO.Compression; 
using AlpataUI.Models;
using Microsoft.AspNetCore.Mvc; 

namespace AlpataUI.Helpers.FileManagerHelper
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAlpataClient _alpataClient;

        public FileManager(IWebHostEnvironment webHostEnvironment, IAlpataClient alpataClient)
        {
            _webHostEnvironment = webHostEnvironment;
            _alpataClient = alpataClient;
        }

        public async Task<IDataResult<bool>> SaveFileAsync(FileDto file, FileStorageLocation storageLocation)
        {
            if (file.FormFile == null || file.FormFile.Length == 0)
            {
                return new ErrorDataResult<bool>(false, "Dosya yok veya boş.");
            }
            if (file.FormFile.Length > 10 * 1024 * 1024) 
            {
                return new ErrorDataResult<bool>(false, "Dosya boyutu 10 mb dan büyük olamaz.");
            }
            return storageLocation switch
            {
                FileStorageLocation.Local => await SaveFileToLocalAsync(file.FormFile),
                FileStorageLocation.Database => await SaveFileToDatabaseAsync(file),
                _ => new ErrorDataResult<bool>(false, "Geçersiz depolama konumu.")
            };
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
            return new SuccessDataResult<bool>(true, uniqueFileName);
        }

        private async Task<IDataResult<bool>> SaveFileToDatabaseAsync(FileDto dto)
        {
            try
            {
                var inv = await CompressFile(dto);
                var result = await _alpataClient.Add(inv, "Inventory/AddInventory");
                return result.Success ? new SuccessDataResult<bool>(result.Success, "Dosya eklendi.") : new ErrorDataResult<bool>(false, "Dosya eklenirken hata oluştu.");
            }
            catch (Exception)
            {
                return new ErrorDataResult<bool>(false, "Dosya eklenirken hata oluştu.");
            }
        }
        private async Task<InventoryDto> CompressFile(FileDto dto)
        {
            InventoryDto inv = new InventoryDto();
            inv.FileName = dto.FormFile.FileName;
            inv.MeetingId = dto.MeetingId;
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry(dto.FormFile.FileName, CompressionLevel.Optimal);

                    using (var entryStream = entry.Open())
                    {
                        await dto.FormFile.CopyToAsync(entryStream);
                    }
                }
                inv.FileData = memoryStream.ToArray();
            }
            return inv;
        }
        public async Task<IDataResult<bool>> DeleteFileAsync(string fileName, FileStorageLocation storageLocation = FileStorageLocation.Local)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return new ErrorDataResult<bool>(false, "Lütfen Dosya Belirtin");
            }

            return storageLocation switch
            {
                FileStorageLocation.Local => await DeleteFileFromLocal(fileName),
                FileStorageLocation.Database => await DeleteFileFromDatabase(fileName),
                _ => new ErrorDataResult<bool>(false, "Geçersiz depolama konumu.")
            };

        }

        private Task<IDataResult<bool>> DeleteFileFromDatabase(string fileName)
        {
            throw new NotImplementedException();
        }

        private async Task<IDataResult<bool>> DeleteFileFromLocal(string fileName)
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
