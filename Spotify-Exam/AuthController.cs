using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spotify_Exam {
	public class AuthController : Controller {
		// GET: Auth
		public ActionResult Index() {
			return this.HttpNotFound();
		}
	}
}