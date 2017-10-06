using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Git.Web.Models
{
    public class SearchModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Search criteria must between 2 and 100 characters in lenght")]
        [MaxLength(100, ErrorMessage = "Search criteria must between 2 and 100 characters in lenght")]
        [DisplayName("Search criteria")]
        public string SearchCriteria { get; set; }

        public SearchResultModel SearchResults { get; private set; }

        internal void SetResults(SearchResultModel results) => SearchResults = results;

    }
}