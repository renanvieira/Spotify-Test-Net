namespace SpotifyExam.Core.Model {

	/// <summary>
	/// Objeto que representa a informação de como acessar as músicas de uma playlist do Spotify.
	/// </summary>
	public class PlaylistTrackInfo : BaseSpotifyObject {

		/// <summary>
		/// URL da API do Spotify para acessar a lista de músicas da playlist
		/// </summary>
		public string Href { get; set; }

		/// <summary>
		/// Total de músicas da playlist.
		/// </summary>
		public int Total { get; set; }

	}
}