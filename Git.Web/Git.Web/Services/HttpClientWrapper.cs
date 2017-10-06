using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Git.Web.Services
{
    public interface IHttpClient
    {
        Task<TType> GetAsync<TType>(string url);
    }

    public class HttpClientWrapper : IHttpClient
    {
        public async Task<TType> GetAsync<TType>(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Git-Repo-Search");

                var response = await client.GetAsync(url);

                switch(response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        string content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TType>(content);
                    default:
                        throw new Exception($"There was an error executing the request, status code {response.StatusCode}");
                }
            }
        }

    }
}
