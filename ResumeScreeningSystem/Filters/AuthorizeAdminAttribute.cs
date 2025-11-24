using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResumeScreeningSystem.Filters
{
    public class AuthorizeAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(isAuthenticated) || isAuthenticated != "true")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else if (userRole != "Admin")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }

            base.OnActionExecuting(context);
        }
    }
}