using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Web.Mvc;
using Rockkola.Models;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;

namespace Rockkola.Controllers
{
    public class RockkolaController : Controller
    {
        // GET: Rockkola
        public ActionResult Index()
        {
            return View();
        }
        public void Declare()
        {
            List<Videos> PLAYLISTVIDEOS = new List<Videos>();
            if(Session["Playlist"] == null)
            {
                Session["Playlist"] = PLAYLISTVIDEOS;

            }
        }

        //public  async ActionResult CreatePlaylist(string id)
        //{
        //    UserCredential credential;
        //    using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
        //    {
        //        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.Load(stream).Secrets,
        //            // This OAuth 2.0 access scope allows for full read/write access to the
        //            // authenticated user's account.
        //            new[] { YouTubeService.Scope.Youtube },
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(this.GetType().ToString())
        //        );
        //    }

        //    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = this.GetType().ToString()
        //    });

        //    var newPlaylist = new Playlist();
        //    newPlaylist.Snippet = new PlaylistSnippet();
        //    newPlaylist.Snippet.Title = "MiPlaylist";
        //    newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
        //    newPlaylist.Status = new PlaylistStatus();
        //    newPlaylist.Status.PrivacyStatus = "public";
        //    newPlaylist =  youtubeService.Playlists.Insert(newPlaylist, "snippet,status").Execute();

        //    var newPlaylistItem = new PlaylistItem();
        //    newPlaylistItem.Snippet = new PlaylistItemSnippet();
        //    newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
        //    newPlaylistItem.Snippet.ResourceId = new ResourceId();
        //    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
        //    newPlaylistItem.Snippet.ResourceId.VideoId = id;
        //    newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
        //    return PartialView("Playlist", newPlaylist);

        //}


        [HttpGet]
        public ActionResult AddVideoPlaylist(string id, string name, string thu, string url)
        {
            Declare();
            Videos video = new Videos
            {
                Id = id,
                Nombre = name,
                Thumbnail = thu,
                Url = url
            };
            List<Videos> auxList = (List<Videos>)Session["Playlist"];
            auxList.Add(video);
            Session["Playlist"] = auxList;
            return PartialView("Playlist",auxList);
        }


        [HttpGet]
        public ActionResult PlayVideo(string id)
        {
            Videos vidtoPlay = new Videos {Id = id };
            return PartialView("Reproductor", vidtoPlay);
        }



        [HttpGet]
        public ActionResult BuscarVideos(string videoWord)
        {
            List<Videos> videos = new List<Videos>();
            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyBhqe3OaIZT7RusiHlV_kBL3z2CExG3Vb4",
                ApplicationName = "Rockola-264715"
            });
            SearchResource.ListRequest searchListRequest = youtube.Search.List("snippet");
            searchListRequest.Q = videoWord;
            searchListRequest.MaxResults = 40;
            SearchListResponse searchListResponse = searchListRequest.Execute();
            foreach (var item in searchListResponse.Items)
            {
                if(item.Id.Kind == "youtube#video")
                {
                    videos.Add(new Videos {
                        Id = item.Id.VideoId,
                        Nombre = item.Snippet.Title,
                        Url = "https://www.youtube.com/embed/" + item.Id.VideoId,
                        Thumbnail = "http://img.youtube.com/vi/" + item.Id.VideoId + "/hqdefault.jpg"


                    });
                }
            }
            return PartialView("searchVideos",videos);
        }
    }
}