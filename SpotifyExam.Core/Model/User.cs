using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpotifyExam.Core.Model {
	public class UserInfo : BaseSpotifyObject {

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[JsonProperty("images")]
		public List<UserImage> Images { get; set; }
	}
}