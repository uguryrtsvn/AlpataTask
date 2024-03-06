using AlpataDAL; 
using AlpataDAL.IRepositories;
using AlpataDAL.Repositories;
using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.Repositories
{
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        AlpataDbContext _db;
        public AppUserRepository(AlpataDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
