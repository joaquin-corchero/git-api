using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Git.Web.Services
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public class HttpClientWrapper : IHttpClient
    {
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Git-Repo-Search");

                return await client.GetAsync(url);
            }
        }

    }

}
