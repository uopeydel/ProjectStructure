using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Pjs1.Main.Routing
{
    public class UrlRedirectRule : IRule
    {
        public UrlRedirectRule(/* Inject any rule for process */)
        {

        }

        public void ApplyRule(RewriteContext context)
        {
            var path = context.HttpContext.Request.Path.ToString().ToLowerInvariant();
            var urlInformation = UrlInformation.Get(path);
            if (CheckRuleForRedirectPath())
            {
                //$"/new/url/here/";
                var newUrl = $"/home";
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Redirect;
                response.Headers[HeaderNames.Location] = newUrl;
                context.Result = RuleResult.EndResponse; // Do not continue processing the request    
            }
        }

        private bool CheckRuleForRedirectPath()
        {
            var IsTrue = "1";
            return IsTrue.Equals("0");
        }

        private static class StatusCodes
        {
            public static readonly int Redirect = (int)HttpStatusCode.MovedPermanently;
            public static readonly int MethodNotAllowed = (int)HttpStatusCode.MethodNotAllowed;
            public static readonly int Ok = (int)HttpStatusCode.OK;
        }
    }
}
