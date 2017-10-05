using Git.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Git.Web.Services
{
    public interface IGitClient
    {
        List<GitRepository> Search(string searchCriteria);
    }

    public class GitClient : IGitClient
    {
        public List<GitRepository> Search(string searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
