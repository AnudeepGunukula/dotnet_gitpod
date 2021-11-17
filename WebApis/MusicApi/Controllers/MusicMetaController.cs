using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;

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


        [HttpPost(Name = "GetTop10Songs")]
        public async void GetTopLinks()
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


            string jsontext = @"{ {""context"":{""client"":{""clientName"":""WEB_MUSIC_ANALYTICS"",""clientVersion"":""0.2"",""hl"":""en"",""gl"":""IN"",""experimentIds"":[],""experimentsToken"":"""",""theme"":""MUSIC""},""capabilities"":{},""request"":{""internalExperimentFlags"":[]}},""browseId"":""FEmusic_analytics_charts_home"",""query"":""chart_params_type=WEEK&perspective=CHART&flags=viral_video_chart&selected_chart=TRACKS&chart_params_id=weekly%3A0%3A0%3Ain""} }";

            var jsonobj = JsonConvert.SerializeObject(jsontext);
            request.Content = new StringContent(jsonobj, null, "application/json");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");


            var response = await httpClient.SendAsync(request);


            var jsonData = response.Content.ReadAsStringAsync();
            System.IO.File.WriteAllText("output.txt", jsonData.Result);


        }


    }
}

