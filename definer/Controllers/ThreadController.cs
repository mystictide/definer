using definer.Business.Threads;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace definer.Controllers
{
    [AllowAnonymous, Route("")]
    public class ThreadController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        [Route("entry"), HttpPost]
        public ActionResult EntryManager(Entry model)
        {
            //var claim = JsonConvert.DeserializeObject<Users>(User.FindFirst("user").Value);
            model.UserID = user.ID;
            model.Date = DateTime.Now;
            model.EditDate = null;
            model.IsActive = true;
            var result = new EntryManager().Add(model);
            return Redirect(Request.Headers["Referer"].ToString());
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
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
