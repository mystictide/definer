using definer.Business.Users;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static definer.Models.RenderView;

namespace definer.Controllers
{
    [Authorize, Route("u")]
    public class AuthorController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        private readonly IViewRenderService _viewRenderService;
        public AuthorController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        [Route("")]
        [Route("{username}")]
        public ActionResult ViewAuthor(string? username)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
            var result = new Users();
            if (username == null || username.Length < 1)
            {
                result = new UserManager().Get(user.Username, user.ID);
            }
            else
            {
                result = new UserManager().Get(username, user.ID);
            }
            return View(result);
        }

        [Route("authorEntries"), HttpGet]
        [Route("authorEntries/{username}"), HttpGet]
        public async Task<JsonResult> authorEntries(string? username, Filter filter, Entry filterModel)
        {
            var result = new Users();
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 7;
            filter.isDetailSearch = false;
            FilteredList<Entry> request = new FilteredList<Entry>()
            {
                filter = filter,
                filterModel = filterModel
            };
            if (username == null || username.Length < 1)
            {
                result = new UserManager().GetbyUsername(request, user.Username);
            }
            else
            {
                result = new UserManager().GetbyUsername(request, username);
            }
            if (user != null)
            {
                result.CurrentUser = user;
            }
            var rendered = await _viewRenderService.RenderToStringAsync("Author/_AuthorEntries", result);
            return Json(rendered);
        }

        [Route("authorFavourites"), HttpGet]
        [Route("authorFavourites/{username}"), HttpGet]
        public async Task<JsonResult> authorFavourites(string? username, Filter filter, Entry filterModel)
        {
            var result = new Users();
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 7;
            filter.isDetailSearch = false;
            FilteredList<Entry> request = new FilteredList<Entry>()
            {
                filter = filter,
                filterModel = filterModel
            };
            if (username == null || username.Length < 1)
            {
                result = new UserManager().GetFavouritesbyUsername(request, user.Username);
            }
            else
            {
                result = new UserManager().GetFavouritesbyUsername(request, username);
            }
            if (user != null)
            {
                result.CurrentUser = user;
            }
            var rendered = await _viewRenderService.RenderToStringAsync("Author/_AuthorFavourites", result);
            return Json(rendered);
        }

        [Route("managebio"), HttpGet]
        public JsonResult ManageBio(string? bio)
        {
            var result = new UserManager().ManageBio(user.ID, bio);
            return Json(result);
        }

        [Route("getbio"), HttpGet]
        public JsonResult GetBio()
        {
            var result = new UserManager().GetBio(user.ID);
            return Json(result);
        }
    }
}
