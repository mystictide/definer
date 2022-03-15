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
                filterModel = filterModel,
            };
            var result = new DMessagesManager().FilteredList(request, user.ID);
            return View(result);
        }

        [Route("m/archive"), HttpGet]
        public ActionResult ViewArchivedMessages(Filter filter, DMessages filterModel)
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
                filterModel = filterModel,
            };
            var result = new DMessagesManager().ArchiveFilteredList(request, user.ID);
            return View(result);
        }

        [Route("m/{ID}")]
        public ActionResult ViewMessage(int ID, Filter filter, DMessagesJunction filterModel)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
            if (new DMessagesManager().CheckDMOwner(ID, user.ID))
            {
                filter.pageSize = 10;
                filter.isDetailSearch = false;
                FilteredList<DMessagesJunction> request = new FilteredList<DMessagesJunction>()
                {
                    filter = filter,
                    filterModel = filterModel
                };
                var result = new DMessagesJunctionManager().GetDMs(request, ID, user.ID);
                if (result == null)
                {
                    return Redirect("/oops");
                }
                return View(result);
            }
            else
            {
                return Redirect("/m/");
            }
        }

        [Route("m/reply"), HttpPost]
        public ActionResult ReplyMessage(DMessages dm)
        {
            var model = new DMessagesJunction();
            model.DMID = dm.ID;
            model.Date = DateTime.Now;
            model.Body = dm.dmBody;
            model.UserID = user.ID;
            model.IsRead = false;
            new DMessagesJunctionManager().Add(model);
            return Redirect("/m/" + dm.ID);
        }

        [Route("m/create"), HttpPost]
        public ActionResult CreateDM(DMViewModel dm)
        {
            var model = new DMessages();
            model.ReceiverID = dm.UserID;
            model.SenderID = user.ID;
            model.IsReceiverActive = true;
            model.IsSenderActive = true;
            var result = new DMessagesManager().Add(model);
            var message = new DMessagesJunction();
            message.DMID = result.ReturnID;
            message.Body = dm.dmBody;
            message.Date = DateTime.Now;
            message.UserID = user.ID;
            message.IsRead = false;
            new DMessagesJunctionManager().Add(message);
            return Redirect("/m/" + result.ReturnID);
        }

        [Route("m/delete/{ID}")]
        public ActionResult DeleteDM(int ID)
        {
            if (new DMessagesManager().CheckDMOwner(ID, user.ID))
            {
                new DMessagesManager().Archive(ID, user.ID, 0);
            }
            return Redirect("/m/");
        }

        [Route("m/reinstate/{ID}")]
        public ActionResult ReinstateDM(int ID)
        {
            if (new DMessagesManager().CheckDMOwner(ID, user.ID))
            {
                new DMessagesManager().Archive(ID, user.ID, 1);
            }
            return Redirect("/m/");
        }

        [Route("unreaddms"), HttpGet]
        public JsonResult CheckUnreadMessages()
        {
            var result = new DMessagesManager().UnreadMessages(user.ID);
            return Json(result);
        }
    }
}
