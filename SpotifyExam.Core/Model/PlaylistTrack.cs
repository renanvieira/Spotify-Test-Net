using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core.Model {
	/// <summary>
	/// Classe que representa o objeto que encapsula os dados uma música na API do Spotify.
	/// </summary>
	public class PlaylistTrack : BaseSpotifyObject {

		public PlaylistTrack() {

		}

		/// <summary>
		/// Dados da música.
		/// </summary>
		public Track Track { get; set; }

	}
}
