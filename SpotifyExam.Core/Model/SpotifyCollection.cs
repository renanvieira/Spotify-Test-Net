using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core.Model {
	public class SpotifyCollection<T> : BaseSpotifyObject {

		public SpotifyCollection() { }

		public string Href { get; set; }

		public List<T> Items { get; set; }

		public int Total { get; set; }


	}
}
