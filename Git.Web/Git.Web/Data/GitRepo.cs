using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool CouldRetriveCommits { get; private set; }

        public string ErrorMessage { get; private set; }

        internal void SetSuccess(List<GitCommit> commits)
        {
            GitCommits = commits.Take(5).ToList();
            CouldRetriveCommits = true;
        }

        internal void SetError(Exception e)
        {
            CouldRetriveCommits = false;
            ErrorMessage = $"Couldn't retrieve commits: {e.Message}";
            GitCommits = new List<GitCommit>();
        }
    }

    public class GitUser
    {
        public string Login { get; set; }

        public int Id { get; set; }

        public string AvatarUrl { get; set; }

        public string Url { get; set; }
    }
}
