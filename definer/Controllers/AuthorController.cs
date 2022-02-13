using definer.Business.Users;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [Authorize, Route("u")]
    public class AuthorController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("")]
        [Route("{username}")]
        //[Route("{username}-{ID}")]
        public ActionResult ViewAuthor(string? username, Filter filter, Entry filterModel)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
            var result = new Users();
            if (username == null || username.Length < 1)
            {
                result = new UserManager().GetbyUsername(user.Username);
            }
            else if (true)
            {
                result = new UserManager().GetbyUsername(username);
            }
            return View(result);
        }
    }
}
