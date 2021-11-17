using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
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

        [HttpGet(Name = "GetTop10Songs")]

        public void Get()
        {
            //string[] toplinks =;
            GetTopLinks();

        }

        public async static void GetTopLinks()
        {


            string json = JsonConvert.SerializeObject(System.IO.File.ReadAllText("top30.json"));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://charts.youtube.com/youtubei/v1/browse?alt=json&key=AIzaSyCzEW7JUJdSql0-2V4tHUb6laYm4iAE_dM";

            using var client = new HttpClient();

            var response = await client.PostAsync(url, httpContent);

            string result = response.Content.ReadAsStringAsync().Result;


            System.IO.File.WriteAllText("output.txt", result);

        }

    }
}

