using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core {

	public class SpotifyClient {

		public string ClientId { get; private set; }
		private string Secret { get; set; }

		public SpotifyClient(string clientId, string clientSecret) {
			this.ClientId = clientId;
			this.Secret = clientSecret;
		}


	}
}
