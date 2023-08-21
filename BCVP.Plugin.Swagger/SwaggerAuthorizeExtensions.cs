using Microsoft.AspNetCore.Builder;

namespace BCVP.Plugin.Swagger
{
    // 定义扩展
    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseSwaggerAuth>();
        }
    }
}
