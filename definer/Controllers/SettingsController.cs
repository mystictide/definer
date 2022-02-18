using definer.Business.Users;
using definer.Entity.Users;
using definer.Models;
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
    }
}
