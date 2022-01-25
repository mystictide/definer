using definer.Entity.Helpers;
using definer.Entity.Threads;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace definer.Models
{
    public static class CustomTagHelpers
    {
        private const string t = "/t/";

        public static List<string> SidebarContent(FilteredList<Threads> result)
        {
            List<string> tags = new List<string>();
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
                tags.Add(list.ToString());
            }
            return tags;
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
    }
}
