using Microsoft.AspNetCore.Mvc;
using Git.Web.Models;
using Git.Web.Services;
using System.Threading.Tasks;

namespace Git.Web.Controllers
{
    public class RepositoryController : Controller
    {
        readonly IGitClient _gitClient;

        public RepositoryController(IGitClient gitClient) => _gitClient = gitClient;

        public ActionResult Index()
        {
           return View(new SearchModel());
        }

        public async Task<IActionResult> Search(SearchModel inputModel)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), inputModel);

            var results = await _gitClient.Search(inputModel.SearchCriteria);

            inputModel.SetResults(results);

            return View(nameof(Index), inputModel);
        }
    }
}
