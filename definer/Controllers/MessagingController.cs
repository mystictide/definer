using definer.Business.Users;
using definer.Entity.Helpers;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [Authorize, Route("")]
    public class MessagingController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("m"), HttpGet]
        public ActionResult ViewMessages(Filter filter, DMessages filterModel)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 10;
            filter.isDetailSearch = false;
            FilteredList<DMessages> request = new FilteredList<DMessages>()
            {
                filter = filter,
                filterModel = filterModel
            };
            var result = new DMessagesManager().FilteredList(request, user.ID);
            return View(result);
        }
    }
}
