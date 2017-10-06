using Git.Web.Controllers;
using Git.Web.Data;
using Git.Web.Models;
using Git.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Git.Tests.Controllers
{

    public class When_working_with_the_repository_controller
    {
        protected RepositoryController _controller;
        protected Mock<IGitClient> _gitClient;

        public When_working_with_the_repository_controller()
        {
            _gitClient = new Mock<IGitClient>();
            _controller = new RepositoryController(_gitClient.Object);
        }

        public class And_loading_the_index : When_working_with_the_repository_controller
        {
            protected ViewResult _viewResult;
            protected SearchModel _resultModel;

            void Execute()
            {
                _viewResult = _controller.Index() as ViewResult;
                _resultModel = (SearchModel)_viewResult.Model;
            }

            [Fact]
            public void Returns_an_empty_search_model()
            {
                Execute();

                Assert.Equal(null, _resultModel.SearchCriteria);
                Assert.Null(_resultModel.SearchResults);
            }
        }

        public class And_searching_repositories : When_working_with_the_repository_controller
        {
            protected IActionResult _viewResult;
            protected SearchModel _inputModel;

            public class And_the_model_is_invalid : And_searching_repositories
            {
                public And_the_model_is_invalid()
                {
                    _inputModel = new SearchModel { SearchCriteria = "s" };
                    _controller.ModelState.AddModelError("TestError", "An error");
                }

                [Fact]
                public async Task No_request_is_made_to_the_git_client()
                {
                    _viewResult = await _controller.Search(_inputModel);

                    _gitClient.Verify(c => c.SearchAsync(It.IsAny<string>()), Times.Never);
                }

                [Fact]
                public async Task The_repos_are_empty()
                {
                    _viewResult = await _controller.Search(_inputModel);

                    var outputModel = (SearchModel)((ViewResult)_viewResult).Model;
                    Assert.Null(outputModel.SearchResults);
                }
            }

            public class And_the_model_is_valid : And_searching_repositories
            {
                SearchResult _searchResults;

                public And_the_model_is_valid()
                {
                    _inputModel = new SearchModel { SearchCriteria = "something to search" };
                }

                void SetupGitClient()
                {
                    _searchResults = new SearchResult{ TotalCount = 10 };

                    _gitClient.Setup(c => c.SearchAsync(_inputModel.SearchCriteria)).ReturnsAsync(_searchResults);
                }

                [Fact]
                public async Task The_git_client_search_is_executed()
                {
                    SetupGitClient();

                    _viewResult = await _controller.Search(_inputModel);

                    _gitClient.Verify(c => c.SearchAsync(_inputModel.SearchCriteria), Times.Once);
                }

                [Fact]
                public async Task The_search_results_get_set()
                {
                    SetupGitClient();

                    _viewResult = await _controller.Search(_inputModel);
                    var outputModel = (SearchModel)((ViewResult)_viewResult).Model;

                    Assert.Equal(_searchResults, outputModel.SearchResults);
                }

                [Fact]
                public async Task Index_view_is_returned()
                {
                    SetupGitClient();

                    _viewResult = await _controller.Search(_inputModel);

                    Assert.Equal(nameof(_controller.Index), ((ViewResult)_viewResult).ViewName);
                }
            }
        }
    }
}