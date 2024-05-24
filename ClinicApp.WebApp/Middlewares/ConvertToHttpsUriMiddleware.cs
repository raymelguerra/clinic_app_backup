
namespace ClinicApp.WebApp.Middlewares
{
    public class ConvertToHttpsUriMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.IsHttps)
            {
                var originalPath = context.Request.Path;
                context.Request.Scheme = "https";
                context.Request.PathBase = "/";
                context.Request.Path = originalPath;
            }
            await next(context);
        }
    }
}
