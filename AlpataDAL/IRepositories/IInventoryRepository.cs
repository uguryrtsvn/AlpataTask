using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.IRepositories
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        public Task<int> GetMeetingInventoryCount(Guid meetId);
    }
}
