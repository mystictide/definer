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

        [Route("m/{ID}")]
        public ActionResult ViewMessage(int ID, Filter filter, DMessagesJunction filterModel)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
            filter.pageSize = 10;
            filter.isDetailSearch = false;
            FilteredList<DMessagesJunction> request = new FilteredList<DMessagesJunction>()
            {
                filter = filter,
                filterModel = filterModel
            };
            var result = new DMessagesJunctionManager().GetDMs(request, ID);
            return View(result);
        }

        [Route("m/reply"), HttpPost]
        public ActionResult ReplyMessage(int DMID, string dmBody)
        {
            var model = new DMessagesJunction();
            model.DMID = DMID;
            model.Date = DateTime.Now;
            model.Body = dmBody;
            model.UserID = user.ID;
            model.IsRead = false;
            new DMessagesJunctionManager().Add(model);
            return Redirect("/m/" + DMID);
        }

        [Route("m/create/{UserID}"), HttpGet]
        public JsonResult CreateDM(int UserID, string dmBody)
        {
            var model = new DMessages();
            model.ReceiverID = UserID;
            model.SenderID = user.ID;
            var result = new DMessagesManager().Add(model);
            var message = new DMessagesJunction();
            message.DMID = result.ReturnID;
            message.Body = dmBody;
            message.Date = DateTime.Now;
            message.UserID = user.ID;
            message.IsRead = false;
            new DMessagesJunctionManager().Add(message);
            return Json(result.ReturnID);
        }
    }
}
