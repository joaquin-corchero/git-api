using Git.Web.Data;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Git.Web.Services
{
    public interface IGitClient
    {
        Task<SearchResult> SearchAsync(string searchCriteria);
    }

    public class GitClient : IGitClient
    {
        readonly IHttpClient _httpClient;

        public GitClient(IHttpClient httpClient) => _httpClient = httpClient;

        public const string SEARCHURL = "https://api.github.com/search/repositories";
        public const string COMMITSURL = "https://api.github.com/repos";

        public async Task<SearchResult> SearchAsync(string searchCriteria)
        {
            try
            {
                var result = await _httpClient.GetAsync<SearchResult>($"{SEARCHURL}?q={searchCriteria}");
                result.Items = result.Items.Take(5).ToList();
                result.Success = true;

                var tasks = result.Items.Select(i => GetCommitsAsync(i));

                await Task.WhenAll(tasks);

                return result;
            }
            catch (Exception e)
            {
                return new SearchResult
                {
                    Success = false,
                    ErrorMessage = $"Couldn't retrieve repos: {e.Message}"
                };
            }
        }

        async Task GetCommitsAsync(GitRepo gitRepo)
        {
            try
            {
                string url = $"{COMMITSURL}/{gitRepo.Owner.Login}/{gitRepo.Name}/commits";
                var result = await _httpClient.GetAsync<List<GitCommit>>(url);
                gitRepo.SetSuccess(result);
            }
            catch(Exception e)
            {
                gitRepo.SetError(e);
            }
        }
    }
}
