using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Runtime.InteropServices;
using static Blog.Core.Extensions.Swagger.CustomApiVersion;

namespace Blog.Core.Extensions.Swagger
{
    /// <summary>
    /// Swagger 启动服务
    /// </summary>
    public static class SwaggerSetup
    {

        //private static readonly ILog log = LogManager.GetLogger(typeof(SwaggerSetup));

        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var basePath = AppContext.BaseDirectory;
            //var basePath2 = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            var ApiName = AppSettings.app(new string[] { "Startup", "ApiName" });

            services.AddSwaggerGen(c =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} 接口文档 - {RuntimeInformation.FrameworkDescription}",
                        Description = $"{ApiName} HTTP API " + version,
                        Contact = new OpenApiContact { Name = ApiName, Email = "fangding@aliyun.com", Url = new Uri("http://www.Blog.Core-system.com/public/index.php") },
                        License = new OpenApiLicense { Name = ApiName + " 官方文档", Url = new Uri("http://www.Blog.Core-system.com/public/index.php") }
                    });
                    c.OrderActionsBy(o => o.RelativePath);
                });


                try
                {
                    //就是这里
                    var xmlPath = Path.Combine(basePath, "Blog.Core.Api.xml");//这个就是刚刚配置的xml文件名
                    c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                    var xmlModelPath = Path.Combine(basePath, "Blog.Core.Model.xml");//这个就是Model层的xml文件名
                    c.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    //log.Error("Blog.Core.Api.xml和Blog.Core.Model.xml 丢失，请检查并拷贝。\n" + ex.Message);
                }

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();


                // ids4和jwt切换
                if (Permissions.IsUseIds4)
                {
                    //接入identityserver4
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{AppSettings.app(new string[] { "Startup", "IdentityServer4", "AuthorizationUrl" })}/connect/authorize"),
                                Scopes = new Dictionary<string, string> {
                                {
                                    "Blog.Core.api","ApiResource id"
                                }
                            }
                            }
                        }
                    });
                }
                else
                {
                    //var schemeName = "Bearer";
                    // Jwt Bearer 认证，必须是 oauth2
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        ////Jwt Bearer ApiKey 认证
                        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",//jwt默认的参数名称
                        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                        Type = SecuritySchemeType.ApiKey

                        ////Jwt Bearer http 认证
                        //Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        //Name = "Authorization",//jwt默认的参数名称
                        //In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                        //Type = SecuritySchemeType.Http,
                        //BearerFormat = "JWT",
                        //Scheme = schemeName.ToLowerInvariant()
                    });

                    //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    //{
                    //    //Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    //    //Name = "Authorization",//jwt默认的参数名称
                    //    //In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    //    //Type = SecuritySchemeType.ApiKey
                    //    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    //    Name = "Authorization",//jwt默认的参数名称
                    //    //Type=SecuritySchemeType.Http,
                    //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    //    Type = SecuritySchemeType.Http,
                    //    BearerFormat = "JWT",
                    //    Scheme = schemeName.ToLowerInvariant()

                    //});


                    //会导致锁状态问题，不用授权的接口多增加了一个未解锁状态
                    //c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    //    {
                    //        new OpenApiSecurityScheme
                    //        {
                    //            Reference = new OpenApiReference
                    //            {
                    //                Type = ReferenceType.SecurityScheme,
                    //                Id = schemeName
                    //            }
                    //        },
                    //        new string[0]
                    //    }
                    //});
                }
            });
        }
    }
}
