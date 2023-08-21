using BCVP.Plugin.Common;
using Microsoft.AspNetCore.Builder;
using SqlSugar.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using static BCVP.Plugin.Common.GlobalVar.CustomApiVersion;

namespace BCVP.Plugin.Swagger
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class SwaggerMildd
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(SwaggerMildd));
        public static void UseSwaggerMildd(this IApplicationBuilder app, Func<Stream> streamHtml)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                var ApiName = AppSettings.app(new string[] { "Startup", "ApiName" });
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });

                // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：{项目名.index.html}
                if (streamHtml.Invoke() == null)
                {
                    var msg = "index.html的属性，必须设置为嵌入的资源";
                    //log.Error(msg);
                    throw new Exception(msg);
                }

                c.IndexStream = streamHtml;
                c.DocExpansion(DocExpansion.None); //->修改界面打开时自动折叠
                                                   //设置为-1 可不显示models
                                                   //c.DefaultModelsExpandDepth(-1);//不显示model

                // 路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                if (AppSettings.app("AppSettings", "HideSwagger", "Enabled").ObjToBool())
                {
                    c.RoutePrefix = AppSettings.app("AppSettings", "HideSwagger", "RoutePrefix").ToString();
                }
                else
                {
                    c.RoutePrefix = "";
                }
            });
        }
    }
}
