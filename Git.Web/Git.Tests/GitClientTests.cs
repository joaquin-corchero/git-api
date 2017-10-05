using Git.Web.Models;
using Git.Web.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
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
            string _searchCriteria = "something";
            List<GitRepository> _result;

            void Execute() =>  _result = _gitClient.Search(_searchCriteria);

            [Fact]
            public void The_http_client_is_called()
            {
                Execute();

                _httpClient.Verify(c => c.Get(_gitClient.SearchUrl, _searchCriteria), Times.Once);
            }
        }
    }
}
