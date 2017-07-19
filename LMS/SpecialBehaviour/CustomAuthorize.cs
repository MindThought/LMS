using System.Web.Mvc;

namespace LMS.SpecialBehaviour
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AuthorizeAttribute), true))
            {
                //skipe authorization check if action has Authorize attribute
                return;
            }
            base.OnAuthorization(filterContext);
        }
    }
}