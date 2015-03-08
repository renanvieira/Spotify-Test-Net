using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core.Model {
	public class PlaylistTrack : BaseSpotifyObject {

		public PlaylistTrack() {

		}

		public Track Track { get; set; }

	}
}
