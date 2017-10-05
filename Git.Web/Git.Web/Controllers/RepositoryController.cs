using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Git.Web.Models;
using Git.Web.Services;

namespace Git.Web.Controllers
{
    public class RepositoryController : Controller
    {
        private IGitClient _gitClient;

        public RepositoryController(IGitClient gitClient)
        {
            this._gitClient = gitClient;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            return View(new SearchModel());
        }

        public ViewResult Search(SearchModel inputModel)
        {
            if(!ModelState.IsValid)
                return View(nameof(Index), inputModel);

            inputModel.SetResults(_gitClient.Search(inputModel.SearchCriteria));

            return View(nameof(Index), inputModel);
        }
    }
}
