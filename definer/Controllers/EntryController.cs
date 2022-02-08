using definer.Business.Threads;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [Authorize, Route("")]
    public class EntryController : Controller
    {
         private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("vote"), HttpGet]
        public JsonResult Vote(EntryAttribute model)
        {
            EntryAttribute result;
            model.UserID = user.ID;
            if (model.Vote.Value)
            { 
                result = new EntryAttributeManager().Vote(model, true);
            }
            else if (!model.Vote.Value)
            {
                result = new EntryAttributeManager().Vote(model, false);
            }
            else
            {
                result = null;
            }
            return Json(result);
        }

        [Route("fav"), HttpGet]
        public JsonResult Favourite(EntryAttribute model)
        {
            model.UserID = user.ID;
            var result = new EntryAttributeManager().Fav(model);
            return Json(result);
        }
    }
}
