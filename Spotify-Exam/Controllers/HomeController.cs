using SpotifyExam.Core;
using SpotifyExam.Core.Model;
using SpotifyExam.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Spotify_Exam.Controllers {
	public class HomeController : BaseController {

		public HomeController() {

		}

		public ActionResult Index() {

			if (this.User.Identity.IsAuthenticated && this.Session["UserInfo"] != null) {

				var userInfo = this.Session["UserInfo"] as UserInfo;

				ViewBag.UserInfo = userInfo;

				if (userInfo.Images != null && userInfo.Images.Count > 0) {
					ViewBag.UserImage = userInfo.Images[0].Url;
				}

				SpotifyCollection<Playlist> playlists = this.SpotifyClient.GetUserPlaylistCollection(userInfo.Id);

				List<Track> allTracksCollection = new List<Track>();

				foreach (var item in playlists.Items) {
					SpotifyCollection<PlaylistTrack> tracks = this.SpotifyClient.GetPlaylistTracks(item.Tracks);

					if (tracks.HasError == false) {
						allTracksCollection.AddRange(tracks.Items.Select(x => x.Track));
					}

				}

				ICollection<Tuple<char, Track>> finalPlaylist = Playlist.GeneratePlaylistUsingUserName(userInfo, allTracksCollection);

				ViewBag.Playlist = finalPlaylist;
			}
			else {
				FormsAuthentication.SignOut();
				ViewBag.SpotifyUrl = this.SpotifyClient.GenerateRedirectURL("playlist-read-private", false);
			}

			return View();
		}

	}
}