using Git.Web.Controllers;
using Git.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Xunit;

namespace Git.Tests.Controllers
{

    public class When_working_with_the_repository_controller
    {
        protected RepositoryController _controller = new RepositoryController();

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
            protected SearchModel _resultModel;
            protected SearchModel _inputModel;

            void Execute()
            {
                _viewResult = _controller.Search(_inputModel) as ViewResult;

                _resultModel = (SearchModel)_viewResult.Model;
            }

            public class And_criteria_is_not_provided : And_searching_repositories
            {
                [Fact]
                public void Empty_response_is_returned()
                {
                    _inputModel = new SearchModel();
                    
                    Execute();

                    Assert.Equal(null, _resultModel.SearchCriteria);
                    Assert.False(_resultModel.Repositories.Any());
                }
            }

            public class And_criteria_is_provided : And_searching_repositories
            {


            }
        }
    }
}