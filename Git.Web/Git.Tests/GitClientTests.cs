using Git.Web.Data;
using Git.Web.Models;
using Git.Web.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Git.Tests
{
    public class When_working_with_the_git_client
    {
        IGitClient _gitClient;
        Mock<IHttpClient> _httpClient;

        public When_working_with_the_git_client()
        {
            _httpClient = new Mock<IHttpClient>();
            _gitClient = new GitClient(_httpClient.Object);
        }

        public class And_searching : When_working_with_the_git_client
        {
            const string _searchCriteria = "something";
            SearchResult _result;
            string _url;

            public And_searching()
            {
                _url = $"{GitClient.SEARCHURL}?q={_searchCriteria}";

                _httpClient.Setup(c => c.GetAsync<SearchResult>(_url))
                    .ReturnsAsync(_result);
            }

            [Fact]
            public async Task The_http_client_is_called()
            {
                await _gitClient.SearchAsync(_searchCriteria);

                _httpClient.Verify(c => c.GetAsync<SearchResult>(_url), Times.Once);
            }

            [Fact]
            public async Task Error_is_set_if_exception_is_thrown()
            {
                _httpClient.Setup(c => c.GetAsync<SearchResult>(_url))
                    .ThrowsAsync(new Exception("Some comunication issue"));

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal("Error getting repos: Some comunication issue", _result.ErrorMessage);
                Assert.False(_result.Success);
            }

            [Fact]
            public async Task Success_can_be_returned()
            {
                var expectedSearchResult = new SearchResult { Items = new List<GitRepo>() };

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal(null, _result.ErrorMessage);
                Assert.True(_result.Success);
            }

            [Fact]
            public async Task Only_5_items_are_returned()
            {
                var expectedSearchResult = new SearchResult { Items = new List<GitRepo>() };
                Enumerable
                    .Range(1, 20)
                    .ToList()
                    .ForEach(i=> expectedSearchResult.Items.Add(new GitRepo { Id = i }));

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal(5, _result.Items.Count());
            }


        }
    }
}
