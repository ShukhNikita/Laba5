using CourseWorkCompany.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWorkCompany.Middleware
{
    public class DBMiddleware
    {
            private readonly RequestDelegate _next;
            public DBMiddleware(RequestDelegate next)
            {
                // инициализация базы данных 
                _next = next;
            }
            public Task Invoke(HttpContext context, Context context1)
            {
                if (!(context.Session.Keys.Contains("starting")))
                {
                    DbInitializer.Initialize(context1);
                    ApplicationInitializer.Initialize(context).Wait();
                    context.Session.SetString("starting", "Yes");
                }
                return _next.Invoke(context);
            }
    }
}
