using definer.Business.Threads;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [AllowAnonymous, Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("sideBar")]
        [HttpGet]
        public JsonResult sideBar(Filter filter, Threads filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 25;
            filter.isDetailSearch = false;
            FilteredList<Threads> request = new FilteredList<Threads>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Threads> result = new ThreadManager().FilteredList(request);
            return Json(CustomTagHelpers.SidebarContent(result));
        }
    }
}