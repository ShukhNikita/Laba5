using CourseWorkCompany.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CourseWorkCompany.Web.Middleware
{
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DBMiddleware>();
        }

    }
}
