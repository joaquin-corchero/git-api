using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Web.Data
{
    public class SearchResult
    {
        public int Total_Count { get; set; }
        public bool ImcompleteResults { get; set; }
        public IList<GitRepo> Items { get; set; }

        public bool CouldRetrieveRepos { get; private set; }
        public string ErrorMessage { get; private set; }

        public SearchResult() { }

        internal void SetSuccess()
        {
            Items = Items.Take(5).ToList();
            CouldRetrieveRepos = true;
        }

        internal static SearchResult CreateWithError(Exception e)
        {
            return new SearchResult
            {
                CouldRetrieveRepos = false,
                ErrorMessage = $"Couldn't retrieve repos: {e.Message}"
            };
        }
    }

}