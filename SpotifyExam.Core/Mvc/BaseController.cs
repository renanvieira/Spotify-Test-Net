using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyExam.Core.Mvc {
	/// <summary>
	/// Classe base para os Controllers da camada Web.
	/// </summary>
	public class BaseController : Controller {

		public BaseController() {

		}

		/// <summary>
		/// Método que retorna uma instância (Singleton) do client da API do Spotify que está armazenado na sessão.
		/// </summary>
		/// <returns></returns>
		private ISpotifyClient GetClientInstance() {

			if (this.Session["SpotifyClient"] == null) {
				string url = string.Format("{0}://{1}{2}", this.Request.Url.Scheme, this.Request.Url.Authority, this.Url.Content("~/Auth/Callback"));
				this.Session["SpotifyClient"] = new SpotifyClient("980a96fe572f4973a1ab3ffd1a664f17", "eb19ab9b852542c78b7961d89fd59b00", new Uri(url));
			}

			return (ISpotifyClient)this.Session["SpotifyClient"];
		}

		/// <summary>
		/// Proprieade para acesso do client da API.
		/// </summary>
		protected ISpotifyClient SpotifyClient {
			get {
				return this.GetClientInstance();
			}
		}
	}
}
