using BCVP.Plugin.Common;
using Microsoft.AspNetCore.Http;
using SqlSugar.Extensions;
using System.Net;
using System.Text;

namespace BCVP.Plugin.Swagger
{
    /// <summary>
    /// 实现类：UseSwaggerAuth
    /// </summary>	
    public class UseSwaggerAuth
    {
        #region 私有变量
        private readonly RequestDelegate requestDelegate;
        #endregion

        /// <summary>
        /// 构造函数，Autoface依赖注入
        /// </summary>
        public UseSwaggerAuth(RequestDelegate _requestDelegate)
        {
            this.requestDelegate = _requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettings.app(new string[] { "Middleware", "IsLocalRequestSwgger", "Enabled" }).ObjToBool())
            {
                if (IsLocalRequest(context))
                {
                    await requestDelegate.Invoke(context);
                }
                else
                {
                    //此段代码提取方法报错，应该是context转发被修改发起request导致,姑且重复
                    if (context.Request.Path.Value.ToLower().Contains("index.html"))
                    {
                        // 判断权限是否正确
                        if (IsAuthorized(context))
                        {
                            await requestDelegate.Invoke(context);
                            return;
                        }

                        // 无权限，跳转swagger登录页
                        context.Response.Redirect("/swg-login.html");
                    }
                    else
                    {
                        await requestDelegate.Invoke(context);
                    }
                }
            }
            else
            {
                if (AppSettings.app("AppSettings", "HideSwagger", "Enabled").ObjToBool())
                {
                    if (context.Request.Path.Value.ToLower().Contains(AppSettings.app("AppSettings", "HideSwagger", "RoutePrefix").ToString()))
                    {
                        // 判断权限是否正确
                        if (IsAuthorized(context))
                        {
                            await requestDelegate.Invoke(context);
                            return;
                        }

                        // 无权限，跳转swagger登录页
                        context.Response.Redirect("/swg-login.html");
                    }
                    else
                    {
                        await requestDelegate.Invoke(context);
                    }
                }
                else
                {
                    if (context.Request.Path.Value.ToLower().Contains("index.html"))
                    {
                        // 判断权限是否正确
                        if (IsAuthorized(context))
                        {
                            await requestDelegate.Invoke(context);
                            return;
                        }

                        // 无权限，跳转swagger登录页
                        context.Response.Redirect("/swg-login.html");
                    }
                    else
                    {
                        await requestDelegate.Invoke(context);
                    }
                }
            }

        }

        public bool IsAuthorized(HttpContext context)
        {
            // 使用session模式 可以使用其他的
            return context.Session.GetString("swagger-code") == "success";
        }

        /// <summary>
        /// 判断是不是本地访问
        /// 本地不用swagger拦截
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsLocalRequest(HttpContext context)
        {
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
            {
                return true;
            }
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }
            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            {
                return true;
            }
            return false;
        }
    }
}
