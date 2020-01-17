using GymBooking.Data;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public interface IUnitOfWork
    {
        
        IApplicationUserGymClassesRepository AppUserGymRepo { get; }
        IGymClassesRepository GymRepo { get; }

        Task CompleteAsync();
    }
}