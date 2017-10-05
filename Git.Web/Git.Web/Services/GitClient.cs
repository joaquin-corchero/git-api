using Git.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Git.Web.Services
{
    public interface IGitClient
    {
        string SearchUrl { get; }

        Task<List<GitRepository>> Search(string searchCriteria);
    }

    public class GitClient : IGitClient
    {
        readonly IHttpClient _httpClient;

        public GitClient(IHttpClient httpClient) => _httpClient = httpClient;

        public string SearchUrl => "https://api.github.com/search/repositories";

        public async Task<List<GitRepository>> Search(string searchCriteria)
        {
            var response = await _httpClient.Get(SearchUrl, searchCriteria);

            return null;
        }
    }
}
