using Spotify_Exam.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit;
using NUnit.Framework;

namespace SpotifyExam.Tests.Web {
	[TestFixture]
    public class NotAuthenticatedHomeControllerTests {

		[Test]
		public void IndexTest() {

			HomeController controller = new HomeController();

			var result = controller.Index() as ViewResult;

			Assert.True(string.IsNullOrWhiteSpace(result.ViewBag.SpotifyUrl) == false);
			Assert.True(controller.User.Identity.IsAuthenticated == false);

		}

	}
}
