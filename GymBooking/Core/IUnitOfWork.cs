using GymBooking.Data;
using System.Threading.Tasks;

namespace GymBooking.Core
{
    public interface IUnitOfWork
    {
        
        IApplicationUserGymClassesRepository UserGymClasses { get; }
        IGymClassesRepository GymClasses { get; }

        Task CompleteAsync();
    }
}