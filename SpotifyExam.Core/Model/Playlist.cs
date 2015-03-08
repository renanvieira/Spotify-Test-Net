using System;
using System.Linq;
using System.Collections.Generic;

namespace SpotifyExam.Core.Model {
	/// <summary>
	/// Objeto que representa os dados de uma playlist do Spotify.
	/// </summary>
	public class Playlist : BaseSpotifyObject {

		/// <summary>
		/// Nome da playlist
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Informação de como acessar as músicas da playlist
		/// </summary>
		public PlaylistTrackInfo Tracks { get; set; }

		/// <summary>
		/// Método que monta um playlist usando as letras do nome do usuário logado.
		/// </summary>
		/// <param name="userInfo">Informações de usuário.</param>
		/// <param name="allTracksCollection">Coleção com as músicas disponíveis para montar a playlist.</param>
		/// <returns>
		///		Uma coleção contendo uma <see cref="Tuple{T1, T2}"/> com a letra e a instancia de <see cref="Track"/> com os dados da música.
		/// </returns>
		public static ICollection<Tuple<char, Track>> GeneratePlaylistUsingUserName(UserInfo userInfo, List<Track> allTracksCollection) {

			if (userInfo == null) { throw new ArgumentNullException(nameof(userInfo)); }
			
			if (allTracksCollection == null || allTracksCollection.Count <= 0) { throw new ArgumentNullException(nameof(allTracksCollection)); }

			char[] letterArray = userInfo.DisplayName.ToUpper().ToCharArray();

			ICollection<Tuple<char, Track>> playlist = new List<Tuple<char, Track>>();

			Track nullTrack = new Track("Null and Void", "Detroit");

			foreach (char letter in letterArray) {

				// Verifica se o caracter é um número, letra ou espaço vazio, caso seja 
				// adiciona uma música com os dados vazios e pula para o proximo caracter.
				if (char.IsWhiteSpace(letter) || char.IsLetterOrDigit(letter) == false) {
					playlist.Add(new Tuple<char, Track>(letter, null));
					continue;
				}

				// Retorna todas as músicas que iniciem com a letra.
				IEnumerable<Track> trackCollection = allTracksCollection.Where(x => x.Name.First<char>() == letter);

				// Cria uma lista sem todas os objetos nulos da playlist.
				var nonNullTracks = playlist.Where(x => x.Item2 != null);

				// Busca por todas as músicas que ainda não foram adicionadas na playlist final e 
				// retorna a primeira (ou nula, caso nao encontre nenhuma).
				Track track = trackCollection.
									Where(y => nonNullTracks.Any(x => x.Item2.Name == y.Name) == false).
									FirstOrDefault();


				if (track == null) {
					playlist.Add(new Tuple<char, Track>(letter, nullTrack));
				}
				else {
					playlist.Add(new Tuple<char, Track>(letter, track));
				}

			}

			return playlist;


		}
	}
}