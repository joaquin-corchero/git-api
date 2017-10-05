using System;

namespace Git.Web.Models
{
    public class GitRepository
    {
        public string Name { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryURL { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastPushDate { get; set; }
    }
}