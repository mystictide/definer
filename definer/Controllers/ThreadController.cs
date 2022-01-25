using definer.Business.Threads;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace definer.Controllers
{
    [AllowAnonymous, Route("t")]
    public class ThreadController : Controller
    {
        [Route("{title}-{ID}")]
        public ActionResult ViewThread(int ID, Filter filter, Entry filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 10;
            filter.isDetailSearch = false;
            filterModel.ThreadID = ID;
            FilteredList<Entry> request = new FilteredList<Entry>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Entry> result = new EntryManager().FilteredList(request);
            return View(result);
        }
    }
}
