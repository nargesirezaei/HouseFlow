using HouseFlowPart1.Interfaces;

namespace HouseFlowPart1.Middleware
{
    public class LogProfilerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _Configuration;


        public LogProfilerMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _Configuration = configuration;
        }
        // Invoke the middleware and write log if enabled
        public async Task InvokeAsync(HttpContext httpContext, ILogService logger)
        {
            WriteLog(logger, httpContext);
            await _next(httpContext);
        }
        // Write a log message if the "LogProfiler" configuration is set to "True"
        private void WriteLog(ILogService logger, HttpContext httpContext)
        {
            var LogProfiler = _Configuration["LogProfiler"];
            if (LogProfiler == "True")
            {
                // Extract request details from HttpContext
                var request = httpContext.Request;
                var method = request.Method;
                var path = request.Path;
                var queryString = request.QueryString.ToString();
                var userAgent = request.Headers["User-Agent"].ToString();

                // Construct the log message with request details
                var logMessage = $"Request: {method} {path}{queryString} - User-Agent: {userAgent}";

                logger.WriteVisitAsync(logMessage);
            }
        }
    }
}
