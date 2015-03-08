using System;
using System.Collections.Generic;
using SpotifyExam.Core.Model;

namespace SpotifyExam.Core {
	public interface ISpotifyClient {
		bool Authenticate(string token, TokenType type);
		Uri GenerateRedirectURL(IEnumerable<string> scopes, bool showDialog);
		UserInfo GetCurrentUserInfo();
		SpotifyCollection<PlaylistTrack> GetPlaylistTracks(PlaylistTrackInfo playlistTrack);
		SpotifyCollection<Playlist> GetUserPlaylistCollection(string userId);

		AuthenticationData AuthenticationData { get; }

	}
}