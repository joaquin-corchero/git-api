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
        public const string SEARCHURL = "https://api.github.com/search/repositories";
        public const string COMMITSURL = "https://api.github.com/repos";

        public GitClient(IHttpClient httpClient) => _httpClient = httpClient;

        public async Task<SearchResult> SearchAsync(string searchCriteria)
        {
            try
            {
                var result = await _httpClient.GetAsync<SearchResult>($"{SEARCHURL}?q={searchCriteria}");
                result.SetSuccess();
                await PopulateCommits(result);

                return result;
            }
            catch (Exception e)
            {
                return SearchResult.CreateWithError(e);
            }
        }

        async Task PopulateCommits(SearchResult result)
        {
            var tasks = result.Items.Select(i => GetCommitsAsync(i));

            await Task.WhenAll(tasks);
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
