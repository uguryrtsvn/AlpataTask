using AlpataBLL.Services.Abstracts;
using AlpataEntities.Dtos.InventoryDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    public class InventoryController : BaseController
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        [HttpPost]
        public async Task<IActionResult> AddInventory(InventoryDto dto)
        {
            var result = await _inventoryService.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }       
        [HttpGet]
        public async Task<IActionResult> GetInventory(string Id)
        {
            var result = await _inventoryService.GetAsync(z=>z.Id == Guid.Parse(Id));
            return result.Success ? Ok(result) : BadRequest(result);
        }    
        [HttpGet]
        public async Task<IActionResult> DeleteInventory(string Id)
        {
            var i = await _inventoryService.GetAsync(z => z.Id == Guid.Parse(Id));
            var result = await _inventoryService.DeleteAsync(i.Data);;
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
