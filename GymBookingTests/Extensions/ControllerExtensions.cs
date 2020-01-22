using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingTests.Extensions
{
    public static class ControllerExtensions
    {
        public static void SetUserIsAuthenticated(this Controller controller, bool isAuthenticated)
        {
            var mockContext = new Mock<HttpContext>(MockBehavior.Default);
            mockContext.SetupGet(httpCon => httpCon.User.Identity.IsAuthenticated).Returns(isAuthenticated);
            controller.ControllerContext = new ControllerContext { HttpContext = mockContext.Object };
        }

        public static void SetAjaxRequest(this Controller controller, bool isAjax)
        {
            var mockContext = new Mock<HttpContext>(MockBehavior.Default);

            if (isAjax)
            {
                mockContext.SetupGet(h => h.Request.Headers["X-Requested-With"]).Returns("XMLHttpRequest");
            }
            else
            {
                mockContext.SetupGet(h => h.Request.Headers["X-Requested-With"]).Returns("");
            }

            controller.ControllerContext = new ControllerContext { HttpContext = mockContext.Object };
        }
    }
}
