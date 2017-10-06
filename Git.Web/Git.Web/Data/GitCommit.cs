using System;

namespace Git.Web.Data
{
    public class GitCommit
    {
        public string Sha { get; set; }
        public Commit Commit { get; set; }
    }

    public class Commit
    {
        public Committer committer { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }
    }

    public class Committer
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Date { get; set; }
    }
}