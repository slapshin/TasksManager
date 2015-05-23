using Model;
using System;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web.Mvc;
using UserRoleTranslation = Model.Meta.Enums.Translation.UserRole;

namespace Web.Utils
{
    public static class Extensions
    {
        public static string RolesToString(this User user)
        {
            StringBuilder builder = new StringBuilder();
            string delim = string.Empty;
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (user.Roles.HasFlag(role))
                {
                    builder.Append(delim + UserRoleTranslation.ResourceManager.GetString(role.ToString()));
                    delim = ", ";
                }
            }

            return builder.ToString();
        }

        public static string ExecutorName(this Task task)
        {
            return task.Executor == null ? string.Empty : task.Executor.Login;
        }

        public static SelectList EnumToSelectList<T>(this T obj, ResourceManager localization = null)
        {
            var values = (from T e in Enum.GetValues(typeof(T))
                          select new ObjectSelectListItem
                          {
                              Value = e,
                              Text = localization == null ? e.ToString() : localization.GetString(e.ToString())
                          }).OrderBy(i => i.Value);
            return new SelectList(values, "Value", "Text", obj);
        }

        public static string Presentation(this Call obj)
        {
            return string.Format("Заявка №{0}", obj.Id);
        }
    }
}