using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace definer.Models
{
    public static class CustomTagHelpers
    {
        private const string t = "/";

        public static string SidebarContent(FilteredList<Threads> result)
        {
            string Pager = "";
            var sidebarDiv = new TagBuilder("div");
            sidebarDiv.AddCssClass("sidebar");
            #region paging
            if (result.filter.pager.TotalPages > 1)
            {
                var page = result.filter.pager.CurrentPage == 0 ? 1 : result.filter.pager.CurrentPage;
                var div = new TagBuilder("div");
                div.AddCssClass("pagination-sidebar");
                var ul = new TagBuilder("ul");
                ul.AddCssClass("paging-list");

                var itemPre = "";
                if (result.filter.pager.CurrentPage != 1)
                {
                    var itemPrev = new TagBuilder("li");
                    itemPrev.AddCssClass("page-item direction");
                    var prevAnchor = new TagBuilder("a");
                    prevAnchor.MergeAttribute("href", "javascript:void(0)");
                    int pagetoGo = page == 1 ? 1 : (page - 1);
                    prevAnchor.MergeAttribute("onClick", "sidebar.filter(" + pagetoGo + ")");
                    prevAnchor.InnerHtml = "&laquo;";
                    itemPrev.InnerHtml = prevAnchor.ToString();
                    itemPre = itemPrev.ToString();
                }

                var pageItem = new TagBuilder("li");
                pageItem.AddCssClass("page-item active");
                var dropdown = new TagBuilder("select");
                dropdown.GenerateId("spages");
                dropdown.AddCssClass("spages");
                List<string> options = new List<string>();
                foreach (var item in result.filter.pager.Pages)
                {
                    var option = new TagBuilder("option");
                    option.AddCssClass("page-link");
                    option.MergeAttribute("value", item.ToString());
                    option.InnerHtml = item.ToString();
                    if (item == result.filter.pager.CurrentPage)
                    {
                        option.MergeAttribute("selected", "selected");
                    }
                    dropdown.InnerHtml += option.ToString();
                }
                pageItem.InnerHtml = dropdown.ToString();

                var divider = new TagBuilder("li");
                divider.AddCssClass("page-item divider");
                divider.InnerHtml = " / ";

                var endPage = new TagBuilder("li");
                endPage.AddCssClass("page-item last");
                var endAnchor = new TagBuilder("a");
                endAnchor.AddCssClass("page-link");
                endAnchor.MergeAttribute("href", "javascript:void(0)");
                endAnchor.MergeAttribute("onClick", "sidebar.filter(" + result.filter.pager.EndPage + ")");
                endAnchor.InnerHtml = result.filter.pager.EndPage.ToString();
                endPage.InnerHtml = endAnchor.ToString();

                var itemLast = "";

                if (page != result.filter.pager.EndPage)
                {
                    var lastPage = new TagBuilder("li");
                    lastPage.AddCssClass("page-item direction");
                    var lastAnchor = new TagBuilder("a");
                    lastAnchor.AddCssClass("page-link");
                    lastAnchor.MergeAttribute("href", "javascript:void(0)");
                    int totalPages = result.filter.pager.TotalPages == page ? result.filter.pager.TotalPages : (page + 1);
                    lastAnchor.MergeAttribute("onClick", "sidebar.filter(" + totalPages + ")");
                    lastAnchor.InnerHtml = "&raquo;";
                    lastPage.InnerHtml = lastAnchor.ToString();
                    itemLast = lastPage.ToString();
                }
                ul.InnerHtml = itemPre + pageItem.ToString() + divider.ToString() + endPage.ToString() + itemLast;
                div.InnerHtml = ul.ToString();
                sidebarDiv.InnerHtml = div.ToString();
                Pager = sidebarDiv.ToString();
            }
            #endregion
            #region threads
            var threadUL = new TagBuilder("ul");
            threadUL.AddCssClass("sidebar-list");
            List<string> threads = new List<string>();
            foreach (var item in result.data)
            {
                var list = new TagBuilder("li");
                var anchor = new TagBuilder("a");
                var small = new TagBuilder("small");

                string url = t + FriendlyURLTitle(item.Title) + "-" + @item.ID;

                anchor.MergeAttribute("href", url);
                small.SetInnerText(item.Entries.ToString());

                anchor.InnerHtml = item.Title + small.ToString();
                list.InnerHtml = anchor.ToString();
                threads.Add(list.ToString());
            }
            threadUL.InnerHtml = string.Join("", threads);
            sidebarDiv.InnerHtml += threadUL.ToString();
            Pager = sidebarDiv.ToString();
            #endregion
            return Pager;
        }

        public static string FriendlyURLTitle(string incomingText)
        {

            if (incomingText != null)
            {
                incomingText = incomingText.ToLower();
                incomingText = incomingText.Replace("ş", "s");
                incomingText = incomingText.Replace("ı", "i");
                incomingText = incomingText.Replace("ö", "o");
                incomingText = incomingText.Replace("ü", "u");
                incomingText = incomingText.Replace("ç", "c");
                incomingText = incomingText.Replace("Ç", "C");
                incomingText = incomingText.Replace("ğ", "g");
                incomingText = incomingText.Replace(" ", "-");
                incomingText = incomingText.Replace("---", "-");
                incomingText = incomingText.Replace("?", "");
                incomingText = incomingText.Replace("/", "");
                incomingText = incomingText.Replace(".", "");
                incomingText = incomingText.Replace("'", "");
                incomingText = incomingText.Replace("#", "");
                incomingText = incomingText.Replace("%", "");
                incomingText = incomingText.Replace("&", "");
                incomingText = incomingText.Replace("*", "");
                incomingText = incomingText.Replace("!", "");
                incomingText = incomingText.Replace("@", "");
                incomingText = incomingText.Replace("+", "-arti-");
                incomingText = incomingText.Trim();
                string encodedUrl = (incomingText ?? "").ToLower();
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
                encodedUrl = encodedUrl.Replace("'", "");
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
                encodedUrl = encodedUrl.Trim('-');
                return encodedUrl;
            }
            else
            {
                return "";
            }
        }

        public static string FormatEntry(string incomingText)
        {
            if (incomingText != null)
            {
                string encodedText = replaceThread(incomingText);
                encodedText = replaceLink(encodedText);
                encodedText = replaceSpoiler(encodedText);
                encodedText = Regex.Replace(encodedText, @"\t\n", "").Trim();
                return encodedText.Replace("]", "");
            }
            else
            {
                return "";
            }
        }

        #region formatHelpers
        public static string replaceThread(string incomingText)
        {
            if (incomingText != null)
            {
                while (incomingText.Contains("[thread "))
                {
                    string open = "[thread ";
                    int start = incomingText.IndexOf(open);
                    int end = incomingText.IndexOf("]", start);
                    string result = incomingText.Substring(start + open.Length, end - (start + open.Length));

                    incomingText = incomingText.Remove(start, open.Length);
                    var anchor = new TagBuilder("a");
                    anchor.InnerHtml = result;
                    anchor.MergeAttribute("href", "/s/" + result);

                    incomingText = incomingText.Replace(result, anchor.ToString());
                }
                return incomingText;
            }
            else
            {
                return "";
            }
        }

        public static string replaceLink(string incomingText)
        {
            if (incomingText != null)
            {
                while (incomingText.Contains("[link "))
                {
                    string open = "[link ";
                    int start = incomingText.IndexOf(open);
                    int end = incomingText.IndexOf("]", start);
                    string result = incomingText.Substring(start + open.Length, end - (start + open.Length));

                    incomingText = incomingText.Remove(start, open.Length);
                    var anchor = new TagBuilder("a");
                    anchor.InnerHtml = "link";
                    anchor.MergeAttribute("href", result);

                    incomingText = incomingText.Replace(result, anchor.ToString());
                }
                return incomingText;
            }
            else
            {
                return "";
            }
        }

        public static string replaceSpoiler(string incomingText)
        {
            if (incomingText != null)
            {
                while (incomingText.Contains("[spoiler "))
                {
                    string open = "[spoiler ";
                    int start = incomingText.IndexOf(open);
                    int end = incomingText.IndexOf("]", start);
                    string result = incomingText.Substring(start + open.Length, end - (start + open.Length));

                    incomingText = incomingText.Remove(start, open.Length);
                    //var div = new TagBuilder("div");
                    //div.AddCssClass("spoiler-overlay");
                    //div.InnerHtml = "spoilers";
                    var span = new TagBuilder("span");
                    span.AddCssClass("spoiler");
                    span.Attributes.Add("tabindex", "0");
                    string inner = result;
                    span.InnerHtml = inner;
                    string elements = span.ToString();

                    incomingText = incomingText.Replace(result, elements);
                }
                return incomingText;
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
