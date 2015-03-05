using SpotifyExam.Core;
using SpotifyExam.Core.Model;
using SpotifyExam.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Spotify_Exam.Controllers {
	public class HomeController : BaseController {

		public HomeController() {

		}

		public ActionResult Index() {


			ViewBag.SpotifyUrl = this.SpotifyClient.GenerateRedirectURL("playlist-read-private", false);

			if (this.User.Identity.IsAuthenticated && this.Session["UserInfo"] != null) {

				var userInfo = this.Session["UserInfo"] as UserInfo;

				ViewBag.UserInfo = userInfo;

				if (userInfo.Images != null && userInfo.Images.Count > 0) {
					ViewBag.UserImage = userInfo.Images[0].Url;
				}

			}
			else {
				FormsAuthentication.SignOut();
			}

			return View();
		}

	}
}