using Newtonsoft.Json;

namespace SpotifyExam.Core.Model {
	public class UserInfo : BaseSpotifyObject {

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

	}
}