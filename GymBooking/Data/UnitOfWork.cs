using GymBooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _context { get; }
        public IGymClassesRepository GymClasses { get; private set; }
        public IApplicationUserGymClassesRepository UserGymClasses { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            GymClasses = new GymClassesRepository(_context);
            UserGymClasses = new ApplicationUserGymClassesRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
