using Model;
using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CustomTranslation = Model.Meta.Enums.Translation;

namespace Web.Utils
{
    public static class HtmlExtensions
    {
        private const string DeleteAction = "Delete";
        private const string EditAction = "Edit";

        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            StringBuilder builder = new StringBuilder();
            AppendCursorButton(builder, "first", currentPage == 1, pageUrl, 1);
            AppendCursorButton(builder, "prev", currentPage == 1, pageUrl, currentPage - 1);

            for (int i = 1; i <= totalPages; i++)
            {
                //Условие что выводим только необходимые номера
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                    if (i == currentPage)
                    {
                        subBuilder.MergeAttribute("href", "#");
                        builder.AppendLine("<li class=\"active\">" + subBuilder.ToString() + "</li>");
                    }
                    else
                    {
                        subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                        builder.AppendLine("<li>" + subBuilder.ToString() + "</li>");
                    }
                }
                else if (((i == 4) && (currentPage > 5)) || ((i == (totalPages - 3)) && (currentPage < (totalPages - 4))))
                {
                    builder.AppendLine("<li class=\"disabled\"> <a href=\"#\">...</a> </li>");
                }
            }

            AppendCursorButton(builder, "next", currentPage == totalPages, pageUrl, currentPage + 1);
            AppendCursorButton(builder, "last", currentPage == totalPages, pageUrl, totalPages);
            return new MvcHtmlString("<ul>" + builder.ToString() + "</ul>");
        }

        public static MvcHtmlString DeleteButton(this HtmlHelper html, string controller, object id)
        {
            return html.ActionLink(" ", DeleteAction, controller, new { id = id }, new { @class = "icon-remove", title = "Удалить", data_confirm = "Действительно удалить?" });
        }

        public static MvcHtmlString EditButton(this HtmlHelper html, string controller, object id)
        {
            return html.ActionLink(" ", EditAction, controller, new { id = id }, new { @class = "icon-pencil", title = "Редактировать" });
        }

        public static MvcHtmlString UserRoleSwitch(this HtmlHelper html, UserRole role, Func<MvcHtmlString> expression)
        {
            return new MvcHtmlString(string.Format("<label class=\"input-control switch\">{0}<span class=\"helper\">{1}</span></label>",
                                                expression(),
                                                CustomTranslation.UserRole.ResourceManager.GetString(role.ToString())));
        }

        public static MvcHtmlString Comment(this HtmlHelper html, string text)
        {
            if (text == null)
            {
                return MvcHtmlString.Empty;
            }
            text = text.Trim().Replace("\"", "&quot;");
            if (string.IsNullOrWhiteSpace(text))
            {
                return MvcHtmlString.Empty;
            }
            return new MvcHtmlString(string.Format("<a class=\"icon-comments-5\" data-comment=\"{0}\" href=\"#\" title=\"Комментарий\"></a>", text));
        }

        public static MvcHtmlString Date(this HtmlHelper html, DateTime? date)
        {
            if (!date.HasValue)
            {
                return MvcHtmlString.Empty;
            }

            return new MvcHtmlString(string.Format("{0:HH:mm:ss dd.MM.yyyy}", date));
        }

        public static MvcHtmlString Deadline(this HtmlHelper html, DateTime? date)
        {
            if (!date.HasValue)
            {
                return MvcHtmlString.Empty;
            }

            string color = "green";

            DateTime now = DateTime.Now.Date;
            if (date.Value.Date == now)
            {
                color = "yellow";
            }
            else if (date.Value.Date < now)
            {
                color = "red";
            }

            return new MvcHtmlString(string.Format("<span class=\"fg-color-{0}\">{1}</span>", color, string.Format("{0:dd.MM.yyyy}", date)));
        }

        public static MvcHtmlString DateEditor(this HtmlHelper html, string name, DateTime? date)
        {
            return new MvcHtmlString(string.Format("<input id=\"{0}\" name=\"{0}\" type=\"text\" value=\"{1}\">",
                                        name, date.HasValue ? string.Format("{0:yyyy-MM-dd}", date) : string.Empty));
        }

        public static MvcHtmlString Priority(this HtmlHelper html, TaskPriority priority)
        {
            string color;
            switch (priority)
            {
                case TaskPriority.High:
                    color = "red";
                    break;

                case TaskPriority.Middle:
                    color = "yellow";
                    break;

                default:
                    color = "green";
                    break;
            }
            return new MvcHtmlString(string.Format("<span class=\"fg-color-{0}\">{1}</span>", color, CustomTranslation.TaskPriority.ResourceManager.GetString(priority.ToString())));
        }

        public static MvcHtmlString Status(this HtmlHelper html, CallStatus status)
        {
            string color;
            switch (status)
            {
                case CallStatus.Completed:
                    color = "green";
                    break;

                case CallStatus.Returned:
                    color = "red";
                    break;

                case CallStatus.Checked:
                    color = "greenDark";
                    break;

                default:
                    color = "darken";
                    break;
            }
            return new MvcHtmlString(string.Format("<span class=\"fg-color-{0}\">{1}</span>", color, CustomTranslation.CallStatus.ResourceManager.GetString(status.ToString())));
        }

        public static MvcHtmlString Status(this HtmlHelper html, TaskStatus status)
        {
            string color;
            switch (status)
            {
                case TaskStatus.Returned:
                    color = "red";
                    break;

                case TaskStatus.Completed:
                    color = "green";
                    break;

                case TaskStatus.Executing:
                    color = "blue";
                    break;

                case TaskStatus.Checked:
                    color = "greenDark";
                    break;

                default:
                    color = "darken";
                    break;
            }

            return new MvcHtmlString(string.Format("<span class=\"fg-color-{0}\">{1}</span>", color, CustomTranslation.TaskStatus.ResourceManager.GetString(status.ToString())));
        }

        private static void AppendCursorButton(StringBuilder builder, string liClass, bool isActive, Func<int, string> pageUrl, int pageUrlParam)
        {
            TagBuilder aBuilder = new TagBuilder("a");
            aBuilder.MergeAttribute("href", isActive ? "#" : pageUrl.Invoke(pageUrlParam));
            builder.AppendLine(string.Format("<li class=\"{0} {1}\">{2}</li>", isActive ? "active" : "", liClass, aBuilder.ToString()));
        }
    }
}