using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TeslaSuperchargers
{
    public class Download
    {
        public static JObject DownloadJObject(string url)
        {
            return JObject.Parse(DownloadPageAsync(url).Result);
        }
      

        public async static Task<string> DownloadPageAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
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
