using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Web.Mvc;
using Rockkola.Models;
using System.Collections.Generic;

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
        public ActionResult PlayVideo(string url)
        {
            Videos vidtoPlay = new Videos {Url = url };
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