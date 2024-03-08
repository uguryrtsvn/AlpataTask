using AlpataBLL.BaseResult.Abstracts;
using AlpataEntities.Dtos.InventoryDtos;

namespace AlpataUI.Helpers.FileUploadHelper
{
    public interface IFileUploadService
    {
        Task<IDataResult<bool>> SaveFileAsync(InventoryDto file, FileStorageLocation storageLocation);
        Task<IDataResult<bool>> DeleteFileAsync(string fileName, FileStorageLocation storageLocation = FileStorageLocation.Local);

    }
}
