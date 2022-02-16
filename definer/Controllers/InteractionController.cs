using definer.Business.Users;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [Authorize, Route("i")]
    public class InteractionController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("setFollowState"), HttpGet]
        public JsonResult SetFollowState(FollowJunction model)
        {
            model.FollowerID = user.ID;
            model.Date = DateTime.Now;
            var result = new FollowJunctionManager().SetState(model);
            return Json(result);
        }

        [Route("setBlockState"), HttpGet]
        public JsonResult SetBlockState(BlockJunction model)
        {
            model.BlockerID = user.ID;
            var result = new BlockJunctionManager().SetState(model);
            return Json(result);
        }
    }
}
