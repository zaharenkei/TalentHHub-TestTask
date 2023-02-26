using System.Net;
using Newtonsoft.Json;

namespace TalentHHub_TestTask.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            var content = JsonConvert.SerializeObject(new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "The unexpected exception occurred.",
                    Details = exception.Message
                });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(content);
        }
    }
}
