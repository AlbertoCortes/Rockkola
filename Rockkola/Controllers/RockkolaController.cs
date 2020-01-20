using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Rockkola.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Rockkola.Controllers
{
    public class RockkolaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void DeclareP()
        {
            List<PlaylistR> playlistVideos = new List<PlaylistR>();
            if (Session["PlayListRL"] == null)
            {
                Session["PlayListRL"] = playlistVideos;
            }
        }

        [HttpGet]
        public ActionResult AddVideoPlaylist(string id, string name, string thu, string url)
        {
            DeclareP();
            int count = 0;
            List<PlaylistR> p = (List<PlaylistR>)Session["PlaylistRL"];
            if (p.Count == 0)
            {
                count = 1;
            }
            else
            {
                count = p.Last().IdVideoCount + 1;
            }

            Videos video = new Videos
            {
                Id = id,
                Nombre = name,
                Thumbnail = thu,
                Url = url
            };
            PlaylistR play = new PlaylistR
            {
                IdVideoCount = count,
                video = video
            };
            p.Add(play);
            Session["PlayListRL"] = p;

            return PartialView("Playlist", p);
        }


        [HttpGet]
        public ActionResult PlayVideo(int cont)
        {
            Videos vidtoPlay = new Videos();
            List<PlaylistR> a = (List<PlaylistR>)Session["PlayListRL"];
            foreach (var i in a)
            {
                if (i.IdVideoCount == cont)
                {
                    vidtoPlay = i.video;
                }
            }
            PlaylistR p = new PlaylistR { IdVideoCount = cont, video = vidtoPlay };

            return PartialView("Reproductor", p);
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
                if (item.Id.Kind == "youtube#video")
                {
                    videos.Add(new Videos
                    {
                        Id = item.Id.VideoId,
                        Nombre = item.Snippet.Title,
                        Url = "https://www.youtube.com/embed/" + item.Id.VideoId,
                        Thumbnail = "http://img.youtube.com/vi/" + item.Id.VideoId + "/hqdefault.jpg"
                    });
                }
            }
            return PartialView("searchVideos", videos);
        }
    }
}