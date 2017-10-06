using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Git.Web.Data
{
    public class GitRepo
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public GitUser Owner { get; set; }

        public string Url { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public string PushedAt { get; set; }

        public string GitUrl { get; set; }

        public IList<GitCommit> GitCommits { get; internal set; }
    }
}
