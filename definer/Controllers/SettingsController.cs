using definer.Business.Threads;
using definer.Business.Users;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [Authorize, Route("settings")]
    public class SettingsController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("prefs")]
        public ActionResult Preferences()
        {
            var result = new PreferenceJunctionManager().Get(user.ID);
            return View(result);
        }

        [Route("prefs"), HttpPost]
        public ActionResult Preferences(bool? pm, int? pagesize)
        {
            var model = new PreferenceJunction();
            model.UserID = user.ID;
            if (pm.HasValue)
            {
                model.Messaging = pm.Value;
            }
            if (pagesize.HasValue)
            {
                model.PageSize = pagesize.Value;
            }
            var result = new PreferenceJunctionManager().Manage(model);
            return Redirect("/settings/prefs");
        }

        [Route("social")]
        public ActionResult Social()
        {
            var result = new SocialJunctionManager().Get(user.ID);
            return View(result);
        }

        [Route("social"), HttpPost]
        public ActionResult Social(SocialJunction model)
        {
            model.UserID = user.ID;
            var result = new SocialJunctionManager().Manage(model);
            return Redirect("/settings/social");
        }

        [Route("account")]
        public ActionResult Account()
        {
            var model = new UserViewModel();
            model.Mail = user.Mail;
            model.Password = user.Password;
            model.Username = user.Username;
            return View(model);
        }

        [Route("blocked")]
        public ActionResult BlockedUsers()
        {
            var result = new BlockJunctionManager().GetBlockedList(user.ID);
            return View(result);
        }

        [Route("archive")]
        public ActionResult EntryArchive(Filter filter, Entry filterModel)
        {
            var result = new Users();
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 15;
            filter.isDetailSearch = false;
            FilteredList<Entry> request = new FilteredList<Entry>()
            {
                filter = filter,
                filterModel = filterModel
            };
            result = new UserManager().GetEntryArchivebyUsername(request, user.Username);

            if (user != null)
            {
                result.CurrentUser = user;
            }
            return View(result);
        }

        [Route("username"), HttpPost]
        public ActionResult ChangeUsername(UserViewModel model)
        {
            var result = new UserManager().UpdateUsername(user.ID, model.Username);
            return Redirect("/account/logout");
        }

        [Route("password"), HttpPost]
        public ActionResult ChangePassword(UserViewModel model)
        {
            PasswordHasher _passwordHasher = new PasswordHasher();
            model.Password = _passwordHasher.HashPassword(model.Password);
            var result = new UserManager().UpdatePassword(user.ID, model.Password);
            return Redirect("/account/logout");
        }

        [Route("email"), HttpPost]
        public ActionResult ChangeEmail(UserViewModel model)
        {
            var result = new UserManager().UpdateEmail(user.ID, model.Mail);
            return Redirect("/account/logout");
        }

        [Route("deactivate"), HttpPost]
        public ActionResult DeactivateAccount()
        {
            var result = new UserManager().DeactivateAccount(user.ID);
            return Redirect("/account/logout");
        }
    }
}
