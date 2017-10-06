using Git.Web.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Git.Web.Models
{
    public class SearchModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Search criteria must between 2 and 25 characters in lenght")]
        [MaxLength(25, ErrorMessage = "Search criteria must between 2 and 25 characters in lenght")]
        [DisplayName("Search criteria")]
        public string SearchCriteria { get; set; }

        public SearchResult SearchResults { get; private set; }

        internal void SetResults(SearchResult results) => SearchResults = results;
    }
}