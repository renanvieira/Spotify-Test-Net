using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpotifyExam.Core.Model {
	/// <summary>
	/// Classe que representa os dados de usuário do Spotify.
	/// </summary>
	public class UserInfo : BaseSpotifyObject {

		/// <summary>
		/// ID do usuário no Spotify.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Nome do usuário.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Nome que aparece no perfil do usuário.
		/// </summary>
		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Lista de imagens do perfil do usuário.
		/// </summary>
		[JsonProperty("images")]
		public List<UserImage> Images { get; set; }
	}
}