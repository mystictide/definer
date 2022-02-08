using definer.Business.Threads;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace definer.Controllers
{
    [Authorize, Route("")]
    public class ThreadController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [AllowAnonymous]
        [Route("entry/{ID}")]
        public ActionResult ViewEntry(int ID)
        {
            Entry result;
            if (user != null)
            {
                ViewBag.User = user;
                result = new EntryManager().Get(ID, user.ID);
            }
            result = new EntryManager().Get(ID);
            return View(result);
        }


        [Route("entry"), HttpPost]
        public ActionResult EntryManager(Entry model)
        {
            if (model.ThreadID > 0)
            {
                model.UserID = user.ID;
                model.Date = DateTime.Now;
                model.EditDate = null;
                model.IsActive = true;
                var result = new EntryManager().Add(model);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                var thread = new Threads();
                thread.Title = model.Title;
                thread.IsActive = true;
                var threadResult = new ThreadManager().Add(thread);

                model.ThreadID = threadResult.ReturnID;
                model.UserID = user.ID;
                model.Date = DateTime.Now;
                model.EditDate = null;
                model.IsActive = true;
                var result = new EntryManager().Add(model);
                return Redirect("/" + CustomTagHelpers.FriendlyURLTitle(model.Title) + "-" + model.ThreadID);
            }
        }

        [Route("edit/entry/{ID}")]
        public ActionResult EditEntry(int ID)
        {
            var result = new EntryManager().Get(ID);
            return View(result);
        }

        [Route("edit/entry"), HttpPost]
        public ActionResult EditEntry(Entry model)
        {
            model.UserID = user.ID;
            model.EditDate = DateTime.Now;
            model.IsActive = true;
            var result = new EntryManager().Update(model);
            return Redirect("/" + CustomTagHelpers.FriendlyURLTitle(model.Title) + "-" + model.ThreadID);
        }
    }
}
