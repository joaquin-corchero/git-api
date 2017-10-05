using Git.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Git.Web.Services
{
    public interface IGitClient
    {
        string SearchUrl { get; }

        List<GitRepository> Search(string searchCriteria);
    }

    public class GitClient : IGitClient
    {
        readonly IHttpClient _httpClient;

        public GitClient(IHttpClient httpClient) => _httpClient = httpClient;

        public string SearchUrl => "https://api.github.com/search/repositories";

        public List<GitRepository> Search(string searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
