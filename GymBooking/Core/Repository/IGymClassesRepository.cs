using GymBooking.Core.Models;
using GymBooking.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public interface IGymClassesRepository
    {
       

        void Add(GymClass gymClass);
        Task<List<GymClass>> GetAllWithUsersAsync();
        bool GetAny(int id);
        Task<GymClass> GetAsync(int? id);
        Task<List<GymClass>> GetHistoryAsync();
        Task<GymClass> GetWithAttendingMembersAsync(int? id);
        void Remove(GymClass gymClass);
        void Update(GymClass gymClass);
        Task<IEnumerable<GymClass>> GetAllAsync();
    }
}