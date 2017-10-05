using Microsoft.AspNetCore.Mvc;
using Git.Web.Models;
using Git.Web.Services;

namespace Git.Web.Controllers
{
    public class RepositoryController : Controller
    {
        readonly IGitClient _gitClient;

        public RepositoryController(IGitClient gitClient) => _gitClient = gitClient;

        public ViewResult Index() => View(new SearchModel());

        public ViewResult Search(SearchModel inputModel)
        {
            if(!ModelState.IsValid)
                return View(nameof(Index), inputModel);

            inputModel.SetResults(_gitClient.Search(inputModel.SearchCriteria));

            return View(nameof(Index), inputModel);
        }
    }
}
