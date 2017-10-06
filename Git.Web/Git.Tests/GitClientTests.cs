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
            string _searchUrl;

            public And_searching()
            {
                _searchUrl = $"{GitClient.SEARCHURL}?q={_searchCriteria}";
            }

            public void SetupHttpClientForSearchResult(SearchResult clientOutput)
            {
                _httpClient.Setup(c => c.GetAsync<SearchResult>(_searchUrl))
                   .ReturnsAsync(clientOutput);
            }

            [Fact]
            public async Task The_http_client_is_called()
            {
                await _gitClient.SearchAsync(_searchCriteria);

                _httpClient.Verify(c => c.GetAsync<SearchResult>(_searchUrl), Times.Once);
            }

            [Fact]
            public async Task Error_is_set_if_exception_is_thrown()
            {
                _httpClient.Setup(c => c.GetAsync<SearchResult>(_searchUrl))
                    .ThrowsAsync(new Exception("Some comunication issue"));

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal("Couldn't retrieve repos: Some comunication issue", _result.ErrorMessage);
                Assert.False(_result.CouldRetrieveRepos);
            }

            [Fact]
            public async Task Success_can_be_returned()
            {
                SetupHttpClientForSearchResult(new SearchResult { Items = new List<GitRepo>() });

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal(null, _result.ErrorMessage);
                Assert.True(_result.CouldRetrieveRepos);
            }

            [Fact]
            public async Task Only_5_items_are_returned()
            {
                var expectedSearchResult = GetNRepos(10);

                SetupHttpClientForSearchResult(expectedSearchResult);

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal(5, _result.Items.Count());
            }

            SearchResult GetNRepos(int n)
            {
                var expectedSearchResult = new SearchResult { Items = new List<GitRepo>() };
                Enumerable
                    .Range(1, n)
                    .ToList()
                    .ForEach(i => expectedSearchResult.Items.Add(
                        new GitRepo { Id = i, Name = $"Repo-{i}", Owner = new GitUser { Login = $"login-{i}" } }
                ));
                return expectedSearchResult;
            }

            [Fact]
            public async Task Commits_are_requested_from_github()
            {
                var numberOfRepos = 2;
                var expectedRepos = GetNRepos(numberOfRepos);
                SetupHttpClientForSearchResult(expectedRepos);
                SetupHttpClientForCommits(expectedRepos);

                _result = await _gitClient.SearchAsync(_searchCriteria);

                _httpClient.Verify(c =>
                    c.GetAsync<List<GitCommit>>(It.IsAny<string>()),
                    Times.Exactly(numberOfRepos)
                );

                _httpClient.Verify(c =>
                    c.GetAsync<List<GitCommit>>(GetCommtRequestUrl(expectedRepos.Items[0])),
                    Times.Once
                );
                _httpClient.Verify(c =>
                    c.GetAsync<List<GitCommit>>(GetCommtRequestUrl(expectedRepos.Items[1])),
                    Times.Once
                );
            }

            [Fact]
            public async Task Repos_get_the_commits_populated()
            {
                var expectedRepos = GetNRepos(1);
                SetupHttpClientForSearchResult(expectedRepos);
                SetupHttpClientForCommits(expectedRepos);

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.Equal(2, _result.Items[0].GitCommits.Count());
                Assert.Equal($"Message 1 for repo {_result.Items[0].Name}", _result.Items[0].GitCommits[0].Commit.Message);
                Assert.Equal($"Message 2 for repo {_result.Items[0].Name}", _result.Items[0].GitCommits[1].Commit.Message);
            }

            [Fact]
            public async Task Error_message_on_the_repo_is_set_when_exception_getting_commits()
            {
                var expectionMessage = "Communication error";
                var expectedRepos = GetNRepos(1);
                SetupHttpClientForSearchResult(expectedRepos);
                _httpClient.Setup(c => c.GetAsync<List<GitCommit>>(It.IsAny<string>()))
                  .ThrowsAsync(new Exception(expectionMessage));

                _result = await _gitClient.SearchAsync(_searchCriteria);

                Assert.False(_result.Items[0].CouldRetriveCommits);
                Assert.Equal($"Couldn't retrieve commits: {expectionMessage}", _result.Items[0].ErrorMessage);
            }

            string GetCommtRequestUrl(GitRepo repo)
            {
                return $"{GitClient.COMMITSURL}/{repo.Owner.Login}/{repo.Name}/commits";
            }

            void SetupHttpClientForCommits(SearchResult expectedRepos)
            {
                expectedRepos.Items.ToList().ForEach(repo => SetupHttpClientForCommit(repo));
            }

            void SetupHttpClientForCommit(GitRepo repo)
            {
                var clientOutput = new List<GitCommit>{
                    new GitCommit {
                        Commit = new Commit {
                             Message = $"Message 1 for repo {repo.Name}"
                        }
                    },
                    new GitCommit {
                        Commit = new Commit {
                             Message = $"Message 2 for repo {repo.Name}"
                        }
                    },
                };

                _httpClient.Setup(c => c.GetAsync<List<GitCommit>>(GetCommtRequestUrl(repo)))
                  .ReturnsAsync(clientOutput);
            }
        }
    }
}
