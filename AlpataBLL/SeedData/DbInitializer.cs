using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.SeedData
{
    public class DbInitializer
    {
        private readonly AlpataDbContext _context;

        public DbInitializer(AlpataDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            var existCheck =_context.Database.CanConnect();
            if (!existCheck) {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
                _context.Seed();
            } 
            
        }
    }
}
