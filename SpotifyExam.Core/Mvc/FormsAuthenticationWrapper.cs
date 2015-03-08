using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace SpotifyExam.Core.Mvc {
	public class FormsAuthenticationWrapper : IAuthenticationProvider {

		public void SignOut() {
			FormsAuthentication.SignOut();
		}
	}
}
