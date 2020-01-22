using GymBooking.Core.Models;
using GymBooking.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public class GymClassesRepository : IGymClassesRepository
    {
        private ApplicationDbContext _context { get; }

        public GymClassesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool GetAny(int id)
        {
            return _context.GymClasses.Any(e => e.Id == id);
        }

        public async Task<GymClass> GetWithAttendingMembersAsync(int? id)
        {
            return await _context.GymClasses
                .Include(a => a.AttendingMembers)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<GymClass> GetAsync(int? id)
        {
            return await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<GymClass>> GetAllWithUsersAsync()
        {
            return await _context.GymClasses
                .Include(g => g.AttendingMembers)
                .ThenInclude(a => a.ApplicationUser)
                .ToListAsync();
        }

        public async Task<List<GymClass>> GetHistoryAsync()
        {
            return await _context.GymClasses
                .Include(g => g.AttendingMembers)
                .ThenInclude(a => a.ApplicationUser)
                .IgnoreQueryFilters()
                .Where(g => g.StartTime < DateTime.Now)
                .ToListAsync();
        }

        public void Add(GymClass gymClass)
        {
            _context.Add(gymClass);
        }

        public void Remove(GymClass gymClass)
        {
            _context.Remove(gymClass);
        }

        public void Update(GymClass gymClass)
        {
            _context.Update(gymClass);
        }

        public async Task<IEnumerable<GymClass>> GetAllAsync()
        {
            return await _context.GymClasses.ToListAsync();
        }
    }
}
