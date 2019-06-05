using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Infrastructure
{
    public static class AddedTimeExtensions
    {
        public static string AlertTime(this HtmlHelper htmlHelper, DateTime date)
        {
            var dateDiff = new DateTime((DateTime.Now - date).Ticks);

            if (dateDiff.Year > 1)
                return string.Format("{0} at {1}", date.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en")), date.ToString("H:mm", CultureInfo.CreateSpecificCulture("en")));
            else if (dateDiff.Day == 2)
                return "Yesterday " + dateDiff.ToString("H:mm", CultureInfo.CreateSpecificCulture("en"));
            else if (dateDiff.Day > 1)
                return string.Format("{0} at {1}", date.ToString("dd MMMM ", CultureInfo.CreateSpecificCulture("en")), date.ToString("H:mm", CultureInfo.CreateSpecificCulture("en")));
            else if (dateDiff.Hour > 0)
                return dateDiff.Hour + " hour";
            else if (dateDiff.Minute > 0)
                return dateDiff.Minute + " min";
            else
                return dateDiff.Second + " sec.";
        }

        public static string CommentTime(this HtmlHelper htmlHelper, DateTime date)
        {
            var dateDiff = new DateTime((DateTime.Now - date).Ticks);

            if (dateDiff.Year > 1)
                return string.Format("{0}", date.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en")));
            else if (dateDiff.Day == 2)
                return "Yesterday";
            else if (dateDiff.Day <= 8 && dateDiff.Day != 1)
                return date.ToString("dddd", CultureInfo.CreateSpecificCulture("en"));
            else if (dateDiff.Day > 8)
                return date.ToString("dd MMMM", CultureInfo.CreateSpecificCulture("en"));
            else
                return date.ToString("H:mm", CultureInfo.CreateSpecificCulture("en"));
        }
    }
}