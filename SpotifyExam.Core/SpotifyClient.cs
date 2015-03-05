using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using SpotifyExam.Core.Model;

namespace SpotifyExam.Core {

	public class SpotifyClient {

		/// <summary>
		/// Client ID da aplicação cadastrada no Spotify.
		/// </summary>
		public string ClientId { get; private set; }

		/// <summary>
		/// Secret da aplicação  gerada no portal de desenvolvedor do Spotify.
		/// </summary>
		private string Secret { get; set; }

		/// <summary>
		/// URL base da API de autenticação.
		/// </summary>
		private const string BASE_AUTH_URL = "https://accounts.spotify.com";

		/// <summary>
		/// URL base da API.
		/// </summary>
		private const string BASE_API_URL = "https://api.spotify.com";

		public IRestClient AuthClient { get; private set; }

		public IRestClient APIClient { get; private set; }

		public string AuthorizationToken { get; private set; }

		public Uri CallbackUrl { get; private set; }

		public AuthenticationData AuthenticationData { get; private set; }

		public SpotifyClient(string clientId, string clientSecret, Uri redirectUri) {

			this.ClientId = clientId;
			this.Secret = clientSecret;
			this.CallbackUrl = redirectUri;

			this.AuthClient = new RestClient(BASE_AUTH_URL);
			this.APIClient = new RestClient(BASE_API_URL);

		}

		#region Public Authentication Methods

		public Uri GenerateRedirectURL(string scope, bool showDialog) {

			string formatString = "{0}/authorize/?client_id={1}&response_type=code&redirect_uri={2}&scope={3}&show_dialog={4}";

			string url = string.Format(formatString, BASE_AUTH_URL, this.ClientId, this.CallbackUrl.ToString(), scope, showDialog.ToString());

			return new Uri(url);
		}

		public bool Authenticate(string token, TokenType type) {

			RestRequest request = new RestRequest("/api/token", Method.POST);

			string authorizationString = string.Format("{0}:{1}", this.ClientId, this.Secret);

			byte[] bytePayload = Encoding.UTF8.GetBytes(authorizationString);

			request.AddHeader("Authorization", string.Format("Basic {0}", Convert.ToBase64String(bytePayload)));


			switch (type) {

				case TokenType.Authorization:
					this.AuthorizationToken = token;
					return this.GetAccessToken(token, request);

				case TokenType.Refresh:
					return this.RefreshAccessToken(token, request);

				case TokenType.Undefined:
				default:
					throw new ArgumentException("Argumento invalido.", "type");

			}

		}

		#endregion

		public UserInfo GetCurrentUserInfo() {

			IRestRequest request = new RestRequest("/v1/me", Method.GET);

			UserInfo userInfo = this.DoAPIRequest<UserInfo>(request);

			return userInfo;
		}

		public SpotifyCollection<Playlist> GetUserPlaylistCollection(string userId) {

			IRestRequest request = new RestRequest(string.Format("/v1/users/{0}/playlists", userId), Method.GET);

			SpotifyCollection<Playlist> response = this.DoAPIRequest<SpotifyCollection<Playlist>>(request);

			return null;
		}

		#region Private Methods

		private T DoAPIRequest<T>(IRestRequest request) where T : BaseSpotifyObject {

			// Verifica se o client está autenticado, caso nao esteja tenta autenticar. 
			if (this.AuthenticationData == null || string.IsNullOrWhiteSpace(this.AuthenticationData.AccessToken) == true) {
				throw new InvalidOperationException("Dados de autenticação inválidos. Refaça o fluxo de autorização.");
			}

			if (this.AuthenticationData.IsTokenExpired() == true) {

				if (this.Authenticate(this.AuthenticationData.RefreshToken, TokenType.Refresh) == false) {
					throw new InvalidOperationException("O 'AccessToken' está expirado e a tentativa de atualiza-lo falhou.");
				}

			}

			request.AddHeader("Authorization", string.Format("Bearer {0}", this.AuthenticationData.AccessToken));

			IRestResponse response = this.APIClient.Execute(request);

			T responseSerialized = JsonConvert.DeserializeObject<T>(response.Content);

			return responseSerialized;
		}

		private bool GetAccessToken(string authToken, IRestRequest request) {
			
			request.AddParameter("grant_type", "authorization_code");
			request.AddParameter("code", authToken);
			request.AddParameter("redirect_uri", this.CallbackUrl.ToString());

			IRestResponse response = this.AuthClient.Execute(request);

			bool isAuthenticated = false;

			this.AuthenticationData = JsonConvert.DeserializeObject<AuthenticationData>(response.Content);

			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				isAuthenticated = true;
			}

			return isAuthenticated;

		}

		private bool RefreshAccessToken(string refreshToken, IRestRequest request) {

			if (this.AuthenticationData == null || string.IsNullOrWhiteSpace(this.AuthenticationData.RefreshToken) == true) {
				throw new InvalidOperationException("Os dados de autenticação não são validos para a atualização do token de acesso.");
			}

			request.AddParameter("grant_type", "refresh_token");
			request.AddParameter("refresh_token", refreshToken);

			IRestResponse response = this.AuthClient.Execute(request);

			bool isAuthenticated = false;

			this.AuthenticationData = JsonConvert.DeserializeObject<AuthenticationData>(response.Content);

			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				isAuthenticated = true;
			}

			return isAuthenticated;
		}


		#endregion

	}
}
