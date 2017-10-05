using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Git.Web.Models;

namespace Git.Web.Controllers
{
    public class RepositoryController : Controller
    {
        // GET: /<controller>/
        public ViewResult Index()
        {
            return View(new SearchModel());
        }

        public ViewResult Search(SearchModel inputModel)
        {
            return View(nameof(Index), inputModel);
        }
    }
}
