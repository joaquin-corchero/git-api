using Git.Web.Controllers;
using Git.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Xunit.Should;

namespace Git.Tests.Controllers
{
    
    public class When_working_with_the_repository_controller
    {
        protected RepositoryController _controller;

        public class And_searching_repositories : When_working_with_the_repository_controller
        {
            ViewResult _viewResult;

            void Execute()
            {
                _viewResult = _controller.Index() as ViewResult;
            }

            [Fact]
            public void When_no_criteria_is_passed()
            {
                var expected = ResponseModel.Empty();

                Execute();

                _viewResult.Model.ShouldBeSameAs(expected);
            }
        }
    }
}
