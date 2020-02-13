using Newtonsoft.Json;
using Rockkola.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        public async Task<ActionResult>  AddHistory(string nombre, string id)
        {
            List<VideoHist> ListHistory = new List<VideoHist>();
            string urlGEThistory = "http://localhost:8080/Api/ApiHistory";
            var request = (HttpWebRequest)WebRequest.Create(urlGEThistory);
            var content = string.Empty;
            string JsonPOST = "{'videoName':'" + nombre + "','videoID':'" + id + "'}";
            //POST json to insert on history table DB    
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    urlGEThistory,
                     new StringContent(JsonPOST, Encoding.UTF8, "application/json"));
            }
            //GET History from DB
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ListHistory = ser.Deserialize<List<VideoHist>>(content);
            return PartialView("AddHistory", ListHistory);
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
                string url = "http://localhost/Api/API?name=" + videoWord;
                var request = (HttpWebRequest)WebRequest.Create(url);
                var content = string.Empty;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
                JavaScriptSerializer ser = new JavaScriptSerializer();
                videos = ser.Deserialize<List<Videos>>(content);
            return PartialView("searchVideos", videos);
        }

 
    }
}