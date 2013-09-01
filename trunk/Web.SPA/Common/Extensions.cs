using System.Security.Principal;
using Web.Common.Auth;

namespace Web.SPA.Common
{
    public static class Extensions
    {
        public static bool IsAdmin(this IPrincipal obj)
        {
            return obj.Identity is UserIndentity ? (obj.Identity as UserIndentity).User.IsAdmin : false;
        }

        public static bool IsCustomer(this IPrincipal obj)
        {
            return obj.Identity is UserIndentity ? (obj.Identity as UserIndentity).User.IsCustomer : false;
        }

        public static bool IsExecutor(this IPrincipal obj)
        {
            return obj.Identity is UserIndentity ? (obj.Identity as UserIndentity).User.IsExecutor : false;
        }

        public static bool IsMaster(this IPrincipal obj)
        {
            return obj.Identity is UserIndentity ? (obj.Identity as UserIndentity).User.IsMaster : false;
        }

        public static bool IsRouter(this IPrincipal obj)
        {
            return obj.Identity is UserIndentity ? (obj.Identity as UserIndentity).User.IsRouter : false;
        }
    }
}