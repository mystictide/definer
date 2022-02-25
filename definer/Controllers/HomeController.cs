﻿using definer.Business.Threads;
using definer.Business.Users;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;
using definer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static definer.Models.RenderView;

namespace definer.Controllers
{
    [AllowAnonymous, Route("")]
    public class HomeController : Controller
    {
        private Users _user;
        public Users user { get { return _user ?? (_user = ValidateUser.ValidateCurrentUser(this)); } }

        private readonly IViewRenderService _viewRenderService;
        public HomeController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        [Route("")]
        [Route("{title}")]
        [Route("{title}-{ID}")]
        public ActionResult Index(string? title, int? ID, Filter filter, Entry filterModel)
        {
            if (user != null)
            {
                ViewBag.User = user;
            }
            if (ID.HasValue)
            {
                FilteredList<Entry> result = new FilteredList<Entry>();
                filter.Keyword = filter.Keyword ?? "";
                //filter.pageSize = user?.UserSettings.PageSize ?? 10;
                filter.isDetailSearch = false;
                filterModel.ThreadID = ID.Value;
                FilteredList<Entry> request = new FilteredList<Entry>()
                {
                    filter = filter,
                    filterModel = filterModel
                };
                if (user != null)
                {
                    result = new EntryManager().FilteredList(request, user.ID);
                }
                else
                {
                    result = new EntryManager().FilteredList(request);
                }
                ViewBag.Entries = result;
                return View();
            }
            else if (title != null)
            {
                var model = new Threads();
                model.Title = title;
                ViewBag.Threads = model;
                return View();
            }
            else
            {
                return View();
            }
        }

        [Route("s/{search}"), HttpGet]
        [Route("s"), HttpPost]
        public ActionResult Search(string search)
        {
            var model = new ThreadManager().GetbyTitle(search);
            if (model == null)
            {
                return Redirect("/" + search);
            }
            return Redirect("/" + CustomTagHelpers.FriendlyURLTitle(model.Title) + "-" + model.ID);
        }

        [Route("sideBar"), HttpGet]
        public async Task<JsonResult> sideBar(Filter filter, Threads filterModel)
        {
            filter.Keyword = filter.Keyword ?? "";
            filter.pageSize = 25;
            filter.isDetailSearch = false;
            FilteredList<Threads> request = new FilteredList<Threads>()
            {
                filter = filter,
                filterModel = filterModel
            };
            FilteredList<Threads> result = new ThreadManager().FilteredList(request);
            var rendered = await _viewRenderService.RenderToStringAsync("Home/_sidebarContent", result);
            return Json(rendered);
        }
    }
}