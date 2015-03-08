using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core.Model {
	/// <summary>
	/// Classe que armazena os dados de autenticação da API do Spotify.
	/// </summary>
	public class AuthenticationData : BaseSpotifyObject {

		public AuthenticationData() {
			this.CreationDate = DateTime.Now;
		}

		/// <summary>
		/// Token de Acesso.
		/// </summary>
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		/// <summary>
		/// Tipo do Token.
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		/// <summary>
		/// Tempo, em segundos, em que o token irá expirar.
		/// </summary>
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		/// <summary>
		/// Token para atualização do token de acesso após a expiração.
		/// </summary>
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		/// <summary>
		/// Data de criação do token.
		/// </summary>
		public DateTime CreationDate { get; private set; }

		/// <summary>
		/// Método que verfica se o AccessToken gerado pela API do Spotify expirou.
		/// </summary>
		/// <returns>TRUE caso tenha expiradoo, FALSE caso o token ainda seja valido.</returns>
		public bool IsTokenExpired() {

			DateTime tokenExpirationDate = this.CreationDate.AddSeconds(this.ExpiresIn);

			if (DateTime.Now > tokenExpirationDate) {
				return true;
			}

			return false;
		}
	}
}
