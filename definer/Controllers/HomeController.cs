using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [AllowAnonymous, Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}