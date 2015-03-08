using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core.Model {
	public abstract class BaseSpotifyObject {

		public BaseSpotifyObject() { }

		/// <summary>
		/// Descrição curta do erro.
		/// </summary>
		[JsonProperty("error")]
		public string Error { get; set; }

		/// <summary>
		/// Descrição mais detalhada do erro.
		/// </summary>
		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }

		/// <summary>
		/// Indica se ocorreu algum erro.
		/// </summary>
		public bool HasError {
			get {

				if (string.IsNullOrWhiteSpace(this.Error) == false || string.IsNullOrWhiteSpace(this.ErrorDescription) == false) {
					return true;
				}

				return false;
			}
		}
	}
}
