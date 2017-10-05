using Git.Web.Controllers;
using Git.Web.Models;
using Git.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Assert.False(_resultModel.Repositories.Any());
            }
        }

        public class And_searching_repositories : When_working_with_the_repository_controller
        {
            protected ViewResult _viewResult;
            protected SearchModel _outputModel;
            protected SearchModel _inputModel;

            void Execute()
            {
                _viewResult = _controller.Search(_inputModel) as ViewResult;
                _outputModel = (SearchModel)_viewResult.Model;
            }

            public class And_criteria_is_not_provided : And_searching_repositories
            {
                [Fact]
                public void Empty_response_is_returned()
                {
                    _inputModel = new SearchModel();

                    Execute();

                    Assert.Equal(null, _outputModel.SearchCriteria);
                    Assert.False(_outputModel.Repositories.Any());
                }

                [Fact]
                public void No_request_is_made_to_the_git_client()
                {
                    _inputModel = new SearchModel();

                    Execute();

                    _gitClient.Verify(c => c.Search(It.IsAny<string>()), Times.Never);
                }
            }

            public class And_the_model_is_invalid : And_searching_repositories
            {
                public And_the_model_is_invalid()
                {
                    _inputModel = new SearchModel { SearchCriteria = "s" };
                    _controller.ModelState.AddModelError("TestError", "An error");
                }

                [Fact]
                public void No_request_is_made_to_the_git_client()
                {
                    Execute();

                    _gitClient.Verify(c => c.Search(It.IsAny<string>()), Times.Never);
                }

                [Fact]
                public void The_repos_are_empty()
                {
                    Execute();

                    Assert.False(_outputModel.Repositories.Any());
                }
            }

            public class And_the_model_is_valid : And_searching_repositories
            {
                List<GitRepository> _gitRepos;

                public And_the_model_is_valid()
                {
                    _inputModel = new SearchModel { SearchCriteria = "something to search" };
                    _gitRepos = new List<GitRepository> {
                        new GitRepository{
                            OwnerName = "Owner 1",
                            RepositoryName = "Repo 1",
                            RepositoryURL = "www.github.com/pepe",
                            CreationDate = DateTime.Now.AddMonths(-1),
                            LastPushDate  = DateTime.Now.AddDays(-1)
                        }
                    };
                }

                [Fact]
                public void The_git_client_search_is_executed()
                {
                    Execute();

                    _gitClient.Verify(c => c.Search(_inputModel.SearchCriteria), Times.Once);
                }
            }
        }
    }
}