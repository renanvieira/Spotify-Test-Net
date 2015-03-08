using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core.Model {

	/// <summary>
	/// Classe que representa o objeto de paginação da API do Spotify.
	/// </summary>
	/// <typeparam name="T">Tipo da coleção de objetos que está sendo paginado.</typeparam>
	public class SpotifyCollection<T> : BaseSpotifyObject {

		public SpotifyCollection() { }

		/// <summary>
		/// URL para retornar o request completo.
		/// </summary>
		public string Href { get; set; }

		/// <summary>
		/// Coleção de objetos que está sendo paginado.
		/// </summary>
		public List<T> Items { get; set; }

		/// <summary>
		/// Total de objetos.
		/// </summary>
		public int Total { get; set; }


	}
}
