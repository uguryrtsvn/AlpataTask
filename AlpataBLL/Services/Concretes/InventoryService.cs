using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.Base;
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Concretes
{
    public class InventoryService : BaseService<Inventory>, IInventoryService
    {
        readonly IInventoryRepository IInventoryService;
        public InventoryService(IInventoryRepository entityRepository) : base(entityRepository)
        {
            IInventoryService = entityRepository;
        }
    }
}
