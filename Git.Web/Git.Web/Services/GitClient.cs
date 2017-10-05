using Git.Web.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace Git.Web.Services
{
    public interface IGitClient
    {
        Task<SearchResult> Search(string searchCriteria);
    }

    public class GitClient : IGitClient
    {
        readonly IHttpClient _httpClient;

        public GitClient(IHttpClient httpClient) => _httpClient = httpClient;

        public const string SEARCHURL = "https://api.github.com/search/repositories";

        public async Task<SearchResult> Search(string searchCriteria)
        {
            var response = await _httpClient.GetAsync($"{SEARCHURL}?q={searchCriteria}");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return await GetSearchResult(response);
                default:
                    return new SearchResult
                    {
                        Success = false,
                        ErrorMessage = $"Status code from Github: {response.StatusCode}"
                    };
            }
        }

        async Task<SearchResult> GetSearchResult(System.Net.Http.HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SearchResult>(content);
            result.Items = result.Items.Take(5).ToList();
            result.Success = true;
            return result;
        }
    }
}
