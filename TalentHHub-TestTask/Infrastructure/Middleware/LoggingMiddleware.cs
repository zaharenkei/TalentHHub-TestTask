using System.Diagnostics;

namespace TalentHHub_TestTask.Infrastructure.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly Stopwatch stopwatch;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory.CreateLogger<LoggingMiddleware>();
            stopwatch = new Stopwatch();
        }

        public async Task Invoke(HttpContext context)
        {
            
            logger.LogInformation("Started {method} {url} request executing.",
                context.Request?.Method, context.Request?.Path.Value);

            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();

            logger.LogInformation("Request {method} {url} executed. Status: {status}; ElapsedTime: {elapsed}",
                context.Request?.Method, context.Request?.Path.Value, context.Response?.StatusCode, stopwatch.Elapsed);
        }
    }
}
