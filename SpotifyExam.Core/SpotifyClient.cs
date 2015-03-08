using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using SpotifyExam.Core.Model;

namespace SpotifyExam.Core {

	/// <summary>
	/// Client para acesso a API do Spotify.
	/// </summary>
	public class SpotifyClient : ISpotifyClient {

		/// <summary>
		/// URL base da API de autenticação.
		/// </summary>
		private const string BASE_AUTH_URL = "https://accounts.spotify.com";

		/// <summary>
		/// URL base da API.
		/// </summary>
		private const string BASE_API_URL = "https://api.spotify.com";

		/// <summary>
		/// Inicializa o client com os dados necessários para realizar o acesso a API.
		/// </summary>
		/// <param name="clientId">ID do app cadastrado no portal de desenvolvedor do Spotify.</param>
		/// <param name="clientSecret">Secret gerado após o cadsatro do app no portal de desenvolvedor do Spofity.</param>
		/// <param name="redirectUri">URL que será usado como callback no processo de autenticação.</param>
		public SpotifyClient(string clientId, string clientSecret, Uri redirectUri) {

			this.ClientId = clientId;
			this.Secret = clientSecret;
			this.CallbackUrl = redirectUri;

			this.AuthClient = new RestClient(BASE_AUTH_URL);
			this.APIClient = new RestClient(BASE_API_URL);

		}


		/// <summary>
		/// Client ID da aplicação cadastrada no Spotify.
		/// </summary>
		public string ClientId { get; private set; }

		/// <summary>
		/// Secret da aplicação  gerada no portal de desenvolvedor do Spotify.
		/// </summary>
		private string Secret { get; set; }


		/// <summary>
		/// Client REST para acesso a API de autenticação.
		/// </summary>
		private IRestClient AuthClient { get;  set; }

		/// <summary>
		/// Client REST para acesso a API.
		/// </summary>
		private IRestClient APIClient { get;  set; }

		/// <summary>
		/// Token de autorização.
		/// </summary>
		private string AuthorizationToken { get; set; }

		/// <summary>
		/// URI de callback que será usado na autenticação.
		/// </summary>
		public Uri CallbackUrl { get; private set; }

		/// <summary>
		/// Propriedade que armazena os dados da autenticação para uso nas requisições.
		/// </summary>
		public AuthenticationData AuthenticationData { get; private set; }

		#region Public Authentication Methods

		/// <summary>
		/// Método que gera a URL para onde o usuário será redirecionado para autorizar o app a acessar a sua conta.
		/// </summary>
		/// <param name="scope">Lista de permissões de acesso aos dados de usuário.</param>
		/// <param name="showDialog">Flag que indica se o usuário irá ter que aprovar o app toda vez que utilizar.</param>
		/// <returns></returns>
		public Uri GenerateRedirectURL(IEnumerable<string> scopes, bool showDialog) {

			string formatString = "{0}/authorize/?client_id={1}&response_type=code&redirect_uri={2}&scope={3}&show_dialog={4}";

			string scope = String.Join(" ", scopes.ToArray());

			string url = string.Format(formatString, BASE_AUTH_URL, this.ClientId, this.CallbackUrl.ToString(), scope, showDialog.ToString());

			return new Uri(url);
		}

		/// <summary>
		/// Método que realiza a autenticação de acordo com o tipo e o token informado.
		/// </summary>
		/// <param name="token">Token.</param>
		/// <param name="type">Tipo do token que será utilizado para a autenticação.</param>
		/// <returns></returns>
		public bool Authenticate(string token, TokenType type) {

			RestRequest request = new RestRequest("/api/token", Method.POST);

			string paylod = string.Format("{0}:{1}", this.ClientId, this.Secret);

			byte[] payloadByteArray = Encoding.UTF8.GetBytes(paylod);

			request.AddHeader("Authorization", string.Format("Basic {0}", Convert.ToBase64String(payloadByteArray)));

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

		/// <summary>
		/// Método que retorna os dados do usuário logado.
		/// </summary>
		/// <returns>Instancia de <see cref="UserInfo"/> com os dados do usuário logado no Spotify.</returns>
		public UserInfo GetCurrentUserInfo() {

			IRestRequest request = new RestRequest("/v1/me", Method.GET);

			UserInfo userInfo = this.DoAPIRequest<UserInfo>(request);

			return userInfo;
		}

		/// <summary>
		/// Método que retorna todas as playlists de um usuário.
		/// </summary>
		/// <param name="userId">ID do usuário.</param>
		/// <returns>
		/// </returns>
		public SpotifyCollection<Playlist> GetUserPlaylistCollection(string userId) {

			IRestRequest request = new RestRequest(string.Format("/v1/users/{0}/playlists", userId), Method.GET);

			SpotifyCollection<Playlist> response = this.DoAPIRequest<SpotifyCollection<Playlist>>(request);

			return response;
		}

		/// <summary>
		/// Método que retorna todas as músicas de uma determinada playlist. 
		/// </summary>
		/// <param name="playlistTrack">Instancia do objeto <see cref="PlaylistTrackInfo"/> contendo os dados da playlist. </param>
		/// <returns></returns>
		public SpotifyCollection<PlaylistTrack> GetPlaylistTracks(PlaylistTrackInfo playlistTrack) {

			Uri requestUri = new Uri(playlistTrack.Href);

			IRestRequest request = new RestRequest(requestUri.AbsolutePath, Method.GET);

			SpotifyCollection<PlaylistTrack> response = this.DoAPIRequest<SpotifyCollection<PlaylistTrack>>(request);

			return response;


		}

		#region Private Methods

		/// <summary>
		/// Wrapper para facilitar os requests a API do Spotify.
		/// </summary>
		/// <typeparam name="TResponse">Tipo que será retornado.</typeparam>
		/// <param name="request">Informações do request (URL, verbo, corpo, etc).</param>
		/// <returns></returns>
		private TResponse DoAPIRequest<TResponse>(IRestRequest request) where TResponse : BaseSpotifyObject {

			// Verifica se o client está autenticado, caso nao esteja retorna uma exceção. 
			if (this.AuthenticationData == null || string.IsNullOrWhiteSpace(this.AuthenticationData.AccessToken) == true) {
				throw new InvalidOperationException("Dados de autenticação inválidos. Refaça o fluxo de autorização.");
			}

			// Verifica se o Access Token expirou, caso tenha expirado, tenta dar um refresh no token
			// caso não consiga efetuar o refresh, retorna exception.
			if (this.AuthenticationData.IsTokenExpired() == true) {

				if (this.Authenticate(this.AuthenticationData.RefreshToken, TokenType.Refresh) == false) {
					throw new InvalidOperationException("O 'AccessToken' está expirado e a tentativa de atualiza-lo falhou.");
				}

			}

			// Adiciona o header de autenticação.
			request.AddHeader("Authorization", string.Format("Bearer {0}", this.AuthenticationData.AccessToken));

			IRestResponse response = this.APIClient.Execute(request);

			TResponse responseSerialized = JsonConvert.DeserializeObject<TResponse>(response.Content);

			return responseSerialized;
		}

		/// <summary>
		/// Método que retorna o Access Token utilizando o Token de Autorização.
		/// </summary>
		/// <param name="authToken">Token de Autorização.</param>
		/// <param name="request">Informações do request a API de autenticação.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Método que atualiza o Access token expirado utilizando o RefresToken.
		/// </summary>
		/// <param name="refreshToken">Refresh Token.</param>
		/// <param name="request">Informações do request a API de autenticação.</param>
		/// <returns></returns>
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
