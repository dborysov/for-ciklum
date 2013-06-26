#region Usings

using System.Web.Mvc;

#endregion

namespace LogSys.Filters
{
    public class UserNameFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string Key = "userName";

            if (filterContext.ActionParameters.ContainsKey(Key) && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.ActionParameters[Key] = filterContext.HttpContext.User.Identity.Name;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}