using GymBooking.Controllers;
using GymBooking.Core;
using GymBooking.Core.Models;
using GymBookingTests.Extensions;
using GymBooking.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymBookingTests.Controllers
{
    [TestClass]
    public class GymClassesControllerTests
    {
        private Mock<IGymClassesRepository> repository;
        private Mock<IUnitOfWork> mockUoW;
        private Mock<IUserStore<ApplicationUser>> userStore;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private GymClassesController controller;
        private const int gymClassIdNotExists = 3;

        [TestInitialize]
        public void SetUp()
        {
            repository = new Mock<IGymClassesRepository>();
            mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(u => u.GymClasses).Returns(repository.Object);

            userStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>
                (userStore.Object, null, null, null, null, null, null, null);

            controller = new GymClassesController(mockUserManager.Object, mockUoW.Object);
        }

        

        [TestMethod]
        public async Task Index_ReturnsViewResult_ShouldPass()
        {
            //Arrange
            controller.SetUserIsAuthenticated(true);
            var vm = new IndexViewModel { History = true };

            //Actual
            var actual = await controller.Index(vm);

            //Assert
            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void Index_ReturnsAllGymClasses()
        {
            var classes = GetGymClassList();
            var expected = new IndexViewModel { GymClasses = classes };

            repository.Setup(g => g.GetAllAsync()).ReturnsAsync(classes);
            var vm = new IndexViewModel { History = false };
            controller.SetUserIsAuthenticated(false);

            var viewResult = controller.Index(vm).Result as ViewResult;

            var actual = (IndexViewModel)viewResult.Model;

            Assert.AreEqual(expected.GymClasses, actual.GymClasses);
        }

        [TestMethod]
        public void Details_GetCorrectGymClass()
        {
            var id = 1;
            var expected = GetGymClassList()[0];
            repository.Setup(g => g.GetAsync(expected.Id)).ReturnsAsync(expected);

            var actual = (ViewResult) controller.Details(id).Result;

            Assert.AreEqual(expected, actual.Model);
        }

        [TestMethod]
        public void Details_NoGymClassExistsWithGivenId_ShouldReturnNotFound()
        {
            var result = (StatusCodeResult)controller.Details(gymClassIdNotExists).Result;
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ReturnsDefaultView_ShouldReturnNull()
        {
            controller.SetAjaxRequest(false);
            var result = controller.Create() as ViewResult;
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public void Create_ReturnsCreatePartialWhenAjax()
        {
            const string viewName = "CreatePartial";
            controller.SetAjaxRequest(true);
            
            var result = controller.Create() as PartialViewResult;

            Assert.AreEqual(result.ViewName, viewName);
        }

        private List<GymClass> GetGymClassList()
        {
            return new List<GymClass>
            {
                new GymClass
                {
                    Id = 1,
                    Name = "Spinning",
                    Description = "Hard",
                    StartTime = DateTime.Now.AddDays(3),
                    Duration = new TimeSpan(0, 10, 0)
                },
                new GymClass
                {
                    Id = 1,
                    Name = "Spinning",
                    Description = "Hard",
                    StartTime = DateTime.Now.AddDays(3),
                    Duration = new TimeSpan(0, 10, 0)
                }
            };
        }
    }
}
