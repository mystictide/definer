using definer.Business.Threads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [AllowAnonymous, Route("t")]
    public class ThreadController : Controller
    {
        [Route("{title}-{ID}")]
        public ActionResult ViewThread(int ID)
        {
            var result = new ThreadManager().Get(ID);
            return View(result);
        }
    }
}
