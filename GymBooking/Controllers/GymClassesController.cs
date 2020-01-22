using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBooking.Core.Models;
using GymBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GymBooking.Core.ViewModels;
using GymBooking.Core;

namespace GymBooking.Controllers
{
    [Authorize]
    public class GymClassesController : Controller 
    {
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public GymClassesController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {

            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index(IndexViewModel vm = null)
        {
            var model = new IndexViewModel();

            if (!User.Identity.IsAuthenticated)
            {
                model.GymClasses = await unitOfWork.GymClasses.GetAllAsync();
                return View(model);
            }
            if (vm.History)
            {
                List<GymClass> gym = await unitOfWork.GymClasses.GetHistoryAsync();
                model = new IndexViewModel { GymClasses = gym };
                return View(model);
            }
            List<GymClass> gymClasses = await unitOfWork.GymClasses.GetAllWithUsersAsync();
            model = new IndexViewModel { GymClasses = gymClasses };
            return View(model);
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetBookings()
        {
            var userId = userManager.GetUserId(User);
            List<GymClass> bookings = await unitOfWork.UserGymClasses.GetAllBookingsAsync(userId);
            return View(bookings);
        }

        // Booking
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null) return NotFound();

            //Hämta den inloggade användarens id
            var userId = userManager.GetUserId(User);

            //Hämta aktuellt gympass
            GymClass currentGymClass = await unitOfWork.GymClasses.GetWithMembersAsync(id);

            //är den aktuella användaren bokad på passet?
            var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == userId);

            //Om inte, boka användaren på passet
            if(attending == null)
            {
                var book = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = currentGymClass.Id
                };
                unitOfWork.UserGymClasses.Add(book);
                await unitOfWork.CompleteAsync();
            }

            //Annars avboka
            else
            {
                unitOfWork.UserGymClasses.Remove(attending);
                await unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        

        // GET: GymClasses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymClass gymClass = await unitOfWork.GymClasses.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return PartialView("CreatePartial");
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.GymClasses.Add(gymClass);
                await unitOfWork.CompleteAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    ////Get all
                    //var gymClasses = await.unityOfWork.GymClasses.GetAllWithUsersAsync();
                    //var vm = new IndexViewModel {GymClasses = gymClasses };
                    //return PartialView("GymClassesPartial", vm);

                    ////Get one
                    var model = await unitOfWork.GymClasses.GetWithAttendingMembersAsync(gymClass.Id);
                    return PartialView(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }

            return View(gymClass);
            
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.GymClasses.Update(gymClass);
                    await unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            unitOfWork.GymClasses.Remove(gymClass);
            await unitOfWork.CompleteAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return unitOfWork.GymClasses.GetAny(id);
        }
    }
}
