using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Download
    {
        public async static Task<JObject> DownloadJObject(string url)
        {
            return  JObject.Parse(await DownloadPageAsync(url));
        }
      

        public async static Task<string> DownloadPageAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                // TODO
                //AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Page Scanner");
                var rm = new HttpRequestMessage { Version = new Version(2, 0), Method = HttpMethod.Get, RequestUri = new Uri(url) };

                var response = await client.SendAsync(rm);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
