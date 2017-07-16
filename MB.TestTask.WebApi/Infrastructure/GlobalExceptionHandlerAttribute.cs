using MB.TestTask.Authentication;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MB.TestTask.WebAPI.Infrastructure
{
    public class GlobalExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public GlobalExceptionHandlerAttribute()
        {
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            RemapExceptionToHttpStatusCode(context);
        }

        private void RemapExceptionToHttpStatusCode(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception == null || exception is HttpResponseException)
            {
                return;
            }

            var statusCode = HttpStatusCode.InternalServerError;
            if (exception is NotImplementedException)
            {
                statusCode = HttpStatusCode.NotImplemented;
            }
            else if (exception is AuthenticationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (exception is AuthorizationException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }
            context.Response = new HttpResponseMessage(statusCode);
        }
    }
}