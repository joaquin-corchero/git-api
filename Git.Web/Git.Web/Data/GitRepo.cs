using System;

namespace Git.Web.Data
{
    public class GitRepo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public GitUser Owner { get; set; }

        public string Url { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime PushedAt { get; set; }

        public string GitUrl { get; set; }
    }
}
