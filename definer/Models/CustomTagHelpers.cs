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
                string encodedText = replaceUser(incomingText);
                encodedText = replaceEntry(encodedText);
                encodedText = replaceThread(encodedText);
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
        public static string replaceUser(string incomingText)
        {
            if (incomingText != null)
            {
                while (incomingText.Contains("[user "))
                {
                    string open = "[user ";
                    int start = incomingText.IndexOf(open);
                    int end = incomingText.IndexOf("]", start);
                    string result = incomingText.Substring(start + open.Length, end - (start + open.Length));

                    incomingText = incomingText.Remove(start, open.Length);
                    var anchor = new TagBuilder("a");
                    anchor.InnerHtml = "@" + result;
                    anchor.MergeAttribute("href", "/u/" + result);

                    incomingText = incomingText.Replace(result, anchor.ToString());
                }
                return incomingText;
            }
            else
            {
                return "";
            }
        }

        public static string replaceEntry(string incomingText)
        {
            if (incomingText != null)
            {
                while (incomingText.Contains("[entry "))
                {
                    string open = "[entry ";
                    int start = incomingText.IndexOf(open);
                    int end = incomingText.IndexOf("]", start);
                    string result = incomingText.Substring(start + open.Length, end - (start + open.Length));

                    incomingText = incomingText.Remove(start, open.Length);
                    var anchor = new TagBuilder("a");
                    anchor.InnerHtml = "#" + result;
                    anchor.MergeAttribute("href", "/entry/" + result);

                    incomingText = incomingText.Replace(result, anchor.ToString());
                }
                return incomingText;
            }
            else
            {
                return "";
            }
        }

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
                    anchor.InnerHtml = result + "*";
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
                    anchor.InnerHtml = "link" + "^";
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
