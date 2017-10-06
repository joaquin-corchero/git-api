using System;

namespace Git.Web.Data
{
    public class GitCommit
    {
        public string Sha { get; set; }
        public Commit Commit { get; set; }

        public GitCommit() { }
    }

    public class Commit
    {
        public Committer committer { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public Commit() { }
    }

    public class Committer
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        public Committer() { }
    }
}