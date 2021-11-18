using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json.Net;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using MusicApi.Models;
namespace MusicApi.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class MusicMetaController : ControllerBase
    {

        private readonly ILogger<MusicMetaController> _music;

        public MusicMetaController(ILogger<MusicMetaController> music)
        {
            this._music = music;
        }

        [HttpPost(Name = "SearchSong")]
        public async Task<IActionResult> SearchSong(string songName)
        {
            string url = $"https://www.google.com/search?client=firefox-b-d&q={songName}+song";
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:94.0) Gecko/20100101 Firefox/94.0");
            request.Headers.TryAddWithoutValidation("Accept", "*/*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");

            var response = await httpClient.SendAsync(request);

            var jsonData = response.Content.ReadAsStringAsync();
            string result = jsonData.Result;

            var musiclist = Search(result);

            return Ok(musiclist);

        }


        [HttpGet]
        public async Task<IActionResult> GetTrendingSongs()
        {
            var httpClient = new HttpClient();


            var request = new HttpRequestMessage(new HttpMethod("POST"), "https://charts.youtube.com/youtubei/v1/browse?alt=json&key=AIzaSyCzEW7JUJdSql0-2V4tHUb6laYm4iAE_dM");


            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:94.0) Gecko/20100101 Firefox/94.0");
            request.Headers.TryAddWithoutValidation("Accept", "*/*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
            request.Headers.TryAddWithoutValidation("X-Goog-Visitor-Id", "");
            request.Headers.TryAddWithoutValidation("X-YouTube-Client-Name", "31");
            request.Headers.TryAddWithoutValidation("X-YouTube-Client-Version", "0.2");
            request.Headers.TryAddWithoutValidation("X-YouTube-Utc-Offset", "330");
            request.Headers.TryAddWithoutValidation("X-YouTube-Time-Zone", "Asia/Kolkata");
            request.Headers.TryAddWithoutValidation("X-YouTube-Ad-Signals", "dt=1637166854665&flash=0&frm&u_tz=330&u_his=2&u_h=864&u_w=1536&u_ah=824&u_aw=1536&u_cd=24&bc=31&bih=722&biw=365&brdim=-7");
            request.Headers.TryAddWithoutValidation("Origin", "https://charts.youtube.com");
            request.Headers.TryAddWithoutValidation("DNT", "1");
            request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
            request.Headers.TryAddWithoutValidation("Referer", "https://charts.youtube.com/charts/TrendingVideos/in");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            request.Headers.TryAddWithoutValidation("TE", "trailers");
            request.Headers.TryAddWithoutValidation("Cookie", "YSC=oJSblTAgE2Y; VISITOR_INFO1_LIVE=0zWqoewM77A");


            var jsonString = @"{'context':{'client':{'clientName':'WEB_MUSIC_ANALYTICS','clientVersion':'0.2','hl':'en','gl':'IN','experimentIds':[],'experimentsToken':'','theme':'MUSIC'},'capabilities':{},'request':{'internalExperimentFlags':[]}},'browseId':'FEmusic_analytics_charts_home','query':'chart_params_type=WEEK&perspective=CHART&flags=viral_video_chart'}";
            request.Content = new StringContent(jsonString);

            var response = await httpClient.SendAsync(request);

            var jsonData = response.Content.ReadAsStringAsync();

            string result = jsonData.Result;

            List<MusicMeta> musiclist = filter(result);

            return Ok(musiclist);
        }


        public static List<MusicMeta> filter(string result)
        {
            List<MusicMeta> musiclist = new List<MusicMeta>();
            string[] arr = result.Split("videos")[1].Split("\"id\":");

            for (int i = 1; i < arr.Length; i++)
            {
                string id;
                string name;
                string thumbnail;
                string url;

                id = arr[i].Split(",")[0].TrimStart();
                id = id.Replace("\"", "");
                name = arr[i].Split("\"title\": ")[1].Split("\",")[0].Replace("\"", "");
                thumbnail = $"https://i.ytimg.com/vi/{id}/maxresdefault.jpg";
                url = $"https://www.youtube.com/watch?v={id}";

                MusicMeta mobj = new MusicMeta();
                mobj.Url = url;
                mobj.SongName = name;
                mobj.thumbnail = thumbnail;

                musiclist.Add(mobj);
            }

            return musiclist;
        }


        public static List<MusicMeta> Search(string result)
        {
            List<MusicMeta> musiclist = new List<MusicMeta>();

            List<string> seenId = new List<string>();
            string url = "\"https://www.youtube.com/watch?v=";
            string splitter = $"href={url}";
            string[] arr = result.Split(splitter);
            for (int i = 1; i < arr.Length; i++)
            {
                string id = arr[i].Split("\"")[0];
                if (seenId.Contains(id))
                {
                    continue;
                }
                else
                {
                    seenId.Add(id);
                }
                url = "https://www.youtube.com/watch?v=" + id;
                System.IO.File.WriteAllText($"output{i}.txt", arr[i]);
                string Name = getName(arr[i]);


                string thumbnail = $"https://i.ytimg.com/vi/{id}/maxresdefault.jpg";

                MusicMeta mobj = new MusicMeta();
                mobj.Url = url;
                mobj.SongName = Name;
                mobj.thumbnail = thumbnail;
                musiclist.Add(mobj);

            }

            return musiclist;
        }


        public static string getName(string str)
        {
            string Name = "";
            string st = "- YouTube<//h3><div class=";
            if (str.Contains("<img alt="))
            {
                Name = str.Split("<img alt=\"")[1].Split("\"")[0];
            }
            else if (str.Contains("aria-label="))
            {
                Name = str.Split("aria-label=\"")[1].Split("\"")[0];
            }
            return Name;
        }
    }

}


