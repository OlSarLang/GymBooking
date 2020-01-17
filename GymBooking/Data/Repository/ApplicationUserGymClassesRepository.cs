using GymBooking.Core.Models;
using GymBooking.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public class ApplicationUserGymClassesRepository : IApplicationUserGymClassesRepository
    {
        private ApplicationDbContext _context { get; }

        public ApplicationUserGymClassesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ApplicationUserGymClass book)
        {
            _context.ApplicationUserGymClasses.Add(book);
        }

        public void Remove(ApplicationUserGymClass attending)
        {
            _context.ApplicationUserGymClasses.Remove(attending);

        }

        public async Task<List<GymClass>> GetAllBookingsAsync(string userId)
        {
            return await _context.ApplicationUserGymClasses
                .Where(ag => ag.ApplicationUserId == userId)
                .IgnoreQueryFilters()
                .Select(ag => ag.GymClass)
                .ToListAsync();
        }

    }
}
