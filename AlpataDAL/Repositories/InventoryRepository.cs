using Microsoft.EntityFrameworkCore;
using AlpataEntities.Entities.Concretes;
using AlpataDAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AlpataDAL.Repositories
{
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        readonly AlpataDbContext _db;
        public InventoryRepository(AlpataDbContext db, IMapper mapper) : base(db, mapper)
        {
            _db = db;
        }

        public async Task<int> GetMeetingInventoryCount(Guid meetId)
        {
            return await _db.Inventories.Where(z => z.MeetingId == meetId).CountAsync();

        }
    }
}
