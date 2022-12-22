

using CourseWorkCompany.Data;

namespace CourseWorkCompany.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public Task Invoke(HttpContext context, IServiceProvider serviceProvider, Context dbContext)
        {

            DbInitializer.Initialize(dbContext);

            return _next.Invoke(context);
        }
    }
}
