using AlpataBLL.BaseResult.Abstracts;
using AlpataUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlpataUI.Helpers.FileManagerHelper
{
    public interface IFileManager
    {
        Task<IDataResult<bool>> SaveFileAsync(FileDto file, FileStorageLocation storageLocation);
        Task<IDataResult<bool>> DeleteFileAsync(string fileName, FileStorageLocation storageLocation = FileStorageLocation.Local);  
    }
}
