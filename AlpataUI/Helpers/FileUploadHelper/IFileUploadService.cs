namespace AlpataUI.Helpers.FileUploadHelper
{
    public interface IFileUploadService
    {
        Task<string> SaveFileAsync(IFormFile file, FileStorageLocation storageLocation);
        Task<bool> DeleteFileAsync(string fileName, FileStorageLocation storageLocation = FileStorageLocation.Local);

    }
}
