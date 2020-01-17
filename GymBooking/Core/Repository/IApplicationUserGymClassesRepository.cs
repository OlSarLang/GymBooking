using GymBooking.Core.Models;
using GymBooking.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public interface IApplicationUserGymClassesRepository
    {
        

        void Add(ApplicationUserGymClass book);
        Task<List<GymClass>> GetAllBookingsAsync(string userId);
        void Remove(ApplicationUserGymClass attending);
    }
}