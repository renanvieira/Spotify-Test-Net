using Spotify_Exam.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace SpotifyExam.Tests.Web {
	public class NotAuthenticatedHomeControllerTests {

		[Fact]
		public void IndexTest() {

			HomeController controller = new HomeController();

			var result = controller.Index() as ViewResult;

			Assert.True(string.IsNullOrWhiteSpace(result.ViewBag.SpotifyUrl) == false);
			Assert.True(controller.User.Identity.IsAuthenticated == false);

		}

	}
}
