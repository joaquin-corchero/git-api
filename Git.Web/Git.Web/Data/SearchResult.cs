using System.Collections.Generic;

namespace Git.Web.Data
{
    public class SearchResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalCount { get; set; }
        public bool ImcompleteResults { get; set; }
        public IList<GitRepo> Items { get; set; }
    }
}
