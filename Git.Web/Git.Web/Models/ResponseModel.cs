using Git.Web.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Git.Web.Models
{
    public class SearchModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Search criteria must between 2 and 25 characters in lenght")]
        [MaxLength(25, ErrorMessage = "Search criteria must between 2 and 25 characters in lenght")]
        public string SearchCriteria { get; set; }

        public SearchResult SearchResults { get; private set; }

        internal void SetResults(SearchResult results) => SearchResults = results;
    }
}