using Newtonsoft.Json;

namespace SpotifyExam.Core.Model {

	/// <summary>
	/// Classe que representa uma imagem do usuário do Spotify.
	/// </summary>
	public class UserImage {

		public UserImage() {}

		/// <summary>
		/// URL da imagem.
		/// </summary>
		[JsonProperty("url")]
		public string Url { get; set; }

	}
}