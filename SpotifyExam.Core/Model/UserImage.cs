using Newtonsoft.Json;

namespace SpotifyExam.Core.Model {

	public class UserImage {

		public UserImage() {}

		[JsonProperty("url")]
		public string Url { get; set; }

	}
}