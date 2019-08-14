using Microsoft.AspNetCore.Builder;

namespace Exercise_4
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseETagger(this IApplicationBuilder app)
        {
            app.UseMiddleware<ETagMiddleware>();
        }
    }
}